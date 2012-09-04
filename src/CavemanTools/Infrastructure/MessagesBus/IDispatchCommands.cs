using System;
using System.Threading.Tasks;

namespace CavemanTools.Infrastructure.MessagesBus
{
    public interface IDispatchCommands
    {
        void Send(ICommand cmd);
        void Send<T>(Action<T> constructor) where T:ICommand,new();
        Task SendAsync(ICommand cmd);
        Task SendAsync<T>(Action<T> constructor) where T : ICommand,new();
    }
}