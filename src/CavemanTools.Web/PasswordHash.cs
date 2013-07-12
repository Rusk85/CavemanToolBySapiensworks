using System;

namespace CavemanTools.Web
{
    public class PasswordHash
    {
        public PasswordHash(string hash,string salt="")
        {
            if (String.IsNullOrWhiteSpace(hash)) throw new ArgumentNullException("hash");
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

        /// <summary>
        /// Creates a SHA 512 password hash with a 16 char salt
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static PasswordHash CreateCavemanHash(string password)
        {
            password.MustNotBeEmpty();
            var hasher = new CavemanHashStrategy();
            var salt = StringUtils.CreateRandomString(16);
            return new PasswordHash(hasher.Hash(password, salt), salt);
        }
    }
}