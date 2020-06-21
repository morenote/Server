using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreNote.Logic.Entity
{
    class BlogInfo
    {

    }
    // 只为blog, 不为note copy hahaha
    public class BlogItem
    {
        [Key]
        public long NoteId{ get; set; }
        public string Abstract{ get; set; }
        public string Content{ get; set; } //可能是content的一部分, 截取.点击more后就是整个信息了
        public bool HasMore{ get; set; }//是否是否还有
        public User user{ get; set; }//用户信息

    }
    public class UserBlogBase
    {
        [Key]
        public string Logo { get; set; } // logo 
        public string Title { get; set; } // 标题 
        public string SubTitle { get; set; } // 副标题 

    }
    public class UserBlogComment
    {
        [Key]
        public long UserBlogCommentId { get; set; }
        public bool CanComment { get; set; } // 是否可以评论 
        public string CommentType { get; set; } // default 
        public string DisqusId { get; set; }  
    }

    public class UserBlogStyle
    {
        [Key]
        public long UserBlogStyleId { get; set; }
        public string Style { get; set; } // 风格 
        public string Css { get; set; } // 自定义css 

    }
    // 每个用户一份博客设置信息
    public class UserBlog
    {
        [Key]
        public long UserId { get; set; } // 谁的 
        public string Logo { get; set; } // 
        public string Title { get; set; } // 标题 
        public string SubTitle { get; set; } // 副标题 
        public string AboutMe { get; set; } // 关于我, 

        public bool CanComment { get; set; } // 是否可以评论 

        public string CommentType { get; set; } // default 
        public string DisqusId { get; set; } // default 



        public string Style { get; set; } // 风格 
        public string Css { get; set; } // 自定义css 

        public long ThemeId { get; set; } // 主题Id 
        public string ThemePath { get; set; } // 储存值, 从Theme中获取, 相对路径 public/ 


        public string[] CateIds { get; set; } // 分类Id, 排序好的
        public string[] Singles { get; set; } //单页, 排序好的, map包含: ["Title"], ["SingleId"]
        
        public int PerPageSize { get; set; } //  
        public string SortField { get; set; } // 排序字段 
        public bool IsAsc { get; set; } // // 排序类型, 降序, 升序, 默认是false, 表示降序, 

        public string SubDomain { get; set; } // 二级域名 
        public string Domain { get; set; } // 自定义域名 
    }
    //博客统计信息
    public class BlogStat
    {
        [Key]
        public long NodeId {get;set;}
        public int ReadNum { get; set; } // 阅读次数 
        public int LikeNum { get; set; } // 点赞次数 
        public int CommentNum { get; set; } // 评论次数 

    }
    //// 单页
    public class BlogSingle
    {
        [Key]
        public long SingleId { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string UrlTitle { get; set; }// 2014/11/11
        public string Content { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime CreatedTime { get; set; }
      

    }

    //------------------------
    // 社交功能, 点赞, 分享, 评论

    // 点赞记录
    public class BlogLike
    {
        [Key]
        public long LikeId { get; set; }
        public long NoteId { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedTime { get; set; }

    }
    // 评论
    public class BlogComment
    {
        [Key]
        public long CommentId { get; set; }
        public long NoteId { get; set; }

        public long UserId { get; set; }// UserId回复ToUserId
        public string Content { get; set; } // 评论内容

        public string ToCommentId { get; set; }// 对某条评论进行回复
        public long ToUserId { get; set; } // 为空表示直接评论, 不回空表示回复某人

        public int LikeNum { get; set; }// 点赞次数, 评论也可以点赞
        public long[] LikeUserIds { get; set; }// 点赞的用户ids

        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 评论是否允许公开显示，评论经过批准允许后才可以公开显示
        /// </summary>
        public bool Allow { get; set; }

    }
    public class BlogCommentPublic
    {
        [Key]
        public long BlogCommentPublicId { get; set; }
        BlogComment blogComment { get; set; }
        public bool IsILikeIt { get; set; }
    }
    public struct BlogUrls
    {
        
        public long BlogUrlsId { get; set; }
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
