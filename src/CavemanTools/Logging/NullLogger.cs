using System;

namespace CavemanTools.Logging
{
    public class NullLogger : LogWriterBase
    {
        public static IWriteToLog Instance { get { return _inst; }}
        static readonly NullLogger _inst=new NullLogger();
        private NullLogger()
        {
            
        }

        public override T GetLogger<T>()
        {
            throw new NotImplementedException();
        }

        public override void Log(LogLevel level, string text)
        {

        }

        public override void Log(LogLevel level, string message, params object[] args)
        {

        }
    }
}