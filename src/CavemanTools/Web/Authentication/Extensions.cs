using System;
using System.Security.Principal;
using System.Web;

namespace CavemanTools.Web.Authentication
{
    public static class Extensions
    {
        /// <summary>
        /// Returns the user identity
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static IUserRightsContext GetCavemanIdentity(this IPrincipal user)
        {
            if (user.Identity.AuthenticationType == CavemanIdentity.Type)
            {
                return user.Identity as CavemanIdentity;
            }
            throw new NotSupportedException("Only caveman identity is supported");
        }

        /// <summary>
        /// Sets the scope for authorization purposes
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="scopeId"></param>
        public static void SetAuthScope(this IPrincipal principal,AuthorizationScopeId scopeId)
        {
            if (scopeId == null) throw new ArgumentNullException("scopeId");
            var usr = principal.GetCavemanIdentity();
            usr.ScopeId = scopeId;
        }
     
    }
}