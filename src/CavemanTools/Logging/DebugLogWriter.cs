using System;
using System.Text;

namespace CavemanTools.Logging
{
	/// <summary>
	///  Used to write debug messages to VS Output window.
	/// </summary>
	public class DebugLogWriter : System.IO.TextWriter	, ILogWriter
	{
		public DebugLogWriter()
		{
			
		}

		private string cat="Caveman Debug";
		public DebugLogWriter(string category)
		{
			cat = category;
		}

		public override void Write(char[] buffer, int index, int count)
		{
			System.Diagnostics.Debug.WriteLine("==============================");
			System.Diagnostics.Debug.Write(new String(buffer, index, count),cat);
		}

		public override void Write(string value)
		{
			System.Diagnostics.Debug.Write(value);
		}

		public override Encoding Encoding
		{
			get { return System.Text.Encoding.Default; }
		}

		#region Old
		//public void WriteInfo(string text)
		//{
		//    Write("Info: "+text);
		//}

		//public void WriteError(string text)
		//{
		//    Write("Error: " + text);
		//}

		//public void WriteWarning(string text)
		//{
		//    Write("Warning: " + text);
		//}

		//public void WriteDebug(string mesage)
		//{
		//    Write("Debug: "+mesage);
		//}

		//public void WriteDebug(string message, params object[] args)
		//{
		//    Write(string.Format("Debug: "+message),args);
		//} 
		#endregion

		public void RegisterEntry(LogLevel level, string text)
		{
			RegisterEntry(level, text, null);
		}

		public void RegisterEntry(LogLevel level, string message, params object[] args)
		{
			message = args == null ? message : string.Format(message, args);
			Write(string.Format("{0}: {1}",level,message));
		}
	}
}