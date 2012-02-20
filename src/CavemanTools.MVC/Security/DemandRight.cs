using System;
using System.Web.Mvc;
using CavemanTools.Web.Security;

namespace CavemanTools.Mvc.Security
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class DemandRightAttribute:AuthorizeAttribute
    {
       
        /// <summary>
        /// Gets or sets the right needed by the user
        /// </summary>
        public ushort Permission { get; set; }

      public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var dt = filterContext.HttpContext.GetUserContext();
            if (dt == null)
            {
                throw new InvalidOperationException("UserContext doesn't exist. You need to register the 'CavemanUserContext' global filter or the 'UserRights' http module");
             //   HandleUnauthorizedRequest(filterContext);
            } 

          if (Permission==UserBasicRights.None)
           {
               return;
           }
                     
            if (!dt.HasRightTo(Permission))
            {
                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    HandleUnauthorizedRequest(filterContext);
                }
                else
                {
                    filterContext.Result = new HttpStatusCodeResult(403, "You are not authorized to view this page");    
                }
                
            }
           
        }
    }
}