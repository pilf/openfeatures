using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace OpenTable.Features.Core.Helpers
{
    public static class EnvironmentConfigHelper
    {
        public static string GetAsJsonString()
        {
            var executingPathUrl = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            Debug.Assert(executingPathUrl != null, "executingPathUrl != null");
            var executingPath = (new Uri(executingPathUrl)).LocalPath;
            var jsonString = File.ReadAllText(Path.Combine(executingPath, "EnvironmentSetup.json"));
            return jsonString;
        }
    }
}