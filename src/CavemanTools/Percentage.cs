using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CavemanTools
{
    public class Percentage
    {
        private decimal _value;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">Percentage amount</param>
        public Percentage(decimal value)
        {
            Value = value;
        }

        public decimal Value
        {
            get { return _value * 100; }
            private set { _value = value / 100; }
        }

        public decimal ApplyTo(decimal amount)
        {
            return amount * Value;
        }

        public override string ToString()
        {
            return Value.ToString("P");
        }
    }
}
