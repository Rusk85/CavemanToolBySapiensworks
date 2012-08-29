﻿using CavemanTools.Logging;
using Xunit;
using System;
using System.Diagnostics;

namespace XTests
{
    public static class LogSetup
    {
        
    public static void ToConsole()
    {
        
        LogHelper.Register(new DeveloperLogger(Console.WriteLine),"devel");
    }
}
   
    public class LoggingHelperTests
    {
        private Stopwatch _t = new Stopwatch();

        public LoggingHelperTests()
        {

        }

        [Fact]
        public void logging_helper_has_default_null_object()
        {
            Assert.NotNull(LogHelper.DefaultLogger);
        }

        private void Write(string format, params object[] param)
        {
            Console.WriteLine(format, param);
        }
    }
}