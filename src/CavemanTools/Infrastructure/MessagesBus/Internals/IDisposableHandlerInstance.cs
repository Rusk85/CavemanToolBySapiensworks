using System;

namespace CavemanTools.Infrastructure.MessagesBus.Internals
{
    interface IDisposableHandlerInstance:IDisposable
    {
        dynamic Object { get; }
    }
}