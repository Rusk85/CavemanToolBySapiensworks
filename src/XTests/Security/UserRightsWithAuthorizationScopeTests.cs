using CavemanTools.Web;
using CavemanTools.Web.Authentication;
using CavemanTools.Web.Security;
using Xunit;
using System;
using System.Diagnostics;

namespace XTests.Security
{
    public class UserRightsWithAuthorizationScopeTests
    {
        private Stopwatch _t = new Stopwatch();
        private UserContextGroup _local;
        private UserContextGroup _global;
        private UserRightsContext _user;

        public UserRightsWithAuthorizationScopeTests()
        {
            _global = new UserContextGroup(2, new ushort[2] { 3, 4 });
            _local = new UserContextGroup(1, new ushort[] {UserBasicRights.DoEverything});
            _local.ScopeId = new DefaultAuthorizationScopeId(1);
            _user = new UserRightsContext(new UserId(2), new[] { _global, _local });
        }

        [Fact]
        public void no_scope_ignores_scoped_rights()
        {
            Assert.True(_user.HasRightTo(3));
            Assert.False(_user.HasRightTo(5));
        }

        [Fact]
        public void in_scope_all_rights_are_checked()
        {
           
            _user.ScopeId=new DefaultAuthorizationScopeId(1);
            Assert.True(_user.HasRightTo(3));
            Assert.True(_user.HasRightTo(UserBasicRights.DoEverything));
        }

        [Fact]
        public void only_current_scope_Is_valid()
        {
           _user.ScopeId=new DefaultAuthorizationScopeId(3);
            Assert.True(_user.HasRightTo(4));
            Assert.False(_user.HasRightTo(5));

        }

        [Fact]
        public void is_in_group_ignores_scope()
        {
            _user.ScopeId=new DefaultAuthorizationScopeId(3);
            Assert.True(_user.IsMemberOf(1,2));
        }

        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }
}