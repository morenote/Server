namespace MoreNote.Logic.Entity.ConfigFile
{
    public class WebSiteConfig
    {
        public bool IsAlreadyInstalled { get; set; }
        public MachineLearningConfig MachineLearning { get; set; }
        public PayJSConfig Payjs { get; set; }
        public PostgreSqlConfig PostgreSql { get; set; }
        public RandomImangeServiceConfig PublicAPI { get; set; }
        public ImageSpidersConfig Spiders { get; set; }
        public UpyunConfig UpyunConfig { get; set; }
     

        public SecurityConfig SecurityConfig{get;set;}=new SecurityConfig();
        public APPConfig APPConfig{get;set;}=new APPConfig();
        public GlobalConfig GlobalConfig{get;set;}=new GlobalConfig();
        public FileStoreConfig FileStoreConfig { get;set;}=new FileStoreConfig();
        public MinIOConfig MinIOConfig { get;set;}=new MinIOConfig();
        public WebSiteConfig()
        {


        }
        public static WebSiteConfig GenerateTemplate()
        {
            WebSiteConfig webSiteConfig=new WebSiteConfig()
            {
                IsAlreadyInstalled=false,
            };
            return webSiteConfig;
        }

    }
}