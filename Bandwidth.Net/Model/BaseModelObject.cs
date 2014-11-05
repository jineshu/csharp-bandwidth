using Newtonsoft.Json.Linq;
using System;

namespace Bandwidth.Net.Model
{
    public abstract class BaseModelObject : ModelBase
    {
        protected readonly BandwidthRestClient client;

        protected BaseModelObject(BandwidthRestClient client, JObject jsonObject)
        {
            this.client = client;
            UpdateProperties(jsonObject);
        }

        protected BaseModelObject(BandwidthRestClient client, String parentUri, JObject jsonObject)
        {
            //Todo:parenturi is not used.. should it be removed..
            //this.parentUri = parentUri
            this.client = client;
            UpdateProperties(jsonObject);
        }

        protected BandwidthRestClient GetClient()
        {
            return client;
        }

        protected abstract String GetUri();
    }
}
