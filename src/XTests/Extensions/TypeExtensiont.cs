using Xunit;
using System;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;

namespace XTests.Extensions
{
    public class TypeExtensiont
    {
        private Stopwatch _t = new Stopwatch();

        public TypeExtensiont()
        {

        }

        [Fact]
        public void user_defined_reference_types()
        {
            var objects = new object[] {2,Guid.Empty,new byte[0], EnvironmentVariableTarget.Process,"string"};
            
            objects.ForEach(o=>o.GetType().IsUserDefinedClass().Should().BeFalse("{0}".ToFormat(o)));

            typeof (int?).IsUserDefinedClass().Should().BeFalse();

            this.GetType().IsUserDefinedClass().Should().BeTrue();
        }

        protected void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }
}