# 软件部署说明



## 安装依赖环境

### 安装dotnet

根据当前的操作系统和电脑环境现在dotnet版本

[Download .NET 6.0 (Linux, macOS, and Windows) (microsoft.com)](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

```
mkdir -p $HOME/dotnet && tar zxf dotnet-sdk-6.0.200-linux-x64.tar.gz -C $HOME/dotnet
export DOTNET_ROOT=$HOME/dotnet
export PATH=$PATH:$HOME/dotnet
```

### 安装数据库

```
docker run -it --name postgres --restart always -e POSTGRES_PASSWORD='your-password' -e ALLOW_IP_RANGE=0.0.0.0/0 -v /home/postgres/data:/var/lib/postgresql -p 55433:5432 -d postgres
```
### 迁移数据库

```
dotnet ef migrations add InitialCreate  --project E:\github\hyfree\Morenote\Server\Src\MoreNote.Logic //替换为你的路径

dotnet ef database update
```


### 安装Redis（可选）

```
docker pull redis:latest
docker run -itd --name redis -p 6379:6379 redis
```

### 安装MINIO（可选）

http://docs.minio.org.cn/docs/master/minio-docker-quickstart-guide

```shell
docker run  -p 9002:9000 --name minio \
  -v /your-path/data:/data \
  -v /your-path/config:/root/.minio \
  -e "MINIO_ACCESS_KEY=username" \
  -e "MINIO_SECRET_KEY=password" \
  -d \
  --restart=always \
  minio/minio server /data
```



### 安装MQ（可选）

```
docker run -d --name rabbitmq -p 5671:5671 -p 5672:5672 -p 4369:4369 -p 25672:25672 -p 15671:15671 -p 15672:15672 rabbitmq:management

docker update rabbitmq --restart=always
```







