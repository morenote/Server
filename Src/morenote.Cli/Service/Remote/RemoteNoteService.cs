using morenote_sync_cli.Models.Model.API;
using morenote_sync_cli.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace morenote_sync_cli.Service.Remote
{
    public class RemoteNoteService
    {
        private string baseURL;

        public RemoteNoteService(string url)
        {
            this.baseURL = url;
        }

        /// <summary>
        /// 获得某笔记本下的笔记(无内容)
        /// </summary>
        /// <param name="notebookId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Note[] GetNotes(string notebookId, string token)
        {
            var url = $"{baseURL}/api/note/getNotes?token={token}&notebookId={notebookId}";
            var json = HttpClientUtil.HttpGet(url);
           
            var notes = Note.InstanceArrayFormJson(json);
            return notes;
        }

        public NoteAndContent GetNoteAndContent(string notebookId, string token)
        {
            var url = $"{baseURL}/api/note/getNoteAndContent?token={token}&noteId={notebookId}";
            var json = HttpClientUtil.HttpGet(url);
           
            var noteAndContext = NoteAndContent.InstanceFormJson(json);
            return noteAndContext;
        }

        public NoteContent GetNoteContent(string noteId, string token)
        {
            var url = $"{baseURL}/api/note/getNoteContent?token={token}&noteId={noteId}";
            var json = HttpClientUtil.HttpGet(url);
           
            var content = NoteContent.InstanceFormJson(json);
            return content;
        }

        /// <summary>
        /// 格式化图片，以便上传到服务器
        /// </summary>
        /// <param name="content"></param>
        /// <param name="isMarkdown"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public string FixContentToServer(string content, bool isMarkdown, string baseUrl)
        {
            //开发是不可能开发的，只能靠复制粘贴这个样子
            //todo:需要实现FixContent
            // string baseUrl = ConfigService.GetSiteUrl();
            string baseUrlPattern = baseUrl;

            // 避免https的url
            if (baseUrl.Substring(0, 8).Equals("https://"))
            {
                baseUrlPattern = baseUrl.Replace(@"https://", @"https*://");
            }
            else
            {
                baseUrlPattern = baseUrl.Replace("http://", "https*://");
            }
            baseUrlPattern = "(?:" + baseUrlPattern + ")*";

            var patterns = new Dictionary<string, string>[]
            {
                new Dictionary<string, string>()
                {
                    { "src","src"},{"middle","/api/file/getImage"},{"param","fileId"},{ "to","getImage?fileId="}
                },
                  new Dictionary<string, string>()
                {
                    { "src","src"},{"middle","/file/outputImage"},{"param","fileId"},{ "to","getImage?fileId="}
                },
                new Dictionary<string, string>()
                {
                    { "src","href"},{ "middle","/attach/download"},{ "param","attachId"},{"to","getAttach?fileId=" }
                },
                new Dictionary<string, string>()
                {
                    { "src","href"},{ "middle","/api/file/getAtach"},{ "param","fileId"},{"to","getAttach?fileId=" }
                }
            };
            foreach (var eachPattern in patterns)
            {
                if (!isMarkdown)
                {
                    Regex reg = null;
                    Regex reg2 = null;

                    // 富文本处理

                    // <img src="http://leanote.com/file/outputImage?fileId=5503537b38f4111dcb0000d1">
                    // <a href="http://leanote.com/attach/download?attachId=5504243a38f4111dcb00017d"></a>

                    if (eachPattern["src"].Equals("src"))
                    {
                        reg = new Regex("<img(?:[^>]+?)(?:" + eachPattern["src"] + "=['\"]*" + baseUrlPattern + eachPattern["middle"] + "\\?" + eachPattern["param"] + "=(?:[a-z0-9A-Z]{24})[\"']*)[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        reg2 = new Regex("<img(?:[^>]+?)(" + eachPattern["src"] + "=['\"]*" + baseUrlPattern + eachPattern["middle"] + "\\?" + eachPattern["param"] + "=([a-z0-9A-Z]{24})[\"']*)[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        reg = new Regex("<a(?:[^>]+?)(?:" + eachPattern["src"] + "=['\"]*" + baseUrlPattern + eachPattern["middle"] + "\\?" + eachPattern["param"] + "=(?:[a-z0-9A-Z]{24})[\"']*)[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        reg2 = new Regex("<a(?:[^>]+?)(" + eachPattern["src"] + "=['\"]*" + baseUrlPattern + eachPattern["middle"] + "\\?" + eachPattern["param"] + "=([a-z0-9A-Z]{24})[\"']*)[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    }
                    content = reg.Replace(content, (math) =>
                    {
                        //Console.WriteLine("markdown表达式1=" + math.Value);
                        if (math.Success)
                        {
                            var eachFind = reg2.Match(math.Value);
                            Console.WriteLine(eachFind.Groups.Count);
                            Console.WriteLine(eachFind.Groups[0]);
                            Console.WriteLine(eachFind.Groups[1]);
                            Console.WriteLine(eachFind.Groups[2]);
                            var src = eachPattern["src"] + "=\"" + baseUrl + "/api/file/" + eachPattern["to"] + eachFind.Groups[2] + "\"";
                            var output = math.Value.Replace(eachFind.Groups[1].Value, src);

                            return output;
                        }
                        return math.Value;
                    });
                }
                else
                {
                    var pre = "!";                       // 默认图片
                    if (eachPattern["src"].Equals("href"))
                    { // 是attach
                        pre = "";
                    }
                    // markdown处理
                    // ![](http://leanote.com/file/outputImage?fileId=5503537b38f4111dcb0000d1)
                    // [selection 2.html](http://leanote.com/attach/download?attachId=5504262638f4111dcb00017f)
                    // [all.tar.gz](http://leanote.com/attach/downloadAll?noteId=5503b57d59f81b4eb4000000)
                    Regex regImageMarkdown = new Regex($@"{pre}\[(?:[^]]*?)\]\({baseUrlPattern}{eachPattern["middle"]}\?{eachPattern["param"]}=(?:[a-z0-9A-Z]{{24}})\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    Regex regImageMarkdown2 = new Regex($@"{pre}\[([^]]*?)\]\({baseUrlPattern}{eachPattern["middle"]}\?{eachPattern["param"] }=([a-z0-9A-Z]{{24}})\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    Console.WriteLine($@"{pre}\[(?:[^]]*?)\]\({baseUrlPattern}{eachPattern["middle"]}\?{eachPattern["param"]}=(?:[a-z0-9A-Z]{24})\)");
                    content = regImageMarkdown.Replace(content, (math) =>
                    {
                        //Console.WriteLine("markdown表达式1=" + math.Value);
                        if (regImageMarkdown2.IsMatch(math.Value))
                        {
                            var eachFind = regImageMarkdown2.Match(math.Value);
                            Console.WriteLine(eachFind.Groups.Count);
                            Console.WriteLine(eachFind.Groups[0]);
                            Console.WriteLine(eachFind.Groups[1]);
                            Console.WriteLine(eachFind.Groups[2]);
                            return pre + "[" + eachFind.Groups[1] + "](" + baseUrl + "/api/file/" + eachPattern["to"] + eachFind.Groups[2] + ")";
                        }
                        return math.Value;
                    });
                }
            }
            return content;
        }

        /// <summary>
        /// 格式化图片，以便本地访问
        /// </summary>
        /// <param name="content"></param>
        /// <param name="isMarkdown"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
         public string FixContentToLocal(string content, bool isMarkdown, string baseUrl)
        {
            //开发是不可能开发的，只能靠复制粘贴这个样子
            //todo:需要实现FixContent
            // string baseUrl = ConfigService.GetSiteUrl();
            string baseUrlPattern = baseUrl;

            // 避免https的url
            if (baseUrl.Substring(0, 8).Equals("https://"))
            {
                baseUrlPattern = baseUrl.Replace(@"https://", @"https*://");
            }
            else
            {
                baseUrlPattern = baseUrl.Replace("http://", "https*://");
            }
            baseUrlPattern = "(?:" + baseUrlPattern + ")*";

            var patterns = new Dictionary<string, string>[]
            {
                new Dictionary<string, string>()
                {
                    { "src","src"},{"middle","/api/file/getImage"},{"param","fileId"},{ "to","getImage?fileId="}
                },
                  new Dictionary<string, string>()
                {
                    { "src","src"},{"middle","/file/outputImage"},{"param","fileId"},{ "to","getImage?fileId="}
                },
                new Dictionary<string, string>()
                {
                    { "src","href"},{ "middle","/attach/download"},{ "param","attachId"},{"to","getAttach?fileId=" }
                },
                new Dictionary<string, string>()
                {
                    { "src","href"},{ "middle","/api/file/getAtach"},{ "param","fileId"},{"to","getAttach?fileId=" }
                }
            };
            foreach (var eachPattern in patterns)
            {
                if (!isMarkdown)
                {
                    Regex reg = null;
                    Regex reg2 = null;

                    // 富文本处理

                    // <img src="http://leanote.com/file/outputImage?fileId=5503537b38f4111dcb0000d1">
                    // <a href="http://leanote.com/attach/download?attachId=5504243a38f4111dcb00017d"></a>

                    if (eachPattern["src"].Equals("src"))
                    {
                        reg = new Regex("<img(?:[^>]+?)(?:" + eachPattern["src"] + "=['\"]*" + baseUrlPattern + eachPattern["middle"] + "\\?" + eachPattern["param"] + "=(?:[a-z0-9A-Z]{24})[\"']*)[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        reg2 = new Regex("<img(?:[^>]+?)(" + eachPattern["src"] + "=['\"]*" + baseUrlPattern + eachPattern["middle"] + "\\?" + eachPattern["param"] + "=([a-z0-9A-Z]{24})[\"']*)[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        reg = new Regex("<a(?:[^>]+?)(?:" + eachPattern["src"] + "=['\"]*" + baseUrlPattern + eachPattern["middle"] + "\\?" + eachPattern["param"] + "=(?:[a-z0-9A-Z]{24})[\"']*)[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        reg2 = new Regex("<a(?:[^>]+?)(" + eachPattern["src"] + "=['\"]*" + baseUrlPattern + eachPattern["middle"] + "\\?" + eachPattern["param"] + "=([a-z0-9A-Z]{24})[\"']*)[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    }
                    content = reg.Replace(content, (math) =>
                    {
                        //Console.WriteLine("markdown表达式1=" + math.Value);
                        if (math.Success)
                        {
                            var eachFind = reg2.Match(math.Value);
                            Console.WriteLine(eachFind.Groups.Count);
                            Console.WriteLine(eachFind.Groups[0]);
                            Console.WriteLine(eachFind.Groups[1]);
                            Console.WriteLine(eachFind.Groups[2]);
                            var src = eachPattern["src"] + "=\"" + baseUrl + "/api/file/" + eachPattern["to"] + eachFind.Groups[2] + "\"";
                            var output = math.Value.Replace(eachFind.Groups[1].Value, src);

                            return output;
                        }
                        return math.Value;
                    });
                }
                else
                {
                    var pre = "!";                       // 默认图片
                    if (eachPattern["src"].Equals("href"))
                    { // 是attach
                        pre = "";
                    }
                    // markdown处理
                    // ![](http://leanote.com/file/outputImage?fileId=5503537b38f4111dcb0000d1)
                    // [selection 2.html](http://leanote.com/attach/download?attachId=5504262638f4111dcb00017f)
                    // [all.tar.gz](http://leanote.com/attach/downloadAll?noteId=5503b57d59f81b4eb4000000)
                    Regex regImageMarkdown = new Regex($@"{pre}\[(?:[^]]*?)\]\({baseUrlPattern}{eachPattern["middle"]}\?{eachPattern["param"]}=(?:[a-z0-9A-Z]{{24}})\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    Regex regImageMarkdown2 = new Regex($@"{pre}\[([^]]*?)\]\({baseUrlPattern}{eachPattern["middle"]}\?{eachPattern["param"] }=([a-z0-9A-Z]{{24}})\)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    Console.WriteLine($@"{pre}\[(?:[^]]*?)\]\({baseUrlPattern}{eachPattern["middle"]}\?{eachPattern["param"]}=(?:[a-z0-9A-Z]{24})\)");
                    content = regImageMarkdown.Replace(content, (math) =>
                    {
                        //Console.WriteLine("markdown表达式1=" + math.Value);
                        if (regImageMarkdown2.IsMatch(math.Value))
                        {
                            var eachFind = regImageMarkdown2.Match(math.Value);
                            Console.WriteLine(eachFind.Groups.Count);
                            Console.WriteLine(eachFind.Groups[0]);
                            Console.WriteLine(eachFind.Groups[1]);
                            Console.WriteLine(eachFind.Groups[2]);
                            return pre + "[" + eachFind.Groups[1] + "](" + baseUrl + "/api/file/" + eachPattern["to"] + eachFind.Groups[2] + ")";
                        }
                        return math.Value;
                    });
                }
            }
            return content;
        }
    }
}