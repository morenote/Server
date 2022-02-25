using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Security.Face
{
    [Table("face_sample"), Index(nameof(FaceSampleId), IsUnique = true)]
    public class FaceSample
    {
        /// <summary>
        /// 存档的人脸样本
        /// </summary>
        [Key]
        [Column("face_sample_id")]
        public long? FaceSampleId { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        [Column("user_id")]
        public long? UserId { get; set; }

        /// <summary>
        /// 人脸样本存储地址
        /// </summary>
        [Column("face_image_path")]
        public string FaceImagePath { get; set; }




    }
}
