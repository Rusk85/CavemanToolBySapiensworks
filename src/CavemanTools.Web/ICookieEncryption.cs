using System.Web;

namespace CavemanTools.Web
{
	/// <summary>
	/// Provides functioanlity to encrypt and decrypt a cookie
	/// </summary>
	public interface ICookieEncryption
	{
		/// <summary>
		/// Encrypts cookie
		/// </summary>
		/// <param name="value">HttpCookie</param>
		void Encrypt(HttpCookie value);
		/// <summary>
		/// Descrypts cookie
		/// </summary>
		/// <param name="cookie">Cookie</param>
		void Decrypt(HttpCookie cookie);

		/// <summary>
		/// Gets encryption secret (between 8 and 16 characters)
		/// </summary>
		string Secret { get;}
	}
}