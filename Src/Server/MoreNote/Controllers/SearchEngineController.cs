﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MoreNote.Logic.Database;
using MoreNote.Logic.Service.Segmenter;
using MoreNote.Models.Entity.Leanote.Notes;

using System.Linq;

namespace MoreNote.Controllers
{
	[Authorize(Roles = "Admin,SuperAdmin")]
	public class SearchEngineController : Controller
	{
		private DataContext dataContext;
		JiebaSegmenterService jiebaSegmenterService = new JiebaSegmenterService();
		public SearchEngineController(DataContext dataContext)
		{
			this.dataContext = dataContext;
		}
		[HttpGet, HttpPost]
		public IActionResult RebuildIndex()
		{

			var noteContents = dataContext.NoteContent.ToList();
			foreach (var noteContent in noteContents)
			{
				//noteContent.ContentVector = jiebaSegmenterService.GetNpgsqlTsVector(noteContent.Content);
			}
			var notes = dataContext.Note.ToList();
			foreach (var note in notes)
			{

				//note.TitleVector = jiebaSegmenterService.GetNpgsqlTsVector(note.Title); ;
			}

			dataContext.SaveChanges();
			return Ok();
		}
		//[HttpGet, HttpPost]
		//public IActionResult Index(string s)
		//{
		//	var query = jiebaSegmenterService.GetSerachNpgsqlTsQuery(s);


		//	var noteContextAray = dataContext.NoteContent.Where(b => b.ContentVector.Matches(query)).ToArray();
		//	var noteAray = dataContext.Note.Where(c => c.TitleVector.Matches(query)).ToArray();
		//	MyResult myResult = new MyResult()
		//	{
		//		Contents = noteContextAray,
		//		Notes = noteAray
		//	};

		//	return Ok(myResult);
		//}
		class MyResult
		{
			public NoteContent[] Contents { get; set; }
			public Note[] Notes { get; set; }


		}
	}
}