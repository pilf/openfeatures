namespace Opentable.Features.Core.Errors
{
	using System.Collections.Generic;

	public class UnknownKeyForgetError : MementoExceptionBase
	{
		public UnknownKeyForgetError(IEnumerable<string> knownKeys, string fullKey) : base(BuildNotFoundMessage(knownKeys, fullKey))
		{
		}
	}
}