using FuxExMachinaLibrary.Compose;
using FuxExMachinaLibrary.Enums;

namespace FuxExMachinaLibrary.Evaluators.RuleEvaluators
{
    /// <inheritdoc />
    /// <summary>
    /// A ICompositionRuleEvaluator which checks for correct leap return behavior - for example,
    /// an ascending leap should be followed by a descending stepwise motion and a descending leap
    /// should be followed by an ascending stepwise motion.
    /// </summary>
    public class LeapReturnRuleEvaluator : ICompositionRuleEvaluator
    {
        /// <inheritdoc />
        /// <summary>
        /// Evaluates a given composition for correct leap return behavior.
        /// </summary>
        /// <param name="composition">The given composition to evaluate</param>
        /// <returns>The composition's total error count</returns>
        public int EvaluateComposition(Composition composition)
        {
            var compositionNotePairs = composition.GetNotePairs();
            var errorCount = 0;

            for (var i = 0; i < compositionNotePairs.Count - 2; ++i)
            {
                var currentNotePairArrivedFromContext = compositionNotePairs[i].ArrivedFromCompositionContext;
                var nextNotePairArrivedFromContext = compositionNotePairs[i + 1].ArrivedFromCompositionContext;

                if (currentNotePairArrivedFromContext.CounterPointNoteMotionSpan != NoteMotionSpan.Leap ||
                    IsCorrectLeapReturn(
                        currentNotePairArrivedFromContext.CounterPointNoteMotionSpan,
                        nextNotePairArrivedFromContext.CounterPointNoteMotionSpan,
                        currentNotePairArrivedFromContext.CounterPointNoteMotion,
                        nextNotePairArrivedFromContext.CounterPointNoteMotion
                    )) continue;

                compositionNotePairs[i].IsDetrimental = true;
                errorCount += 1;
            }

            return errorCount;
        }

        /// <summary>
        /// Determines whether the given NoteMotionSpan and NoteMotion values create a correct leap return.
        /// </summary>
        /// <param name="currentNoteMotionSpan">The current NoteMotionSpan</param>
        /// <param name="nextNoteMotionSpan">The next NoteMotionSpan</param>
        /// <param name="currentNoteMotion">The current NoteMotion</param>
        /// <param name="nexNoteMotion">The next NoteMotion</param>
        /// <returns>the given NoteMotionSpan and NoteMotion values create a correct leap return</returns>
        private static bool IsCorrectLeapReturn(
            NoteMotionSpan currentNoteMotionSpan,
            NoteMotionSpan nextNoteMotionSpan,
            NoteMotion currentNoteMotion,
            NoteMotion nexNoteMotion
        )
        {
            return currentNoteMotionSpan == NoteMotionSpan.Leap &&
                   nextNoteMotionSpan == NoteMotionSpan.Step &&
                   currentNoteMotion != nexNoteMotion;
        }
    }
}