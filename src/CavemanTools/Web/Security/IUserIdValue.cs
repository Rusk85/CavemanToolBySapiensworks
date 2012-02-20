using System;

namespace CavemanTools.Web.Security
{
    public interface IUserIdValue:IEquatable<IUserIdValue>
    {
        /// <summary>
        /// Gets the id as an object
        /// </summary>
        object Value { get; }
        /// <summary>
        /// Gets the id as the underlying type.
        /// Default is int
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T ValueAs<T>();
        /// <summary>
        /// Id value as string
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}