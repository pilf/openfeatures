using System;
using System.Linq;
using OpenTable.Features.Core.Interfaces;
using OpenTable.Features.Core.ProductionProtection;
using TechTalk.SpecFlow;

using Opentable.Features.Core.Errors;

namespace OpenTable.Features.Core
{
	public class OtStepBase<TEnv> : Steps, IScenarioContext where TEnv: IBasicEnvironmentSettings
	{
		private readonly TEnv _environmentSettings;
		private readonly ISiteGenerator<TEnv> _siteGenerator;
		private const string OtSiteKey = "OTSiteKey";
		private const string HasErrorKey = "HasErrorKey";
		private const string HasScreenShotKey = "HasScreenShotKey";

		protected OtStepBase(TEnv environmentSettings, ISiteGenerator<TEnv> siteGenerator)
		{
			_environmentSettings = environmentSettings;
			_siteGenerator = siteGenerator;
		}

        [AfterScenario]
		public void AfterScenario()
		{
			if (ScenarioContext.Current.ContainsKey(OtSiteKey))
			{
				ScenarioContext.Current.Get<CoypuOtSite<TEnv>>(OtSiteKey).Browser.CloseBrowser();
				ScenarioContext.Current.Remove(OtSiteKey);
			}
		}

		[AfterStep]
		public void AfterStep()
		{
			if (IsFirstStepDefinitionError())
			{
				ScenarioContext.Current.Add(HasErrorKey, "true");
			}

			if (IsInErrorWithoutAScreenShot())
			{
				try
				{
                    Console.WriteLine("Site.Browser.CurrentBrowserUrl.Equals('{0}')", Site.Browser.CurrentBrowserUrl);

                    Site.Browser.TakeScreenShot("testErrors");
					ScenarioContext.Current.Add(HasScreenShotKey, "true");
				}
				catch (Exception)
				{
				    //Console.WriteLine("An exception was caught while trying to take a screenshot of the failed test");
				}
			}
		}

	    [AfterStep("pdiff")]
	    public void PDiff()
	    {
	        try
	        {
	            Site.Browser.TakeScreenShot("pDiff");
	        }
	        catch{}
	    }

	    public IOtSite<TEnv> Site
		{
			get
			{
				if (ScenarioContext.Current.ContainsKey(OtSiteKey))
				{
					return ScenarioContext.Current.Get<CoypuOtSite<TEnv>>(OtSiteKey);
				}

			    CheckSiteProtector();

			    var otSite = _siteGenerator.Generate(this, _environmentSettings);

			    ScenarioContext.Current.Set(otSite, OtSiteKey);

			    return otSite;
			}
		}

		private void CheckSiteProtector()
		{
			var productionProtection = new ProductionProtector(new EnvironmentSettingsReader(_environmentSettings.IpAddresses));

			Uri uri;
			try
			{
				uri = new Uri(_environmentSettings.BaseUrl);
			}
			catch (Exception ex)
			{
				throw new Exception(
					string.Format("Problem trying to parse the host from given base url '({0})'", _environmentSettings.BaseUrl), ex);
			}

			productionProtection.VerifyDomainAddressIsResolvedToAnAllowedIp(uri.Host);
		}

		private static string KeyFromType<T>(string tag)
		{
			return String.Format("{0}*{1}", typeof(T).Name, tag ?? String.Empty);
		}

		private static bool IsInErrorWithoutAScreenShot()
		{
			return ScenarioContext.Current.ContainsKey(HasErrorKey) && !ScenarioContext.Current.ContainsKey(HasScreenShotKey);
		}

		private static bool IsFirstStepDefinitionError()
		{
			return ScenarioContext.Current.TestError != null && !ScenarioContext.Current.ContainsKey(HasErrorKey);
		}

		protected void Remember<T>(T thingToRemember)
		{
			Remember(thingToRemember, null);
		}

		protected void Remember<T>(T thingToRemember, string tag)
		{
			if (!ScenarioContext.Current.ContainsKey(KeyFromType<T>(tag)))
			{
				ScenarioContext.Current.Add(KeyFromType<T>(tag), thingToRemember);
                Console.WriteLine("[Debug Info] Remembering: {0} With Value: {1}", tag, thingToRemember);
			}
			else
			{
				throw new RememberKeyAlreadyExistsError(KeyFromType<T>(tag));
			}
		}

		protected T Recall<T>()
		{
			return Recall<T>(null);
		}

		protected T Recall<T>(string tag)
		{
			if (ScenarioContext.Current.ContainsKey(KeyFromType<T>(tag)))
			{
				return (T)ScenarioContext.Current[KeyFromType<T>(tag)];
			}
			throw new UnknownKeyRecallError(ScenarioContext.Current.Keys, KeyFromType<T>(tag));
		}

		public static void Forget<T>()
		{
			Forget<T>(null);
		}

		public static void Forget<T>(string tag)
		{
			if (ScenarioContext.Current.ContainsKey(KeyFromType<T>(tag)))
			{
				ScenarioContext.Current.Remove(KeyFromType<T>(tag));
			}
			else
			{
				throw new UnknownKeyForgetError(ScenarioContext.Current.Keys, KeyFromType<T>(tag));
			}
		}

		public string CurrentScenarioTitle()
		{
			return ScenarioContext.Current.ScenarioInfo.Title;
		}

		public string CurrentFeatureTitle()
		{
			return FeatureContext.Current.FeatureInfo.Title;
		}

		public bool CurrentScenarioHasTag(string tag)
		{
			var loweredTag = tag.ToLowerInvariant();
			return ScenarioContext.Current.ScenarioInfo.Tags.Any(a => a.ToLowerInvariant() == loweredTag);
		}
	}
}