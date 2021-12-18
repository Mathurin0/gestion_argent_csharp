using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Ardoise.DAL
{
    public class Soiree_DAL
    {
        public int ID { get; set; }

        public string Lieu { get; set; }
        public DateTime? Date { get; set; }

        public List<Participant_DAL> Participants { get; set; }

        public Soiree_DAL(string lieu, DateTime? date, IEnumerable<Participant_DAL> participants)
                    => (Lieu, Date, Participants) = (lieu, date, participants.ToList());
        public Soiree_DAL(int id, string lieu, DateTime? date, IEnumerable<Participant_DAL> participants)
                    => (ID, Lieu, Date, Participants) = (id, lieu, date, participants.ToList());

        public void Insert(SqlConnection connexion)
        {

            using (var commande = new SqlCommand())
            {
                commande.Connection = connexion;
                commande.CommandText = "insert into Soiree (lieu, date)"
                                        + " values (@lieu, @date); SELECT SCOPE_IDENTITY()";
                commande.Parameters.Add(new SqlParameter("@lieu", Lieu));
                commande.Parameters.Add(new SqlParameter("@date", Date));
                ID = (int)commande.ExecuteScalar();
            }

            foreach (var item in Participants)
            {
                item.IDSoiree = ID;
                item.Insert(connexion);
            }
        }
    }
}
