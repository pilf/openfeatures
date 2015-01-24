namespace OpenTable.Features.Core.Interfaces
{
    public interface ISiteGenerator<TEnv> where TEnv : IBasicEnvironmentSettings
    {
	    IOtSite<TEnv> Generate(IScenarioContext context, TEnv environment);
    }
}