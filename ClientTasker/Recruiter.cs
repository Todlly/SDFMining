using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientTasker
{
    public struct PendingJob
    {
        public int WorkerID;
        public int JobID;

        public PendingJob(int wid, int jid)
        {
            WorkerID = wid;
            JobID = jid;
        }
    }

    public class Recruiter
    {
        private Socket ServerSocket { get; }
        private EndPoint Server { get; }
        public List<int> WorkersIDs { get; set; } = new List<int>();
        private Form1 Tasker { get; }
        private List<PendingJob> PendingJobs { get; set; } = new List<PendingJob>();
        private Random Random { get; set; } = new Random();

        public Recruiter(EndPoint endpoint, Form1 parent)
        {
            Tasker = parent;
            ServerSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            Server = endpoint;

            try
            {
                ServerSocket.Connect("192.168.56.1", 7000);
                byte[] answer = new byte[1024];
                int ind = ServerSocket.Receive(answer);
                ThreadStart Listen = new ThreadStart(ListenServer);
                Thread listening = new Thread(Listen);
                listening.Start();
                string ans = Encoding.ASCII.GetString(answer);
                RefreshWorkersList();
            }
            catch (Exception e)
            {

            }
        }

        private void ReceiveJobResult(PendingJob job, int[] resultData)
        {
            if (PendingJobs.Contains(job))
            {
                PendingJobs.Remove(job);
                Tasker.CompletedJobs.Add(new Job(job.JobID, resultData));
            }
        }

        private void ExecuteCommand(string command)
        {
            string[] cmdParts = command.Split(' ');

            switch (cmdParts[0])
            {
                case "JobResult":
                    int jobID = Convert.ToInt32(cmdParts[1]);
                    int workerID = Convert.ToInt32(cmdParts[2]);
                    string[] strData = cmdParts[3].Split('|');
                    int[] intData = new int[strData.Length];
                    for (int i = 0; i < strData.Length; i++)
                        intData[i] = Convert.ToInt32(strData[i]);
                    ReceiveJobResult(new PendingJob(workerID, jobID), intData);
                    break;
            }
        }

        private void ListenServer()
        {
            while (true)
            {
                byte[] buff = new byte[1024];
                int ind = ServerSocket.Receive(buff);
                string cmd = Encoding.ASCII.GetString(buff).Substring(0, ind);
                ExecuteCommand(cmd);
            }
        }

        public void RefreshWorkersList()
        {
            ServerSocket.Send(Encoding.ASCII.GetBytes("get workers\r\n"));
            byte[] answer = new byte[1024];
            int len = ServerSocket.Receive(answer);
            string[] stringsAnswer = Encoding.ASCII.GetString(answer).Substring(0, len).Split(' ');
            if (stringsAnswer[0] == "Workers")
            {
                List<int> availableIDs = new List<int>();
                foreach (string strID in stringsAnswer[1].Split('|'))
                {
                    availableIDs.Add(Convert.ToInt32(strID));
                    int availableID = Convert.ToInt32(strID);
                    if (!WorkersIDs.Contains(availableID))
                        WorkersIDs.Add(availableID);
                }
                foreach (int existingID in WorkersIDs)
                {
                    if (!availableIDs.Contains(existingID))
                        WorkersIDs.Remove(existingID);
                }
            }
        }

        public int ChooseWorker()
        {
            return WorkersIDs[0];
        }

        public void Disconnect()
        {
            ServerSocket.Disconnect(false);
        }

        public void GiveJob(int workerID, int jobID, int[] data)
        {
            string dataString = "";
            for (int i = 0; i < data.Length; i++)
            {
                dataString += data[i];
                if (i != data.Length - 1)
                    dataString += '|';
            }
            string command = "job " + workerID + " " + jobID + " " + dataString + " \r\n";
            PendingJobs.Add(new PendingJob(workerID, jobID));

            ServerSocket.Send(Encoding.ASCII.GetBytes(command));

        }
    }
}
