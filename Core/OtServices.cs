using System;

using OpenTable.Features.Core.Interfaces;

namespace OpenTable.Features.Core
{
	public class OtServices<TEnv> : ICoreComponents<TEnv> where TEnv: IBasicEnvironmentSettings
    {
	    public string GetTranslation(string textKey)
	    {
	        throw new NotImplementedException();
	    }

	    public TEnv EnvironmentSettings { get; private set; }

	    public T TestData<T>() where T : ITestDataObject
	    {
	        throw new NotImplementedException();
	    }

	    public T Service<T>() where T : IServiceObject
	    {
	        throw new NotImplementedException();
	    }
    }
}
