using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandwidth.Net.Model
{
    /// <summary>
    /// Information about number.
    /// </summary>
    public class NumberInfo : ModelBase
    {
        public NumberInfo(JObject jsonObject)
        {
            UpdateProperties(jsonObject);
        }

        public String GetName()
        {
            return GetPropertyAsString("name");
        }

        public String GetNumber()
        {
            return GetPropertyAsString("number");
        }

        public DateTime? GetCreated()
        {
            return GetPropertyAsDate("created");
        }

        public DateTime? GetUpdated()
        {
            return GetPropertyAsDate("updated");
        }

        public override String ToString()
        {
            return "NumberInfo{" +
                   "name='" + GetName() + '\'' +
                   ", number='" + GetNumber() + '\'' +
                   ", created=" + GetCreated() +
                   ", updated=" + GetUpdated() +
                   '}';
        }
    }
}
