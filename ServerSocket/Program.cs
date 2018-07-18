using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TestMuti
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            int recv;
            byte[] data = new byte[1024];
            //定义一个IP终结点，用于socket监听
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
            //新建一个基于流操作的TCP socket
            Socket newsock = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            //将终结点绑定至socket，并监听端口
            newsock.Bind(ipep);
            newsock.Listen(10);
            Console.WriteLine("等待连接...");
            //有客户端连接时，新建客户端socket
            Socket client = newsock.Accept();
            //获取客户端IP和端口号
            IPEndPoint clientip = (IPEndPoint)client.RemoteEndPoint;
            Console.WriteLine("客户端IP:" + clientip.Address + " 端口号:" + clientip.Port);
            string welcome = "welcome here!";
            data = Encoding.ASCII.GetBytes(welcome);
            //将数据发至客户端
            client.Send(data, data.Length, SocketFlags.None);
            while (true) 
            {
                data = new byte[1024];
                recv = client.Receive(data);
                Console.WriteLine("recv=" + recv);
                if (recv == 0) break;
                Console.WriteLine(Encoding.ASCII.GetString(data,0,recv));
                client.Send(data, recv, SocketFlags.None);
            }
            Console.WriteLine("Disconnected from" + clientip.Address);
            client.Close();
            newsock.Close();
        }
    }
}
