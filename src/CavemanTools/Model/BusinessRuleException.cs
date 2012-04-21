using System;

namespace CavemanTools.Model
{
    /// <summary>
    /// This exception is used to communicate errors to the UI
    /// </summary>
    public class BusinessRuleException:Exception
    {
        public BusinessRuleException(string key,string message):base(message)
        {
            Name = key;         
        }

        public string Name { get; private set; }
        
    }
}