using System.Threading.Tasks;

namespace CavemanTools.Infrastructure
{
    public interface IHandleActionAsync<TInput, TOutput> where TInput : class where TOutput : class
    {
        Task<TOutput> Handle(TInput input);
    }
}