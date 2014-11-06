using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Bandwidth.Net
{
    public class Utils
    {
        public static JArray Response2JSONArray(RestResponse response)
        {
            if (response.IsJson() && response.ResponseText != null)
            {
                try
                {
                    return JArray.Parse(response.ResponseText);
                }
                catch (InvalidOperationException ex)
                {
                    throw new IOException(ex.Message);
                }
            }
            else
            {
                throw new IOException("Response is not a JSON format.");
            }
        }
    }
}
