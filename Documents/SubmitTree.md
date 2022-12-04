# 概述

morenote参考git提供Tree机制实现文件的同步，客户端提交数据前应该检查自己的树是否与服务端的树是否相同。

## 基本概念

### 提交树 SubmitTree

服务端每个笔记仓库都会拥有一个SubmitTree，类似于区块链，SubmitTree由若干SubmitBlock链接形成。

### 提交块 SubmitBlock

客户端可以将若干操作打包成一个SubmitBlock。

```
string Id  //SubmitBlockId
int Version//默认为1
string UserId//发起用户
string Date//操作日期
long Height//高度
string PreBlockHash//前块哈希
string SubmitHash//提交哈希（本块哈希）
string BlockHash//区块哈希
```

### 操作（Operation）

 每个Operation意味着一次有效操作，比创建一个资源、更新一个资源、删除一个资源。MoreNote通过Operation记录实现对历史数据的记录

```
string id  //OperationId
string SubmitId//提交ID
string Method//操作
string Target//目标
string Attributes//属性
string DataIndex//数据索引
string DateHash//数据哈希
string OperationHash//操作哈希
```

### 压缩（Compression）

将多个重复的操作压缩成一个，防止SubmitTree变得非常高，同时压缩空间提高数据库的效率。

### Method

- Post //创建资源
- Put //更新资源
- PATCH//修补资源
- DELETE//删除资源

### Target

修改对象id

### Attributes

- note
  
  - title

- notebook
  
  - title

### Data

数据  

## 交互流程

1. 客户端将本地操作打包成1个或多个SubmitBlock

2. 客户端请求SubmitTree的最新高度和最新区块

3. 如果高度相同并且最新区块哈希一致，执行syn

4. 如果高度不一致或最新区块不一致，pull服务端SubmitTree作为副本tree，并将本地SubmitTree合并到副本tree

5. 提交副本tree到服务端，服务端执行合并

6. 客户端请求SubmitTree的最新高度和最新区块

7. 如果高度相同并且最新区块哈希一致，使用副本tree替换本地tree

8. 否则，放弃副本tree，重新执行步骤2
