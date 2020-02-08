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
        // 通过id, userId得到noteIds
        public static long GetNoteIds(long imageId)
        {
            return 0;
        }
        //TODO: 这个web可以用, 但api会传来, 不用用了
        // 解析内容中的图片, 建立图片与note的关系
        // <img src="/file/outputImage?fileId=12323232" />
        // 图片必须是我的, 不然不添加
        // imgSrc 防止博客修改了, 但内容删除了
        public static bool UpdateNoteImages(long userId,long noteId,string imgSrc,string content)
        {
            
            return true;
        }
        // 复制图片, 把note的图片都copy给我, 且修改noteContent图片路径
        public static string CopyNoteImages(long fromNoteId,long fromUserId,long newNoteId,string content,long toUserID)
        {
            throw new Exception();
        }
        public static NoteFile[] getImagesByNoteIds(long[] noteIds)
        {
            //using (var db = new DataContext())
            //{
            //    var result = db.NoteImage.Where(b=>noteIds.Contains(b.NoteId));
            //    if (result==null)
            //    {
            //        return null;
            //    }
            //    NoteImage[] noteImages = result.ToArray();
            //    return noteImages;
            //}
            return null;
        }
        public static NoteImage[] getImagesByNoteId(long noteId)
        {
            using (var db = new DataContext())
            {
                var result = db.NoteImage.Where(b => b.NoteId==noteId);
                NoteImage[] noteImages = result.ToArray();
                return noteImages;
            }
            
        }



    }
}
