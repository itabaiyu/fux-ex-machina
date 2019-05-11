using System.Linq;
using FuxExMachinaLibrary.Compose.Notes;
using FuxExMachinaLibrary.Evaluators.RuleEvaluators;
using FuxExMachinaLibrary.Factories;
using FuxExMachinaTests.Utility;
using NUnit.Framework;

namespace FuxExMachinaTests.EvaluatorTests
{
    [TestFixture]
    public class DissonanceRuleEvaluatorTest
    {
        private FuxExMachinaFactory _factory;
        private DissonanceRuleEvaluator _ruleEvaluator;

        [SetUp]
        public void Setup()
        {
            _factory = FuxExMachinaTestFactoryProvider.GetTestFactory();
            _ruleEvaluator = new DissonanceRuleEvaluator();
        }

        [Test]
        public void EvaluateCompositionWithoutDissonance()
        {
            const int expectedEvaluation = 0;
            var composition = _factory.CreateComposition();

            // Non-dissonant
            composition.AddNotes(1, 3, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(2, 4, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(3, 5, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(7, 2, _factory.CreateCompositionContext(), new NoteChoice());

            var evaluation = _ruleEvaluator.EvaluateComposition(composition);
            Assert.AreEqual(expectedEvaluation, evaluation);

            var detrimentalNotePairCount = composition.GetNotePairs().Count(notePair => notePair.IsDetrimental);
            Assert.AreEqual(expectedEvaluation, detrimentalNotePairCount);
        }

        [Test]
        public void EvaluateCompositionWithDissonance()
        {
            const int expectedEvaluation = 12;
            var composition = _factory.CreateComposition();

            // whole-steps/sevenths
            composition.AddNotes(1, 7, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(1, 2, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(2, 3, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(3, 4, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(4, 5, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(5, 6, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(6, 7, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(7, 8, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(7, 1, _factory.CreateCompositionContext(), new NoteChoice());

            // Tri-tones
            composition.AddNotes(7, 4, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(4, 7, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(7, 13, _factory.CreateCompositionContext(), new NoteChoice());

            var evaluation = _ruleEvaluator.EvaluateComposition(composition);
            Assert.AreEqual(expectedEvaluation, evaluation);

            var detrimentalNotePairCount = composition.GetNotePairs().Count(notePair => notePair.IsDetrimental);
            Assert.AreEqual(expectedEvaluation, detrimentalNotePairCount);
        }

        [Test]
        public void EvaluateCompositionWithMixedDissonance()
        {
            const int expectedEvaluation = 12;
            var composition = _factory.CreateComposition();

            // whole-steps/sevenths
            composition.AddNotes(1, 7, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(1, 2, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(2, 3, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(3, 4, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(4, 5, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(5, 6, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(6, 7, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(7, 8, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(7, 1, _factory.CreateCompositionContext(), new NoteChoice());

            // Tri-tones
            composition.AddNotes(7, 4, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(4, 7, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(7, 13, _factory.CreateCompositionContext(), new NoteChoice());

            // Non-dissonant
            composition.AddNotes(1, 3, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(2, 4, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(3, 5, _factory.CreateCompositionContext(), new NoteChoice());

            var evaluation = _ruleEvaluator.EvaluateComposition(composition);
            Assert.AreEqual(expectedEvaluation, evaluation);

            var detrimentalNotePairCount = composition.GetNotePairs().Count(notePair => notePair.IsDetrimental);
            Assert.AreEqual(expectedEvaluation, detrimentalNotePairCount);
        }
    }
}