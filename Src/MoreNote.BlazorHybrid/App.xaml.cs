using Fleck;

namespace MoreNote.BlazorHybrid
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            var server=new WebSocketServer("ws://127.0.0.1:8800");
            server.Start(socket =>
            {
                socket.OnOpen = () => { };
                socket.OnClose = () => { };
                socket.OnMessage = message =>
                {
                    socket.Send("Echo: " + message);
                };
            });
        }
    }
}
