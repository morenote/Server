

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

morenote是使用.Net构建的跨平台笔记托管服务，可以运行在Linux和window平台。morenote是基于leanote开发的，提供与leanote一致的API表现，并且最大限度的支持leanote的笔记文件的编码格式。相对而言，MoreNote提供了更多的控制选项和更差劲的性能和安全表现👏。

## 项目目标

构建一个开源的可以值得信赖的开源笔记托管服务

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

代码基本是fork的leanote的项目代码，目前整体项目完成度小于20%。

V 0.0.1的设计目标仅仅是是实现leanote 2.6.1的大部分功能。

- Dev版本：是开发中的版本，bug和性能问题非常多，如果你不想你的软件随时崩溃，请不要选择该版本。
- 测试版：说明该版本已经稳定，bug和问题是比较少的，但是软件可能会崩溃和出现问题。如果你不具有一定的软件问题解决能力，请不要选择该版本。
- 发布版：说明程序已经接近稳定，严重bug和性能问题已经得到修复或者缓解。

发行里程碑：


|  版本   | Dev版  | 测试版|发布版|兼容版本|备注|
|  ----    | ----  |---- |---- | --- | --- |
| V 0.0.1  |进行中⏳ |  没有😒|   没有😒|leanote 2.6.1🤦‍♂️|不支持共享笔记，不支持博客模板|



## 开发进度表

[任务表](./Documents/Tasks.md)

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

