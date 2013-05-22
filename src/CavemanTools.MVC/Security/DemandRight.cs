using System;
using System.Linq;
using System.Web.Mvc;
using CavemanTools.Web.Security;

namespace CavemanTools.Mvc.Security
{
    /// <summary>
    /// Forms auth only
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,AllowMultiple = true)]
    public class DemandRightAttribute:FilterAttribute,IAuthorizationFilter
    {
        private ushort[] _rights;

        /// <summary>
        /// Gets or sets the right needed by the user
        /// </summary>        
        [Obsolete("Use constructor",true)]
        public ushort Permission { get; set; }

        /// <summary>
        /// The authorization succeeds if user has any of the specified rights
        /// </summary>
        /// <param name="rights"></param>
        public DemandRightAttribute(params ushort[] rights)
        {
            if (rights.Length==0) throw new InvalidOperationException("At least a right needs to be specified!");
            _rights = rights;
        }

      public void OnAuthorization(AuthorizationContext filterContext)
        {
            var dt = filterContext.HttpContext.GetUserContext();
            if (dt == null)
            {
                throw new InvalidOperationException("UserContext doesn't exist. You need to register the 'CavemanUserContext' global filter or the 'UserRights' http module");
             }           
                    
          //if ( Permission==UserBasicRights.None)
          // {
          //     return;
          // }
                     
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