using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NickelProject.Logic.Entity
{
    public class UserEntity
    {
        [Key]//主键 
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]  //设置自增
        public string Userid { get; set; }
        public string PassWord { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        /// <summary>
        /// 用户组
        /// </summary>
        public string UserGroup { get; set; }
        /// <summary>
        /// 备注 用户自签名
        /// </summary>
        public string Intro { get; set; }
        public string Address { get; set; }
        public bool activity { get; set; }//是否激活
        /// <summary>
        /// 用户token授权
        /// </summary>
        public string Token { get; set; }
        public string Telephone { get; set; }
        public string QQ { get; set; }
        public string Twitter { get; set; }
        public string Avatar { get; set; }
        public int Rank { get; set; }
        public int Credit { get; set; }
        public int Gb { get; set; }

    }
}
