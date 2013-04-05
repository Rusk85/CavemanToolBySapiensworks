using System.Web.Routing;

namespace CavemanTools.Mvc.Routing
{
    public interface IGlobalRoutePolicy
    {
        void ApplyTo(Route route);
    }
}