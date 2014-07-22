using System;

namespace CavemanTools.Logging
{
    public interface IWriteToLog
    {
        /// <summary>
        /// Writes a formatted log entry with the specified logging level
        /// </summary>
        /// <param name="level">Status</param>
        /// <param name="message">Entry Text</param>
        /// <param name="args">List of arguments</param>
        void Log(object source,LogLevel level, string message, params object[] args);
        
        void Log(string source,LogLevel level, string message, params object[] args);
        
        void LogException(string source, LogLevel level, Exception ex);
        void LogException(object source, LogLevel level, Exception ex);
        
    }
}