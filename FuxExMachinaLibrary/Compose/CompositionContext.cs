using System;
using FuxExMachinaLibrary.Compose.Notes;
using FuxExMachinaLibrary.Enums;
using FuxExMachinaLibrary.Evaluators;

namespace FuxExMachinaLibrary.Compose
{
    /// <summary>
    /// A class which encapsulates the concept of a composition context. It contains information
    /// about the directions, spans, and scale degrees of both the counterpoint and cantus firmus
    /// lines in the composition at a given time.
    /// </summary>
    public class CompositionContext
    {
        /// <summary>
        /// The counterpoint note motion.
        /// </summary>
        /// <seealso cref="NoteMotion"/>
        public NoteMotion CounterPointNoteMotion { get; }

        /// <summary>
        /// The counterpoint note motion span.
        /// </summary>
        /// <seealso cref="NoteMotionSpan"/>
        public NoteMotionSpan CounterPointNoteMotionSpan { get; }

        /// <summary>
        /// The counterpoint scale degree.
        /// </summary>
        /// <seealso cref="ScaleDegree"/>
        public ScaleDegree CounterPointNoteScaleDegree { get; }

        /// <summary>
        /// The cantus firmus note motion.
        /// </summary>
        /// <seealso cref="NoteMotion"/>
        public NoteMotion CantusFirmusNoteMotion { get; }

        /// <summary>
        /// The cantus firmus note motion span.
        /// </summary>
        /// <seealso cref="NoteMotionSpan"/>
        public NoteMotionSpan CantusFirmusNoteMotionSpan { get; }

        /// <summary>
        /// The cantus firmus scale degree.
        /// </summary>
        /// <seealso cref="ScaleDegree"/>
        public ScaleDegree CantusFirmusNoteScaleDegree { get; }

        /// <summary>
        /// The maximum distance between two notes that can be considered non-leap.
        /// </summary>
        private const int MaxNonLeapDistance = 2;

        /// <summary>
        /// CompositionContext constructor: default empty constructor.
        /// </summary>
        public CompositionContext()
        {
        }

        /// <summary>
        /// CompositionContext constructor.
        /// </summary>
        /// <param name="previousNotes">The previous notes in the composition</param>
        /// <param name="currentNotes">The current notes in the composition</param>
        /// <param name="scaleDegreeEvaluator">ScaleDegreeEvaluator used to help initialize class properties</param>
        public CompositionContext(NotePair previousNotes, NotePair currentNotes,
            ScaleDegreeEvaluator scaleDegreeEvaluator)
        {
            CantusFirmusNoteScaleDegree = scaleDegreeEvaluator.GetScaleDegreeFromNote(currentNotes.CantusFirmusNote);
            CantusFirmusNoteMotion = GetNoteMotionFromNotes(
                previousNotes.CantusFirmusNote,
                currentNotes.CantusFirmusNote
            );
            CantusFirmusNoteMotionSpan = GetNoteMotionSpanFromNotes(
                previousNotes.CantusFirmusNote,
                currentNotes.CantusFirmusNote
            );

            CounterPointNoteScaleDegree = scaleDegreeEvaluator.GetScaleDegreeFromNote(currentNotes.CounterPointNote);
            CounterPointNoteMotion = GetNoteMotionFromNotes(
                previousNotes.CounterPointNote,
                currentNotes.CounterPointNote
            );
            CounterPointNoteMotionSpan = GetNoteMotionSpanFromNotes(
                previousNotes.CounterPointNote,
                currentNotes.CounterPointNote
            );
        }

        /// <summary>
        /// CompositionContext constructor: initialize class properties with explicit values.
        /// </summary>
        /// <param name="counterPointNoteMotion">The counterpoint note motion</param>
        /// <param name="counterPointNoteMotionSpan">The counterpoint note motion span</param>
        /// <param name="counterPointNoteScaleDegree">The counterpoint note scale degree</param>
        /// <param name="cantusFirmusNoteMotion">The cantus firmus note motion</param>
        /// <param name="cantusFirmusNoteMotionSpan">The cantus firmus note motion span</param>
        /// <param name="cantusFirmusNoteScaleDegree">The cantus firmus scale degree</param>
        public CompositionContext(
            NoteMotion counterPointNoteMotion,
            NoteMotionSpan counterPointNoteMotionSpan,
            ScaleDegree counterPointNoteScaleDegree,
            NoteMotion cantusFirmusNoteMotion,
            NoteMotionSpan cantusFirmusNoteMotionSpan,
            ScaleDegree cantusFirmusNoteScaleDegree
        )
        {
            CounterPointNoteMotion = counterPointNoteMotion;
            CounterPointNoteMotionSpan = counterPointNoteMotionSpan;
            CounterPointNoteScaleDegree = counterPointNoteScaleDegree;
            CantusFirmusNoteMotion = cantusFirmusNoteMotion;
            CantusFirmusNoteMotionSpan = cantusFirmusNoteMotionSpan;
            CantusFirmusNoteScaleDegree = cantusFirmusNoteScaleDegree;
        }

        /// <summary>
        /// Method to retrieve the note motion from the given notes.
        /// </summary>
        /// <param name="previousNote">The previous note</param>
        /// <param name="currentNote">The current note</param>
        /// <returns>The note motion from the previous note to the current note</returns>
        public NoteMotion GetNoteMotionFromNotes(int previousNote, int currentNote)
        {
            if (previousNote == currentNote)
            {
                return NoteMotion.Oblique;
            }

            return previousNote < currentNote ? NoteMotion.Ascending : NoteMotion.Descending;
        }

        /// <summary>
        /// Method to retrieve the note motion span from the given notes.
        /// </summary>
        /// <param name="previousNote">The previous note</param>
        /// <param name="currentNote">The current note</param>
        /// <returns>The note motion span from the previous note to the current note</returns>
        public NoteMotionSpan GetNoteMotionSpanFromNotes(int previousNote, int currentNote)
        {
            return Math.Abs(previousNote - currentNote) > MaxNonLeapDistance
                ? NoteMotionSpan.Leap
                : NoteMotionSpan.Step;
        }

        /// <summary>
        /// Override of the Equals method to handle CompositionContext equivalence.
        /// </summary>
        /// <param name="context">The CompositionContext to compare with this CompositionContext</param>
        /// <returns>Whether the composition contexts are equal or not</returns>
        public bool Equals(CompositionContext context)
        {
            if (null == context)
            {
                return false;
            }

            if (ReferenceEquals(this, context))
            {
                return true;
            }

            return Equals(CounterPointNoteMotion, context.CounterPointNoteMotion) &&
                   Equals(CounterPointNoteMotionSpan, context.CounterPointNoteMotionSpan) &&
                   Equals(CounterPointNoteScaleDegree, context.CounterPointNoteScaleDegree) &&
                   Equals(CantusFirmusNoteMotion, context.CantusFirmusNoteMotion) &&
                   Equals(CantusFirmusNoteMotionSpan, context.CantusFirmusNoteMotionSpan) &&
                   Equals(CantusFirmusNoteScaleDegree, context.CantusFirmusNoteScaleDegree);
        }
    }
}