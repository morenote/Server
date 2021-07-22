using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Z.EntityFramework.Plus;

namespace MoreNote.Logic.Service
{
    public class AttachService
    {
        private DataContext dataContext;
        public NoteService NoteService { get;set;}
        
        public AttachService(DataContext dataContext)
        {
            this.dataContext=dataContext;
           
        }

        //add attach
        //api 调用时，添加attach之前时没有note的
        //fromApi表示是api添加的, updateNote传过来的, 此时不要incNote's usn, 因为updateNote会inc的
        public bool AddAttach(AttachInfo attachInfo, bool fromApi, out string msg)
        {
           
            attachInfo.CreatedTime = DateTime.Now;
            var ok = InsertAttach(attachInfo);
            var note = NoteService.GetNoteById(attachInfo.NoteId);
            long? userId = 0L;
            if (note.NoteId !=null)
            {
                userId = note.UserId;
            }
            else
            {
                userId = attachInfo.UploadUserId;
            }
            if (ok)
            {
                // 更新笔记的attachs num
                UpdateNoteAttachNum(attachInfo.NoteId, 1);
            }
            if (!fromApi)
            {
                // 增长note's usn
                NoteService.IncrNoteUsn(attachInfo.NoteId, attachInfo.UserId);
            }
            msg = "";
            return true;
        }

        //插入AttachInfo
        private bool InsertAttach(AttachInfo attachInfo)
        {
           
                dataContext.AttachInfo.Add(attachInfo);
                return dataContext.SaveChanges() > 0;
            
        }

        // 更新笔记的附件个数
        // addNum 1或-1
        public bool UpdateNoteAttachNum(long? noteId, int addNUm)
        {
                Note note = dataContext.Note
                   .Where(b => b.NoteId == noteId).FirstOrDefault();
                if (note == null)
                {
                    return false;
                }
                else
                {
                    note.AttachNum = note.AttachNum + addNUm;
                    dataContext.SaveChanges();
                    return true;
                }
            
        }

        // list attachs
        public AttachInfo[] ListAttachs(long? noteId, long? userId)
        {
           
                var attachs = dataContext.AttachInfo.Where(b => b.NoteId == noteId && b.UserId == userId).ToArray();
                return attachs;
                //todo:权限控制
            
        }

        // api调用, 通过noteIds得到note's attachs, 通过noteId归类返回
        public Dictionary<long?, List<AttachInfo>> GetAttachsByNoteIds(long?[] noteIds)
        {
           
                Dictionary<long?, List<AttachInfo>> dic = new Dictionary<long?, List<AttachInfo>>();
                foreach (long? id in noteIds)
                {
                    var result = dataContext.AttachInfo.Where(b => noteIds.Contains(b.NoteId));
                    if (result != null)
                    {
                        List<AttachInfo> attachs = result.ToList<AttachInfo>();
                        dic.Add(id, attachs);
                    }
                }
                return dic;
                //todo:// 权限控制
            
        }

        public bool UpdateImageTitle(long? userId, long? fileId, string title)
        {
           
                var image = dataContext.NoteFile.Where(file => file.FileId == fileId && file.UserId == userId);
                if (image != null)
                {
                    var imageFile = image.FirstOrDefault();
                    imageFile.Title = title;
                }
                return dataContext.SaveChanges() > 0;
            
        }

        public AttachInfo[] GetAttachsByNoteId(long? noteId)
        {
           
                var attachs = dataContext.AttachInfo.Where(b => b.NoteId == noteId).ToArray();
                return attachs;
                //todo:// 权限控制
            
        }

        // Delete note to delete attas firstly
        public bool DeleteAllAttachs(long? noteId, long? userId)
        {
           

           
                var attachInfos = dataContext.AttachInfo.Where(b => b.NoteId == noteId && b.UserId == userId).ToArray();
                if (attachInfos != null && attachInfos.Length > 0)
                {
                    foreach (var attach in attachInfos)
                    {
                        DeleteAttach(attach.AttachId, userId);
                        //todo:实现os删除，因为文件可能在对象储存上
                        /**
                         * 这里存在的问题就是morenote可能允许多种储存方式
                         * 
                         * 文件可能就会落在第三方对象储存服务商的对象储存上
                         * 
                         * 这里要判断附件的地址
                         * 
                         * 同时支持超大附件
                         * */
                    }
                }
                return true;

                //todo :需要实现此功能
            
        }

        // delete attach
        // 删除附件为什么要incrNoteUsn ? 因为可能没有内容要修改的
        public bool DeleteAttach(long? attachId, long? userId)
        {
            if (attachId != 0 && userId != 0)
            {
                
                    var attach = dataContext.AttachInfo.Where(b => b.AttachId == attachId && b.UserId == userId).FirstOrDefault();
                    long? noteId = attach.NoteId;
                    string path = attach.Path;
                    dataContext.AttachInfo.Where(b => b.AttachId == attachId && b.UserId == userId).Delete();
                    UpdateNoteAttachNum(noteId, -1);
                    DeleteAttachOnDisk(path);
                    return true;
                
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

        public AttachInfo GetAttach(long? attachId, long? userId)
        {
            throw new Exception();
        }

        public AttachInfo GetAttach(long? attachId)
        {
            
                var result = dataContext.AttachInfo.Where(b => b.AttachId == attachId);
                return result == null ? null : result.FirstOrDefault();
            
        }

        public bool CopyAttachs(long? noteId, long? toNoteId)
        {
            throw new Exception();
        }

        public bool UpdateOrDeleteAttachApi(long? noteId, long? userId, APINoteFile[] files)
        {
            var attachs = ListAttachs(noteId, userId);
            HashSet<string> nowAttachs = new HashSet<string>(20);
            foreach (var file in files)
            {
                if (file.IsAttach && !string.IsNullOrEmpty(file.FileId))
                {
                    nowAttachs.Add(file.FileId);
                }
            }
            foreach (var attach in attachs)
            {
                var fileId = attach.AttachId.ToHex24();
                if (!nowAttachs.Contains(fileId))
                {
                    // 需要删除的
                    // TODO 权限验证去掉
                    DeleteAttach(attach.AttachId, userId);
                }
            }
            return true;
        }
    }
}