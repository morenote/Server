using MoreNote.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace morenote_sync_cli.Models.Model.API
{
    public class AuthOk
    {
        public bool Ok { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

        public string Msg { get; set; }

        public static AuthOk InstanceFormJson(string json)
        {
            AuthOk authOk = JsonSerializer.Deserialize<AuthOk>(json, MyJsonConvert.GetSimpleOptions());
            return authOk;
        }

        public string ToJson()
        {
            string json = JsonSerializer.Serialize(this, MyJsonConvert.GetSimpleOptions());
            return json;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("OK=" + this.Ok + "\r\n");
            sb.Append("Token=" + this.Token + "\r\n");
            sb.Append("UserId=" + this.UserId + "\r\n");
            sb.Append("Email=" + this.Email + "\r\n");
            sb.Append("Username=" + this.Username + "\r\n");
            sb.Append("Msg=" + this.Msg + "\r\n");
            return sb.ToString();
        }
    }
}