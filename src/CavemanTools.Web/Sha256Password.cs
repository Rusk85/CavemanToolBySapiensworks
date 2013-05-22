using System;

namespace CavemanTools.Web
{
    /// <summary>
    /// Password hashing strategy using sha256
    /// </summary>
    public class Sha256Password:IHashPassword
    {
        public string Hash(string text, string salt = null)
        {
            return (text + ";" + salt??"").Sha256();
        }
    }

}