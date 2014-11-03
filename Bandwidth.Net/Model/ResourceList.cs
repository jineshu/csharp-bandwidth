using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Bandwidth.Net.Model
{
    public class ResourceList<T> : List<T> where T :class
    {
        protected int page;
        protected int size;
        protected int index = 0;

        // Currently only the nextLink is used. The others are in place for a 
        // a list iterator implementaiton
        protected String previousLink;
        protected String nextLink = null;
        protected String firstLink;

        private RestResponse response;

        protected T clazz;
        protected String resourceUri;

        private IBandwidthClient client;

        public ResourceList(String resourceUri, T clazz) : base()
        {

            page = 0;
            size = 25;

            this.resourceUri = resourceUri;
            this.clazz = clazz;
        }

        public ResourceList(int page, int size, String resourceUri, T clazz)
            : base()
        {

            this.page = page;
            this.size = size;
            this.resourceUri = resourceUri;
            this.clazz = clazz;
        }

        /**
	 * initializes ArrayList with first page from BW API
	 */

        public void Initialize()
        {
            JObject jsonParams = new JObject();

            jsonParams.Add("page", page);
            jsonParams.Add("size", size);

            GetPage(jsonParams);
        }

        /**
	 * This method updates the page value, creates the params for the API call and clears the current list
	 */

        protected void GetNextPage()
        {
            JObject jObjectParams = new JObject();
            page++;
            jObjectParams.Add("page", page);
            jObjectParams.Add("size", size);
            //TODO:need to check what does the clear method does in this cotext.
            //clear();

            GetPage(jObjectParams);
        }

        /**
	 * This method makes the API call to get the list value for the specified resource. It loads the return
	 * from the API into the arrayList, updates the index if necessary and sets the new link values
	 * @jsonParams params
	 */

        protected void GetPage(JObject jsonParams)
        {

            if (this.client == null)
                client = BandwidthRestClient.GetInstance();

            RestResponse response = client.Get(resourceUri, jsonParams.ToObject<Dictionary<string, object>>());

            JArray array = Utils.Response2JSONArray(response);

            foreach (Object obj in array)
            {
                Type type = typeof(T);
                T elem = Activator.CreateInstance(type, new[] { typeof(BandwidthClient), typeof(JObject) }) as T;
                Add(elem);
            }

            // if anything comes back, reset the index
            if (array.Count > 0)
                this.index = 0;

            // set the next links
            this.NextLink = response.NextLink;
            this.PreviousLink = response.PreviousLink;
            this.FirstLink = response.FirstLink;
        }

        public String FirstLink
        {
            get { return firstLink; }
            set{ firstLink = value;}
        }

        public String NextLink
        {
            get { return nextLink; }
            set { nextLink = value; }
        }

        public String PreviousLink
        {
            get { return previousLink; }
            set { previousLink = value; }
        }
    }
}
