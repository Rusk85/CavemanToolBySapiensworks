using System;
using System.Web;

// ReSharper disable CheckNamespace

namespace CavemanTools.Web.Authentication
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// For Caveman Auth only
    /// </summary>
    public static class Authentication
    {
        public static void SetLoginCookie(this HttpResponseBase resp, Guid id, bool rememberMe = false,
                                          TimeSpan? expire = null)
        {
            CavemanAuthenticationModule.SetAuthCookie(resp.Cookies, id, rememberMe, expire);
        }

        public static void DestroyLoginCookie(this HttpResponseBase resp)
        {
            CavemanAuthenticationModule.DestroyAuthCookie(resp.Cookies);
        }
    }
}

