using FuxExMachinaLibrary.Compose;

namespace FuxExMachinaLibrary.Evaluators.RuleEvaluators
{
    /// <inheritdoc />
    /// <summary>
    /// A ICompositionRuleEvaluator which checks for parallel perfects within a given composition.
    /// </summary>
    public class ParallelPerfectsRuleEvaluator : ICompositionRuleEvaluator
    {
        /// <summary>
        /// The scale degree evaluator, used for determining perfect intervals from notes.
        /// </summary>
        private readonly ScaleDegreeEvaluator _scaleDegreeEvaluator;

        /// <summary>
        /// ParallelPerfectsRuleEvaluator constructor.
        /// </summary>
        /// <param name="scaleDegreeEvaluator">The ScaleDegreeEvaluator to use</param>
        public ParallelPerfectsRuleEvaluator(ScaleDegreeEvaluator scaleDegreeEvaluator)
        {
            _scaleDegreeEvaluator = scaleDegreeEvaluator;
        }

        /// <inheritdoc />
        /// <summary>
        /// Evaluates a given composition for parallel perfect behavior.
        /// </summary>
        /// <param name="composition">The given composition to evaluate</param>
        /// <returns>The composition's total error count</returns>
        public int EvaluateComposition(Composition composition)
        {
            var notePairs = composition.GetNotePairs();
            var errorCount = 0;

            for (var i = 0; i < notePairs.Count - 1; ++i)
            {
                var currentNotePair = notePairs[i];
                var nextNotePair = notePairs[i + 1];

                if (!_scaleDegreeEvaluator.IsPerfectInterval(
                        currentNotePair.CantusFirmusNote,
                        currentNotePair.CounterPointNote
                    ) ||
                    !_scaleDegreeEvaluator.IsPerfectInterval(
                        nextNotePair.CantusFirmusNote,
                        nextNotePair.CounterPointNote
                    )) continue;

                nextNotePair.IsDetrimental = true;
                errorCount += 1;
            }

            return errorCount;
        }
    }
}