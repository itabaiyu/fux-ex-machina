using FuxExMachinaLibrary.Enums;

namespace FuxExMachinaLibrary.Compose.Notes
{
    /// <summary>
    /// A class which represents the next chosen notes for the musical line
    /// being generated. It encapsulates note motion and degree of change for
    /// both the cantus firmus and counterpoint lines.
    /// </summary>
    public class NoteChoice
    {
        /// <summary>
        /// The motion to be used to arrive at the next counterpoint note.
        /// </summary>
        public NoteMotion CounterPointNoteMotion { get; }

        /// <summary>
        /// The motion to be used to arrive at the next cantus firmus note.
        /// </summary>
        public NoteMotion CantusFirmusNoteMotion { get; }

        /// <summary>
        /// The number of scale degrees which the counterpoint will move to
        /// reach its next note.
        /// </summary>
        public int CounterPointScaleDegreeChange { get; }

        /// <summary>
        /// The number of scale degrees which the cantus firmus will move to
        /// reach its next note.
        /// </summary>
        public int CantusFirmusScaleDegreeChange { get; }

        /// <summary>
        /// NoteChoice constructor.
        /// </summary>
        /// <param name="counterPointNoteMotion">The counterpoint note motion</param>
        /// <param name="cantusFirmusNoteMotion">The cantus firmus note motion</param>
        /// <param name="counterPointScaleDegreeChange">The number of scale degrees the counterpoint should change</param>
        /// <param name="cantusFirmusScaleDegreeChange">The number of scale degrees the cantus firmus should change</param>
        public NoteChoice(
            NoteMotion counterPointNoteMotion = NoteMotion.Oblique,
            NoteMotion cantusFirmusNoteMotion = NoteMotion.Oblique,
            int counterPointScaleDegreeChange = 0,
            int cantusFirmusScaleDegreeChange = 0
        )
        {
            CantusFirmusNoteMotion = cantusFirmusNoteMotion;
            CounterPointNoteMotion = counterPointNoteMotion;
            CantusFirmusScaleDegreeChange = cantusFirmusScaleDegreeChange;
            CounterPointScaleDegreeChange = counterPointScaleDegreeChange;
        }

        /// <summary>
        /// Override of the Equals method, to handle NoteChoice equivalence.
        /// </summary>
        /// <param name="noteChoice">The other NoteChoice we are comparing this NoteChoice to</param>
        /// <returns>Whether the two NoteChoice objects are equal or not.</returns>
        public bool Equals(NoteChoice noteChoice)
        {
            if (null == noteChoice)
            {
                return false;
            }

            if (ReferenceEquals(this, noteChoice))
            {
                return true;
            }

            return Equals(CounterPointNoteMotion, noteChoice.CounterPointNoteMotion) &&
                   Equals(CantusFirmusNoteMotion, noteChoice.CantusFirmusNoteMotion) &&
                   Equals(CounterPointScaleDegreeChange, noteChoice.CounterPointScaleDegreeChange) &&
                   Equals(CantusFirmusScaleDegreeChange, noteChoice.CantusFirmusScaleDegreeChange);
        }
    }
}