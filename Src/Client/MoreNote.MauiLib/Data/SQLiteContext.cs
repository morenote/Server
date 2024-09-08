using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using MoreNote.Models.Entity.Leanote.Notes;
using MoreNote.Models.Entiys.Leanote.Notes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.MauiLib.Data
{
  
    public class SQLiteContext: DbContext
    {
        string connectionString;

        public DbSet<Notebook> Notebook { get; set; }

        public DbSet<NoteCollection> NoteCollection { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<NoteContent> NoteContents { get; set; }


        public SQLiteContext(string databaseName)
        {
           
             connectionString = new SqliteConnectionStringBuilder()
            {
                Mode = SqliteOpenMode.ReadWriteCreate,
                DataSource = databaseName,
                Password = ""
            }.ToString();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString);
            
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        //public static  void Migrate(string dbName)
        //{
        //    using (var db=new SQLiteContext(dbName))
        //    {
        //        db.Database.Migrate();
        //        db.SaveChanges();
        //    }

        //}

    }
}
