using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Linq;


namespace CavemanTools.Web.Security
{
    public static class AuthenticationUtils
    {
        ///// <summary>
        ///// Register implementation of IUserId interface to use with authentication ticket
        ///// </summary>
        ///// <param name="t">Type of class implementing IUserId</param>
        //public static void RegisterUserIdType(Type t)
        //{
        //    var data = t.GetFullTypeName();
        //        //string.Format("{0}, {1}", t.FullName, Assembly.GetAssembly(t));
        //    if (!_types.Contains(data))
        //    {
        //        _types.Add(data);
        //    }
        //}

        //static List<string> _types = new List<string>(2) { "CavemanTools.Web.Security.UserId, CavemanTools", "CavemanTools.Web.Security.UserGuid, CavemanTools" };

        public static AuthenticationTicketData Unpack(string ticket)
         {
             if (ticket == null) throw new ArgumentNullException("ticket");
             AuthenticationTicketData res=null;
             if (!String.IsNullOrEmpty(ticket))
             {
                 var items = ticket.Split(';');
                 try
                 {
                     res = new AuthenticationTicketData();
                     res.UserId = UnpackUserId(items[0]);
                     res.Groups = items[1].Split(',').Select(d => d.Parse<int>()).ToArray();
                     // res.GroupId = items[1].ConvertTo<int>();
                 }
                 catch(Exception)
                 {
                     //nothing, invalid ticket
                     res = null;
                 }
             }
             return res;

         }

        public static string Pack(this AuthenticationTicketData data)
        {
            if (data == null) throw new ArgumentNullException("data");
            return String.Format("{0};{1}",PackUserId(data.UserId),string.Join(",",data.Groups));
        }

        static string PackUserId(IUserIdValue uid)
        {
            if (uid == null) throw new ArgumentNullException("uid");
            var tpn = uid.GetType().GetFullTypeName();
            return String.Format("{0}|{1}",tpn,uid.ToString());
        }

        static IUserIdValue UnpackUserId(string data)
        {
            if (String.IsNullOrEmpty(data)) return null;
            var items = data.Split('|');
            if (items.Length==2)
            {
                var typeName = items[0];
                var uid = (IUserIdValue) Activator.CreateInstance(Type.GetType(typeName, true), items[1]);
                return uid;               
            }
            return null;
        }


        public static HttpCookie CreateCookie(IUserIdValue userId, string name, IEnumerable<int> group, bool isPersistent = false)
        {
            var user = new AuthenticationTicketData() { Groups = @group, UserId = userId };
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

        /// <summary>
        /// Creates and attaches to response the authentication cookie
        /// </summary>
        /// <param name="response"></param>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="group"></param>
        /// <param name="isPersistent"></param>
        /// <returns></returns>
        public static HttpCookie SetAuthCookie(this HttpResponse response,IUserIdValue userId, string name, IEnumerable<int> group, bool isPersistent = false)
        {
            var ck = CreateCookie(userId, name, group, isPersistent);
            response.AppendCookie(ck);
            return ck;
        }

        /// <summary>
        /// Returns the user context created by the UserRights module
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IUserRightsContext GetUserContext(this HttpContext context)
        {
            return context.Items[UserRightsModule.ContextKey] as IUserRightsContext;
        }

        public static void SetAuthScope(AuthorizationScopeId scopeId)
        {
            if (scopeId == null) throw new ArgumentNullException("scopeId");
            var ctx = HttpContext.Current;
            var usr = ctx.GetUserContext();
            if (usr!=null) usr.ScopeId = scopeId;
            else
            {
                ctx.Items[UserRightsModule.ContextScopeIdKey] = scopeId;    
            }
        }
    }

    

    public class AuthenticationTicketData
    {
        public IUserIdValue UserId { get; set; }
        public IEnumerable<int> Groups { get; set; }
    }


}