using System;
using System.Collections.Generic;
using Xunit;

namespace Ardoise.DAL.Test
{
    public class ParticipantDepot_DAL_Test
    {
        [Fact]
        public void TestGetAll()
        {
            var participant = new List<Participant_DAL>();
            var depotParticipant = new ParticipantDepot_DAL();

            participant = depotParticipant.GetAll();


            Assert.NotNull(participant);
            Assert.NotNull(participant[0].Nom);
        }

        [Fact]
        public void TestGetByIDSoiree()
        {
            var participants = new List<Participant_DAL>();
            var depotParticipant = new ParticipantDepot_DAL();

            participants = depotParticipant.GetAllByIDSoiree(2);

            Assert.Equal(4, participants.Count);
        }

        [Fact]
        public void TestGetByID()
        {
            Participant_DAL participant;
            var depotParticipant = new ParticipantDepot_DAL();

            participant = depotParticipant.GetByID(1);

            Assert.NotNull(participant);
            Assert.NotNull(participant.Nom);
        }

        [Fact]
        public void TestInsert()
        {
            var participant = new Participant_DAL(20, "Corentin", "CUVELIER");
            participant.IDSoiree = 1;
            var depotParticipant = new ParticipantDepot_DAL();

            var participant_insert = depotParticipant.Insert(participant);

            Assert.NotNull(participant_insert);
        }

        [Fact]
        public void TestUpdate()
        {
            var depotParticipant = new ParticipantDepot_DAL();

            var participant = depotParticipant.GetByID(1);
            participant.Nom = "RATIE";
            participant.Montant = 50;

            depotParticipant.Update(participant);
            var soiree_recup = depotParticipant.GetByID(participant.ID);

            Assert.Equal("RATIE", soiree_recup.Nom);
            Assert.Equal(50, soiree_recup.Montant);
        }

        [Fact]
        public void TestDelete()
        {
            var depotParticipant = new ParticipantDepot_DAL();
            var participants = depotParticipant.GetAll();

            depotParticipant.Delete(participants[5]);
        }
    }
}
