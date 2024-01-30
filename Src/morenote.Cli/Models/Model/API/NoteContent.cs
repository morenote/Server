using MoreNote.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace morenote_sync_cli.Models.Model.API
{
    public class NoteContent
    {
        public string NoteContentId { get; set; }

        public string NoteId { get; set; }

        public string UserId { get; set; }

        public bool IsBlog { get; set; } // 为了搜索博客

        public string Content { get; set; }//内容

        //public string WebContent{ get;set;}//为web页面优化的内容

        public string Abstract { get; set; } // 摘要, 有html标签, 比content短, 在博客展示需要, 不放在notes表中

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string UpdatedUserId { get; set; } // 如果共享了,  并可写, 那么可能是其它他修改了

        public bool IsHistory { get; set; }//是否是历史纪录


           public static NoteContent InstanceFormJson(string json)
        {
            NoteContent noteContent = JsonSerializer.Deserialize<NoteContent>(json, MyJsonConvert.GetOptions());
            return noteContent;
        }
    }
}