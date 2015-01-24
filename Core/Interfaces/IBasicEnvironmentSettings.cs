using System.Collections.Generic;

namespace OpenTable.Features.Core.Interfaces
{
	public interface IBasicEnvironmentSettings
	{
		string BaseUrl { get; }
		IEnumerable<string> IpAddresses { get; }
		int DomainId { get; }
	}
}