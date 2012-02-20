using System.Collections.Generic;
using System.Linq;

namespace System.Reflection
{
	public static class AssemblyExtensions
	{
		/// <summary>
		/// Returns public types implementing T
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="asm"></param>
		/// <returns></returns>
		public static IEnumerable<Type> GetTypesImplementing<T>(this Assembly asm)
		{
			if (asm == null) throw new ArgumentNullException("asm");
			return asm.GetExportedTypes().Where(tp => (typeof (T)).IsAssignableFrom(tp));
		}

		public static IEnumerable<Type> GetTypesWithAttribute<T>(this Assembly asm) where T:Attribute
		{
			if (asm == null) throw new ArgumentNullException("asm");
			return asm.GetExportedTypes().Where(a => a.HasCustomAttribute<T>());
		}
	}
}