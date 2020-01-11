using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace MoreNote.Logic.Entity
{
    public class Note
    {
        [Key]
        public long NoteId { get; set; }// // 必须要设置bson:"_id" 不然mgo不会认为是主键
        public long UserId { get; set; }//  // 谁的
        public long CreatedUserId { get; set; }// // 谁创建的(UserId != CreatedUserId, 是因为共享). 只是共享才有, 默认为空, 不存 必须要加omitempty
        public long NotebookId { get; set; }
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
        public long UpdatedUserId { get; set; } // 如果共享了, 并可写, 那么可能是其它他修改了

        // 2015/1/15, 更新序号
        public int Usn { get; set; } // UpdateSequenceNum 
        public bool IsDeleted { get; set; } // 删除位 
        public bool IsPublicShare { get; set; }
        public long ContentId { get; set; }//当前笔记的笔记内容 


    }
    public class NoteContent
    {
        [Key]
        //[JsonConverter(typeof(string))] //解决序列化时被转成数值的问题
        public long NoteId { get; set; }
        public long UserId { get; set; }
        public bool IsBlog { get; set; } // 为了搜索博客 
        public string Content { get; set; }//内容
        public string Abstract { get; set; } // 摘要, 有html标签, 比content短, 在博客展示需要, 不放在notes表中
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public long UpdatedUserId { get; set; } // 如果共享了,  并可写, 那么可能是其它他修改了

        public int IsHistory { get; set; }//是否是历史纪录
    }
    // 基本信息和内容在一起
    public class NoteAndContent
    {
    
        public Note note{ get; set; }
        public NoteContent noteContent{ get; set; }
    }
    // 历史记录
    // 每一个历史记录对象
    public class EachHistory
    {
  
        public string UpdatedUserId { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string Content { get; set; }
    }
    public class NoteContentHistory
    {
     
        public long NoteId { get; set; }
        public long UserId { get; set; } // 所属者 
        public EachHistory[] Histories { get; set; }
    }

    // 为了NoteController接收参数

    // 更新note或content
    // 肯定会传userId(谁的), NoteId
    // 会传Title, Content, Tags, 一种或几种

    public class NoteOrContent
    {
        [Key]
        public long NotebookId { get; set; }
        public long NoteId { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Src { get; set; }
        public string ImgSrc { get; set; }
        public string Tags { get; set; }
    
        public string Content { get; set; }
        public string Abstract { get; set; }
        public bool IsNew { get; set; }
        public bool IsMarkdown { get; set; }

        public string FromUserId { get; set; }//// 为共享而新建
        public bool IsBlog { get; set; }//是否是blog, 更新note不需要修改, 添加note时才有可能用到, 此时需要判断notebook是否设为Blog


    }
    // 分开的
    public class NoteAndContentSep
    {
        public long NoteAndContentSepId{ get; set; }
        
        public Note NoteInfo{ get; set; }
        public NoteContent NoteContentInfo{ get; set; }
    }

}
