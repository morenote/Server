namespace MoreNote.Logic.Entity
{
    /// <summary>
    /// 文件储存方式
    /// 这个值决定了执行文件操作
    /// </summary>
    public enum StorageTypeEnum
    {

        
        /// <summary>
        /// 本地磁盘
        /// </summary>
        LocalDisk = 100,
        /// <summary>
        /// 又拍云 对象储存
        /// </summary>
        UpYunOSS=200,
        /// <summary>
        /// 阿里云对象储存
        /// </summary>
        ALiYunOSS=201,
        /// <summary>
        /// 优刻得对象储存
        /// </summary>
        UcloudOSS=202,
        /// <summary>
        /// 七牛云对象储存
        /// </summary>
        QiNiuOSS=203,
        /// <summary>
        /// 华为云对象储存
        /// </summary>
        HuaWeiOSS=204,
        /// <summary>
        /// FTP服务
        /// </summary>
        FTP=300,
        /// <summary>
        /// WebDAV服务
        /// </summary>
        WebDAV=400,
    }
}