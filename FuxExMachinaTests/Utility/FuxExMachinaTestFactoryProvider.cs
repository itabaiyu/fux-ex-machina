using Atrea.Utilities;
using FuxExMachinaLibrary.Compose;
using FuxExMachinaLibrary.Compose.Notes;
using FuxExMachinaLibrary.Decorators;
using FuxExMachinaLibrary.Evaluators;
using FuxExMachinaLibrary.Evaluators.RuleEvaluators;
using FuxExMachinaLibrary.Factories;
using FuxExMachinaLibrary.Loggers;

namespace FuxExMachinaTests.Utility
{
    public static class FuxExMachinaTestFactoryProvider
    {
        private static FuxExMachinaFactory _instance;

        public static FuxExMachinaFactory GetTestFactory()
        {
            if (_instance != null) return _instance;

            var random = new CryptoRandom();
            var scaleDegreeEvaluator = new ScaleDegreeEvaluator();
            var compositionContext = new CompositionContext();
            var compositionContexts = new CompositionContextCollection();
            var noteChoices = new NoteChoiceCollection();
            var noteChoiceWeightGenerator = new NoteChoiceWeightGenerator(random);

            var ruleEvaluatorFactory = new RuleEvaluatorFactory(
                new AscendingSeventhRuleEvaluator(scaleDegreeEvaluator),
                new DissonanceRuleEvaluator(),
                new DissonantLeapRuleEvaluator(scaleDegreeEvaluator, compositionContext),
                new DoubledLeapRuleEvaluator(),
                new DoubledNoteRuleEvaluator(scaleDegreeEvaluator),
                new LeapReturnRuleEvaluator(),
                new MultipleLeapRuleEvaluator(),
                new ParallelPerfectsRuleEvaluator(scaleDegreeEvaluator)
            );

            var aggregateEvaluator = new AggregateRuleEvaluator(ruleEvaluatorFactory);

            var mordentDecorator = new MordentDecorator(scaleDegreeEvaluator, random);
            var passingToneDecorator = new PassingToneDecorator(scaleDegreeEvaluator, random);
            var appogiaturaDecorator = new AppogiaturaDecorator(scaleDegreeEvaluator, random);

            _instance = new FuxExMachinaFactory(
                compositionContexts,
                noteChoices,
                noteChoiceWeightGenerator,
                scaleDegreeEvaluator,
                new CompositionEvaluator(aggregateEvaluator),
                new CompositionDecorator(passingToneDecorator, mordentDecorator, appogiaturaDecorator),
                new CryptoRandom(),
                new FuxExMachinaNullLogger()
            );

            return _instance;
        }
    }
}