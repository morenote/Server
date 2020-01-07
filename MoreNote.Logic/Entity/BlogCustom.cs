using leanote.Common.Type;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreNote.Logic.Entity
{
    class BlogCustom
    {
    }
    // 仅仅为了博客的主题
    public class BlogInfoCustom
    {
        [Key]
        public long UserId{ get; set; }
        public string Username{ get; set; }
        public string UserLogo{ get; set; }
        public string Title{ get; set; }
        public string SubTitle{ get; set; }
        public string Logo{ get; set; }
        public string OpenComment{ get; set; }
        public string CommentType{ get; set; }
        public string ThemeId{ get; set; }
        public string SubDomain{ get; set; }
        public string Domain{ get; set; }
      
    }
    public class Post
    {
        [Key]
        public long NoteId{ get; set; }
        public string Title{ get; set; }
        public string UrlTitle{ get; set; }
        public string ImgSrc{ get; set; }
        public DateTime CreatedTime{ get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime PublicTime { get; set; }
        public string Desc{ get; set; }
        public string Abstract{ get; set; }
        public string Content{ get; set; }
        public string[] Tags{ get; set; }
        public int CommentNum{ get; set; }
        public int ReadNum{ get; set; }
        public int LikeNum{ get; set; }
        public bool IsMarkdown{ get; set; }
    }
    // 归档
    public class ArchiveMonth
    {
        [Key]
        public long ArchiveMonthId{ get; set; }
        public int Month{ get; set; }
        public Post[] Posts{ get; set; }
    }
    public class Archive
    {
        [Key]
        public long ArchiveId { get; set; }
        public int Year { get; set; }
        public ArchiveMonth[] MonthAchives { get; set; }
        public Post[] Posts { get; set; }
    }
    public class Cate
    {
        [Key]
        public long CateId { get; set; }
        public string ParentCateId { get; set; }
        public string Title { get; set; }
        public string UrlTitle { get; set; }
        public Cate[] Children { get; set; }
    }
}
