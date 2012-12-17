using System;

namespace CavemanTools.Infrastructure.MessagesBus.Internals
{
   interface IInvokeMessageHandler
   {
       void Execute(dynamic msg);
       dynamic ExecuteRequest(dynamic msg);
       void Publish(dynamic evnt);
       IDisposableHandlerInstance GetInstance();
       Type HandlerType { get; }
   }
}