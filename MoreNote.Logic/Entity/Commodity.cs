using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreNote.Logic.Entity
{
    /// <summary>
    /// 商品
    /// </summary>
    [Table("commodity")]
    public class Commodity
    {
        [Key]
        [Column("commodity_id")]
        public long CommodityId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [Column("commodity_name")]
        public string CommodityName { get; set; }
        /// <summary>
        /// 商品价格 单位分
        /// </summary>
        [Column("total_fee")]
        public string TotalFee { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        [Column("repertory")]
        public int Repertory { get; set; }
        /// <summary>
        /// 商品类型
        /// </summary>
        [Column("commodity_type")]
        public int CommodityType { get; set; }



    }
}
