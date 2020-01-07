using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;

namespace MoreNote.Logic.Service
{
    public class NotebookService
    {
        public static bool Add(Notebook notebook)
        {
            using (var db = new DataContext())
            {
                var result = db.Notebook.Add(notebook);
                return db.SaveChanges() > 0;
            }
        }



        public static List<Notebook> GetAll(long userid)
        {
            using (var db = new DataContext())
            {
                var result = db.Notebook
                    .Where(b => b.UserId == userid).ToList<Notebook>();
                return result;
            }
        }
        public static List<Notebook> GetNoteBookTree(long userid)
        {
            List<Notebook> notebooks = GetAll(userid);
            List<Notebook> noteBookTrees = (from Notebook n in notebooks
                                            where n.ParentNotebookId == 0
                                            select n).ToList<Notebook>();
            foreach (Notebook notebook in noteBookTrees)
            {
                notebook.Subs = GetNoteBookTree(notebook.NotebookId, ref notebooks);
            }
            return noteBookTrees;
        }
        private static List<Notebook> GetNoteBookTree(long noteid, ref List<Notebook> notebooks)
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

    }
}
