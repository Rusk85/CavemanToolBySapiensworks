namespace CavemanTools.Infrastructure
{
    public interface IHandleEvent<T> where T:IEvent
    {
        void Handle(T evnt);
    }
}