using FuxExMachinaLibrary.Compose;

namespace FuxExMachinaLibrary.Evaluators.RuleEvaluators
{
    /// <summary>
    /// Interface which all composition rule evaluators should implement.
    /// </summary>
    public interface ICompositionRuleEvaluator
    {
        /// <summary>
        /// Evaluates a given composition.
        /// </summary>
        /// <param name="composition">The given composition to evaluate</param>
        /// <returns>The composition's total error count</returns>
        int EvaluateComposition(Composition composition);
    }
}