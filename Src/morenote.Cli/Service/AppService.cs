using morenote_sync_cli.ExtensionMethod;
using morenote_sync_cli.Interface;
using morenote_sync_cli.Models.Model;
using morenote_sync_cli.Models.Model.API;
using morenote_sync_cli.Service.Remote;
using morenote_sync_cli.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace morenote_sync_cli.Service
{
    public class AppService
    {
        private CommandModel cmd;
       
        private IPrinter printer;
        LocalNoteRepository localNoteRepository;
        public AppService(CommandModel cmd, IPrinter printer)
        {
            this.cmd = cmd;
            this.printer = printer;
            localNoteRepository = new LocalNoteRepository(cmd.CurDir, printer);
        }

        public void Init()
        {
            Console.WriteLine("正在初始化文件夹");
            var curDir = PathUtil.GetCurDir();

            if (Directory.Exists($"{curDir}/.msync".PathConvert()))
            {
                Console.WriteLine(".Config已经存在");
            }
            else
            {
                Directory.CreateDirectory($"{curDir}/.msync".PathConvert());
            }
            if (File.Exists($"{curDir}/.msync/RepositoryStatus.json".PathConvert()))
            {
                Console.WriteLine("已经初始化RepositoryStatus.json，无需初始化");
            }
            else
            {
                File.Copy($"{PathUtil.GetExeDir()}/.msync/RepositoryStatus.json".PathConvert(), $"{curDir}/.msync/RepositoryStatus.json".PathConvert());
                Console.WriteLine("成功初始化RepositoryStatus.json.json");
            }
            Utils.Printer.WriteLine("Success!!!", ConsoleColor.Green);
        }

        /// <summary>
        /// 克隆笔记到当前文件夹
        /// </summary>
        public void Clone()
        {   
            Init();
           
            var token = string.Empty;
            if (cmd.IsExistParameter("-token"))
            {
                token = cmd.GetParameterValue("-token");
            }
            else
            {
                if (cmd.IsExistParameter("-email") && cmd.IsExistParameter("-password"))
                {
                    RemoteAuthService remoteAuthService = new RemoteAuthService(cmd.GetParameterValue("-remote"));
                    this.printer.WirteLine("remoteAuthService.Login");
                    AuthOk authOk = remoteAuthService.Login(cmd.GetParameterValue("-email"), cmd.GetParameterValue("-password"));
                    if (authOk.Ok)
                    {
                        printer.WriteSuccess(authOk.ToString());
                    }
                    else
                    {
                        printer.WriteError(authOk.ToString());

                    }
                    localNoteRepository.InitUserInfo(authOk, cmd);
                    RemoteNoteBookService remoteNoteBookService=new RemoteNoteBookService(cmd.GetParameterValue("-remote"));
                    var books=remoteNoteBookService.GetNotebooks(authOk.Token);
                    printer.WriteSuccess("已经请求到bookList");
                    localNoteRepository.InitBooks(books);

                }
            }
        }
    }
}