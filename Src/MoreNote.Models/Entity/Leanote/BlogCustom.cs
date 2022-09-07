
using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreNote.Logic.Entity
{

    [Table("blog_info_custom")]
    // 仅仅为了博客的主题
    public class BlogInfoCustom: BaseEntity
    {


       
        [Column("user_id")]
        public long? UserId{ get; set; }
        [Column("username")]
        public string Username{ get; set; }
        [Column("user_logo")]
        public string UserLogo{ get; set; }
        [Column("title")]
        public string Title{ get; set; }
        [Column("sub_title")]
        public string SubTitle{ get; set; }
        [Column("logo")]
        public string Logo{ get; set; }
        [Column("open_comment")]
        public string OpenComment{ get; set; }
        [Column("comment_type")]
        public string CommentType{ get; set; }
        [Column("theme_id")]
        public string ThemeId{ get; set; }
        [Column("sub_domain")]
        public string SubDomain{ get; set; }
        [Column("domain")]
        public string Domain{ get; set; }
      
    }
    [Table("post")]
    public class Post: BaseEntity
    {
       
        [Column("note_id")]
        public long? NoteId{ get; set; }
        [Column("title")]
        public string Title{ get; set; }
        [Column("url_title")]
        public string UrlTitle{ get; set; }
        [Column("img_src")]
        public string ImgSrc{ get; set; }
        [Column("created_time")]
        public DateTime CreatedTime{ get; set; }
        [Column("updated_time")]
        public DateTime UpdatedTime { get; set; }
        [Column("public_time")]
        public DateTime PublicTime { get; set; }
        [Column("desc")]
        public string Desc{ get; set; }
        [Column("abstract")]
        public string Abstract{ get; set; }
        [Column("content")]
        public string Content{ get; set; }
        [Column("tags")]
        public string[] Tags{ get; set; }
        [Column("comment_num")]
        public int CommentNum{ get; set; }
        [Column("read_num")]
        public int ReadNum{ get; set; }
        [Column("like_num")]
        public int LikeNum{ get; set; }
        [Column("is_markdown")]
        public bool IsMarkdown{ get; set; }
    }
    // 归档 数据容器 不需要数据库储存
    public class ArchiveMonth
    {
        public int Month{ get; set; }
        public Post[] Posts{ get; set; }
    }
    public class Archive
    {
        public int Year { get; set; }
        //public ArchiveMonth[] MonthAchives { get; set; }
        public Note[] Posts { get; set; }
    }
    [Table("cate")]
    public class Cate: BaseEntity
    {
        
        [Column("parent_cate_id")]
        public string ParentCateId { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("url_title")]
        public string UrlTitle { get; set; }
        [Column("children")]
        public Cate[] Children { get; set; }
    }
}
