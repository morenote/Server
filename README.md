# MoreNote

MoreNote是基于leanote的.net core发行版





## 项目状态


|  版本   | Dev版  | 测试版|发布版
|  ----    | ----  |---- |---- |
| V 0.0.1  |进行中 |   没有|   没有|


## 区别

MoreNote与leanote的主要区别如下：

| 区别| MoreNote | leanote |
| :----- | :----: | :----: |
| 开发语言 | C#(.net core 3.1) | Go |
| 数据库 | PostGreSQL | MongoDB  |
| 性能 | 非常慢 | 非常快  |
| 内存占有 | 370M/1.8G | 应该比我少😆  |
| 主题 | 不支持 | 支持主题包安装  |
| 支持 | 不支持 | 社区支持&付费版支持  |
| 多用户 | 不支持 | 支持  |
| 全文检索 | 不支持 | 支持  |
| 缓存 | 不支持 | NoSQL  |

## 介绍

使用.net Core搭建笔记托管服务，理论上应该比Go版的要慢一点，并且占有更多的内存。

当前是根据[leanote](https://github.com/leanote/leanote)的5e61291703a81514ef271ca01a379d151778895f提交作为参考的.net core发行版

当前正在开发的0.0.1版本对应的版本是leanote 2.6.1。

原则上提供与leanote一致的API表现，并且原则上最大限度的支持leanote的笔记文件的编码格式。

相对而言，MoreNote提供了更多的控制选项和更差劲的性能表现👏，MoreNote可以占有更多内存和CPU避免服务器处于长期闲置状态。
同时MoreNote有更多的编码漏洞和权限漏洞，所以当前版本仅供学习📚用途。

**特性**

* 兼容：与Leanote笔记的API兼容，可以直接使用leanote的桌面软件或移动端软件登录【开发中】
* 管理：更强大的托管后台【计划】
* 保密：允许使用密码加密笔记或者限制访问【计划】


#### 软件架构
前端框架： LeanoteUI、AmazeUI、JQuery

后端框架：asp .net mvc core 3.1

服务器端： Centos7、Ubuntu18（原则上是支持dockers的）

数据库端： PostgreSQL11或12

缓存服务： Redis【可选】  

分布式节点：暂无

#### 安装教程

```ssh
git clone URL
cd  NickelProject
dotnet run
```

#### 使用说明
 MoreNote的主要设计目标是轻量型的笔记托管服务，MoreNote使用高性能的dotnet core设计(然并卵😂)。


#### 参与贡献

主要开发： hyfree

#### 特别感谢
- .NetCore(微软出品的C#)
- AmazeUI(前端UI框架)
- JQuery(JavaScript脚本库)
- Supervisor(Linux进程守护程序) 
- Redis(高性能缓存服务)
- PostgreSQL(高性能数据库)
- WangEditer(Web富文本编辑器)
- Leanote(笔记服务)

