using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    public abstract class ModelBase
    {
        protected internal readonly Dictionary<string, object> properties
            = new Dictionary<string, object>();

        protected void UpdateProperties(JObject jsonObject)
        {
            if (jsonObject != null)
            {
                foreach (KeyValuePair<string, JToken> jObject in jsonObject)
                {
                    string name = jObject.Key;
                    JToken value = jObject.Value;
                    properties.Add(name, value);
                }
            }
        }

        public string GetId()
        {
            return GetPropertyAsString("id");
        }

        protected string GetPropertyAsString(string key)
        {
            object dictionaryKeysValue;
            properties.TryGetValue(key, out dictionaryKeysValue);
            return (string)dictionaryKeysValue;
        }

        protected string[] GetPropertyAsStringArray(string key)
        {
            if (properties.ContainsKey(key))
            {
                object dictionaryValue;
                properties.TryGetValue(key, out dictionaryValue);
                List<object> list = (List<object>) dictionaryValue;

                if (list != null)
                {
                    string[] arr = new string[list.Count];
                    for (int i = 0; i < list.Count; i++)
                    {
                        object obj = list[i];
                        arr[i] = obj.ToString();
                    }
                    return arr;
                }
                else
                    return null;
            }
            else
                return null;
        }

        protected Object GetProperty(String key)
        {
            Object dictionaryValue = null;
            properties.TryGetValue(key, out dictionaryValue);
            return dictionaryValue;
        }

        protected Boolean? GetPropertyAsBoolean(String key)
        {
            Object dictionaryValue = null;
            properties.TryGetValue(key, out dictionaryValue);
            if (dictionaryValue == null)
                return null;
            if (dictionaryValue is Boolean)
                return (Boolean) dictionaryValue;
            else
                return dictionaryValue.Equals("true");
        }

        protected long GetPropertyAsLong(String key)
        {
            object value;
            properties.TryGetValue(key, out value);
            return (long) value;
        }

        protected Double GetPropertyAsDouble(String key)
        {
            Object dictionaryValue;
            properties.TryGetValue(key, out dictionaryValue);

            if (dictionaryValue is Double)
                return (Double)dictionaryValue;
            else
                return Convert.ToDouble(dictionaryValue);
        }

        protected DateTime? GetPropertyAsDate(String key)
        {
            Object dictionaryValue;
            properties.TryGetValue(key, out dictionaryValue);
            if (dictionaryValue == null) return null;
            if (dictionaryValue is long) return new DateTime((long) dictionaryValue);

            return DateTime.ParseExact(dictionaryValue.ToString(), BandwidthConstants.TRANSACTION_DATE_TIME_PATTERN, null);
        }

        protected void PutProperty(String key, Object value)
        {
            properties.Add(key, value);
        }
    }
}
