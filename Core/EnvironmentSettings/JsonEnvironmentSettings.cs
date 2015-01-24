using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using OpenTable.Features.Core.Interfaces;

namespace OpenTable.Features.Core.EnvironmentSettings
{
    public class JsonEnvironmentSettings : IConsumerSiteEnvironment
    {
        public JsonEnvironmentSettings(string configurationName, string jsonString)
        {
            var configuration = JObject.Parse(jsonString);

            if (configuration[configurationName]["environment"] != null)
                Environment = configuration[configurationName]["environment"].ToString();

            if (configuration[configurationName]["webdbConnStr"] != null)
                WebDbConnectionString = configuration[configurationName]["webdbConnStr"].ToString();
            
            if (configuration[configurationName]["weblogdbConnStr"] != null)
                WebLogDbConnectionString = configuration[configurationName]["weblogdbConnStr"].ToString();

            if (configuration[configurationName]["remoteWebDriverAddress"] != null)
                RemoteWebDriverAddress = configuration[configurationName]["remoteWebDriverAddress"].ToString();

            if (configuration[configurationName]["availabilitySearchServiceAgentUri"] != null)
                AvailabilitySearchServiceAgentUri = configuration[configurationName]["availabilitySearchServiceAgentUri"].ToString();

            BaseUrl = configuration[configurationName]["baseUrl"].ToString();

            if (configuration[configurationName]["knownIpRanges"] != null)
                IpAddresses = JArray.Parse(configuration[configurationName]["knownIpRanges"].ToString()).Select(x => (string)x);

            int domainId;
            int.TryParse(configuration[configurationName]["domainId"].ToString(), out domainId);
            DomainId = domainId;

            if (configuration[configurationName]["demoLandVenuesOnly"] != null)
            {
                bool demoLandVenuesOnly;
                bool.TryParse(configuration[configurationName]["demoLandVenuesOnly"].ToString(), out demoLandVenuesOnly);
                DemoLandVenuesOnly = demoLandVenuesOnly;
            }
        }

        public string Environment { get; private set; }
        public string WebDbConnectionString { get; private set; }
        public string WebLogDbConnectionString { get; private set; }
        public string AvailabilitySearchServiceAgentUri { get; private set; }
        public string BaseUrl { get; private set; }
        public string RemoteWebDriverAddress { get; private set; }
        public IEnumerable<string> IpAddresses { get; private set; }
        public int DomainId { get; private set; }
        public bool DemoLandVenuesOnly { get; private set; }
    }
}