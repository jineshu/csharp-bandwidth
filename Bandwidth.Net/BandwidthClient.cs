using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bandwidth.Net
{
    //TODO: This class is simliar to BandwidthRestClient class move common code in one class
    public class BandwidthClient
    {
         protected static readonly String GET = "get";
        protected static readonly String POST = "post";
        protected static readonly String PUT = "put";
        protected static readonly String DELETE = "delete";

        public static String BANDWIDTH_USER_ID = "BANDWIDTH_USER_ID";
        public static String BANDWIDTH_API_TOKEN = "BANDWIDTH_API_TOKEN";
        public static String BANDWIDTH_API_SECRET = "BANDWIDTH_API_SECRET";
        public static String BANDWIDTH_API_ENDPOINT = "BANDWIDTH_API_ENDPOINT";
        public static String BANDWIDTH_API_VERSION = "BANDWIDTH_API_VERSION";

        protected readonly String usersUri;
        protected readonly String baseUri;

        protected readonly String token;
        protected readonly String secret;

        protected HttpClient httpClient;

        protected static BandwidthClient INSTANCE;

        protected String apiEndpoint;
        protected String apiVersion;

        public static BandwidthClient GetInstance()
        {
            if (INSTANCE == null)
            {
                //TODO:Read the emvironment variables here, since this is portale library we cannot acess Enviorment.GetEnviormentVariables.
                String userId, apiToken, apiSecret, apiEndpoint, apiVersion;
                userId = apiToken = apiSecret = apiEndpoint = apiVersion = null;

                INSTANCE = new BandwidthClient(userId, apiToken, apiSecret, apiEndpoint, apiVersion);
            }
            return INSTANCE;
        }

        protected BandwidthClient(String userId, String token, String secret, String apiEndpoint, String apiVersion)
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


        public RestResponse Get(string uri, Dictionary<string, object> mapParams)
        {
            HttpClient client = new HttpClient();

            String path = GetPath(uri);


            // TODO put this in a method
            List<KeyValuePair<String, String>> pairs = new List<KeyValuePair<String, String>>();
            foreach (String key in mapParams.Keys)
            {
                pairs.Add(new KeyValuePair<string, string>(key, mapParams[key].ToString()));
            }

            Uri fullUri = BuildUri(path, pairs);

            WebRequest httpWebRequest = HttpWebRequest.Create(fullUri);
            
            SetHeaders(httpWebRequest);
            //TODO:need to make the getrequest and get the response and depend on the response create the restresponse
            //HttpWebResponse httpResponse = HttpWebResponse.

            //return RestResponse.CreateRestResponse(httpResponse);
            return null;
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
        }

        protected void SetHeaders(WebRequest request)
        {
            request.Headers["Accept"] = "application/json";
            request.Headers["Accept-Charset"] = "utf-8";
            String s = String.Format("{0}:{1}", token, secret);
            String auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
            request.Headers["Authorization"] = "Basic " + auth;
        }

        protected Uri BuildUri(String path, List<KeyValuePair<String, String>> queryStringParams)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(path);

            if (queryStringParams != null && queryStringParams.Count > 0)
            {
                sb.Append("?");
                foreach (var queryStringParam in queryStringParams)
                {
                    sb.Append(String.Format("{0}={1}", queryStringParam.Key, queryStringParam.Value));
                }
            }

            Uri uri;
            try
            {
                uri = new Uri(sb.ToString());
            }
            catch (FormatException e)
            {
                throw new InvalidOperationException("Invalid uri", e);
            }
            return uri;
        }
    }
}
