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
            using (var db = new DataContext())
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

            using (var db = new DataContext())
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

            using (var db = new DataContext())
            {
                var result = db.Notebook.Add(notebook);
                return db.SaveChanges() > 0;
            }
        }
        public static bool UpdateNotebookApi(long userId,long notebookId,string title,long parentNotebookId,int seq,int usn,out Notebook notebook)
        {
            using (var db = new DataContext())
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
            using (var db = new DataContext())
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
            using (var db = new DataContext())
            {
                var result = db.Notebook.
                    Where(b=>b.UserId==userid&&b.Usn>afterUsn).Take(maxEntry);
                return result.ToArray();
            }
        }
        public static bool DeleteNotebookForce(long userId,long notebookId,int usn)
        {
            using (var db = new DataContext())
            {
               //var result = db.Notebook.Where(note=> note.NotebookId== notebookId && note.UserId==userId&&note.Usn==usn).Delete();
               var result = db.Notebook.Where(note=> note.NotebookId== notebookId && note.UserId==userId&&note.Usn==usn).Update(x=>new Notebook { IsDeleted=true});
                return result > 0;
            }
        }
    }
}
