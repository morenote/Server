using System;
using System.ComponentModel.DataAnnotations;

namespace MoreNote.Logic.Entity
{
    public class NoteFile
    {
        public long FileId { get; set; }//服务器端id
        public long LocalFileId { get; set; }//LocalFileId
        public string Type { get; set; }// images/png, doc, xls, 根据fileName确定
        public string Title { get; set; }
        public bool HasBody { get; set; }// 传过来的值是否要更新内容
        public bool IsAttach { get; set; }// 是否是附件, 不是附件就是图片
    }
    public class ApiNote
    {
        public long NoteId { get; set; }
        public long NotebookId { get; set; }
        public long UserId { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        //	ImgSrc     string
        public string[] Tags { get; set; }
        public string Abstract { get; set; }
        public string Content { get; set; }
        public bool IsMarkdown { get; set; }
        //	FromUserId string // 为共享而新建
        public bool IsBlog { get; set; }
        public bool IsTrash { get; set; }
        public bool IsDeleted { get; set; }
        public int Usn { get; set; }
        public NoteFile[] Files { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime PublicTime { get; set; }
        



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
        public  long LastSyncTime { get; set; }//"上次同步时间"(暂时无用)} unix时间戳
        public  int LastSyncUsn { get; set; }
    
    }

    public class ApiNotebook
    {
        public long NotebookId { get; set; }
        public long UserId{ get; set; }
        public long ParentNotebookId{ get; set; }
        public int Seq{ get; set; }//顺序
        public string Title{ get; set; }
        public string UrlTitle{ get; set; }
        public bool IsBlog{ get; set; }
        public DateTime CreatedTime{ get; set; }
        public DateTime UpdatedTime{ get; set; }
        public int Usn{ get; set; }
        public bool IsDeleted{ get; set; }
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
