using System;

namespace CavemanTools.Web.Security
{
    /// <summary>
    /// Implments the user id value as a GUID
    /// </summary>
    public class UserGuid:UserIdBase<Guid>
    {
        public UserGuid(Guid id) : base(id)
        {
        }

        public UserGuid(string serializedId) : base(serializedId)
        {
        }

        protected override Guid ConvertFromString(string value)
        {
            return Guid.Parse(value);
        }

        public override bool Equals(IUserIdValue other)
        {
            var o = other as UserGuid;
            if (o == null) return false;
            return other.ValueAs<Guid>().Equals(Id);
        }

        
    }
}