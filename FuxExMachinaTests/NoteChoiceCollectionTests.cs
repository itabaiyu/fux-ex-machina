using System.Linq;
using Atrea.Extensions;
using FuxExMachinaLibrary.Factories;
using FuxExMachinaTests.Utility;
using NUnit.Framework;

namespace FuxExMachinaTests
{
    [TestFixture]
    public class NoteChoiceCollectionTests
    {
        private FuxExMachinaFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = FuxExMachinaTestFactoryProvider.GetTestFactory();
        }

        [Test]
        public void OnlyGeneratesUniqueNoteChoices()
        {
            var noteChoiceCollection = _factory.NoteChoices;
            var noteChoices = noteChoiceCollection.GetNoteChoices();

            var uniqueNoteChoices = noteChoices
                .DistinctBy(noteChoice => new
                {
                    noteChoice.CantusFirmusNoteMotion,
                    noteChoice.CantusFirmusScaleDegreeChange,
                    CounterpointNoteMotion = noteChoice.CounterPointNoteMotion,
                    CounterpointScaleDegreeChange = noteChoice.CounterPointScaleDegreeChange
                }).ToList();

            Assert.AreEqual(uniqueNoteChoices.Count, noteChoices.Count);
        }
    }
}