using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace morenote_sync_cli.Models.Model
{
    /// <summary>
    /// 命令模型
    /// </summary>
    public class CommandModel
    {
        /// <summary>
        /// 主命令
        /// </summary>
        public string MainCommand { get;set;}
        /// <summary>
        /// 命令参数
        /// </summary>
        public Dictionary<string, string> Parameters {get;set; }=new Dictionary<string, string>();


        public string CurDir{get;set;}
      
        /// <summary>
        /// 是否存在这个参数
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        public bool IsExistParameter(string Parameter)
        {
            return Parameters.ContainsKey(Parameter);
        }
        /// <summary>
        /// 获取命令参数的值
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string GetParameterValue(string parameter)
        {
            if (this.Parameters.ContainsKey(parameter))
            {
                return this.Parameters[parameter];
            }
            else
            {
                return null;
            }
        }
        public static CommandModel Instance(string[] args,string curDir)
        {
            var cmd = new CommandModel();
            var list=args.ToList();
            cmd.MainCommand = list[0];
            cmd.CurDir = curDir;

            for (int index = 1; index < list.Count; index++)
            {
                if (list[index].Contains("-")&&index<(list.Count-1)&&!list[index+1].Contains("-"))
                {
                    cmd.Parameters.Add(list[index],list[index+1]);
                    index++;
                }
                else
                {
                     cmd.Parameters.Add(list[index],String.Empty);
                }
            }
          
            return cmd;
        }
        public string Print()
        {
            StringBuilder sb=new StringBuilder();
            sb.Append(this.MainCommand+"\r\n");
            foreach (var item in this.Parameters)
            {
                sb.Append(item.Key+"="+item.Value+"\r\n");
            }
           return sb.ToString();
        }

    }
}
