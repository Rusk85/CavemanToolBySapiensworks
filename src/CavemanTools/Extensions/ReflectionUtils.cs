using System.Diagnostics;

namespace System.Reflection
{
	public static class ReflectionUtils
	{
		/// <summary>
		/// Used for checking if a class implements an interface
		/// </summary>
		/// <typeparam name="T">Interface</typeparam>
		/// <param name="type">Class Implementing the interface</param>
		/// <returns></returns>
		public static bool Implements<T>(this Type type)
		{
			return (typeof(T)).IsAssignableFrom(type);
		}

		/// <summary>
		/// Returns true if object is specifically of type. 
		/// Use "is" operator to check if an object is an instance of a type that derives from type.
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="o">Object</param>
		/// <returns></returns>
		public static bool IsExactlyType<T>(this object o)
		{
			return (o.GetType() == typeof(T));
		}

		/// <summary>
		/// Used for resource localizing
		/// </summary>
		/// <param name="type"></param>
		/// <exception cref="ArgumentException">If property was not found</exception>
		/// <param name="propertyName">Public static property name</param>
		/// <returns></returns>
		public static T GetStaticProperty<T>(this object type, string propertyName)
		{
			var tp = type.GetType().GetProperty(propertyName, BindingFlags.Static);
			if (tp == null) throw new ArgumentException("Property doesn't exist.", "propertyName");
			return tp.GetValue(null, null).ConvertTo<T>();
		}

		/// <summary>
		/// Gets the value of a property
		/// </summary>
		/// <typeparam name="T">Type of property value</typeparam>
		/// <param name="object">Object to get value from</param>
		/// <param name="propertyName">Property name</param>
		/// <returns></returns>
		public static T GetPropertyValue<T>(this object @object,string propertyName)
		{
			return GetPropertyValue(@object,propertyName).ConvertTo<T>();
		}

		/// <summary>
		/// Gets the value of a public property
		/// </summary>
		/// <param name="object">Object to get value from</param>
		/// <param name="propertyName">Public property name</param>
		/// <returns></returns>
		public static object GetPropertyValue(this object @object, string propertyName)
		{
			if (@object == null) throw new ArgumentNullException("@object");
			var tp = @object.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.IgnoreCase | BindingFlags.Public);
			if (tp == null) throw new ArgumentException("Property doesn't exist.", "propertyName");
			return tp.GetValue(@object, null);
		}

        

		/// <summary>
		/// Gets the file version of current executing assembly
		/// </summary>
		/// <returns></returns>
		public static string CurrentFileVersion()
		{
			return FileVersionInfo.GetVersionInfo(Assembly.GetCallingAssembly().Location).FileVersion;
		}

		  /// <summary>
		  /// Gets the assembly version
		  /// </summary>
		  /// <returns></returns>
		public static Version CurrentAssemblyVersion()
		{
			return Assembly.GetCallingAssembly().GetName().Version;
		}

        /// <summary>
        /// Returns the full name of type, including assembly, i.e: namespace.type, assembly
        /// </summary>
        /// <param name="t">Type</param>
        /// <returns></returns>
        public static string GetFullTypeName(this Type t)
        {
            if (t == null) throw new ArgumentNullException("t");
            return string.Format("{0}, {1}", t.FullName, Assembly.GetAssembly(t).GetName().Name);
        }
	}
}