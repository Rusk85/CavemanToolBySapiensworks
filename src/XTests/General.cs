
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
            Write(System.Environment.Version.ToString());
        }


        private void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }  
}