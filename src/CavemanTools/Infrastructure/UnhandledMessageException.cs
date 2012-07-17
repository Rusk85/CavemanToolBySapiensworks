using System;

namespace CavemanTools.Infrastructure
{
    public class UnhandledMessageException:AbstractErrorMessage
    {
        public UnhandledMessageException(Exception ex)
        {
            UnhandledException = ex;
        }

        public Exception UnhandledException { get; set; }
    }
}