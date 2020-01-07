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
            string a = System.IO.File.ReadAllText(@"E:\github\MoreNote\MoreNote\TextFile.txt");
            NoteContent noteContent = NoteContentService.SelectNoteContent(123123);

            string json = JsonSerializer.Serialize(noteContent, MyJsonConvert.GetOptions());

            NoteContent noteContent2 = JsonSerializer.Deserialize<NoteContent>(json, MyJsonConvert.GetOptions());
            Console.WriteLine(noteContent2.UserId);
            Console.WriteLine(noteContent2.NoteId);
            Console.WriteLine(noteContent2.UpdatedUserId);
            //Console.WriteLine(noteContent2.Content);

            //string json=JsonConvert.SerializeObject(noteContent);
            //Console.WriteLine(json);
            // Assert.Fail();
        }

        [TestMethod()]

        public void AddNoteTest()
        {
            string noteJson = System.IO.File.ReadAllText(@"E:\Project\JSON\note\getNoteContent.json");
            Note note = JsonSerializer.Deserialize<Note>(noteJson, MyJsonConvert.GetOptions());
            note.NoteId = 2019;
            note.ContentId = 201901;
            NoteService.AddNote(note);

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
            Note note=NoteService.SelectNoteByTag("Java");
            Console.WriteLine(note.Title);
            //Assert.Fail();
        }
    }

}