using Coypu;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using OpenQA.Selenium.Remote;
using OpenTable.Features.Core.Interfaces;

namespace OpenTable.Features.Core
{
	public class CoypuOtSite<TEnv> : IOtSite<TEnv>, OtDriverBrowser.IDependencies where TEnv: IBasicEnvironmentSettings
    {
        public class UnexpectedPageError<T> : Exception
        {
            public UnexpectedPageError(CoypuOtSite<TEnv> coypuOtSite)
                : base(GenerateMessage(coypuOtSite))
            {
            }

            private static string GenerateMessage(CoypuOtSite<TEnv> site)
            {
                var expectedMessage = string.Format("Expected the page to be {0}, but wasn't.  ", typeof(T).Name);

                try
                {
                    var currentPage = site.DetermineCurrentPage();
                    return string.Format(expectedMessage + "Instead found page of type {0} with URL of {1}", currentPage.GetType().Name, site.Browser.CurrentBrowserUrl);
                }
                catch (UnableToDetermineCurrentPageError)
                {
                    return string.Format(expectedMessage + "No known page object admits to being the current page.");
                }
                catch(UnableToDecideCurrentPageError e)
                {
                    return string.Format(expectedMessage + e.Message);
                }
            }
        }

        private class UnableToDecideCurrentPageError : Exception
        {
            public UnableToDecideCurrentPageError(IEnumerable<IPageObject> pagesClaimingToBeCurrent)
                : base(GenerateMessage(pagesClaimingToBeCurrent))
            {
            }

            private static string GenerateMessage(IEnumerable<IPageObject> pagesClaimingToBeCurrent)
            {
                string pageTypes = pagesClaimingToBeCurrent
                        .Select(s => s.GetType().Name)
                        .Aggregate((c, n) => c + ", " + n);
                
                const string message = "Multiple pages claim to be current, namely: ({0}), so cannot decide current page!";
                
                return string.Format(message, pageTypes);
            }
        }

        private class UnableToDetermineCurrentPageError : Exception
        {
        }

        private readonly TEnv _envSettings;
        private readonly IList<IPageObject> _knownPageObjects;
        private readonly IList<ITestDataObject> _knownModelObjects;
        private readonly IList<IServiceObject> _knownServiceObjects;
	    private readonly ISiteDictionary _phrases;

        public CoypuOtSite(BrowserSession browserSession, IScenarioContext scenarioContext, TEnv envSettings, ISiteDictionary phrases, IComponentGenerator<TEnv> componentGenerator)
        {
            _envSettings = envSettings;
            _phrases = phrases;

            var nativeDriver = ((RemoteWebDriver)browserSession.Native);
            Browser = new OtDriverBrowser(this, browserSession, scenarioContext, nativeDriver);

            _knownPageObjects = componentGenerator.BuildPageObjects(this); 
            _knownModelObjects = componentGenerator.BuildTestModelObjects(this); 
            _knownServiceObjects = componentGenerator.BuildServiceObjects(this);
        }

        public string GetTranslation(string textKey)
        {
            return _phrases.GetText(textKey);
        }

        public TEnv EnvironmentSettings
        {
           get { return _envSettings; }
        }

        public T TestData<T>() where T : ITestDataObject
        {
            try
            {
                return (T)_knownModelObjects.First( s => typeof(T).IsAssignableFrom(s.GetType()));
            }
            catch (Exception)
            {
                throw new ArgumentException(String.Format("Don't know how to make model object of type '{0}'", typeof(T).Name));
            }
        }

        public T Page<T>() where T : IPageObject
        {
            try
            {
                return (T)_knownPageObjects.First(s => s.GetType().Name == typeof(T).Name);
            }
            catch (Exception)
            {
                throw new ArgumentException(String.Format("Don't know how to make page object of type '{0}'", typeof(T).Name));
            }
        }

        public T Service<T>() where T : IServiceObject
        {
            try
            {
                return (T)_knownServiceObjects.First(s => s.GetType().Name == typeof(T).Name);
            }
            catch (Exception)
            {
                throw new ArgumentException(String.Format("Don't know how to make service object of type '{0}'", typeof(T).Name));
            }
        }

        public IOtBrowser Browser { get; private set; }

        public IPageObject CurrentPageWillBeEither<T, U>() 
            where T: IPageObject
            where U: IPageObject
        {
            Browser.WaitForPageToLoad();

            if(Browser.CurrentPageIs<T>())
                return Page<T>();
            if(Browser.CurrentPageIs<U>())
                return Page<U>();
            throw new Exception(String.Format("Expected current page to be either {0} or {1}, it wasn't", typeof(T).Name, typeof(U).Name));
        }

        public T WaitForPage<T>() where T : IPageObject
        {
            IPageObject page = Page<T>();

            var i = 0;

            while (i <= 25 && !page.IsCurrentPage())
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                i++;
            }

            if (!page.IsCurrentPage())
                throw new UnexpectedPageError<T>(this);

            return (T)page;
        }

        public T AssertPageIsNow<T>() where T : IPageObject
        {
            Browser.WaitForPageToLoad();
            var page = Page<T>();
            return page.AssertPageIsNow<T>();
        }

        #region Private

        private IPageObject DetermineCurrentPage()
        {
            var pagesClaimingToBeCurrent = _knownPageObjects.Where(SafeIsCurrentPage).ToList();
            switch (pagesClaimingToBeCurrent.Count())
            {
                case 0: throw new UnableToDetermineCurrentPageError();
                case 1: return pagesClaimingToBeCurrent.Single();
                default: throw new UnableToDecideCurrentPageError(pagesClaimingToBeCurrent);
            }
        }

        private static bool SafeIsCurrentPage(IPageObject page)
        {
            try
            {
                return page.IsCurrentPage();
            }
            catch (Exception)
            {
                return false;
            }
        }
#endregion

#region Browser dependencies
        public IPageObject ResolvePage<T>() where T : IPageObject
        {
            return Page<T>();
        }
#endregion
    }
}