using System.Collections.Generic;
using OpenTable.Features.Core.Interfaces;

namespace OpenTable.Features.Core.EnvironmentSettings
{
	public class BasicEnvironmentSettings : IBasicEnvironmentSettings
	{
		public BasicEnvironmentSettings(string baseUrl, IEnumerable<string> allowedIpAddresses, int domainId)
		{
			BaseUrl = baseUrl;
			IpAddresses = allowedIpAddresses;
			DomainId = domainId;
		}

		public string BaseUrl { get; private set; }
		public IEnumerable<string> IpAddresses { get; private set; }
		public int DomainId { get; private set; }
	}
}