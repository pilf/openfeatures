using OpenTable.Features.Core.Interfaces;

namespace OpenTable.Features.ServiceBundle.TestData
{
    public interface IJsonResponse : ITestDataObject 
    {
        string ResolveVariables(string inputJson);

        string Minify(string inputJson);
    }
}