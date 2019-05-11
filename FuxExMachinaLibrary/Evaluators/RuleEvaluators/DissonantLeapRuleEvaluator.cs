using FuxExMachinaLibrary.Compose;
using FuxExMachinaLibrary.Enums;

namespace FuxExMachinaLibrary.Evaluators.RuleEvaluators
{
    /// <inheritdoc />
    /// <summary>
    /// A ICompositionRuleEvaluator which checks for dissonant leaps within a given composition.
    /// </summary>
    public class DissonantLeapRuleEvaluator : ICompositionRuleEvaluator
    {
        /// <summary>
        /// The scale degree evaluator, used for transforming notes into ScaleDegrees.
        /// </summary>
        private readonly ScaleDegreeEvaluator _scaleDegreeEvaluator;

        /// <summary>
        /// A CompositionContext which is used to determine note motion spans.
        /// </summary>
        private readonly CompositionContext _context;

        /// <summary>
        /// DissonantLeapRuleEvaluator constructor.
        /// </summary>
        /// <param name="scaleDegreeEvaluator">The ScaleDegreeEvaluator to use</param>
        /// <param name="context">The CompositionContext to use</param>
        public DissonantLeapRuleEvaluator(ScaleDegreeEvaluator scaleDegreeEvaluator, CompositionContext context)
        {
            _scaleDegreeEvaluator = scaleDegreeEvaluator;
            _context = context;
        }

        /// <inheritdoc />
        /// <summary>
        /// Evaluates a given composition for dissonant leaps.
        /// </summary>
        /// <param name="composition">The given composition to evaluate</param>
        /// <returns>The composition's total error count</returns>
        public int EvaluateComposition(Composition composition)
        {
            var compositionNotePairs = composition.GetNotePairs();
            var errorCount = 0;

            for (var i = 0; i < compositionNotePairs.Count - 1; ++i)
            {
                var currentNotePair = compositionNotePairs[i];
                var nextNotePair = compositionNotePairs[i + 1];

                if (!IsDissonantLeap(currentNotePair.CantusFirmusNote, nextNotePair.CantusFirmusNote) &&
                    !IsDissonantLeap(currentNotePair.CounterPointNote, nextNotePair.CounterPointNote)) continue;

                nextNotePair.IsDetrimental = true;
                errorCount += 1;
            }

            return errorCount;
        }

        /// <summary>
        /// Determines whether nor not the distance between two notes is a dissonant leap or not.
        /// </summary>
        /// <param name="currentNote">The current note</param>
        /// <param name="nextNote">The next note</param>
        /// <returns>Whether the distance between two notes is a dissonant leap or not</returns>
        private bool IsDissonantLeap(
            int currentNote,
            int nextNote
        )
        {
            if (_context.GetNoteMotionSpanFromNotes(currentNote, nextNote) != NoteMotionSpan.Leap)
            {
                return false;
            }

            var currentScaleDegree = _scaleDegreeEvaluator.GetScaleDegreeFromNote(currentNote);
            var nextScaleDegree = _scaleDegreeEvaluator.GetScaleDegreeFromNote(nextNote);

            return currentScaleDegree.IsDissonantWith(nextScaleDegree);
        }
    }
}