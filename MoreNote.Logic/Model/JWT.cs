using System;
using System.Collections.Generic;
using System.Text;
using MoreNote.Common.Utils;
using MoreNote.Common.Utils;

namespace MoreNote.Logic.Model
{
    public class JWT
    {
        public JWT_Header Header { get; set; }
        public JWT_Payload Payload { get; set; }

        public string Signature { get; set; }

        public  JWT GetJWTFormBase64(string base64)
        {
            return null;
        }
        public  JWT GetJWTFormJson(string base64)
        {
            return null;
        }
        public  JWT GetJWT(long? tokenId,string userNmae,long? userId, string group, long? exp= 31536000)
        {

            JWT_Header header = new JWT_Header()
            {
                alg = "SHA1"
            };
            JWT_Payload payload = new JWT_Payload
            {
                tokenId = tokenId,
                iss = "localhost",
                username = userNmae,
                userId = userId,
                group = group,
                startTime = UnixTimeHelper.GetTimeStampInLong(),
                exp =exp ,
                random = RandomTool.CreatSafeNum(8)
            };
            StringBuilder message = new StringBuilder();
            message.Append(header.alg);
            message.Append(payload.tokenId);
            message.Append(payload.iss);
            message.Append(payload.username);
            message.Append(payload.userId);
            message.Append(payload.group);
            message.Append(payload.startTime);
            message.Append(payload.exp);
            message.Append(payload.random);
            string password = "";
            string signature = SHAEncryptHelper.Hash1Encrypt(message + password);
            JWT jWT = new JWT()
            {
                Header = header,
                Payload = payload,
                Signature = signature
            };
            return jWT;
        }

        public  string ToJSon(string base64)
        {
            return null;

        }
        public  string ToBase64(string base64)
        {
            return null;
        }
        public bool IsValid()
        {
            return false;
        }




    }
    public class JWT_Header
    {
        public string alg { get; set; }

    }
    public class JWT_Payload
    {
        public long? tokenId { get; set; }
        public string iss { get; set; }
        public string username { get; set; }
        public long? userId { get; set; }
        public string group { get; set; }
        public long? startTime { get; set; }
        public long? exp { get; set; }
        public string random { get; set; }
    }
}
