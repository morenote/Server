using MoreNote.Common.Cryptography;
using MoreNote.Common.Util;
using MoreNote.Common.Utils;

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

using UpYunLibrary.ContentRecognition;

namespace UpYunLibrary.OSS
{
    public class UpYunOSS
    {
        public static string GetPolicy(UPYunOSSOptions options)
        {
            string json = JsonSerializer.Serialize(options, MyJsonConvert.GetOptions());
            json = json.Replace("save_key", "save-key");
            var policy = Base64Helper.Encode(json);
            return policy;
        }
       public static string GetSignature(string policy, string form_api_secret)
        {
            var signature = SHAEncryptHelper.MD5Encrypt(policy + '&' + form_api_secret);
            return signature;
        }
    }
}
