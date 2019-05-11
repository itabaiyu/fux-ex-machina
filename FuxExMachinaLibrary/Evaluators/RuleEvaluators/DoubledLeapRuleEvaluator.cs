using FuxExMachinaLibrary.Compose;
using FuxExMachinaLibrary.Enums;

namespace FuxExMachinaLibrary.Evaluators.RuleEvaluators
{
    /// <inheritdoc />
    /// <summary>
    /// A ICompositionRuleEvaluator which checks for doubled leap behavior in a given composition.
    /// </summary>
    public class DoubledLeapRuleEvaluator : ICompositionRuleEvaluator
    {
        /// <inheritdoc />
        /// <summary>
        /// Evaluates a given composition for doubled leap behavior.
        /// </summary>
        /// <param name="composition">The given composition to evaluate</param>
        /// <returns>The composition's total error count</returns>
        public int EvaluateComposition(Composition composition)
        {
            var notePairs = composition.GetNotePairs();
            var errorCount = 0;

            for (var index = 0; index < notePairs.Count - 1; index++)
            {
                var notePair = notePairs[index];
                var nextNotePair = notePairs[index + 1];

                var arrivedFromCompositionContext = nextNotePair.ArrivedFromCompositionContext;

                if (arrivedFromCompositionContext.CantusFirmusNoteMotionSpan != NoteMotionSpan.Leap ||
                    arrivedFromCompositionContext.CounterPointNoteMotionSpan != NoteMotionSpan.Leap ||
                    arrivedFromCompositionContext.CantusFirmusNoteMotion !=
                    arrivedFromCompositionContext.CounterPointNoteMotion) continue;

                notePair.IsDetrimental = true;
                errorCount += 1;
            }

            return errorCount;
        }
    }
}