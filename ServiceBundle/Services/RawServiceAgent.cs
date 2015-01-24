using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using OpenTable.Features.Core.Interfaces;

namespace OpenTable.Features.ServiceBundle.Services
{
    public class RawServiceAgent : IServiceObject
    {
        public class Response
        {
            private readonly HttpResponseMessage _response;

            public Response(HttpResponseMessage response)
            {
                _response = response;
            }

            public string Body
            {
                get
                {
                    return _response.Content.ReadAsStringAsync().Result;
                }
            }

            public HttpStatusCode ResponseCode
            {
                get
                {
                    return _response.StatusCode;
                }
            }

            public bool HasHeaderMatching(string header, Func<string, bool> matcher)
            {
                return _response.Content.Headers
                        .Union(_response.Headers)
                        .Where(w => w.Key.Trim().ToLower() == header.ToLower())
                        .SelectMany(s => s.Value)
                        .Any(matcher);
            }

            public Dictionary<string, string> Headers
            {
                get
                {
                    var result = _response.Content.Headers
                            .Union(_response.Headers)
                            .Select(h => new
                                {
                                    key = h.Key,
                                    value = string.Join("; ", h.Value.ToArray())
                                })
                            .ToDictionary(k => k.key, v => v.value);

                    return result;
                }
            }
        }

        public class Request
        {
            private readonly string _baseUrl;
            private Dictionary<string, string> _headers = new Dictionary<string, string>();
            private string _targetUrl;

            private readonly Dictionary<string, string> _queryParas = new Dictionary<string, string>();

            private string _requestBody;

            private Request(string baseUrl)
            {
                _baseUrl = baseUrl;
            }

            public void AddHeader(string header, string value)
            {
                _headers.Add(header, value);
            }

            public void AddQueryParam(string name, string value)
            {
                _queryParas.Add(name, value);
            }

            public void SetRelativeAddress(string relativeUri)
            {
                _targetUrl = string.Format("{0}/{1}", _baseUrl.TrimEnd(new [] {'/'}), relativeUri.TrimStart(new [] {'/'}));
            }

            /// <summary>
            /// Usage example:
            ///     myrequest.DeleteHeaders((h, v) => h.ToLower() == "cookie")
            /// This will remove all headers with a header named 'cookie'
            /// </summary>
            /// <param name="predicate"></param>
            public void DeleteHeaders(Func<string, string, bool> predicate)
            {
                _headers = _headers.Where(kvp => !predicate(kvp.Key, kvp.Value)).ToDictionary(k => k.Key, v => v.Value);
            }

            public void SetBody(string requestBody)
            {
                _requestBody = requestBody;
            }

            public HttpRequestMessage AsHttpRequestMessage()
            {
                var query = _queryParas
                    .Select(s => s.Key + "=" + s.Value)
                    .Aggregate("", (s, a) => s + ((s == "") ? "?" + a : "&" + a));

                var uriString = _targetUrl + query;

                var request = new HttpRequestMessage(Verb, uriString);

                if (_requestBody != null)
                {
                    request.Content = new StringContent(_requestBody);
                }

                foreach (var header in _headers)
                {
                    if (header.Key.ToLower() == "content-type")
                    {
                        request.Content.Headers.ContentType = new MediaTypeHeaderValue(header.Value);
                    }
                    else
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }

                return request;
            }

            public static Request GetRequest(string baseUrl)
            {
                return new Request(baseUrl) { Verb = HttpMethod.Get };
            }

            public static Request PostRequest(string baseUrl)
            {
                return new Request(baseUrl) { Verb = HttpMethod.Post };
            }

            public HttpMethod Verb { get; private set; }

            public enum HttpVerbs { GET, POST }
        }

        private readonly string _baseUrl;

        public RawServiceAgent(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public Request BuildRequest()
        {
            return Request.GetRequest(_baseUrl);
        }

        public Request BuildPostRequest()
        {
            return Request.PostRequest(_baseUrl);
        }

        public Response SendRequest(Request directRequest)
        {
            using(var handler = new HttpClientHandler { UseCookies = false })
                using (var client = new HttpClient(handler))
                {
                    var request = directRequest.AsHttpRequestMessage();
                    var response = client.SendAsync(request);
                    return new Response(response.Result);
                }
        }
    }
}