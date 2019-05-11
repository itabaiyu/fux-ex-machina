using FuxExMachinaLibrary.Evaluators.RuleEvaluators;

namespace FuxExMachinaLibrary.Factories
{
    /// <summary>
    /// Factory for generating ICompositionRuleEvaluators.
    /// </summary>
    public class RuleEvaluatorFactory
    {
        /// <summary>
        /// The AscendingSeventhRuleEvaluator.
        /// </summary>
        public AscendingSeventhRuleEvaluator AscendingSeventhRuleEvaluator { get; }

        /// <summary>
        /// The DissonanceRuleEvaluator.
        /// </summary>
        public DissonanceRuleEvaluator DissonanceRuleEvaluator { get; }

        /// <summary>
        /// The DissonantLeapRuleEvaluator
        /// </summary>
        public DissonantLeapRuleEvaluator DissonantLeapRuleEvaluator { get; }

        /// <summary>
        /// The DoubledLeapRuleEvaluator.
        /// </summary>
        public DoubledLeapRuleEvaluator DoubledLeapRuleEvaluator { get; }

        /// <summary>
        /// The DoubledNoteRuleEvaluator.
        /// </summary>
        public DoubledNoteRuleEvaluator DoubledNoteRuleEvaluator { get; }

        /// <summary>
        /// The LeapReturnRuleEvaluator.
        /// </summary>
        public LeapReturnRuleEvaluator LeapReturnRuleEvaluator { get; }

        /// <summary>
        /// The MultipleLeapRuleEvaluator.
        /// </summary>
        public MultipleLeapRuleEvaluator MultipleLeapRuleEvaluator { get; }

        /// <summary>
        /// ParallelPerfectsRuleEvaluator
        /// </summary>
        public ParallelPerfectsRuleEvaluator ParallelPerfectsRuleEvaluator { get; }

        /// <summary>
        /// RuleEvaluatorFactory constructor.
        /// </summary>
        /// <param name="ascendingSeventhRuleEvaluator">The AscendingSeventhRuleEvaluator to use</param>
        /// <param name="dissonanceRuleEvaluator">The DissonanceRuleEvaluator to use</param>
        /// <param name="dissonantLeapRuleEvaluator">The DissonantLeapRuleEvaluator to use</param>
        /// <param name="doubledLeapRuleEvaluator">The DoubledLeapRuleEvaluator to use</param>
        /// <param name="doubledNoteRuleEvaluator">The DoubledNoteRuleEvaluator to use</param>
        /// <param name="leapReturnRuleEvaluator">The LeapReturnRuleEvaluator to use</param>
        /// <param name="multipleLeapRuleEvaluator">The MultipleLeapRuleEvaluator to use</param>
        /// <param name="parallelPerfectsRuleEvaluator">The ParallelPerfectsRuleEvaluator to use</param>
        public RuleEvaluatorFactory(
            AscendingSeventhRuleEvaluator ascendingSeventhRuleEvaluator,
            DissonanceRuleEvaluator dissonanceRuleEvaluator,
            DissonantLeapRuleEvaluator dissonantLeapRuleEvaluator,
            DoubledLeapRuleEvaluator doubledLeapRuleEvaluator,
            DoubledNoteRuleEvaluator doubledNoteRuleEvaluator,
            LeapReturnRuleEvaluator leapReturnRuleEvaluator,
            MultipleLeapRuleEvaluator multipleLeapRuleEvaluator,
            ParallelPerfectsRuleEvaluator parallelPerfectsRuleEvaluator
        )
        {
            AscendingSeventhRuleEvaluator = ascendingSeventhRuleEvaluator;
            DissonanceRuleEvaluator = dissonanceRuleEvaluator;
            DissonantLeapRuleEvaluator = dissonantLeapRuleEvaluator;
            DoubledLeapRuleEvaluator = doubledLeapRuleEvaluator;
            DoubledNoteRuleEvaluator = doubledNoteRuleEvaluator;
            LeapReturnRuleEvaluator = leapReturnRuleEvaluator;
            MultipleLeapRuleEvaluator = multipleLeapRuleEvaluator;
            ParallelPerfectsRuleEvaluator = parallelPerfectsRuleEvaluator;
        }
    }
}