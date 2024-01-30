using morenote_sync_cli.Interface;
using morenote_sync_cli.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace morenote_sync_cli.Service.Printer
{
    public class ConsolePrinter : IPrinter
    {
        public void WirteLine(string msg)
        {
            Utils.Printer.WriteLine(msg);
        }

        public void WriteError(string msg)
        {
            Utils.Printer.WriteLine(msg, ConsoleColor.Red);
        }

        public void WriteSuccess(string msg)
        {
            Utils.Printer.WriteLine(msg, ConsoleColor.Green);
        }

        public void WriteWarning(string msg)
        {
            Utils.Printer.WriteLine(msg, ConsoleColor.Yellow);
        }
    }
}