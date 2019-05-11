using System.Linq;
using FuxExMachinaLibrary.Compose.Notes;
using FuxExMachinaLibrary.Evaluators.RuleEvaluators;
using FuxExMachinaLibrary.Factories;
using FuxExMachinaTests.Utility;
using NUnit.Framework;

namespace FuxExMachinaTests.EvaluatorTests
{
    public class LeapReturnRuleEvaluatorTest
    {
        private FuxExMachinaFactory _factory;
        private LeapReturnRuleEvaluator _ruleEvaluator;
        private readonly NoteChoice _mockNoteChoice = new NoteChoice();

        [SetUp]
        public void Setup()
        {
            _factory = FuxExMachinaTestFactoryProvider.GetTestFactory();
            _ruleEvaluator = new LeapReturnRuleEvaluator();
        }

        [Test]
        [TestCase(1, 4, 3, 0)]
        [TestCase(1, 4, 6, 1)]
        public void EvaluateComposition(
            int counterPointNote,
            int secondCounterPointNote,
            int thirdCounterPointNote,
            int expectedEvaluation
        )
        {
            var composition = _factory.CreateComposition();

            composition.AddNotes(1, 1, composition.GetCurrentCompositionContext(), _mockNoteChoice);
            composition.AddNotes(1, 1, composition.GetCurrentCompositionContext(), _mockNoteChoice);
            composition.AddNotes(1, 1, composition.GetCurrentCompositionContext(), _mockNoteChoice);
            composition.AddNotes(1, counterPointNote, composition.GetCurrentCompositionContext(), _mockNoteChoice);
            composition.AddNotes(1, secondCounterPointNote, composition.GetCurrentCompositionContext(), _mockNoteChoice);
            composition.AddNotes(1, thirdCounterPointNote, composition.GetCurrentCompositionContext(), _mockNoteChoice);
            composition.AddNotes(1, 1, composition.GetCurrentCompositionContext(), _mockNoteChoice);
            composition.AddNotes(1, 1, composition.GetCurrentCompositionContext(), _mockNoteChoice);
            composition.AddNotes(1, 1, composition.GetCurrentCompositionContext(), _mockNoteChoice);

            var evaluation = _ruleEvaluator.EvaluateComposition(composition);
            Assert.AreEqual(expectedEvaluation, evaluation);

            var detrimentalNotePairCount = composition.GetNotePairs().Count(notePair => notePair.IsDetrimental);
            Assert.AreEqual(expectedEvaluation, detrimentalNotePairCount);
        }
    }
}