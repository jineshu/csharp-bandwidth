using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandwidth.Net
{
    public interface IBandwidthClient
    {
        RestResponse Get(String uri, Dictionary<String, Object> mapParams);
    }
}
