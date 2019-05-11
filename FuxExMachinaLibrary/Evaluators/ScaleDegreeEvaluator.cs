using System;
using FuxExMachinaLibrary.Enums;

namespace FuxExMachinaLibrary.Evaluators
{
    /// <summary>
    /// Class which is used to evaluate and transform scale degrees.
    /// </summary>
    public class ScaleDegreeEvaluator
    {
        /// <summary>
        /// const values representing scale degrees
        /// </summary>
        public const int Root = 1;

        public const int Second = 2;
        public const int Third = 3;
        public const int Fourth = 4;
        public const int Fifth = 5;
        public const int Sixth = 6;
        public const int Seventh = 7;

        /// <summary>
        /// The number of unique notes in the diatonic scale.
        /// </summary>
        private const int NotesPerScale = 7;

        /// <summary>
        /// Do the two given notes create a perfect interval?
        /// </summary>
        /// <param name="leftNote">The left note for comparison</param>
        /// <param name="rightNote">The right note for comparison</param>
        /// <returns>Whether the given notes create a perfect interval or not</returns>
        public bool IsPerfectInterval(int leftNote, int rightNote)
        {
            var leftScaleDegree = GetScaleDegreeFromNote(leftNote);
            var rightScaleDegree = GetScaleDegreeFromNote(rightNote);

            return leftScaleDegree.IsPerfectWith(rightScaleDegree);
        }

        /// <summary>
        /// Generates the correct scale degree for the given note.
        /// </summary>
        /// <param name="note">The note to transform into a ScaleDegree</param>
        /// <returns>A ScaleDegree generated from the given note</returns>
        public ScaleDegree GetScaleDegreeFromNote(int note)
        {
            var normalizedNote = note % NotesPerScale;

            // Handle sevenths
            if (normalizedNote == 0)
            {
                normalizedNote = Seventh;
            }

            switch (normalizedNote)
            {
                case Root:
                {
                    return ScaleDegree.Root;
                }
                case Second:
                {
                    return ScaleDegree.Second;
                }
                case Third:
                {
                    return ScaleDegree.Third;
                }
                case Fourth:
                {
                    return ScaleDegree.Fourth;
                }
                case Fifth:
                {
                    return ScaleDegree.Fifth;
                }
                case Sixth:
                {
                    return ScaleDegree.Sixth;
                }
                case Seventh:
                {
                    return ScaleDegree.Seventh;
                }
                default:
                {
                    throw new Exception("Could not retrieve scale degree by integer.");
                }
            }
        }
    }
}