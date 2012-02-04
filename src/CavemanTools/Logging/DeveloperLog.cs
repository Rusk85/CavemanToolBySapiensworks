namespace CavemanTools.Logging
{
    public class DeveloperLog:LogWriterBase
    {
        public override T GetLogger<T>()
        {
            throw new System.NotImplementedException();
        }

        public override void Log(LogLevel level, string text)
        {
            System.Diagnostics.Debug.WriteLine(text,level.ToString());
        }

        public override void Log(LogLevel level, string message, params object[] args)
        {
            System.Diagnostics.Debug.WriteLine(string.Format(message,args), level.ToString());
        }
    }
}