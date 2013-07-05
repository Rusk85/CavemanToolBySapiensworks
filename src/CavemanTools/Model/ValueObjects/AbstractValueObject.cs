using System;

namespace CavemanTools.Model.ValueObjects
{
    public abstract class AbstractValueObject<T>
    {
        protected T _value;

        public AbstractValueObject(T value)
        {
            if (!Validate(value)) throw new ArgumentException();
            _value = value;
        }

        protected abstract bool Validate(T value);

        public T Value
        {
            get { return _value; }        
        }


        public override string ToString()
        {
            return "[{0}]{1}".ToFormat(GetType().Name,_value);
        }
     }
}