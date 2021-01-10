using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Logic.Service
{
    public class UpgradeService
    {
        // 添加了PublicTime, RecommendTime
        public  bool UpgradeBlog()
        {
            throw new Exception();
        }
        // 11-5自定义博客升级, 将aboutMe移至pages
        /*
        <li>Migrate "About me" to single(a single post)</li>
        <li>Add some default themes to administrator</li>
        <li>Generate "UrlTitle" for all notes. "UrlTitle" is a friendly url for post</li>
        <li>Generate "UrlTitle" for all notebooks</li>
        <li>Generate "UrlTitle" for all singles</li>
        */
        public  bool UpgradeBetaToBeta2(long userId)
        {
            throw new Exception();
        }
        // Usn设置
        // 客户端 api
        public  void moveTag()
        {
            throw new Exception();
        }
        public  void setNotebookUsn()
        {
            throw new Exception();
        }
        public  void setNoteUsn()
        {
            throw new Exception();
        }
        // 升级为Api, beta.4
        public  bool Api(long userId)
        {
            throw new Exception();

        }

    }
}
