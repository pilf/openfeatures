namespace OpenTable.Features.Core.Interfaces
{
    public interface IScenarioContext
    {
        string CurrentScenarioTitle();
        string CurrentFeatureTitle();
	    bool CurrentScenarioHasTag(string tag);
    }
}