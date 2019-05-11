namespace FuxExMachinaLibrary.Loggers
{
    /// <inheritdoc />
    /// <summary>
    /// A logger which does nothing. Useful for when wanting to run things in "silent" mode,
    /// while in a unit testing context, for example.
    /// </summary>
    public class FuxExMachinaNullLogger : IFuxExMachinaLogger
    {
        /// <inheritdoc />
        /// <summary>
        /// Does nothing with the log string.
        /// </summary>
        /// <param name="logString">The log string to log</param>
        public void Log(string logString) { }
    }
}