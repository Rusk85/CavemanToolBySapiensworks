using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Web.Caching;
using System.Web.Mvc;
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
using Xunit.Extensions;

namespace XTests
{
   
   
    public class General
    {
        private Stopwatch _t = new Stopwatch();
      
        public General()
        {
            
        }

        [Fact]
        public void FactMethodName()
        {
            var sess = SessionId.NewId();
            var s2 = SessionId.Parse(sess.ToString());
            Assert.Equal(sess,s2);
            for (int i = 0; i < 50; i++)
            {
                Write(SessionId.NewId().ToString());
            }
            Write(sess);
            Write(s2.ToString().Length);

            Assert.Throws<ArgumentException>(() => SessionId.Parse("sdfsfs"));
            
        }


        private void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }  
}