using System;
using System.Linq;
using System.Threading;
using CavemanTools.Web;
using Xunit;

namespace XTests.Web
{
	public class OnlineUsersTests
	{
		private MemoryOnlineUsersRepository _ou;

		public OnlineUsersTests()
		{
			_ou = new MemoryOnlineUsersRepository() { ExpirationInterval = TimeSpan.FromMilliseconds(500) };
		}

		[Fact]
		public void Adding_Users()
		{
			_ou.CheckInMember("ano",2);
			_ou.CheckInAnonymous("b");
		}

		[Fact]
		public void Only_Members()
		{
			 _ou.CheckInMember("ano", 2);
			 _ou.CheckInMember("be",9);
			Assert.Equal(0,_ou.CountAnonymous());
		}

		[Fact]
		public void One_Users_One_Ano()
		{
			_ou.CheckInMember("ano", 2);
			_ou.CheckInAnonymous("b");
			Assert.Equal(1,_ou.CountMembers());
			Assert.Equal(1,_ou.CountAnonymous());
		}

		[Fact]
		public void User_Expired_After_500MS()
		{
			_ou.CheckInMember("bla",2);
			Assert.Equal(1,_ou.CountMembers());
			Thread.Sleep(500);
			Assert.Equal(0,_ou.CountMembers());
		}

		[Fact]
		public void One_User_two_Ano_In_ReturnData()
		{
			_ou.CheckInAnonymous("bla");
			_ou.CheckInAnonymous("fi");
			_ou.CheckInMember("ff",1);
			var res = _ou.GetOnlineUsers();
			Assert.Equal(2, res.AnonymousCount);
			var user = res.OnlineMembers.First();
			Assert.Equal("ff",user.Name);
			Assert.False(user.IsAnonymous);
		}
	}
}