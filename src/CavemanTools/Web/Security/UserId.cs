using System;

namespace CavemanTools.Web.Security
{
    /// <summary>
    /// Implements the user id value as an int
    /// </summary>
    public class UserId:UserIdBase<int>
    {
        public UserId(int id) : base(id)
        {
        }

        public UserId(string serializedId) : base(serializedId)
        {
        }

        protected override int ConvertFromString(string value)
        {
            return Int32.Parse(value);
        }

        public override bool Equals(IUserIdValue other)
        {
            var o = other as UserId;
            if (o == null) return false;
            return other.ValueAs<int>() == Id;
        }

    }
}