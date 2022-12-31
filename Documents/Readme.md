# 警告

> 如果您计划使用MoreNotet托管您的笔记，请您务必知晓： MoreNote仍处于有限的内部开发中，`MoreNote` 建议不要在生产环境中使用或托管您的重要笔记，您的笔记很有可能再某次崩溃中全部丢失。 所以，检查你的服务器，检查部署路径，并且使用风险自担。

# 概述

# 快速使用

# 技术细节

# API接口

# 后台搭建

## 环境依赖

安装.Net7 Runtime或.Net7 SDK。

## 配置文件

后台应用需用从配置文件获取运行环境的必要信息，应用程序会尝试在一下路径下面搜索config.json文件。

```shell
#Windows会依次遍历以下路径
C:\morenote\config.json #Windows

#Linux会依次遍历以下路径
/morenote/config.json #Linux(优先级高)
{home目录}/morenote/config.json #Linux 用户目录
/etc/morenote/config.json # Linux etc目录
CONFIG #linux环境变量(优先级低)
```

Config.json的内容请查看Config.md。
