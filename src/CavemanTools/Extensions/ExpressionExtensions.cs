using System.Reflection;

namespace System.Linq.Expressions
{
    public static  class ExpressionExtensions
	{
		/// <summary>
		/// Returns reflection information for a property expression
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="property">Lambda returning the property</param>
		/// <returns></returns>
		public static MemberInfo GetPropertyInfo<T>(this Expression<Func<T,object>> property)
		{
			if (property == null) throw new ArgumentNullException("property");
			var prop = property.Body as MemberExpression;
			if (prop == null && property.Body is UnaryExpression)
			{
				prop = (property.Body as UnaryExpression).Operand as MemberExpression;
			}
			if (prop == null) throw new FormatException("Lambda should be a property");
			return prop.Member;
		}

        ///// <summary>
        ///// Returns reflection information for a method call expression
        ///// </summary>
        ///// <typeparam name="T">Type</typeparam>
        ///// <param name="method">Lambda with method call</param>
        ///// <returns></returns>
        //public static MethodInfo GetMethodInfo<T>(this Expression<Func<T, object>> method)
        //{
        //    if (method == null) throw new ArgumentNullException("method");
        //    var prop = method.Body as MethodCallExpression;

        //    if (prop == null && method.Body is UnaryExpression)
        //    {
        //        prop = method.Body.As<UnaryExpression>().Operand.As<MethodCallExpression>();
        //    }

        //    if (prop == null) throw new FormatException("Lambda should be a method call");
        //    return prop.Method;
        //}

        /// <summary>
        /// Returns reflection information for a method call expression
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="method">Lambda with method call</param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo<T>(this Expression<Action<T>> method)
        {
            if (method == null) throw new ArgumentNullException("method");
            var prop = method.Body as MethodCallExpression;
            if (prop == null) throw new FormatException("Lambda should be a method call");
            return prop.Method;
        }

        ///// <summary>
        ///// Returns reflection information for a property expression
        ///// </summary>
        ///// <typeparam name="T">Type</typeparam>
        ///// <param name="property">Lambda returning the property</param>
        ///// <returns></returns>
        //public static MemberInfo GetInfo<T, V>(this Expression<Func<T, V>> property)
        //{
        //    if (property == null) throw new ArgumentNullException("property");
        //    var prop = property.Body as MemberExpression;
        //    if (prop == null && property.Body is UnaryExpression)
        //    {
        //        prop = (property.Body as UnaryExpression).Operand as MemberExpression;
        //    }
        //    if (prop == null) throw new FormatException("Lambda should be a property");
        //    return prop.Member;
        //}
	}
}