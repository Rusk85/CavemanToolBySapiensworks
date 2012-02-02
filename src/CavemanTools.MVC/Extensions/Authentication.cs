using CavemanTools.Web.Security;

// ReSharper disable CheckNamespace
namespace System.Web.Mvc
// ReSharper restore CheckNamespace
{
    public static class Authentication
    {
       
        /// <summary>
        /// Creates and attaches to response the authentication cookie
        /// </summary>
        /// <param name="response"></param>
        /// <param name="userId"></param>
        /// <param name="name">username </param>
        /// <param name="group">group id</param>
        /// <param name="isPersistent"></param>
        /// <returns></returns>
        public static HttpCookie SetAuthCookie(this HttpResponseBase response, int userId, string name, int group, bool isPersistent = false)
        {
            return SetAuthCookie(response, new UserId(userId), name, group, isPersistent);
        }

        /// <summary>
        /// Creates and attaches to response the authentication cookie
        /// </summary>
        /// <param name="response"></param>
        /// <param name="userId"></param>
        /// <param name="name">username</param>
        /// <param name="group">group id</param>
        /// <param name="isPersistent"></param>
        /// <returns></returns>
        public static HttpCookie SetAuthCookie(this HttpResponseBase response, Guid userId, string name, int group, bool isPersistent = false)
        {
            return SetAuthCookie(response, new UserGuid(userId), name, group, isPersistent);
        }

        /// <summary>
        /// Creates and attaches to response the authentication cookie
        /// </summary>
        /// <param name="response"></param>
        /// <param name="userId"></param>
        /// <param name="name">username</param>
        /// <param name="group">group id</param>
        /// <param name="isPersistent"></param>
        /// <returns></returns>
        public static HttpCookie SetAuthCookie(this HttpResponseBase response, IUserIdValue userId, string name, int group, bool isPersistent = false)
        {
            var ck = AuthenticationUtils.CreateCookie(userId, name, group, isPersistent);
            response.AppendCookie(ck);
            return ck;
        }

        /// <summary>
        /// Returns the user context created by the UserRights module
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IUserRightsContext GetUserContext(this HttpContextBase context)
        {
            return context.Items[UserRightModule.ContextKey] as IUserRightsContext;
        }

        /// <summary>
        /// Returns the user context created by the UserRights module
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IUserRightsContext GetUserContext(this WebViewPage page)
        {
            return page.ViewContext.HttpContext.GetUserContext();
        }
    }
}