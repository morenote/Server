using Microsoft.VisualStudio.TestTools.UnitTesting;

using MoreNote.Models.Entity.Leanote.Notes;

namespace MoreNote.Logic.Service.Tests
{
	[TestClass()]
	public class NotebookServiceTests
	{
		[TestMethod()]
		public void AddTest()
		{
			//Notebook notebook1 = new Notebook() { NotebookId = SnowFlake_Net.NextId() };

			//Notebook notebook2 = new Notebook() { NotebookId = SnowFlake_Net.NextId() };

			//Notebook notebook3 = new Notebook() { NotebookId = SnowFlake_Net.NextId() };
			//notebook2.ParentNotebookId = notebook1.NotebookId;
			//notebook3.ParentNotebookId = notebook1.NotebookId;

			//NotebookService.AddNotebook(notebook1);
			//NotebookService.AddNotebook(notebook2);
			//NotebookService.AddNotebook(notebook3);

			// Assert.Fail();
		}

		[TestMethod()]
		public void GetNoteBookTreeTest()
		{
			//Notebook[] notebooks = NotebookService.GetNoteBookTree(1208692382644703232);
			//string json = JsonSerializer.Serialize(notebooks, MyJsonConvert.GetOptions());
			//Console.WriteLine(json);
			// Assert.Fail();
		}
		[TestMethod()]
		public void ADDNoteBookTreeTest()
		{
			//string text = System.IO.File.ReadAllText(@"E:\Project\JSON\GetNoteBookTree.json");
			//List<Notebook> notebooks = JsonSerializer.Deserialize<List<Notebook>>(text, MyJsonConvert.GetOptions());
			//foreach(Notebook n in notebooks)
			//{
			//    Console.WriteLine(n.Title);
			//    InsertALL(n);
			//}
			// Assert.Fail();
		}
		private void InsertALL(NoteCollection notebook)
		{
			//NotebookService.AddNotebook(notebook);
			//foreach (Notebook n in notebook.Subs)
			//{
			//   InsertALL(n);
			//}
		}

	}
}