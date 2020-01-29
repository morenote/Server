using System;
using System.Collections.Generic;
using System.Text;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;

namespace MoreNote.Logic.Service
{
    public class AttachService
    {
        //add attach
        //api 调用时，添加attach之前时没有note的
        //fromApi表示是api添加的, updateNote传过来的, 此时不要incNote's usn, 因为updateNote会inc的
        public static bool AddAttach(AttachInfo attachInfo,bool fromApi,string msg)
        {
            attachInfo.CreatedTime = DateTime.Now;
            int a = 0;
            using (var db = new DataContext())
            {
                
                var result = db.AttachInfo.Add(attachInfo);
                a = db.SaveChanges();
                
            }
            if (a < 1) return false;

            return false;



        }
        public static bool UpdateNoteAttachNum(long noteId,int addNUm)
        {
         throw new Exception();
        }
        public static AttachInfo[] ListAttachs(long noteId,long userId)
        {
            throw new Exception();
        }
        public static Dictionary<string,AttachInfo[] > getAttachsByNoteIds(long[] noteId)
        {
            return null;
        }
        public static bool UpdateImageTitle(long userId,long fileId,string title)
        {
           throw new Exception();
        }
        public static bool DeleteAllAttachs(long noteId,long userId)
        {
            throw new Exception();
        }
        public static bool DeleteAttach(long attachId ,long userId)
        {
            throw new Exception();
        }
        public static AttachInfo GetAttach(long attachId,long userId)
        {
            throw new Exception();
        }
        public static bool CopyAttachs(long noteId,long toNoteId)
        {
            throw new Exception();
        }

        public static bool UpdateOrDeleteAttachApi(long noteId,NoteFile[] files)
        {
            throw new Exception();
        }


    }
}
