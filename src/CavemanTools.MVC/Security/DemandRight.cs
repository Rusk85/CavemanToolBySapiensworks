using System;
using System.Web.Mvc;

namespace CavemanTools.Mvc.Security
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class DemandRightAttribute:AuthorizeAttribute
    {
        /// <summary>
        /// 0 is empty
        /// </summary>
        public byte Right { get; set; }

      public override void OnAuthorization(AuthorizationContext filterContext)
        {
           if (!AuthorizeCore(filterContext.HttpContext) || Right==0) return;
           var dt = filterContext.HttpContext.GetUserContext();
           if (dt == null)
           {
               HandleUnauthorizedRequest(filterContext);
           }
           else
           {
               if (!dt.HasRight(Right))
               {
                   filterContext.Result = new HttpStatusCodeResult(403, "You have no rights for this");
               }
           }
            
        }
    }
}