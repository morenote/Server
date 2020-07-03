using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MoreNote.Logic.Service.Tests
{
    [TestClass()]
    public class NoteServiceTests
    {
        [TestMethod()]
        public void GetNoteContentTest()
        {
    
        }

        [TestMethod()]

        public void AddNoteTest()
        {
            //string noteJson = System.IO.File.ReadAllText(@"E:\Project\JSON\note\getNoteContent.json");
            //Note note = JsonSerializer.Deserialize<Note>(noteJson, MyJsonConvert.GetOptions());
            //note.NoteId = 2019;
            //note.ContentId = 201901;
            //NoteService.AddNote(note);

            // Assert.Fail();
        }

        [TestMethod()]
        public void InsertNoteContentTest()
        {
            string ContentJson = System.IO.File.ReadAllText(@"E:\Project\JSON\note\getNoteContent.json");
            NoteContent noteConteny = JsonSerializer.Deserialize<NoteContent>(ContentJson, MyJsonConvert.GetOptions());
            noteConteny.NoteId = 201901;
            noteConteny.UpdatedUserId = SnowFlake_Net.GenerateSnowFlakeID();
            noteConteny.UserId = SnowFlake_Net.GenerateSnowFlakeID();
            NoteContentService.InsertNoteContent(noteConteny);

        }

        [TestMethod()]
        public void SelectNoteTestByTag()
        {

       
        }

        [TestMethod()]
        public void UpdateNoteTest()
        {
            //Note note=NoteService.GetNoteById(1225057276952449024);
            //note.Desc="dexsss";
            //var result=NoteService.UpdateNote(note);
            //Console.WriteLine(result);

           
        }
    }

}