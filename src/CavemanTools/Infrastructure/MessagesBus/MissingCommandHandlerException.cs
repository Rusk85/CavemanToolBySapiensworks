using System;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public class MissingCommandHandlerException:Exception
    {
        public MissingCommandHandlerException(string msg):base(msg)
        {
            
        }
        public MissingCommandHandlerException(AbstractErrorMessage msg)
        {
            ErrorMessage = msg;
        }

        public AbstractErrorMessage ErrorMessage { get; set; }
    }
}