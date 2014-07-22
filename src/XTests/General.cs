
using CavemanTools;
using CavemanTools.Logging;
using FluentAssertions;
using Xunit;
using System;
using System.Diagnostics;

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
            var sessionId = SessionId.NewId();
            Write(sessionId.GetHashCode());
          Write(SessionId.NewId().GetHashCode());

            var s2 = SessionId.Parse(sessionId.ToString());
            Write(s2.GetHashCode());
            Assert.Equal(sessionId.GetHashCode(),s2.GetHashCode());
            //  @"acesta e un 'test'".AddSlashes().Should().Be("acesta e un \'test\'");
        }

        [Fact]
        public void exception_log()
        {
            LogManager.OutputToConsole();
            this.LogError(new InvalidOperationException("something"));
        }

        private void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }  
}