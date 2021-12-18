using System;
using System.Data.SqlClient;

namespace Ardoise.DAL
{
    public class Participant_DAL
    {
        public double Montant { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; private set; }

        public int IDSoiree { get; set; }

        public int ID { get; set; }

        public Participant_DAL(double montant, string nom, string prenom) => (Montant, Nom, Prenom) = (montant, nom, prenom);

        public Participant_DAL(int id, double montant, string nom, string prenom, int idSoiree)
                => (ID, Montant, Nom, Prenom, IDSoiree) = (id, montant, nom, prenom, idSoiree);

        internal void Insert(SqlConnection connexion)
        {
            using (var commande = new SqlCommand())
            {
                commande.Connection = connexion;
                commande.CommandText = "insert into Participant(montant, nom, prenom, idSoiree)"
                                + " values (montant, nom, prenom, @idSoiree)";
                commande.Parameters.Add(new SqlParameter("@montant", Montant));
                commande.Parameters.Add(new SqlParameter("@nom", Nom));
                commande.Parameters.Add(new SqlParameter("@prenom", Prenom));
                commande.Parameters.Add(new SqlParameter("@idSoiree", IDSoiree));

                commande.ExecuteNonQuery();
            }
        }
    }
}