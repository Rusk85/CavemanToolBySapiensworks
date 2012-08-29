namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface IExecuteCommand<T> where T : ICommand
    {
        void Execute(T cmd);
    }
}