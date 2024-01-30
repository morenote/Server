using morenote_sync_cli.ExtensionMethod;
using morenote_sync_cli.Interface;
using morenote_sync_cli.Models.Model;
using morenote_sync_cli.Service;
using morenote_sync_cli.Service.Printer;
using morenote_sync_cli.Utils;
using System.Text;

namespace morenote_sync_cli;

public class Program
{
    private static string dsc = PathUtil.dsc.ToString();
    private static IPrinter printer=new ConsolePrinter();
   
    public static void Main(string[] args)
    {
        if (args == null || !args.Any())
        {
            Printer.WriteLine("Invalid input parameter",ConsoleColor.Red);
            
            PrintHelper();
            return;
        }
        var argsList = args.ToList();

        var cmd=CommandModel.Instance(args,PathUtil.GetCurDir());
        printer.WriteSuccess(cmd.Print());
       
        Console.WriteLine("=================");
        AppService app=new AppService(cmd,printer);
        switch (cmd.MainCommand)
        {
            case "init":
                app.Init();
                break;
            case "clone":
                app.Clone();
                break;
            case "update":
                Printer.WriteLine("开发中");
                break;
            default:
                break;
        }
        if (argsList[0].ToLower().Equals("help"))
        {
            PrintHelper();
            return;
        }
    }

    public static void PrintHelper()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("These are common msync commands used in various situations: \r\n");
        stringBuilder.Append("msync init        Initialize a workspace folder \r\n");
        stringBuilder.Append("msync clone       Clone the remote notes to the current folder \r\n");
        stringBuilder.Append("msync commit      Submit notes to local database \r\n");
        stringBuilder.Append("msync update      Sync data (pull&push)\r\n");
        stringBuilder.Append("msync status      View current status\r\n");
        stringBuilder.Append("msync gc          Cache file and garbage file recovery, compression database\r\n");
        stringBuilder.Append("msync push        Push data to the server\r\n");
        stringBuilder.Append("msync pull        Pull notes from the server\r\n");
        stringBuilder.Append("msync checkout    set-url -u URL\r\n");
        stringBuilder.Append("msync remte login -n userName -p password\r\n");
        stringBuilder.Append("msync archive\r\n");

        stringBuilder.Append("mnsc help\r\n");
        Console.WriteLine(stringBuilder);
    }
}