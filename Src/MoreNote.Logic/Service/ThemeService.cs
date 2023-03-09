using MoreNote.Logic.Database;
using MoreNote.Models.Entity.Leanote.Blog;
using System.Linq;

namespace MoreNote.Logic.Service
{
    public class ThemeService
    {
        private DataContext dataContext;

        public ThemeService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public FriendLinks[] GetURLs(long? themeId)
        {
            var result = dataContext.FriendLinks.
                       Where(b => b.ThemeId.Equals(themeId));

            return result.ToArray();
        }

        public bool InsertTheme(Theme theme)
        {
            var result = dataContext.Theme.Add(theme);
            return dataContext.SaveChanges() > 0;
        }

        public bool InsertURL(FriendLinks fl)
        {
            var result = dataContext.FriendLinks.Add(fl);
            return dataContext.SaveChanges() > 0;
        }
    }
}