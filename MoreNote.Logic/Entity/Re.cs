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
        public string[] List;
        public string[] Item;
        public static Re NewRe()
        {
            return new Re()
            {
                Ok=false
            };
        }
    }
}
