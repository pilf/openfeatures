namespace OpenTable.Features.Core.ProductionProtection
{
	public interface IProductionProtector
	{
		void VerifyDomainAddressIsResolvedToAnAllowedIp(string domainName);
	}
}