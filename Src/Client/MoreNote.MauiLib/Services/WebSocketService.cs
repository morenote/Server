using Fleck;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.MSync.Services
{
    public class WebSocketService
    {
        WebSocketServer server;

        public WebSocketService()
        {
            server = new WebSocketServer("ws://0.0.0.0:8181");
        }



        public void Start()
        {
            server.Start(socket =>
            {
                socket.OnOpen = () => Console.WriteLine("Open!");
                socket.OnClose = () => Console.WriteLine("Close!");
                socket.OnMessage = message => 
                {
                    Console.WriteLine(message);
                    socket.Send(message);

                };
            });
        }

        //接收message字符串，message是一个json，订阅的消息
        public void OnMessage(string message)
        {
            //解析message
            //根据message的类型，进行不同的操作

           
        }
        

    }
}
