using System;

namespace OpenTable.Features.Core.Helpers
{
    public static class EnvironmentVariableHelper
    {
        public static string Get(string variableName, string defaultValue = null)
        {
            var result = Environment.GetEnvironmentVariable(variableName) ?? defaultValue;

            if (result == null)
            {
                var message = string.Format("Tried to access environment variable '{0}' but was not found, and no default was provided", variableName);
                throw new Exception(message);
            }
            return result;
        }
    }
}