using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
