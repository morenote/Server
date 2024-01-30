using MoreNote.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace morenote_sync_cli.Models.Model.API
{
    public class ApiNotebook
    {
        public string NotebookId { get; set; }
        public string UserId { get; set; }
        public string ParentNotebookId { get; set; }
        public int Seq { get; set; }//顺序
        public string Title { get; set; }
        public string UrlTitle { get; set; }
        public bool IsBlog { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int Usn { get; set; }
        public bool IsDeleted { get; set; }

        public static ApiNotebook InstanceFormJson(string json)
        {
            var book = JsonSerializer.Deserialize<ApiNotebook>(json, MyJsonConvert.GetSimpleOptions());
            return book;
        }

        public static ApiNotebook[] InstanceArrayFormJson(string json)
        {
            var books = JsonSerializer.Deserialize<ApiNotebook[]>(json, MyJsonConvert.GetOptions());
            return books;
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