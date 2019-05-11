using System;
using Atrea.Utilities;
using Autofac;
using FuxExMachinaLibrary.Compose;
using FuxExMachinaLibrary.Compose.Notes;
using FuxExMachinaLibrary.Decorators;
using FuxExMachinaLibrary.Evaluators;
using FuxExMachinaLibrary.Evaluators.RuleEvaluators;
using FuxExMachinaLibrary.Factories;
using FuxExMachinaLibrary.Loggers;

namespace FuxExMachina
{
    internal class Program
    {
        private static void Main()
        {
            CreateCompositionWithGeneticAlgorithm();

            Console.ReadLine();
        }

        /// <summary>
        /// Prompts the user for input, runs the genetic algorithm on the population, and outputs
        /// the final composition notes.
        /// </summary>
        private static void CreateCompositionWithGeneticAlgorithm()
        {
            var container = BuildContainer();
            var prompt = container.Resolve<FuxExMachinaPrompt>();

            prompt.PromptWelcome();

            var factory = container.Resolve<FuxExMachinaFactory>();
            var composerPopulation = factory.CreateComposers();

            composerPopulation.PopulationCount = prompt.PromptForPopulationCount();
            composerPopulation.CrossoverCount = prompt.PromptForCrossoverCount(composerPopulation.PopulationCount);
            composerPopulation.GenerationCount = prompt.PromptForGenerationCount();
            composerPopulation.MutationRate = prompt.PromptForMutationRate();

            var evaluators = prompt.PromptForCompositionRuleEvaluators();
            factory.CompositionEvaluator.BuildEvaluator(evaluators);

            composerPopulation.InitializeComposers();
            composerPopulation.ProcessGenerations();

            var finalComposition = composerPopulation.GetFinalComposition();

            var compositionDecorator = factory.CompositionDecorator;
            compositionDecorator.DecorateComposition(finalComposition);

            Console.WriteLine(finalComposition.ToString());
        }

        /// <summary>
        /// Handles building the container for dependency injection.
        /// </summary>
        /// <returns>A built container for resolving classes</returns>
        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<FuxExMachinaFactory>();
            builder.RegisterType<NoteChoiceCollection>().AsSelf().SingleInstance();
            builder.RegisterType<CompositionContextCollection>().AsSelf().SingleInstance();
            builder.RegisterType<NoteChoiceWeightGenerator>().AsSelf().SingleInstance();
            builder.RegisterType<CryptoRandom>().AsSelf().SingleInstance();
            builder.RegisterType<ScaleDegreeEvaluator>().AsSelf().SingleInstance();
            builder.RegisterType<CompositionEvaluator>().AsSelf().SingleInstance();
            builder.RegisterType<AggregateRuleEvaluator>().AsSelf().SingleInstance();
            builder.RegisterType<RuleEvaluatorFactory>().AsSelf().SingleInstance();
            builder.RegisterType<AscendingSeventhRuleEvaluator>().AsSelf().SingleInstance();
            builder.RegisterType<DissonanceRuleEvaluator>().AsSelf().SingleInstance();
            builder.RegisterType<DissonantLeapRuleEvaluator>().AsSelf().SingleInstance();
            builder.RegisterType<DoubledLeapRuleEvaluator>().AsSelf().SingleInstance();
            builder.RegisterType<DoubledNoteRuleEvaluator>().AsSelf().SingleInstance();
            builder.RegisterType<LeapReturnRuleEvaluator>().AsSelf().SingleInstance();
            builder.RegisterType<MultipleLeapRuleEvaluator>().AsSelf().SingleInstance();
            builder.RegisterType<ParallelPerfectsRuleEvaluator>().AsSelf().SingleInstance();
            builder.RegisterType<CompositionContext>().AsSelf().SingleInstance();
            builder.RegisterType<FuxExMachinaPrompt>().AsSelf().SingleInstance();
            builder.RegisterType<PassingToneDecorator>().AsSelf().SingleInstance();
            builder.RegisterType<MordentDecorator>().AsSelf().SingleInstance();
            builder.RegisterType<AppogiaturaDecorator>().AsSelf().SingleInstance();
            builder.RegisterType<CompositionDecorator>().AsSelf().SingleInstance();
            builder.RegisterType<FuxExMachinaConsoleLogger>().As<IFuxExMachinaLogger>().SingleInstance();

            return builder.Build();
        }
    }
}