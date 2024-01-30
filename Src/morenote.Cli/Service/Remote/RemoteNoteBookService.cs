using morenote_sync_cli.Models.Model.API;
using morenote_sync_cli.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace morenote_sync_cli.Service.Remote
{
    public class RemoteNoteBookService
    {
        string baseURL;
        public RemoteNoteBookService(string url)
        {
            this.baseURL = url;
        }

        public ApiNotebook[] GetNotebooks(string token)
        {
            var url=$"{baseURL}/api/notebook/getNotebooks?token={token}";
            var json=HttpClientUtil.HttpGet(url);
           
            var books=ApiNotebook.InstanceArrayFormJson(json);
            return books;
        }
    }
}
