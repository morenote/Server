using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;

namespace MoreNote.Logic.Service
{
    public class ThemeService
    {
        public static FriendLinks[] GetURLs(long themeId)
        {

            using (var db = DataContext.getDataContext())
            {
                var result = db.FriendLinks.
                    Where(b => b.ThemeId.Equals(themeId));

                return result.ToArray();
            }
        }
        public static bool InsertTheme(Theme theme)
        {
            using (var db = DataContext.getDataContext())
            {
                var result = db.Theme.Add(theme);
                return db.SaveChanges() > 0;
            }

        }
        public static bool InsertURL(FriendLinks fl)
        {
            using (var db = DataContext.getDataContext())
            {
                var result = db.FriendLinks.Add(fl);
                return db.SaveChanges() > 0;
            }

        }
    }
}
