using System.Collections.Generic;

namespace CavemanTools.Model
{
	/// <summary>
	/// Holds limited result set and total number of items from a query.
	/// Used for pagination.
	/// </summary>
	/// <typeparam name="T">Type of item</typeparam>
	public class ResultSet<T>:IResultSet<T>
	{
		public int Count
		{
			get; set;
		}

	    public ResultSet()
	    {
	        Items = new T[0];
	    }

		public IEnumerable<T> Items
		{
			get; set;
		}
	}
}