using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreNote.Logic.Entity
{
    /// <summary>
    /// 解析线路
    /// </summary>
    public class ResolutionLocation
    {
        [Key]
         public long  ResolutionLocationID{get;set;}//解析线路ID
         public long  StrategyID{get;set;}//解析线路ID
         public string  URL{get;set;}//线路地址
         public int Score{get;set;}//线路评分
         public int  Weight{get;set;}//分配权重 0~5
         public int  Speed{get;set;}//访问速度 0~5
    }
}
