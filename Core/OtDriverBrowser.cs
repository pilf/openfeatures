using System;
using Coypu;
using OpenQA.Selenium.Remote;
using OpenTable.Features.Core.Interfaces;
using OpenTable.Features.Core.ScreenShotCapture;

namespace OpenTable.Features.Core
{
	public class OtDriverBrowser : IOtBrowser
	{
		public interface IDependencies
		{
			IPageObject ResolvePage<T>() where T : IPageObject;
		}

		private readonly IDependencies _dependencies;
		private readonly BrowserSession _browserSession;
		private readonly RemoteWebDriver _nativeDriver;
		private readonly IScenarioContext _scenarioContext;

        public OtDriverBrowser(IDependencies dependencies, BrowserSession browserSession, IScenarioContext scenarioContext, RemoteWebDriver nativeDriver)
		{
			_dependencies = dependencies;
			_browserSession = browserSession;
			_scenarioContext = scenarioContext;
            _nativeDriver = nativeDriver;
		}

		public bool CurrentPageIs<T>() where T : IPageObject
		{
			return _dependencies.ResolvePage<T>().IsCurrentPage();
		}

		public bool BrowserUrlContains(string text)
		{
			WaitForPageToLoad();
 
			return _browserSession.Location.AbsoluteUri.Contains(text);
		}

        public void MaximizeBrowserWindow()
	    {
            _browserSession.MaximiseWindow();
	    }

	    public void ResizeBrowserWindow(int width, int height)
        {
            _browserSession.ResizeTo(width, height);
        }

		public void WaitForPageToLoad()
		{
			_browserSession.TryUntil(() => { }, () => _browserSession.ExecuteScript("return document.readyState") == "complete", TimeSpan.FromSeconds(2), new Options { Timeout = TimeSpan.FromSeconds(30) });
		}

		public void VisitRootUrl()
		{
			_browserSession.Visit("");   
		}

		#region only used by OtScenarioStepBase
		public void CloseBrowser()
		{
			_browserSession.Dispose(); 
		}

	    public void TakeScreenShot(string folderName)
		{
			ScreenShot.TakeScreenshot(_scenarioContext, _browserSession.Driver, folderName);
		}

		public string CurrentBrowserUrl 
		{
			get { return _browserSession.Location.AbsoluteUri; }
		}
		#endregion
	}
}