namespace CavemanTools.Web
{
	/// <summary>
	/// Defines the minimum information required about an online user.
	/// Used by implementations of IOnlineUsersRepository
	/// </summary>
	public interface IOnlineUserInfo
	{
		/// <summary>
		/// Gets or sets user id, use null for anonymous users
		/// </summary>
		object UserId { get; set; }

		/// <summary>
		/// Gets or sets the name of the user
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Gets if the visitor is anonymous
		/// </summary>
		bool IsAnonymous { get; }
	}
}