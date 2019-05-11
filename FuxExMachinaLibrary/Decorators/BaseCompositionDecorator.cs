using System.Collections.Generic;
using Atrea.Extensions;
using Atrea.Utilities;
using FuxExMachinaLibrary.Compose;
using FuxExMachinaLibrary.Evaluators;

namespace FuxExMachinaLibrary.Decorators
{
    /// <summary>
    /// Abstract ICompositionDecorator which other ICompositionDecorator should inherit from.
    /// </summary>
    public abstract class BaseCompositionDecorator : ICompositionDecorator
    {
        /// <summary>
        /// Scale degree helper used to transform notes into scale degrees.
        /// </summary>
        protected readonly ScaleDegreeEvaluator ScaleDegreeEvaluator;

        /// <summary>
        /// The random number generator, used to generate rolls for possible ornamentation.
        /// </summary>
        protected readonly CryptoRandom Random;

        /// <summary>
        /// A representation of an empty composition decoration.
        /// </summary>
        protected readonly List<int> EmptyDecoration = new List<int>();

        /// <summary>
        /// BaseCompositionDecorator constructor.
        /// </summary>
        /// <param name="scaleDegreeEvaluator">The ScaleDegreeEvaluator to use</param>
        /// <param name="random">The CryptoRandom to use</param>
        protected BaseCompositionDecorator(ScaleDegreeEvaluator scaleDegreeEvaluator, CryptoRandom random)
        {
            ScaleDegreeEvaluator = scaleDegreeEvaluator;
            Random = random;
        }

        /// <inheritdoc />
        /// <summary>
        /// Decorates a given composition with musical ornamentation.
        /// </summary>
        /// <param name="composition">The composition to decorate</param>
        public void DecorateComposition(Composition composition)
        {
            var notePairs = composition.GetNotePairs();

            for (int i = 0, j = 1; j < notePairs.Count; i++, j++)
            {
                var currentNotePair = notePairs[i];
                var nextNotePair = notePairs[j];

                if (currentNotePair.CounterPointDecorations.None())
                {
                    currentNotePair.CounterPointDecorations.AddRange(
                        GetPossibleDecorations(
                            currentNotePair.CounterPointNote,
                            nextNotePair.CounterPointNote
                        )
                    );
                }

                if (currentNotePair.CantusFirmusDecorations.None())
                {
                    currentNotePair.CantusFirmusDecorations.AddRange(
                        GetPossibleDecorations(
                            currentNotePair.CantusFirmusNote,
                            nextNotePair.CantusFirmusNote
                        )
                    );
                }
            }
        }

        /// <summary>
        /// Decorates the composition based on the current and next notes.
        /// </summary>
        /// <param name="currentNote">The current note</param>
        /// <param name="nextNote">The next note</param>
        /// <returns>A list of decorative notes, possibly empty</returns>
        protected abstract List<int> GetPossibleDecorations(int currentNote, int nextNote);
    }
}