using System;
using System.Linq;
using System.Web.Mvc;
using CavemanTools.Web.Authentication;

namespace CavemanTools.Mvc.Authentication
{
    /// <summary>
    /// For Caveman Authentication
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,AllowMultiple = true)]
    public class RequiresPermissionAttribute:FilterAttribute,IAuthorizationFilter
    {
        private ushort[] _rights;

       
        /// <summary>
        /// The authorization succeeds if user has any of the specified rights
        /// </summary>
        /// <param name="rights"></param>
        public RequiresPermissionAttribute(params ushort[] rights)
        {
            if (rights.Length==0) throw new InvalidOperationException("At least a right needs to be specified!");
            _rights = rights;
        }

      public void OnAuthorization(AuthorizationContext filterContext)
      {
          var dt = filterContext.HttpContext.User.GetCavemanIdentity();
          
                    
            if (!_rights.Any(dt.HasRightTo))
            {
                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
                else
                {
                    filterContext.Result = new HttpStatusCodeResult(403, "You are not authorized to view this page");    
                }
                
            }
           
        }
    }
}