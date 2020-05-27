using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreNote.Logic.Entity
{
    /// <summary>
    /// 商品
    /// </summary>
    public class Good
    {
        [Key]
        public long GoodId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodName { get; set; }
        /// <summary>
        /// 商品价格 单位分
        /// </summary>
        public string TotalFee { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public int Repertory { get; set; }
        /// <summary>
        /// 商品类型
        /// </summary>
        public int GoodType { get; set; }



    }
}
