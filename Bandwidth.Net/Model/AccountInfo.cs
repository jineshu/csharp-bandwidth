using Newtonsoft.Json.Linq;
using System;

namespace Bandwidth.Net.Model
{
    public class AccountInfo : BaseModelObject
    {
        public AccountInfo(BandwidthRestClient client, JObject jsonObject)
            : base(client, jsonObject)
        {
        }

        protected override String GetUri()
        {
            return client.GetUserResourceUri(BandwidthConstants.ACCOUNT_URI_PATH);
        }

        public String GetAccountType()
        {
            return GetPropertyAsString("accountType");
        }

        public double GetBalance()
        {
            return GetPropertyAsDouble("balance");
        }

        public override String ToString()
        {
            return "AccountInfo{" +
                   "accountType='" + GetAccountType() + '\'' +
                   ", balance=" + GetBalance() +
                   '}';
        }
    }
}
