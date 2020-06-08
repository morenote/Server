using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Logic.Entity
{
   public class CrowdfundingProject
    {
        public long ProjectId { get; set; }
        public int Money { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

    }
}
