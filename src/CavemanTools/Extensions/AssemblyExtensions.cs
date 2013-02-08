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
		/// <param name="instantiable">True to return only types that can be instantiated i.e no interfaces and no abstract classes</param>
		/// <returns></returns>
		public static IEnumerable<Type> GetTypesImplementing<T>(this Assembly asm,bool instantiable=false)
		{
			if (asm == null) throw new ArgumentNullException("asm");
			var res= asm.GetExportedTypes().Where(tp => (typeof (T)).IsAssignableFrom(tp));
            if (instantiable)
            {
                res = res.Where(t => !t.IsAbstract && !t.IsInterface);
            }
		    return res;
		}

		public static IEnumerable<Type> GetTypesWithAttribute<T>(this Assembly asm) where T:Attribute
		{
			if (asm == null) throw new ArgumentNullException("asm");
			return asm.GetExportedTypes().Where(a => a.HasCustomAttribute<T>());
		}
	}
}