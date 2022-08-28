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
            var yarn = GetNPM(path);
      
            await yarn.SetRegistry("http://registry.npm.taobao.org/");
            await yarn.Init();
            await yarn.InstallDev("vuepress@next");

        }

        public async Task Build(string path)
        {
            var yarn = GetNPM(path);
            await yarn.Run("vuepress build docs");
        }

        public NodePackageManagement GetNPM(string path)
        {
            var node = new NodeService();
            var yarn = node.GetNPM(NPMType.YARN);
            yarn.SetWorkingDirectory(path);
            return yarn;
        }
        public async Task WriteNoteBook(Notebook notebook,string path)
        {
            var bookPath=System.IO.Path.Join(path,notebook.Title);
            if (!Directory.Exists(bookPath))
            {
                Directory.CreateDirectory(bookPath);
            }
            var books= notebookService.GetNotebookChildren(notebook.NotebookId);
            foreach (var book in books)
            {

              await  WriteNoteBook(book,bookPath);
            }
            var notes=notebookService.GetNotebookChildrenNote(notebook.NotebookId);
            foreach (var note in notes)
            {
              await  WriteNote(note,bookPath);

            }



        }
        public async Task WriteNote(Note note,string path)
        {
           var noteContent=this.noteContentService.GetNoteContent(note.NoteId);
           var mdFilePath=System.IO.Path.Join(path,$"{note.Title}.md");
           File.WriteAllText(mdFilePath, noteContent.Content);

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
            foreach (var notebook in books)
            {
              await  WriteNoteBook(notebook, docsPath);
            }


            await Build(notesRepositoryPath);
        }


    }
}