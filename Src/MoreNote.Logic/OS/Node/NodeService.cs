using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliWrap;

namespace MoreNote.Logic.OS.Node
{
    public class NodeService
    {

        public NodePackageManagement GetNPM(NPMType npmName)
        {
            switch (npmName)
            {
                case NPMType.NPM:
                    return new NPMService();
                 
                case NPMType.PNPM:
                    return new PNPMService();
                  
                case NPMType.YARN:
                    return new Yarn();
                default:
                    return new NPMService();
            }


        }

    }
}
