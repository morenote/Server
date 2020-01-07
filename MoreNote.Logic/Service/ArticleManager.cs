using EntityFramework.Extensions;
using MySql.Data.MySqlClient;
using NickelProject.Logic.DB;
using NickelProject.Logic.Entity;
using System.Collections.Generic;
using System.Linq;

namespace NickelProject.Logic.Service
{
    public class ArticleManager
    {
        public static void InsertArticle(ArticleEntity art)
        {
            using (var db = new DataContext())
            {
                db.Article.Add(art);
                db.SaveChanges();
            }
        }
        public static void UpdateArticleById(ArticleEntity article)
        {

            using (var db = new DataContext())
            {
                ArticleEntity articleEntity = db.Article
                    .Where(b => b.ArticleId.Equals(article.ArticleId)).FirstOrDefault();
                if (articleEntity != null)
                {
                    articleEntity = article;
                }
               
                db.SaveChanges();
            }
            

        }
        public static List<ArticleEntity> FindArticleByTitle(string title)
        {

            using (var db = new DataContext())
            {
                List<ArticleEntity> ar = db.Article
                    .Where(b => b.Title.Equals(title)).ToList<ArticleEntity>();
                if (ar != null)
                {
                    return ar;
                }
                else
                {
                    return new List<ArticleEntity>();

                }
            }
          
         

        }
        public static List<ArticleEntity> FindArticleByArticleId(string articleId)
        {

            using (var db = new DataContext())
            {
                List<ArticleEntity> articleEntities = db.Article
                    .Where(b => b.ArticleId.Equals(articleId))
                    .ToList<ArticleEntity>();
                if (articleEntities != null)
                {
                    return articleEntities;
                }
                else
                {
                    return new List<ArticleEntity>();
                }
            }
                
        }
        public static List<ArticleEntity> SelectAllArticle()
        {
            using (var db = new DataContext())
            {
                List<ArticleEntity> articleEntities = db.Article
                    .ToList<ArticleEntity>();
                if (articleEntities != null)
                {
                    return articleEntities;
                }
                else
                {
                    return new List<ArticleEntity>();
                }
            }
           
        }
        public static List<ArticleEntity> SelectAllArticle(int minLimit, int maxLimit)
        {
            using (var db = new DataContext())
            {
                List<ArticleEntity> articleEntities = db.Article.Skip(minLimit).Take(maxLimit)
                    .ToList<ArticleEntity>();
                if (articleEntities != null)
                {
                    return articleEntities;
                }
                else
                {
                    return new List<ArticleEntity>();
                }
            }
        

        }

        
        public static void DeleteChapterByArticleId(string articleId)
        {
            
            using (var db = new DataContext())
            {

                //db.Chapter.Delete(u => u.ArticleId.Equals(articleId));
                db.Chapter.Where(u => u.ArticleId.Equals(articleId)).Delete();
                db.SaveChanges();

            }
        
        }
       
    }
}
