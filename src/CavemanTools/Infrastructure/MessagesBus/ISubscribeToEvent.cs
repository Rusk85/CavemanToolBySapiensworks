namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface ISubscribeToEvent<T> where T:IEvent
    {
        void Handle(T evnt);
    }
}