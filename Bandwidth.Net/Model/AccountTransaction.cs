using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Bandwidth.Net.Model
{
    public class AccountTransaction : BaseModelObject
    {
        public AccountTransaction(BandwidthRestClient client, JObject jsonObject) : base(client, jsonObject)
        {
        }

        protected override string GetUri()
        {
            return client.GetUserResourceInstanceUri(BandwidthConstants.ACCOUNT_TRANSACTIONS_URI_PATH, GetId());
        }

        public String GetType()
        {
            return GetPropertyAsString("type");
        }

        public DateTime? GetDateTime()
        {
            return GetPropertyAsDate("time");
        }

        public Double GetAmount()
        {
            return GetPropertyAsDouble("amount");
        }

        public long GetUnits()
        {
            return GetPropertyAsLong("units");
        }

        public String GetProductType()
        {
            return GetPropertyAsString("productType");
        }

        public String GetNumber()
        {
            return GetPropertyAsString("number");
        }

        public override string ToString()
        {
            return "AccountTransaction{" +
                "id='" + GetId() + '\'' +
                ", type='" + GetType() + '\'' +
                ", dateTime=" + GetDateTime() +
                ", amount=" + GetAmount() +
                ", units=" + GetUnits() +
                ", productType='" + GetProductType() + '\'' +
                ", number='" + GetNumber() + '\'' +
                '}';
        }
    }
}
