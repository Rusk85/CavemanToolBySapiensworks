using System;

namespace CavemanTools.Web
{
    /// <summary>
    /// Hashed password class
    /// </summary>
    [Obsolete("Use PasswordHash")]
    public class HashedPassword:PasswordHash
    {
        public HashedPassword(string hash, string salt = "") : base(hash, salt)
        {
        }
    }
}