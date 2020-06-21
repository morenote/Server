using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using Z.EntityFramework.Plus;

namespace MoreNote.Logic.Service
{
    public class AttachService
    {
        //add attach
        //api 调用时，添加attach之前时没有note的
        //fromApi表示是api添加的, updateNote传过来的, 此时不要incNote's usn, 因为updateNote会inc的
        public static bool AddAttach(AttachInfo attachInfo,bool fromApi,out string msg)
        {
            attachInfo.CreatedTime = DateTime.Now;
            int a = 0;
            using (var db = DataContext.getDataContext())
            {
                
                db.AttachInfo.Add(attachInfo);
                var result= db.SaveChanges()>0;

                // api调用时, 添加attach之前是没有note的
                if (result)
                {
                    UpdateNoteAttachNum(attachInfo.NoteId,1);

                }
                if (!fromApi)
                {
                    NoteService.IncrNoteUsn(attachInfo.NoteId, attachInfo.UserId);

                }
                msg="";
                return true;
            }
        }
        // 更新笔记的附件个数
        // addNum 1或-1
        public static bool UpdateNoteAttachNum(long noteId,int addNUm)
        {
            using (var db = DataContext.getDataContext())
            {
                Note note=db.Note
                    .Where(b=>b.NoteId==noteId).FirstOrDefault();
                if (note==null)
                {return false;

                }
                else
                {
                    note.AttachNum= note.AttachNum+addNUm;
                    db.SaveChanges();
                    return true;
                }
            }
        }
        public static AttachInfo[] ListAttachs(long noteId,long userId)
        {
            
            using (var db=new DataContext())
            {
                var attachs=db.AttachInfo.Where(b=>b.NoteId==noteId&&b.UserId==userId).ToArray();
               return attachs;
                //todo:// 权限控制
            }
            throw new Exception();
        }
        public static AttachInfo[] getAttachsByNoteIds(long[] noteIds)
        {
            using (var db = DataContext.getDataContext())
            {
                AttachInfo[] attachs = db.AttachInfo.Where(b => noteIds.Contains(b.NoteId) ).ToArray();
                return attachs;
                //todo:// 权限控制
            }
        }
        public static AttachInfo[] getAttachsByNoteId(long noteId)
        {
            using (var db = DataContext.getDataContext())
            {
                var attachs = db.AttachInfo.Where(b => b.NoteId == noteId).ToArray();
                return attachs;
                //todo:// 权限控制

            }

        }

        public static bool UpdateImageTitle(long userId,long fileId,string title)
        {
           throw new Exception();
        }
        public static bool DeleteAllAttachs(long noteId,long userId)
        {
            using (var db=new DataContext())
            {
                var attachInfos=db.AttachInfo.Where(b=>b.NoteId==noteId&&b.UserId==userId).ToArray();
                if (attachInfos!=null&&attachInfos.Length>0)
                {

                    foreach (var attach in attachInfos)
                    {

                        DeleteAttach(attach.AttachId,userId);
                    }

                }

                return true;
            }
            //todo :需要实现此功能
            return true;
        }
        // delete attach
        // 删除附件为什么要incrNoteUsn ? 因为可能没有内容要修改的
        public static bool DeleteAttach(long attachId ,long userId)
        {
            if (attachId!=0&&userId!=0)
            {

                using (var db=new DataContext())
                {
                  var attach=  db.AttachInfo.Where(b=>b.AttachId==attachId&&b.UserId==userId).FirstOrDefault();
                    long noteId= attach.NoteId;
                    string path=attach.Path;
                  db.AttachInfo.Where(b => b.AttachId == attachId && b.UserId == userId).Delete();
                  UpdateNoteAttachNum(noteId, -1);
                    DeleteAttachOnDisk(path);
                    return true;
                 // db.AttachInfo.Remove(attach);
                }
            }
            else
            {
                return false;
            }
        }
        //todo： 考虑该函数的删除文件的安全性，是否存在注入的风险
        private static void DeleteAttachOnDisk(string path)
        {
         
            File.Delete(path);
        }
        public static AttachInfo GetAttach(long attachId,long userId)
        {
            throw new Exception();
        }
        public static AttachInfo GetAttach(long attachId)
        {
            using (var db=new DataContext())
            {
                var result=db.AttachInfo.Where(b=>b.AttachId==attachId);
                return result==null?null:result.FirstOrDefault();

            }
        }
        public static bool CopyAttachs(long noteId,long toNoteId)
        {
            throw new Exception();
        }

        public static bool UpdateOrDeleteAttachApi(long noteId,long userId,APINoteFile[] files)
        {

            var attachs=ListAttachs(noteId,userId);
            HashSet<string> nowAttachs=new HashSet<string>(20);
            foreach (var file in files)
            {
                if (file.IsAttach&& !string.IsNullOrEmpty(file.FileId))
                {
                    nowAttachs.Add(file.FileId);
                }

            }
            foreach (var attach in attachs)
            {
              var fileId=attach.AttachId.ToString("x");
                if (!nowAttachs.Contains(fileId))
                {
                    // 需要删除的
                    // TODO 权限验证去掉
                    DeleteAttach(attach.AttachId,userId);

                }
            }
           return true;
        }
        

    }
}
