using FuxExMachinaLibrary.Compose;

namespace FuxExMachinaLibrary.Evaluators.RuleEvaluators
{
    /// <inheritdoc />
    /// <summary>
    /// A ICompositionRuleEvaluator which checks for dissonance within a given composition.
    /// </summary>
    public class DissonanceRuleEvaluator : ICompositionRuleEvaluator
    {
        /// <inheritdoc />
        /// <summary>
        /// Evaluates the given composition for dissonant notes.
        /// </summary>
        /// <param name="composition">The composition to evaluate</param>
        /// <returns>The composition's total error count</returns>
        public int EvaluateComposition(Composition composition)
        {
            var errorCount = 0;

            composition.GetNotePairs().ForEach(
                notePair =>
                {
                    if (!notePair.IsDissonant()) return;

                    notePair.IsDetrimental = true;
                    errorCount++;
                }
            );

            return errorCount;
        }
    }
}