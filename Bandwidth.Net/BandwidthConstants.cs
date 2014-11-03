using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandwidth.Net
{
    public abstract class BandwidthConstants
    {
        private BandwidthConstants()
        {
        }

        public static readonly String TRANSACTION_DATE_TIME_PATTERN = "yyyy-MM-dd'T'HH:mm:ss'Z'";

        // REST constants
        public static readonly String API_ENDPOINT = "https://api.catapult.inetwork.com";
        public static readonly String API_VERSION = "v1";

        public static readonly String USERS_URI_PATH = "users/%s"; // userId as a parameter
        public static readonly String CALLS_URI_PATH = "calls";
        public static readonly String PHONE_NUMBER_URI_PATH = "phoneNumbers";
        public static readonly String CONFERENCES_URI_PATH = "conferences";
        public static readonly String ERRORS_URI_PATH = "errors";
        public static readonly String MESSAGES_URI_PATH = "messages";
        public static readonly String AVAILABLE_NUMBERS_URI_PATH = "availableNumbers";
        public static readonly String AVAILABLE_NUMBERS_TOLL_FREE_URI_PATH = "availableNumbers/tollFree";
        public static readonly String AVAILABLE_NUMBERS_LOCAL_URI_PATH = "availableNumbers/local";
        public static readonly String BRIDGES_URI_PATH = "bridges";
        public static readonly String RECORDINGS_URI_PATH = "recordings";
        public static readonly String ACCOUNT_URI_PATH = "account";
        public static readonly String APPLICATIONS_URI_PATH = "applications";
        public static readonly String MEDIA_URI_PATH = "media";
        public static readonly String ACCOUNT_TRANSACTIONS_URI_PATH = "account/transactions";
        public static readonly String GATHER_URI_PATH = "gather";

    }
}
