```json
{
  "IsAlreadyInstalled": true,
  "MachineLearning": {
    "CanMachineLearning": false,
    "SpamModelPath": "垃圾评论的机器学习模型文件的存放地址"
  },
  "Payjs": {
    "PayCode": 1,
    "PayJS_MCHID": "XXXXXX",
    "PayJS_Key": "XXXXXX"
  },
  "PostgreSql": {
    "Connection": "Host=localhost;Port=XXXXX;Database=XXXX; User ID=XXXX;Password=XXXXX;"
  },
  "PublicAPI": {
    "CanRandomImageAPI": true,
    "CanTokenAntiTheftChain": true,
    "RandomImageFuseSize": 2000,
    "RandomImageSize": 20,
    "UpdateTime": 60
  },
  "Spiders": {
    "CanCrawlerWorker": false,
    "Reptile_Delay_Second": 30
  },
  "UpyunConfig": null,
  "SecurityConfig": {
    "Secret": "Base64格式的随机密钥",
    "OpenRegister": false,
    "OpenDemo": false,
    "ShareYourData": false,
    "AdminUsername": "admin",
    "DemoUsername": "demo",
    "LogFolder": "",
    "SessionExpires": 10,
    "MaintenanceMode": false,
    "PasswordHashAlgorithm": "argon2",
    "PasswordHashIterations": 8,
    "PasswordStoreMemorySize": 2048,
    "NeedVerificationCode": "AUTO",
    "FIDO2Config": {
      "ServerDomain": "localhost",
      "ServerName": "localhost",
      "Origin": "https://localhost:5001",
      "TimestampDriftTolerance": 300000
  }
  },
  "APPConfig": {
    "APPName": "Leanote",
    "SiteUrl": "https://www.morenote.top",
    "BlogUrl": "/blog",
    "LeaUrl": "",
    "NoteUrl": "/note/note",
    "SearchUrl": "/",
    "DB": "postgresql",
    "Dev": true,
    "FileFolder": null
  },
  "GlobalConfig": {
    "DemonstrationOnly": true,
    "StorageTypeEnum": 0
  },
  "FileStoreConfig": {
    "MainFolder": "/morenote",
    "FileStorage": "minio",
    "BrowserDownloadExpiresInt": 3600,
    "BrowserUploadExpiresInt": 3600,
    "UploadAvatarMaxSizeMB": 20,
    "UploadBlogLogoMaxSizeMB": 20,
    "UploadImageMaxSizeMB": 20,
    "UploadAttachMaxSizeMB": 500,
    "TempFolder": "/var/tmp"
  },
  "MinIOConfig": {
    "Endpoint": "127.0.0.1:9002",
    "WithSSL": false,
    "CDNEndpoint": "minio.morenote.top",
    "CDNWithSSL": true,
    "NoteFileBucketName": "morenote",
    "RandomImagesBucketName": "random-images",
    "MINIO_ACCESS_KEY": "你的minio-access",
    "MINIO_SECRET_KEY": "你的minio-key",
    "BrowserDownloadExpiresInt": 3600
  },
  "RedisConfig": {
    "IsEnable": false,
    "Configuration": "localhost",
    "InstanceName": "RedisDistributedCache"
}
}


```



具体含义请查阅一下链接地址：

[Server/Src/MoreNote.Config](https://github.com/morenote/Server/tree/master/Src/MoreNote.Config/ConfigFile)


