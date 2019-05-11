using FuxExMachinaLibrary.Compose;

namespace FuxExMachinaLibrary.Evaluators.RuleEvaluators
{
    /// <inheritdoc />
    /// <summary>
    /// A ICompositionRuleEvaluator which checks for doubled notes within a given composition.
    /// </summary>
    public class DoubledNoteRuleEvaluator : ICompositionRuleEvaluator
    {
        /// <summary>
        /// The scale degree evaluator, used for transforming notes into ScaleDegrees.
        /// </summary>
        private readonly ScaleDegreeEvaluator _scaleDegreeEvaluator;

        /// <summary>
        /// DoubledNoteRuleEvaluator constructor.
        /// </summary>
        /// <param name="scaleDegreeEvaluator">The ScaleDegreeEvaluator to use</param>
        public DoubledNoteRuleEvaluator(ScaleDegreeEvaluator scaleDegreeEvaluator)
        {
            _scaleDegreeEvaluator = scaleDegreeEvaluator;
        }

        /// <inheritdoc />
        /// <summary>
        /// Evaluates a given composition for doubled notes.
        /// </summary>
        /// <param name="composition">The given composition to evaluate</param>
        /// <returns>The composition's total error count</returns>
        public int EvaluateComposition(Composition composition)
        {
            var errorCount = 0;

            for (var index = 1; index < composition.GetNotePairs().Count; index++)
            {
                var notePair = composition.GetNotePairs()[index];
                if (_scaleDegreeEvaluator.GetScaleDegreeFromNote(notePair.CantusFirmusNote) !=
                    _scaleDegreeEvaluator.GetScaleDegreeFromNote(notePair.CounterPointNote)) continue;

                notePair.IsDetrimental = true;
                errorCount += 1;
            }

            return errorCount;
        }
    }
}