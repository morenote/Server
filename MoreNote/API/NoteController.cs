using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.ModelBinder;
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
    public JsonResult GetSyncNotes(int afterUsn, int maxEntry,string token)
    {
        if (maxEntry == 0) maxEntry = 100;
        ApiNote[] apiNotes = NoteService.GetSyncNotes(getUserIdByToken(token), afterUsn, maxEntry);
        return Json(apiNotes, MyJsonConvert.GetOptions());
    }
    //todo:得到笔记本下的笔记
    public IActionResult GetNotes(string notebookId,string token,string userId)
    {
        Note[] notes=NoteService.ListNotes(1,1,false,1,2, defaultSortField,false,false);
        long myNotebookId=MyConvert.HexToLong(notebookId);
        
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
    public IActionResult GetNoteAndContent()
    {
        return null;

    }
    //todo:格式化URL
    public IActionResult fixPostNotecontent()
    {
        return null;
    }
    //todo:得到内容
    public IActionResult GetNoteContent()
    {
        return null;
    }
    //todo:添加笔记
    public JsonResult AddNote(ApiNote noteOrContent)
    {
        ApiRe apiRe=new ApiRe();
        if (noteOrContent==null||string.IsNullOrEmpty(noteOrContent.NotebookId))
        {
            return Json(new ApiRe() { Ok=false,Msg= "notebookIdNotExists" },MyJsonConvert.GetSimpleOptions());
        }
        // TODO 先上传图片/附件, 如果不成功, 则返回false
        //
        if (noteOrContent.Files!=null&& noteOrContent.Files.Length>0)
        {

        }

        return null;
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
            APINoteFile[] files=noteOrContent.Files;
            if (files!=null&&files.Length>0)
            {


            }
        }
    }
}