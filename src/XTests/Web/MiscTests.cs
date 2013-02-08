using Xunit;
using System;
using System.Diagnostics;
using System.Web;

namespace XTests.Web
{
    public class MiscTests
    {
        private Stopwatch _t = new Stopwatch();

        public MiscTests()
        {

        }

        [Fact]
        public void extract_subdomain_works()
        {
            var url = "test.example.com";
            Assert.Equal("test",url.ExtractSubdomain());
            Assert.Equal(string.Empty,"example.com".ExtractSubdomain());
        }

        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }
}