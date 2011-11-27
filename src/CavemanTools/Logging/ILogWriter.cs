namespace CavemanTools.Logging
{
	/// <summary>
	/// Used by LoggingUtility extension methods.
	/// </summary>
	public interface ILogWriter
	{
		//void WriteInfo(string text);
		//void WriteInfo(string message, params object[] args);
		/// <summary>
		/// Writes a log entry with the specified logging level
		/// </summary>
		/// <param name="level">Status</param>
		/// <param name="text">Entry Text</param>
		void RegisterEntry(LogLevel level,string text);

		/// <summary>
		/// Writes a formatted log entry with the specified logging level
		/// </summary>
		/// <param name="level">Status</param>
		/// <param name="message">Entry Text</param>
		/// <param name="args">List of arguments</param>
		void RegisterEntry(LogLevel level,string message, params object[] args);

		//void WriteError(string text);
		//void WriteError(string message, params object[] args);
	
		//void WriteWarning(string text);
		//void WriteWarning(string message, params object[] args);

		//void WriteDebug(string mesage);
		//void WriteDebug(string message, params object[] args);
	}
}