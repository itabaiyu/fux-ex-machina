using FuxExMachinaLibrary.Enums;
using FuxExMachinaLibrary.Factories;
using FuxExMachinaTests.Utility;
using NUnit.Framework;

namespace FuxExMachinaTests
{
    [TestFixture]
    public class CompositionContextTests
    {
        private FuxExMachinaFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = FuxExMachinaTestFactoryProvider.GetTestFactory();
        }

        [Test]
        public void ContextsAreEqual()
        {
            var leftCompositionContext = _factory.CreateCompositionContext(
                NoteMotion.Ascending,
                NoteMotionSpan.Leap,
                ScaleDegree.Fifth,
                NoteMotion.Descending,
                NoteMotionSpan.Leap,
                ScaleDegree.Fourth
            );

            var rightCompositionContext = _factory.CreateCompositionContext(
                NoteMotion.Ascending,
                NoteMotionSpan.Leap,
                ScaleDegree.Fifth,
                NoteMotion.Descending,
                NoteMotionSpan.Leap,
                ScaleDegree.Fourth
            );

            Assert.True(leftCompositionContext.Equals(rightCompositionContext));
        }

        [Test]
        public void ContextsAreNotEqual()
        {
            var leftCompositionContext = _factory.CreateCompositionContext(
                NoteMotion.Ascending,
                NoteMotionSpan.Leap,
                ScaleDegree.Fifth,
                NoteMotion.Descending,
                NoteMotionSpan.Leap,
                ScaleDegree.Fourth
            );

            var rightCompositionContext = _factory.CreateCompositionContext(
                NoteMotion.Ascending,
                NoteMotionSpan.Leap,
                ScaleDegree.Fifth,
                NoteMotion.Ascending,
                NoteMotionSpan.Leap,
                ScaleDegree.Fourth
            );

            Assert.False(leftCompositionContext.Equals(rightCompositionContext));
        }

        [Test]
        [TestCase(5, 6, NoteMotion.Ascending)]
        [TestCase(6, 3, NoteMotion.Descending)]
        [TestCase(6, 5, NoteMotion.Descending)]
        public void GetNoteMotionFromNotes(int previousNote, int currentNote, NoteMotion expectedNoteMotion)
        {
            var context = _factory.CreateCompositionContext();
            var noteMotion = context.GetNoteMotionFromNotes(previousNote, currentNote);

            Assert.AreEqual(expectedNoteMotion, noteMotion);
        }

        [Test]
        [TestCase(5, 6, NoteMotionSpan.Step)]
        [TestCase(6, 6, NoteMotionSpan.Step)]
        [TestCase(6, 5, NoteMotionSpan.Step)]
        [TestCase(600, 500, NoteMotionSpan.Leap)]
        [TestCase(500, 600, NoteMotionSpan.Leap)]
        public void GetNoteMotionSpanFromNotes(int previousNote, int currentNote, NoteMotionSpan expectedNoteMotionSpan)
        {
            var context = _factory.CreateCompositionContext();
            var noteMotionSpan = context.GetNoteMotionSpanFromNotes(previousNote, currentNote);

            Assert.AreEqual(expectedNoteMotionSpan, noteMotionSpan);
        }
    }
}