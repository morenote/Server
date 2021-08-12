using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Common.Utils;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MoreNote.Logic.Service.Tests
{
    [TestClass()]
    public class NoteServiceTests
    {
        [TestMethod()]
        public void GetNoteContentTest()
        {
            //using (var db = DataContext.getDataContext())
            //{
            //    var list = dataContext.NoteContent.ToList();
            //    foreach (var item in list)
            //    {
            //        if (item.Content==null)
            //        {
            //            continue;

            //        }
            //        Console.WriteLine(item.NoteId);
            //        //11ae1ade40021000aaaaaaaa
            //        //处理图片链接
            //        Regex regexImage = new Regex(@"(https*://[^/]*?/api/file/getImage\?fileId=)([a-zA-Z0-9]{16})aaaaaaaa");
            //        if (regexImage.IsMatch(item.Content))
            //        {
            //            item.Content = regexImage.Replace(item.Content, "${1}" + "00000000"+"${2}");
            //            Console.WriteLine(item.Content);
            //        }

            //        //处理附件链接
            //        Regex regexFuJian = new Regex(@"(https*://[^/]*?/api/file/getAttach\?fileId=)([a-zA-Z0-9]{16})aaaaaaaa");
            //        if (regexFuJian.IsMatch(item.Content))
            //        {
            //            item.Content = regexFuJian.Replace(item.Content,  "${1}" + "00000000"+"${2}");
            //        }

            //    }
            //    dataContext.SaveChanges();

            //}

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
            //string ContentJson = System.IO.File.ReadAllText(@"E:\Project\JSON\note\getNoteContent.json");
            //NoteContent noteConteny = JsonSerializer.Deserialize<NoteContent>(ContentJson, MyJsonConvert.GetOptions());
            //noteConteny.NoteId = 201901;
            //noteConteny.UpdatedUserId = SnowFlake_Net.GenerateSnowFlakeID();
            //noteConteny.UserId = SnowFlake_Net.GenerateSnowFlakeID();
            //NoteContentService.InsertNoteContent(noteConteny);

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

        [TestMethod()]
        public void FixMarkDownxContentTest()
        {
           string context="![title](/api/file/getImage?fileId=13c8329167421000)";
           NoteService noteService=new NoteService(null);
           var result=  noteService.FixContent(context,true,@"https://localhost:44392");
           Console.WriteLine("");
           Console.WriteLine(result);
        }
        [TestMethod()]
        public void FixContentTest2()
        {
           string context="<img src=\"http://leanote.com/file/outputImage?fileId=5503537b38f4111dcb0000d1\">";
         
           NoteService noteService=new NoteService(null);
           var result=  noteService.FixContent(context,false,@"https://localhost:44392");
           Console.WriteLine("");
           Console.WriteLine(result);
        }
            [TestMethod()]
        public void FixContentTest3()
        {
           
           string context="<p><img src=\"/api/file/getImage?fileId=611502cf05dadb007a000002\"></p>";
           NoteService noteService=new NoteService(null);
           var result=  noteService.FixContent(context,false,@"https://localhost:44392");
           Console.WriteLine("");
           Console.WriteLine(result);
        }
    }

}