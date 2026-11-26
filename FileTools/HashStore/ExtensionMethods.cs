using System.Collections.Generic;

namespace FileTools.HashStore
{
    public static class ExtensionMethods
	{
		public static string ToCommaSeparated(this IEnumerable<string> list)
		{
			return string.Join(",", list);
		}
	}
}
