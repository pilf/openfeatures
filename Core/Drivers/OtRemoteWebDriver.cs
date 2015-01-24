using System;
using System.Configuration;
using Coypu.Drivers;
using Coypu.Drivers.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenTable.Features.Core.EnvironmentSettings;
using OpenTable.Features.Core.Helpers;

namespace OpenTable.Features.Core.Drivers
{
	public class OtRemoteWebDriver : SeleniumWebDriver
	{
		public OtRemoteWebDriver(Browser browser)
			: base(new ScreenShotTakingRemoteWebDriver(
				   new Uri(GetRemoteWebDriverAddress()), 
				   DesiredCapabilities.Firefox()), 
				   browser)
		{
		}

		public OtRemoteWebDriver(Browser browser, ICapabilities capabilities)
			: base(new ScreenShotTakingRemoteWebDriver(capabilities), browser)
		{
		}

		public class ScreenShotTakingRemoteWebDriver : RemoteWebDriver, ITakesScreenshot
		{
			public ScreenShotTakingRemoteWebDriver(ICapabilities desiredCapabilities)
				: base(desiredCapabilities)
			{
			}

			public ScreenShotTakingRemoteWebDriver(Uri remoteAddress, ICapabilities desiredCapabilities)
				: base(remoteAddress, desiredCapabilities)
			{
			}

			public Screenshot GetScreenshot()
			{
				Response screenshotResponse = Execute(DriverCommand.Screenshot, null);
				var base64 = screenshotResponse.Value.ToString();
				return new Screenshot(base64);
			}
		}

		private static string GetRemoteWebDriverAddress()
		{
			var jsonString = EnvironmentConfigHelper.GetAsJsonString();
            var context = EnvironmentVariableHelper.Get("WA_EnvironmentConfiguraiton", ConfigurationManager.AppSettings["DefaultEnvironmentSettings"]);
			var envSettings = new JsonEnvironmentSettings(context, jsonString);

			if (envSettings.RemoteWebDriverAddress == null)
				throw new Exception("There is no RemoteWebDriverAddress setting for the current environment");

			return envSettings.RemoteWebDriverAddress;
		}
	}
}