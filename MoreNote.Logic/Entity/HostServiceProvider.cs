using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreNote.Logic.Entity
{
    /// <summary>
    /// 主机服务
    /// </summary>
    public class HostServiceProvider
    {
        [Key]
        public long HostServiceProviderId {get;set; }
        public string HostName { get;set;}//主机品牌名
        public long ServiceProviderCompanyId { get;set;}//主机服务商
        public string ServiceType { get;set;}//服务类型
        public string ISP { get;set;}//网信部备案号
        public string BeiAnGov { get;set;}//公安联网备案
        public string WebSite { get; set; }//网址
        public string[] OldWebSite { get; set; }//曾用网址
        public string RegistrationPlace { get; set; }//注册地
        public DateTime FoundDate { get; set; }//首次营业时间
        public bool IsBlock { get; set; }//是否拉黑
        public int RiskIndex { get; set; }//风险指数
        public int MentionByName { get; set; }//指名度
        public bool AnomalyDetection { get; set; }//异常检测
    }
    /// <summary>
    /// 主机服务商
    /// </summary>
    public class ServiceProviderCompany
    {
        [Key]
        public  long ServiceProviderCompanyId { get;set;}
        public string SPName { get;set;}//服务商名称
        public long LegalPersonId { get;set;}//公司法人
        public DateTime RegionDate { get;set;}//注册时间
        public string WebSite { get;set;}//网址
        public string[] OldWebSite { get;set;}//曾用网址
        public string RegistrationPlace { get;set;}//注册地
        public DateTime FoundDate { get;set;}//首次营业时间
        public bool IsBlock { get;set;}//是否拉黑
        public int RiskIndex { get;set;}//风险指数
        public int MentionByName { get;set;}//指名度
        public bool AnomalyDetection { get;set;}//异常检测

    }
   
    /// <summary>
    /// 服务商法人代表
    /// </summary>
    public class ServiceProviderLegalPerson
    {
        [Key]
        public  long PersonId { get;set;}//法人ID
        public  string Name { get;set;}//法人名称
        public string About { get;set;}//关于
    }
    /// <summary>
    /// 报告人
    /// </summary>
    public class Reporter
    {
        [Key]
        public long ReporterId { get;set;}
        public string Name { get;set;}
        public string WebSite { get;set;}//报告人拥有的
        public bool IsIdentify { get;set; }//是否鉴别

    }
    public class SecretReport
    {
        [Key]
        public long SecretReportId { get;set;}//报告ID
        public long hostServiceProviderId{get;set;}//主机服务
        public long serviceProviderCompanyId { get;set;}//服务商
        public long ReporterId { get;set;}//报告人
        public  bool IsRisk { get;set;}//是否是风险报告
        public  string ReportContent { get;set;}//报告内容
    }
}
