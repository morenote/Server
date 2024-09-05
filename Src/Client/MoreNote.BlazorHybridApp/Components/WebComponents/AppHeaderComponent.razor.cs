


using MoreNote.MauiLib.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.BlazorHybridApp.Components.WebComponents
{
    public partial class AppHeaderComponent
    {

       public async void EnsureDeletedAsync()
        {
          await  SQLiteManager.EnsureDeletedAsync();
        }
        public async void EnsureCreatedAsync()
        {
            await SQLiteManager.EnsureCreatedAsync();
        }
    }
}
