namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface IExecuteRequest<T,R> where T:ICommand
    {
        R Execute(T cmd);             
    }    
}