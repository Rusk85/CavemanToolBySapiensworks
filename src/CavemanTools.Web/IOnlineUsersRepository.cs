using System;

namespace CavemanTools.Web
{
	/// <summary>
	/// Provides functionality for repository managing online users list
	/// </summary>
	public interface IOnlineUsersRepository
	{
		/// <summary>
		/// Gets or sets the time period in which an user is considered online
		/// Default is 15 minutes
		/// </summary>
		TimeSpan ExpirationInterval { get; set; }

		/// <summary>
		/// Gets the list of online users
		/// </summary>
		/// <returns></returns>
		OnlineUsersData GetOnlineUsers();

		void CheckInAnonymous(string identifier);

		/// <summary>
		///  Registers a visitor as being online
		/// </summary>
		/// <param name="name">Username or a random string for anonymous users</param>
		/// <param name="userId">Can be null for anonymous users</param>
		void CheckInMember(string name,object userId);
	}
}