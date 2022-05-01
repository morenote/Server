using System;

namespace MoreNote.Logic.Entity
{
    /// <summary>
    /// 对应Api.go的NoteFile数据结构
    /// </summary>
    public class APINoteFile
    {
        public string FileId { get; set; }//服务器端id
        public string LocalFileId { get; set; }//LocalFileId
        public string Type { get; set; }// images/png, doc, xls, 根据fileName确定
        public string Title { get; set; }
        public bool HasBody { get; set; }// 传过来的值是否要更新内容
        public bool IsAttach { get; set; }// 是否是附件, 不是附件就是图片
    }

    /// <summary>
    /// 供API参数绑定使用的类
    /// </summary>
    public class ApiNote
    {
        public string NoteId { get; set; }
        public string NotebookId { get; set; }
        

        public string UserId { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }

        //	ImgSrc     string
        public string[] Tags { get; set; }

        public string Abstract { get; set; }
        public string Content { get; set; }
        public bool? IsMarkdown { get; set; }

        //	FromUserId string // 为共享而新建
        public bool? IsBlog { get; set; }

        public bool? IsTrash { get; set; }
        public bool? IsDeleted { get; set; }
        public int? Usn { get; set; }
        public APINoteFile[] Files { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public DateTime PublicTime { get; set; }
    }

    // 内容
    public class ApiNoteContent
    {
        public long? NoteId { get; set; }
        public long? UserId { get; set; }
        public string Content { get; set; }
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
        public long? LastSyncTime { get; set; }//"上次同步时间"(暂时无用)} unix时间戳
        public int LastSyncUsn { get; set; }
    }

    public class ApiNotebook
    {
        public long? NotebookId { get; set; }
        public long? UserId { get; set; }
        public long? ParentNotebookId { get; set; }
        public int Seq { get; set; }//顺序
        public string Title { get; set; }
        public string UrlTitle { get; set; }
        public bool IsBlog { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int Usn { get; set; }
        public bool IsDeleted { get; set; }
    }

    //---------
    // api 返回
    //--------

    // 一般返回
    public class ApiRe
    {
        public long? Id { get; set; }//唯一标识（可选）
        public DateTime? Timestamp { get; set; } = DateTime.Now;//消息创建时间（可选）
        public bool Ok { get; set; }//消息状态  成功或失败
        public string Msg { get; set; }//提示信息或者错误信息，或者其他描述性辅助信息（可选）
        public int ErrorCode { get; set; }//错误代码
        public dynamic Data { get; set; }//返回的数据
        public PageInfo  PageInfo { get; set; }//分页信息

    }
    /// <summary>
    /// 分页信息
    /// </summary>
    public struct PageInfo
    {
        public int PageNumber { get; set; }//分页位置 从0开始 
        public int PageSize { get; set; }//分页的大小,每页包含多少数据 
        public int PageSum { get; set; }//一共有多少页

    }

    public struct Sign
    {

        public string Pubk { get;set; }//签署者公钥
           
    }
    public class ApiStatusCode
    {
        public const int OK = 200;
        public const int NotModified = 304;
        public const int BadRequest = 400;
        public const int Unauthorized = 401;
        public const int Forbidden = 403;
        public const int NotFound = 404;
        public const int MethodNotAllowed = 405;
        
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
        public bool Ok { get; set; }
        public string Msg { get; set; }
        public int Usn { get; set; }
    }
}