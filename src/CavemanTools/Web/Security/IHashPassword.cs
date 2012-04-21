namespace CavemanTools.Web.Security
{
    public interface IHashPassword
    {
        /// <summary>
        /// Creates a hash for a string using salt
        /// </summary>
        /// <param name="text">Value to hash</param>
        /// <param name="salt">Optional random string</param>
        /// <returns></returns>
        string Hash(string text, string salt = null);       
    }
}