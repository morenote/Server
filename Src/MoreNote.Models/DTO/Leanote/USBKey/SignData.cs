using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.DTO.Leanote.USBKey
{
    public class SignData
    {
        public string Data { get; set; }//待签名数据 可以为json或xml
        public byte[] GetBytes()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Data=" + this.Data);

            var data = Encoding.ASCII.GetBytes(stringBuilder.ToString());
            return data;
        }
    }
}
