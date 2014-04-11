
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
            var data = @"
""Angie Rodriguez: Bon, nous avons acheté une laveuse et une sécheuse chez Ameublement Elvis au Plateau (Papineau et Laurier, si je ne me trompe pas...) et c'était bon! BTW, nous avons besoin de les vendre si jamais tu en as besoin. ""(7 feb 2014, https://www.facebook.com/loredana.lillo/posts/483089435147812?comment_id=2433101&offset=0&total_comments=8)
";
           Write(data);
            Write(data.AddSlashes());
          //  @"acesta e un 'test'".AddSlashes().Should().Be("acesta e un \'test\'");
        }


        private void Write(object format, params object[] param)
        {
            Console.WriteLine(format.ToString(), param);
        }
    }  
}