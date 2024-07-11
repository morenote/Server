using Morenote.Models.Models.Entity;

using MoreNote.Models.Enums.VCS;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entiys.VCS
{
    /// <summary>
    /// 提交内容
    /// </summary>
    public class ActionEntity:BaseEntity
    {

        public ActionTarget ActionTarget { get; set; }

        public long? TargetId { get; set; }
        public ActionType ActionType { get; set; }

        public string? Value { get; set; } 
    }
}
