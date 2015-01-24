using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace OpenTable.Features.Core.ProductionProtection
{
	public class TriedToAccessNotAllowedIpException : Exception
	{
		public TriedToAccessNotAllowedIpException(IEnumerable<IPAddress> allowedIpAddresses, IEnumerable<IPAddress> accessedIpAddresses, string accessedDomain)
			: base(String.Format(
				"Tests tried to access not allowed ip address. Allowed addresses: {0}. Accessed addresses: {1}. Accessed domain: {2}. " +
				"Try changing your system hosts file entries for accessed domain name or updating local dns " +
				"cache with 'ipconfig.exe /flushdns' command.",
				string.Join(",",allowedIpAddresses.Select(ip => ip.ToString())),
				string.Join(",",accessedIpAddresses.Select(ip => ip.ToString())),
				accessedDomain))
		{
		}
	}
}