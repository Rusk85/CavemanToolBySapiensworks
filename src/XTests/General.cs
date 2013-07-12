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
            var s = new CavemanHashStrategy();
            var d=s.Hash("hah", StringUtils.CreateRandomString(16));
        }


        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }  
}