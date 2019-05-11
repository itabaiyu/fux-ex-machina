using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Atrea.Extensions;
using FuxExMachinaLibrary.Compose;
using FuxExMachinaLibrary.Enums;
using FuxExMachinaLibrary.Factories;

namespace FuxExMachinaLibrary.Evaluators.RuleEvaluators
{
    /// <inheritdoc />
    /// <summary>
    /// A class which is an aggregation of many ICompositionRuleEvaluators.
    /// </summary>
    public class AggregateRuleEvaluator : ICompositionRuleEvaluator
    {
        /// <summary>
        /// The evaluators to use in this AggregateRuleEvaluator.
        /// </summary>
        private readonly List<ICompositionRuleEvaluator> _evaluators = new List<ICompositionRuleEvaluator>();

        /// <summary>
        /// The rule evaluator factory - used to create specific rule evaluators.
        /// </summary>
        private readonly RuleEvaluatorFactory _factory;

        /// <summary>
        /// A list of default evaluators to use if none are chosen.
        /// </summary>
        public static readonly IList<CompositionRuleEvaluator> DefaultChosenEvaluators =
            new ReadOnlyCollection<CompositionRuleEvaluator>
            (
                new List<CompositionRuleEvaluator>
                {
                    CompositionRuleEvaluator.AscendingSeventhRuleEvaluator,
                    CompositionRuleEvaluator.DissonanceRuleEvaluator,
                    CompositionRuleEvaluator.DissonantLeapRuleEvaluator,
                    CompositionRuleEvaluator.DoubledLeapRuleEvaluator,
                    CompositionRuleEvaluator.DoubledNoteRuleEvaluator,
                    CompositionRuleEvaluator.LeapReturnRuleEvaluator,
                    CompositionRuleEvaluator.MultipleLeapRuleEvaluator,
                    CompositionRuleEvaluator.ParallelPerfectsRuleEvaluator
                }
            );

        /// <summary>
        /// AggregateRuleEvaluator constructor.
        /// </summary>
        /// <param name="factory">The rule evaluator factory to use</param>
        public AggregateRuleEvaluator(RuleEvaluatorFactory factory)
        {
            _factory = factory;
        }

        /// <inheritdoc />
        /// <summary>
        /// Evaluates a given composition and returns its error count.
        /// </summary>
        /// <param name="composition">The given composition to evaluate</param>
        /// <returns>The composition's total error count</returns>
        public int EvaluateComposition(Composition composition)
        {
            if (_evaluators.None())
            {
                BuildEvaluator();
            }

            return _evaluators.Sum(evaluator => evaluator.EvaluateComposition(composition));
        }

        /// <summary>
        /// Builds this AggregateRuleEvaluator by building up its list of evaluators to use.
        /// </summary>
        /// <param name="chosenEvaluators">A list of chosen evaluators to build this AggregateRuleEvaluator with</param>
        public void BuildEvaluator(List<CompositionRuleEvaluator> chosenEvaluators = null)
        {
            chosenEvaluators = chosenEvaluators ?? DefaultChosenEvaluators.ToList();

            chosenEvaluators.ForEach(
                chosenEvaluator =>
                {
                    switch (chosenEvaluator)
                    {
                        case CompositionRuleEvaluator.AscendingSeventhRuleEvaluator:
                            AddEvaluator(_factory.AscendingSeventhRuleEvaluator);
                            break;
                        case CompositionRuleEvaluator.DissonanceRuleEvaluator:
                            AddEvaluator(_factory.DissonanceRuleEvaluator);
                            break;
                        case CompositionRuleEvaluator.DissonantLeapRuleEvaluator:
                            AddEvaluator(_factory.DissonantLeapRuleEvaluator);
                            break;
                        case CompositionRuleEvaluator.DoubledLeapRuleEvaluator:
                            AddEvaluator(_factory.DoubledLeapRuleEvaluator);
                            break;
                        case CompositionRuleEvaluator.DoubledNoteRuleEvaluator:
                            AddEvaluator(_factory.DoubledNoteRuleEvaluator);
                            break;
                        case CompositionRuleEvaluator.LeapReturnRuleEvaluator:
                            AddEvaluator(_factory.LeapReturnRuleEvaluator);
                            break;
                        case CompositionRuleEvaluator.MultipleLeapRuleEvaluator:
                            AddEvaluator(_factory.MultipleLeapRuleEvaluator);
                            break;
                        case CompositionRuleEvaluator.ParallelPerfectsRuleEvaluator:
                            AddEvaluator(_factory.ParallelPerfectsRuleEvaluator);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(chosenEvaluators));
                    }
                }
            );
        }

        /// <summary>
        /// Adds a specific evaluator to this AggregateRuleEvaluator's list of composition rule evaluators.
        /// </summary>
        /// <param name="evaluator">The specific evaluator to add</param>
        private void AddEvaluator(ICompositionRuleEvaluator evaluator)
        {
            _evaluators.Add(evaluator);
        }
    }
}