using System;

namespace CavemanTools.Model.ValueObjects
{
    public abstract class AbstractValueObject<T>
    {
        public AbstractValueObject(T value)
        {
            if (!Validate(value)) throw new ArgumentException();
            Value = value;
        }

        protected abstract bool Validate(T value);

        public T Value { get; private set; }

   
        public override string ToString()
        {
            return Value.ToString();
        }
     }
}