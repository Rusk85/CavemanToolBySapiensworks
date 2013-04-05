using System.Web.Routing;

namespace CavemanTools.Mvc.Routing
{
    /// <summary>
    /// How to generate routes from actions
    /// </summary>
    public interface IRouteConvention
    {
        bool Match(ActionCall actionCall);
        Route Build(ActionCall actionInfo);
    }
    
}