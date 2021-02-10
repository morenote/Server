using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreNote.Logic.Entity
{
    /// <summary>
    /// 解析策略
    /// </summary>
    [Table("resolution_strategy")]
    public  class ResolutionStrategy
    {
        [Key]
        [Column("strategy_id")]
        public long? StrategyID{ get;set;}//策略ID
        [Column("strategy_key")]
        public String StrategyKey{ get;set;}//策略授权密钥
        [Column("strategy_name")]
        public string StrategyName{ get;set;}//策略名称
        [Column("check_time")]
        public int CheckTime { get;set;}//健康检查时间
    }
}
