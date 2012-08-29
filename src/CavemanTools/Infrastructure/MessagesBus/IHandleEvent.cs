namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface IHandleEvent<T> where T:IEvent
    {
        void Handle(T evnt);
    }
}