namespace OpenTable.Features.Core.Interfaces
{
	public interface IOtSite<out TEnv> : ICoreComponents<TEnv>
            where TEnv : IBasicEnvironmentSettings
	{
		IOtBrowser Browser { get; }

		T Page<T>() where T : IPageObject;

		IPageObject CurrentPageWillBeEither<T, U>() 
			where T: IPageObject
			where U: IPageObject;

		T WaitForPage<T>() where T : IPageObject;
		T AssertPageIsNow<T>() where T : IPageObject;
	}
}