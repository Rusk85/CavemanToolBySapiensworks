using CavemanTools.Web.Security;
using Xunit;
using System;
using System.Diagnostics;

namespace XTests.Security
{
    public class UserRightsContextTests
    {
        private Stopwatch _t = new Stopwatch();
        private UserRightsContext _user;

        public UserRightsContextTests()
        {
            _user = new UserRightsContext(1, new UserContextGroup(1, new byte[1] {UserBasicRights.Login}));            
        }

        [Fact]
        public void user_is_member_of_a_group()
        {
            Assert.True(_user.IsMemberOf(new int[]{1,2}));
        }


        [Fact]
        public void user_has_right_to_login()
        {
            Assert.True(_user.HasRightTo(UserBasicRights.Login));
        }
        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }
}