using System.Collections.Generic;
using FuxExMachinaLibrary.Enums;
using FuxExMachinaLibrary.Evaluators;

namespace FuxExMachinaLibrary.Compose.Notes
{
    /// <summary>
    /// A class encapsulating the concept of a pair of notes. Also contains information
    /// about how the notes were chosen.
    /// </summary>
    public class NotePair
    {
        /// <summary>
        /// The cantus firmus note of this note pair.
        /// </summary>
        public int CantusFirmusNote { get; }

        /// <summary>
        /// Decorations to the cantus firmus note.
        /// </summary>
        public List<int> CantusFirmusDecorations { get; } = new List<int>();

        /// <summary>
        /// The counterpoint note of this note pair.
        /// </summary>
        public int CounterPointNote { get; }

        /// <summary>
        /// Decorations to the counterpoint note.
        /// </summary>
        public List<int> CounterPointDecorations { get; } = new List<int>();

        /// <summary>
        /// The composition context which this NotePair was generated from.
        /// </summary>
        public CompositionContext ArrivedFromCompositionContext { get; }

        /// <summary>
        /// The note choice which led to this NotePair being generated.
        /// </summary>
        public NoteChoice ArrivedFromNoteChoice { get; }

        /// <summary>
        /// Is this NotePair detrimental to the composition quality?
        /// </summary>
        public bool IsDetrimental { get; set; }

        /// <summary>
        /// ScaleDegreeEvaluator used to handle scale degree operations within this NotePair class.
        /// See the IsDissonant method below for context.
        /// </summary>
        private readonly ScaleDegreeEvaluator _scaleDegreeEvaluator;

        /// <summary>
        /// NotePair constructor.
        /// </summary>
        /// <param name="cantusFirmusNote">The cantus firmus note</param>
        /// <param name="counterPointNote">The counterpoint note</param>
        /// <param name="arrivedFromCompositionContext">The CompositionContext from which this NotePair was generated</param>
        /// <param name="arrivedFromNoteChoice">The NoteChoice which led to this NotePair being generated</param>
        /// <param name="scaleDegreeEvaluator">The ScaleDegreeEvaluator to use with this NotePair</param>
        public NotePair(
            int cantusFirmusNote,
            int counterPointNote,
            CompositionContext arrivedFromCompositionContext,
            NoteChoice arrivedFromNoteChoice,
            ScaleDegreeEvaluator scaleDegreeEvaluator
        )
        {
            CantusFirmusNote = cantusFirmusNote;
            CounterPointNote = counterPointNote;
            ArrivedFromCompositionContext = arrivedFromCompositionContext;
            ArrivedFromNoteChoice = arrivedFromNoteChoice;
            _scaleDegreeEvaluator = scaleDegreeEvaluator;
        }

        /// <summary>
        /// Is this NotePair dissonant?
        /// </summary>
        /// <returns>Whether the NotePair is dissnont or not</returns>
        public bool IsDissonant()
        {
            var cantusFirmusScaleDegree = _scaleDegreeEvaluator.GetScaleDegreeFromNote(CantusFirmusNote);
            var counterPointScaleDegree = _scaleDegreeEvaluator.GetScaleDegreeFromNote(CounterPointNote);

            return cantusFirmusScaleDegree.IsDissonantWith(counterPointScaleDegree);
        }
    }
}