using System.Linq;
using CavemanTools.Web.Security;
using Xunit;
using System;
using System.Diagnostics;

namespace XTests.Security
{
    public class UserContextMultipleGroupsTests
    {
        private Stopwatch _t = new Stopwatch();
        private UserRightsContext _ctx;

        public UserContextMultipleGroupsTests()
        {
            var grp1 = new UserContextGroup(1, Enumerable.Empty<ushort>());
            var grp2 = new UserContextGroup(2, new ushort[] {UserBasicRights.Login});
            _ctx = new UserRightsContext(new UserId(2), new[] {grp1, grp2});
        }

        [Fact]
        public void is_member_of_a_group()
        {
            Assert.True(_ctx.IsMemberOf(new[]{1}));
            Assert.True(_ctx.IsMemberOf(new[]{2}));
        }


        [Fact]
        public void user_has_right_to_login()
        {
            Assert.True(_ctx.HasRightTo(UserBasicRights.Login));
        }
        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }
}