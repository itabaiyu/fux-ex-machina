using System.Linq;
using FuxExMachinaLibrary.Compose.Notes;
using FuxExMachinaLibrary.Evaluators;
using FuxExMachinaLibrary.Evaluators.RuleEvaluators;
using FuxExMachinaLibrary.Factories;
using FuxExMachinaTests.Utility;
using NUnit.Framework;

namespace FuxExMachinaTests.EvaluatorTests
{
    public class ParallelPerfectsRuleEvaluatorTest
    {
        private FuxExMachinaFactory _factory;
        private ParallelPerfectsRuleEvaluator _ruleEvaluator;

        [SetUp]
        public void Setup()
        {
            _factory = FuxExMachinaTestFactoryProvider.GetTestFactory();
            _ruleEvaluator = new ParallelPerfectsRuleEvaluator(new ScaleDegreeEvaluator());
        }

        [Test]
        public void EvaluateCompositionWithoutParallelPerfects()
        {
            const int expectedEvaluation = 0;
            var composition = _factory.CreateComposition();

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
        public void EvaluateCompositionWithParallelPerfects()
        {
            const int expectedEvaluation = 3;
            var composition = _factory.CreateComposition();

            composition.AddNotes(1, 5, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(2, 6, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(3, 7, _factory.CreateCompositionContext(), new NoteChoice());
            composition.AddNotes(4, 8, _factory.CreateCompositionContext(), new NoteChoice());

            var evaluation = _ruleEvaluator.EvaluateComposition(composition);
            Assert.AreEqual(expectedEvaluation, evaluation);

            var detrimentalNotePairCount = composition.GetNotePairs().Count(notePair => notePair.IsDetrimental);
            Assert.AreEqual(expectedEvaluation, detrimentalNotePairCount);
        }
    }
}