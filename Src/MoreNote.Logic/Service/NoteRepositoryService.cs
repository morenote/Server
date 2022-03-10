using MoreNote.Logic.Database;
using MoreNote.Models.Entity.Leanote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service
{
    public class NoteRepositoryService
    {
        DataContext dataContext;
        public NoteRepositoryService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
       public  List<NotesRepository> GetNoteRepositoryList(long? userId)
        {
            var list = dataContext.NotesRepository.Where(b => b.OwnerId == userId).ToList<NotesRepository>();
            return list;
        }
    }
}
