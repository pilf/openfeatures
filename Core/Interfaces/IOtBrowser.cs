namespace OpenTable.Features.Core.Interfaces
{
    public interface IOtBrowser
    {
        bool CurrentPageIs<T>() where T : IPageObject;
        bool BrowserUrlContains(string text);
        void WaitForPageToLoad();
        void VisitRootUrl();
        void MaximizeBrowserWindow();
        void ResizeBrowserWindow(int width, int height);
        void CloseBrowser();
        void TakeScreenShot(string folderName);
	    string CurrentBrowserUrl { get; }
    }
}