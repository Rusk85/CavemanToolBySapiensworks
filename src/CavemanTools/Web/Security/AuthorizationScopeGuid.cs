using System;

namespace CavemanTools.Web.Security
{
    public class AuthorizationScopeGuid:AuthorizationScopeId
    {
        public AuthorizationScopeGuid(Guid value)
        {
            Value = value;
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public override bool Equals(AuthorizationScopeId other)
        {
            if (other == null) return false;
            var scope = other as AuthorizationScopeGuid;
            if (scope == null) return false;
            return ValueAs<Guid>() == scope.ValueAs<Guid>();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as AuthorizationScopeGuid);
        }
    }
}