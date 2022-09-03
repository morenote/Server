using Masuit.Tools;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.OS.Node;
using MoreNote.Models.Entity.Leanote;

using SixLabors.ImageSharp.Drawing;

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
        private WebSiteConfig configFile ;
        private string workingDirectory;
        private NotebookService notebookService;
        private NoteContentService noteContentService;
        public VuePressBuilder(ConfigFileService configFileService, NotebookService notebookService,NoteContentService noteContentService)
        {
            this.configFile = configFileService.WebConfig;
            this.workingDirectory = this.configFile.BlogConfig.BlogBuilderWorkingDirectory;
            this.notebookService = notebookService;
            this.noteContentService = noteContentService;
        }

        public void SetWorkingDirectory(string workingDirectory)
        {
            this.workingDirectory= workingDirectory;
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
            await npm.Run("vuepress build docs");
        }

        public NodePackageManagement GetNPM(string path)
        {
            var node = new NodeService();
            var npm = node.GetNPM(NPMType.PNPM);
            npm.SetWorkingDirectory(path);
            return npm;
        }
        public async Task WriteNoteBook(Notebook notebook,string path,StringBuilder stringBuilder,string link)
        {
            stringBuilder.Append($"{{text: '{notebook.Title}',");
           

            var books = notebookService.GetNotebookChildren(notebook.NotebookId);
            var notes = notebookService.GetNotebookChildrenNote(notebook.NotebookId);
            var bookPath=System.IO.Path.Join(path,notebook.Title);
            if (!Directory.Exists(bookPath))
            {
                Directory.CreateDirectory(bookPath);
            }
            if (books.Any() || notes.Any())
            {
                stringBuilder.Append(" children: [");
            }
            
            foreach (var book in books)
            {
              await  WriteNoteBook(book,bookPath, stringBuilder, link + "/" + notebook.Title);
                stringBuilder.Append(",");
            }
           
            foreach (var note in notes)
            {
              await  WriteNote(note,bookPath, stringBuilder,link+"/"+notebook.Title);
              stringBuilder.Append(",");
            }
            if (books.Any() || notes.Any())
            {
                stringBuilder.Append(" children: ]");
                
            }
           
            stringBuilder.Append($"}}");

        }
        public async Task WriteNote(Note note,string path, StringBuilder stringBuilder,string link)
        {
           var noteContent=this.noteContentService.GetNoteContent(note.NoteId);
           var mdFilePath=System.IO.Path.Join(path,$"{note.Title}.md");
           File.WriteAllText(mdFilePath, noteContent.Content);
           stringBuilder.Append($"{{text: '{note.Title}', link: '{link}/{note.Title}.md'}},");
        }
 
       
        public async Task WriteNotesRepository(NotesRepository notesRepository)
        {

            //工作目录
            if (!Directory.Exists(this.workingDirectory))
            {
                Directory.CreateDirectory(this.workingDirectory);
            }
            var notesRepositoryIdHex=notesRepository.Id.ToHex();
            var notesRepositoryPath=this.workingDirectory+ notesRepositoryIdHex;
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
              await  WriteNoteBook(notebook, docsPath, sb,"/");
            }
            sb.Append("[");
            var readMePath=System.IO.Path.Join(docsPath,"README.md");
            File.WriteAllText(readMePath,notesRepository.Description);
            //生成配置文件
            var  vuePressPath=System.IO.Path.Join(docsPath,".vuepress");
            if (!Directory.Exists(vuePressPath))
            {
                Directory.CreateDirectory(vuePressPath);
            }
            var configPath= System.IO.Path.Join(vuePressPath,"config.js");
            var configValue= File.ReadAllText(this.configFile.BlogConfig.BlogBuilderVuePressTemplate+"config.ts");
            configValue= configValue.Replace("@sidebar", sb.ToString());
            File.WriteAllText(configPath,configValue);
            //生成侧边栏
            await Build(notesRepositoryPath);
        }


    }



}