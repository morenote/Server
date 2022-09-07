using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreNote.Logic.Entity
{
    /// <summary>
    /// 解析线路
    /// </summary>
    [Table("resolution_location")]
    public class ResolutionLocation: BaseEntity
    {
        
        [Column("strategy_id")]
        public long?  StrategyID{get;set;}//解析线路ID
        [Column("url")]
        public string  URL{get;set;}//线路地址
        [Column("score")]
        public int Score{get;set;}//线路评分
        [Column("weight")]
        public int  Weight{get;set;}//分配权重 0~5
        [Column("speed")]
        public int  Speed{get;set;}//访问速度 0~5
    }
}
