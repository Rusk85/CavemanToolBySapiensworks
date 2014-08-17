
using CavemanTools;
using CavemanTools.Logging;
using FluentAssertions;
using Xunit;
using System;
using System.Diagnostics;

namespace XTests
{

    class Test
    {
        public void Do(string t)
        {
           Console.Write(t);
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
            string i = "4";
            dynamic t = new Test();
            t.Do((object)i);
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