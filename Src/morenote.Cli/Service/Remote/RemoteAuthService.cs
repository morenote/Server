using morenote_sync_cli.Models.Model.API;
using morenote_sync_cli.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace morenote_sync_cli.Service.Remote
{
    public class RemoteAuthService
    {
        private string url;

        public RemoteAuthService(string url)
        {
            this.url = url;
        }

        public AuthOk Login(string email, string pwd)
        {
            
            var json = HttpClientUtil.HttpGet($"{this.url}/api/Auth/login?email={email}&pwd={pwd}");
            var authOk = AuthOk.InstanceFormJson(json);
            return authOk;
        }
    }
}