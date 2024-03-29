# MQTT

MQTT（Message Queuing Telemetry Transport，消息队列遥测传输协议），是一种基于发布/订阅（publish/subscribe）模式的轻量级协议，该协议构建于TCP/IP协议之上，MQTT最大优点在于，可以以极少的代码和有限的带宽，为连接远程设备提供实时可靠的消息服务。作为一种低开销、低带宽占用的即时通讯协议，使其在物联网、小型设备、移动应用等方面有较广泛的应用。

MQTT是一个基于客户端-服务器的消息发布/订阅传输协议。MQTT协议是轻量、简单、开放和易于实现的，这些特点使它适用范围非常广泛。在很多情况下，包括受限的环境中，如：机器与机器（M2M）通信和物联网（IoT）。其在，通过卫星链路通信传感器、偶尔拨号的医疗设备、智能家居、及一些小型化设备中已广泛使用。

# 消息

## 作用

当多个客户端同时连接服务端时，当一方发送同步时，其他方会收到服务端提供的消息通知，通知其余客户端应该同步数据。

当客户端发送同步请求时，同步请求会经过服务端队列，服务端并不会立即处理该请求，该请求会处于pending状态。

当服务端处理完成请求后，服务端通过MQTT发送通知，告知客户端已经完成请求。
