using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardoise.DAL
{
    public class SoireeDepot_DAL : Depot_DAL<Soiree_DAL>
    {
        public override List<Soiree_DAL> GetAll()
        {
            CreerConnexionEtCommande();

            commande.CommandText = "select ID, lieu, date from Soiree";
            var reader = commande.ExecuteReader();

            var depotParticipants = new ParticipantDepot_DAL();

            var listeSoirees = new List<Soiree_DAL>();

            while (reader.Read())
            {
                var participants = depotParticipants.GetAllByIDSoiree(reader.GetInt32(0));

                var soiree = new Soiree_DAL(reader.GetInt32(0),
                                        reader.GetString(1),
                                        reader.GetDateTime(2),
                                        participants);

                listeSoirees.Add(soiree);
            }

            DetruireConnexionEtCommande();

            return listeSoirees;
        }

        public override Soiree_DAL GetByID(int ID)
        {
            CreerConnexionEtCommande();

            commande.CommandText = "select ID, lieu, date from Soiree where ID=@ID";
            commande.Parameters.Add(new SqlParameter("@ID", ID));
            var reader = commande.ExecuteReader();

            var depotParticipants = new ParticipantDepot_DAL();

            Soiree_DAL soiree;

            if (reader.Read())
            {
                var participants = depotParticipants.GetAllByIDSoiree(reader.GetInt32(0));

                soiree = new Soiree_DAL(reader.GetInt32(0),
                                        reader.GetString(1),
                                        reader.GetDateTime(2),
                                        participants);
            }
            else
            {
                throw new Exception($"Pas de Soiree avec l'ID {ID}");
            }

            DetruireConnexionEtCommande();

            return soiree;
        }

        public override Soiree_DAL Insert(Soiree_DAL soiree)
        {
            CreerConnexionEtCommande();

            commande.CommandText = "insert into Soiree(lieu, date)"
                                    + " values (@lieu, @date); select scope_identity()";
            commande.Parameters.Add(new SqlParameter("@lieu", soiree.Lieu));
            commande.Parameters.Add(new SqlParameter("@date", soiree.Date));
            var ID = Convert.ToInt32((decimal)commande.ExecuteScalar());
            soiree.ID = ID;

            DetruireConnexionEtCommande();

            var depotParticipant = new ParticipantDepot_DAL();
            foreach (var participant in soiree.Participants)
            {
                participant.IDSoiree = ID;
                depotParticipant.Insert(participant);
            }

            return soiree;
        }


        public override Soiree_DAL Update(Soiree_DAL soiree)
        {
            CreerConnexionEtCommande();

            commande.CommandText = "update Soiree set lieu=@lieu, date=@date where ID=@ID";
            commande.Parameters.Add(new SqlParameter("@ID", soiree.ID));
            commande.Parameters.Add(new SqlParameter("@lieu", soiree.Lieu));
            commande.Parameters.Add(new SqlParameter("@date", soiree.Date));

            var nbLignes = (int)commande.ExecuteNonQuery();

            if (nbLignes != 1)
            {
                throw new Exception($"Impossible de mettre à jour le Soiree d'ID {soiree.ID}");
            }

            soiree.Date = GetByID(soiree.ID).Date;

            DetruireConnexionEtCommande();

            var depotParticipant = new ParticipantDepot_DAL();
            foreach (var item in soiree.Participants)
            {
                depotParticipant.Update(item);
            }

            return soiree;
        }

        public override void Delete(Soiree_DAL soiree)
        {
            var depotParticipant = new ParticipantDepot_DAL();
            var participants = depotParticipant.GetAllByIDSoiree(soiree.ID);
            for (int i = 0; i < participants.Count; i++)
            {
                depotParticipant.Delete(participants[i]);
            }

            CreerConnexionEtCommande();

            commande.CommandText = "delete from Soiree where ID=@ID";
            commande.Parameters.Add(new SqlParameter("@ID", soiree.ID));

            var nbLignes = (int)commande.ExecuteNonQuery();

            if (nbLignes != 1)
            {
                throw new Exception($"Impossible de supprimer le Soiree d'ID {soiree.ID}");
            }

            DetruireConnexionEtCommande();
        }


    }
}
