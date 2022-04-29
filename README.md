[中文](./README_Chinese.md)

[English](./README_English.md)

# 赞助



![image-20220102113326000](README.assets/image-20220102113326000.png)



# MoreNote

> MoreNote是基于[leanote](https://github.com/leanote/leanote)的.net发行版  
> Free. Cross-platform. Open source.  
> A developer platform for building all  apps.  
> But  Who cares?

在树莓派上的演示网站：<a href="https://www.morenote.top/" target="_blank">morenote云笔记</a>

github:https://github.com/hyfree/MoreNote

gitee:https://gitee.com/hyfree_cn/MoreNote/


## MoreNote是什么

![MoreNote.NET Core](https://github.com/hyfree/MoreNote/workflows/MoreNote.NET%20Core/badge.svg?event=push)

morenote是使用.net构建的跨平台笔记托管服务，可以运行在Linux和window平台。

morenote是基于leanote的代码二次开发的，提供与leanote一致的API表现，并且最大限度的支持leanote的笔记文件的编码格式。

相对而言，MoreNote提供了更多的控制选项和更差劲的性能和安全表现👏。

> 重大变更：morenote的api可能与leanote的不再保持兼容，为了提供团队协作功能，在api上会有较大程度的变更。morenote不再保证与leanote完全兼容。

## 使用前警告

> 如果您计划使用MoreNotet托管您的笔记，请您务必知晓：
> MoreNote仍处于有限的内部开发中，`MoreNote` 建议不要在生产环境中使用或托管您的重要笔记，您的笔记很有可能再某次崩溃中全部丢失。
> 所以，检查你的服务器，检查部署路径，并且使用风险自担。

## 画饼

构建一个开源的可以值得信赖的开源笔记托管服务

-   [ ] **分布式**：morenote是分布式，您的每个设备上都拥有一个完整的笔记库，中心服务器的崩溃不会影响你的笔记安全。
-   [ ] **协作**：你可以选择离线工作，亦可以选择通过服务器与其他人协同工作。
-   [ ] **同步**：你的笔记可以眨眼间在不同设备之间快速同步。
-   [ ] **通用格式**：morenote优先使用markdown格式化的文档，你可以自由的使用第三方软件编辑。
-   [ ] **开源软件**：morenote是开源软件，所有代码公开，不存在后门和隐私窃取
-   [ ] **端到端加密**：morenote提供端到端加密的选项，即使是服务器的管理员也无法偷窥用户的数据
-   [ ] **笔记单独加密**：允许使用加密算法对笔记数据单独加密
-   [ ] **更多编辑器**：morenote适配了很多第三方开源的编辑器，你可以自由的选择你喜欢的
-   [ ] **本地化编辑**：使用msync下载笔记数据到本地，然后使用第三方编辑器修改，最后保存笔记并且快速同步
-   [ ] **数据兼容**：与Leanote笔记的API兼容，可以直接使用leanote的桌面软件或移动端软件登录（不再受支持）
-   [ ] **不同的风格**：更强大的托管后台，更简洁有力的布局，更清新简约的编辑界面
-   [ ] **API接口**：morenote提供API接口，方便定制功能和插件
-   [ ] **插件主题**：morenote提供丰富的插件和主题供用户使用


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

## 与leanote的主要区别

MoreNote与leanote的主要区别如下：

| 区别| MoreNote | leanote |
| :----- | :----: | :----: |
| 开发语言 | C# | Go |
| 数据库 | PostgreSQL | MongoDB  |
| 性能 | 非常慢 | 非常快  |
| 内存 |  1GB | 35MB+78MB |
| 主题 | 不支持（开发中） | 博客主题 |
| 支持 | 社区支持 | 社区支持&QQ群 |
| 多租户 | 支持 | 支持  |
| 全文检索 | 笔记标题√笔记内容√附件标题× | 支持  |
| 实时协作 | 不支持 | 不支持 |
| 笔记历史 | 不支持（开发中） | 支持 |
| 缓存加速 | 内存或Redis | MongoDB |
| 编辑器 | ace、tinymce、vditor、textbus | ace、tinymce |
| 文件管理器 | 支持 | 不支持 |
| WebDAV | 不支持 | 不支持 |
| 端到端加密 | 不支持 | 支持 |
| 两步验证（开发中） | FIDO2&  人脸登录& Google  Authentication | 不支持 |
| 反垃圾评论 | 基于机器学习自动识别 | 不支持 |
| 跨平台 | 支持 | 支持 |
| 开放API接口 | 不支持（开发中） | 不支持 |
| 笔记备份功能 | 不支持（开发中） | 不支持 |
| 笔记灾难恢复 | 不支持（开发中） | 不支持 |
| 笔记导出功能 | msync(开发中) | 支持 |




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

- .Net(微软出品的跨平台开发语言) 编程语言
- ng-zorro-antd(前端UI框架) MIT开源许可
- Ant Design(前端UI框架)  MIT开源许可
- JQuery(JavaScript脚本库)  MIT开源许可
- Supervisor(Linux进程守护程序)  基础组件
- Redis(高性能缓存服务) 基础组件
- PostgreSQL(高性能数据库) 基础组件
- WangEditer(Web富文本编辑器)  MIT开源许可
- Textbus(优秀的富文本编辑器)  GPL开源许可
- Vditor(markdown编辑器)  MIT开源许可
- Masuit.LuceneEFCore.SearchEngine(基于EntityFrameworkCore和Lucene.NET实现的全文检索搜索引擎)  MIT开源许可
- Masuit.MyBlogs(高性能低占用的博客系统)  MIT开源许可
- Leanote(Go语言实现的云笔记服务) GPL开源许可
- minio（分布式对象存储服务）组件使用
- MrDoc（在线文档库）GPL开源许可
- MxsDoc(Web文件管理系统) GPL开源许可
- joplin（开源笔记软件） [MIT开源许可](https://github.com/laurent22/joplin/blob/dev/LICENSE)

####  向他们的作品致敬

>   如果你觉得morenote不够完美，那么我建议你去尝试以下笔记软件或服务
>
>   morenote在开发过程广泛的借鉴了其他比较流行的笔记软件的风格和设计

-   Notion
-   Wolai
-   Appflowy
-   为知笔记
-   蚂蚁笔记
-   印象笔记
-   思源笔记
-   语雀
-   飞书
-   joplin
-   roamedit
-   GoodNotes
-   simplenote
-   onenote
-   树莓盘
-   NGX File Cloud 网盘 & NAS
-   可道云
-   LESLIE NOTE 
-   有道云笔记
-   专注笔记
-   幕布
-   无忧·企业文档
-   DOKUWIKI

