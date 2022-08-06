[中文](./README_Chinese.md)

[English](./README_English.md)

# MoreNote

> MoreNote is a .net software based on leanote.  
> Free. Cross-platform. Open source.  
> A developer platform for building all  apps.  
> But  Who cares?

Demo website ：<a href="https://dev.morenote.top/" target="_blank">Morenote  Cloud note service implemented in C#</a>

github:https://github.com/hyfree/MoreNote

gitee:https://gitee.com/hyfree_cn/MoreNote/






## What is MoreNote

![MoreNote.NET Core](https://github.com/hyfree/MoreNote/workflows/MoreNote.NET%20Core/badge.svg?event=push)

Morenote is a cross-platform note hosting service built using C#, which can run on Linux and Windows platforms. Morenote is developed based on leanote, provides API performance consistent with leanote, and supports leanote's note file format to the maximum. MoreNote provides more control options and better performance and safety performance👏



## What we want 

Build a trustworthy open source note hosting service 

## Warning before use 

> If you plan to use MoreNotet to store your notes, please be sure to read ：
> MoreNote is still under limited internal development. `MoreNote` recommends not to use or host your important notes in a production environment. It is very likely that all your notes will be lost in a crash. So, check your server, check the deployment path, and use it at your own risk. 

**feature**

* Data compatible ：Compatible with Leanote notes API, you can log in directly with Leanote's desktop software or mobile software [under development] 
* Backstage management ：More powerful control panel [plan] 
* data encryption：Allow the use of encryption algorithms to protect note data [plan]
* distributed ：Any client connected to MoreNote can get a complete copy based on the notes [plan]
* data backup：Allows to use any data source (WebDev, FTP, OSS, etc.) to back up notes, and use any data source to restore data [plan]
* Disaster recovery ：Use any complete copy or backup file to restore the note data [plan] 


## project status

The code is basically the leanote project code of fork, and the current overall project completion rate is less than 20%.

The goal of V 0.0.1 is to achieve most of the features of leanote 2.6.1. 

- Dev Version：It is a version under development. There are many bugs and performance problems. If you don't want your software to crash at any time, please don't choose this version.
- beta Version：IIt shows that this version is stable, bugs and problems are relatively few, but the software may crash and cause problems. If you do not know software development, please do not choose this version.
- Release Version：It shows that the program is good, and bugs and performance problems have been fixed or alleviated.

Release milestones ：


| version | Dev  | beta |Release|Compatible|Remark|
|  ----    | ----  |---- |---- | --- | --- |
| V 0.0.1  |进行中⏳ | NA😒 | NA😒 |leanote 2.6.1🤦‍♂️|no support： shared notes, does not support blog templates|



## Development schedule

[Task table](./Documents/Tasks.md)

## Difference with leanote

The main differences between MoreNote and leanote are as follows：

| difference | MoreNote | leanote |
| :----- | :----: | :----: |
| Development language | C#(.Net5) | Go |
| Database | PostgreSQL12 | MongoDB  |
| Performance | very slow | very fast |
| RAM | 2GB | -  |
| Custom theme | not support | support |
| customer service | not support | Community support |
| Multiple users | support | support |
| Full Text Search | support | support |
| Cache | not support | NoSQL  |
| message queue | not support | not support |




#### Software Architecture
Front-end framework： LeanoteUI、AmazeUI、JQuery

Backend framework：asp .net mvc  core

operating system： Ubuntu 18/20  or Centos7/8  or  Other operating systems

Database： PostgreSQL12

Cache ： Redis

#### Installation tutorial
```ssh
git clone URL
cd  Morenote
dotnet run
```

#### Instructions for use
 The main goal of MoreNote is to design a lightweight note hosting service. MoreNote uses high-performance .Net5 to develop a high-performance cross-platform cloud note hosting service.


#### Participate in contribution 

Main development： hyfree

#### Standing on the shoulders of giants
- .NetCore(Cross-platform development language produced by Microsoft)
- AmazeUI(Front-end UI framework)
- JQuery(JavaScript script library)
- Supervisor(Linux process daemon) 
- Redis(High-performance cache service)
- PostgreSQL(High-performance database)
- WangEditer(Web rich text editor)
- Textbus(Excellent rich text editor)
- Vditor(markdown editor)
- Masuit.LuceneEFCore.SearchEngine(Full-text search search engine based on EntityFrameworkCore and Lucene.NET)
- Masuit.MyBlogs(High-performance and low-occupancy blog system)
- Leanote(Cloud note service implemented in Go language)

