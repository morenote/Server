using MoreNote.Logic.Entity;
using MoreNote.Logic.ExtensionMethods.DI;
using System;
using System.Linq;
using Z.EntityFramework.Plus;

namespace MoreNote.Logic.Service
{
    public class AlbumService
    {
        public const int IMAGE_TYPE = 0;
        private DependencyInjectionService dependencyInjection;

        public AlbumService(DependencyInjectionService dependencyInjectionService)
        {
            this.dependencyInjection = dependencyInjectionService;
        }

        //add album
        public bool AddAlbum(Album album)
        {
            album.CreatedTime = DateTime.Now;
            album.Type = IMAGE_TYPE;
            using (var sc = dependencyInjection.GetServiceScope())
            {
                using (var dataContext = sc.GetDataContext())
                {
                    var result = dataContext.Album.Add(album);
                    return dataContext.SaveChanges() > 0;
                }
            }
        }

        //get albums
        public Album[] GetAlbums(long userId)
        {
            using (var dataContext = dependencyInjection.GetDataContext())
            {
                var result = dataContext.Album
                 .Where(b => b.UserId.Equals(userId));
                return result.ToArray();
            }
        }

        // delete album
        // presupposition: has no images under this ablum
        public bool DeleteAlbum(long userId, long albumId)
        {
            using (var dataContext = dependencyInjection.GetDataContext())
            {
                if (dataContext.File.Where(b => b.AlbumId == albumId && b.UserId == userId).Count() == 0)
                {
                    return dataContext.Album.Where(a => a.AlbumId == albumId).Delete() > 0;
                }
                return false;
            }
        }

        public bool UpdateAlbum(long albumId, long userId, string name)
        {
            using (var dataContext = dependencyInjection.GetDataContext())
            {
                var result = dataContext.Album
                  .Where(b => b.AlbumId.Equals(albumId) && b.UserId.Equals(userId));
                if (result != null)
                {
                }
                result.FirstOrDefault().Name = name;
                return dataContext.SaveChanges() > 0;
            }
        }
    }
}