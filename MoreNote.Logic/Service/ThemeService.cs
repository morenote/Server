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
       
         DependencyInjectionService dependencyInjectionService;
        public ThemeService(DependencyInjectionService dependencyInjectionService)
        {
            this.dependencyInjectionService = dependencyInjectionService;
           
        }

        public  FriendLinks[] GetURLs(long themeId)
        {

          	using(var dataContext = dependencyInjectionService.GetDataContext())
		{
		 var result = dataContext.FriendLinks.
                    Where(b => b.ThemeId.Equals(themeId));

                return result.ToArray();
		
		}
               
            
        }
        public  bool InsertTheme(Theme theme)
        {
            	using(var dataContext = dependencyInjectionService.GetDataContext())
		{
		 var result = dataContext.Theme.Add(theme);
                return dataContext.SaveChanges() > 0;
		
		}
               
            

        }
        public  bool InsertURL(FriendLinks fl)
        {	using(var dataContext = dependencyInjectionService.GetDataContext())
		{
		  var result = dataContext.FriendLinks.Add(fl);
                return dataContext.SaveChanges() > 0;
		
		}
            
              
            

        }
    }
}
