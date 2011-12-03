using System;
using System.Web.Mvc;
using CavemanTools.Web.Security;

namespace CavemanTools.Mvc.Security
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class DemandRightAttribute:AuthorizeAttribute
    {
       
        public byte Right { get; set; }

      public override void OnAuthorization(AuthorizationContext filterContext)
        {
           if (!AuthorizeCore(filterContext.HttpContext) || Right==UserBasicRights.None) return;
           var dt = filterContext.HttpContext.GetUserContext();
           if (dt == null)
           {
               HandleUnauthorizedRequest(filterContext);
           }
           else
           {
               if (!dt.HasRightTo(Right))
               {
                   filterContext.Result = new HttpStatusCodeResult(403, "You have no rights for request");
               }
           }
            
        }
    }
}