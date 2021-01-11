using MoreNote.Logic.Entity;
using System;

namespace MoreNote.Logic.Service
{
    public class TrashService
    {
       
        DependencyInjectionService dependencyInjectionService;
        public TrashService(DependencyInjectionService dependencyInjectionService)
        {
          this.dependencyInjectionService = dependencyInjectionService;
        }
        // 回收站
        // 可以移到noteSerice中

        // 不能删除notebook, 如果其下有notes!
        // 这样回收站里只有note

        // 删除笔记后(或删除笔记本)后入回收站
        // 把note, notebook设个标记即可!
        // 已经在trash里的notebook, note不能是共享!, 所以要删除共享


        // 删除note
        // 应该放在回收站里
        // 有trashService
        public  bool DeleteNote(long noteId, long userId)
        {
            throw new Exception();
        }
        // 删除别人共享给我的笔记
        // 先判断我是否有权限, 笔记是否是我创建的
        public  bool DeleteSharedNote(long noteId, long myUserId)
        {
            throw new Exception();
        }
        // recover
        public  bool recoverNote(long noteId, long notebookId, long userId)
        {
            throw new Exception();
        }
        // 删除trash
        public  bool DeleteTrash(long noteId, long userId)
        {
            throw new Exception();
        }
        //todo 删除废纸篓
        public  bool DeleteTrashApi(long noteId, long userId, int usn, out string msg, out int afterUsn)
        {
            NoteService noteService=dependencyInjectionService.GetNoteService();
            AttachService attachService=dependencyInjectionService.GetAttachService();
            NoteContentService noteContentService=dependencyInjectionService.GetNoteContentService();
            NotebookService notebookService=dependencyInjectionService.GetNotebookService();
            Note note = noteService.GetNote(noteId, userId);
            if (note==null)
            {
                msg = "notExists";
                afterUsn = 0;
                //todo: 存疑
                return false;
            }
        
            if (note.Usn != usn)
            {
                msg = "conflict";
                afterUsn = 0;
                return false;
            }
            // 设置删除位
            //afterUsn = UserService.IncrUsn(userId);
            // delete note's attachs
           var result= noteService.SetDeleteStatus(noteId,userId,out afterUsn);
            if (!result)
            {
                msg= "设置删除位错误";
                afterUsn=0;
                return false;
            }
            attachService.DeleteAllAttachs(noteId, userId);

            // 删除content history           
            noteContentService.DeleteByIdAndUserId(noteId, userId, true);
            notebookService.ReCountNotebookNumberNotes(note.NotebookId);
            msg = "";
            return true;
        }
        // 列出note, 排序规则, 还有分页
        // CreatedTime, UpdatedTime, title 来排序
        public  Note[] ListNotes(long userId, int pageNumber, int pageSize, string sortField, bool isAsc)
        {
            throw new Exception();
        }


    }
}
