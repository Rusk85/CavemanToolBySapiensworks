using System;
using System.Web;


namespace CavemanTools.Web
{
	/// <summary>
	/// Provides an encrypted cookie to store seesion values
	/// </summary>
	public class EncryptedCookie
	{
		private HttpCookie _cookie;
		private ICookieEncryption _encryptor;

		/// <summary>
		/// Constructs a nw encrypted cookie
		/// </summary>
		/// <param name="name">Cookie name</param>
		/// <param name="encryptionSecret">secret salt for encryption</param>
		public EncryptedCookie(string name,string encryptionSecret):this(new HttpCookie(name), new DefaultCookieEncryption(encryptionSecret))
		{
			
		}

		/// <summary>
		/// Constructs a nw encrypted cookie
		/// </summary>
		/// <param name="cookie">Cookie name</param>
		/// <param name="salt">secret salt for encryption</param>
		public EncryptedCookie(HttpCookie cookie,string salt):this(cookie,new DefaultCookieEncryption(salt))
		{
			
		}

		/// <summary>
		/// Creates a new encrypted cookie from request or a new one if it's missing
		/// </summary>
		/// <param name="request">Request collection</param>
		/// <param name="name">Cookie name</param>
		/// <param name="secret">Secret key</param>
		/// <returns></returns>
		public static EncryptedCookie FromRequest(HttpCookieCollection request,string name, string secret)
		{
			return FromRequest(request, name, new DefaultCookieEncryption(secret));
		}

		/// <summary>
		/// Creates a new encrypted cookie from request or a new one if it's missing
		/// </summary>
		/// <param name="request">Request collection</param>
		/// <param name="name">cookie name</param>
		/// <param name="encryptor">encrypting strategy</param>
		/// <returns></returns>
		public static EncryptedCookie FromRequest(HttpCookieCollection request,string name,ICookieEncryption encryptor)
		{
			if (request == null) throw new ArgumentNullException("request");
			if (name == null) throw new ArgumentNullException("name");
			if (encryptor == null) throw new ArgumentNullException("encryptor");
			if (request[name]==null)
			{
				return new EncryptedCookie(new HttpCookie(name),encryptor);
			}

			return new EncryptedCookie(request[name],encryptor);
		}

		/// <summary>
		/// Constructs a nw encrypted cookie
		/// </summary>
		/// <exception cref="ArgumentNullException"></exception>
		/// <param name="cookie">Cookie</param>
		/// <param name="encryptor">Encripting strategy</param>
		public EncryptedCookie(HttpCookie cookie,ICookieEncryption encryptor)
		{
			if (cookie == null) throw new ArgumentNullException("cookie");
			if (encryptor == null) throw new ArgumentNullException("encryptor");
			_cookie = cookie;
			_encryptor = encryptor;
		}

		/// <summary>
		/// Decrypts and opens the cookie for modification
		/// </summary>
		/// <returns></returns>
		public void Unseal()
		{
			_encryptor.Decrypt(Cookie);
			_isSealed = false;
		}

		/// <summary>
		/// Gets or sets cookie value for key
		/// </summary>
		/// <param name="key">Key</param>
		/// <returns></returns>
		public string this[string key]
		{
			get
			{
				if (_isSealed) throw new InvalidOperationException("Cookie is sealed, nothing can be retrieved or added");
				return Cookie.Values[key];
			}

			set
			{
				if (_isSealed) throw new InvalidOperationException("Cookie is sealed, nothing can be retrieved or added");
				Cookie.Values[key] = value;
			}
		}

		public void Clear()
		{
			Cookie.Value = "";
		}

		private bool _isSealed;
		/// <summary>
		/// Encrypts and seals the cookie, nothing can be modified or retrieved
		/// </summary>
		public void Seal()
		{
			if (_isSealed) return;
			_encryptor.Encrypt(Cookie);
			_isSealed = true;
		}

		
		/// <summary>
		/// Gets a cookie value as object of type
		/// </summary>
		/// <typeparam name="T">Object type</typeparam>
		/// <param name="key">Key</param>
		/// <param name="defaultValue">return this if key was not found or conversion failed</param>
		/// <returns></returns>
		public T GetValue<T>(string key,T defaultValue)
		{
			if (_isSealed) throw new InvalidOperationException("Cookie is sealed, nothing can be retrieved or added");
			return Cookie.Values[key].Parse(defaultValue);
		}

		

		/// <summary>
		/// Gets underlying cookie
		/// </summary>
		public HttpCookie Cookie
		{
			get { return _cookie; }
		}

		/// <summary>
		/// Saves the encrypted cookie into collection
		/// </summary>
		/// <remarks>
		/// If cookie is not sealed it will try to seal it first.
		/// </remarks>
		/// <param name="response">Cookie collection</param>
		public void Save(HttpCookieCollection response)
		{
			if (response == null) throw new ArgumentNullException("response");
			if (!_isSealed) Seal();
            
			if (response[Cookie.Name]==null)
			{
				response.Add(Cookie);
			}
			else
			{
				response.Set(Cookie);
			}
		}
	}
}