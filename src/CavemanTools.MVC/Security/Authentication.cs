using System;
using System.Web;
using System.Web.Security;
using CavemanTools.Security;

namespace CavemanTools.Mvc.Security
{
    public static class Authentication
    {
        public static HttpCookie CreateCookie(int userId,string name,int group,bool isPersistent=false)
        {
            var user = new AuthenticationTicketData() {GroupId = group, Id = userId};
            var ft = new FormsAuthenticationTicket(2, name, DateTime.Now, DateTime.Now.Add(FormsAuthentication.Timeout),
                                                   isPersistent, user.Pack());
            var ck = new HttpCookie(FormsAuthentication.FormsCookieName)
                         {
                             Value = FormsAuthentication.Encrypt(ft),
                             Path = FormsAuthentication.FormsCookiePath,
                             Domain = FormsAuthentication.CookieDomain
                         };
            if (isPersistent)
            {
                ck.Expires = DateTime.Now.Add(FormsAuthentication.Timeout);
            }
            return ck;
        }
        public static void SetAuthCookie(this HttpResponseBase response, int userId, string name, int group, bool isPersistent = false)
        {
            var ck = CreateCookie(userId, name, group, isPersistent);
            response.AppendCookie(ck);
        }

        public static IUserRightsContext GetUserContext(this HttpContextBase context)
        {
            return context.Items["_usr-context_"] as IUserRightsContext;
        }
    }
}