namespace FuxExMachinaLibrary.Compose.Notes
{
    /// <summary>
    /// A weight concept which simulates a given roll space (between the WeightFloor and
    /// WeightCeiling). In context, a weight is tied to a given NoteChoice and used to
    /// determine which NoteChoice will get chosen.
    /// </summary>
    public class NoteChoiceWeight
    {
        /// <summary>
        /// The weight value.
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// The weight floor. Marks the beginning of this weight's roll space.
        /// </summary>
        public int WeightFloor { get; set; }

        /// <summary>
        /// The weight ceiling. Marks the ending of this weight's roll space.
        /// </summary>
        public int WeightCeiling { get; set; }

        /// <summary>
        /// NoteChoiceWeight constructor.
        /// </summary>
        /// <param name="weight">The weight value</param>
        /// <param name="weightFloor">The weight floor</param>
        /// <param name="weightCeiling">The weight ceiling</param>
        public NoteChoiceWeight(int weight, int weightFloor, int weightCeiling)
        {
            Weight = weight;
            WeightFloor = weightFloor;
            WeightCeiling = weightCeiling;
        }
    }
}