using MoreNote.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace morenote_sync_cli.Models.Model.API
{
    public class Note
    {
        // // 必须要设置bson:"_id" 不然mgo不会认为是主键

        public string NoteId { get; set; }

        public string UserId { get; set; }//  // 谁的

        public string CreatedUserId { get; set; }// // 谁创建的(UserId != CreatedUserId, 是因为共享). 只是共享才有, 默认为空, 不存 必须要加omitempty

        public string NotebookId { get; set; }

        public string Title { get; set; }//标题

        public string Desc { get; set; }//描述, 非html

        public string Src { get; set; }//来源, 2016/4/22

        public string ImgSrc { get; set; }//图片, 第一张缩略图地址

        public string[] Tags { get; set; }

        public bool IsTrash { get; set; }//是否是trash, 默认是false

        public bool IsBlog { get; set; }/// 是否设置成了blog 2013/12/29 新加

        public string UrlTitle { get; set; }// // 博客的url标题, 为了更友好的url, 在UserId, UrlName下唯一

        public bool IsRecommend { get; set; }// 是否为推荐博客 2014/9/24新加

        public bool IsTop { get; set; }//log是否置顶

        public bool HasSelfDefined { get; set; } // 是否已经自定义博客图片, desc, abstract

        // 2014/9/28 添加评论社交功能

        public int ReadNum { get; set; } // 阅读次数

        public int LikeNum { get; set; } // 点赞次数

        public int CommentNum { get; set; } // 评论次数

        public bool IsMarkdown { get; set; }// 是否是markdown笔记, 默认是false

        public int AttachNum { get; set; }//// 2014/9/21, attachments num

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public DateTime RecommendTime { get; set; }// 推荐时间

        public DateTime PublicTime { get; set; } // 发表时间, 公开为博客则设置

        public string UpdatedUserId { get; set; } // 如果共享了, 并可写, 那么可能是其它他修改了

        // 2015/1/15, 更新序号

        public int Usn { get; set; } // UpdateSequenceNum

        public bool IsDeleted { get; set; } // 删除位

        public bool IsPublicShare { get; set; }

        public string ContentId { get; set; }//当前笔记的笔记内容

        public string AccessPassword { get; set; }//当前笔记的访问密码

        public static Note InstanceFormJson(string json)
        {
            var book = JsonSerializer.Deserialize<Note>(json, MyJsonConvert.GetSimpleOptions());
            return book;
        }

        public static Note[] InstanceArrayFormJson(string json)
        {
            var books = JsonSerializer.Deserialize<Note[]>(json, MyJsonConvert.GetOptions());
            return books;
        }
    }
}