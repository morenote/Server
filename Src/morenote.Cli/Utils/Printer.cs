using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace morenote_sync_cli.Utils
{
    public class Printer
    {
        public static void WriteLine(string msg,ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void WriteLine(string msg)
        {
            
            Console.WriteLine(msg);
            
        }
    }
}
