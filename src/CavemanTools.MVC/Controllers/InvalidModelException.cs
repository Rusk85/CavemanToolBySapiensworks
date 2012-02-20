using System;

namespace CavemanTools.Mvc.Controllers
{
    public class InvalidModelException:Exception
    {
        public InvalidModelException(string text):base(text)
        {
            
        } 
    }
}