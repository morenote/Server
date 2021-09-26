using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Entity.File
{
    public class UploadSizeLimit
    {
        public long uploadImageSize { get;set;}
        public long uploadBlogLogoSize { get;set;}
        public long uploadAttachSize { get;set;}
        public long uploadAvatarSize { get;set;}
    }
}
