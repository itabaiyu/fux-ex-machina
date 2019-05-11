using System;
using System.Collections.Generic;
using System.Linq;
using FuxExMachinaLibrary.Compose.Notes;

namespace FuxExMachinaLibrary.Compose
{
    /// <summary>
    /// A class which represents a given composer's strategy for composing music.
    /// </summary>
    public class CompositionStrategy
    {
        /// <summary>
        /// The composition strategy dictionary. This dictionary is used to retrieve the next note choice given a
        /// current composition context and roll for note choice weight.
        /// </summary>
        private Dictionary<CompositionContext, List<NoteChoiceWeightNoteChoice>> StrategyDictionary { get; set; } =
            new Dictionary<CompositionContext, List<NoteChoiceWeightNoteChoice>>();

        /// <summary>
        /// The possible note choices which can be made.
        /// </summary>
        private readonly NoteChoiceCollection _noteChoices;

        /// <summary>
        /// The note choice weight generator, used to initialize the strategy dictionary in this CompositionStrategy.
        /// </summary>
        private readonly NoteChoiceWeightGenerator _noteChoiceWeightGenerator;

        /// <summary>
        /// The possible CompositionContexts which can occur within the composition.
        /// </summary>
        private readonly CompositionContextCollection _compositionContexts;

        /// <summary>
        /// The value to which note choice weights are adjusted in the MinimizeDetrimentalNoteChoiceWeight method below.
        /// </summary>
        private const int AdjustedNoteChoiceWeightValue = 1;

        /// <summary>
        /// CompositionStrategy constructor.
        /// </summary>
        /// <param name="noteChoices">The note choices</param>
        /// <param name="noteChoiceWeightGenerator">The note choice weight generator</param>
        /// <param name="compositionContexts">The composition contexts</param>
        public CompositionStrategy(
            NoteChoiceCollection noteChoices,
            NoteChoiceWeightGenerator noteChoiceWeightGenerator,
            CompositionContextCollection compositionContexts
        )
        {
            _noteChoices = noteChoices;
            _noteChoiceWeightGenerator = noteChoiceWeightGenerator;
            _compositionContexts = compositionContexts;
        }

        /// <summary>
        /// Merges a given composition strategy with this composition strategy by merging each strategy dictionary together.
        /// </summary>
        /// <param name="compositionStrategy">The CompositionStrategy to be used when merging</param>
        /// <returns>A CompositionStrategy containing attributes from both this CompositionStrategy and the passed one</returns>
        public CompositionStrategy CreateMergedCompositionStrategy(CompositionStrategy compositionStrategy)
        {
            var strategyDictionary = new Dictionary<CompositionContext, List<NoteChoiceWeightNoteChoice>>();

            var i = 0;
            foreach (var keyValuePair in StrategyDictionary)
            {
                var context = keyValuePair.Key;
                var noteChoiceWeightsNoteChoices = keyValuePair.Value;

                strategyDictionary.Add(
                    context,
                    i % 2 == 0 ? noteChoiceWeightsNoteChoices : compositionStrategy.StrategyDictionary[context]
                );

                i++;
            }

            var newCompositionStrategy =
                new CompositionStrategy(_noteChoices, _noteChoiceWeightGenerator, _compositionContexts)
                {
                    StrategyDictionary = strategyDictionary
                };

            return newCompositionStrategy;
        }

        /// <summary>
        /// Initializes this CompositionStrategy's strategy dictionary with random initialization.
        /// </summary>
        public void InitializeStrategyDictionary()
        {
            var contexts = _compositionContexts.GetCompositionContexts();
            var noteChoices = _noteChoices.GetNoteChoices();

            foreach (var context in contexts)
            {
                var weights = _noteChoiceWeightGenerator.GenerateNoteChoiceWeights(noteChoices.Count);
                var noteWeightsNoteChoices =
                    weights.Select((weight, index) => new NoteChoiceWeightNoteChoice(weight, noteChoices[index]))
                        .ToList();

                StrategyDictionary.Add(context, noteWeightsNoteChoices);
            }
        }

        /// <summary>
        /// Retrieves the next NoteChoice for the composition using the given CompositionContext to key into the strategy
        /// dictionary. Once the possible note choices are retrieved, a roll is made to choose the actual NoteChoice to return.
        /// </summary>
        /// <param name="context">The current CompositionContext</param>
        /// <returns>The next note choice for the composition</returns>
        public NoteChoice GetNextNoteChoice(CompositionContext context)
        {
            var noteChoiceWeightsNoteChoices = StrategyDictionary[context];
            var roll = _noteChoiceWeightGenerator.RollForNoteWeight(noteChoiceWeightsNoteChoices.Count);

            var foundNoteChoiceWeightNoteChoice = noteChoiceWeightsNoteChoices
                .FirstOrDefault(
                    noteWeightNoteChoice => noteWeightNoteChoice.NoteChoiceWeight.WeightFloor <= roll &&
                                            noteWeightNoteChoice.NoteChoiceWeight.WeightCeiling >= roll
                );

            if (foundNoteChoiceWeightNoteChoice != null)
            {
                return foundNoteChoiceWeightNoteChoice.NoteChoice;
            }

            throw new Exception("Unable to retrieve next note choice!");
        }

        /// <summary>
        /// Minimizes the roll space for the given CompositionContext and detrimental NoteChoice to help prevent choosing
        /// that detrimental note again.
        /// </summary>
        /// <param name="context">The CompositionContext which led to the NoteChoice being chosen</param>
        /// <param name="noteChoice">The detrimental NoteChoice</param>
        public void MinimizeDetrimentalNoteChoiceWeight(CompositionContext context, NoteChoice noteChoice)
        {
            var specificContext = _compositionContexts.GetSpecificContext(context);
            var noteChoiceWeightsNoteChoices = StrategyDictionary[specificContext];

            var foundNoteChoiceWeightNoteChoice =
                noteChoiceWeightsNoteChoices.FirstOrDefault(
                    noteWeightNoteChoice => noteWeightNoteChoice.NoteChoice.Equals(noteChoice)
                );

            var index = noteChoiceWeightsNoteChoices.IndexOf(foundNoteChoiceWeightNoteChoice);

            switch (index)
            {
                case -1:
                    return;
                case 0:
                    var firstNoteChoiceWeightNoteChoice = noteChoiceWeightsNoteChoices[0];
                    var secondNoteChoiceWeightNoteChoice = noteChoiceWeightsNoteChoices[1];

                    firstNoteChoiceWeightNoteChoice.NoteChoiceWeight.Weight = AdjustedNoteChoiceWeightValue;
                    firstNoteChoiceWeightNoteChoice.NoteChoiceWeight.WeightCeiling = AdjustedNoteChoiceWeightValue;

                    secondNoteChoiceWeightNoteChoice.NoteChoiceWeight.WeightFloor =
                        firstNoteChoiceWeightNoteChoice.NoteChoiceWeight.Weight + 1;
                    secondNoteChoiceWeightNoteChoice.NoteChoiceWeight.Weight =
                        secondNoteChoiceWeightNoteChoice.NoteChoiceWeight.WeightCeiling -
                        secondNoteChoiceWeightNoteChoice.NoteChoiceWeight.WeightFloor;
                    break;
                default:
                    if (index == noteChoiceWeightsNoteChoices.Count - 1)
                    {
                        var lastNoteChoiceWeightNoteChoice =
                            noteChoiceWeightsNoteChoices[noteChoiceWeightsNoteChoices.Count - 1];
                        var secondToLastNoteChoiceWeightNoteChoice =
                            noteChoiceWeightsNoteChoices[noteChoiceWeightsNoteChoices.Count - 2];

                        lastNoteChoiceWeightNoteChoice.NoteChoiceWeight.Weight = AdjustedNoteChoiceWeightValue;
                        lastNoteChoiceWeightNoteChoice.NoteChoiceWeight.WeightFloor =
                            _noteChoiceWeightGenerator.GetNoteChoiceWeightCeiling(noteChoiceWeightsNoteChoices.Count) -
                            lastNoteChoiceWeightNoteChoice.NoteChoiceWeight.Weight;

                        secondToLastNoteChoiceWeightNoteChoice.NoteChoiceWeight.WeightCeiling =
                            lastNoteChoiceWeightNoteChoice.NoteChoiceWeight.WeightFloor - 1;
                        secondToLastNoteChoiceWeightNoteChoice.NoteChoiceWeight.Weight =
                            secondToLastNoteChoiceWeightNoteChoice.NoteChoiceWeight.WeightCeiling -
                            secondToLastNoteChoiceWeightNoteChoice.NoteChoiceWeight.WeightFloor;
                    }
                    else
                    {
                        var noteChoiceWeightNoteChoice = noteChoiceWeightsNoteChoices[index];
                        var nextNoteChoiceWeightNoteChoice = noteChoiceWeightsNoteChoices[index + 1];

                        noteChoiceWeightNoteChoice.NoteChoiceWeight.Weight = AdjustedNoteChoiceWeightValue;
                        noteChoiceWeightNoteChoice.NoteChoiceWeight.WeightCeiling =
                            noteChoiceWeightNoteChoice.NoteChoiceWeight.WeightFloor +
                            noteChoiceWeightNoteChoice.NoteChoiceWeight.Weight;

                        nextNoteChoiceWeightNoteChoice.NoteChoiceWeight.WeightFloor =
                            noteChoiceWeightNoteChoice.NoteChoiceWeight.WeightCeiling + 1;
                        nextNoteChoiceWeightNoteChoice.NoteChoiceWeight.Weight =
                            nextNoteChoiceWeightNoteChoice.NoteChoiceWeight.WeightCeiling -
                            nextNoteChoiceWeightNoteChoice.NoteChoiceWeight.WeightFloor;
                    }

                    break;
            }
        }

        /// <summary>
        /// Mutates this CompositionStrategy to a given mutation rate.
        /// </summary>
        /// <param name="mutationRate">The rate at which to mutate this CompositionStrategy</param>
        public void Mutate(double mutationRate)
        {
            var mutationCount = (int) (StrategyDictionary.Count * mutationRate);
            var contexts = _compositionContexts.GetCompositionContexts();
            var noteChoices = _noteChoices.GetNoteChoices();

            for (var i = 0; i < mutationCount; i++)
            {
                var roll = _noteChoiceWeightGenerator.Roll(contexts.Count);
                var mutationContext = contexts[roll];

                var weights = _noteChoiceWeightGenerator.GenerateNoteChoiceWeights(noteChoices.Count);
                var noteWeightsNoteChoices =
                    weights.Select((weight, index) => new NoteChoiceWeightNoteChoice(weight, noteChoices[index]))
                        .ToList();

                StrategyDictionary[mutationContext] = noteWeightsNoteChoices;
            }
        }
    }
}