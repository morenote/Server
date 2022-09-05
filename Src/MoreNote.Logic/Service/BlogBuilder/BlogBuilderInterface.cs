﻿using MoreNote.Logic.Entity;
using MoreNote.Models.Entity.Leanote;

using System;
using System.Collections.Generic;
using System.Linq;
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
        public  Task InitDefault(string path);
        public Task Build(string path);

        public  Task<bool> WriteNoteBook(Notebook notebook, string path, StringBuilder stringBuilder, string link);
        public  Task WriteNote(Note note, string path, StringBuilder stringBuilder, string link);
        public  Task WriteNotesRepository(NotesRepository notesRepository);

    }
}