using CRS.ChannelManager.Library.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using RestSharp;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace CRS.ChannelManager.Library.Utils
{
    public static class HttpUtils
    {
        private static ILogger? _logger { get; set; } = null;

        public static void SetLog(ILogger? logger = null)
        {
            _logger = logger;
        }

        private static void WriteLog(string? baseUrl, string? endPoint, Dictionary<string, string>? headers, string? body, string? response)
        {
            if (_logger != null)
            {
                _logger.LogInformation($"baseUrl: {baseUrl}, endPoint {endPoint}, headers: {JsonConvert.SerializeObject(headers)}, body: {body}, response: {response}");
            }
        }

        public static async Task<TRes?> PostJson<TReq, TRes>(string baseApiUrl, string enpointUrl, Dictionary<string, string> headers, TReq requestBody, ILogger? logger = null) where TReq : class
        {
            _logger = logger;
            var options = new RestClientOptions(baseApiUrl)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(enpointUrl, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            foreach (var headerItem in headers)
            {
                request.AddHeader(headerItem.Key, headerItem.Value);
            }

            string bodyJson = JsonConvert.SerializeObject(requestBody);
            request.AddStringBody(bodyJson, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            WriteLog(baseApiUrl, enpointUrl, headers, bodyJson, response.Content);
            if (response.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(response?.Content))
                {
                    return JsonConvert.DeserializeObject<TRes>(response?.Content);
                }
                else
                    return default(TRes);
            }
            else
            {
                throw new DomainExceptionBase(response?.Content, _logger);
            }
        }

        public static async Task<TRes?> PutJson<TReq, TRes>(string baseApiUrl, string enpointUrl, Dictionary<string, string> headers, TReq requestBody, ILogger? logger = null) where TReq : class
        {
            _logger = logger;
            var options = new RestClientOptions(baseApiUrl)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(enpointUrl, Method.Put);
            request.AddHeader("Content-Type", "application/json");
            foreach (var headerItem in headers)
            {
                request.AddHeader(headerItem.Key, headerItem.Value);
            }

            string bodyJson = JsonConvert.SerializeObject(requestBody);
            request.AddStringBody(bodyJson, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            WriteLog(baseApiUrl, enpointUrl, headers, bodyJson, response.Content);
            if (response.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(response?.Content))
                    return JsonConvert.DeserializeObject<TRes>(response?.Content);
                else
                    return default(TRes);
            }
            else
            {
                throw new DomainExceptionBase(response?.Content, _logger);
            }
        }

        public static async Task<TRes?> DeleteJson<TRes>(string baseApiUrl, string enpointUrl, Dictionary<string, string> headers, ILogger? logger = null)
        {
            _logger = logger;
            var options = new RestClientOptions(baseApiUrl)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(enpointUrl, Method.Delete);
            request.AddHeader("Content-Type", "application/json");
            foreach (var headerItem in headers)
            {
                request.AddHeader(headerItem.Key, headerItem.Value);
            }

            RestResponse response = await client.ExecuteAsync(request);
            WriteLog(baseApiUrl, enpointUrl, headers, string.Empty, response.Content);
            if (response.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(response?.Content))
                    return JsonConvert.DeserializeObject<TRes>(response?.Content);
                else
                    return default(TRes);
            }
            else
            {
                throw new DomainExceptionBase(response?.Content, _logger);
            }
        }

        public static async Task<TRes?> GetJson<TRes>(string baseApiUrl, string enpointUrl, Dictionary<string, string> headers, ILogger? logger = null)
        {
            _logger = logger;
            var options = new RestClientOptions(baseApiUrl)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(enpointUrl, Method.Get);
            request.AddHeader("Content-Type", "application/json");

            foreach (var headerItem in headers)
            {
                request.AddHeader(headerItem.Key, headerItem.Value);
            }

            RestResponse response = await client.ExecuteAsync(request);
            WriteLog(baseApiUrl, enpointUrl, headers, string.Empty, response.Content);
            if (response.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(response?.Content))
                    return JsonConvert.DeserializeObject<TRes>(response?.Content);
                else
                    return default(TRes);
            }
            else
            {
                throw new DomainExceptionBase(response?.Content, _logger);
            }
        }

        public static async Task<TRes?> PostFile<TRes>(string apiURL, Dictionary<string, string> headers, List<IFormFile> requestBody, ILogger? logger = null)
        {
            _logger = logger;
            var client = new RestClient(apiURL);
            var request = new RestRequest(string.Empty, Method.Post);
            request.AddHeader("Content-Type", "multipart/form-data");
            byte[] fileBytes = null;
            using (var memoryStream = new MemoryStream())
            {
                if (requestBody.Any() && requestBody.Count == 1)
                {
                    requestBody.FirstOrDefault().CopyTo(memoryStream);
                    fileBytes = memoryStream.ToArray();
                    request.AddFile("image", fileBytes, requestBody.FirstOrDefault().FileName);
                }
                else
                {
                    for (var i = 0; i < requestBody.Count; i++)
                    {
                        var imageKey = string.Format("image{0}", i + 1);
                        requestBody[i].CopyTo(memoryStream);
                        fileBytes = memoryStream.ToArray();
                        request.AddFile(imageKey, fileBytes, requestBody[i].FileName);
                        memoryStream.SetLength(0);
                    }
                }
            }
            var response = client.Execute(request);
            WriteLog(apiURL, string.Empty, headers, string.Empty, response.Content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (!string.IsNullOrEmpty(response?.Content))
                {
                    var res = JsonConvert.DeserializeObject<TRes>(response?.Content);
                    return res;
                }
                else
                    return default(TRes);
            }
            else
            {
                throw new DomainExceptionBase(response?.Content, _logger);
            }
        }

        public static async Task<TRes?> PostForm<TRes>(string baseApiUrl, string enpointUrl, Dictionary<string, string> headers, Dictionary<string, string> addParameter, ILogger? logger = null)
        {
            _logger = logger;
            var options = new RestClientOptions(baseApiUrl)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(enpointUrl, Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            foreach (var headerItem in headers)
            {
                request.AddHeader(headerItem.Key, headerItem.Value);
            }
            foreach (var item in addParameter)
            {
                request.AddParameter(item.Key, item.Value);
            }
            RestResponse response = await client.ExecuteAsync(request);
            WriteLog(baseApiUrl, enpointUrl, headers, JsonConvert.SerializeObject(addParameter), response.Content);
            if (response.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(response?.Content))
                    return JsonConvert.DeserializeObject<TRes>(response?.Content);
                else
                    return default(TRes);
            }
            else
            {
                throw new DomainExceptionBase(response?.Content, _logger);
            }
        }

        public static async Task<byte[]?> PostJson<TReq>(string baseApiUrl, string enpointUrl, Dictionary<string, string> headers, TReq requestBody, ILogger? logger = null) where TReq : class
        {
            _logger = logger;
            var options = new RestClientOptions(baseApiUrl)
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest(enpointUrl, Method.Post);
            request.AddHeader("Content-Type", "application/json");
            foreach (var headerItem in headers)
            {
                request.AddHeader(headerItem.Key, headerItem.Value);
            }

            string bodyJson = JsonConvert.SerializeObject(requestBody);
            request.AddStringBody(bodyJson, DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            WriteLog(baseApiUrl, enpointUrl, headers, bodyJson, response.Content);
            if (response.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(response?.Content))
                {
                    return response?.RawBytes;
                }
                else
                    return default(byte[]);
            }
            else
            {
                throw new DomainExceptionBase(response?.Content, _logger);
            }
        }
    }
}
