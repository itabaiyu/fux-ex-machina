namespace FuxExMachinaLibrary.Compose.Notes
{
    /// <summary>
    /// A pairing of NoteChoiceWeight and NoteChoice.
    /// </summary>
    /// <seealso cref="CompositionStrategy"/>
    public class NoteChoiceWeightNoteChoice
    {
        /// <summary>
        /// The NoteChoiceWeight tied to the NoteChoice
        /// </summary>
        public NoteChoiceWeight NoteChoiceWeight { get; }

        /// <summary>
        /// The NoteChoice tied to the NoteChoiceWeight
        /// </summary>
        public NoteChoice NoteChoice { get; }

        /// <summary>
        /// NoteChoiceWeightNoteChoice constructor.
        /// </summary>
        /// <param name="noteChoiceWeight">The NoteChoiceWeight object to initialize this NoteChoiceWeightNoteChoice with</param>
        /// <param name="noteChoice">The NoteChoice object to initialize this NoteChoiceWeightNoteChoice with</param>
        public NoteChoiceWeightNoteChoice(NoteChoiceWeight noteChoiceWeight, NoteChoice noteChoice)
        {
            NoteChoiceWeight = noteChoiceWeight;
            NoteChoice = noteChoice;
        }
    }
}