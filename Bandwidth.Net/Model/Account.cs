using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public class Account : BaseModelObject
    {
        public static Account GetAccount()
        {
            BandwidthRestClient client = BandwidthRestClient.GetInstance();

            return new Account(client);
        }
        public Account(BandwidthRestClient client) : base(client, null)
        {
        }
        
        /// <summary>
        /// Gets your current account information.
        ///@return information account information
        /// </summary>
        /// <returns></returns>
        public AccountInfo GetAccountInfo()
        {
            JObject jsonObject = client.GetObject(GetUri());
            return new AccountInfo(client, jsonObject);
        }
        protected override string GetUri()
        {
            return client.GetUserResourceUri(BandwidthConstants.ACCOUNT_URI_PATH);
        }
        
        /// <summary>
        /// Creates builder for getting transactions of the account.
        /// Example:
        /// <code>List<AccountTransaction> list = account.QueryTransactionsBuilder().MaxItems(5).Type("charge").List();</code>
        /// @return builder for getting transactions
        /// </summary>
        /// <returns></returns>
        public TransactionsQueryBuilder QueryTransactionsBuilder()
        {
            return new TransactionsQueryBuilder(this);
        }

        private List<AccountTransaction> GetTransactions(Dictionary<String, Object> param)
        {
            String transactionUri = GetAccountTransactionsUri();
            JArray array = client.GetArray(transactionUri, param);
            List<AccountTransaction> transactions = new List<AccountTransaction>();
            foreach (JObject jObject in array)
            {
                transactions.Add(new AccountTransaction(client, jObject));
            }
            return transactions;
        }

        private String GetAccountTransactionsUri()
        {
            return String.Join("/", new string[] {GetUri(), "transactions"});
        }
        public class TransactionsQueryBuilder
        {
            private Account _account;
            public TransactionsQueryBuilder(Account account)
            {
                _account = account;
            }
            private Dictionary<string, object> param = new Dictionary<string, object>();

            public TransactionsQueryBuilder MaxItem(int maxItems)
            {
                param.Add("maxItems", maxItems);
                return this;
            }

            public TransactionsQueryBuilder FromDate(DateTime formDateTimeDate)
            {
                param.Add("formDate", DateTime.ParseExact(formDateTimeDate.ToString(), BandwidthConstants.TRANSACTION_DATE_TIME_PATTERN, null));
                return this;
            }

            public TransactionsQueryBuilder ToDate(DateTime toDateTime)
            {
                param.Add("toDate", DateTime.ParseExact(toDateTime.ToString(), BandwidthConstants.TRANSACTION_DATE_TIME_PATTERN, null));
                return this;
            }

            public TransactionsQueryBuilder Type(String type)
            {
                param.Add("type", type);
                return this;
            }

            public TransactionsQueryBuilder Page(int page)
            {
                param.Add("page", page);
                return this;
            }
            public TransactionsQueryBuilder Size(int size)
            {
                param.Add("page", size);
                return this;
            }

            public List<AccountTransaction> List()
            {
                return _account.GetTransactions(param);
            }
        }

    }
}
