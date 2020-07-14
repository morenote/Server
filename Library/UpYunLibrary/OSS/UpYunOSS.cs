using System.Text.Json;

namespace UpYunLibrary.OSS
{
    public class UpYunOSS
    {
        public static string GetPolicy(UPYunOSSOptions options)
        {
            string json = JsonSerializer.Serialize(options, JsonConvert.GetOptions());
            json = json.Replace("save_key", "save-key");
            var policy = Base64Helper.Encode(json);
            return policy;
        }

        public static string GetSignature(string policy, string form_api_secret)
        {
            var signature = SHA.MD5Encrypt(policy + '&' + form_api_secret);
            return signature;
        }
    }
}