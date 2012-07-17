namespace CavemanTools.Infrastructure.Internals
{
    internal interface IRemoveHandler
    {
        void Unsubscribe(ISubscription s);
    }
}