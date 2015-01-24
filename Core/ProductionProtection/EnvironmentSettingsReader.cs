using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace OpenTable.Features.Core.ProductionProtection
{
	public class EnvironmentSettingsReader : IProductionProtectionConfigurationReader
	{
		private readonly IPAddress[] _allowedIps;

		public EnvironmentSettingsReader(IEnumerable<string> allowedIps)
		{
			try
			{
				if (allowedIps != null)
                    _allowedIps = allowedIps.Select(IPAddress.Parse).ToArray();
			}
			catch (Exception ex)
			{
			    var providedAllowedIpValue = _allowedIps != null ? _allowedIps.ToString() : "<null>";
				throw new Exception(
					string.Format("Could not parse the IP addresses:  This is the JSON string used: {0} ", providedAllowedIpValue)
						+ "-- note it should be something like [ '127.0.0.1', '10.10.10.10' ]",
					ex);
			}
		}

		public IPAddress[] GetAllowedIpAddresses()
		{
			return _allowedIps;
		}
	}
}