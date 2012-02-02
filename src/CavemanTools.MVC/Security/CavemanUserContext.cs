using System.Web;
using System.Web.Mvc;
using CavemanTools.Web.Security;

namespace CavemanTools.Mvc.Security
{
    /// <summary>
    /// Asp.net mvc 3 global filter which creates UserContext
    /// </summary>
    public sealed class CavemanUserContext:IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            UserRightModule.CreateUserRightContext(HttpContext.Current, DependencyResolver.Current.GetService<IUserRightsRepository>());
        }
    }
}