using FuxExMachinaLibrary.Enums;
using FuxExMachinaLibrary.Evaluators;
using NUnit.Framework;

namespace FuxExMachinaTests.EvaluatorTests
{
    [TestFixture]
    public class ScaleDegreeEvaluatorTests
    {
        [Test]
        [TestCase(1, ScaleDegree.Root)]
        [TestCase(8, ScaleDegree.Root)]
        [TestCase(7, ScaleDegree.Seventh)]
        [TestCase(9, ScaleDegree.Second)]
        [TestCase(10, ScaleDegree.Third)]
        [TestCase(700025, ScaleDegree.Fourth)]
        [TestCase(12, ScaleDegree.Fifth)]
        [TestCase(13, ScaleDegree.Sixth)]
        [TestCase(14, ScaleDegree.Seventh)]
        [TestCase(3, ScaleDegree.Third)]
        [TestCase(3 + 7, ScaleDegree.Third)]
        [TestCase(3 + 14, ScaleDegree.Third)]
        [TestCase(3 + (7 * 1000), ScaleDegree.Third)]
        [TestCase(1 + 21, ScaleDegree.Root)]
        [TestCase(1 + 21, ScaleDegree.Root)]
        [TestCase(7001, ScaleDegree.Root)]
        [TestCase(7015, ScaleDegree.Root)]
        public void GetCorrectScaleDegree(int pitch, ScaleDegree expectedScaleDegree)
        {
            var scaleDegreeEvaluator = new ScaleDegreeEvaluator();
            var scaleDegree = scaleDegreeEvaluator.GetScaleDegreeFromNote(pitch);

            Assert.AreEqual(expectedScaleDegree, scaleDegree);
        }
    }
}