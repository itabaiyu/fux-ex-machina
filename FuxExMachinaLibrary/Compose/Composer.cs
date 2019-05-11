using System.Collections.Generic;
using FuxExMachinaLibrary.Compose.Notes;
using FuxExMachinaLibrary.Enums;
using FuxExMachinaLibrary.Evaluators;
using FuxExMachinaLibrary.Factories;

namespace FuxExMachinaLibrary.Compose
{
    /// <summary>
    /// A class which represents a composer with a unique CompositionStrategy used
    /// to create musical compositions.
    /// </summary>
    public class Composer
    {
        /// <summary>
        /// The composition strategy with which to create compositions.
        /// </summary>
        public CompositionStrategy CompositionStrategy { get; set; }

        /// <summary>
        /// The composition this composer composes.
        /// </summary>
        public Composition Composition { get; }

        /// <summary>
        /// The average composition score this composer achieved during training.
        /// </summary>
        public int AverageCompositionScore { get; private set; }

        /// <summary>
        /// The factory class used for generating various other required classes.
        /// </summary>
        private readonly FuxExMachinaFactory _factory;

        /// <summary>
        /// The composition evaluator with which to evaluate this composers compositions.
        /// </summary>
        private readonly CompositionEvaluator _compositionEvaluator;

        /// <summary>
        /// The default note count to train this composer with.
        /// </summary>
        public const int TrainingNoteCount = 1000;

        /// <summary>
        /// The default number of training iterations to train to.
        /// </summary>
        private const int TrainingIterations = 1;

        /// <summary>
        /// The default mutation rate value with which this composer is mutated by mutating
        /// its composition strategy.
        /// </summary>
        private const double MutationRate = 0.05;

        /// <summary>
        /// The starting counterpoint note. Two octaves above the cantus firmus' starting note.
        /// </summary>
        private const int StartingCounterPointNote = 15;

        /// <summary>
        /// The starting cantus firmus note. Two octaves below the counterpoint's starting note.
        /// </summary>
        private const int StartingCantusFirmusNote = 1;

        /// <summary>
        /// The starting note modifier, which helps ensure notes do not enter a negative-valued note space.
        /// todo: Introduce the concept of valid note ranges, apply them to the note choices, and deprecate this.
        /// </summary>
        private const int StartingNoteModifier = 7000;

        /// <summary>
        /// Composer constructor.
        /// </summary>
        /// <param name="factory">The factory to use with this composer</param>
        public Composer(FuxExMachinaFactory factory)
        {
            _factory = factory;
            _compositionEvaluator = _factory.CompositionEvaluator;
            Composition = _factory.CreateComposition();
            CompositionStrategy = _factory.CreateCompositionStrategy();
        }

        /// <summary>
        /// Initializes this composers CompositionStrategy.
        /// </summary>
        public void InitializeCompositionStrategy()
        {
            CompositionStrategy.InitializeStrategyDictionary();
        }

        /// <summary>
        /// Trains this composer by having it create compositions, have them evaluated, and adjusting
        /// its composition strategy based off of the evaluation.
        /// </summary>
        /// <param name="trainingIterations">The number of training iterations to perform</param>
        public void Train(int trainingIterations = TrainingIterations)
        {
            var totalCompositionScore = 0;

            for (var i = 0; i < trainingIterations; i++)
            {
                Compose();

                totalCompositionScore += _compositionEvaluator.EvaluateComposition(Composition);

                OptimizeCompositionStrategy();
            }

            AverageCompositionScore = totalCompositionScore / trainingIterations;
        }

        /// <summary>
        /// The composer generates a composition using its unique CompositionStrategy. The composition is initialized
        /// with the default starting notes (defined above) and then iteratively built by identifying the current
        /// composition context, retrieving the next NoteChoice by giving the CompositionStrategy that context, and
        /// applying that NoteChoice to the current NotePair in order to create the next NotePair.
        /// </summary>
        /// <param name="totalNotes"></param>
        public void Compose(int totalNotes = TrainingNoteCount)
        {
            Composition.Reset();

            var context = Composition.GetCurrentCompositionContext();

            Composition.AddNotes(
                StartingCantusFirmusNote + StartingNoteModifier,
                StartingCounterPointNote + StartingNoteModifier,
                context,
                new NoteChoice()
            );

            for (var i = 0; i < totalNotes - 1; ++i)
            {
                context = Composition.GetCurrentCompositionContext();

                var noteChoice = CompositionStrategy.GetNextNoteChoice(context);
                var currentNotes = Composition.GetCurrentNotes();

                var nextCantusFirmusNote = noteChoice.CantusFirmusNoteMotion == NoteMotion.Ascending
                    ? currentNotes.CantusFirmusNote + noteChoice.CantusFirmusScaleDegreeChange
                    : currentNotes.CantusFirmusNote - noteChoice.CantusFirmusScaleDegreeChange;

                var nextCounterPointNote = noteChoice.CounterPointNoteMotion == NoteMotion.Ascending
                    ? currentNotes.CounterPointNote + noteChoice.CounterPointScaleDegreeChange
                    : currentNotes.CounterPointNote - noteChoice.CounterPointScaleDegreeChange;

                Composition.AddNotes(nextCantusFirmusNote, nextCounterPointNote, context, noteChoice);
            }
        }

        /// <summary>
        /// Optimizes this composer's CompositionStrategy by minimizing the NoteChoiceWeights which
        /// led to note choices which were detrimental to the composition.
        /// </summary>
        public void OptimizeCompositionStrategy()
        {
            foreach (var detrimentalNotePair in Composition.GetDetrimentalNotePairs())
            {
                CompositionStrategy.MinimizeDetrimentalNoteChoiceWeight(
                    detrimentalNotePair.ArrivedFromCompositionContext,
                    detrimentalNotePair.ArrivedFromNoteChoice
                );
            }
        }

        /// <summary>
        /// Creates two child composers by mating this composer with the passed composer and merging
        /// their composition strategies together.
        /// </summary>
        /// <param name="mate">The other composer to mate this composer with</param>
        /// <returns>A list which consists of two composer children.</returns>
        public IList<Composer> CreateComposerChildren(Composer mate)
        {
            var firstChild = _factory.CreateComposer();
            var secondChild = _factory.CreateComposer();

            firstChild.CompositionStrategy = mate.CompositionStrategy.CreateMergedCompositionStrategy(
                CompositionStrategy
            );

            secondChild.CompositionStrategy = CompositionStrategy.CreateMergedCompositionStrategy(
                mate.CompositionStrategy
            );

            return new List<Composer> {firstChild, secondChild};
        }

        /// <summary>
        /// Mutates this composer's CompositionStrategy.
        /// </summary>
        /// <param name="mutationRate">The rate at which to mutate this composer's CompositionStrategy</param>
        /// <seealso cref="CompositionStrategy"/>
        public void Mutate(double mutationRate = MutationRate)
        {
            CompositionStrategy.Mutate(mutationRate);
        }
    }
}