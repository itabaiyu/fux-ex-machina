using System.Linq;
using FuxExMachinaLibrary.Compose;
using FuxExMachinaLibrary.Compose.Notes;
using FuxExMachinaLibrary.Evaluators;
using FuxExMachinaLibrary.Evaluators.RuleEvaluators;
using FuxExMachinaLibrary.Factories;
using FuxExMachinaTests.Utility;
using NUnit.Framework;

namespace FuxExMachinaTests.EvaluatorTests
{
    public class DissonantLeapRuleEvaluatorTest
    {
        private FuxExMachinaFactory _factory;
        private DissonantLeapRuleEvaluator _ruleEvaluator;

        [SetUp]
        public void Setup()
        {
            _factory = FuxExMachinaTestFactoryProvider.GetTestFactory();
            var scaleDegreeEvaluator = new ScaleDegreeEvaluator();
            _ruleEvaluator = new DissonantLeapRuleEvaluator(scaleDegreeEvaluator, new CompositionContext());
        }

        [Test]
        [TestCase(1, 1, 2, 2, 0)]
        [TestCase(1, 7, 3, 4, 1)]
        [TestCase(1, 1, 4, 7, 1)]
        public void EvaluateComposition(
            int cantusFirmusNote,
            int counterPointNote,
            int secondCantusFirmusNote,
            int secondCounterPointNote,
            int expectedEvaluation
        )
        {
            var composition = _factory.CreateComposition();

            composition.AddNotes(cantusFirmusNote, counterPointNote, composition.GetCurrentCompositionContext(),
                new NoteChoice());
            composition.AddNotes(secondCantusFirmusNote, secondCounterPointNote,
                composition.GetCurrentCompositionContext(), new NoteChoice());

            var evaluation = _ruleEvaluator.EvaluateComposition(composition);
            Assert.AreEqual(expectedEvaluation, evaluation);

            var detrimentalNotePairCount = composition.GetNotePairs().Count(notePair => notePair.IsDetrimental);
            Assert.AreEqual(expectedEvaluation, detrimentalNotePairCount);
        }
    }
}