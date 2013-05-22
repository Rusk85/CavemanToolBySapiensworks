using System;
using System.Collections.Generic;

namespace CavemanTools.Web.Security.AccessRules
{
    //public class ValidateCredentialsFactory : IValidateCredentialsFactory
    //{
    //    Dictionary<Type,Func<Type, IValidateCredentials>> _access=new Dictionary<Type, Func<Type, IValidateCredentials>>();

    //    /// <summary>
    //    /// Removes all rules
    //    /// </summary>
    //    public void Clear()
    //    {
    //        _access.Clear();
    //    }


    //    /// <summary>
    //    /// Register access rule 
    //    /// </summary>
    //    /// <param name="factory">function returning a new instance of type. If null the DefaultFactory method is used</param>
    //    public void RegisterAccessRule<T>(Func<Type, IValidateCredentials> factory=null) where T:IValidateCredentials
    //    {
    //        if (factory == null) factory = DefaultFactory;
    //        RegisterAccessRule(typeof(T),factory);
    //    }


    //    /// <summary>
    //    /// Register access rule using the default factory method
    //    /// </summary>
    //    /// <param name="type">Type implementing IValidateCredentials</param>
    //    /// <exception cref="ArgumentNullException">if any of arguments is null</exception>
    //    public void RegisterAccessRule(Type type)
    //    {
    //        RegisterAccessRule(type,DefaultFactory);
    //    }

    //    /// <summary>
    //    /// Register access rule 
    //    /// </summary>
    //    /// <param name="type">Type implementing IValidateCredentials</param>
    //    /// <exception cref="ArgumentNullException">if any of arguments is null</exception>
    //    /// <param name="factory">function returning a new instance of type</param>
    //    public void RegisterAccessRule(Type type,Func<Type, IValidateCredentials> factory)
    //    {
    //        if (type == null) throw new ArgumentNullException("type");
    //        if (factory == null) throw new ArgumentNullException("factory");
    //        _access[type] = factory;
    //    }
        
    //    /// <summary>
    //    /// Default factory method for creating registered access rules
    //    /// </summary>
    //    /// <param name="tp">Type implementing IValidateCredentials</param>
    //    /// <returns></returns>
    //    public IValidateCredentials DefaultFactory(Type tp)
    //    {
    //        if (tp == null) throw new ArgumentNullException("tp");
    //        return Activator.CreateInstance(tp) as IValidateCredentials;
    //    }

    //    /// <summary>
    //    /// Returns a new instance of the requested access rule
    //    /// </summary>
    //    /// <param name="typeName">Full name of type implementing IValidateCredentials</param>
    //    /// <exception cref="ArgumentException">if type is not registered</exception>
    //    /// <returns></returns>
    //    public IValidateCredentials GetAccessRule(string typeName)
    //    {
    //        return GetAccessRule(Type.GetType(typeName));
    //    }

    //    /// <summary>
    //    /// Returns a new instance of the requested access rule
    //    /// </summary>
    //    /// <exception cref="ArgumentException">if type is not registered</exception>
    //    /// <returns></returns>
    //    public IValidateCredentials GetAccessRule<T>() where T:IValidateCredentials
    //    {
    //        return GetAccessRule(typeof(T));
    //    }

    //    /// <summary>
    //    /// Returns a new instance of the requested access rule
    //    /// </summary>
    //    /// <param name="type">Type implementing IValidateCredentials</param>
    //    /// <exception cref="ArgumentException">if type is not registered</exception>
    //    /// <returns></returns>
    //    public IValidateCredentials GetAccessRule(Type type)
    //    {
    //        Func<Type, IValidateCredentials> activator;
    //        if (_access.TryGetValue(type,out activator))
    //        {
    //            return activator(type);
    //        }
    //        throw new ArgumentException(string.Format("AccessRule type '{0}' not registered",type.FullName));
    //    }
    //}
}