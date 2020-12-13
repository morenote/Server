namespace MoreNote.Logic.Entity.ConfigFile
{
    public class WebSiteConfig
    {
        public bool IsAlreadyInstalled { get; set; }
        public MachineLearningConfig MachineLearning { get; set; }
        public PayJSConfig Payjs { get; set; }
        public PostGreSqlConfig PostgreSql { get; set; }
        public PublicAPIConfig PublicAPI { get; set; }
        public SpidersConfig Spiders { get; set; }
        public UpYunCDNConfig UpYunCDN { get; set; }
        public UpYunOSSConfig UpYunOSS { get; set; }
        public WebSiteConfig()
        {

        }
    }
}