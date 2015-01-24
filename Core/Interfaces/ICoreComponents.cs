namespace OpenTable.Features.Core.Interfaces
{
    public interface ICoreComponents<out TEnv>
        where TEnv : IBasicEnvironmentSettings
    {
        string GetTranslation(string textKey);
        TEnv EnvironmentSettings { get; }
        T TestData<T>() where T : ITestDataObject;
        T Service<T>() where T : IServiceObject;
    }
}