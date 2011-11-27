using System;
using System.Diagnostics;

namespace CavemanTools.Logging
{
	public class LogEventLogWriter:ILogWriter
	{
		private readonly EventLog log;

		public LogEventLogWriter(string src ):this("Application",src)
		{

		}
		public LogEventLogWriter(string logname, string src)
		{
			log = new EventLog();
			log.Log = logname;
			if (string.IsNullOrEmpty(src)) src = "Application";
			log.Source = src;
		}
		
		public void RegisterEntry(LogLevel level, string text)
		{
			RegisterEntry(level,text,null);
		}

		public void RegisterEntry(LogLevel level, string message, params object[] args)
		{
			message = args == null ? message : string.Format(message, args);
			log.WriteEntry(message,Translate(level));
		}

		EventLogEntryType Translate(LogLevel level)
		{
			switch (level)
			{
				case LogLevel.Debug:
				case LogLevel.Info:return EventLogEntryType.Information;
				case LogLevel.Warn:return EventLogEntryType.Warning;
				case LogLevel.Fatal:
				case LogLevel.Error:return EventLogEntryType.Error;
			}

			throw new NotSupportedException();
		}
	}
}
