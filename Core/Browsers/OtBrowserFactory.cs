using System;
using System.Configuration;
using Coypu.Drivers;

namespace OpenTable.Features.Core.Browsers
{
    public static class OtBrowserFactory
	{
        private const string EnvironmentVarTestBrowser = "Browser";

        public static Browser GetBrowser()
        {
            var browserSetting = Environment.GetEnvironmentVariable(EnvironmentVarTestBrowser) ??
                                    ConfigurationManager.AppSettings["Browser"];

            if (browserSetting.ToLower().Equals("firefox"))
			    return Browser.Firefox;
            if (browserSetting.ToLower().Equals("chrome"))
                return Browser.Chrome;
            if (browserSetting.ToLower().Equals("internetexplorer"))
                return Browser.InternetExplorer;
            throw new Exception(string.Format("Unspecified Browser: {0}", browserSetting));
		}
	}
}