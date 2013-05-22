using System;

namespace CavemanTools.Web.Authentication
{
    /// <summary>
    /// Base class to implement user id value
    /// </summary>
    /// <typeparam name="T">Underlying value type</typeparam>
    public abstract class UserIdBase<T>:IUserIdValue
    {
        protected T Id;

        public UserIdBase(T id)
        {
            Id = id;
        }

        public UserIdBase(string serializedId)
        {
            Id = ConvertFromString(serializedId);
        }

        protected abstract T ConvertFromString(string value);
        

        public object Value
        {
            get { return Id; }
        }

        public TID ValueAs<TID>()
        {
            if (typeof(T).Equals(typeof(TID)))
                return (TID)Value;
            throw new InvalidOperationException(string.Format("User id is of type '{0}'.", typeof (T).Name));
        }

        public abstract bool Equals(IUserIdValue other);

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals((IUserIdValue)obj);
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}