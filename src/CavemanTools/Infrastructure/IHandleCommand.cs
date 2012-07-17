namespace CavemanTools.Infrastructure
{
    public interface IHandleCommand<T> where T : ICommand
    {
        void Handle(T cmd);
    }
}