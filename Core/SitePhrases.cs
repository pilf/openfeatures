using System;
using Newtonsoft.Json.Linq;
using OpenTable.Features.Core.Interfaces;

namespace OpenTable.Features.Core
{
    public class SitePhrases : ISiteDictionary
    {
        private readonly int _domainId;
        private readonly JObject _jsonDatabase;

        public SitePhrases(int domainId, string phrasesDatabaseAsJsonString)
        {
            _domainId = domainId;
            _jsonDatabase = JObject.Parse(phrasesDatabaseAsJsonString);
        }

        public string GetText(string textKey)
        {
            var keyedValue = _jsonDatabase[textKey];
            string domain;

            if (keyedValue == null || !keyedValue.HasValues)
            {
                throw new Exception(string.Format("Phrases dictionary json may be badly formatted"));
            }
            switch (_domainId)
            {
                case 1:
                    domain = "com";
                    break;
                case 2:
                    domain = "jp";
                    break;
                case 3:
                    domain = "de";
                    break;
                case 6:
                    domain = "commx";
                    break;
                case 70:
                    domain = "couk";
                    break;
                default:
                    throw new Exception(string.Format("No dictionary found for domainId: {0}", _domainId));
            }
            try
            {
                return keyedValue[domain].ToString();  
            }
            catch (Exception)
            {
                throw new Exception(string.Format("Does not contain a value for key: {0}, and domainId: {1}", textKey, _domainId));
            }
        }
    }
}