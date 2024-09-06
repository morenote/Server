using Microsoft.Extensions.Logging;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.HySystem;
using MoreNote.Config.ConfigFile;
using MoreNote.Logic.OS.Node;
using MoreNote.Models.Entity.Leanote;
using MoreNote.Models.Entity.Leanote.Notes;

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
		private NoteCollectionService notebookService;
		private NoteContentService noteContentService;

		private ILogger logger;

		public VuePressBuilder(ConfigFileService configFileService,
			NoteCollectionService notebookService,
			NoteContentService noteContentService,
			ILogger<VuePressBuilder> logger)
		{
			this.configFile = configFileService.ReadConfig();
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

		public async Task<StringBuilder> WriteNoteBook(NoteCollection notebook, string path, string link, string sapce)
		{

			sapce = sapce + "    ";//空格
			StringBuilder sb = new StringBuilder();
			sb.Append($"{{text: '{notebook.Title}'");

			var books = notebookService.GetNotebookChildren(notebook.Id);
			var notes = notebookService.GetNotebookChildrenNote(notebook.Id);
			var bookPath = System.IO.Path.Join(path, notebook.Id.ToHex());
			if (!Directory.Exists(bookPath))
			{
				Directory.CreateDirectory(bookPath);
			}

			if (!books.Any() && !notes.Any())
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (books.Any() || notes.Any())
			{
				stringBuilder.Append(",children: [ \r\n");
			}
			bool flag = false;

			foreach (var book in books)
			{

				var result = await WriteNoteBook(book, bookPath, link + notebook.Id.ToHex() + "/", sapce);
				if (result != null)
				{
					flag = true;
					stringBuilder.Append(sapce);
					stringBuilder.Append(result);
				}
				if (book != books.Last() && result != null)
				{
					stringBuilder.Append(",\r\n");
				}
			}
			if (books.Any() && notes.Any())
			{
				stringBuilder.Append(",");
			}

			foreach (var note in notes)
			{

				var noteSB = await WriteNote(note, bookPath, link + notebook.Id.ToHex());
				if (noteSB != null)
				{
					flag = true;

					stringBuilder.Append(sapce + "    ");
					stringBuilder.Append(noteSB);
					if (note != notes.Last())
					{
						stringBuilder.Append(",\r\n");
					}
				}

			}
			if (books.Any() || notes.Any())
			{
				stringBuilder.Append("\r\n");
				stringBuilder.Append(sapce);
				stringBuilder.Append("]");

			}



			if (flag == false)
			{
				return null;

			}
			sb.Append(stringBuilder);
			sb.Append("}");
			return sb;
		}

		public async Task<StringBuilder> WriteNote(Note note, string path, string link)
		{
			StringBuilder stringBuilder = new StringBuilder();
			var noteContent = this.noteContentService.GetNoteContentByNoteId(note.Id);
			var mdFilePath = System.IO.Path.Join(path, $"{note.Id.ToHex()}.md");
			StringBuilder frontmatterBuilder = new StringBuilder();
			frontmatterBuilder.Append("---\r\n");
			frontmatterBuilder.Append("lang: zh-CN\r\n");
			frontmatterBuilder.Append($"title: {MyStringUtil.FilterSpecial(note.Title)}\r\n");
			frontmatterBuilder.Append($"description: {MyStringUtil.FilterSpecial(note.Title)}\r\n");
			frontmatterBuilder.Append("---\r\n");
			frontmatterBuilder.Append(noteContent.Content);
			var fileStream = File.Open(mdFilePath, FileMode.Create);
			var streamWriter = new StreamWriter(fileStream);
			await streamWriter.WriteAsync(frontmatterBuilder.ToString());
			streamWriter.Close();
			fileStream.Close();
			stringBuilder.Append($"{{text: '{MyStringUtil.FilterSpecial(note.Title)}', link: '{link}/{note.Id.ToHex()}.md'}}");

			return stringBuilder;
		}

		public async Task WriteNotesRepository(Repository notesRepository)
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
				var temp = await WriteNoteBook(notebook, docsPath, "/", "");
				if (temp != null)
				{
					sb.Append(temp);
					if (notebook != books.Last())
					{
						sb.Append(",\r\n");
					}
				}

			}
			sb.Append("]");
			var readMePath = Path.Join(docsPath, "README.md");
			File.WriteAllText(readMePath, notesRepository.Description);
			//生成配置文件
			var vuePressPath = Path.Join(docsPath, ".vuepress");
			if (!Directory.Exists(vuePressPath))
			{
				Directory.CreateDirectory(vuePressPath);
			}
			var configPath = Path.Join(vuePressPath, "config.js");
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