using System.Collections.Generic;
using System.Reflection;
using CavemanTools.Web.Security;

// ReSharper disable CheckNamespace
namespace System.Web.Mvc
// ReSharper restore CheckNamespace
{
    public static class Authentication
    {
       /// <summary>
       ///  Use this ONLY if you don't use an IoC container
       /// </summary>
       /// <param name="repository"></param>
        public static void RegisterRepositoryForDependecyResolver(Func<IUserRightsRepository> repository)
        {
            if (repository == null) throw new ArgumentNullException("repository");
          
             DependencyResolver.SetResolver(t =>
            {
                if (t.Implements<IUserRightsRepository>())
                {
                    return repository();
                }
                else
                {
                    try
                    {
                        return Activator.CreateInstance(t);
                    }
                    catch
                    {
                        return (object)null;
                    }
                }
                
            }, t =>
            {
                return new object[0];
            });
        }

        /// <summary>
        /// Creates and attaches to response the authentication cookie
        /// </summary>
        /// <param name="response"></param>
        /// <param name="userId"></param>
        /// <param name="name">username </param>
        /// <param name="group">group id</param>
        /// <param name="isPersistent"></param>
        /// <returns></returns>
        public static HttpCookie SetAuthCookie(this HttpResponseBase response, int userId, string name, IEnumerable<int> groups, bool isPersistent = false)
        {
            return SetAuthCookie(response, new UserId(userId), name, groups, isPersistent);
        }

        /// <summary>
        /// Creates and attaches to response the authentication cookie
        /// </summary>
        /// <param name="response"></param>
        /// <param name="userId"></param>
        /// <param name="name">username</param>
        /// <param name="groups">User groups</param>
        /// <param name="isPersistent"></param>
        /// <returns></returns>
        public static HttpCookie SetAuthCookie(this HttpResponseBase response, Guid userId, string name, IEnumerable<int> groups, bool isPersistent = false)
        {
            return SetAuthCookie(response, new UserGuid(userId), name, groups, isPersistent);
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
        public static HttpCookie SetAuthCookie(this HttpResponseBase response, IUserIdValue userId, string name, IEnumerable<int> group, bool isPersistent = false)
        {
            var ck = AuthenticationUtils.CreateCookie(userId, name, group, isPersistent);
            response.AppendCookie(ck);
            return ck;
        }

        /// <summary>
        /// Returns the Caveman user context 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IUserRightsContext GetUserContext(this HttpContextBase context)
        {
            return context.Items[UserRightModule.ContextKey] as IUserRightsContext;
        }

        /// <summary>
        /// Returns the Caveman user context 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static IUserRightsContext GetUserContext(this WebViewPage page)
        {
            return page.ViewContext.HttpContext.GetUserContext();
        }

        /// <summary>
        /// Returns the Caveman user context 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static IUserRightsContext GetUserContext(this Controller c)
        {
            return c.HttpContext.GetUserContext();
        }
    }
}