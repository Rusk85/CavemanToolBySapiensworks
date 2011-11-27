using System.Collections.Generic;

namespace CavemanTools.Web
{
	/// <summary>
	/// Used by IOnlineUsersRepository to return information
	/// </summary>
	public class OnlineUsersData
	{
		/// <summary>
		/// Gets the number of anonymous users
		/// </summary>
		public int AnonymousCount { get; internal set; }
		/// <summary>
		/// Gets a sequence of online members
		/// </summary>
		public IEnumerable<IOnlineUserInfo> OnlineMembers { get; set; }
	
	}
}