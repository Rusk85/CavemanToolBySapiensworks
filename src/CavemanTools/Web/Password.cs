using System;

namespace CavemanTools.Web
{
    /// <summary>
    /// Use it to encapsulate password hash generation. 
    /// </summary>
    public class Password
    {
        /// <summary>
        /// Generates a password hash using sha256
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt">A random 7 characters string will be used if empty</param>
        public Password(string password, string salt = null):this(password,new Sha256Password(),salt)
        {
            
        }

        /// <summary>
        /// Generates a password hash using the specified hashing strategy
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hasher">Hashing strategy, implement <see cref="IHashPassword"/></param>
        /// <param name="salt">A random 7 characters string will be used if empty</param>
        public Password(string password,IHashPassword hasher, string salt=null)
        {
            if (hasher == null) throw new ArgumentNullException("hasher");
            salt = salt ?? StringUtils.RandomString(7);
            Hash = new HashedPassword(hasher.Hash(password, salt), salt);
        }

        /// <summary>
        /// Gets hashed password
        /// </summary>
        public HashedPassword Hash { get; private set; }
    }
}