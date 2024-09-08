using MoreNote.Models.Entity.Leanote;
using MoreNote.Models.Entity.Leanote.Notes;
using MoreNote.Models.Entiys.Leanote.Notes;

using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.BlogBuilder
{
	/// <summary>
	/// 博客生成器 接口
	/// </summary>
	public interface BlogBuilderInterface
	{
		public void SetWorkingDirectory(string workingDirectory);
		public Task InitDefault(string path);
		public Task Build(string path);

		public Task<StringBuilder> WriteNoteBook(NoteCollection notebook, string path, string link, string sapce);
		public Task<StringBuilder> WriteNote(Note note, string path, string link);
		public Task WriteNotesRepository(Notebook notebook);

	}
}
