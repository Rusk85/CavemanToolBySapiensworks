using System.Collections.Generic;

namespace CavemanTools.Model
{
	/// <summary>
	/// Holds limited result set and total number of items from a query.
	/// Used for pagination.
	/// </summary>
	/// <typeparam name="T">Type of item</typeparam>
	public interface IResultSet<T>
	{
		/// <summary>
		/// Gets the total number of items
		/// </summary>
		int Count { get;}

		/// <summary>
		/// Gets result items
		/// </summary>
		IEnumerable<T> Items{ get;}	
	}
}