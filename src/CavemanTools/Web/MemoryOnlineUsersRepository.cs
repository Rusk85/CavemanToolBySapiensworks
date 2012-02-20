using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CavemanTools.Web
{
	/// <summary>
	/// Manages online users list. Thread safe.
	/// </summary>
	public class MemoryOnlineUsersRepository : IOnlineUsersRepository
	{
		List<OnlineUserInfo> _users=new List<OnlineUserInfo>();


		public MemoryOnlineUsersRepository()
		{
			ExpirationInterval=new TimeSpan(0,15,0);
			SetPurgeTime();
		}


		#region IOnlineUsersRepository
		/// <summary>
		/// Gets or sets the time period in which an user is considered online
		/// Default is 15 minutes
		/// </summary>
		public TimeSpan ExpirationInterval { get; set; }

		/// <summary>
		/// Gets info about the online users.
		/// </summary>
		/// <returns></returns>
		public OnlineUsersData GetOnlineUsers()
		{
			var snapshot = new OnlineUsersData();				
			snapshot.AnonymousCount = CountAnonymous();
			snapshot.OnlineMembers = GetOnlineMembers();			
			return snapshot;
		}

		public void CheckInAnonymous(string identifier)
		{
			CheckInMember(identifier, null);
		}

		/// <summary>
		///  Registers a visitor as being online
		/// </summary>
		/// <param name="name">Username or a random string for anonymous users</param>
		/// <param name="userId">Can be null for anonymous users</param>
		public void CheckInMember(string name, object userId)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("user");

			lock (SyncRoot)
			{
				var item = _users.Find(d => d.Name.Equals(name));

				if (item == null)
				{
					item = new OnlineUserInfo() { Name = name, UserId = userId };
					_users.Add(item);

				}
				item.Expiraton = DateTime.UtcNow.Add(ExpirationInterval);
			}

		}

		#endregion

		#region Specific
		/// <summary>
		/// Returns a sequence of the online authenticated users
		/// </summary>
		/// <returns></returns>
		public IEnumerable<IOnlineUserInfo> GetOnlineMembers()
		{
			if (IsPurgeTime)
			{
				PurgeExpired();
			}

			lock (SyncRoot)
			{
				return ValidUsers.Where(d => !d.IsAnonymous).ToArray();
			}

		}

		/// <summary>
		/// Gets the number of anonymous users
		/// </summary>
		/// <value></value>
		public int CountAnonymous()
		{
			lock (SyncRoot)
			{
				return ValidUsers.Count(d => d.IsAnonymous);
			}			
		}

		/// <summary>
		/// Gets the number of online users
		/// </summary>
		public int CountMembers()
		{
			lock (SyncRoot)
			{
				return ValidUsers.Count(d => !d.IsAnonymous);
			}			
		}
		#endregion

		#region Private/Protected
		private DateTime _purgeTime;


		protected bool IsPurgeTime
		{
			get { return (_purgeTime <= DateTime.UtcNow); }
		}

		protected object SyncRoot
		{
			get { return ((IList)_users).SyncRoot; }
		}
		protected IEnumerable<OnlineUserInfo> ValidUsers
		{
			get
			{
				var dt = DateTime.UtcNow;
				return _users.Where(d => d.Expiraton >= dt);
			}
		}

		void SetPurgeTime()
		{
			_purgeTime = DateTime.UtcNow.AddMinutes(10);
		}

		void PurgeExpired()
		{
			lock (SyncRoot)
			{
				var dt = DateTime.UtcNow;
				_users.RemoveAll(d => d.Expiraton < dt);
			}
			SetPurgeTime();
		}



		#endregion
	}
}