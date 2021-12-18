using Ardoise.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardoise
{
    public class ardoise
    {
        private List<int> a_faire = new List<int>(); //si la personne doit rembourser, c'est -1. Si la personne doit se faire rembourser, c'est 1. Et si la personne a deja pile le total, c'est 0

        public Soiree_DAL entree_ardoise()
        {
            Console.WriteLine("Entrez le lieu de la soiree");
            string lieuSoiree = Console.ReadLine();
            Console.WriteLine("Entrez la date de la soiree au format mm/jj/aaaa");
            DateTime? dateSoiree = DateTime.ParseExact(Console.ReadLine(), "MM/dd/yyyy", null);
            int finit = 0;
            string prenom;
            string nom;
            double argent;
            var participants = new List<Participant_DAL>();
            while (finit != 1)
            {
                Console.WriteLine("Entrez le prenom du participant");
                prenom = Console.ReadLine();
                Console.WriteLine("Entrez le nom du participant");
                nom = Console.ReadLine();
                Console.WriteLine("Entrez l'argent avance par le participant");
                argent = double.Parse(Console.ReadLine());
                Console.WriteLine("Si vous avez entre tous les participants, entrez 1. Sinon, entrez 0 : ");
                finit = int.Parse(Console.ReadLine());
                var participant = new Participant_DAL(argent, nom, prenom);
                participants.Add(participant);
            }
            var depotSoiree = new SoireeDepot_DAL();
            var soiree = new Soiree_DAL(lieuSoiree, dateSoiree, participants);
            depotSoiree.Insert(soiree);

            return soiree;
        }

        public void reset()
        {
            a_faire.Clear();
        }
        public void gestion_ardoise(Soiree_DAL soiree)
        {
            var depotParticipants = new ParticipantDepot_DAL();
            var participants = depotParticipants.GetAllByIDSoiree(soiree.ID);
            //faire moyenne total argent
            double total_argent = 0;
            for (int i = 0; i < participants.Count(); i++)
            {
                total_argent += participants[i].Montant;
            }
            double moyenne = total_argent / participants.Count;

            //savoir combient doit donner/recevoir chaque personne
            for (int i = 0; i < participants.Count(); i++)
            {
                participants[i].Montant -= moyenne;
                if (participants[i].Montant == 0)
                {
                    a_faire.Add(0);
                }
                else if (participants[i].Montant > 0)
                {
                    a_faire.Add(1);
                }
                else if (participants[i].Montant < 0)
                {
                    a_faire.Add(-1);
                }
            }

            //assigner les remboursements entre les personnes
            var fin = 0;
            while (fin == 0)
            {
                fin = 1;
                for (int i = 0; i < participants.Count(); i++)
                {
                    for (int j = i+1; j < participants.Count(); j++)
                    {
                        var total1 = participants[i].Montant;
                        var total2 = participants[j].Montant;
                        var a_faire1 = this.a_faire[i];
                        var a_faire2 = this.a_faire[j];

                        if (a_faire1 != 0 && a_faire2 != 0 && a_faire1+a_faire2 == 0) 
                        {
                            fin = 0;
                            if (a_faire1 == 1)
                            {
                                if (total1+total2 >=0)
                                {
                                    Console.WriteLine(participants[j].Prenom + " " + participants[j].Nom + " doit " + (-total2) + " a " + participants[i].Prenom + " " + participants[i].Nom);
                                    participants[i].Montant += total2;
                                    participants[j].Montant = 0;
                                    this.a_faire[j] = 0;
                                    if (participants[i].Montant == 0)
                                    {
                                        this.a_faire[i] = 0;
                                    }
                                }
                                else if (total1 + total2 <= 0)
                                {
                                    Console.WriteLine(participants[j].Prenom + " " + participants[j].Nom + " doit " + total1 + " a " + participants[i].Prenom + " " + participants[i].Nom);
                                    participants[i].Montant = 0;
                                    participants[j].Montant += total1;
                                    this.a_faire[i] = 0;
                                    if (participants[j].Montant == 0)
                                    {
                                        this.a_faire[j] = 0;
                                    }
                                }
                            }
                            else if (a_faire1 == -1)
                            {
                                if (total1 + total2 <= 0)
                                {
                                    Console.WriteLine(participants[i].Prenom + " " + participants[i].Nom + " doit " + total2 + " a " + participants[j].Prenom + " " + participants[j].Nom);
                                    participants[i].Montant += total2;
                                    participants[j].Montant = 0;
                                    this.a_faire[j] = 0;
                                    if (participants[i].Montant == 0)
                                    {
                                        this.a_faire[i] = 0;
                                    }
                                }
                                else if (total1 + total2 >= 0)
                                {
                                    Console.WriteLine(participants[i].Prenom + " " + participants[i].Nom + " doit " + (-total1) + " a " + participants[j].Prenom + " " + participants[j].Nom);
                                    participants[i].Montant = 0;
                                    participants[j].Montant += total1;
                                    participants[i].Montant = 0;
                                    if (participants[j].Montant == 0)
                                    {
                                        this.a_faire[j] = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}