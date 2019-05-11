using System.Collections.Generic;
using System.Linq;
using FuxExMachinaLibrary.Enums;

namespace FuxExMachinaLibrary.Compose.Notes
{
    /// <summary>
    /// A class which represents all of the possible note choices that can be made
    /// within the musical pieces generated.
    /// </summary>
    public class NoteChoiceCollection
    {
        /// <summary>
        /// The list of possible note choices.
        /// </summary>
        private readonly List<NoteChoice> _noteChoices = new List<NoteChoice>();

        /// <summary>
        /// Retrieves all of the possible note choices that can be made.
        /// </summary>
        /// <returns>A list of unique NoteChoices</returns>
        public IList<NoteChoice> GetNoteChoices()
        {
            // If we have already generated our note choices, return them.
            if (_noteChoices.Any())
            {
                return _noteChoices;
            }

            Initialize();

            return _noteChoices;
        }

        /// <summary>
        /// Private method which initializes all of the note choices in this collection. Builds up a
        /// unique list of NoteChoices using functional LINQ methods, ultimately creating a cartesian
        /// product of all possible valid NoteChoice configurations.
        /// </summary>
        private void Initialize()
        {
            _noteChoices.Clear();

            const int maxScaleDegreeChange = 3;
            const int pointScaleDegreeChange = 0;

            var counterPointScaleDegreeChanges = Enumerable.Range(1, maxScaleDegreeChange).ToList();
            var cantusFirmusScaleDegreeChanges = Enumerable.Range(1, maxScaleDegreeChange).ToList();

            NoteMotion[] counterPointNoteMotions = {NoteMotion.Ascending, NoteMotion.Descending};
            NoteMotion[] cantusFirmusNoteMotions = {NoteMotion.Ascending, NoteMotion.Descending};

            _noteChoices.AddRange(
                counterPointScaleDegreeChanges.SelectMany(
                    counterPointScaleDegreeChange => cantusFirmusScaleDegreeChanges.SelectMany(
                        cantusFirmusScaleDegreeChange => counterPointNoteMotions.SelectMany(
                            counterPointNoteMotion => cantusFirmusNoteMotions.Select(
                                cantusFirmusNoteMotion => new NoteChoice(
                                    counterPointNoteMotion,
                                    cantusFirmusNoteMotion,
                                    counterPointScaleDegreeChange,
                                    cantusFirmusScaleDegreeChange
                                )
                            )
                        )
                    )
                ).Concat(
                    counterPointScaleDegreeChanges.SelectMany(
                        counterPointScaleDegreeChange => counterPointNoteMotions.Select(
                            counterPointNoteMotion => new NoteChoice(
                                counterPointNoteMotion,
                                NoteMotion.Oblique,
                                counterPointScaleDegreeChange
                            )
                        )
                    )
                ).Concat(
                    cantusFirmusScaleDegreeChanges.SelectMany(
                        cantusFirmusScaleDegreeChange => cantusFirmusNoteMotions.Select(
                            cantusFirmusNoteMotion => new NoteChoice(
                                NoteMotion.Oblique,
                                cantusFirmusNoteMotion,
                                pointScaleDegreeChange,
                                cantusFirmusScaleDegreeChange
                            )
                        )
                    )
                ).ToList()
            );
        }
    }
}