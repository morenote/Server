using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreNote.Common.Utils;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using Z.EntityFramework.Plus;

namespace MoreNote.Logic.Service
{
    public class NotebookService
    {
        public static Notebook GetNotebookById(long notebookId)
        {
            using (var db = DataContext.getDataContext())
            {
                var result = db.Notebook.
                    Where(b => b.NotebookId == notebookId).FirstOrDefault();
                return result;
            }

        }
        public static bool AddNotebook(Notebook notebook)
        {
            if (notebook.NotebookId==0)
            {
                notebook.NotebookId = SnowFlake_Net.GenerateSnowFlakeID();
            }
            notebook.UrlTitle = notebook.NotebookId.ToString("X");

            notebook.Usn = UserService.IncrUsn(notebook.UserId);

            DateTime now = DateTime.Now;
            notebook.CreatedTime = now;
            notebook.UpdatedTime = now;

            using (var db = DataContext.getDataContext())
            {
                var result = db.Notebook.Add(notebook);
                return db.SaveChanges() > 0;
            }
        }
        public static bool AddNotebook(ref Notebook notebook)
        {
            if (notebook.NotebookId == 0)
            {
                notebook.NotebookId = SnowFlake_Net.GenerateSnowFlakeID();
            }
            notebook.UrlTitle = notebook.NotebookId.ToString("X");

            notebook.Usn = UserService.IncrUsn(notebook.UserId);

            DateTime now = DateTime.Now;
            notebook.CreatedTime = now;
            notebook.UpdatedTime = now;

            using (var db = DataContext.getDataContext())
            {
                var result = db.Notebook.Add(notebook);
                return db.SaveChanges() > 0;
            }
        }
        public static bool UpdateNotebookApi(long userId,long notebookId,string title,long parentNotebookId,int seq,int usn,out Notebook notebook)
        {
            using (var db = DataContext.getDataContext())
            {
                var result = db.Notebook.
                    Where(b=>b.NotebookId==notebookId);
                if (result==null)
                {
                    notebook = null;
                    return false;
                }
                notebook = result.FirstOrDefault();
                notebook.Title = title;
                notebook.Usn = UserService.IncrUsn(userId);
                notebook.Seq = seq;
                notebook.UpdatedTime = DateTime.Now;
                notebook.ParentNotebookId = parentNotebookId;
                return db.SaveChanges() > 0;
            }
        }


        public static Notebook[] GetAll(long userid)
        {
            using (var db = DataContext.getDataContext())
            {
                var result = db.Notebook
                    .Where(b => b.UserId == userid).ToArray<Notebook>();
                return result;
            }
        }
        public static Notebook[] GetNoteBookTree(long userid)
        {
            Notebook[] notebooks = GetAll(userid);
            Notebook[] noteBookTrees = (from Notebook n in notebooks
                                            where n.ParentNotebookId == 0
                                            select n).ToArray<Notebook>();
            foreach (Notebook notebook in noteBookTrees)
            {
                notebook.Subs = GetNoteBookTree(notebook.NotebookId, ref notebooks);
            }
            return noteBookTrees;
        }
        private static List<Notebook> GetNoteBookTree(long noteid, ref Notebook[] notebooks)
        {
            List<Notebook> noteBookTrees = (from Notebook n in notebooks
                                            where n.ParentNotebookId == noteid
                                            select n).ToList<Notebook>();
            foreach (Notebook notebook in noteBookTrees)
            {
                notebook.Subs = GetNoteBookTree(notebook.NotebookId, ref notebooks);
            }
            return noteBookTrees;
        }
        public static Notebook[] GeSyncNotebooks(long userid,int afterUsn,int maxEntry)
        {
            using (var db = DataContext.getDataContext())
            {
                var result = db.Notebook.
                    Where(b=>b.UserId==userid&&b.Usn>afterUsn).Take(maxEntry);
                return result.ToArray();
            }
        }
       
        public static SubNotebooks sortSubNotebooks(SubNotebooks eachNotebooks )
        {
            throw new Exception();
        }
        // 整理(成有关系)并排序
        // GetNotebooks()调用
        // ShareService调用
        public SubNotebooks ParseAndSortNotebooks(Notebook[] userNotebooks,bool noParentDelete,bool needSort)
        {
            throw new Exception();
        }
        public static Notebook GetNotebook(long noteBookId,long userId)
        {
            throw new Exception();
        }
        
        public static Notebook GetNotebookByUserIdAndUrlTitle(long userId,string notebookIdOrUrl)
        {
            throw new Exception();
        }
        public static SubNotebooks GetNotebooks(long userId)
        {
            throw new Exception();
        }
        public static SubNotebooks GetNotebooksByNotebookIds(long[] notebookIds)
        {
            throw new Exception();
        }
        // 判断是否是blog
        public static bool IsBlog(long? notebookId)
        {
            return false;
        }
        // 判断是否是我的notebook
        public static bool IsMyNotebook(long notebookId)
        {
            throw new Exception();
        }
        // 更新笔记本信息
        // 太广, 不用
        /*
        func (this *NotebookService) UpdateNotebook(notebook info.Notebook) bool {
            return db.UpdateByIdAndUserId2(db.Notebooks, notebook.NotebookId, notebook.UserId, notebook)
        }
        */
        // 更新笔记本标题
        // [ok]
        public static bool UpdateNotebookTitle(long notebookId,long userId,string title)
        {
            //在未优化的前提下，全部修改和局部修改的性能是一样的
            //可以直接执行原生SQL提高性能
            throw new Exception();
        }
        public static bool UpdateNotebook(long userId,long notebookId,object needUpdate)
        {
            throw new Exception();
        }
        // ToBlog or Not
        public static bool ToBlog (long userId,bool isBlog)
        {
            throw new Exception();
        }
        // 查看是否有子notebook
        // 先查看该notebookId下是否有notes, 没有则删除
        public static bool  DeleteNotebook(long userId,long notebookId)
        {
            throw new Exception();
        }

        // API调用, 删除笔记本, 不作笔记控制
        public static bool DeleteNotebookForce(long userId, long notebookId, int usn)
        {
            using (var db = DataContext.getDataContext())
            {
                //var result = db.Notebook.Where(note=> note.NotebookId== notebookId && note.UserId==userId&&note.Usn==usn).Delete();
                var result = db.Notebook.Where(note => note.NotebookId == notebookId && note.UserId == userId && note.Usn == usn).Update(x => new Notebook { IsDeleted = true });
                return result > 0;
            }
        }

        // 排序
        // 传入 notebookId => Seq
        // 为什么要传入userId, 防止修改其它用户的信息 (恶意)
        // [ok]
        public static bool SortNotebooks(long userId,Dictionary<string,int> notebookId2Seqs)
        {
            throw new Exception();
        }
        // 排序和设置父
        public static bool DragNotebooks(long userId,long curNotebookId,long parentNotebookId,string[] siblings)
        {
            throw new Exception();
        }
        // 重新统计笔记本下的笔记数目
        // noteSevice: AddNote, CopyNote, CopySharedNote, MoveNote
        // trashService: DeleteNote (recove不用, 都统一在MoveNote里了)
        public static bool ReCountNotebookNumberNotes(long? notebookId)
        {
            using (var db = DataContext.getDataContext())
            {
                var count = db.Note.Where(b=>b.NotebookId==notebookId&&b.IsTrash==false&&b.IsDeleted==false).Count();
                var notebook=db.Notebook.Where(b=>b.NotebookId==notebookId).FirstOrDefault();
                notebook.NumberNotes=count;
                return db .SaveChanges()>0;

            }

                throw new Exception();
        }
        public static void ReCountAll()
        {
            /*
                // 得到所有笔记本
                notebooks := []info.Notebook{}
                db.ListByQWithFields(db.Notebooks, bson.M{}, []string{"NotebookId"}, &notebooks)

                for _, each := range notebooks {
                    this.ReCountNotebookNumberNotes(each.NotebookId.Hex())
                }
            */
            throw new Exception();
        }

    }
}
