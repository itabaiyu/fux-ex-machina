using FuxExMachinaLibrary.Compose;
using FuxExMachinaLibrary.Enums;

namespace FuxExMachinaLibrary.Evaluators.RuleEvaluators
{
    /// <inheritdoc />
    /// <summary>
    /// A ICompositionRuleEvaluator which checks for multiple leaps in the same direction.
    /// </summary>
    public class MultipleLeapRuleEvaluator : ICompositionRuleEvaluator
    {
        /// <inheritdoc />
        /// <summary>
        /// Evaluates a given composition for multiple leaps in the same direction.
        /// </summary>
        /// <param name="composition">The given composition to evaluate</param>
        /// <returns>The composition's total error count</returns>
        public int EvaluateComposition(Composition composition)
        {
            var compositionNotePairs = composition.GetNotePairs();
            var errorCount = 0;

            for (var i = 0; i < compositionNotePairs.Count - 1; ++i)
            {
                var currentNotePairArrivedFromContext = compositionNotePairs[i].ArrivedFromCompositionContext;
                var nextNotePairArrivedFromContext = compositionNotePairs[i + 1].ArrivedFromCompositionContext;

                if (!IsMultipleLeap(
                        currentNotePairArrivedFromContext.CantusFirmusNoteMotion,
                        nextNotePairArrivedFromContext.CantusFirmusNoteMotion,
                        currentNotePairArrivedFromContext.CantusFirmusNoteMotionSpan,
                        nextNotePairArrivedFromContext.CantusFirmusNoteMotionSpan
                    ) && !IsMultipleLeap(
                        currentNotePairArrivedFromContext.CounterPointNoteMotion,
                        nextNotePairArrivedFromContext.CounterPointNoteMotion,
                        currentNotePairArrivedFromContext.CounterPointNoteMotionSpan,
                        nextNotePairArrivedFromContext.CounterPointNoteMotionSpan
                    )) continue;

                compositionNotePairs[i].IsDetrimental = true;
                errorCount += 1;
            }

            return errorCount;
        }

        /// <summary>
        /// Determines whether the NoteMotion and NoteMotionSpan values create multiple leaps in the same direction.
        /// </summary>
        /// <param name="currentNoteArrivedFromNoteMotion">The current arrived from NoteMotion</param>
        /// <param name="nextNoteArrivedFromNoteMotion">The next arrived from NoteMotion</param>
        /// <param name="currentNoteArrivedFromNoteMotionSpan">The current arrived from NoteMotionSpan</param>
        /// <param name="nextNoteArrivedFromNoteMotionSpan">The next arrived from NoteMotionSpan</param>
        /// <returns></returns>
        private static bool IsMultipleLeap(
            NoteMotion currentNoteArrivedFromNoteMotion,
            NoteMotion nextNoteArrivedFromNoteMotion,
            NoteMotionSpan currentNoteArrivedFromNoteMotionSpan,
            NoteMotionSpan nextNoteArrivedFromNoteMotionSpan
        )
        {
            if (currentNoteArrivedFromNoteMotionSpan != NoteMotionSpan.Leap)
            {
                return false;
            }

            return currentNoteArrivedFromNoteMotionSpan == nextNoteArrivedFromNoteMotionSpan &&
                   currentNoteArrivedFromNoteMotion == nextNoteArrivedFromNoteMotion;
        }
    }
}