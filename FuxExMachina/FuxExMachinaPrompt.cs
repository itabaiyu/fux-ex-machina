using System;
using System.Collections.Generic;
using System.Linq;
using FuxExMachinaLibrary.Enums;

namespace FuxExMachina
{
    /// <summary>
    /// A class which prompts for user input.
    /// </summary>
    public class FuxExMachinaPrompt
    {
        /// <summary>
        /// Displays a welcome message for the program.
        /// </summary>
        public void PromptWelcome()
        {
            Console.WriteLine("Welcome to the Fux Ex Machina composition generator!");
            Console.WriteLine(
                "This program uses a genetic algorithm to produce a composition which follows your chosen musical rules."
            );
            Console.Write("Press <ENTER> to continue. Follow the prompts to enter your desired input:");
            Console.ReadLine();
            Console.WriteLine();
        }

        /// <summary>
        /// Prompts the user for a valid population count.
        /// </summary>
        /// <returns>The user's chosen population count</returns>
        public uint PromptForPopulationCount()
        {
            while (true)
            {
                try
                {
                    Console.Write("Population size (must be greater than 1): ");
                    var populationCount = Convert.ToUInt32(Console.ReadLine());

                    if (populationCount >= 2) return populationCount;
                }
                catch (Exception exception) when (exception is FormatException || exception is OverflowException)
                {
                    Console.WriteLine("Input was given in an invalid format.");
                }
            }
        }

        /// <summary>
        /// Prompts the user for a valid crossover count.
        /// </summary>
        /// <param name="populationCount">The population count - used for validation</param>
        /// <returns>The user's chosen crossover count</returns>
        public uint PromptForCrossoverCount(uint populationCount)
        {
            var crossoverCount = uint.MaxValue;

            while (crossoverCount > populationCount || crossoverCount < 2)
            {
                try
                {
                    Console.Write(
                        $"Crossover count (must be less than or equal to population size ({populationCount}) and greater than 1): "
                    );
                    crossoverCount = Convert.ToUInt32(Console.ReadLine());
                }
                catch (Exception exception) when (exception is FormatException || exception is OverflowException)
                {
                    Console.WriteLine("Input was given in an invalid format.");
                }
            }

            return crossoverCount;
        }

        /// <summary>
        /// Prompts the user for a valid generation count.
        /// </summary>
        /// <returns>The user's chosen generation count</returns>
        public uint PromptForGenerationCount()
        {
            uint generationCount = 0;
            while (generationCount < 1)
            {
                try
                {
                    Console.Write("Number of generations (greater than zero): ");
                    generationCount = Convert.ToUInt32(Console.ReadLine());
                }
                catch (Exception exception) when (exception is FormatException || exception is OverflowException)
                {
                    Console.WriteLine("Input was given in an invalid format.");
                }
            }

            return generationCount;
        }

        /// <summary>
        /// Prompts the user for a valid mutation rate.
        /// </summary>
        /// <returns>The user's chosen mutation rate</returns>
        public double PromptForMutationRate()
        {
            var mutationRate = uint.MaxValue;
            const int maxMutationRate = 100;

            while (maxMutationRate < mutationRate)
            {
                try
                {
                    Console.Write("Mutation rate (0 - 100): ");
                    mutationRate = Convert.ToUInt32(Console.ReadLine());
                }
                catch (Exception exception) when (exception is FormatException || exception is OverflowException)
                {
                    Console.WriteLine("Input was given in an invalid format.");
                }
            }

            return 1d * mutationRate / maxMutationRate;
        }

        /// <summary>
        /// Prompts the user for the evaluators they want to use when creating a composition.
        /// </summary>
        /// <returns>A list of chosen composition evaluators</returns>
        public List<CompositionRuleEvaluator> PromptForCompositionRuleEvaluators()
        {
            var chosenCompositionRuleEvaluators = new List<CompositionRuleEvaluator>();
            var evaluators = (CompositionRuleEvaluator[]) Enum.GetValues(typeof(CompositionRuleEvaluator));

            Console.WriteLine();

            while (true)
            {
                Console.Write("Use default composition rules? (Y/N): ");

                var userChoice = Console.ReadLine();

                if (userChoice == null || userChoice.ToUpper() != "Y" && userChoice.ToUpper() != "N")
                {
                    Console.WriteLine("Invalid input. Please enter \"Y\" or \"N\".");
                    continue;
                }

                if (userChoice.ToUpper() == "Y")
                {
                    return evaluators.ToList();
                }

                break;
            }

            Console.WriteLine();

            foreach (var evaluator in evaluators)
            {
                while (true)
                {
                    Console.Write($"{evaluator.Description()}? (Y/N): ");

                    var userChoice = Console.ReadLine();

                    if (userChoice == null || userChoice.ToUpper() != "Y" && userChoice.ToUpper() != "N")
                    {
                        Console.WriteLine("Invalid input. Please enter \"Y\" or \"N\".");
                        continue;
                    }

                    if (userChoice.ToUpper() == "Y")
                    {
                        chosenCompositionRuleEvaluators.Add(evaluator);
                    }

                    break;
                }
            }

            if (chosenCompositionRuleEvaluators.Any()) return chosenCompositionRuleEvaluators;

            Console.WriteLine("No composition rules chosen. Using default rules!");
            chosenCompositionRuleEvaluators.AddRange(evaluators.ToList());

            return chosenCompositionRuleEvaluators;
        }
    }
}