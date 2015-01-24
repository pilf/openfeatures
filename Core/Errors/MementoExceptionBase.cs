namespace Opentable.Features.Core.Errors
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public abstract class MementoExceptionBase : Exception
	{
		protected MementoExceptionBase(string message) : base(message)
		{
		}

		protected static string BuildNotFoundMessage(IEnumerable<string> keys, string fullKey)
		{
			return String.Format(
				"Could not find key type {0}.  Known keys: [{1}]",
				fullKey,
				RenderAllKeys(keys));
		}

		protected static object RenderAllKeys(IEnumerable<string> keys)
		{
			return keys.Aggregate((j, i) => j + "," + i);
		}
	}
}