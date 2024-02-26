using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreNote.Common.Helper
{
	public static class DistinctHelper
	{
		static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			var identifiedKeys = new HashSet<TKey>();
			return source.Where(element => identifiedKeys.Add(keySelector(element)));
		}
	}
}
