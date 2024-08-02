using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entiys.VCS
{
    /// <summary>
    /// 快照
    /// </summary>
    public class SnapshotEntity:BaseEntity
    {
        /// <summary>
        /// 父快照
        /// </summary>
        public long Parent { get; set; }
        /// <summary>
        /// 快照高度
        /// </summary>
        public uint Height { get; set; }
        /// <summary>
        /// Tree索引
        /// </summary>
        public long[] NoteBook { get; set; }

    }
}
