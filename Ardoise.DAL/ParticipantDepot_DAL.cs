using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardoise.DAL
{
    public class ParticipantDepot_DAL : Depot_DAL<Participant_DAL>
    {
        public override List<Participant_DAL> GetAll()
        {
            CreerConnexionEtCommande();

            commande.CommandText = "select ID, montant, nom, prenom, idSoiree from Participant";
            var reader = commande.ExecuteReader();

            var listeDeParticipants = new List<Participant_DAL>();


            while (reader.Read())
            {
                var participant = new Participant_DAL(reader.GetInt32(0),
                                        reader.GetDouble(1),
                                        reader.GetString(2),
                                        reader.GetString(3),
                                        reader.GetInt32(4));

                listeDeParticipants.Add(participant);
            }

            DetruireConnexionEtCommande();

            return listeDeParticipants;
        }

        public List<Participant_DAL> GetAllByIDSoiree(int IDSoiree)
        {
            CreerConnexionEtCommande();

            commande.CommandText = "select ID,montant,nom,prenom,idSoiree from Participant where idSoiree=@idSoiree";
            commande.Parameters.Add(new SqlParameter("@idSoiree", IDSoiree));

            var reader = commande.ExecuteReader();

            var listeDeParticipants = new List<Participant_DAL>();

            while (reader.Read())
            {
                var participant = new Participant_DAL(reader.GetInt32(0),
                                        reader.GetDouble(1),
                                        reader.GetString(2),
                                        reader.GetString(3),
                                        reader.GetInt32(4));

                listeDeParticipants.Add(participant);
            }

            DetruireConnexionEtCommande();

            return listeDeParticipants;
        }

        public override Participant_DAL GetByID(int ID)
        {
            CreerConnexionEtCommande();

            commande.CommandText = "select ID,montant,nom,prenom,idSoiree from Participant where ID=@ID";
            commande.Parameters.Add(new SqlParameter("@ID", ID));
            var reader = commande.ExecuteReader();

            Participant_DAL participant;


            if (reader.Read())
            {
                participant = new Participant_DAL(reader.GetInt32(0),
                                        reader.GetDouble(1),
                                        reader.GetString(2),
                                        reader.GetString(3),
                                        reader.GetInt32(4));
            }
            else
                throw new Exception($"Pas de participant dans la BDD avec l'ID {ID}");

            DetruireConnexionEtCommande();

            return participant;
        }

        public override Participant_DAL Insert(Participant_DAL participant)
        {
            CreerConnexionEtCommande();

            commande.CommandText = "insert into Participant(montant, nom, prenom, idSoiree)"
                                    + " values (@montant, @nom, @prenom, @idSoiree); select scope_identity()";
            commande.Parameters.Add(new SqlParameter("@montant", participant.Montant));
            commande.Parameters.Add(new SqlParameter("@nom", participant.Nom));
            commande.Parameters.Add(new SqlParameter("@prenom", participant.Prenom));
            commande.Parameters.Add(new SqlParameter("@idSoiree", participant.IDSoiree));
            var ID = Convert.ToInt32((decimal)commande.ExecuteScalar());

            participant.ID = ID;

            DetruireConnexionEtCommande();

            return participant;
        }

        public override Participant_DAL Update(Participant_DAL participant)
        {
            CreerConnexionEtCommande();

            commande.CommandText = "update Participant set montant=@montant, nom=@nom, prenom=@prenom, idSoiree=@idSoiree"
                                    + " where ID=@ID";
            commande.Parameters.Add(new SqlParameter("@ID", participant.ID));
            commande.Parameters.Add(new SqlParameter("@montant", participant.Montant));
            commande.Parameters.Add(new SqlParameter("@nom", participant.Nom));
            commande.Parameters.Add(new SqlParameter("@prenom", participant.Prenom));
            commande.Parameters.Add(new SqlParameter("@idsoiree", participant.IDSoiree));
            var nombreDeLignesAffectees = (int)commande.ExecuteNonQuery();

            if (nombreDeLignesAffectees != 1)
            {
                throw new Exception($"Impossible de mettre à jour le participant d'ID {participant.ID}");
            }

            DetruireConnexionEtCommande();

            return participant;
        }

        public override void Delete(Participant_DAL participant)
        {
            CreerConnexionEtCommande();

            commande.CommandText = "delete from Participant where ID=@ID";
            commande.Parameters.Add(new SqlParameter("@ID", participant.ID));
            var nombreDeLignesAffectees = (int)commande.ExecuteNonQuery();

            if (nombreDeLignesAffectees != 1)
            {
                throw new Exception($"Impossible de supprimer le participant d'ID {participant.ID}");
            }

            DetruireConnexionEtCommande();
        }


    }
}
