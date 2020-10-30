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
    class Server
    {
        static int[] PoolID = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        static List<Client> clientsList = new List<Client>();

        static int[] GetWorkers()
        {
            List<Client> workers = clientsList.FindAll(client => client.ClientType == ClientType.Worker);
            int[] workersIDs = new int[workers.Count];

            for (int i = 0; i < workers.Count; i++)
                workersIDs[i] = workers[i].ID;

            return workersIDs;
        }

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
                    clientsList.Remove(client);
                    Console.WriteLine("Disconnecting ID " + client.ID);
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
                            sender.Socket.Send(Encoding.ASCII.GetBytes("Your ID is " + sender.ID + " now" + '\n'));
                            break;
                        case "type":
                            switch (parts[2])
                            {
                                case "worker":
                                    clientsList[clientsList.IndexOf(sender)].ClientType = ClientType.Worker;
                                    sender.Socket.Send(Encoding.ASCII.GetBytes("Your type is now " + clientsList[clientsList.IndexOf(sender)].ClientType + "\n"));
                                    break;
                            }
                            break;
                    }
                    break;
                case "get":
                    switch (parts[1])
                    {
                        case "workers":
                            int[] availableWorkers = GetWorkers();
                            string msg = "Workers ";
                            foreach (int id in availableWorkers)
                                msg += id + "|";
                            sender.Socket.Send(Encoding.ASCII.GetBytes(msg));
                            break;
                    }
                    break;
                case "lst":
                    string reply = "Now connected: ";
                    foreach (Client cli in clientsList)
                        reply += cli.ID + " ";
                    reply += '\n';
                    sender.Socket.Send(Encoding.ASCII.GetBytes(reply));
                    break;
                case "msg":
                    if (!int.TryParse(parts[1], out int addressID) || !clientsList.Exists(addressCli => addressCli.ID == addressID))
                    {
                        sender.Socket.Send(Encoding.ASCII.GetBytes("Invalid address ID" + "\r\n"));
                        break;
                    }
                    string message = "";
                    for (int i = 2; i < parts.Length; i++)
                        message += parts[i] + ' ';
                    Client addresser = clientsList.Find(client => client.ID == addressID);
                    addresser.Socket.Send(Encoding.ASCII.GetBytes(message + "\n"));
                    break;
                case "job":
                    if (!int.TryParse(parts[1], out int addresID) || !clientsList.Exists(addressCli => addressCli.ID == addresID))
                    {
                        sender.Socket.Send(Encoding.ASCII.GetBytes("Invalid address ID" + "\n"));
                        break;
                    }
                    Client addreser = clientsList.Find(client => client.ID == addresID);
                    string mesge = "";
                    for (int i = 2; i < parts.Length; i++)
                        mesge += parts[i] + ' ';
                    addreser.Socket.Send(Encoding.ASCII.GetBytes("job " + sender.ID + " " + mesge + "\n"));
                    break;
            }
        }

        static void Main(string[] args)
        {
            Socket serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 7000));

            serverSocket.Listen(15);

            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                Client client = new Client(clientSocket);
                foreach(int ID in PoolID)
                {
                    if(clientsList.Find(cli => cli.ID == ID) == null)
                    {
                        client.ID = ID;
                        break;
                    }
                }
                clientsList.Add(client);
                Console.WriteLine("Connected ID " + client.ID);
                client.Socket.Send(Encoding.ASCII.GetBytes("Connected. Given ID: " + client.ID + "\n"));

                Thread thread = new Thread(new ParameterizedThreadStart(ClientListen));
                thread.Start(client);
            }
        }
    }
}
