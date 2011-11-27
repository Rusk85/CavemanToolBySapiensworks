using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using CavemanTools.Reflection;

namespace CavemanTools.Extensions
{
	public static class ObjectExtend
	{

		/// <summary>
		/// Creates dictionary from object properties.
		/// </summary>
		/// <param name="value">Object</param>
		/// <returns></returns>
		public static IDictionary<string,object> ToDictionary(this object value)
		{
			return value.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(value, null));			
		}
		
		/// <summary>
		/// Generates a string containing the properties and values of the object
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public static string ToDebugString(this object val)
		{
			if (val == null) return string.Empty;
			var sb = new StringBuilder();
			var dict=val.ToDictionary();
			dict.ForEach(
				kv => sb.AppendFormat("{0}={1}\n", kv.Key, kv.Value));
			return sb.ToString();
		}

		/// <summary>
		/// Creates destination object and copies source. Use CopyOptionsAttribute to mark the properties you want ignored.
        /// Use Automapper for heavy duty mapping
        /// </summary>
		/// <seealso cref="CopyOptionsAttribute"/>
		/// <typeparam name="T">Destination type must have parameterless constructor</typeparam>
		/// <param name="source">Object to copy</param>
		public static T CopyTo<T>(this object source) where T :class, new() 
		{
			var destination = new T();
			source.CopyTo(destination);
			return destination;
		}


		/// <summary>
		/// Copy source object into destination. For ocasional use.
		/// Use Automapper for heavy duty mapping
		/// </summary>
		/// <exception cref="ArgumentNullException">If source or destinaiton are null</exception>
		/// <typeparam name="T">Destination Type</typeparam>
		/// <param name="source">Object to copy from</param>
		/// <param name="destination">Object to copy to. Unmatching or read-only properties are ignored</param>
		public static void CopyTo<T>(this object source, T destination) where T : class
		{
			if (source == null) throw new ArgumentNullException("source");
			if (destination == null) throw new ArgumentNullException("destination");
			var srcType = source.GetType();
			var attr = destination.GetType().GetSingleAttribute<CopyOptionsAttribute>();
			if (attr != null)
			{
				if (attr.IgnoreProperty) ;
			}

			foreach (var destProperty in destination.GetType().GetProperties(BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.FlattenHierarchy))
			{
				if (!destProperty.CanWrite) continue;

				var pSource = srcType.GetProperty(destProperty.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);
				if (pSource == null) continue;
				var o = pSource.GetValue(source, null);
				if (!pSource.PropertyType.Equals(destProperty.PropertyType))
				{
					o = ConvertTo(o, destProperty.PropertyType);
				}
				destProperty.SetValue(destination, o, null);
			}
		}

		
		/// <summary>
		/// Converts object to type.
		/// Supports conversion to Enum, DateTime,TimeSpan
		/// </summary>
		/// <exception cref="InvalidCastException"></exception>
		/// <param name="data">Object to be converted</param>
		/// <param name="tp">Type to convert to</param>
		/// <returns></returns>
		public static object ConvertTo(this object data, Type tp)
		{
			if (data == null) throw new ArgumentNullException("data");
			if (tp.IsEnum)
			{
				if (data.GetType().FullName == "System.String")
				{
					return Enum.Parse(tp, data.ToString());
				}
				var o = Enum.ToObject(tp, data);
				return o;
			}

			if (tp.IsValueType)
			{
				if (tp == typeof(TimeSpan))
				{
					return TimeSpan.Parse(data.ToString().Trim());					
				}

				if (tp == typeof(DateTime))
				{
					data = data.ToString().Trim();
				}
			}
			if (tp == typeof(CultureInfo)) return new CultureInfo(data.ToString());
			return System.Convert.ChangeType(data, tp);
		}

		/// <summary>
		///	Tries to convert the object to type.
		/// If it fails it throws an exception
		/// </summary>
		/// <exception cref="InvalidCastException"></exception>
		/// <exception cref="FormatException"></exception>
		/// <typeparam name="T">Type to convert to</typeparam>
		/// <param name="data">Object</param>
		/// <returns></returns>
		public static T ConvertTo<T>(this object data)
		{
			var tp = typeof(T);			
			var temp = (T)ConvertTo(data, tp);
			return temp;			
		}

		

		/// <summary>
		///	 Tries to convert the object to type.
		/// If it fails it returns the default of type.
		/// </summary>
		/// <typeparam name="T">Type to convert to</typeparam>
		/// <param name="data">Object</param>
		/// <returns></returns>
		public static T SilentConvertTo<T>(this object data)
		{
			var tp = typeof (T);  
			try
			{
				var temp = (T) ConvertTo(data, tp);
				return temp;
		}
			catch (InvalidCastException)
			{
				return default(T);
			}			
		}


		public static T As<T>(this object o)where  T:class 
		{
			if (o == null) throw new ArgumentNullException("o");
			return o as T;
		}
		
	}
}								 