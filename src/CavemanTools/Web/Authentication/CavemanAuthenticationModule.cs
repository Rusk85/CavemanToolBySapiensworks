using System.Security.Principal;
using System.Web;
using System;
using System.Web.Configuration;
using CavemanTools.Extensions;

namespace CavemanTools.Web.Authentication
{
    public class CavemanAuthenticationModule:IHttpModule
    {
        private readonly IAuthenticationService _service;

        public static string CookieName = "CaveAuth";
        public static string CookiePath = "/";
        public static string CookieDomain = "";
        public static TimeSpan SessionValidity = TimeSpan.FromDays(1);
        public static TimeSpan RememberMeValidity = TimeSpan.FromDays(30);

        private string LoginRedirect = "";
        public const string WebConfigLoginRedirectKey = "Caveman:LoginRedirect";
        public CavemanAuthenticationModule(IAuthenticationService service)
        {
            _service = service;
            LoginRedirect = WebConfigurationManager.AppSettings[WebConfigLoginRedirectKey];
         }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application </param>
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += context_AuthenticateRequest;
           
            context.EndRequest += context_EndRequest;
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            if (LoginRedirect.IsNullOrEmpty()) return;
            var context = sender.As<HttpApplication>().Context;
            if (context.Response.StatusCode != 401)
                return;
            context.Response.Redirect(LoginRedirect, false);
        }

        void context_AuthenticateRequest(object sender, System.EventArgs e)
        {
            var ctx = sender.As<HttpApplication>().Context;

            var id = GetSessionId(ctx.Request.Cookies);
            if (id != null)
            {
                var user = _service.Get(id.Value);
                if (user != null)
                {
                    ctx.User=new GenericPrincipal(user,new string[0]);
                    return;
                }
                else
                {
                    DestroyAuthCookie(ctx.Response.Cookies);
                }
            }
            ctx.User=new GenericPrincipal(new CavemanIdentity(null,new IUserContextGroup[0]),new string[0]);
        }

       

        static Guid? GetSessionId(HttpCookieCollection cookies)
        {
            var ck = cookies[CookieName];
            if (ck != null)
            {
                try
                {
                    return new Guid(Convert.FromBase64String(ck.Value));
                }
                catch (FormatException)
                {
                    //invalid base64
                }
                catch (ArgumentException)
                {
                    //invalid guid
                }
            }
            return null;
        }

       
        static void SetCookie(HttpCookieCollection cookies, HttpCookie ck)
        {
            ck.Path = CookiePath;
            if (!CookieDomain.IsNullOrEmpty())
            {
                ck.Domain = CookieDomain;
            }
            cookies.Attach(ck);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookies"></param>
        /// <param name="id"></param>
        /// <param name="rememberMe"></param>
        /// <param name="valability">Custom valability. Remember me is ignored</param>
        public static void SetAuthCookie(HttpCookieCollection cookies, Guid id, bool rememberMe = false, TimeSpan? valability = null)
        {
            var ck = new HttpCookie(CookieName, id.ToBase64());

            if (valability != null)
            {
                ck.Expires = DateTime.UtcNow.Add(valability.Value);
            }
            else
            {
                if (rememberMe)
                {
                    ck.Expires = DateTime.UtcNow.Add(RememberMeValidity);
                }

            }
            SetCookie(cookies, ck);
        }


        public static void DestroyAuthCookie(HttpCookieCollection resp)
        {
            var ck = new HttpCookie(CookieName, "");
            ck.Expires = new DateTime(2012, 1, 1);
            SetCookie(resp, ck);
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}