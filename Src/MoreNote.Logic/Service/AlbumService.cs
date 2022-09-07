using MoreNote.Logic.Entity;
using MoreNote.Logic.ExtensionMethods.DI;
using System;
using MoreNote.Logic.Database;
using System.Linq;
using Z.EntityFramework.Plus;

namespace MoreNote.Logic.Service
{
    public class AlbumService
    {
        public const int IMAGE_TYPE = 0;

        private DataContext dataContext;
        public AlbumService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        //add album
        public bool AddAlbum(Album album)
        {
            album.CreatedTime = DateTime.Now;
            album.Type = IMAGE_TYPE;

            var result = dataContext.Album.Add(album);
            return dataContext.SaveChanges() > 0;

        }

        //get albums
        public Album[] GetAlbums(long? userId)
        {

            var result = dataContext.Album
             .Where(b => b.UserId.Equals(userId));
            return result.ToArray();

        }

        // delete album
        // presupposition: has no images under this ablum
        public bool DeleteAlbum(long? userId, long? albumId)
        {

            if (dataContext.NoteFile.Where(b => b.AlbumId == albumId && b.UserId == userId).Count() == 0)
            {
                return dataContext.Album.Where(a => a.Id == albumId).Delete() > 0;
            }
            return false;
        }

        public bool UpdateAlbum(long? albumId, long? userId, string name)
        {

            var result = dataContext.Album
            .Where(b => b.Id.Equals(albumId) && b.UserId.Equals(userId));
            if (result != null)
            {
            }
            result.FirstOrDefault().Name = name;
            return dataContext.SaveChanges() > 0;

        }
    }
}