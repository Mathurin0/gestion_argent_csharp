using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardoise
{
    class ardoise
    {
        private List<string> nom = new List<string>();
        private List<double> argent = new List<double>();
        private List<int> a_faire = new List<int>(); //si la personne doit rembourser, c'est -1. Si la personne doit se faire rembourser, c'est 1. Et si la personne a deja pile le total, c'est 0

        public void entree_ardoise()
        {
            Console.WriteLine("Entrez le lieu de la soiree");
            string lieuSoiree = Console.ReadLine();
            Console.WriteLine("Entrez la date de la soiree au format mm/jj/aaaa");
            DateTime? dateSoiree = DateTime.Parse(Console.ReadLine());
            var depotSoiree = new Soire
            int finit = 0;
            while (finit != 1)
            {
                Console.WriteLine("Entrez le nom de la personne");
                nom.Add(Console.ReadLine());
                Console.WriteLine("Entrez l'argent avance par la personne");
                argent.Add(double.Parse(Console.ReadLine()));
                Console.WriteLine("Si vous avez entre toutes les personnes, entrez 1. Sinon, entrez 0 : ");
                finit = int.Parse(Console.ReadLine());
            }
        }

        public void reset()
        {
            nom.Clear();
            argent.Clear();
            a_faire.Clear();
        }
        public void gestion_ardoise()
        {
            //faire moyenne total argent
            double total_argent = 0;
            for (int i = 0; i < this.nom.Count(); i++)
            {
                total_argent += argent[i];
            }
            var moyenne = total_argent / this.nom.Count;

            //savoir combient doit donner/recevoir chaque personne
            for (int i = 0; i < this.nom.Count(); i++)
            {
                this.argent[i] -= moyenne;
                if (this.argent[i] == 0)
                {
                    a_faire.Add(0);
                }
                else if (this.argent[i] > 0)
                {
                    a_faire.Add(1);
                }
                else if (this.argent[i] < 0)
                {
                    a_faire.Add(-1);
                }
            }

            //assigner les remboursements entre les personnes
            var fin = 0;
            while (fin == 0)
            {
                fin = 1;
                for (int i = 0; i < this.nom.Count(); i++)
                {
                    for (int j = i+1; j < this.nom.Count(); j++)
                    {
                        var total1 = this.argent[i];
                        var total2 = this.argent[j];
                        var a_faire1 = this.a_faire[i];
                        var a_faire2 = this.a_faire[j];

                        if (a_faire1 != 0 && a_faire2 != 0 && a_faire1+a_faire2 == 0) 
                        {
                            fin = 0;
                            if (a_faire1 == 1)
                            {
                                if (total1+total2 >=0)
                                {
                                    Console.WriteLine(this.nom[j] + " doit " + (-total2) + " a " + this.nom[i]);
                                    this.argent[i] += total2;
                                    this.argent[j] = 0;
                                    this.a_faire[j] = 0;
                                    if (this.argent[i] == 0)
                                    {
                                        this.a_faire[i] = 0;
                                    }
                                }
                                else if (total1 + total2 <= 0)
                                {
                                    Console.WriteLine(this.nom[j] + " doit " + total1 + " a " + this.nom[i]);
                                    this.argent[i] = 0;
                                    this.argent[j] += total1;
                                    this.a_faire[i] = 0;
                                    if (this.argent[j] == 0)
                                    {
                                        this.a_faire[j] = 0;
                                    }
                                }
                            }
                            else if (a_faire1 == -1)
                            {
                                if (total1 + total2 <= 0)
                                {
                                    Console.WriteLine(this.nom[j] + " doit " + total2 + " a " + this.nom[i]);
                                    this.argent[i] += total2;
                                    this.argent[j] = 0;
                                    this.a_faire[j] = 0;
                                    if (this.argent[i] == 0)
                                    {
                                        this.a_faire[i] = 0;
                                    }
                                }
                                else if (total1 + total2 >= 0)
                                {
                                    Console.WriteLine(this.nom[j] + " doit " + (-total1) + " a " + this.nom[i]);
                                    this.argent[i] = 0;
                                    this.argent[j] += total1;
                                    this.a_faire[i] = 0;
                                    if (this.argent[j] == 0)
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