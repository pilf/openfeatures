namespace OpenTable.Features.Core.Interfaces
{
    public interface IConsumerSiteEnvironment : IBasicEnvironmentSettings
    {
        string Environment { get; }
        string WebDbConnectionString { get; }
        string WebLogDbConnectionString { get; }
        string AvailabilitySearchServiceAgentUri { get; }
        string RemoteWebDriverAddress { get; }
        bool DemoLandVenuesOnly { get; }
    }
}