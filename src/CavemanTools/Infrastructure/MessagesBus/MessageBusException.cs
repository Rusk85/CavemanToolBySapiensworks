using System;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public class MessageBusException : Exception
    {
        public MessageBusException(Exception exception):base("See inner exception for details",exception)
        {
            
        }
    }
}