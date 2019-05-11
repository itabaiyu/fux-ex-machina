using System.Collections.Generic;
using Atrea.Utilities;
using FuxExMachinaLibrary.Enums;
using FuxExMachinaLibrary.Evaluators;

namespace FuxExMachinaLibrary.Decorators
{
    /// <inheritdoc />
    /// <summary>
    /// A BaseCompositionDecorator which adds appogiatura ornamentation to the given composition.
    /// </summary>
    public class AppogiaturaDecorator : BaseCompositionDecorator
    {
        /// <inheritdoc />
        /// <summary>
        /// AppogiaturaDecorator constructor.
        /// </summary>
        /// <param name="scaleDegreeEvaluator">The ScaleDegreeEvaluator to use</param>
        /// <param name="random">The CryptoRandom to use</param>
        public AppogiaturaDecorator(
            ScaleDegreeEvaluator scaleDegreeEvaluator,
            CryptoRandom random
        ) : base(
            scaleDegreeEvaluator,
            random
        )
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Decorates the composition based on the current and next notes.
        /// </summary>
        /// <param name="currentNote">The current note</param>
        /// <param name="nextNote">The next note</param>
        /// <returns>A list of decorative notes, possibly empty</returns>
        protected override List<int> GetPossibleDecorations(int currentNote, int nextNote)
        {
            var currentScaleDegree = ScaleDegreeEvaluator.GetScaleDegreeFromNote(currentNote);
            var nextScaleDegree = ScaleDegreeEvaluator.GetScaleDegreeFromNote(nextNote);

            if (!currentScaleDegree.IsAdjacentTo(nextScaleDegree) || Random.Next(1, 3) % 2 != 0)
            {
                return EmptyDecoration;
            }

            if (currentNote < nextNote)
            {
                return new List<int> {currentNote + 1, currentNote - 1, currentNote};
            }

            return currentScaleDegree != ScaleDegree.Sixth
                ? new List<int> {currentNote - 1, currentNote + 1, currentNote}
                : EmptyDecoration;
        }
    }
}