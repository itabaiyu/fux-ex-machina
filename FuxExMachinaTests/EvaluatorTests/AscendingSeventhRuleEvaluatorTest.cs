using System.Linq;
using FuxExMachinaLibrary.Compose.Notes;
using FuxExMachinaLibrary.Evaluators;
using FuxExMachinaLibrary.Evaluators.RuleEvaluators;
using FuxExMachinaLibrary.Factories;
using FuxExMachinaTests.Utility;
using NUnit.Framework;

namespace FuxExMachinaTests.EvaluatorTests
{
    public class AscendingSeventhRuleEvaluatorTest
    {
        private FuxExMachinaFactory _factory;
        private AscendingSeventhRuleEvaluator _ruleEvaluator;

        [SetUp]
        public void Setup()
        {
            _factory = FuxExMachinaTestFactoryProvider.GetTestFactory();
            _ruleEvaluator = new AscendingSeventhRuleEvaluator(new ScaleDegreeEvaluator());
        }

        [Test]
        [TestCase(6, 7, 8, 0)]
        [TestCase(6, 7, 6, 1)]
        [TestCase(8, 7, 6, 0)]
        [TestCase(8, 7, 8, 0)]
        public void EvaluateComposition(
            int cantusFirmusNote,
            int secondCantusFirmusNote,
            int thirdCantusFirmusNote,
            int expectedEvaluation
        )
        {
            var composition = _factory.CreateComposition();

            composition.AddNotes(cantusFirmusNote, 0, composition.GetCurrentCompositionContext(), new NoteChoice());
            composition.AddNotes(secondCantusFirmusNote, 0, composition.GetCurrentCompositionContext(),
                new NoteChoice());
            composition.AddNotes(thirdCantusFirmusNote, 0, composition.GetCurrentCompositionContext(),
                new NoteChoice());

            var evaluation = _ruleEvaluator.EvaluateComposition(composition);
            Assert.AreEqual(expectedEvaluation, evaluation);

            var detrimentalNotePairCount = composition.GetNotePairs().Count(notePair => notePair.IsDetrimental);
            Assert.AreEqual(expectedEvaluation, detrimentalNotePairCount);
        }
    }
}