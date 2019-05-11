using System.Collections.Generic;
using System.Linq;
using FuxExMachinaLibrary.Compose.Notes;
using FuxExMachinaLibrary.Enums;
using FuxExMachinaLibrary.Factories;

namespace FuxExMachinaLibrary.Compose
{
    /// <summary>
    /// A class which represents a musical composition.
    /// </summary>
    public class Composition
    {
        /// <summary>
        /// The list of notes which make up the composition.
        /// </summary>
        private readonly List<NotePair> _notePairs = new List<NotePair>();

        /// <summary>
        /// The factory used for creating additional required classes.
        /// </summary>
        private readonly FuxExMachinaFactory _factory;

        /// <summary>
        /// The possible composition contexts the composition can be in.
        /// </summary>
        private readonly CompositionContextCollection _compositionContexts;

        /// <summary>
        /// Composition constructor.
        /// </summary>
        /// <param name="factory">The factory to initialize this composition with</param>
        public Composition(FuxExMachinaFactory factory)
        {
            _factory = factory;
            _compositionContexts = _factory.CompositionContexts;
        }

        /// <summary>
        /// Add notes to the composition.
        /// </summary>
        /// <param name="cantusFirmusNote">The cantus firmus note to add</param>
        /// <param name="counterPointNote">The counterpoint note to add</param>
        /// <param name="arrivedFromCompositionContext">The composition context from which the notes were generated</param>
        /// <param name="arrivedFromNoteChoice">The note choice which led to these notes being generated</param>
        public void AddNotes(
            int cantusFirmusNote,
            int counterPointNote,
            CompositionContext arrivedFromCompositionContext,
            NoteChoice arrivedFromNoteChoice
        )
        {
            _notePairs.Add(
                _factory.CreateNotePair(
                    cantusFirmusNote,
                    counterPointNote,
                    arrivedFromCompositionContext,
                    arrivedFromNoteChoice
                )
            );
        }

        /// <summary>
        /// Retrieves the note pairs making up this composition.
        /// </summary>
        /// <returns></returns>
        public List<NotePair> GetNotePairs()
        {
            return _notePairs;
        }

        /// <summary>
        /// Retrieves the detrimental note pairs in this composition.
        /// </summary>
        /// <returns></returns>
        public IList<NotePair> GetDetrimentalNotePairs()
        {
            return _notePairs.Where(notePair => notePair.IsDetrimental).ToList();
        }

        /// <summary>
        /// Retrieves the current (most recent) notes of this composition.
        /// </summary>
        /// <returns></returns>
        public NotePair GetCurrentNotes()
        {
            return _notePairs.Last();
        }

        /// <summary>
        /// Retrieves the current composition context.
        /// </summary>
        /// <returns></returns>
        public CompositionContext GetCurrentCompositionContext()
        {
            if (_notePairs.Count > 1)
            {
                return _compositionContexts.GetSpecificContext(
                    _factory.CreateCompositionContext(
                        _notePairs[_notePairs.Count - 2],
                        _notePairs[_notePairs.Count - 1]
                    )
                );
            }

            return _compositionContexts.GetSpecificContext(
                _factory.CreateCompositionContext(
                    NoteMotion.Descending,
                    NoteMotionSpan.Step,
                    ScaleDegree.Root,
                    NoteMotion.Descending,
                    NoteMotionSpan.Step,
                    ScaleDegree.Root
                )
            );
        }

        /// <summary>
        /// Resets the composition by clearing all of its notes.
        /// </summary>
        public void Reset()
        {
            _notePairs.Clear();
        }

        public override string ToString()
        {
            var scaleDegreeEvaluator = _factory.ScaleDegreeEvaluator;
            var output = "\n";

            foreach (var notePair in _notePairs.Take(100))
            {
                output +=
                    $"Cantus Firmus: {scaleDegreeEvaluator.GetScaleDegreeFromNote(notePair.CantusFirmusNote)} ({notePair.CantusFirmusNote})";
                output = notePair.CantusFirmusDecorations.Aggregate(output,
                    (current, cantusFirmusDecoration) => current +
                                                         $", {scaleDegreeEvaluator.GetScaleDegreeFromNote(cantusFirmusDecoration)} ({cantusFirmusDecoration})");
                output +=
                    $" | CounterPoint: {scaleDegreeEvaluator.GetScaleDegreeFromNote(notePair.CounterPointNote)} ({notePair.CounterPointNote})";
                output = notePair.CounterPointDecorations.Aggregate(output,
                    (current, counterPointDecoration) => current +
                                                         $", {scaleDegreeEvaluator.GetScaleDegreeFromNote(counterPointDecoration)} ({counterPointDecoration})");
                output += '\n';
            }

            return output;
        }
    }
}