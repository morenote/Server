using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreNote.Logic.Entity
{
    public class SpamInfo
    {
        [Key]
        public long SpamId { get; set; }
        public string Input { get; set; }
        public bool Prediction { get; set; }//机器识别结果
        public float Score { get; set; }//机器识别可靠性
        public bool ManualCheck { get; set; }//是否经过人工复核
        public bool ManualResult { get; set; }//人工识别结果
        public DateTime Data { get; set; }//输入时间
    }
}
