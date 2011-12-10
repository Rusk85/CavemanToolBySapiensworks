using System;

namespace CavemanTools.Web.Security.AccessRules
{
    public interface IValidateCredentialsFactory
    {
        /// <summary>
        /// Removes all rules
        /// </summary>
        void Clear();

        /// <summary>
        /// Register access rule 
        /// </summary>
        /// <param name="factory">function returning a new instance of type. If null the DefaultFactory method is used</param>
        void RegisterAccessRule<T>(Func<Type, IValidateCredentials> factory=null) where T:IValidateCredentials;

        /// <summary>
        /// Register access rule using the default factory method
        /// </summary>
        /// <param name="type">Type implementing IValidateCredentials</param>
        /// <exception cref="ArgumentNullException">if any of arguments is null</exception>
        void RegisterAccessRule(Type type);

        /// <summary>
        /// Register access rule 
        /// </summary>
        /// <param name="type">Type implementing IValidateCredentials</param>
        /// <exception cref="ArgumentNullException">if any of arguments is null</exception>
        /// <param name="factory">function returning a new instance of type</param>
        void RegisterAccessRule(Type type,Func<Type, IValidateCredentials> factory);

        /// <summary>
        /// Returns a new instance of the requested access rule
        /// </summary>
        /// <param name="typeName">Full name of type implementing IValidateCredentials</param>
        /// <exception cref="ArgumentException">if type is not registered</exception>
        /// <returns></returns>
        IValidateCredentials GetAccessRule(string typeName);

        /// <summary>
        /// Returns a new instance of the requested access rule
        /// </summary>
        /// <param name="type">Type implementing IValidateCredentials</param>
        /// <exception cref="ArgumentException">if type is not registered</exception>
        /// <returns></returns>
        IValidateCredentials GetAccessRule(Type type);

        /// <summary>
        /// Returns a new instance of the requested access rule
        /// </summary>
        /// <exception cref="ArgumentException">if type is not registered</exception>
        /// <returns></returns>
        IValidateCredentials GetAccessRule<T>() where T:IValidateCredentials;
    }
}