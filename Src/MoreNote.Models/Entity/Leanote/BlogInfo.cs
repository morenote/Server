using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreNote.Logic.Entity
{
    public  class BlogInfo
    {
        public String UserId { get; set; }
        public String Username { get; set; }
        public String UserLogo { get; set; }
        public String Title { get; set; }
        public String SubTitle { get; set; }
        public String Logo { get; set; }
        public bool OpenComment { get; set; }
        public string CommentType { get; set; } // leanote, or disqus
        public String DisqusId { get; set; }
        public String ThemeId { get; set; }
        public String SubDomain { get; set; }
        public String Domain { get; set; }

      


    }
   
    // 只为blog, 不为note copy hahaha
    public class BlogItem
    {
     
        public Note Note{ get; set; }
        public string Abstract{ get; set; }
        public string Content{ get; set; } //可能是content的一部分, 截取.点击more后就是整个信息了
        public bool HasMore{ get; set; }//是否是否还有
        public User User{ get; set; }//用户信息

    }
    [Table("user_blog_base")]
    public class UserBlogBase: BaseEntity
    {
       
        [Column("logo")]
        public string Logo { get; set; } // logo 
        [Column("title")]
        public string Title { get; set; } // 标题 
        [Column("sub_title")]
        public string SubTitle { get; set; } // 副标题 

    }
    [Table("user_blog_comment")]
    public class UserBlogComment: BaseEntity
    {
       
        
        [Column("can_comment")]
        public bool CanComment { get; set; } // 是否可以评论 
        [Column("comment_type")]
        public string CommentType { get; set; } // default   CommentType // leanote, or disqus
        [Column("disqus_id")]
        public string? DisqusId { get; set; }  
    }
    [Table("user_blog_style")]
    public class UserBlogStyle: BaseEntity
    {
        
        [Column("style")]
        public string Style { get; set; } // 风格 
        [Column("css")]
        public string Css { get; set; } // 自定义css 

    }
    // 每个用户一份博客设置信息
    [Table("user_blog")]
    public class UserBlog: BaseEntity
    {
        
        [Column("user_id")]
        public long? UserId { get; set; } // 谁的 
        [Column("logo")]
        public string? Logo { get; set; } // 
        [Column("title")]
        public string? Title { get; set; } // 标题 
        [Column("sub_title")]
        public string? SubTitle { get; set; } // 副标题 
        [Column("about_me")]
        public string? AboutMe { get; set; } // 关于我, 
        [Column("can_comment")]

        public bool CanComment { get; set; } // 是否可以评论 
        [Column("comment_type")]

        public string? CommentType { get; set; } // default 
        [Column("disqus_id")]
        public string? DisqusId { get; set; } // default 


        [Column("style")]
        public string? Style { get; set; } // 风格 
        [Column("css")]
        public string? Css { get; set; } // 自定义css 
        [Column("theme_id")]

        public long? ThemeId { get; set; } // 主题Id 
        [Column("theme_path")]
        public string? ThemePath { get; set; } // 储存值, 从Theme中获取, 相对路径 public/ 

        [Column("cate_ids")]
        public string[]? CateIds { get; set; } // 分类Id, 排序好的
        [Column("singles")]
        public string[]? Singles { get; set; } //单页, 排序好的, map包含: ["Title"], ["SingleId"]
        [Column("per_page_size")]
        public int PerPageSize { get; set; } //  
        [Column("sort_field")]
        public string? SortField { get; set; } // 排序字段 
        [Column("is_asc")]
        public bool IsAsc { get; set; } // // 排序类型, 降序, 升序, 默认是false, 表示降序, 
        [Column("sub_domain")]
        public string? SubDomain { get; set; } // 二级域名 
        [Column("domain")]
        public string? Domain { get; set; } // 自定义域名 
    }
    //博客统计信息
    [Table("blog_stat")]
    public class BlogStat: BaseEntity
    {
        
        [Column("node_id")]
        public long? NodeId {get;set;}
        [Column("read_num")]
        public int ReadNum { get; set; } // 阅读次数 
        [Column("like_num")]
        public int LikeNum { get; set; } // 点赞次数 
        [Column("comment_num")]
        public int CommentNum { get; set; } // 评论次数 

    }
    //// 单页
    [Table("blog_single")]
    public class BlogSingle: BaseEntity
    {
        
        [Column("user_id")]
        public long? UserId { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("url_title")]
        public string UrlTitle { get; set; }// 2014/11/11
        [Column("content")]
        public string Content { get; set; }
        [Column("updated_time")]
        public DateTime UpdatedTime { get; set; }
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }
      

    }

    //------------------------
    // 社交功能, 点赞, 分享, 评论

    // 点赞记录
    [Table("blog_like")]
    public class BlogLike: BaseEntity
    {
       
        [Column("note_id")]
        public long? NoteId { get; set; }
        [Column("user_id")]
        public long? UserId { get; set; }
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }

    }
    // 评论
    [Table("blog_comment")]
    public class BlogComment: BaseEntity
    {
        
        [Column("note_id")]
        public long? NoteId { get; set; }
        [Column("user_id")]
        public long? UserId { get; set; }// UserId回复ToUserId
        [Column("content")]
        public string Content { get; set; } // 评论内容
        [Column("to_comment_id")]
        public long? ToCommentId { get; set; }// 对某条评论进行回复
        [Column("to_user_id")]
        public long? ToUserId { get; set; } // 为空表示直接评论, 不回空表示回复某人
        [Column("like_num")]
        public int LikeNum { get; set; }// 点赞次数, 评论也可以点赞
        [Column("like_user_ids")]
        public long?[] LikeUserIds { get; set; }// 点赞的用户ids
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 评论是否允许公开显示，评论经过批准允许后才可以公开显示
        /// </summary>
        [Column("allow")]
        public bool Allow { get; set; }

    }
    [Table("blog_comment_public")]
    public class BlogCommentPublic: BaseEntity
    {
        
        [Column("blog_comment")]
        BlogComment BlogComment { get; set; }
        [Column("is_i_like_it")]
        public bool IsILikeIt { get; set; }
    }

    public struct BlogUrls
    {
        
        public long? BlogUrlsId { get; set; }
        public string IndexUrl { get; set; }
        public string CateUrl { get; set; }
        public string SearchUrl { get; set; }
        public string SingleUrl { get; set; }
        public string PostUrl { get; set; }
        public string ArchiveUrl { get; set; }
        public string TagsUrl { get; set; }
        public string TagPostsUrl { get; set; }
    }
    

}
