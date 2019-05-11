using System.Linq;
using FuxExMachinaLibrary.Compose.Notes;
using FuxExMachinaLibrary.Evaluators.RuleEvaluators;
using FuxExMachinaLibrary.Factories;
using FuxExMachinaTests.Utility;
using NUnit.Framework;

namespace FuxExMachinaTests.EvaluatorTests
{
    public class DoubledLeapRuleEvaluatorTest
    {
        private FuxExMachinaFactory _factory;
        private DoubledLeapRuleEvaluator _ruleEvaluator;

        [SetUp]
        public void Setup()
        {
            _factory = FuxExMachinaTestFactoryProvider.GetTestFactory();
            _ruleEvaluator = new DoubledLeapRuleEvaluator();
        }

        [Test]
        public void EvaluateCompositionWithoutDoubledLeaps()
        {
            const int expectedEvaluation = 0;
            var composition = _factory.CreateComposition();

            composition.AddNotes(1, 3, composition.GetCurrentCompositionContext(), new NoteChoice());
            composition.AddNotes(2, 4, composition.GetCurrentCompositionContext(), new NoteChoice());
            composition.AddNotes(3, 5, composition.GetCurrentCompositionContext(), new NoteChoice());
            composition.AddNotes(7, 2, composition.GetCurrentCompositionContext(), new NoteChoice());

            var evaluation = _ruleEvaluator.EvaluateComposition(composition);
            Assert.AreEqual(expectedEvaluation, evaluation);

            var detrimentalNotePairCount = composition.GetNotePairs().Count(notePair => notePair.IsDetrimental);
            Assert.AreEqual(expectedEvaluation, detrimentalNotePairCount);
        }

        [Test]
        public void EvaluateCompositionWithDoubledLeaps()
        {
            const int expectedEvaluation = 2;
            var composition = _factory.CreateComposition();

            composition.AddNotes(1, 1, composition.GetCurrentCompositionContext(), new NoteChoice());
            composition.AddNotes(5, 10, composition.GetCurrentCompositionContext(), new NoteChoice());
            composition.AddNotes(10, 15, composition.GetCurrentCompositionContext(), new NoteChoice());
            composition.AddNotes(15, 20, composition.GetCurrentCompositionContext(), new NoteChoice());

            var evaluation = _ruleEvaluator.EvaluateComposition(composition);
            Assert.AreEqual(expectedEvaluation, evaluation);

            var detrimentalNotePairCount = composition.GetNotePairs().Count(notePair => notePair.IsDetrimental);
            Assert.AreEqual(expectedEvaluation, detrimentalNotePairCount);
        }

        [Test]
        public void EvaluateCompositionWithDoubledLeapsChangingDirection()
        {
            const int expectedEvaluation = 0;
            var composition = _factory.CreateComposition();

            composition.AddNotes(1, 10, composition.GetCurrentCompositionContext(), new NoteChoice());
            composition.AddNotes(5, 1, composition.GetCurrentCompositionContext(), new NoteChoice());
            composition.AddNotes(1, 10, composition.GetCurrentCompositionContext(), new NoteChoice());
            composition.AddNotes(15, 1, composition.GetCurrentCompositionContext(), new NoteChoice());
            composition.AddNotes(1, 10, composition.GetCurrentCompositionContext(), new NoteChoice());

            var evaluation = _ruleEvaluator.EvaluateComposition(composition);
            Assert.AreEqual(expectedEvaluation, evaluation);

            var detrimentalNotePairCount = composition.GetNotePairs().Count(notePair => notePair.IsDetrimental);
            Assert.AreEqual(expectedEvaluation, detrimentalNotePairCount);
        }
    }
}