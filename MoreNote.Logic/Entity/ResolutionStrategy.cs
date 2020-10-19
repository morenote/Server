using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreNote.Logic.Entity
{
    /// <summary>
    /// 解析策略
    /// </summary>
    public  class ResolutionStrategy
    {
        [Key]
        public long StrategyID{ get;set;}//策略ID
         public String StrategyKey{ get;set;}//策略授权密钥
        public string StrategyName{ get;set;}//策略名称
        public int CheckTime { get;set;}//健康检查时间
    }
}
