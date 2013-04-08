using System.Web.Routing;

namespace CavemanTools.Mvc.Routing
{
    public interface IRouteGlobalPolicy
    {
        void ApplyTo(Route route);
    }
}