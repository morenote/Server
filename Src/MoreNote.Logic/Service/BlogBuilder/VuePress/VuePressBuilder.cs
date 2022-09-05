using Microsoft.Extensions.Logging;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.OS.Node;
using MoreNote.Models.Entity.Leanote;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.BlogBuilder.VuePress
{
    public class VuePressBuilder : BlogBuilderInterface
    {
        private WebSiteConfig configFile;
        private string workingDirectory;
        private NotebookService notebookService;
        private NoteContentService noteContentService;

        private ILogger logger;

        public VuePressBuilder(ConfigFileService configFileService,
            NotebookService notebookService,
            NoteContentService noteContentService,
            ILogger<VuePressBuilder> logger)
        {
            this.configFile = configFileService.WebConfig;
            this.workingDirectory = this.configFile.BlogConfig.BlogBuilderWorkingDirectory;
            this.notebookService = notebookService;
            this.noteContentService = noteContentService;
            this.logger = logger;
        }

        public void SetWorkingDirectory(string workingDirectory)
        {
            this.workingDirectory = workingDirectory;
        }

        /// <summary>
        /// 使用默认模块初始化
        /// </summary>
        public async Task InitDefault(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var npm = GetNPM(path);

            await npm.SetRegistry("https://registry.npmmirror.com/");
            await npm.Init();
            await npm.InstallDev("vue");
            await npm.InstallDev("@vuepress/client@next");
            await npm.InstallDev("vuepress@next");
        }

        public async Task Build(string path)
        {
            var npm = GetNPM(path);
            await npm.Run(" vuepress build docs");
        }

        public NodePackageManagement GetNPM(string path)
        {
            var node = new NodeService(this.logger);
            var npm = node.GetNPM(NPMType.PNPM);
            npm.SetWorkingDirectory(path);
            return npm;
        }

        public async Task<bool> WriteNoteBook(Notebook notebook, string path, StringBuilder stringBuilder11, string link)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{{text: '{notebook.Title}'");

            var books = notebookService.GetNotebookChildren(notebook.NotebookId);
            var notes = notebookService.GetNotebookChildrenNote(notebook.NotebookId);
            var bookPath = System.IO.Path.Join(path, notebook.NotebookId.ToHex());
            if (!Directory.Exists(bookPath))
            {
                Directory.CreateDirectory(bookPath);
            }
            if (!books.Any() && !notes.Any())
            {
                return false;
            }

            if (books.Any() || notes.Any())
            {
                sb.Append(",children: [");
            }

            bool app = false;
            foreach (var book in books)
            {
                StringBuilder bookSB = new StringBuilder();
                var result = await WriteNoteBook(book, bookPath, bookSB, link + notebook.Title);
                if (result == true)
                {
                    app = true;
                }
                if (book != books.Last() && bookSB.Length != 0 && result)
                {
                    sb.Append(",\r\n");
                }
            }
            if (books.Any() && notes.Any() && app)
            {
                sb.Append(",");
            }
            if (notes.Any())
            {
                app = true;
            }
            foreach (var note in notes)
            {
                await WriteNote(note, bookPath, sb, link + notebook.NotebookId.ToHex());
                if (note != notes.Last())
                {
                    sb.Append(",");
                }
            }
            if (books.Any() || notes.Any())
            {
                sb.Append("]");
            }

            sb.Append($"}}");
            stringBuilder11.Append(sb);
            return app;
        }

        public async Task WriteNote(Note note, string path, StringBuilder stringBuilder, string link)
        {
            var noteContent = this.noteContentService.GetNoteContent(note.NoteId);
            var mdFilePath = System.IO.Path.Join(path, $"{note.NoteId.ToHex()}.md");
            StringBuilder frontmatterBuilder = new StringBuilder();
            frontmatterBuilder.Append("---\r\n");
            frontmatterBuilder.Append("lang: zh-CN\r\n");
            frontmatterBuilder.Append($"title:{note.Title}\r\n");
            frontmatterBuilder.Append($"description:{note.Desc}\r\n");
            frontmatterBuilder.Append("---\r\n");
            frontmatterBuilder.Append(noteContent.Content);
            var fileStream = File.Open(mdFilePath, FileMode.Create);
            var streamWriter = new StreamWriter(fileStream);
            streamWriter.Write(frontmatterBuilder.ToString());
            streamWriter.Close();
            fileStream.Close();
            stringBuilder.Append($"{{text: '{note.Title}', link: '{link}/{note.NoteId.ToHex()}.md'}}");
        }

        public async Task WriteNotesRepository(NotesRepository notesRepository)
        {
            //工作目录
            if (!Directory.Exists(this.workingDirectory))
            {
                Directory.CreateDirectory(this.workingDirectory);
            }
            var notesRepositoryIdHex = notesRepository.Id.ToHex();
            var notesRepositoryPath = this.workingDirectory + notesRepositoryIdHex;
            if (!Directory.Exists(notesRepositoryPath))
            {
                Directory.CreateDirectory(notesRepositoryPath);
            }
            await InitDefault(notesRepositoryPath);
            var docsPath = System.IO.Path.Join(notesRepositoryPath, "docs");
            if (!Directory.Exists(notesRepositoryPath))
            {
                Directory.CreateDirectory(notesRepositoryPath);
            }
            //递归生成md
            var books = notebookService.GetRootNotebooks(notesRepository.Id);
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (var notebook in books)
            {
                await WriteNoteBook(notebook, docsPath, sb, "/");
                if (notebook != books.Last())
                {
                    sb.Append(",\r\n");
                }
            }
            sb.Append("]");
            var readMePath = System.IO.Path.Join(docsPath, "README.md");
            File.WriteAllText(readMePath, notesRepository.Description);
            //生成配置文件
            var vuePressPath = System.IO.Path.Join(docsPath, ".vuepress");
            if (!Directory.Exists(vuePressPath))
            {
                Directory.CreateDirectory(vuePressPath);
            }
            var configPath = System.IO.Path.Join(vuePressPath, "config.js");
            var configValue = File.ReadAllText(this.configFile.BlogConfig.BlogBuilderVuePressTemplate + "config.ts");
            //替换配置文件
            configValue = configValue.Replace("@sidebar", sb.ToString());
            configValue = configValue.Replace("@title", notesRepository.Name);
            File.WriteAllText(configPath, configValue);
            //生成侧边栏
            await Build(notesRepositoryPath);
        }
    }
}