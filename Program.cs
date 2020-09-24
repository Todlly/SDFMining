using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace serv
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Any, 7000));

            server.Listen(5);

            while (true)
            {
                Socket client = server.Accept();

                string stream = "";
                while (true)
                {
                    byte[] buff = new byte[8];
                    client.Receive(buff);
                    string s = Encoding.ASCII.GetString(buff);

                    string[] st = s.Split('\0');

                    if (s.IndexOf('\n') == -1)
                    {
                        foreach (string i in st)
                            stream += i;
                        continue;
                    }

                    Console.Write(stream);
                }
            }
        }
    }
}
