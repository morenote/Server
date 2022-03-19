using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Database;
using MoreNote.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using Z.EntityFramework.Plus;

namespace MoreNote.Logic.Service
{
    public class NotebookService
    {
        private DataContext dataContext;
        public UserService UserService { get; set; }

        public NotebookService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public Notebook GetNotebookById(long? notebookId)
        {
            var result = dataContext.Notebook.
                Where(b => b.NotebookId == notebookId).FirstOrDefault();
            return result;
        }

        public bool AddNotebook(Notebook notebook)
        {
            if (notebook.NotebookId == 0)
            {
                notebook.NotebookId = SnowFlakeNet.GenerateSnowFlakeID();
            }
            notebook.UrlTitle = notebook.NotebookId.ToHex24();

            notebook.Usn = UserService.IncrUsn(notebook.UserId);

            DateTime now = DateTime.Now;
            notebook.CreatedTime = now;
            notebook.UpdatedTime = now;

            var result = dataContext.Notebook.Add(notebook);
            return dataContext.SaveChanges() > 0;
        }

        public bool AddNotebook(ref Notebook notebook)
        {
            if (notebook.NotebookId ==null)
            {
                notebook.NotebookId = SnowFlakeNet.GenerateSnowFlakeID();
            }
            notebook.UrlTitle = notebook.NotebookId.ToHex24();

            notebook.Usn = UserService.IncrUsn(notebook.UserId);

            DateTime now = DateTime.Now;
            notebook.CreatedTime = now;
            notebook.UpdatedTime = now;

            var result = dataContext.Notebook.Add(notebook);
            return dataContext.SaveChanges() > 0;
        }
        public bool AddNotebook(long? notebookId,long? userid,long? parentNotebookId,string title,out Notebook one)
        {
            var notebook=dataContext.Notebook.Where(b=>b.NotebookId== notebookId&&b.UserId==userid).FirstOrDefault();
            if (notebook==null)
            {
                one=new Notebook()
                {
                    NotebookId=notebookId,
                    Title=title,
                    UserId=userid,
                    ParentNotebookId=parentNotebookId
                    
                };
                return AddNotebook(one);

            }
            one=null;
            return false;

        }
        public bool UpdateNotebookApi(long? userId, long? notebookId, string title, long? parentNotebookId, int seq, int usn, out Notebook notebook)
        {
            var result = dataContext.Notebook.
                 Where(b => b.NotebookId == notebookId);
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
            notebook.ParentNotebookId = parentNotebookId;
            return dataContext.SaveChanges() > 0;
        }

        public Notebook[] GetAll(long? userid)
        {
            var result = dataContext.Notebook
                   .Where(b => b.UserId == userid).OrderBy(b=>b.Title).ToArray<Notebook>();
            return result;
        }

        public Notebook[] GetNoteBookTree(long? userid)
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

        private static List<Notebook> GetNoteBookTree(long? noteid, ref Notebook[] notebooks)
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

        public Notebook[] GeSyncNotebooks(long? userid, int afterUsn, int maxEntry)
        {
            var result = dataContext.Notebook.
                    Where(b => b.UserId == userid && b.Usn > afterUsn).OrderBy(b=>b.Usn).Take(maxEntry);
            return result.ToArray();
        }

        public SubNotebooks sortSubNotebooks(SubNotebooks eachNotebooks)
        {
            throw new Exception();
        }

        // 整理(成有关系)并排序
        // GetNotebooks()调用
        // ShareService调用
        public List<Notebook> ParseAndSortNotebooks(List<Notebook> userNotebooks, bool needSort)
        {
            var result = RecursiveSpanningTrees(userNotebooks, null, needSort);

            return result;
        }

        /// <summary>
        /// 递归生成目录树（排序方式=>title）
        /// </summary>
        /// <param name="userNotebooks"></param>
        /// <param name="result"></param>
        private List<Notebook> RecursiveSpanningTrees(List<Notebook> userNotebooks, long? notebookId, bool needSort)
        {
            List<Notebook> temp = userNotebooks.FindAll((n1) => n1.ParentNotebookId == notebookId);
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
                item.Subs = RecursiveSpanningTrees(userNotebooks, item.NotebookId, needSort);
            }

            return temp;
        }

        public Notebook GetNotebook(long? noteBookId, long? userId)
        {
            throw new Exception();
        }

        public Notebook GetNotebookByUserIdAndUrlTitle(long? userId, string notebookIdOrUrl)
        {
            throw new Exception();
        }

        public List<Notebook> GetNotebooks(long? userId)
        {
            List<Notebook> userNotebooks = new List<Notebook>();
            userNotebooks = dataContext.Notebook.Where(b =>b.UserId == userId&& b.IsDeleted == false ).OrderBy(b=>b.Title).ToList<Notebook>();
            if (userNotebooks == null)
            {
                return null;
            }
            return ParseAndSortNotebooks(userNotebooks, true);
        }

        public SubNotebooks GetNotebooksByNotebookIds(long[] notebookIds)
        {
            throw new Exception();
        }

        // 判断是否是blog
        public bool IsBlog(long? notebookId)
        {
            return false;
        }

        // 判断是否是我的notebook
        public bool IsMyNotebook(long? notebookId)
        {
            throw new Exception();
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
        public bool UpdateNotebookTitle(long? notebookId, long? userId, string title)
        {
            var notebook=dataContext.Notebook.Where(b=>b.NotebookId== notebookId
                                                       && b.UserId==userId).FirstOrDefault();
            if (notebook==null)
            {
                return false;
            }
            var usn=UserService.IncrUsn(userId);
            notebook.Title=title;
            notebook.Usn=usn;
            dataContext.SaveChanges();
            return true;
        }

        public  List<Notebook> GetRootNotebooks(long? repositoryId)
        {

            var books= dataContext.Notebook
                .Where(b=>b.NotesRepositoryId==repositoryId
                        &&b.ParentNotebookId==Notebook.RootParentNotebookId)
                .ToList<Notebook>();
            return books;
           
        }

        public bool UpdateNotebook(long? userId, long? notebookId, object needUpdate)
        {
            throw new Exception();
        }

        // ToBlog or Not
        public bool ToBlog(long? userId,long? notebookId, bool isBlog)
        {
            //todo:实现这个ToBlog
            var usn=UserService.IncrUsn(userId);
            dataContext.Notebook.Where(b => b.NotebookId == notebookId && b.UserId == userId)
                                  .Update(x => new Notebook() { IsBlog = isBlog, Usn = usn });
            if (isBlog)
            {
                dataContext.Note.Where(b => b.NotebookId == notebookId && b.UserId == userId)
                .Update(x => new Note() { IsBlog = isBlog, PublicTime = DateTime.Now });
            }
            else
            {
                dataContext.Note.Where(b => b.NotebookId == notebookId && b.UserId == userId)
                            .Update(x => new Note() { IsBlog = isBlog, HasSelfDefined = false,Usn=usn });

            }
            
            dataContext.SaveChanges();

            return true;
        }

        // 查看是否有子notebook
        // 先查看该notebookId下是否有notes, 没有则删除
        public bool DeleteNotebook(long? userId, long? notebookId, out string msg)
        {
            msg=string.Empty;
            var childrenNotebooks = dataContext.Notebook.Where(b => b.ParentNotebookId == notebookId
                                                              && b.UserId == userId
                                                              && b.IsDeleted == false);
            if (childrenNotebooks.Any())
            {
                msg = "笔记本下有子笔记本";
                return false;
            }
            else
            {
                var childrenNotes = dataContext.Note.Where(note => note.NotebookId == notebookId
                                                                 && note.UserId == userId
                                                                 && note.IsTrash == false
                                                                 && note.IsDeleted == false);
                if (childrenNotes.Any())
                {
                    msg = "笔记本下有笔记";
                    return false;
                }
                else
                {
                    //删除空笔记本
                    var book = dataContext.Notebook.Where(notebook => notebook.UserId == userId
                                                                     && notebook.NotebookId == notebookId).FirstOrDefault();
                    if (book == null)
                    {
                        msg = "笔记本无效或者已经被删除,请刷新浏览器后重试";
                        return false;
                    }
                    book.IsDeleted=true;
                    UserService.IncrUsn(userId);
                    return true;
                }
            }
        }

        // API调用, 删除笔记本, 不作笔记控制
        public bool DeleteNotebookForce(long? userId, long? notebookId, int usn)
        {
            //var result = dataContext.Notebook.Where(note=> note.NotebookId== notebookId && note.UserId==userId&&note.Usn==usn).Delete();
            var result = dataContext.Notebook.Where(note => note.NotebookId == notebookId && note.UserId == userId && note.Usn == usn).Update(x => new Notebook { IsDeleted = true });
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
            
            var notebook = dataContext.Notebook.Where(b => b.NotebookId == curNotebookId && b.UserId == userId).FirstOrDefault();
            if (notebook == null)
            {
                return false;
            }
            notebook.ParentNotebookId = parentNotebookId;
            dataContext.SaveChanges();
            var usn=UserService.IncrUsn(userId);
            //排序
            dataContext.Notebook.Where(b=>b.UserId==userId&&siblings.Contains(b.NotebookId))
                .Update(x=>new Notebook(){ Usn=usn});
            dataContext.SaveChanges();
            return true;

        }

        // 重新统计笔记本下的笔记数目
        // noteSevice: AddNote, CopyNote, CopySharedNote, MoveNote
        // trashService: DeleteNote (recove不用, 都统一在MoveNote里了)
        public bool ReCountNotebookNumberNotes(long? notebookId)
        {
            var count = dataContext.Note.Where(b => b.NotebookId == notebookId && b.IsTrash == false && b.IsDeleted == false).Count();
            var notebook = dataContext.Notebook.Where(b => b.NotebookId == notebookId).FirstOrDefault();
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