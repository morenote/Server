using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Models.DTO.Vditor.Upload
{
   public class FetchFileResponse
    {
        public string msg{get;set;}=string.Empty;
        public int code{get;set;}=0;
        public FetchData data{get;set;}

    }
}
