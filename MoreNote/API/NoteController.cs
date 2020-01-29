using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

namespace MoreNote.API
{
    [Route("api/[controller]/[action]")]
    // [ApiController]
    public class NoteController : ApiBaseController
    {
        public NoteController(IHttpContextAccessor accessor) : base(accessor)
        {
        }

        //todo:获取同步的笔记
        //public JsonResult GetSyncNotes([ModelBinder(BinderType = typeof(Hex2LongModelBinder))]long userId,int afterUsn,int maxEntry,string token)
        //{
        //    if (maxEntry==0) maxEntry=100;
        //    ApiNote[] apiNotes=NoteService.GetSyncNotes(userId,afterUsn,maxEntry);
        //    return Json(apiNotes,MyJsonConvert.GetOptions());
        //}
        public JsonResult GetSyncNotes(int afterUsn, int maxEntry, string token)
        {
            if (maxEntry == 0) maxEntry = 100;
            ApiNote[] apiNotes = NoteService.GetSyncNotes(getUserIdByToken(token), afterUsn, maxEntry);
            return Json(apiNotes, MyJsonConvert.GetOptions());
        }
        //todo:得到笔记本下的笔记
        public IActionResult GetNotes(string notebookId, string token)
        {
            Note[] notes = NoteService.ListNotes(getUserIdByToken(token), MyConvert.HexToLong(notebookId), false);
            long myNotebookId = MyConvert.HexToLong(notebookId);
            return null;
        }
        //todo:得到trash
        public IActionResult GetTrashNotes()
        {
            return null;
        }
        //todo:获取笔记
        public IActionResult GetNote()
        {
            return null;
        }
        //todo:得到note和内容
        public IActionResult GetNoteAndContent(string token, string noteId)
        {
            NoteAndContent noteAndContent = NoteService.GetNoteAndContent(MyConvert.HexToLong(noteId), getUserIdByToken());

            if (noteAndContent == null)
            {
                return Json(new ApiRe() { Ok = false, Msg = "" }, MyJsonConvert.GetOptions());

            }
            else
            {
                return Json(new ApiRe()
                {
                    Ok=false,
                    Msg= "GetNoteAndContent_is_error"
                });;
            }

        }
        //todo:格式化URL
        public IActionResult fixPostNotecontent()
        {
            return null;
        }
        //todo:得到内容
        public IActionResult GetNoteContent(string token,string noteId)
        {
            Note note=NoteService.GetNote(MyConvert.HexToLong(noteId),getUserIdByToken(token));
            NoteContent noteContent=NoteContentService.GetNoteContent(MyConvert.HexToLong(noteId), getUserIdByToken(token));
            if (noteContent!=null&&!string.IsNullOrEmpty(noteContent.Content))
            {

                noteContent.Content=NoteService.FixContent(noteContent.Content,note.IsMarkdown);
            }
            ApiNoteContent apiNote=new ApiNoteContent()
            {
                NoteId = note.NoteId,
                UserId=note.UserId,
                Content=noteContent.Content
            };
            ApiRe falseRe=new ApiRe()
            {
                Ok=false,
                Msg= "GetNoteContent_is_error"
            };
            return Json(apiNote,MyJsonConvert.GetOptions());

        }
        //todo:添加笔记
        public JsonResult AddNote(ApiNote noteOrContent,string token)
        {
            long userId= getUserIdByToken(token);
            ApiRe apiRe = new ApiRe();
            long myUserId=userId;
            if (noteOrContent == null || string.IsNullOrEmpty(noteOrContent.NotebookId))
            {
                return Json(new ApiRe() { Ok = false, Msg = "notebookIdNotExists" }, MyJsonConvert.GetSimpleOptions());
            }
            long noteId=MyConvert.HexToLong(noteOrContent.NoteId);
            if (noteId==0)
            {
                noteId=SnowFlake_Net.GenerateSnowFlakeID();

            }
          

            // TODO 先上传图片/附件, 如果不成功, 则返回false
            //
            int attachNum=0;
            if (noteOrContent.Files != null && noteOrContent.Files.Length > 0)
            {

            }
            fixPostNotecontent(ref noteOrContent);
            Note note=new Note()
            {
                UserId=userId,
                NoteId=noteId,
                NotebookId=MyConvert.HexToLong(noteOrContent.NotebookId),
                Title=noteOrContent.Title,
                Tags=noteOrContent.Tags,
                Desc=noteOrContent.Desc,

                IsBlog=noteOrContent.IsBlog,
                IsMarkdown= noteOrContent.IsMarkdown,
		        AttachNum= attachNum,
		        CreatedTime=noteOrContent.CreatedTime,
		        UpdatedTime=noteOrContent.UpdatedTime,
            };
            NoteContent noteContent=new NoteContent()
            {
                NoteContentId=SnowFlake_Net.GenerateSnowFlakeID(),
                NoteId=noteId,
                UserId=    userId,
		        IsBlog= note.IsBlog,
		        Content= noteOrContent.Content,
		        Abstract= noteOrContent.Abstract,
		        CreatedTime= noteOrContent.CreatedTime,
		        UpdatedTime=noteOrContent.UpdatedTime,
            };
            // 通过内容得到Desc, abstract
            if (string.IsNullOrEmpty(noteOrContent.Abstract))
            {
                note.Desc= MyHtmlHelper.SubStringHTMLToRaw(noteContent.Content,200);
                noteContent.Abstract=MyHtmlHelper.SubStringHTML(noteContent.Content,200,"");
            }
            else
            {
                note.Desc =MyHtmlHelper.SubStringHTMLToRaw(noteContent.Abstract, 200);
            }
            note=NoteService.AddNoteAndContent(note,noteContent,myUserId);
            if (note==null||note.NoteId==0)
            {
                return Json(new ApiRe()
                {
                    Ok=false,
                    Msg= "AddNoteAndContent_is_error"
                });
            }
            //添加需要返回的
            noteOrContent.NoteId=note.NoteId.ToString("x");
            noteOrContent.Usn = note.Usn;
            noteOrContent.CreatedTime = note.CreatedTime;
            noteOrContent.UpdatedTime = note.UpdatedTime;
            noteOrContent.UserId = getUserIdByToken(token).ToString("x");
            noteOrContent.IsMarkdown = note.IsMarkdown;
            // 删除一些不要返回的, 删除Desc?
            noteOrContent.Content = "";
            noteOrContent.Abstract = "";
            //	apiNote := info.NoteToApiNote(note, noteOrContent.Files)

            return Json(noteOrContent,MyJsonConvert.GetOptions());
        }
        //todo:更新笔记
        public IActionResult UpdateNote()
        {

            return null;
        }
        //todo:删除trash
        public IActionResult DeleteTrash()
        {
            return null;
        }
        //todo:导出成PDF
        public IActionResult ExportPdf()
        {
            return null;
        }

        // content里的image, attach链接是
        // https://leanote.com/api/file/getImage?fileId=xx
        // https://leanote.com/api/file/getAttach?fileId=xx
        // 将fileId=映射成ServerFileId, 这里的fileId可能是本地的FileId
        public void fixPostNotecontent(ref ApiNote noteOrContent)
        {
            if (noteOrContent == null || string.IsNullOrEmpty(noteOrContent.Content))
            {
                return;
            }
            APINoteFile[] files = noteOrContent.Files;
            if (files != null && files.Length > 0)
            {

            }
        }
    }
}