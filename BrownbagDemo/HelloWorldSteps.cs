using System;
using System.Collections.Generic;
using Coypu;
using Gherkin.Ast;
using Moq;
using OpenTable.Features.Core;
using OpenTable.Features.Core.Interfaces;
using Should;
using TechTalk.SpecFlow;

namespace BrownbagDemo
{
    [Binding]
    public class HelloWorldSteps : OtStepBase<OfSettings>
    {
        [When(@"I go to home page")]
        public void WhenIGoToHomePage()
        {
            Site.Page<HomePage>().Visit();
        }
        
        [Then(@"I will see some text")]
        public void ThenIWillSeeSomeText()
        {
            Site.Page<HomePage>().ClickProvenResults();
        }

        protected HelloWorldSteps() : base(new OfSettings(), new OfSiteGenerator()) 
        {
        }
    }

    public class OfSiteGenerator : ISiteGenerator<OfSettings>
    {
        public IOtSite<OfSettings> Generate(IScenarioContext context, OfSettings environment)
        {
            var browserSession = new Coypu.BrowserSession();
            return new CoypuOtSite<OfSettings>(
                browserSession,
                context,
                environment,
                new OfSiteDictionary(),
                new OfComponentGenerator(browserSession));
        }
    }

    public class OfComponentGenerator : IComponentGenerator<OfSettings>
    {
        private readonly BrowserSession _browserSession;

        public OfComponentGenerator(BrowserSession browserSession)
        {
            _browserSession = browserSession;
        }

        public IList<IPageObject> BuildPageObjects(IOtSite<OfSettings> site)
        {
            return new List<IPageObject> {new HomePage(site, _browserSession)};
        }

        public IList<ITestDataObject> BuildTestModelObjects(IOtSite<OfSettings> site)
        {
            return new List<ITestDataObject> { };
        }

        public IList<IServiceObject> BuildServiceObjects(IOtSite<OfSettings> site)
        {
            return new List<IServiceObject> { };
        }
    }

    public class OfSiteDictionary : ISiteDictionary
    {
        public string GetText(string textKey) => "not implemented";
    }

    public class HomePage : IPageObject
    {
        private readonly IOtSite<OfSettings> _site;
        private readonly BrowserSession _browserSession;

        public HomePage(IOtSite<OfSettings> site, BrowserSession browserSession)
        {
            _site = site;
            _browserSession = browserSession;
        }

        public bool IsCurrentPage() =>
            _site.Browser.CurrentBrowserUrl.Contains("guestline.com");

        public void Visit()
        {
            _browserSession.Visit("https://guestline.com");
        }

        public void ClickProvenResults()
        {
            var button = _browserSession.FindLink("Proven Results");
            button.Exists().ShouldBeTrue(); 
        }
    }


    public class OfSettings : IBasicEnvironmentSettings
    {
        public string BaseUrl { get; } = "https://www.google.com/";
        public IEnumerable<string> IpAddresses { get; } = new string[] { };
    }
}
