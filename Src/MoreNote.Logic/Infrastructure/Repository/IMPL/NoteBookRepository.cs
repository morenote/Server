﻿/**
 *  Thanks for open source!
 *  date：2023-03-08
 *  license:https://github.com/ldqk/Masuit.MyBlogs/blob/master/LICENSE 
 *  git version：b78c483a0dea0d00350bdf44bf788ceb51190e46
 */
using Morenote.Logic.Infrastructure.Repository.Interface;
using MoreNote.Models.Entity.Leanote.Notes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace MoreNote.Logic.Infrastructure.Repository.IMPL
{


    public class NoteBookRepository : BaseRepository<Notebook>, IBaseRepository<Notebook>
    {
        public override Notebook AddEntity(Notebook t)
        {
            DataContext.Add(t);
            return t;
        }


    }
}
