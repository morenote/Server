# MoreNote

> MoreNote是基于[leanote](https://github.com/leanote/leanote)的.net发行版  
> Free. Cross-platform. Open source.  
> A developer platform for building all  apps.  
> But  Who cares?

演示网站：<a href="https://www.morenote.top/" target="_blank">morenote云笔记</a>

github:https://github.com/hyfree/MoreNote

gitee:https://gitee.com/hyfree_cn/MoreNote/

 
## 概述

![MoreNote.NET Core](https://github.com/hyfree/MoreNote/workflows/MoreNote.NET%20Core/badge.svg?event=push)

使用.net构建云笔记托管服务，并运行在Linux上。


原则上尽量提供与leanote一致的API表现，并且最大限度的支持leanote的笔记文件的编码格式。

相对而言，MoreNote提供了更多的控制选项和更差劲的性能和安全表现👏。

## 项目目标

构建一个开源的可以值得信赖的开源笔记托管系统

## 使用前警告

> 如果您计划使用MoreNotet托管您的笔记，请您务必知晓：
> MoreNote仍处于有限的内部开发中，`MoreNote` 建议不要在生产环境中使用或托管您的重要笔记，您的笔记很有可能再某次崩溃中全部丢失。
> 所以，检查你的服务器，检查部署路径，并且使用风险自担。

**特性**

* 数据兼容：与Leanote笔记的API兼容，可以直接使用leanote的桌面软件或移动端软件登录【开发中】
* 后台管理：更强大的托管后台【计划】
* 数据加密：允许使用加密算法保护笔记数据【计划】
* 分布式：任意一个客户端连接到MoreNote均可以获得基于笔记的一个完整拷贝【计划】
* 数据备份：允许使用任意数据源（WebDev、FTP、OSS等）备份笔记，并使用任意数据源恢复数据【计划】
* 灾难恢复：使用任意一个完整拷贝或备份文件均可以恢复笔记数据【计划】


## 项目状态

代码基本是fork的leanote的项目代码，目前整体项目完成度小于5%。

目前已经copy了关于leanote桌面端API的相关代码。

V 0.0.1的设计目标仅仅是是实现leanote 2.6.1的相关功能，不考虑是否完全OK。


|  版本   | Dev版  | 测试版|发布版|兼容版本|
|  ----    | ----  |---- |---- | --- |
| V 0.0.1  |进行中⏳ |  没有😒|   没有😒|leanote 2.6.1🤦‍♂️|

## 开发进度

- [x] 当前版本已经兼容 leanote的桌面端的部分API
- [ ] 当前版本已经兼容 leanote的移动端
- [ ] 当前版本支持网页编辑器
## 最近计划表
- [ ] 针对于博客图片的格式化处理，使图片URL对外输出的时候伪静态，以便CDN可以更好的发挥作用
- [ ] 数据全量备份、数据增量备份、数据取回、数据导出、数据迁移
##  开发备忘录

- [ ] 语雀的前端编辑器真好用，如果leanote的前端编辑器（桌面端和Web端）能够升级一下就好了
- [ ] leanote的Android客户端需要得到维护，贼难用
- [ ] leanote缺少一个可用的谷歌浏览器插件，无法剪辑网页
- [ ] 参考语雀，leanote的桌面端需要得到维护
- [ ] 数据储存服务：支持FTP、对象储存（阿里云、腾讯云、华为云、又拍云、七牛云等）、WebDVA等协议
- [ ] 软件部署文档、使用教程需要得到维护（重要！！）
- [ ] Docker First：所有特性优先支持Docker
- [ ] 支持Mysql和mariadb数据库
- [ ] 基于C#/WPF实现一个新的桌面端和新的移动端APP🎄

## 与leanote的区别

MoreNote与leanote的主要区别如下：

| 区别| MoreNote | leanote |
| :----- | :----: | :----: |
| 开发语言 | C#(.Net5) | Go |
| 数据库 | PostgreSQL | MongoDB  |
| 性能 | 非常慢 | 非常快  |
| 内存占有 |  至少1GB可用内存 | 应该比我少😆  |
| 主题 | 不支持 | 支持主题包安装  |
| 支持 | 不支持 | 社区支持&付费版支持  |
| 多用户 | 不支持 | 支持  |
| 全文检索 | 不支持 | 支持  |
| 缓存 | 不支持 | NoSQL  |




#### 软件架构
前端框架： LeanoteUI、AmazeUI、JQuery

后端框架：asp .net mvc 5

服务器端： Centos7（原则上是支持dockers的）

数据库端： PostgreSQL11或12

缓存服务： Redis【可选】  

分布式节点：暂无

#### 安装教程
目前仅支持CentOS7
```ssh
git clone URL
cd  NickelProject
dotnet run
```

#### 使用说明
 MoreNote的主要设计目标是轻量型的笔记托管服务，MoreNote使用高性能的.Net5设计低性能的跨平台云笔记托管服务(然并卵)。


#### 参与贡献

主要开发： hyfree

#### 站在巨人的肩上
- .NetCore(微软出品的跨平台开发语言)
- AmazeUI(前端UI框架)
- JQuery(JavaScript脚本库)
- Supervisor(Linux进程守护程序) 
- Redis(高性能缓存服务)
- PostgreSQL(高性能数据库)
- WangEditer(Web富文本编辑器)
- Textbus(优秀的富文本编辑器)
- Vditor(markdown编辑器)
- Masuit.LuceneEFCore.SearchEngine(基于EntityFrameworkCore和Lucene.NET实现的全文检索搜索引擎)
- Masuit.MyBlogs(高性能低占用的博客系统)
- Leanote(Go语言实现的云笔记服务)

