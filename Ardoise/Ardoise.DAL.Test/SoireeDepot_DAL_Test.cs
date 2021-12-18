using System;
using System.Collections.Generic;
using Xunit;

namespace Ardoise.DAL.Test
{
    public class SoireeDepot_DAL_Test
    {
        [Fact]
        public void TestGetAll()
        {
            var soirees = new List<Soiree_DAL>();
            var depotSoiree = new SoireeDepot_DAL();

            soirees = depotSoiree.GetAll();

            Assert.NotNull(soirees);
            Assert.NotNull(soirees[0].Lieu);
        }

        [Fact]
        public void TestGetByID()
        {
            Soiree_DAL soiree;
            var depotSoiree = new SoireeDepot_DAL();

            soiree = depotSoiree.GetByID(1);

            Assert.NotNull(soiree);
            Assert.NotNull(soiree.Lieu);
        }

        [Fact]
        public void TestInsert()
        {
            string? entree = "12/25/2021";
            DateTime? date = DateTime.ParseExact(entree, "MM/dd/yyyy", null);
            var participants = new List<Participant_DAL>();
            participants.Add(new Participant_DAL(50, "Cameron", "RICHARD"));
            participants.Add(new Participant_DAL(20, "Corentin", "CUVELIER"));
            participants.Add(new Participant_DAL(35, "Mathurin", "RATIE"));

            var depotSoiree = new SoireeDepot_DAL();
            var soiree = new Soiree_DAL("Australian", date, participants);

            soiree = depotSoiree.Insert(soiree);

            Assert.Equal("Australian", soiree.Lieu);
            Assert.Equal(soiree.Date, date);
            Assert.Equal(soiree.Participants, participants);
        }

        [Fact]
        public void TestUpdate()
        {
            var depotSoiree = new SoireeDepot_DAL();
            string? entree = "03/15/2022";
            DateTime? date = DateTime.ParseExact(entree, "MM/dd/yyyy", null);
            var soiree = depotSoiree.GetByID(1);
            soiree.Date = date;
            soiree.Lieu = "Dock Yard";

            depotSoiree.Update(soiree);
            var soiree_recup = depotSoiree.GetByID(soiree.ID);

            Assert.Equal(soiree_recup.Date, date);
            Assert.Equal("Dock Yard", soiree_recup.Lieu);
        }

        [Fact]
        public void TestDelete()
        {
            var depotSoiree = new SoireeDepot_DAL();
            var soiree = depotSoiree.GetAll();

            depotSoiree.Delete(soiree[4]);
        }
    }
}
