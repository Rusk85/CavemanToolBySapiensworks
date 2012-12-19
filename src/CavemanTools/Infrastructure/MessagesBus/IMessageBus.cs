using System;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface IMessageBus:IConfigureMessageBus,IDispatchMessages
    {
        
    }
}