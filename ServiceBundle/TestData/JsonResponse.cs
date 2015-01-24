using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using OpenTable.Features.Core.Interfaces;

namespace OpenTable.Features.ServiceBundle.TestData
{
    public class JsonResponse<T> : IJsonResponse
        where T: IBasicEnvironmentSettings
    {
        public JsonResponse(T  envSettings)
        {
            EnvSettings = envSettings;
        }

        protected T EnvSettings { get; private set; }

        protected string ResolveVariables(string tokenString, Dictionary<string, string> variableReplacements)
        {
            return variableReplacements.Aggregate(
                    tokenString, 
                    (s, a) => s.Replace(string.Format("(|{0}|)", a.Key), a.Value));
        }

        public string ResolveVariables(string inputJson)
        {
            return ComputationToken(ResolveVariables(inputJson, VariableSubstitutions()));
        }

        private string ComputationToken(string tokenString)
        {
            return ComputationalTokens().Aggregate(
                    tokenString, 
                    (s, a) => s.Replace(string.Format("<|{0}|>", a.Key), a.Value()));
        }

        protected virtual Dictionary<string, string> VariableSubstitutions()
        {
            return new Dictionary<string, string>
            {
                { "BaseUrl", EnvSettings.BaseUrl }
            };
        }

        protected virtual Dictionary<string, Func<string>> ComputationalTokens()
        {
            return new Dictionary<string, Func<string>> 
                {
                    { "Any string", () => ".*"},
                    { "Alphanumeric", () => ".*"}
                };
        }


        public string Minify(string inputJson)
        {
            return Regex.Replace(inputJson, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
        }
    }
}
