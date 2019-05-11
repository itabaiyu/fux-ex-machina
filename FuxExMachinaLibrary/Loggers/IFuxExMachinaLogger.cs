namespace FuxExMachinaLibrary.Loggers
{
    /// <summary>
    /// A class which handles logging to output.
    /// </summary>
    public interface IFuxExMachinaLogger
    {
        /// <summary>
        /// Logs the log string to output.
        /// </summary>
        /// <param name="logString">The log string to log</param>
        void Log(string logString = "");
    }
}