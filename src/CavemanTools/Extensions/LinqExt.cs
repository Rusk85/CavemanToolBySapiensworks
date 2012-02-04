using System.Collections.Generic;


namespace System.Linq
{
	public static class LinqExt
	{
	
		/// <summary>
		/// Executes function for each sequence item
		/// </summary>
		/// <typeparam name="TSource">Sequence</typeparam>
		/// <param name="source">Function to execute</param>
		/// <returns></returns>
		public static void ForEach<TSource>(this IEnumerable<TSource> source,Action<TSource> action
			) 
		{
			if (source == null) throw new ArgumentNullException("source");
			if (action == null) throw new ArgumentNullException("action");
			foreach (var b in source)
			{
				action(b);
			}
		}
	}
}