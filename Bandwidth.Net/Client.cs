using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Bandwidth.Net
{
    public interface Client //: IDisposable
    {
        RestResponse Get(String uri, Dictionary<String, Object> param);

        /*
         * 
         * 
        private readonly HttpClient _client;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly string _userPath;


        public Client(string userId, string apiToken, string secret,
            string baseUrl = "https://api.catapult.inetwork.com/")
        {
            if (userId == null) throw new ArgumentNullException("userId");
            if (apiToken == null) throw new ArgumentNullException("apiToken");
            if (secret == null) throw new ArgumentNullException("secret");
            _userPath = string.Format("users/{0}", userId);
            var url = new UriBuilder(baseUrl) {Path = "/v1/"};
            _client = new HttpClient {BaseAddress = url.Uri};
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", apiToken, secret))));
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            _jsonSerializerSettings.Converters.Add(new StringEnumConverter
            {
                CamelCaseText = true,
                AllowIntegerValues = false
            });
            _jsonSerializerSettings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
            Calls = new Calls(this);
            Recordings = new Recordings(this);
            Account = new Account(this);
            Applications = new Applications(this);
            AvailableNumbers = new AvailableNumbers(this);
            Bridges = new Bridges(this);
            Errors = new Errors(this);
            Conferences = new Conferences(this);
            Media = new Media(this);
            Messages = new Messages(this);
            NumberInfo = new NumberInfo(this);
            PhoneNumbers = new PhoneNumbers(this);
        }

        #region Base Http methods

        internal async Task<HttpResponseMessage> MakeGetRequest(string path, IDictionary<string, string> query = null,
            string id = null, bool disposeResponse = false)
        {
            string urlPath = FixPath(path);
            if (id != null)
            {
                urlPath = urlPath + "/" + id;
            }
            if (query != null && query.Count > 0)
            {
                urlPath = string.Format("{0}?{1}", urlPath,
                    string.Join("&",
                        from p in query select string.Format("{0}={1}", p.Key, Uri.EscapeDataString(p.Value))));
            }
            HttpResponseMessage response = await _client.GetAsync(urlPath);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                response.Dispose();
                throw;
            }
            if (!disposeResponse) return response;
            response.Dispose();
            return null;
        }


        internal async Task<TResult> MakeGetRequest<TResult>(string path, IDictionary<string, string> query = null,
            string id = null)
        {
            using (HttpResponseMessage response = await MakeGetRequest(path, query, id))
            {
                if (response.Content.Headers.ContentType != null &&
                    response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return json.Length > 0
                        ? JsonConvert.DeserializeObject<TResult>(json, _jsonSerializerSettings)
                        : default(TResult);
                }
            }
            return default(TResult);
        }

        internal async Task<HttpResponseMessage> MakePostRequest(string path, object data, bool disposeResponse = false)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.None, _jsonSerializerSettings);
            HttpResponseMessage response =
                await _client.PostAsync(FixPath(path), new StringContent(json, Encoding.UTF8, "application/json"));
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                response.Dispose();
                throw;
            }
            if (!disposeResponse) return response;
            response.Dispose();
            return null;
        }

        internal async Task<HttpResponseMessage> PutData(string path, Stream stream, string mediaType,
            bool disposeResponse = false)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            return await PutFileContent(path, mediaType, disposeResponse, new StreamContent(stream));
        }

        internal async Task<HttpResponseMessage> PutData(string path, byte[] buffer, string mediaType,
            bool disposeResponse = false)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");
            return await PutFileContent(path, mediaType, disposeResponse, new ByteArrayContent(buffer));
        }

        private async Task<HttpResponseMessage> PutFileContent(string path, string mediaType, bool disposeResponse,
            HttpContent content)
        {
            if (mediaType != null)
            {
                content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            }
            HttpResponseMessage response = await _client.PutAsync(FixPath(path), content);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch
            {
                response.Dispose();
                throw;
            }
            if (!disposeResponse) return response;
            response.Dispose();
            return null;
        }


        internal async Task<TResult> MakePostRequest<TResult>(string path, object data)
        {
            using (HttpResponseMessage response = await MakePostRequest(path, data))
            {
                if (response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return json.Length > 0
                        ? JsonConvert.DeserializeObject<TResult>(json, _jsonSerializerSettings)
                        : default(TResult);
                }
            }
            return default(TResult);
        }

        internal async Task MakeDeleteRequest(string path)
        {
            using (HttpResponseMessage response = await _client.DeleteAsync(FixPath(path)))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        #endregion

        public Calls Calls { get; private set; }
        public Recordings Recordings { get; private set; }
        public Account Account { get; private set; }
        public Applications Applications { get; private set; }
        public AvailableNumbers AvailableNumbers { get; private set; }
        public Bridges Bridges { get; private set; }
        public Errors Errors { get; private set; }
        public Conferences Conferences { get; private set; }
        public Media Media { get; private set; }
        public Messages Messages { get; private set; }
        public NumberInfo NumberInfo { get; private set; }
        public PhoneNumbers PhoneNumbers { get; private set; }

        public void Dispose()
        {
            _client.Dispose();
        }

        internal string ConcatUserPath(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
            if (path[0] == '/')
            {
                return _userPath + path;
            }
            return string.Format("{0}/{1}", _userPath, path);
        }

        private static string FixPath(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
            return (path[0] == '/') ? path.Substring(1) : path;
        }
    }
      */
    }
}