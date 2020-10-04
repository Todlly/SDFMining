using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace serv
{
    class Program
    {
        static List<Client> clientsList = new List<Client>();

        static public void ClientListen(Object clnt)
        {
            string stream = "";

            Client client = clnt as Client;
            while (true)
            {
                byte[] buff = new byte[1024];
                int length = 0;
                if ((length = client.Socket.Receive(buff)) == 0)
                {
                    Console.WriteLine("disconnecting id " + client.ID);
                    break;
                }

                string s = Encoding.ASCII.GetString(buff, 0, length);
                stream += s;
                while (stream.IndexOf("\r\n") != -1)
                {
                    int index = stream.IndexOf("\r\n");
                    string command = stream.Substring(0, index);
                    Console.WriteLine(command);
                    Execute(command, client);
                    stream = stream.Remove(0, index + 2);
                }
            }
        }

        static void Execute(string command, Client sender)
        {
            string[] parts = command.Split(' ');
            switch (parts[0])
            {
                case "set":
                    switch (parts[1])
                    {
                        case "id":
                            sender.ID = Convert.ToInt32(parts[2]);
                            sender.Socket.Send(Encoding.ASCII.GetBytes("test"));
                            break;
                    }
                    break;
                case "lst":
                    string reply = "";
                    foreach (Client cli in clientsList)
                    {
                        reply += cli.ID + "\n";
                    }
                    sender.Socket.Send(Encoding.ASCII.GetBytes(reply));
                    break;
            }
        }

        static void Main(string[] args)
        {
            Socket serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 7000));

            serverSocket.Listen(5);

            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                Client client = new Client(clientSocket);
                clientsList.Add(client);

                Thread thread = new Thread(new ParameterizedThreadStart(ClientListen));
                thread.Start(client);
            }
        }
    }
}
