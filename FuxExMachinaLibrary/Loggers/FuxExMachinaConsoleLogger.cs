using System;

namespace FuxExMachinaLibrary.Loggers
{
    /// <inheritdoc />
    /// <summary>
    /// An implementation of IFuxExMachinaLogger which logs to the console.
    /// </summary>
    public class FuxExMachinaConsoleLogger : IFuxExMachinaLogger
    {
        /// <inheritdoc />
        /// <summary>
        /// Logs the log string out to the console.
        /// </summary>
        /// <param name="logString">The log string to log</param>
        public void Log(string logString) => Console.WriteLine(logString);
    }
}