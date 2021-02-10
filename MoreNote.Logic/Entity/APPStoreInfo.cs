using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;

namespace MoreNote.Logic.Entity
{
   public class APPStoreInfo
    {
      public Resp_Data resp_data { get; set; }
        public void Test()
        {
           
        }

    }
    public class Resp_Data
    {
        public AppInfo[] app_list { get; set; }
    }
    [Table("app_info")]
    public class AppInfo
    {
        [Key]
        public long? appid { get; set; }
        public string appautor { get; set; }
        public string appdetail { get; set; }
        public string appname { get; set; }
        public string apppackage { get; set; }
        public string appdownurl { get; set; }
        public string applogourl { get; set; }
        public string appversion { get; set; }
        public string[] imglist { get; set; }
        public string appsize { get; set; }//多少字节 long? 1345616451515
        public bool agreement;
    }
}
