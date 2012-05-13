using System.Dynamic;
using CavemanTools.Data;
using Xunit;
using System;
using System.Diagnostics;
using System.Reflection;

namespace XTests.Reflection
{
    public class ReflectionUtilsTests
    {
        private Stopwatch _t = new Stopwatch();

        public ReflectionUtilsTests()
        {

        }

        class MyClass
        {
            public int Id { get; set; }
            public DateTime? Date { get; set; }
        }

        [Fact]
        public void get_property_Value_from_normal_object()
        {
            var c = new MyClass();
            c.Id = 23;
            c.Date=new DateTime(2012,05,03);
            Assert.Equal(23,c.GetPropertyValue("Id"));
            Assert.Equal(new DateTime(2012,05,03),c.GetPropertyValue("Date"));
        }

        [Fact]
        public void convert_to_nullable()
        {
            object d="Ok";
            JsonStatus? target;
            target = d.ConvertTo<JsonStatus?>();
            Assert.Equal(JsonStatus.Ok, target);
            target = d.ConvertTo<JsonStatus>();
            Assert.Equal(JsonStatus.Ok,target);
            d =2;
            target = d.ConvertTo<JsonStatus?>();
            Assert.Equal(JsonStatus.Redirect, target);
            target = d.ConvertTo<JsonStatus>();
            Assert.Equal(JsonStatus.Redirect, target);
        }

        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }
}