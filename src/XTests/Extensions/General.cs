using System.Web;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace XTests.Extensions
{
    public class General
    {
        private Stopwatch _t = new Stopwatch();

        public General()
        {

        }

        public class BlaController { }

        [Fact]
        public void namespace_to_website_relative_path()
        {
            typeof(BlaController).ToWebsiteRelativePath(GetType().Assembly).Should().Be("~/Extensions/Bla");
            typeof(BlaController).ToWebsiteRelativePath(GetType().Assembly,false).Should().Be("~/Extensions/BlaController");
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}