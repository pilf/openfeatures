using System.Collections.Generic;

namespace OpenTable.Features.Core.Interfaces
{
    public interface IComponentGenerator<TEnv> where TEnv: IBasicEnvironmentSettings
    {
        IList<IPageObject> BuildPageObjects(IOtSite<TEnv> site);
        IList<ITestDataObject> BuildTestModelObjects(IOtSite<TEnv> site);
        IList<IServiceObject> BuildServiceObjects(IOtSite<TEnv> site);
    }
}