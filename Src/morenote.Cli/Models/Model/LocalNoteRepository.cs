using morenote_sync_cli.ExtensionMethod;
using morenote_sync_cli.Interface;
using morenote_sync_cli.Models.Model.API;
using morenote_sync_cli.Service.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace morenote_sync_cli.Models.Model
{
    /// <summary>
    /// 本地笔记存储库
    /// </summary>
    public class LocalNoteRepository
    {
        public string dir;
        public IPrinter printer;

        public LocalNoteRepository(string dir, IPrinter printer)
        {
            this.dir = dir;
            this.printer = printer;
        }

        public string GetRepositoryStatusFile()
        {
            return $"{dir}/.msync/RepositoryStatus.json".PathConvert();
        }

        public RepositoryStatus LoadRepositoryStatus()
        {
            var path = $"{dir}/.msync/RepositoryStatus.json".PathConvert();
            var json = File.ReadAllText(path);
            var result = RepositoryStatus.InstanceFormJson(json);
            return result;
        }

        public void InitUserInfo(AuthOk authOk, CommandModel commandModel)
        {
            RepositoryStatus repositoryStatus = new RepositoryStatus();
            repositoryStatus.Address = commandModel.GetParameterValue("-remote");
            repositoryStatus.SetAuthOk(authOk);

            string json = repositoryStatus.ToBeautifulJson();
            if (!File.Exists($"{dir}/.msync/RepositoryStatus.json".PathConvert()))
            {
                var stream = File.Create($"{dir}/.msync/RepositoryStatus.json".PathConvert());
                stream.Close();
            }
            File.WriteAllText(GetRepositoryStatusFile(), json);
        }

        /// <summary>
        /// 初始化笔记
        /// </summary>
        /// <param name="books"></param>
        public void InitBooks(ApiNotebook[] books)
        {
            InitBooksDir(books);
        }

        public void InitBooksDir(ApiNotebook[] books)
        {
            foreach (var book in books)
            {
                if (string.IsNullOrEmpty(book.ParentNotebookId) || book.ParentNotebookId.Equals("0"))
                {
                    if (!Directory.Exists($"{dir}/{book.Title}".PathConvert()))
                    {
                        Directory.CreateDirectory($"{dir}/{book.Title}".PathConvert());
                    }

                    var repositoryStatus = LoadRepositoryStatus();
                    var remoteNoteService = new RemoteNoteService(repositoryStatus.Address);
                    var notes = remoteNoteService.GetNotes(book.NotebookId, repositoryStatus.Token);
                    foreach (var note in notes)
                    {
                        var content = remoteNoteService.GetNoteContent(note.NoteId, repositoryStatus.Token);

                        var ext = note.IsMarkdown ? ".md" : "html";
                        var filePath = $"{dir}/{book.Title}/{note.Title}{ext}".PathConvert();
                        printer.WirteLine($"正在写入{filePath}");
                        File.WriteAllText(filePath, content.Content);
                    }
                    string json = book.ToJson();

                    File.WriteAllText($"{dir}/{book.Title}/book.json", json);
                    CreartBookDir(books, book.NotebookId, $"{dir}/{book.Title}");
                }
            }
        }

        private void CreartBookDir(ApiNotebook[] books, string bookId, string bookDir)
        {
            foreach (var book in books)
            {
                if (book.ParentNotebookId.Equals(bookId))
                {
                    if (!Directory.Exists($"{bookDir}/{book.Title}".PathConvert()))
                    {
                        Directory.CreateDirectory($"{bookDir}/{book.Title}".PathConvert());
                    }
                   
                    var repositoryStatus = LoadRepositoryStatus();
                    var remoteNoteService = new RemoteNoteService(repositoryStatus.Address);
                    var notes = remoteNoteService.GetNotes(book.NotebookId, repositoryStatus.Token);
                    foreach (var note in notes)
                    {
                        var content = remoteNoteService.GetNoteContent(note.NoteId, repositoryStatus.Token);

                        var ext = note.IsMarkdown ? ".md" : "html";
                        var filePath = $"{bookDir}/{book.Title}/{note.Title}{ext}".PathConvert();
                        printer.WirteLine($"正在写入{filePath}");
                        File.WriteAllText(filePath, content.Content);
                    }
                    string json = book.ToJson();
                    File.WriteAllText($"{bookDir}/{book.Title}/book.json", json);
                     CreartBookDir(books, book.NotebookId, $"{bookDir}/{book.Title}");
                }
            }
        }
    }
}