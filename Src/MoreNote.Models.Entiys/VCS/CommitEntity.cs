using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entiys.VCS
{
    /// <summary>
    /// 提交对象
    /// </summary>
    public class CommitEntity:BaseEntity
    {
        /// <summary>
        /// 提交用户
        /// </summary>
        public long? Parent {  get; set; }
        public long? Author {  get; set; }
        public long? Committer {  get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime? CommitDate { get; set; }
        /// <summary>
        /// 提交消息
        /// </summary>
        public string? CommitMessage { get; set; }
        /// <summary>
        /// SHA256
        /// </summary>
        public string? SHA256 { get; set; }
        /// <summary>
        /// 提交内容的集合
        /// </summary>
        public ActionEntity[]? ActionCollection { get; set; }

    }
}
