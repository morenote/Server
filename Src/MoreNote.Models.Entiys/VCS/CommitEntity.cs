using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entiys.VCS
{
    public class CommitEntity:BaseEntity
    {

        public long? CommitUser {  get; set; }
        public DateTime? CommitDate { get; set; }
        public string? CommitMessage { get; set; }
        public string? HASH { get; set; }
        public ActionEntity[]? ActionCollection { get; set; }

    }
}
