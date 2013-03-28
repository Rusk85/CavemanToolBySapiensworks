using System;

namespace CavemanTools.Web
{
    /// <summary>
    /// Hashed password class
    /// </summary>
    public class HashedPassword
    {
        public HashedPassword(string hash,string salt="")
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
}