using Bandwidth.Net.Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bandwidth.Net
{
    public class BandwidthRestClient : Client
    {
        protected static readonly String GET = "get";
        protected static readonly String POST = "post";
        protected static readonly String PUT = "put";
        protected static readonly String DELETE = "delete";

        public static String BANDWIDTH_USER_ID = null;
        public static String BANDWIDTH_API_TOKEN = null;
        public static String BANDWIDTH_API_SECRET = null;
        public static String BANDWIDTH_API_ENDPOINT = null;
        public static String BANDWIDTH_API_VERSION = null;

        protected readonly String usersUri;
        protected readonly String baseUri;

        protected readonly String token;
        protected readonly String secret;

        protected HttpClient httpClient;

        protected static BandwidthRestClient INSTANCE;

        protected String apiEndpoint;
        protected String apiVersion;

        public static BandwidthRestClient GetInstance()
        {
            return INSTANCE;
        }

        public static BandwidthRestClient GetInstance(string userId, string apiToken, string secret, string apiEndpoint = "https://api.catapult.inetwork.com", string apiVersion = "v1")
        {
            if (INSTANCE==null)
            {
                BANDWIDTH_USER_ID = userId;
                BANDWIDTH_API_TOKEN = apiToken;
                BANDWIDTH_API_SECRET = secret;
                BANDWIDTH_API_ENDPOINT = apiEndpoint;
                BANDWIDTH_API_VERSION = apiVersion;
                INSTANCE=new BandwidthRestClient(userId, apiToken, secret, apiEndpoint, apiVersion);
            }
            return INSTANCE;
        }


        public String GetUserUri()
        {
            return usersUri;
        }

        public String GetUserResourceUri(String path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            return String.Join("/", new String[] {GetUserUri(), path});
        }

        public String GetUserResourceInstanceUri(String path, String instanceId)
        {
            if (String.IsNullOrEmpty(path) || String.IsNullOrEmpty(instanceId))
                throw new ArgumentNullException("path", "Path or Instance Id cannot be null");
            return GetUserResourceUri(path) + "/" + instanceId;
        }

        public String GetBaseResourceUri(String path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            return path + "/";
        }

        protected BandwidthRestClient(String userId, String token, String secret, String apiEndpoint, String apiVersion)
        {
            usersUri = String.Format(BandwidthConstants.USERS_URI_PATH, userId);
            baseUri = "";

            this.token = token;
            this.secret = secret;

            this.apiEndpoint = apiEndpoint;
            this.apiVersion = apiVersion;

            if (apiEndpoint == null || apiVersion == null)
            {
                this.apiEndpoint = BandwidthConstants.API_ENDPOINT;
                this.apiVersion = BandwidthConstants.API_VERSION;
            }

            httpClient = new HttpClient();
        }

        /// <summary>
        /// Returns information about this number.
        /// @param number searching number
        /// @return information about the number
        /// </summary>
        public NumberInfo GetNumberInfoByNumber(String number)
        {
            String uri = String.Join("/", new String[] {"phoneNumbers", "numberInfo", number});
            JObject obj = GetObject(uri);
            return new NumberInfo(obj);
        }

        public RestResponse Get(string uri, Dictionary<string, object> mapParams)
        {
            String path = GetPath(uri);

            // TODO put this in a method

            Dictionary<String, String> pairs = mapParams.Keys.ToDictionary(key => key, key => mapParams[key].ToString());

            Uri fullUri = BuildUri(path, pairs);

            HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(fullUri);

            SetHeaders(httpWebRequest);

            HttpWebResponse httpResponse = (HttpWebResponse) httpWebRequest.GetResponseAsync().Result;

            return RestResponse.CreateRestResponse(httpResponse);
        }

        protected void SetHeaders(WebRequest request)
        {
            request.Headers["Accept"] = "application/json";
            request.Headers["Accept-Charset"] = "utf-8";
            String s = String.Format("{0}:{1}", token, secret);
            String auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
            request.Headers["Authorization"] = "Basic " + auth;
        }

        public JArray GetArray(String uri, Dictionary<String, Object> param)
        {
            String path = GetPath(uri);

            RestResponse response = Request(path, GET, param);

            if (response.IsError()) throw new IOException(response.GetResponseText());

            if (response.IsJson())
                return JArray.Parse(response.GetResponseText());
            else
                throw new IOException("Response is not a JSON format.");
        }

        public JObject GetObject(String uri)
        {
            String path = GetPath(uri);

            return GetObjectFromLocation(path);
        }

        public JObject GetObjectFromLocation(String locationUrl)
        {
            RestResponse response = Request(locationUrl, GET);
            if (response.IsError())
                throw new IOException(response.GetResponseText());

            if (response.IsJson())
                return JObject.Parse(response.GetResponseText());

            else
                throw new IOException("Response is not a JSON format.");
        }

        public JObject Create(String uri, Dictionary<String, Object> param)
        {
            String path = GetPath(uri);
            RestResponse response = Request(path, POST, param);
            if (response.IsError()) throw new IOException(response.GetResponseText());

            String location = response.GetLocation();
            if (location != null)
            {
                response = Request(location, GET);
                if (response.IsError()) throw new IOException(response.GetResponseText());

                if (response.IsJson())
                    return JObject.Parse(response.GetResponseText());
                else
                    throw new IOException("Response is not a JSON format.");
            }
            else
                throw new IOException("There is no location of new application.");
        }

        public RestResponse Post(String uri, Dictionary<String, Object> param)
        {
            String path = GetPath(uri);
            RestResponse response = Request(path, POST, param);
            if (response.IsError()) throw new IOException(response.GetResponseText());

            return response;
        }

        public void Delete(String uri)
        {
            String path = GetPath(uri);
            RestResponse response = Request(path, DELETE);
            if (response.IsError()) throw new IOException(response.GetResponseText());
        }


        public void UploadFile(String uri, object temp /* File sourceFile*/, String contentType)
        {
            //TODO:Can not use Fileinfo in project 
            //String path = GetPath(uri);

            //HttpPut request = (HttpPut) SetupRequest(path, PUT, null);
            //request.SetEntity(contentType == null ? new FileEntity(sourceFile) : new FileEntity(sourceFile, ContentType.parse(contentType)));

            //performRequest(request);
        }

        public void DownloadFileTo(String uri, object temp /*File destFile*/)
        {
            //TODO:Can not use Fileinfo in project
            //string path = GetPath(uri); 
            //HttpGet request = (HttpGet) SetupRequest(path, GET, Collections.<String, Object>emptyMap());
            //HttpResponse response;

            //OutputStream outputStream = null;
            //try {
            //    response = httpClient.Execute(request);
            //    HttpEntity entity = response.GetEntity();

            //    StatusLine status = response.GetStatusLine();
            //    int statusCode = status.GetStatusCode();
            //    if (statusCode >= 400) throw new IOException(EntityUtils.toString(entity));

            //    outputStream = new BufferedOutputStream(new FileOutputStream(destFile));
            //    entity.writeTo(outputStream);
            //} catch (final ClientProtocolException e1) {
            //    throw new IOException(e1);
            //} catch (final IOException e1) {
            //    throw new IOException(e1);
            //} finally {
            //    try {
            //        if (outputStream != null) outputStream.close();
            //    } catch (IOException ignore) {
            //    }
            //}
        }

        public String GetPath(String uri)
        {
            String[] parts = new String[]
                             {
                                 apiEndpoint,
                                 apiVersion,
                                 uri,
                             };

            return String.Join("/", parts);
            ;
        }

        protected RestResponse Request(String path, String method)
        {
            return Request(path, method, null);
        }

        protected RestResponse Request(String path, String method, Dictionary<String, Object> paramList)
        {
            if (paramList == null) paramList = new Dictionary<string, object> {};

            HttpWebRequest request = SetupRequest(path, method, paramList);
            return PerformRequest(request);
        }

        protected RestResponse PerformRequest(HttpWebRequest request)
        {
            HttpWebResponse response = (HttpWebResponse) request.GetResponseAsync().Result;
            String responseBody = "";

            int statusCode = (int) response.StatusCode;

            RestResponse restResponse = new RestResponse(responseBody, statusCode);

            restResponse.SetStatus((int) response.StatusCode);

            string contentType = response.Headers["Content-Type"];
            restResponse.SetContentType(contentType);

            string location = response.Headers["Location"];
            restResponse.SetLocation(location);

            return restResponse;
        }

        protected HttpWebRequest SetupRequest(String path, String method, Dictionary<String, Object> param)
        {
            HttpWebRequest request = BuildMethod(method, path, param);
            SetHeaders(request);
            return request;
        }

        protected HttpWebRequest BuildMethod(String method, String path, Dictionary<String, Object> param)
        {
            if (String.Equals(method, GET))
            {
                return GenerateGetRequest(path, param);
            }
            else if (String.Equals(method, POST))
            {
                return GeneratePostRequest(path, param);
            }
            else if (String.Equals(method, PUT))
            {
                return GeneratePutRequest(path, param);
            }
            else if (String.Equals(method, DELETE))
            {
                return GenerateDeleteRequest(path);
            }
            else
            {
                throw new Exception("Must not be here.");
            }
        }

        protected HttpWebRequest GenerateGetRequest(String path, Dictionary<String, Object> paramMap)
        {

            Dictionary<String, String> pairs = paramMap.Keys.ToDictionary(key => key, key => paramMap[key].ToString());

            Uri uri = BuildUri(path, pairs);
            var req = (HttpWebRequest) WebRequest.Create(uri);
            req.Method = "GET";
            return req;
        }

        protected HttpWebRequest GeneratePostRequest(String path, Dictionary<String, Object> paramMap)
        {
            Uri uri = BuildUri(path);

            string strData = JsonConvert.SerializeObject(paramMap);

            var req = (HttpWebRequest) WebRequest.Create(uri);
            req.Method = "POST";
            byte[] byteArray = Encoding.UTF8.GetBytes(strData);
            Stream dataStream = req.GetRequestStreamAsync().Result;
            dataStream.Write(byteArray, 0, byteArray.Length);

            return req;
        }

        protected HttpWebRequest GenerateDeleteRequest(String path)
        {
            Uri uri = BuildUri(path);
            var req = (HttpWebRequest) WebRequest.Create(uri);
            req.Method = "DELETE";
            return req;
        }

        protected HttpWebRequest GeneratePutRequest(String path, Dictionary<String, Object> paramMap)
        {
            Uri uri = BuildUri(path);
            var req = (HttpWebRequest) WebRequest.Create(uri);

            if (paramMap != null)
            {
                String strData = JsonConvert.SerializeObject(paramMap);
                req.Method = "PUT";
                byte[] byteArray = Encoding.UTF8.GetBytes(strData);
                Stream dataStream = req.GetRequestStreamAsync().Result;
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            return req;
        }

        protected Uri BuildUri(String path)
        {
            return BuildUri(path, null);
        }

        protected Uri BuildUri(String path, Dictionary<string, string> queryStringParams)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(path);

            if (queryStringParams != null && queryStringParams.Count > 0)
            {
                foreach (var queryStringParam in queryStringParams)
                {
                    byte[] keyBytes = Encoding.UTF8.GetBytes(queryStringParam.Key);
                    byte[] valueBytes = Encoding.UTF8.GetBytes(queryStringParam.Value);
                    string data = string.Format("?{0}={1}",
                        Encoding.UTF8.GetString(keyBytes, 0, keyBytes.Length),
                        Encoding.UTF8.GetString(valueBytes, 0, valueBytes.Length));
                    sb.Append(data);
                }
            }

            Uri uri = new Uri(sb.ToString());
            return uri;
        }
    }
}
