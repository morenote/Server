using System;
using System.Collections.Generic;
using System.Text;
using MoreNote.Logic.Entity;
using MoreNote.Logic.DB;
using System.Linq;
using Z.EntityFramework.Plus;

namespace MoreNote.Logic.Service
{
     public class AlbumService
    {
        const int  IMAGE_TYPE=0;
        //add album
        public static bool AddAlbum(Album album)
        {
           album.CreatedTime=DateTime.Now;
            album.Type=IMAGE_TYPE;
            using (var db = DataContext.getDataContext())
            {
               var result= db.Album.Add(album);
                return  db.SaveChanges()>0;
            }
        }
        //get albums
        public static Album[] GetAlbums(long userId)
        {
           
            using (var db = DataContext.getDataContext())
            {
                var result = db.Album
                    .Where(b => b.UserId.Equals(userId));
                return result.ToArray();
            }
        }
        // delete album
        // presupposition: has no images under this ablum
        public static bool DeleteAlbum(long userId,long albumId)
        {
            using (var db = DataContext.getDataContext())
            {
                if (db.File.Where(b=>b.AlbumId==albumId&&b.UserId==userId).Count()==0)
                {
                  return  db.Album.Where(a=>a.AlbumId==albumId).Delete()>0;
                }
                return false;
            }
        }
    }
}
