using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace morenote_sync_cli.Interface
{
    public interface IPrinter
    {
        public void WirteLine(string msg);

        public void WriteError(string msg);

        public void WriteWarning(string msg);

        public void WriteSuccess(string msg);
    }
}