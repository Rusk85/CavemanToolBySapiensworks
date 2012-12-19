using System;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public class UnhandledMessageException:AbstractErrorMessage
    {
        public UnhandledMessageException(Exception ex,IMessage msg)
        {
            UnhandledException = ex;
            Msg = msg;
        }

        public Exception UnhandledException { get; set; }
        public IMessage Msg { get; set; }
    }
}