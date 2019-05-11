using Atrea.Utilities;
using FuxExMachinaLibrary.Compose;
using FuxExMachinaLibrary.Compose.Notes;
using FuxExMachinaLibrary.Decorators;
using FuxExMachinaLibrary.Enums;
using FuxExMachinaLibrary.Evaluators;
using FuxExMachinaLibrary.Loggers;

namespace FuxExMachinaLibrary.Factories
{
    /// <summary>
    /// Factory for creating various classes used in the Fux Ex Machina application.
    /// </summary>
    public class FuxExMachinaFactory
    {
        /// <summary>
        /// A collection of composition contexts.
        /// </summary>
        /// <seealso cref="CompositionContextCollection"/>
        public CompositionContextCollection CompositionContexts { get; }

        /// <summary>
        /// A collection of possible note choices.
        /// </summary>
        /// <seealso cref="NoteChoiceCollection"/>
        public NoteChoiceCollection NoteChoices { get; }

        /// <summary>
        /// A note weight choice generator, used for generating NoteWeightChoices.
        /// </summary>
        /// <seealso cref="NoteChoiceWeightGenerator"/>
        /// <seealso cref="NoteChoiceWeight"/>
        public NoteChoiceWeightGenerator NoteChoiceWeightGenerator { get; }

        /// <summary>
        /// A ScaleDegreeEvaluator used for scale degree evaluation and transformation.
        /// </summary>
        /// <seealso cref="Evaluators.ScaleDegreeEvaluator"/>
        public ScaleDegreeEvaluator ScaleDegreeEvaluator { get; }

        /// <summary>
        /// The composition evaluator to be used.
        /// </summary>
        /// <seealso cref="CompositionEvaluator"/>
        public CompositionEvaluator CompositionEvaluator { get; }

        /// <summary>
        /// The composition decorator to be used.
        /// </summary>
        public CompositionDecorator CompositionDecorator { get; }

        /// <summary>
        /// Random number generator which generates cryptographically strong random numbers.
        /// </summary>
        /// <seealso cref="CryptoRandom"/>
        public CryptoRandom Random { get; }

        /// <summary>
        /// The logger to use.
        /// </summary>
        public IFuxExMachinaLogger Logger { get; }

        /// <summary>
        /// FuxExMachinaFactory constructor.
        /// </summary>
        /// <param name="compositionContexts">The composition contexts to use</param>
        /// <param name="noteChoices">The note choices to use</param>
        /// <param name="noteChoiceWeightGenerator">The note choice weight generator to use</param>
        /// <param name="scaleDegreeEvaluator">The scale degree evaluator to use</param>
        /// <param name="compositionEvaluator">The composition evaluator to use</param>
        /// <param name="compositionDecorator">The composition decorator to use</param>
        /// <param name="random">The random number generator to use</param>
        /// <param name="logger">The logger to use</param>
        public FuxExMachinaFactory(
            CompositionContextCollection compositionContexts,
            NoteChoiceCollection noteChoices,
            NoteChoiceWeightGenerator noteChoiceWeightGenerator,
            ScaleDegreeEvaluator scaleDegreeEvaluator,
            CompositionEvaluator compositionEvaluator,
            CompositionDecorator compositionDecorator,
            CryptoRandom random,
            IFuxExMachinaLogger logger
        )
        {
            CompositionContexts = compositionContexts;
            NoteChoices = noteChoices;
            NoteChoiceWeightGenerator = noteChoiceWeightGenerator;
            ScaleDegreeEvaluator = scaleDegreeEvaluator;
            CompositionEvaluator = compositionEvaluator;
            CompositionDecorator = compositionDecorator;
            Random = random;
            Logger = logger;
        }

        /// <summary>
        /// Creates a composer.
        /// </summary>
        /// <returns>A new Composer</returns>
        public Composer CreateComposer()
        {
            return new Composer(this);
        }

        /// <summary>
        /// Creates a ComposerPopulation.
        /// </summary>
        /// <returns>A new ComposerPopulation</returns>
        public ComposerPopulation CreateComposers()
        {
            return new ComposerPopulation(this);
        }

        /// <summary>
        /// Creates an empty composition.
        /// </summary>
        /// <returns>An empty Composition</returns>
        public Composition CreateComposition()
        {
            return new Composition(this);
        }

        /// <summary>
        /// Creates a CompositionStrategy.
        /// </summary>
        /// <returns>A new CompositionStrategy</returns>
        public CompositionStrategy CreateCompositionStrategy()
        {
            return new CompositionStrategy(NoteChoices, NoteChoiceWeightGenerator, CompositionContexts);
        }

        /// <summary>
        /// Creates a new NotePair with the given values.
        /// </summary>
        /// <param name="cantusFirmusNote">The cantus firmus note to use</param>
        /// <param name="counterPointNote">The counterpoint note to use</param>
        /// <param name="arrivedFromCompositionContext">The arrived from composition context to use</param>
        /// <param name="arrivedFromNoteChoice">The arrived from note choice to use</param>
        /// <returns></returns>
        public NotePair CreateNotePair(
            int cantusFirmusNote,
            int counterPointNote,
            CompositionContext arrivedFromCompositionContext,
            NoteChoice arrivedFromNoteChoice
        )
        {
            return new NotePair(
                cantusFirmusNote,
                counterPointNote,
                arrivedFromCompositionContext,
                arrivedFromNoteChoice,
                ScaleDegreeEvaluator
            );
        }

        /// <summary>
        /// Creates a CompositionContext.
        /// </summary>
        /// <returns>A new CompositionContext</returns>
        public CompositionContext CreateCompositionContext()
        {
            return new CompositionContext();
        }

        /// <summary>
        /// Override for creating a CompositionContext from given NotePair values.
        /// </summary>
        /// <param name="previousNotes">The previous notes to use</param>
        /// <param name="currentNotes">The current notes to use</param>
        /// <returns></returns>
        public CompositionContext CreateCompositionContext(NotePair previousNotes, NotePair currentNotes)
        {
            return new CompositionContext(previousNotes, currentNotes, ScaleDegreeEvaluator);
        }

        /// <summary>
        /// Override for creating a CompositionContext with specified values.
        /// </summary>
        /// <param name="counterPointNoteMotion">The counterpoint NoteMotion to use</param>
        /// <param name="counterPointNoteMotionSpan">The counterpoint NoteMotionSpan to use</param>
        /// <param name="counterPointNoteScaleDegree">The counterpoint ScaleDegree to use</param>
        /// <param name="cantusFirmusNoteMotion">The cantus firmus NoteMotion to use</param>
        /// <param name="cantusFirmusNoteMotionSpan">The cantus firmus NoteMotionSpan to use</param>
        /// <param name="cantusFirmusNoteScaleDegree">The cantus firmus ScaleDegree to use</param>
        /// <returns></returns>
        public CompositionContext CreateCompositionContext(
            NoteMotion counterPointNoteMotion,
            NoteMotionSpan counterPointNoteMotionSpan,
            ScaleDegree counterPointNoteScaleDegree,
            NoteMotion cantusFirmusNoteMotion,
            NoteMotionSpan cantusFirmusNoteMotionSpan,
            ScaleDegree cantusFirmusNoteScaleDegree
        )
        {
            return new CompositionContext(
                counterPointNoteMotion,
                counterPointNoteMotionSpan,
                counterPointNoteScaleDegree,
                cantusFirmusNoteMotion,
                cantusFirmusNoteMotionSpan,
                cantusFirmusNoteScaleDegree
            );
        }
    }
}