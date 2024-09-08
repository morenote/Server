


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
            
            var result= await  SQLiteManager.EnsureDeletedAsync();
            if (result)
            {
                await _message.Success("EnsureDeletedAsync is Success");
            }
            else
            {
                await _message.Error("EnsureDeletedAsync is Error");
            }
        }
        public async void CheckStatus()
        {
            //var id= idGenerator.NextHexId();
            if (SQLiteManager.Exists())
            {
                await _message.Success($"SQLite：已经初始化完成。");
            }
            else
            {
                await _message.Error($"SQLite：当前处于销毁状态。");

            }
        }
        public async void EnsureCreatedAsync()
        {
           

            var result= await SQLiteManager.EnsureCreatedAsync();
            if (result)
            {
               await _message.Success("EnsureCreatedAsync is Success");
            }
            else
            {
                await _message.Error("EnsureCreatedAsync is Error");
            }
        }
    }
}
