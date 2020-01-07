using System;
using System.ComponentModel.DataAnnotations;

namespace MoreNote.Logic.Entity
{
    public class NoteFile
    {
        public long FileId;//服务器端id
        public long LocalFileId;//LocalFileId
        public string Type;// images/png, doc, xls, 根据fileName确定
        public string Title;
        public bool HasBody;// 传过来的值是否要更新内容
        public bool IsAttach;// 是否是附件, 不是附件就是图片
    }
    public class ApiNote
    {
        public long NoteId;
        public long NotebookId;
        public long UserId;
        public string Title;
        public string Desc;
        //	ImgSrc     string
        public string[] Tags;
        public string Abstract;
        public string Content;
        public bool IsMarkdown;
    }
    // 内容
    public class ApiNoteContent
    {
        long NoteId;
        long UserId;
        string Content;
    }
    //----------
    // 用户信息
    //----------
    public class ApiUser
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Verified { get; set; }
        public string Logo { get; set; }

    }
    public class ApiGetSyncState 
    {
       public  int LastSyncUsn { get; set; }
        public DateTime LastSyncTime { get; set; }//"上次同步时间"(暂时无用)}
    }

    public class ApiNotebook
    {
        public long NotebookId;
        public long UserId;
        public string ParentNotebookId;
        public string Seq;//顺序
        public string Title;
        public string UrlTitle;
        public string IsBlog;
        public string CreatedTime;
        public string UpdatedTime;
        public string Usn;
        public string IsDeleted;
    }
    //---------
    // api 返回
    //--------

    // 一般返回
    public class ApiRe
    {
        public bool Ok { get; set; }
        public string Msg { get; set; }
    }
    // auth
    public class AuthOk
    {
        public bool Ok { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
    // 供notebook, note, tag更新的返回数据用
    public class ReUpdate
    {
        [Key]
        public bool Ok;
        public string Msg;
        public int Usn;
    }

}
