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
        public int Code;
        public string Msg;
        public string Id;
        public string[] List;
        public string[] Item;
    }
}
