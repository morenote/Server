
using EntityFramework.Extensions;
using NickelProject.Logic.DB;
using NickelProject.Logic.Entity;
using System.Collections.Generic;
using System.Linq;

namespace NickelProject.Logic.Service
{
    public class AuthorManager
    {
        public static void AddAuthor(AuthorEntity author)
        {
            using (var db = new DataContext())
            {
                db.Author.Add(author);
                db.SaveChanges();
            }
          
        }
        public static int DeleteAuthorById(string id)
        {
            using (var db = new DataContext())
            {

                db.Author.Delete(u => u.Id.Equals(id));
                db.SaveChanges();

            }

            return 0;
        }
        public static List<AuthorEntity> SelectAuthorById(string id)
        {
            using (var db = new DataContext())
            {
                List<AuthorEntity> authorEntities = db.Author
                    .Where(b => b.Id.Equals(id))
                    .ToList<AuthorEntity>();
                if (authorEntities != null)
                {
                    return authorEntities;
                }
                else
                {
                    return new List<AuthorEntity>();
                }
            }
        }
        public static List<AuthorEntity> SelectAuthorByName(string name)
        {
            using (var db = new DataContext())
            {
                List<AuthorEntity> authorEntities = db.Author
                    .Where(b => b.Name.Equals(name))
                    .ToList<AuthorEntity>();
                if (authorEntities != null)
                {
                    return authorEntities;
                }
                else
                {
                    return new List<AuthorEntity>();
                }
            }
        }
        public static List<AuthorEntity> SelectAllAuthor()
        {
            using (var db = new DataContext())
            {
                List<AuthorEntity> authorEntities = db.Author
                  
                    .ToList<AuthorEntity>();
                if (authorEntities != null)
                {
                    return authorEntities;
                }
                else
                {
                    return new List<AuthorEntity>();
                }
            }
        }
        public static void UpdataAuthor(AuthorEntity author)
        {
            using (var db = new DataContext())
            {
                AuthorEntity authorEntity = db.Author
                    .Where(b => b.Id.Equals(author.Id)).FirstOrDefault();
                authorEntity = author;
                db.SaveChanges();
            }
            

        }
    
    }
}
