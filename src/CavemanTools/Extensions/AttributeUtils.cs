namespace System.Reflection
{
	public static class AttributeUtils
	{
		/// <summary>
		/// Returns all custom attributes of specified type
		/// </summary>
		/// <typeparam name="T">Attribute</typeparam>
		/// <param name="provider">Custom attributes provider</param>
		/// <returns></returns>
		public static T[] GetCustomAttributes<T>(this ICustomAttributeProvider provider) where T : Attribute
		{
			return GetCustomAttributes<T>(provider, true);
		}

		/// <summary>
		/// Returns all custom attributes of specified type
		/// </summary>
		/// <typeparam name="T">Attribute</typeparam>
		/// <param name="provider">Custom attributes provider</param>
		/// <param name="inherit">When true, look up the hierarchy chain for custom attribute </param>
		/// <returns></returns>
		public static T[] GetCustomAttributes<T>(this ICustomAttributeProvider provider, bool inherit) where T : Attribute
		{
			if (provider == null)
				throw new ArgumentNullException("provider");
			T[] attributes = provider.GetCustomAttributes(typeof(T), inherit) as T[];
			if (attributes == null)
			{
				return new T[0];
			}
			return attributes;
		}

		/// <summary>
		/// Gets a single or the first custom attribute of specified type
		/// </summary>
		/// <typeparam name="T">Attribute</typeparam>
		/// <param name="memberInfo">Custom Attribute provider</param>
		/// <param name="inherit">True to lookup the hierarchy chain for the attribute</param>
		/// <returns></returns>
		public static T GetSingleAttribute<T>(this ICustomAttributeProvider memberInfo,bool inherit=true) where T : Attribute
		{
            if (memberInfo == null) throw new ArgumentNullException("memberInfo");
			var list = memberInfo.GetCustomAttributes(typeof(T), inherit);
			if (list.Length > 0) return (T)list[0];
			return null;
		}	
	
		public static bool HasCustomAttribute<T>(this ICustomAttributeProvider mi,bool inherit=true) where T:Attribute
		{
			return mi.GetSingleAttribute<T>(inherit) != null;
		}
        
        public static bool HasCustomAttribute<T>(this ICustomAttributeProvider mi,Func<T,bool> condition,bool inherit=true) where T:Attribute
        {
            var a = mi.GetSingleAttribute<T>(inherit);
            if (a != null)
            {
                return condition(a);
            }
            return false;
        }
	}
}