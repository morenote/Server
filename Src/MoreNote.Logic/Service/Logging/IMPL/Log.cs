using MoreNote.CryptographyProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.Logging.IMPL
{
    public class Log
    {
        public string logText { get; set; }
        public string mac { get; set; }

        public Log()
        {

        }
        public Log(string logText, string mac)
        {
            this.logText = logText ?? throw new ArgumentNullException(nameof(logText));
            this.mac = mac ?? throw new ArgumentNullException(nameof(mac));
        }
        public Log(string logText)
        {
            this.logText = logText ?? throw new ArgumentNullException(nameof(logText));
           
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        public static Log FromJson(string json)
        {
            return JsonSerializer.Deserialize<Log>(json);
        }

        public async Task<Log>  AddMac(ICryptographyProvider cryptographyProvider)
        {
            var bytes = Encoding.UTF8.GetBytes(this.logText);
            var base64 = Convert.ToBase64String(bytes);
            this.mac = await cryptographyProvider.Hmac(base64);
            return this;
        }
        public async Task<bool> VerifyHmac(ICryptographyProvider cryptographyProvider)
        {
            var bytes = Encoding.UTF8.GetBytes(this.logText);
            var base64 = Convert.ToBase64String(bytes);
            return await cryptographyProvider.VerifyHmac(base64, mac);
        }
    }
}
