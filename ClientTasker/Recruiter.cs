using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public Socket ServerSocket { get; }
        public List<int> AvailableWorkersIDs { get; set; } = new List<int>();
        private Form1 Tasker { get; }
        private List<PendingJob> PendingJobs { get; set; } = new List<PendingJob>();
        private Random Random { get; set; } = new Random();

        public Recruiter(Form1 parent)
        {
            Tasker = parent;
            ServerSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

        }

        public void TryConnect(EndPoint endPoint)
        {
            try
            {
                ServerSocket.Connect(endPoint);
                ThreadStart Listen = new ThreadStart(ListenServer);
                Thread listening = new Thread(Listen);
                listening.Start();
                ServerSocket.Send(Encoding.ASCII.GetBytes("set type tasker\r\n"));
                RefreshWorkersList();
            }
            catch (Exception e)
            {
                MessageBox.Show("Connection failed");
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
            Console.WriteLine(command);
            string[] cmdParts = command.Trim().Split(' ');

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
                case "Workers":
                    if (cmdParts.Length <= 1)
                        break;

                    List<int> availableIDs = new List<int>();
                    foreach (string strID in cmdParts[1].Split('|'))
                    {
                        availableIDs.Add(Convert.ToInt32(strID));
                        int availableID = Convert.ToInt32(strID);
                        if (!AvailableWorkersIDs.Contains(availableID))
                            AvailableWorkersIDs.Add(availableID);
                    }
                    foreach (int existingID in AvailableWorkersIDs)
                    {
                        if (!availableIDs.Contains(existingID))
                            AvailableWorkersIDs.Remove(existingID);
                    }
                    break;
                case "Disconnect":
                    ServerSocket.Close();
                    break;
            }
        }

        private void ListenServer()
        {
            string stream = "";
            while (true)
            {
                if (!ServerSocket.Connected)
                    break;

                byte[] buff = new byte[1024];
                int ind = ServerSocket.Receive(buff);
                string recievedMessage = Encoding.ASCII.GetString(buff).Substring(0, ind);
                stream += recievedMessage;
                int index = -1;
                while ((index = stream.IndexOf('\n')) != -1)
                {
                    string command = stream.Substring(0, index);
                    stream = stream.Remove(0, index + 1);
                    ExecuteCommand(command);
                }
            }
        }

        public void RefreshWorkersList()
        {
            ServerSocket.Send(Encoding.ASCII.GetBytes("get workers \r\n"));
        }

        public int ChooseWorker()
        {
            if (AvailableWorkersIDs.Count > 0)
                return AvailableWorkersIDs[0];
            else
                return -1;
        }

        public void Disconnect()
        {
            ServerSocket.Send(Encoding.ASCII.GetBytes("Disconnect\r\n"));
        }

        public void GiveJob(int jobID, int[] data)
        {
            string dataString = "";
            int workerID;
            if ((workerID = ChooseWorker()) == -1)
            {
                Debug.WriteLine("No available workers");
                return;
            }

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
