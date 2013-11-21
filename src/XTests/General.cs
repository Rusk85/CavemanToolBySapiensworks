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
            Write(System.Environment.Version.ToString());
        }


        private void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }  
}