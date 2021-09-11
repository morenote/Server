using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MoreNote.Logic.Entity
{
    // controller ajax返回
    public class ResponseMessage
    {
        public bool Ok { get; set; }
        public int Code { get; set; }
        public string Msg { get; set; }
        public string Id { get; set; }
        public dynamic List { get; set; }
        public dynamic Item { get; set; }
        public static  ResponseMessage NewRe()
        {
            return new ResponseMessage()
            {
                Ok=false
            };
        }
    }
}
