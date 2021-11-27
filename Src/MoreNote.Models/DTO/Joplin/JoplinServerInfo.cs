using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Models.DTO.Joplin
{
   public class JoplinServerInfo
    {
        public int version{get;set;}=3;

        public E2EE e2ee{get;set;}=new E2EE();
        public ActiveMasterKeyId activeMasterKeyId{get;set;}=new ActiveMasterKeyId();
        public List<string> masterKeys=new List<string>();
    }
}
