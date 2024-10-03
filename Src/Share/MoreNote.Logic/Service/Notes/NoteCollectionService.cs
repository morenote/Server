using MoreNote.Common.ExtensionMethods;
using MoreNote.Logic.Database;
using MoreNote.Logic.Service.DistributedIDGenerator;
using MoreNote.Models.Entity.Leanote.Notes;

using System;
using System.Collections.Generic;
using System.Linq;

using Z.EntityFramework.Plus;

namespace MoreNote.Logic.Service.Notes
{
    public class NoteCollectionService
    {
        private DataContext dataContext;
        public UserService UserService { get; set; }

        public NoteService NoteService { get; set; }
        private IDistributedIdGenerator IdGenerator;
        public NoteCollectionService(DataContext dataContext, NoteService noteService, IDistributedIdGenerator IdGenerator)
        {
            this.IdGenerator = IdGenerator;
            this.dataContext = dataContext;
            NoteService = noteService;
        }

        public NoteCollection GetNoteCollectionById(long? notebookId)
        {
            var result = dataContext.NoteCollection.
                Where(b => b.Id == notebookId).FirstOrDefault();
            return result;
        }
        public NoteCollection GetNoteCollectionById(long? notebookId, long? repositoryId)
        {
            var result = dataContext.NoteCollection.
                Where(b => b.Id == notebookId && b.NotesRepositoryId == repositoryId).FirstOrDefault();
            return result;
        }
        /// <summary>
        /// 获得子目录
        /// </summary>
        /// <param name="notebookId"></param>
        /// <returns></returns>
        public List<NoteCollection> GetNotebookChildren(long? notebookId)
        {
            var result = dataContext.NoteCollection.
                Where(b => b.ParentCollectionId == notebookId &&
                           b.IsDeleted == false &&
                           b.IsTrash == false).OrderBy(b => b.Title).ToList();
            return result;
        }

        public bool AddNoteCollection(NoteCollection noteCollection)
        {
            if (noteCollection.Id == 0)
            {
                noteCollection.Id = IdGenerator.NextId();
            }
            noteCollection.UrlTitle = noteCollection.Id.ToHex();

            noteCollection.Usn = UserService.IncrUsn(noteCollection.UserId);

            DateTime now = DateTime.Now;
            noteCollection.CreatedTime = now;
            noteCollection.UpdatedTime = now;

            var result = dataContext.NoteCollection.Add(noteCollection);
            return dataContext.SaveChanges() > 0;
        }

        public bool AddNoteCollection(ref NoteCollection notebook)
        {
            if (notebook.Id == null)
            {
                notebook.Id = IdGenerator.NextId();
            }
            notebook.UrlTitle = notebook.Id.ToHex();

            notebook.Usn = UserService.IncrUsn(notebook.UserId);

            DateTime now = DateTime.Now;
            notebook.CreatedTime = now;
            notebook.UpdatedTime = now;

            var result = dataContext.NoteCollection.Add(notebook);
            return dataContext.SaveChanges() > 0;
        }
        public bool AddNoteCollection(long? notebookId, long? userid, long? parentNotebookId, string title, out NoteCollection one)
        {
            var notebook = dataContext.NoteCollection.Where(b => b.Id == notebookId && b.UserId == userid).FirstOrDefault();
            if (notebook == null)
            {
                one = new NoteCollection()
                {
                    Id = notebookId,
                    Title = title,
                    UserId = userid,
                    ParentCollectionId = parentNotebookId

                };
                return AddNoteCollection(one);

            }
            one = null;
            return false;

        }
        public bool UpdateNoteCollectionApi(long? userId, long? notebookId, string title, long? parentNotebookId, int seq, int usn, out NoteCollection notebook)
        {
            var result = dataContext.NoteCollection.
                 Where(b => b.Id == notebookId);
            if (result == null)
            {
                notebook = null;
                return false;
            }
            notebook = result.FirstOrDefault();
            notebook.Title = title;
            notebook.Usn = UserService.IncrUsn(userId);
            notebook.Seq = seq;
            notebook.UpdatedTime = DateTime.Now;
            notebook.ParentCollectionId = parentNotebookId;
            return dataContext.SaveChanges() > 0;
        }

        public NoteCollection[] GetAll(long? userid)
        {
            var result = dataContext.NoteCollection
                   .Where(b => b.UserId == userid).OrderBy(b => b.Title).ToArray();
            return result;
        }
        public NoteCollection[] GetAllByNotesRepositoryId(long? repositoryId)
        {
            var result = dataContext.NoteCollection
                   .Where(b => b.NotesRepositoryId == repositoryId && b.IsDeleted == false && b.IsTrash == false).OrderBy(b => b.Title).ToArray();
            return result;
        }

        public NoteCollection[] GetNoteBookCollectionByNotesRepositoryId(long? repositoryId)
        {
            NoteCollection[] notebooks = GetAllByNotesRepositoryId(repositoryId);
            NoteCollection[] noteBookTrees = (from NoteCollection n in notebooks
                                              where n.ParentCollectionId == 0
                                              select n).ToArray();
            foreach (NoteCollection notebook in noteBookTrees)
            {
                notebook.Subs = GetNoteCollectionTree(notebook.Id, ref notebooks);
            }
            return noteBookTrees;
        }



        public NoteCollection[] GetNoteBookTree(long? userid)
        {
            NoteCollection[] notebooks = GetAll(userid);
            NoteCollection[] noteBookTrees = (from NoteCollection n in notebooks
                                              where n.ParentCollectionId == 0
                                              select n).ToArray();
            foreach (NoteCollection notebook in noteBookTrees)
            {
                notebook.Subs = GetNoteCollectionTree(notebook.Id, ref notebooks);
            }
            return noteBookTrees;
        }

        private static List<NoteCollection> GetNoteCollectionTree(long? noteid, ref NoteCollection[] notebooks)
        {
            List<NoteCollection> noteBookTrees = (from NoteCollection n in notebooks
                                                  where n.ParentCollectionId == noteid
                                                  select n).ToList();
            foreach (NoteCollection notebook in noteBookTrees)
            {
                notebook.Subs = GetNoteCollectionTree(notebook.Id, ref notebooks);
            }
            return noteBookTrees;
        }

        public NoteCollection[] GeSyncNotebooks(long? userid, int afterUsn, int maxEntry)
        {
            var result = dataContext.NoteCollection.
                    Where(b => b.UserId == userid && b.Usn > afterUsn).OrderBy(b => b.Usn).Take(maxEntry);
            return result.ToArray();
        }

        public SubNotebooks SortSubNotebooks(SubNotebooks eachNotebooks)
        {
            throw new Exception();
        }

        // 整理(成有关系)并排序
        // GetNotebooks()调用
        // ShareService调用
        public List<NoteCollection> ParseAndSortNoteCollection(List<NoteCollection> userNotebooks, bool needSort)
        {
            var result = RecursiveSpanningTrees(userNotebooks, null, needSort);

            return result;
        }

        /// <summary>
        /// 递归生成目录树（排序方式=>title）
        /// </summary>
        /// <param name="userNotebooks"></param>
        /// <param name="result"></param>
        private List<NoteCollection> RecursiveSpanningTrees(List<NoteCollection> userNotebooks, long? notebookId, bool needSort)
        {
            List<NoteCollection> temp = userNotebooks.FindAll((n1) => n1.ParentCollectionId == notebookId);
            if (needSort)
            {
                temp.Sort((n1, n2) => n1.Title.CompareTo(n2.Title));
            }

            if (temp.IsNullOrNothing())
            {
                return null;
            }
            foreach (var item in temp)
            {
                item.Subs = RecursiveSpanningTrees(userNotebooks, item.Id, needSort);
            }

            return temp;
        }

        public NoteCollection GetNoteCollection(long? noteBookId, long? userId)
        {
            throw new Exception();
        }

        public NoteCollection GetNoteCollectionByUserIdAndUrlTitle(long? userId, string notebookIdOrUrl)
        {
            throw new Exception();
        }

        public List<NoteCollection> GetNoteCollections(long? userId)
        {
            List<NoteCollection> userNotebooks = new List<NoteCollection>();
            userNotebooks = dataContext.NoteCollection.Where(b => b.UserId == userId && b.IsDeleted == false).OrderBy(b => b.Title).ToList();
            if (userNotebooks == null)
            {
                return null;
            }
            return ParseAndSortNoteCollection(userNotebooks, true);
        }

        public SubNotebooks GetNoteCollectionsByNoteCollectionIds(long[] notebookIds)
        {
            throw new Exception();
        }

        // 判断是否是blog
        public bool IsBlog(long? notebookId)
        {
            return false;
        }

        // 判断是否是我的notebook
        public bool IsMyNoteCollection(long? notebookId)
        {
            throw new Exception();
        }

        public bool AnyNoteCollection(long? parentNotebookId)
        {
            var bookAny = dataContext.NoteCollection.Where(b => b.ParentCollectionId == parentNotebookId &&
                                                      b.IsDeleted == false &&
                                                      b.IsTrash == false).Any();
            return bookAny;
        }
        public bool AnyNote(long? parentNotebookId)
        {
            var noteAny = dataContext.Note.Where(b => b.NotebookId == parentNotebookId &&
                                                        b.IsDeleted == false &&
                                                        b.IsTrash == false).Any();
            return noteAny;
        }



        // 更新笔记本信息
        // 太广, 不用
        /*
        func (this *NotebookService) UpdateNotebook(notebook info.Notebook) bool {
            return dataContext.UpdateByIdAndUserId2(dataContext.Notebooks, notebook.NotebookId, notebook.UserId, notebook)
        }
        */

        // 更新笔记本标题
        // [ok]
        public bool UpdateNoteCollectionTitle(long? notebookId, long? userId, string title)
        {
            var notebook = dataContext.NoteCollection.Where(b => b.Id == notebookId
                                                       && b.UserId == userId).FirstOrDefault();
            if (notebook == null)
            {
                return false;
            }
            var usn = UserService.IncrUsn(userId);
            notebook.Title = title;
            notebook.Usn = usn;
            dataContext.SaveChanges();
            return true;
        }
        public bool UpdateNoteCollectionTitle(long? notebookId, string title)
        {
            var notebook = dataContext.NoteCollection.Where(b => b.Id == notebookId).FirstOrDefault();
            if (notebook == null)
            {
                return false;
            }

            notebook.Title = title;

            dataContext.SaveChanges();
            return true;
        }

        public List<NoteCollection> GetRootNoteCollections(long? repositoryId)
        {

            var books = dataContext.NoteCollection
                .Where(b => b.NotesRepositoryId == repositoryId
                        && b.ParentCollectionId == NoteCollection.RootParentNotebookId
                        && b.IsDeleted == false
                        && b.IsTrash == false)
                .OrderBy(x => x.Title)
                .ToList();
            return books;

        }

        public bool UpdateNoteCollection(long? userId, long? notebookId, object needUpdate)
        {
            throw new Exception();
        }

        // ToBlog or Not
        public bool ToBlog(long? userId, long? notebookId, bool isBlog)
        {
            //todo:实现这个ToBlog
            var usn = UserService.IncrUsn(userId);
            dataContext.NoteCollection.Where(b => b.Id == notebookId && b.UserId == userId)
                                  .Update(x => new NoteCollection() { IsBlog = isBlog, Usn = usn });
            if (isBlog)
            {
                dataContext.Note.Where(b => b.NotebookId == notebookId && b.UserId == userId)
                .Update(x => new Note() { IsBlog = isBlog, PublicTime = DateTime.Now });
            }
            else
            {
                dataContext.Note.Where(b => b.NotebookId == notebookId && b.UserId == userId)
                            .Update(x => new Note() { IsBlog = isBlog, HasSelfDefined = false, Usn = usn });

            }

            dataContext.SaveChanges();

            return true;
        }

        // 查看是否有子notebook
        // 先查看该notebookId下是否有notes, 没有则删除
        public bool DeleteNoteCollection(long? notebookId, int usn)
        {

            if (AnyNote(notebookId) || AnyNoteCollection(notebookId))
            {

            }
            //删除空笔记本
            var book = dataContext.NoteCollection.Where(notebook => notebook.Id == notebookId).FirstOrDefault();
            if (book == null)
            {

                return false;
            }
            book.IsDeleted = true;
            book.Usn = usn;
            dataContext.SaveChanges();
            return true;
        }









        public List<Note> GetNotebookChildrenNote(long? notebookId)
        {
            return dataContext.Note.Where(b => b.NotebookId == notebookId &&
                                          b.IsDeleted == false &&
                                          b.IsTrash == false).ToList();

        }
        /// <summary>
        /// 递归删除笔记本
        /// </summary>
        /// <param name="notebookId"></param>
        /// <param name="usn"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool DeleteNotebookRecursively(long? notebookId, int usn)
        {


            var list = GetNotebookChildren(notebookId);
            if (list.Any())
            {
                foreach (var item in list)
                {
                    DeleteNotebookRecursively(item.Id, usn);
                }
            }

            var noteList = GetNotebookChildrenNote(notebookId);
            if (noteList.Any())
            {
                foreach (var note in noteList)
                {
                    NoteService.DeleteNote(note.Id, usn);
                }
            }

            //删除空笔记本
            var book = dataContext.NoteCollection.Where(notebook => notebook.Id == notebookId).FirstOrDefault();
            if (book == null)
            {
                return false;
            }
            book.IsDeleted = true;
            book.Usn = usn;
            dataContext.SaveChanges();

            return true;

        }

        // API调用, 删除笔记本, 不作笔记控制
        public bool DeleteNotebookForce(long? userId, long? notebookId, int usn)
        {
            //var result = dataContext.Notebook.Where(note=> note.NotebookId== notebookId && note.UserId==userId&&note.Usn==usn).Delete();
            var result = dataContext.NoteCollection.Where(note => note.Id == notebookId &&
                                                    note.UserId == userId &&
                                                    note.Usn == usn)
                .Update(x => new NoteCollection { IsDeleted = true });
            return result > 0;
        }





        // 排序
        // 传入 notebookId => Seq
        // 为什么要传入userId, 防止修改其它用户的信息 (恶意)
        // [ok]
        public bool SortNotebooks(long? userId, Dictionary<string, int> notebookId2Seqs)
        {
            throw new Exception();
        }

        // 排序和设置父
        public bool DragNotebooks(long? userId, long? curNotebookId, long? parentNotebookId, long?[] siblings)
        {

            var notebook = dataContext.NoteCollection.Where(b => b.Id == curNotebookId && b.UserId == userId).FirstOrDefault();
            if (notebook == null)
            {
                return false;
            }
            notebook.ParentCollectionId = parentNotebookId;
            dataContext.SaveChanges();
            var usn = UserService.IncrUsn(userId);
            //排序
            dataContext.NoteCollection.Where(b => b.UserId == userId && siblings.Contains(b.Id))
                .Update(x => new NoteCollection() { Usn = usn });
            dataContext.SaveChanges();
            return true;

        }

        // 重新统计笔记本下的笔记数目
        // noteSevice: AddNote, CopyNote, CopySharedNote, MoveNote
        // trashService: DeleteNote (recove不用, 都统一在MoveNote里了)
        public bool ReCountNotebookNumberNotes(long? notebookId)
        {
            var count = dataContext.Note.Where(b => b.NotebookId == notebookId && b.IsTrash == false && b.IsDeleted == false).Count();
            var notebook = dataContext.NoteCollection.Where(b => b.Id == notebookId).FirstOrDefault();
            notebook.NumberNotes = count;
            return dataContext.SaveChanges() > 0;
        }

        public void ReCountAll()
        {
            /*
                // 得到所有笔记本
                notebooks := []info.Notebook{}
                dataContext.ListByQWithFields(dataContext.Notebooks, bson.M{}, []string{"NotebookId"}, &notebooks)

                for _, each := range notebooks {
                    this.ReCountNotebookNumberNotes(each.NotebookId.Hex())
                }
            */
            throw new Exception();
        }
    }
}