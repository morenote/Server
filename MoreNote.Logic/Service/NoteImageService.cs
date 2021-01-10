using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MoreNote.Logic.Service
{
    public class NoteImageService
    {
        private DataContext dataContext;

        public NoteImageService(DependencyInjectionService dependencyInjectionService,DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        // 通过id, userId得到noteIds

        public  long GetNoteIds(long imageId)
        {
            return 0;
        }
        //TODO: 这个web可以用, 但api会传来, 不用用了
        // 解析内容中的图片, 建立图片与note的关系
        // <img src="/file/outputImage?fileId=12323232" />
        // 图片必须是我的, 不然不添加
        // imgSrc 防止博客修改了, 但内容删除了
        public  bool UpdateNoteImages(long userId,long noteId,string imgSrc,string content)
        {
            
            return true;
        }
        // 复制图片, 把note的图片都copy给我, 且修改noteContent图片路径
        public  string CopyNoteImages(long fromNoteId,long fromUserId,long newNoteId,string content,long toUserID)
        {
            throw new Exception();
        }
        public  Dictionary<long,List<NoteFile>> getImagesByNoteIds(long[] noteIds)
        {
            //using (var db = DataContext.getDataContext())
            //{
            //    var result = dataContext.NoteImage.Where(b=>noteIds.Contains(b.NoteId));
            //    if (result==null)
            //    {
            //        return null;
            //    }
            //    NoteImage[] noteImages = result.ToArray();
            //    return noteImages;
            //}
            return null;
        }
        public  NoteImage[] getImagesByNoteId(long noteId)
        {
          
                var result = dataContext.NoteImage.Where(b => b.NoteId==noteId);
                NoteImage[] noteImages = result.ToArray();
                return noteImages;
         
            
        }



    }
}
