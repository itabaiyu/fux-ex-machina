using System;
using System.Collections.Generic;
using System.Linq;
using FuxExMachinaLibrary.Enums;

namespace FuxExMachinaLibrary.Compose
{
    /// <summary>
    /// A class which contains the cartesian product of all possible CompositionContext configurations
    /// that can occur within a given composition.
    /// </summary>
    public class CompositionContextCollection
    {
        /// <summary>
        /// The list of unique composition contexts.
        /// </summary>
        private static readonly List<CompositionContext> Contexts = new List<CompositionContext>();

        /// <summary>
        /// Retrieves the composition contexts.
        /// </summary>
        /// <returns>A list of unique composition contexts</returns>
        public IList<CompositionContext> GetCompositionContexts()
        {
            if (Contexts.Any())
            {
                return Contexts;
            }

            Initialize();

            return Contexts;
        }

        /// <summary>
        /// Retrieves a specific context instance from the collection using a query context. This is
        /// used within the CompositionStrategy in order to key into it's composition strategy dictionary.
        /// </summary>
        /// <param name="queryContext">The query context to find a context instance for</param>
        /// <returns>An existing CompositionContext that matches the query context</returns>
        public CompositionContext GetSpecificContext(CompositionContext queryContext)
        {
            if (!Contexts.Any())
            {
                Initialize();
            }

            return Contexts.FirstOrDefault(context => context.Equals(queryContext));
        }

        /// <summary>
        /// Initializes all possible CompositionContexts configurations and adds them to the Contexts list.
        /// </summary>
        private static void Initialize()
        {
            var noteMotions = (NoteMotion[]) Enum.GetValues(typeof(NoteMotion));
            var noteMotionSpans = (NoteMotionSpan[]) Enum.GetValues(typeof(NoteMotionSpan));
            var scaleDegrees = (ScaleDegree[]) Enum.GetValues(typeof(ScaleDegree));

            Contexts.AddRange(
                noteMotions.SelectMany(
                    counterPointNoteMotion => noteMotionSpans.SelectMany(
                        counterPointNoteMotionSpan => scaleDegrees.SelectMany(
                            counterPointNoteScaleDegree => noteMotions.SelectMany(
                                cantusFirmusNoteMotion => noteMotionSpans.SelectMany(
                                    cantusFirmusNoteMotionSpan => scaleDegrees.Select(
                                        cantusFirmusNoteScaleDegree => new CompositionContext(
                                            counterPointNoteMotion,
                                            counterPointNoteMotionSpan,
                                            counterPointNoteScaleDegree,
                                            cantusFirmusNoteMotion,
                                            cantusFirmusNoteMotionSpan,
                                            cantusFirmusNoteScaleDegree
                                        )
                                    )
                                )
                            )
                        )
                    )
                ).ToList()
            );
        }
    }
}