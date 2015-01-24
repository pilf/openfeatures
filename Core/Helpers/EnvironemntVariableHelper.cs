using System;

namespace OpenTable.Features.Core.Helpers
{
    [Obsolete("Mis-spelt, please use EnvironmentVariableHelper")]
    public static class EnvironemntVariableHelper
    {
        public static string Get(string variableName, string defaultValue = null)
        {
            return EnvironmentVariableHelper.Get(variableName, defaultValue);
        }
    }
}