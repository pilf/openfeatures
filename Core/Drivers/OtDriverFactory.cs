using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using Coypu.Drivers.Selenium;
using Coypu.Drivers.Watin;

namespace OpenTable.Features.Core.Drivers
{
    public static class OtDriverFactory
    {
        private const string EnvironmentVarUseRemoteWebDriver = "UseRemoteWebDriver";

        private const string EnvironmentVarUseDriver = "Driver";

        private static readonly IDictionary<string, Type> KnownDrivers = new Dictionary<string, Type>
                {  
                    { "selenium", typeof(SeleniumWebDriver) },
                    { "watin", typeof(WatiNDriver) },
                    { "remote", typeof(OtRemoteWebDriver) }
                };

        public static Type GetWebDriverType()
        {
            if (UseRemote()) return typeof (OtRemoteWebDriver);

            var driverSetting = DriverSettings();

            try
            {
                return KnownDrivers[DriverSettings().ToLower()];
            }
            catch
            {
                const string Message = "Unspecified or unknown driver: '{0}'.  You need to setup either an environment variable "
                    + "named {1} or else an appsetting of the same name and it must be set to one of the supported drivers: [{2}].";

                var knownDriversAsString = KnownDrivers.Keys.Aggregate((s, a) => string.Format("'{0}', '{1}'", a, s));
                throw new Exception(string.Format(Message, driverSetting, EnvironmentVarUseDriver, knownDriversAsString));
            }
        }

        private static string DriverSettings()
        {
            return Environment.GetEnvironmentVariable(EnvironmentVarUseDriver) 
                ?? ConfigurationManager.AppSettings["Driver"];
        }

        private static bool UseRemote()
        {
            var useRemoteSettings = Environment.GetEnvironmentVariable(EnvironmentVarUseRemoteWebDriver) 
                    ?? ConfigurationManager.AppSettings["UseRemoteWebDriver"];

            try
            {
                return (useRemoteSettings != null) ? bool.Parse(useRemoteSettings) : false;
            }
            catch (InvalidCastException)
            {
                throw new Exception(string.Format(
                        "Value found for UseRemoteSettings was given as '{0}', but failed to convert to boolean.",
                        useRemoteSettings));
            }
        }
    }
}