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
        private string data;
        public int x0 = 0;
        public int y0 = 0;
        public bool pending = true;
        public bool busy = false;
        private int giver;
        private string result;
        private Socket serverSocket;

        public Job(string data, int giver, Socket giverSocket)
        {
            this.data = data;
            this.giver = giver;
            this.serverSocket = giverSocket;
        }

        public byte[] ReturnResult()
        {
            string output = "msg " + giver + " JobResult " + result + "\r\n";
            return Encoding.ASCII.GetBytes(output);
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

    public class Program
    {
        public static Socket ServerSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

        static List<Job> Jobs = new List<Job>();

        public const int MaxJobs = 4;

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
                        ServerSocket.Send(job.ReturnResult());
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
                case "msg":
                    string message = "";
                    for (int i = 2; i < cmdParts.Length; i++)
                        message += cmdParts[i] + ' ';
                    Console.WriteLine("Message from " + cmdParts[1] + ": "  + message + "\r\n");
                    break;
                case "job":
                    if (Jobs.Count < MaxJobs)
                    {
                        string data = cmdParts[2];
                        Jobs.Add(new Job(data, Convert.ToInt32(cmdParts[1]), ServerSocket));
                    }
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
            ServerSocket.Connect("192.168.23.10", 7000);
            ThreadStart Work = new ThreadStart(Working);
            Thread StartWorking = new Thread(Work);
            StartWorking.Start();

            ThreadStart Listen = new ThreadStart(ListeningServer);
            Thread StartListening = new Thread(Listen);
            StartListening.Start();

            while (true)
            {
                string cmd = Console.ReadLine();
                ServerSocket.Send(Encoding.ASCII.GetBytes(cmd + "\r\n"));
            }
        }
    }
}
