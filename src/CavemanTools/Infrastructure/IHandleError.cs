namespace CavemanTools.Infrastructure
{
    public interface IHandleError<T> where T:AbstractErrorMessage
    {
        void Handle(T cmd);
    }
}