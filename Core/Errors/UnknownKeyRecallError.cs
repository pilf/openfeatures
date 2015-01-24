namespace Opentable.Features.Core.Errors
{
	using System.Collections.Generic;

	public class UnknownKeyRecallError : MementoExceptionBase
	{
		public UnknownKeyRecallError(IEnumerable<string> knownKeys, string fullKey) : base(BuildNotFoundMessage(knownKeys, fullKey))
		{
		}
	}
}