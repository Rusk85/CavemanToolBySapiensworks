using System;

namespace CavemanTools.Web.Authentication
{
    public abstract class AuthorizationScopeId:IEquatable<AuthorizationScopeId>
    {
        public abstract bool Equals(AuthorizationScopeId other);
        public object Value { get; protected set; }
        public T ValueAs<T>()
        {
            return (T) Value;
        }
    }
}