using FuxExMachinaLibrary.Compose;
using FuxExMachinaLibrary.Enums;

namespace FuxExMachinaLibrary.Evaluators.RuleEvaluators
{
    /// <inheritdoc />
    /// <summary>
    /// A specific ICompositionRuleEvaluator which checks for violations of ascending seventh note
    /// motion not resolving to the root.
    /// </summary>
    public class AscendingSeventhRuleEvaluator : ICompositionRuleEvaluator
    {
        /// <summary>
        /// The scale degree evaluator, used for transforming notes into ScaleDegrees.
        /// </summary>
        private readonly ScaleDegreeEvaluator _scaleDegreeEvaluator;

        /// <summary>
        /// AscendingSeventhRuleEvaluator constructor.
        /// </summary>
        /// <param name="scaleDegreeEvaluator">The ScaleDegreeEvaluator to use</param>
        public AscendingSeventhRuleEvaluator(ScaleDegreeEvaluator scaleDegreeEvaluator)
        {
            _scaleDegreeEvaluator = scaleDegreeEvaluator;
        }

        /// <inheritdoc />
        /// <summary>
        /// Evaluates a given composition for ascending seventh note resolution.
        /// </summary>
        /// <param name="composition">The composition to evaluate</param>
        /// <returns>The composition's total error count</returns>
        public int EvaluateComposition(Composition composition)
        {
            var compositionNotePairs = composition.GetNotePairs();
            var errorCount = 0;

            for (var i = 1; i < compositionNotePairs.Count - 1; ++i)
            {
                var currentNotePair = compositionNotePairs[i];
                var nextNotePair = compositionNotePairs[i + 1];

                var nextNotePairArrivedFromContext = nextNotePair.ArrivedFromCompositionContext;

                if (!IncorrectlyResolves(currentNotePair.CantusFirmusNote, nextNotePair.CantusFirmusNote,
                        nextNotePairArrivedFromContext.CantusFirmusNoteMotion) &&
                    !IncorrectlyResolves(currentNotePair.CounterPointNote, nextNotePair.CounterPointNote,
                        nextNotePairArrivedFromContext.CounterPointNoteMotion)) continue;

                nextNotePair.IsDetrimental = true;
                errorCount += 1;
            }

            return errorCount;
        }

        /// <summary>
        /// Determines whether or not the given notes correctly resolve according to the ascending seventh rule.
        /// </summary>
        /// <param name="currentNote">The current note</param>
        /// <param name="nextNote">The next note</param>
        /// <param name="currentNoteArrivedFromNoteMotion">The note motion which the current note arrived from</param>
        /// <returns>Whether the given notes correctly resolve</returns>
        private bool IncorrectlyResolves(int currentNote, int nextNote, NoteMotion currentNoteArrivedFromNoteMotion)
        {
            return _scaleDegreeEvaluator.GetScaleDegreeFromNote(currentNote) == ScaleDegree.Seventh &&
                   currentNoteArrivedFromNoteMotion == NoteMotion.Ascending &&
                   _scaleDegreeEvaluator.GetScaleDegreeFromNote(nextNote) != ScaleDegree.Root;
        }
    }
}