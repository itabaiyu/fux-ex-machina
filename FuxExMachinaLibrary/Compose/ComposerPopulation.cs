using System;
using System.Collections.Generic;
using System.Linq;
using Atrea.Extensions;
using FuxExMachinaLibrary.Factories;

namespace FuxExMachinaLibrary.Compose
{
    /// <summary>
    /// Class which represents a population of composers. New generations of composers are generated according
    /// to the set hyper-parameters.
    /// </summary>
    public class ComposerPopulation
    {
        /// <summary>
        /// The factory used for creating required classes.
        /// </summary>
        private readonly FuxExMachinaFactory _factory;

        /// <summary>
        /// The composers which make up the composer population.
        /// </summary>
        private List<Composer> _composers = new List<Composer>();

        /// <summary>
        /// The total number of composers in the population.
        /// </summary>
        public uint PopulationCount { get; set; } = 25;

        /// <summary>
        /// The total number of composers which will be used for crossover into the next generation.
        /// </summary>
        public uint CrossoverCount { get; set; } = 10;

        /// <summary>
        /// The total number of generations to run.
        /// </summary>
        public uint GenerationCount { get; set; } = 100;

        /// <summary>
        /// The mutation rate for the composer population.
        /// </summary>
        public double MutationRate { get; set; } = 0.05;

        private const double MutationRateMinimumTolerance = 0.00001;

        /// <summary>
        /// ComposerPopulation constructor.
        /// </summary>
        /// <param name="factory">The factory to use</param>
        public ComposerPopulation(FuxExMachinaFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Initializes the composer population by creating the required number of composers
        /// and initializing their composition strategies with random values.
        /// </summary>
        public void InitializeComposers()
        {
            for (var i = 0; i < PopulationCount; i++)
            {
                var composer = _factory.CreateComposer();
                composer.InitializeCompositionStrategy();
                _composers.Add(composer);
            }
        }

        /// <summary>
        /// Runs through multiple generations of composers:
        /// 1) Each composer trains on a composition.
        /// 2) The composers are evaluated by the CompositionEvaluator.
        /// 3) The best composers are chosen for crossover.
        /// 4) The population count is corrected (reduced or added to) to achieve a full population.
        /// 5) Mutation is applied to the new generation of composers.
        /// </summary>
        public void ProcessGenerations()
        {
            ReportHyperParameters();

            var allTimeBestComposerScore = int.MaxValue;
            var allTimeWorstComposerScore = int.MinValue;

            for (var generationNumber = 1; generationNumber <= GenerationCount; ++generationNumber)
            {
                Train();

                var averageGenerationScore = _composers.Select(c => c.AverageCompositionScore).Average();
                var bestComposerScore = _composers.Select(c => c.AverageCompositionScore).Min();
                var worstComposerScore = _composers.Select(c => c.AverageCompositionScore).Max();

                if (bestComposerScore < allTimeBestComposerScore)
                {
                    allTimeBestComposerScore = bestComposerScore;
                }

                if (worstComposerScore > allTimeWorstComposerScore)
                {
                    allTimeWorstComposerScore = worstComposerScore;
                }

                ReportGenerationStats(
                    generationNumber,
                    averageGenerationScore,
                    bestComposerScore,
                    allTimeBestComposerScore,
                    allTimeWorstComposerScore
                );

                Crossover();
                Mutate();
            }
        }

        /// <summary>
        /// Outputs the chosen hyper-parameters.
        /// </summary>
        private void ReportHyperParameters()
        {
            var logger = _factory.Logger;

            logger.Log();
            logger.Log("Running genetic algorithm with the following hyper-parameters:\n");
            logger.Log($"Population count: {PopulationCount}");
            logger.Log($"Crossover count: {CrossoverCount}");
            logger.Log($"Generation count: {GenerationCount}");
            logger.Log($"Mutation rate: {(int) (MutationRate * 100)}%");
            logger.Log();
            logger.Log("Error rates:\n");
        }

        /// <summary>
        /// Outputs a given composer generation's statistics.
        /// </summary>
        /// <param name="generationNumber">The generation number</param>
        /// <param name="averageGenerationScore">The generation's average score</param>
        /// <param name="bestComposerScore">The best composer score in the given generation</param>
        /// <param name="allTimeBestComposerScore">The all time best composer score across all generations</param>
        /// <param name="allTimeWorstComposerScore">The all time worst composer score across all generations</param>
        private void ReportGenerationStats(
            int generationNumber,
            double averageGenerationScore,
            double bestComposerScore,
            int allTimeBestComposerScore,
            int allTimeWorstComposerScore
        )
        {
            var logger = _factory.Logger;

            logger.Log(
                $"{"Generation " + generationNumber,-15}" +
                $"{" | Average: " + decimal.Parse(Math.Round(averageGenerationScore / 10.0, 2).ToString("N2")),-18}" +
                $"{" | Best: " + decimal.Parse(Math.Round(bestComposerScore / 10.0, 2).ToString("N2")),-15}" +
                $"{" | All Time Best: " + decimal.Parse(Math.Round(allTimeBestComposerScore / 10.0, 2).ToString("N2")),-24}" +
                $"{" | All Time Worst: " + decimal.Parse(Math.Round(allTimeWorstComposerScore / 10.0, 2).ToString("N2"))}"
            );
        }

        /// <summary>
        /// Trains each composer in the population.
        /// </summary>
        public void Train()
        {
            foreach (var composer in _composers)
            {
                composer.Train();
            }
        }

        /// <summary>
        /// Performs crossover using a given number of top performing composers to create children composers, then
        /// corrects the population if inaccurate.
        /// </summary>
        private void Crossover()
        {
            var topComposers = _composers.OrderBy(c => c.AverageCompositionScore).Take((int) (CrossoverCount))
                .ToList();

            // Shuffle the top composers so mates are chosen randomly.
            topComposers.Shuffle();

            var childComposers = new List<Composer>();

            for (int i = 0, j = 1; j < topComposers.Count; i++, j++)
            {
                childComposers.AddRange(topComposers[i].CreateComposerChildren(topComposers[j]));
            }

            _composers = childComposers;

            CorrectPopulationCount();
        }

        /// <summary>
        /// Mutates a given percentage of individuals in the population.
        /// </summary>
        private void Mutate()
        {
            if (Math.Abs(MutationRate) < MutationRateMinimumTolerance)
            {
                return;
            }

            var mutationCount = (int) (_composers.Count * MutationRate);
            var random = _factory.Random;

            for (var i = 0; i < mutationCount; i++)
            {
                _composers[random.Next(_composers.Count)].Mutate(MutationRate);
            }
        }

        /// <summary>
        /// Corrects the population count by purging/adding composers as needed.
        /// </summary>
        private void CorrectPopulationCount()
        {
            if (IsFullPopulation())
            {
                return;
            }

            if (_composers.Count > PopulationCount)
            {
                PurgeExtraIndividuals();
                return;
            }

            BackfillMissingIndividuals();
        }

        /// <summary>
        /// Is the current population full?
        /// </summary>
        /// <returns>Whether the current population is full or not</returns>
        private bool IsFullPopulation() => _composers.Count == PopulationCount;

        /// <summary>
        /// Backfills new composers until a full population is reached. The new composers have randomly initialized
        /// composition strategies.
        /// </summary>
        private void BackfillMissingIndividuals()
        {
            while (true)
            {
                if (IsFullPopulation())
                {
                    break;
                }

                var newComposer = _factory.CreateComposer();
                newComposer.InitializeCompositionStrategy();

                _composers.Add(newComposer);
            }
        }

        /// <summary>
        /// Purges extra composers randomly from the population until a full population is reached.
        /// </summary>
        private void PurgeExtraIndividuals()
        {
            var random = _factory.Random;

            while (true)
            {
                if (IsFullPopulation())
                {
                    break;
                }

                _composers.RemoveAt(random.Next(_composers.Count));
            }
        }

        /// <summary>
        /// Retrieves the final, best composition from the final population.
        /// </summary>
        /// <returns>A final composition to be displayed to the user</returns>
        public Composition GetFinalComposition()
        {
            _composers.ForEach(composer => composer.Compose());
            var finalCompositions = _composers.Select(composer => composer.Composition).ToList();

            return _factory.CompositionEvaluator.GetBestComposition(finalCompositions);
        }
    }
}