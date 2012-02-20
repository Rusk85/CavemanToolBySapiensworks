using System;

namespace CavemanTools.Web
{
	/// <summary>
	/// Default implementation of IOnlineUserInfo
	/// </summary>
	public class OnlineUserInfo : IOnlineUserInfo
	{
		/// <summary>
		/// Gets or sets user id, use null for anonymous users
		/// </summary>
		public object UserId { get; set; }
		/// <summary>
		/// Gets or sets the name of the user
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Gets or sets the date when the user checked in
		/// </summary>
		internal DateTime Expiraton { get; set; }

		/// <summary>
		/// Gets if the visitor is anonymous
		/// </summary>
		public bool IsAnonymous
		{
			get { return UserId == null; }
		}
	}
}