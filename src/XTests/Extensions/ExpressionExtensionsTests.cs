using System.Linq.Expressions;
using Xunit;
using System;
using System.Diagnostics;

namespace XTests.Extensions
{
    
    public class Test
    {
        public void Method(int id)
        {
            
        }

        public int Bla()
        {
            return 0;
        }

        public Test Id { get; set; }
    }
    
    public class ExpressionExtensionsTests
    {
        private Stopwatch _t = new Stopwatch();
        private Type _tp;

        public ExpressionExtensionsTests()
        {
            _tp = typeof (Test);
        }

        [Fact]
        public void get_method_info_from_expression_when_returning_void()
        {
            var mi = ExpressionExtensions.GetMethodInfo<Test>(t => t.Method(2));
            Assert.Equal(_tp.GetMethod("Method"),mi);
            ExpressionExtensions.GetPropertyInfo<Test>(t => t.Id);
            ExpressionExtensions.GetPropertyInfo<Test>(t => t.Id.Id);
        }
    
        [Fact]
        public void get_method_info_from_expression_when_returning_something()
        {
            var mi = ExpressionExtensions.GetMethodInfo<Test>(t => t.Bla());
            Assert.Equal(_tp.GetMethod("Bla"),mi);
        }

        protected void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }
}