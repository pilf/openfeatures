using System.Net;

namespace OpenTable.Features.Core.ProductionProtection
{
	public interface IProductionProtectionConfigurationReader
	{
		IPAddress[] GetAllowedIpAddresses();
	}
}