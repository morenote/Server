# 锁

服务端提供锁机制保护被编辑的文本，当锁被设置时，其他用户无法发起编辑操作。

```
string id//锁id
string LockType//锁类型 全局锁、目录锁、编辑锁、协作锁
string LockedObject//锁定对象id
string UserId//发起用户
String ClientId//客户端Id
string Date//发起时间
string Message//发起者消息
string LockTime//锁时间 单位秒
```

## 全局锁 GlobalLock

- 作用：当全局锁被激活时，笔记数据将进入只读模式。

- 权限：发起者需要拥有管理员权限

- 时限：永久

- 解除：需要管理员手动解除，无法通过强制解锁解除锁定

## 目录锁 CatalogLock

- 作用：阻止其他用户进行目录编辑

- 权限：发起者需要拥有写权限

- 时限：单位秒，最长为10s

- 解锁：发起者，但拥有写权限的用户可以通过强制解锁来解除锁定

- 强制解锁：拥有写权限

## 编辑锁 WriteLock

- 作用：阻止其他用户写入数据

- 权限：发起者需要拥有写权限

- 时限：单位秒，最长为600s

- 刷新：发起者可以通过更新锁时间来延长锁

- 解锁：发起者，但拥有写权限的用户可以通过强制解锁来解除锁定

## 协作锁 CollaborationLock

- 作用：要求其他参与者以协作模式进行编辑，否则等待等待解锁才可以进行编辑

- 权限：发起者需要拥有写权限

- 时限：单位秒，最长为600s

- 刷新：发起者可以通过更新锁时间来延长锁

- 解锁：发起者，但拥有写权限的用户可以通过强制解锁来解除锁定

## 解锁  Unlocking

- 作用：发起者解除锁定

- 权限：发起者或拥有权限的用户

## 强制解锁 ForcedUnlocking

- 作用：强制解除CatalogLock或WriteLock

- 权限：发起者或拥有权限的用户

## 列出锁 ListLocks

- 作用：列出全部锁

- 权限：拥有读权限的所有用户