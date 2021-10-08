using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Models.DTO.Vditor.Upload
{
   public class UploadData
    {
        public List<string> errFiles{get;set;}=new List<string>();
        public Dictionary<string,string> succMap{get;set;}=new Dictionary<string, string>();
    }
}
