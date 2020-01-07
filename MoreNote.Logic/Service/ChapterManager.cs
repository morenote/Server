using EntityFramework.Extensions;
using MySql.Data.MySqlClient;
using NickelProject.Logic.DB;
using NickelProject.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NickelProject.Logic.Service
{
    public class ChapterManager
    {

        public static void InsertChapter(ChapterEntity chapter)
        {
            using (var db = new DataContext())
            {
                db.Chapter.Add(chapter);
                db.SaveChanges();
            }
            
        }
        public static List<ChapterEntity> SelectChapterByArticleId(string articleId)
        {
            using (var db = new DataContext())
            {
                List<ChapterEntity> chapterEntities = db.Chapter
                    .Where(b => b.ArticleId.Equals(articleId))
                    .ToList<ChapterEntity>();
                if (chapterEntities != null)
                {
                    return chapterEntities;
                }
                else
                {
                    return new List<ChapterEntity>();

                }
            }
         
        }
        public static List<ChapterEntity> SelectChapterByChapterId(string chapterId)
        {
            using (var db = new DataContext())
            {
                List<ChapterEntity> chapterEntities = db.Chapter
                    .Where(b => b.ChapterId.Equals(chapterId))
                    .ToList<ChapterEntity>();
                if (chapterEntities != null)
                {
                    return chapterEntities;
                }
                else
                {
                    return new List<ChapterEntity>();

                }
            }
           
        }
        public static List<ChapterEntity> SelectAllChapter(int minLimit, int maxLimit)
        {
            using (var db = new DataContext())
            {
                List<ChapterEntity> chapterEntities = db.Chapter
                    .ToList<ChapterEntity>();
                if (chapterEntities != null)
                {
                    return chapterEntities;
                }
                else
                {
                    return new List<ChapterEntity>();

                }
            }
            
        }
        public static int UpdateChapterByChapterId(ChapterEntity chapter)
        {
            int a = 0;
            using (var db = new DataContext())
            {
                ChapterEntity chapterEntity = db.Chapter
                    .Where(b => b.ChapterId.Equals(chapter.ChapterId)).FirstOrDefault();
                if (chapterEntity != null)
                {
                    chapterEntity = chapter;
                }

               a= db.SaveChanges();
            }
            return a;
        }
        public static int UpdateChapterByChapterId(string chapterId, string chapterTitle, string chapterContextHtml, string chapterSummary, int v)
        {
            int a = 0;
            using (var db = new DataContext())
            {
                ChapterEntity chapterEntity = db.Chapter
                    .Where(b => b.ChapterId.Equals(chapterId)).FirstOrDefault();
                if (chapterEntity != null)
                {
                    chapterEntity .Title= chapterTitle;
                    chapterEntity .Content= chapterContextHtml;
                    chapterEntity .Summary= chapterSummary;
                    chapterEntity.SerialNumber = v;
                }

                a = db.SaveChanges();
            }
            return a;
        }
        public static int  DeleteChapterByChapterId(string chapterId)
        {
            int a = 0;
            using (var db = new DataContext())
            {

                db.Chapter.Delete(u => u.ChapterId.Equals(chapterId));
                a=db.SaveChanges();
            }
            return a;
         

        }
        public static int DeleteChapterByArticleId(string articleId)
        {
            int a = 0;
            using (var db = new DataContext())
            {
                db.Chapter.Delete(u => u.ArticleId.Equals(articleId));
               a= db.SaveChanges();
            }
            return a;

        }

       
    }
}
