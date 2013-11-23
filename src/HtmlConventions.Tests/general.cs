using System.Linq.Expressions;
using CavemanTools.Model;
using Xunit;
using System;
using System.Diagnostics;
using FluentAssertions;

namespace HtmlConventions.Tests
{
        public class general
    {
        private Stopwatch _t = new Stopwatch();

        public general()
        {

        }

        [Fact]
        public void test()
        {
            
          
        }

        protected void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }
}