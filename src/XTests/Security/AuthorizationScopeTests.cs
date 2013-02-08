using CavemanTools.Web.Security;
using Xunit;
using System;
using System.Diagnostics;

namespace XTests.Security
{
    public class AuthorizationScopeTests
    {
        private Stopwatch _t = new Stopwatch();

        public AuthorizationScopeTests()
        {

        }

        [Fact]
        public void default_authorization_scopeId()
        {
            var scope = new DefaultAuthorizationScopeId(3);
            var scope1 = new DefaultAuthorizationScopeId(3);
            Assert.Equal(scope,scope1);
            var scope2 = new DefaultAuthorizationScopeId(4);
            Assert.NotEqual(scope,scope2);
        }

        [Fact]
        public void authorization_scopeGuid()
        {
            var guid = Guid.NewGuid();
            var scope = new AuthorizationScopeGuid(guid);
            var scope1 = new AuthorizationScopeGuid(guid);
            
            Assert.Equal(scope, scope1);

            var scope2 = new AuthorizationScopeGuid(Guid.NewGuid());
            Assert.NotEqual(scope, scope2);

            var scopeint = new DefaultAuthorizationScopeId(3);
            Assert.NotEqual(scope as AuthorizationScopeId,scopeint as AuthorizationScopeId);
        }


        protected void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }
}