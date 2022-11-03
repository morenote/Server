[中文](./README_Chinese.md)

[English](./README_English.md)

# 赞助

![image-20220102113326000](README.assets/image-20220102113326000.png)

# MoreNote

> MoreNote是基于[leanote](https://github.com/leanote/leanote)的.net发行版  
> Free. Cross-platform. Open source.  
> A developer platform for building all  apps.  
> But  Who cares?

## MoreNote是什么

![MoreNote.NET Core](https://github.com/hyfree/MoreNote/workflows/MoreNote.NET%20Core/badge.svg?event=push)

MoreNote是使用C#开发的跨平台笔记托管服务，可以运行在云服务器，同时支持运行在资源受限的物联网设备上（比如树莓派4B、ROCK PI 4）。

morenote支持托管你的笔记、文档、文件、图片或者视频资料。

在树莓派4B上的演示网站：

<a href="https://dev.morenote.top/" target="_blank">dev版本：morenote云笔记</a>

稳定版本：暂无
## 项目目标

构建一个开源的可以值得信赖的开源笔记托管服务

## 使用前警告

> 如果您计划使用MoreNotet托管您的笔记，请您务必知晓：
> MoreNote仍处于有限的内部开发中，`MoreNote` 建议不要在生产环境中使用或托管您的重要笔记，您的笔记很有可能再某次崩溃中全部丢失。
> 所以，检查你的服务器，检查部署路径，并且使用风险自担。

## 特色（画饼）

- [ ] **分布式**：morenote是分布式版本控制系统，您的每个设备上都拥有一个完整的笔记库，中心服务器的崩溃不会影响你的笔记安全；
- [ ] **快速协作**：你可以选择离线工作，亦可以选择通过中心化服务器与其他人协同工作，这是您的自由，我们不会干涉；
- [ ] **同步**：你的笔记可以眨眼间在不同设备之间快速同步，不会丢失，不会泄露；
- [ ] **通用格式**：morenote优先使用markdown格式，你可以自由的使用第三方编辑器处理，这是您的自由，我们不会干涉；
- [ ] **开源软件**：morenote的代码是开源，所有代码是公开，您可以自由的审查软件中是否存在后门和安全风险；
- [ ] **端到端加密**：morenote提供端到端加密的支持，你的数据在传输和存储过程中始终保密；
- [ ] **笔记单独加密**：允许使用加密算法对笔记数据单独加密；
- [ ] **更多编辑器**：morenote适配了很多第三方开源的编辑器，你可以选择你喜欢的第三方编辑器；
- [ ] **本地化编辑**：使用msync下载笔记数据到本地，然后使用第三方编辑器修改，然后与服务器同步；
- [ ] **数据兼容**：与Leanote笔记的API兼容，可以直接使用leanote的桌面软件或移动端软件登录（不再受支持）
- [ ] **不同的风格**：更强大的托管后台，更简洁有力的布局，更清新简约的编辑界面（使用了ng-alain）；
- [ ] **API接口**：MoreNote提供开发的API接口，方便定制功能和插件；
- [ ] **插件主题**：MoreNote提供丰富的插件和主题供用户使用；
- [ ] **文件托管**：MoreNote就是你的私有云盘！！

## 项目状态

代码基本是fork的leanote的项目代码，目前整体项目完成度小于20%。

V 0.0.1的设计目标仅仅是是实现leanote 2.6.1的大部分功能。

- Dev版本：是开发中的版本，bug和性能问题非常多，如果你不想你的软件随时崩溃，请不要选择该版本。
- 测试版：说明该版本已经稳定，bug和问题是比较少的，但是软件可能会崩溃和出现问题。如果你不具有一定的软件问题解决能力，请不要选择该版本。
- 发布版：说明程序已经接近稳定，严重bug和性能问题已经得到修复或者缓解。
## 项目逻辑
morenote是统一的开源笔记托管服务，用于托管用户笔记。用户笔记可以借助morenote在各个平台之间流转，而不依赖于各个平台。

morenote支持多种开源web编辑器和webapi接口用于接收用户的笔记输入，存储在数据库中。

输入的结果自动触发各种自动话任务，比如静态网站生成、博客推送任务、社区推送任务、批处理任务、机器人、加载serverless脚本。

![image](https://user-images.githubusercontent.com/26597853/187007479-51889d5d-992c-4ca6-a635-70e946e977bb.png)


## 发行里程碑：

| 版本      | Dev版 | 测试版  | 发布版  | 兼容版本               | 备注              |
| ------- | ---- | ---- | ---- | ------------------ | --------------- |
| V 0.0.1 | 进行中⏳ | 没有😒 | 没有😒 | leanote 2.6.1🤦‍♂️ | 不支持共享笔记，不支持博客模板 |

## 开发进度表

[任务表](./Documents/Tasks.md)

## 与leanote的主要区别

MoreNote与leanote的主要区别如下：

| 区别      | MoreNote                                           | leanote     |
|:------- |:--------------------------------------------------:|:-----------:|
| 开发语言    | C#                                                 | Go          |
| 数据库     | PostgreSQL                                         | MongoDB     |
| 性能      | 快                                                  | 非常快         |
| 内存      | 最少2GB，建议4GB                                        | 最少2GB       |
| 主题      | 不支持（开发中）                                           | 博客主题        |
| 支持      | 社区支持                                               | 社区支持&QQ群    |
| 多租户     | 支持                                                 | 支持          |
| 全文检索    | 笔记标题√笔记内容√附件标题×（开发中）                               | 支持          |
| 实时协作    | 不支持（开发中）                                           | 不支持         |
| 笔记历史    | 不支持（开发中）                                           | 支持          |
| 缓存加速    | 内存缓存或Redis                                         | 不支持         |
| 编辑器     | ace、tinymce、vditor、textbus                         | ace、tinymce |
| 文件管理器   | 支持                                                 | 不支持         |
| 文件传输协议  | 不支持(开发中：WebDAV、SAMBA、FTP、SFTP)                     | 不支持         |
| 端到端加密   | 不支持（开发中）                                           | 支持          |
| 两步验证    | 支持安全令牌登录（开发中：FIDO2&  人脸登录& Google  Authentication） | 不支持         |
| 反垃圾评论   | 基于机器学习自动识别                                         | 不支持         |
| 跨平台     | 支持                                                 | 支持          |
| 开放API接口 | 不支持（开发中）                                           | 支持          |
| 笔记备份功能  | 不支持（开发中）                                           | 不支持         |
| 笔记灾难恢复  | 不支持（开发中）                                           | 不支持         |
| 笔记导出功能  | msync(开发中)                                         | 支持          |

#### 软件架构

前端框架： ng-alain

后端框架：asp .net  core

服务器端： ubuntu Server 或其他linux操作系统

数据库端： PostgreSQL11或12

缓存服务： Redis【开发中】  

消息队列：RabbitMQ【开发中】

全文检索：Elasticsearch【开发中】、Lucene.Net【开发中】、postgreql【已经就绪】

对象存储：MINIO

#### 安装教程

目前仅支持CentOS7

```ssh
git clone URL
cd  NickelProject
dotnet run
```

#### 参与贡献

主要开发： hyfree

#### 站在巨人的肩上

- .Net(微软出品的跨平台开发语言) 编程语言
- ng-zorro-antd(前端UI框架) MIT开源许可
- Ant Design(前端UI框架)  MIT开源许可
- ng-alain（前端UI框架）[MIT开源许可](https://github.com/ng-alain/ng-alain/blob/master/LICENSE)
- JQuery(JavaScript脚本库)  MIT开源许可
- Supervisor(Linux进程守护程序)  基础组件
- Redis(高性能缓存服务) 基础组件
- PostgreSQL(高性能数据库) 基础组件
- WangEditer(Web富文本编辑器)  [MIT开源许可](https://github.com/wangeditor-team/wangEditor/blob/master/LICENSE)
- Textbus(优秀的富文本编辑器)  [GPL开源许可](https://github.com/textbus/textbus/blob/main/LICENSE)
- Vditor(markdown编辑器)  [MIT开源许可](https://github.com/Vanessa219/vditor/blob/master/LICENSE)
- Masuit.LuceneEFCore.SearchEngine(基于EntityFrameworkCore和Lucene.NET实现的全文检索搜索引擎)  [MIT开源许可](https://github.com/ldqk/Masuit.LuceneEFCore.SearchEngine/blob/master/LICENSE)
- Masuit.MyBlogs(高性能低占用的博客系统)  [MIT开源许可](https://github.com/ldqk/Masuit.MyBlogs)
- Leanote(Go语言实现的云笔记服务) [GPL开源许可](https://github.com/leanote/leanote)
- minio（分布式对象存储服务）[组件使用](http://www.minio.org.cn/)
- MrDoc（在线文档库）[GPL开源许可](https://github.com/zmister2016/MrDoc/blob/master/LICENSE)
- MxsDoc(Web文件管理系统) [GPL开源许可](https://gitee.com/RainyGao/DocSys/blob/master/LICENSE)
- joplin（开源笔记软件） [MIT开源许可](https://github.com/laurent22/joplin/blob/dev/LICENSE)
- AutumnBox [GPL开源协议](https://github.com/zsh2401/AutumnBox/blob/master/LICENSE)



#### 向他们的作品致敬

>   如果你觉得morenote不够完美，那么我建议你去尝试以下笔记软件或服务
> 
>   morenote在开发过程广泛的借鉴了其他比较流行的笔记软件的风格和设计

- Notion
- Wolai
- Appflowy
- 为知笔记
- 蚂蚁笔记
- 印象笔记
- 思源笔记
- 语雀
- 飞书
- joplin
- roamedit
- GoodNotes
- simplenote
- onenote
- 树莓盘
- NGX File Cloud 网盘 & NAS
- 可道云
- LESLIE NOTE 
- 有道云笔记
- 专注笔记
- 幕布
- 无忧·企业文档
- DOKUWIKI
