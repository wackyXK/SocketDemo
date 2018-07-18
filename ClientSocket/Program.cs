using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ClientSocket
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            byte[] data = new byte[1024];
            //定义一个基于流操作的TCP socket
            Socket newclient = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("请输入服务端ip");
            string ipadd = Console.ReadLine();
            Console.WriteLine();
            Console.Write("请输入服务端端口:");
            int port = Convert.ToInt32(Console.ReadLine());
            //将输入的ip和端口号绑定至终结点
            IPEndPoint ie = new IPEndPoint(IPAddress.Parse(ipadd), port);
            try
            {
                //连接至该ip
                newclient.Connect(ie);
            }
            catch (SocketException e) 
            {
                Console.WriteLine("unable to connect to server");
                Console.WriteLine(e.ToString());
                return;
            }
            //接收返回数据
            int recv = newclient.Receive(data);
            string stringdata = Encoding.ASCII.GetString(data, 0, recv);
            Console.WriteLine(stringdata);
            while (true) 
            {
                string input = Console.ReadLine();
                if (input == "exit") break;
                newclient.Send(Encoding.ASCII.GetBytes(input));
                data = new byte[1024];
                recv = newclient.Receive(data);
                stringdata = Encoding.ASCII.GetString(data,0,recv);
                Console.WriteLine(stringdata);
            }
            Console.WriteLine("disconnect from sercer...");
            newclient.Shutdown(SocketShutdown.Both);
            newclient.Close();
        }
    }
}
