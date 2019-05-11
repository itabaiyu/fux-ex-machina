using System.Linq;
using Atrea.Extensions;
using FuxExMachinaLibrary.Compose;
using FuxExMachinaLibrary.Enums;
using FuxExMachinaLibrary.Factories;
using FuxExMachinaTests.Utility;
using NUnit.Framework;

namespace FuxExMachinaTests
{
    [TestFixture()]
    public class CompositionContextCollectionTests
    {
        private FuxExMachinaFactory _factory;
        private CompositionContextCollection _compositionContexts;

        [SetUp]
        public void Setup()
        {
            _factory = FuxExMachinaTestFactoryProvider.GetTestFactory();
            _compositionContexts = _factory.CompositionContexts;
        }

        [Test]
        public void OnlyGeneratesUniqueContexts()
        {
            var contexts = _factory.CompositionContexts.GetCompositionContexts();

            var uniqueContexts = contexts
                .DistinctBy(context => new
                {
                    context.CantusFirmusNoteMotion,
                    context.CantusFirmusNoteMotionSpan,
                    context.CantusFirmusNoteScaleDegree,
                    context.CounterPointNoteMotion,
                    context.CounterPointNoteMotionSpan,
                    context.CounterPointNoteScaleDegree
                }).ToList();

            Assert.AreEqual(contexts.Count, uniqueContexts.Count);
        }

        public void RetrievesSpecificContext()
        {
            var queryContext = _factory.CreateCompositionContext(
                NoteMotion.Ascending,
                NoteMotionSpan.Leap,
                ScaleDegree.Fifth,
                NoteMotion.Descending,
                NoteMotionSpan.Leap,
                ScaleDegree.Fourth
            );

            var resultContext = _compositionContexts.GetSpecificContext(queryContext);

            Assert.True(resultContext.Equals(queryContext));
        }
    }
}