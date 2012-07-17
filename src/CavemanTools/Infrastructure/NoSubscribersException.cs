using System;

namespace CavemanTools.Infrastructure
{
    public class NoSubscribersException:Exception
    {
        public NoSubscribersException(AbstractErrorMessage msg)
        {
            ErrorMessage = msg;
        }

        public AbstractErrorMessage ErrorMessage { get; set; }
    }
}