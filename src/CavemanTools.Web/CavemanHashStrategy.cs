using System;

namespace CavemanTools.Web
{
    public class CavemanHashStrategy:IHashPassword
    {
        /// <summary>
        /// Creates a hash for a string using salt
        /// </summary>
        /// <param name="text">Value to hash</param>
        /// <param name="salt">Optional random string</param>
        /// <returns></returns>
        public string Hash(string text, string salt = null)
        {
            if (salt==null) throw new ArgumentException("Salt is required for this hash");
            
            if (salt.Length%2 != 0)
            {
                salt=salt.PadRight(salt.Length + 1, '*');
            }

            var first = salt.Substring(0, salt.Length/2);

            var pwd = first + text+salt.Substring(salt.Length/2,salt.Length);
            return pwd.Sha512();
        }
    }
}