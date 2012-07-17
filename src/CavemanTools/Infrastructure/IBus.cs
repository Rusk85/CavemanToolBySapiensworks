using System.Threading.Tasks;

namespace CavemanTools.Infrastructure
{
    public interface IBus:ISetupServiceBus,IDispatchMessages
    {
        /// <summary>
        /// Creates a new service bus containing all handlers and subscriptions.
        /// Best used in a multi threading app (web app)
        /// </summary>
        /// <returns></returns>
        IBus SpawnLocal();
        //todo autosetup handlers
    }
}