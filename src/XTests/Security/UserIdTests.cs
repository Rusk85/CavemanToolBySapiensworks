using CavemanTools.Web.Authentication;
using CavemanTools.Web.Security;
using Xunit;
using System;
using System.Diagnostics;

namespace XTests.Security
{
    public class UserIdTests
    {
        private Stopwatch _t = new Stopwatch();

        public UserIdTests()
        {

        }

        [Fact]
        public void userid_int_works_properly()
        {
            var uid = new UserId(3);
            Assert.Equal((object)3,uid.Value);
            Assert.Equal(3,uid.ValueAs<int>());
            Assert.Throws<InvalidOperationException>(() => uid.ValueAs<long>());
        }

        [Fact]
        public void userid_int_from_string_works_properly()
        {
            var uid = new UserId("3");
            Assert.Equal((object)3, uid.Value);
            Assert.Equal(3, uid.ValueAs<int>());
            Assert.Throws<InvalidOperationException>(() => uid.ValueAs<long>());
        }

        [Fact]
        public void invalid_userid_from_string_throws()
        {
            Assert.Throws<FormatException>(() => new UserId("%"));
        }


        [Fact]
        public void userguid_works_properly()
        {
            var g = Guid.NewGuid();
            var uid = new UserGuid(g);
            Assert.Equal((object)g, uid.Value);
            Assert.Equal(g, uid.ValueAs<Guid>());
            Assert.Throws<InvalidOperationException>(() => uid.ValueAs<string>());
        }

        [Fact]
        public void userguid_from_string_works_properly()
        {
            var g = Guid.NewGuid();
            var uid = new UserGuid(g.ToString());
            Assert.Equal((object)g, uid.Value);
            Assert.Equal(g, uid.ValueAs<Guid>());
            Assert.Throws<InvalidOperationException>(() => uid.ValueAs<string>());
        }

        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }
}