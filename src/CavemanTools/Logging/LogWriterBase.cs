using System;

namespace CavemanTools.Logging
{
    public abstract class LogWriterBase:ILogWriter,IWriteToLog
    {
        public abstract T GetLogger<T>() where T:class ;
        

        public void Info(string text)
        {
            Log(LogLevel.Info,text);
        }

        public void Info(string format, params object[] args)
        {
            Log(LogLevel.Info, format,args);
        }

        public void Log(LogLevel level, string text)
        {
            Log("none",level,text);
        }

        public void Log(LogLevel level, string message, params object[] args)
        {
            Log("none", level, message,args);
        }


        public void Trace(string message)
        {
            Log(LogLevel.Trace, message);
        }

        public void Trace(string format, params object[] args)
        {
            Log(LogLevel.Trace, format, args);
        }

        public void Error(string text)
        {
            Log(LogLevel.Error, text);
        }

        public void Error(string format, params object[] args)
        {
            Log(LogLevel.Error, format, args);
        }

        public void Warning(string text)
        {
            Log(LogLevel.Warn, text);
        }

        public void Warning(string format, params object[] args)
        {
            Log(LogLevel.Warn, format, args);
        }

        public void Debug(string mesage)
        {
            Log(LogLevel.Debug, mesage); 
        }

        public void Debug(string format, params object[] args)
        {
            Log(LogLevel.Debug, format, args);
        }

        protected virtual string FormatSource(object src)
        {
            return "[" + src + "]";
        }

        public virtual void LogException(string source,LogLevel level, Exception ex)
        {
            Log(source,level,ex.ToString());
        }

        public virtual void LogException(object source, LogLevel level, Exception ex)
        {
            Log(source,level,ex.ToString());
        }

        public virtual void Log(object source, LogLevel level, string message, params object[] args)
        {
            Log(FormatSource(source),level,message,args);
        }

        public abstract void Log(string source, LogLevel level, string message, params object[] args);

    }
}