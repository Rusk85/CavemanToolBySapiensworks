using System.Reflection;
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
            _user = new UserRightsContext(new UserId(1), new UserContextGroup(1, new ushort[1] {UserBasicRights.Login}));            
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

        [Fact]
        public void pack_unpack_authentication_ticket_is_ok()
        {
            var auth = new AuthenticationTicketData() {UserId = new UserId(3), Groups = new[] {3, 6}};
            var packed = auth.Pack();
            var auth2 = AuthenticationUtils.Unpack(packed);
            Assert.Equal(auth.Groups,auth2.Groups);
            Assert.Equal(auth.UserId,auth2.UserId);
            
        }

        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }
}