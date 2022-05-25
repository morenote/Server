using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.VerificationCode
{
    /// <summary>
    /// 生成验证码图片
    /// </summary>
    public interface ICaptchaGenerator
    {
        /// <summary>
        /// 生成图片
        /// </summary>
        /// <param name="code"></param>
        /// <param name="codeLength"></param>
        /// <returns></returns>
        public byte[] GenerateImage(out string code, int codeLength = 4);

    }
}
