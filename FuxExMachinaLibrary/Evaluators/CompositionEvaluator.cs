using System.Collections.Generic;
using System.Linq;
using Atrea.Extensions;
using FuxExMachinaLibrary.Compose;
using FuxExMachinaLibrary.Enums;
using FuxExMachinaLibrary.Evaluators.RuleEvaluators;

namespace FuxExMachinaLibrary.Evaluators
{
    /// <summary>
    /// A class which can evaluate compositions and generate their error counts.
    /// </summary>
    public class CompositionEvaluator
    {
        /// <summary>
        /// The aggregate rule evaluator to use when evaluating compositions.
        /// </summary>
        private readonly AggregateRuleEvaluator _aggregateRuleEvaluator;

        /// <summary>
        /// CompositionEvaluator constructor.
        /// </summary>
        /// <param name="aggregateRuleEvaluator">The AggregateRuleEvaluator to use</param>
        public CompositionEvaluator(AggregateRuleEvaluator aggregateRuleEvaluator)
        {
            _aggregateRuleEvaluator = aggregateRuleEvaluator;
        }

        /// <summary>
        /// Builds the AggregateRuleEvaluator's specific evaluators from the given list of chosen evaluators.
        /// </summary>
        /// <param name="chosenEvaluators"></param>
        public void BuildEvaluator(List<CompositionRuleEvaluator> chosenEvaluators = null)
        {
            _aggregateRuleEvaluator.BuildEvaluator(chosenEvaluators);
        }

        /// <summary>
        /// Evaluates a given composition and returns its error count.
        /// </summary>
        /// <param name="composition">The given composition to evaluate</param>
        /// <returns>The composition's total error count</returns>
        public int EvaluateComposition(Composition composition)
        {
            return _aggregateRuleEvaluator.EvaluateComposition(composition);
        }

        /// <summary>
        /// Retrieves the best composition from a list of compositions.
        /// </summary>
        /// <param name="compositions">The compositions being evaluated</param>
        /// <returns>The best composition</returns>
        public Composition GetBestComposition(List<Composition> compositions)
        {
            var compositionScoreLookups = compositions.ToMultiLookup(
                composition => _aggregateRuleEvaluator.EvaluateComposition(composition)
            );

            var bestErrorCount = compositionScoreLookups.Keys.Min();

            return compositionScoreLookups[bestErrorCount].FirstOrDefault();
        }
    }
}