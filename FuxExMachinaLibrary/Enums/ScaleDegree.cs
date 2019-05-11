using System;

namespace FuxExMachinaLibrary.Enums
{
    /// <summary>
    /// Enum which represents the possible scale degrees in a piece of music.
    /// </summary>
    public enum ScaleDegree
    {
        Root,
        Second,
        Third,
        Fourth,
        Fifth,
        Sixth,
        Seventh
    }

    /// <summary>
    /// Class to provide extension methods for the ScaleDegree enum.
    /// </summary>
    internal static class ScaleDegrees
    {
        /// <summary>
        /// Is the given ScaleDegree dissonant with this ScaleDegree? 
        /// </summary>
        /// <param name="left">The left ScaleDegree for comparison</param>
        /// <param name="right">The right ScaleDegree for comparison</param>
        /// <returns>Whether the scale degrees are dissonant or not</returns>
        public static bool IsDissonantWith(this ScaleDegree left, ScaleDegree right)
        {
            return left.IsAdjacentTo(right) || left.IsTritoneWith(right) || left.IsSeventhWith(right);
        }

        /// <summary>
        /// Is the given ScaleDegree adjacent to this ScaleDegree? 
        /// </summary>
        /// <param name="left">The left ScaleDegree for comparison</param>
        /// <param name="right">The right ScaleDegree for comparison</param>
        /// <returns>Whether the scale degrees are adjacent or not</returns>
        public static bool IsAdjacentTo(this ScaleDegree left, ScaleDegree right)
        {
            return Math.Abs((int) left - (int) right) == 1;
        }

        /// <summary>
        /// Is the given ScaleDegree a third apart from this ScaleDegree?
        /// </summary>
        /// <param name="left">The left ScaleDegree for comparison</param>
        /// <param name="right">The right ScaleDegree for comparison</param>
        /// <returns>Whether the scale degrees are a third apart or not</returns>
        public static bool IsThirdWith(this ScaleDegree left, ScaleDegree right)
        {
            return Math.Abs((int) left - (int) right) == 2;
        }

        /// <summary>
        /// Is the given ScaleDegree a tritone with this ScaleDegree?
        /// See <a href="https://en.wikipedia.org/wiki/Tritone">this link</a> for more information.
        /// </summary>
        /// <param name="left">The left ScaleDegree for comparison</param>
        /// <param name="right">The right ScaleDegree for comparison</param>
        /// <returns>Whether the scale degrees create a tritone or not</returns>
        public static bool IsTritoneWith(this ScaleDegree left, ScaleDegree right)
        {
            if (left == ScaleDegree.Seventh && right == ScaleDegree.Fourth)
            {
                return true;
            }

            return left == ScaleDegree.Fourth && right == ScaleDegree.Seventh;
        }

        /// <summary>
        /// Is the given ScaleDegree a seventh with this ScaleDegree? 
        /// </summary>
        /// <param name="left">The left ScaleDegree for comparison</param>
        /// <param name="right">The right ScaleDegree for comparison</param>
        /// <returns>Whether the scale degrees are a seventh apart or not</returns>
        public static bool IsSeventhWith(this ScaleDegree left, ScaleDegree right)
        {
            if (left == ScaleDegree.Root && right == ScaleDegree.Seventh)
            {
                return true;
            }

            return left == ScaleDegree.Seventh && right == ScaleDegree.Root;
        }

        /// <summary>
        /// Is the given ScaleDegree perfect with this ScaleDegree?
        /// See <a href="https://en.wikipedia.org/wiki/Perfect_fourth">this link</a> for more information.
        /// See <a href="https://en.wikipedia.org/wiki/Perfect_fifth">this link</a> for more information.
        /// </summary>
        /// <param name="left">The left ScaleDegree for comparison</param>
        /// <param name="right">The right ScaleDegree for comparison</param>
        /// <returns>Whether the scale degrees are perfect or not</returns>
        public static bool IsPerfectWith(this ScaleDegree left, ScaleDegree right)
        {
            if (left == ScaleDegree.Root && right == ScaleDegree.Fifth)
            {
                return true;
            }

            if (left == ScaleDegree.Fifth && right == ScaleDegree.Root)
            {
                return true;
            }

            if (left == ScaleDegree.Second && right == ScaleDegree.Sixth)
            {
                return true;
            }

            if (left == ScaleDegree.Sixth && right == ScaleDegree.Second)
            {
                return true;
            }

            if (left == ScaleDegree.Third && right == ScaleDegree.Seventh)
            {
                return true;
            }

            if (left == ScaleDegree.Seventh && right == ScaleDegree.Third)
            {
                return true;
            }

            if (left == ScaleDegree.Fourth && right == ScaleDegree.Root)
            {
                return true;
            }

            return left == ScaleDegree.Root && right == ScaleDegree.Fourth;
        }
    }
}