using System;

namespace CavemanTools.Infrastructure.Internals
{
    interface ISubscription
    {
        void Handle(IMessage msg);
        bool CanHandle(Type evnt);
        bool IsExactlyFor(Type evnt);
    }
}