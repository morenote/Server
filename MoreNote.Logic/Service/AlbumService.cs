using System;
using System.Collections.Generic;
using System.Text;
using MoreNote.Logic.Entity;
using MoreNote.Logic.DB;
using System.Linq;

namespace MoreNote.Logic.Service
{
     public class AlbumService
    {
        public static bool AddAlbum(Album album)
        {
            int a = 0;
            using (var db = new DataContext())
            {
               var result= db.Album.Add(album);
                a = db.SaveChanges();
                return  db.SaveChanges()>0;
            }
            
        }
        public static Album[] GetAlbums(long userId)
        {
           
            using (var db = new DataContext())
            {
                var result = db.Album
                    .Where(b => b.UserId.Equals(userId));
                return result.ToArray();
            }
        }
        public static bool DeleteAlbum(long userId)
        {
            using (var db = new DataContext())
            {
                db.Album.Where(a => a.UserId.Equals(userId));
                return db.SaveChanges()>0;
            }
        }
    }
}
