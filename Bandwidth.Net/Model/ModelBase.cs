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

        protected void updateProperties(JObject jsonObject)
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
        protected internal string GetPropertyAsString(string key)
        {
            object value = null;
            properties.TryGetValue(key, out value);
            return (string)value;
        }
        protected string[] GetPropertyAsStringArray(string key)
        {
            if (properties.ContainsKey(key))
            {
                //TODO : [SuppressWarnings("unchecked")]
                object o = null;
                properties.TryGetValue(key, out o);
                List<object> list = (List<object>)o;

                string[] arr = new string[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    object obj = list[i];
                    arr[i] = obj.ToString();
                }
                return arr;
            }
            else
            {
                return null;
            }
        }

        protected Object GetProperty(String key)
        {
            Object o = null;
            properties.TryGetValue(key, out o);
            return o;
        }

        protected Boolean? GetPropertyAsBoolean(String key)
        {
            Object o = null;
            properties.TryGetValue(key, out o);
            if (o == null)
                return null;
            if (o is Boolean)
                return (Boolean)o;
            else
                return o.Equals("true");
        }

        protected long GetPropertyAsLong(String key)
        {
            object value;
            properties.TryGetValue(key, out value);
            return (long)value;
        }

        protected Double GetPropertyAsDouble(String key)
        {
            Object o;
            properties.TryGetValue(key, out o);

            if (o is Double)
                return (Double)o;
            else
                return Convert.ToDouble(o);
        }

        protected DateTime? getPropertyAsDate(String key)
        {
            Object o;
            properties.TryGetValue(key, out o);
            if (o == null) return null;
            if (o is long) return new DateTime((long)o);

            return DateTime.ParseExact(o.ToString(), BandwidthConstants.TRANSACTION_DATE_TIME_PATTERN, null);

        }

        protected void PutProperty(String key, Object value)
        {
            properties.Add(key, value);
        }


    }
}
