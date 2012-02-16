using System;

namespace CavemanTools.Web.Security
{
    public abstract class AuthorizationScopeId:IEquatable<AuthorizationScopeId>
    {
        public abstract bool Equals(AuthorizationScopeId other);

    }
}