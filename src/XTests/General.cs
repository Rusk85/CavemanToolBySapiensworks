using System.Collections.Generic;
using System.Text;
using System.Web.Caching;
using CavemanTools;
using CavemanTools.Infrastructure;
using CavemanTools.Logging;
using CavemanTools.Web;
using XTests.Mvc.Controllers;
using Xunit;
using System;
using System.Diagnostics;
using CavemanTools.Strings;
using System.Reflection;

namespace XTests
{
    public class Hah
    {
        public int Id { get; set; }
        public MyClass Class { get; set; }
        public List<string> List {get; set; }

        public Hah()
        {
            Class=new MyClass(){Id=45};
            List=new List<string>();
            List.Add("hi");
        }
    }
   
    public class General
    {
        private Stopwatch _t = new Stopwatch();
      
        public General()
        {
            
        }

        [Fact]
        public void FactMethodName()
        {
            var g = Guid.NewGuid();
            Write(g.ToString().Length.ToString());
            var data = Convert.ToBase64String(g.ToByteArray());
            Write(data.Length.ToString());
            var g1 = new Guid(Convert.FromBase64String(data));
            Assert.Equal(g,g1);
            
        }


        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }  
}