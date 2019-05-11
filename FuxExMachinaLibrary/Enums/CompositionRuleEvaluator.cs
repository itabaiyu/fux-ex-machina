using System;
using FuxExMachinaLibrary.Evaluators.RuleEvaluators;

namespace FuxExMachinaLibrary.Enums
{
    /// <summary>
    /// Enum which represents the available composition rule evaluators.
    /// </summary>
    /// <seealso cref="AggregateRuleEvaluator"/>
    public enum CompositionRuleEvaluator
    {
        AscendingSeventhRuleEvaluator,
        DissonanceRuleEvaluator,
        DissonantLeapRuleEvaluator,
        DoubledLeapRuleEvaluator,
        DoubledNoteRuleEvaluator,
        LeapReturnRuleEvaluator,
        MultipleLeapRuleEvaluator,
        ParallelPerfectsRuleEvaluator
    }

    /// <summary>
    /// Static class providing descriptions of each of the evaluator functionalities.
    /// </summary>
    public static class CompositionRuleEvaluators
    {
        public static string Description(this CompositionRuleEvaluator evaluator)
        {
            switch (evaluator)
            {
                case CompositionRuleEvaluator.AscendingSeventhRuleEvaluator:
                    return "Learn to correctly resolve ascending sevenths";
                case CompositionRuleEvaluator.DissonanceRuleEvaluator:
                    return "Learn to avoid dissonance";
                case CompositionRuleEvaluator.DissonantLeapRuleEvaluator:
                    return "Learn to avoid dissonant leaps";
                case CompositionRuleEvaluator.DoubledLeapRuleEvaluator:
                    return "Learn to avoid doubled leaps";
                case CompositionRuleEvaluator.DoubledNoteRuleEvaluator:
                    return "Learn to avoid doubled notes";
                case CompositionRuleEvaluator.LeapReturnRuleEvaluator:
                    return "Learn to correctly resolve leaps";
                case CompositionRuleEvaluator.MultipleLeapRuleEvaluator:
                    return "Learn to avoid multiple leaps in the same direction";
                case CompositionRuleEvaluator.ParallelPerfectsRuleEvaluator:
                    return "Learn to avoid parallel perfect intervals";
                default:
                    throw new ArgumentOutOfRangeException(nameof(evaluator), evaluator, null);
            }
        }
    }
}