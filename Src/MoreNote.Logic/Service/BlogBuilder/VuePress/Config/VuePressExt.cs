using MoreNote.Common.ExtensionMethods;
using MoreNote.Models.Entity.Leanote.Notes;

using System.IO;
using System.Text;

namespace MoreNote.Logic.Service.BlogBuilder.VuePress.Config
{
	/// <summary>
	/// VuePress扩展方法
	/// </summary>
	public static class VuePressExt
	{

		public static string toJsObjectForSiderBar(this Notebook notebook)
		{
			return null;
		}
		public static string toJsObjectForSiderBar(this Note note, string link)
		{
			return $"{{text: '{note.Title}', link: '{link}/{note.Id.ToHex()}.md'}}";

		}
		public static void WriteVuePressMdFile(this Note note, NoteContent noteContent, string link, string bookPath)
		{
			var mdFilePath = Path.Join(bookPath, $"{note.Id.ToHex()}.md");
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
		}
	}
}
