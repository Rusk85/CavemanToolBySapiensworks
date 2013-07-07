using System;

namespace CavemanTools.Web
{
    public class PasswordHash
    {
        public PasswordHash(string hash,string salt="")
        {
            if (string.IsNullOrWhiteSpace(hash)) throw new ArgumentNullException("hash");
            if (salt == null) throw new ArgumentNullException("salt");
            Hash = hash;
            Salt = salt;
        }

        /// <summary>
        /// Gets hash
        /// </summary>
        public string Hash { get; private set; }
        /// <summary>
        /// Gets salt used for hashing
        /// </summary>
        public string Salt { get; private set; }
        /// <summary>
        /// True if the supplied argument matches hash
        /// </summary>
        /// <param name="otherHash"></param>
        /// <returns></returns>
        public bool Matches(string otherHash)
        {
            return otherHash != null && (Hash.Equals(otherHash, StringComparison.InvariantCultureIgnoreCase));
        }
        
    }

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