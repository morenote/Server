using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MoreNote.Logic.Entity
{
    // controller ajax返回
    public class Re
    {
        public bool Ok { get; set; }
        public int Code { get; set; }
        public string Msg { get; set; }
        public string Id { get; set; }
        public string List { get; set; }
        public Dictionary<string,string> Item { get; set; }
        public static  Re NewRe()
        {
            return new Re()
            {
                Ok=false
            };
        }
    }
}
