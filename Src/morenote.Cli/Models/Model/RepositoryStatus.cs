using MoreNote.Common.Utils;
using morenote_sync_cli.Models.Model.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace morenote_sync_cli.Models.Model
{
    public class RepositoryStatus
    {
        public string Address { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }


        public void SetAuthOk(AuthOk authOk)
        {
            this.Token = authOk.Token;
            this.UserId = authOk.UserId;
            this.Username = authOk.Username;
        }

        public static RepositoryStatus InstanceFormJson(string json)
        {
            var repositoryStatus = JsonSerializer.Deserialize<RepositoryStatus>(json, MyJsonConvert.GetSimpleOptions());
            return repositoryStatus;
        }

        public string ToJson()
        {
            string json = JsonSerializer.Serialize(this, MyJsonConvert.GetSimpleOptions());
            return json;
        }

        public string ToBeautifulJson()
        {
            string json = JsonSerializer.Serialize(this, MyJsonConvert.GetBeautifulOptions());
            return json;
        }
    }
}