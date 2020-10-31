using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientWorker
{
    public class Job
    {
        public int ID { get; }
        public string data;
        public int x0 = 0;
        public int y0 = 0;
        public bool pending = true;
        public bool busy = false;
        public int giver;
        public string result;
        public Socket serverSocket;

        public Job(string data, int giver, Socket giverSocket, int jobID)
        {
            this.data = data;
            this.giver = giver;
            this.serverSocket = giverSocket;
            this.ID = jobID;
        }

        

        public void DoJob()
        {
            string[] nums = data.Split('|');
            string output = "";
            for (int i = 0; i < nums.Length; i++)
            {
                output += Convert.ToInt32(nums[i]) * 2;
                if (i != nums.Length - 1) output += '|';
            }

            result = output;
        }
    }

    public class Worker
    {
        public static Socket ServerSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

        static List<Job> Jobs = new List<Job>();

        public static int WorkID { get; private set; }
        public const int MaxJobs = 4;

        private static Random Random = new Random();

        public static void Working()
        {
            string stream = "";
            while (true)
            {
                for (int i = 0; i < Jobs.Count; i++)
                {
                    Job job = Jobs[i];
                    if (job.pending && !job.busy)
                    {
                        job.pending = false;
                        job.busy = true;
                        job.DoJob();
                        string output = "msg " + job.giver + " JobResult " + job.ID + " " + WorkID + " " + job.result + " \r\n";
                        ServerSocket.Send(Encoding.ASCII.GetBytes(output));
                        Jobs.Remove(job);
                    }
                }
            }
        }

        static string DoJob(string data)
        {
            string[] nums = data.Split('|');
            string output = "";
            for (int i = 0; i < nums.Length; i++)
            {
                output += Convert.ToInt32(nums[i]) * 2;
                if (i != nums.Length - 1) output += '|';
            }

            return output;
        }

        static void EncodeCmd(string cmd)
        {
            string[] cmdParts = cmd.Split(' ');
            switch (cmdParts[0])
            {
                case "JobResult":
                    
                    break;
                case "msg":
                    string message = "";
                    for (int i = 2; i < cmdParts.Length; i++)
                        message += cmdParts[i] + ' ';
                    //Console.WriteLine("Message from " + cmdParts[1] + ": "  + message + "\r\n");
                    break;
                case "job":
                    if (Jobs.Count < MaxJobs)
                    {
                        string data = cmdParts[3];
                        Jobs.Add(new Job(data, Convert.ToInt32(cmdParts[1]), ServerSocket, Convert.ToInt32(cmdParts[2])));
                    }
                    break;
                case "IDGiven":
                    WorkID = Convert.ToInt32(cmdParts[1]);
                    break;
            }
        }

        public static void ListeningServer()
        {
            string stream = "";
            while (true)
            {
                byte[] buff = new byte[1024];
                int length = 0;
                length = ServerSocket.Receive(buff);

                string s = Encoding.ASCII.GetString(buff, 0, length);
                stream += s;
                while (stream.IndexOf("\n") != -1)
                {
                    int index = stream.IndexOf("\n");
                    string command = stream.Substring(0, index);
                    Console.WriteLine(command);
                    EncodeCmd(command);
                    stream = stream.Remove(0, index + 1);
                }
            }
        }

        static void Main(string[] args)
        {
            ServerSocket.Connect("192.168.56.1", 7000);
            ThreadStart Work = new ThreadStart(Working);
            Thread StartWorking = new Thread(Work);
            StartWorking.Start();

            ThreadStart Listen = new ThreadStart(ListeningServer);
            Thread StartListening = new Thread(Listen);
            StartListening.Start();
            ServerSocket.Send(Encoding.ASCII.GetBytes("set type worker\r\n"));

            while (true)
            {
                string cmd = Console.ReadLine();
                ServerSocket.Send(Encoding.ASCII.GetBytes(cmd + "\r\n"));
            }
        }
    }
}
