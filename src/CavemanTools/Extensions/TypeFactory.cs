using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace System.Reflection
{
    /// <summary>
    /// For fast creation of types, useful for creating POCOs
    /// </summary>
    public static class TypeFactory
    {
        static object actLock = new object();
        private static Dictionary<Type, Func<object>> _actCache;

        static TypeFactory()
        {
            _actCache = new Dictionary<Type, Func<object>>();
        }

        /// <summary>
        /// Gets factory to create instance of type using the public parameterless ctor.
        /// Use it when you want to create many instances of the same type in different objects
        /// Aprox, 1.3x faster than Activator, almost as fast a manual if you cache and reuse the delegate
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Func<object> GetFactory(Type t)
        {
            Func<object> inv;
            var key = t;
            lock (actLock)
            {
                if (_actCache == null) _actCache = new Dictionary<Type, Func<object>>();
                if (!_actCache.TryGetValue(t,out inv))
                {
                    var meth = new DynamicMethod("dynctor", typeof(object), new Type[0], t);
                    var il = meth.GetILGenerator();
                    il.Emit(OpCodes.Newobj, t.GetConstructor(new Type[0]));
                    il.Emit(OpCodes.Ret);
                    inv = (Func<object>)meth.CreateDelegate(Expression.GetFuncType(typeof(object)));
                    _actCache[t] = inv;
                }

            }

            return inv;
        }
    }
}