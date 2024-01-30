using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace morenote_sync_cli.Utils
{
    public class HttpClientUtil
    {
        public static string HttpGet(string url)
        {
            var httpClient = new HttpClient();

            var response = httpClient.GetAsync(url).Result;
            var str = response.Content.ReadAsStringAsync().Result;
            return str;
        }
    }
}