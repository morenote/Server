using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NpgsqlTypes;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace MoreNote.Logic.Entity
{
    [Table("note"),Index(nameof(UserId),nameof(IsBlog),nameof(IsDeleted))]
    public class Note
    {
        // // 必须要设置bson:"_id" 不然mgo不会认为是主键
        [Key] 
        [Column("note_id")] 
        public long? NoteId{ get; set; }
        
        [Column("user_id")]
        public long? UserId { get; set; }//  // 谁的

        [Column("notes_repository_id")]
        public long? NotesRepositoryId { get; set; }//仓库id

        [Column("created_user_id")]
        public long? CreatedUserId { get; set; }// // 谁创建的(UserId != CreatedUserId, 是因为共享). 只是共享才有, 默认为空, 不存 必须要加omitempty
        [Column("notebook_id")]
        public long? NotebookId { get; set; }
        [Column("title")]
        public string? Title { get; set; }//标题
        [Column("title_vector"),JsonIgnore]
        public NpgsqlTsVector? TitleVector { get; set; }
        [Column("desc")]
        public string? Desc { get; set; }//描述, 非html
        [Column("src")]
        public string? Src { get; set; }//来源, 2016/4/22
        [Column("img_src")]
        public string? ImgSrc { get; set; }//图片, 第一张缩略图地址
        [Column("tags")]
        public string[]? Tags { get; set; }
        [Column("is_trash")]
        public bool IsTrash { get; set; }//是否是trash, 默认是false
        [Column("is_blog")]
        public bool IsBlog { get; set; }/// 是否设置成了blog 2013/12/29 新加
        [Column("url_title")]
        public string UrlTitle { get; set; }// // 博客的url标题, 为了更友好的url, 在UserId, UrlName下唯一
        [Column("is_recommend")]
        public bool IsRecommend { get; set; }// 是否为推荐博客 2014/9/24新加
        [Column("is_top")]
        public bool IsTop { get; set; }//log是否置顶
        [Column("has_self_defined")]
        public bool HasSelfDefined { get; set; } // 是否已经自定义博客图片, desc, abstract

        // 2014/9/28 添加评论社交功能
        [Column("read_num")]
        public int ReadNum { get; set; } // 阅读次数 
        [Column("like_num")]
        public int LikeNum { get; set; } // 点赞次数 
        [Column("comment_num")]
        public int CommentNum { get; set; } // 评论次数 

        [Column("is_markdown")]
        public bool IsMarkdown { get; set; }// 是否是markdown笔记, 默认是false
        [Column("attach_num")]
        public int AttachNum { get; set; }//// 2014/9/21, attachments num
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }
        [Column("updated_time")]
        public DateTime UpdatedTime { get; set; }
        [Column("recommend_time")]
        public DateTime RecommendTime { get; set; }// 推荐时间
        [Column("public_time")]
        public DateTime PublicTime { get; set; } // 发表时间, 公开为博客则设置
        [Column("updated_user_id")]
        public long? UpdatedUserId { get; set; } // 如果共享了, 并可写, 那么可能是其它他修改了

        // 2015/1/15, 更新序号
        [Column("usn")]
        public int Usn { get; set; } // UpdateSequenceNum 
        [Column("is_deleted")]
        public bool IsDeleted { get; set; } // 删除位 
        [Column("is_public_share")]
        public bool IsPublicShare { get; set; }
        [Column("content_id")]
        public long? ContentId { get; set; }//当前笔记的笔记内容 
        [Column("access_password")]
        public string? AccessPassword  { get; set; }//当前笔记的访问密码



    }
    /// <summary>
    /// <para>笔记内容和可以被允许修改的属性</para>
    /// <para>
    ///  一个note可以拥有多个NoteContent,但是只允许有一个处于活动状态
    ///  剩余的NoteContent被识别为历史记录
    /// </para>
    /// </summary>
    [Table("note_content"),Index(nameof(NoteId),nameof(UserId),nameof(IsHistory))]
    public class NoteContent
    {
        [Key]
        [Column("note_content_id")]
        //[JsonConverter(typeof(string))] //解决序列化时被转成数值的问题
        public long? NoteContentId {get;set;}
        [Column("note_id")]
        public long? NoteId { get; set; }
        [Column("user_id")]
        public long? UserId { get; set; }
        [Column("is_blog")]
        public bool IsBlog { get; set; } // 为了搜索博客 
        [Column("content")]
        public string? Content { get; set; }//内容
        [Column("content_vector"),JsonIgnore]
        public NpgsqlTsVector? ContentVector { get; set; }


        //public string WebContent{ get;set;}//为web页面优化的内容
        [Column("abstract")]
        public string? Abstract { get; set; } // 摘要, 有html标签, 比content短, 在博客展示需要, 不放在notes表中
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }
        [Column("updated_time")]
        public DateTime UpdatedTime { get; set; }
        [Column("updated_user_id")]
        public long? UpdatedUserId { get; set; } // 如果共享了,  并可写, 那么可能是其它他修改了
        [Column("is_history")]
        public bool IsHistory { get; set; }//是否是历史纪录
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
     
        public long? NoteId { get; set; }
        public long? UserId { get; set; } // 所属者 
        public EachHistory[] Histories { get; set; }
    }

    // 为了NoteController接收参数

    // 更新note或content
    // 肯定会传userId(谁的), NoteId
    // 会传Title, Content, Tags, 一种或几种

    public class NoteOrContent
    {
      
        public long? NotebookId { get; set; }
        public long? NoteId { get; set; }
        public long? UserId { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Src { get; set; }
        public string ImgSrc { get; set; }
        public string Tags { get; set; }
    
        public string Content { get; set; }
        public string Abstract { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsMarkdown { get; set; }

        public long? FromUserId { get; set; }//// 为共享而新建
        public bool? IsBlog { get; set; }//是否是blog, 更新note不需要修改, 添加note时才有可能用到, 此时需要判断notebook是否设为Blog
    }
    // 分开的
    public class NoteAndContentSep
    {
        public long? NoteAndContentSepId{ get; set; }
        public Note NoteInfo{ get; set; }
        public NoteContent NoteContentInfo{ get; set; }
    }


}
