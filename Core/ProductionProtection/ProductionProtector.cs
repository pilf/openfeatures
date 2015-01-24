using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace OpenTable.Features.Core.ProductionProtection
{
	public class ProductionProtector : IProductionProtector
	{
		private readonly IPAddress[] _allowedIpAddresses;

		public ProductionProtector(IProductionProtectionConfigurationReader productionProtectionConfigurationReader)
		{
			_allowedIpAddresses = productionProtectionConfigurationReader.GetAllowedIpAddresses();
		}

		private bool IsDnsPointingToProduction(IEnumerable<IPAddress> accessedIpAddresses)
		{
		    return _allowedIpAddresses != null && accessedIpAddresses.Any(ip => !_allowedIpAddresses.Contains(ip) );
		}

	    public void VerifyDomainAddressIsResolvedToAnAllowedIp(string domainName)
		{
			IPAddress[] accessedIpAddresses;
			try
			{
				accessedIpAddresses = Dns.GetHostAddresses(domainName);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Unable to resolve the host name '{0}'", domainName), ex);
			}

			if (IsDnsPointingToProduction(accessedIpAddresses))
			{
				throw new TriedToAccessNotAllowedIpException(_allowedIpAddresses, accessedIpAddresses, domainName);
			}
		}
	}
}