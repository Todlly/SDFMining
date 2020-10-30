using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientTasker
{
    public class Recruiter
    {
        private Socket ServerSocket { get; }
        private EndPoint Server { get; }
        public List<int> WorkersIDs { get; set; } = new List<int>();

        public Recruiter(EndPoint endpoint)
        {
            ServerSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            Server = endpoint;

            try
            {
                ServerSocket.Connect("192.168.0.3", 7000);
                byte[] answer = new byte[1024];
                int ind = ServerSocket.Receive(answer);
                string ans = Encoding.ASCII.GetString(answer);
                RefreshWorkersList();
            }
            catch (Exception e)
            {

            }
        }

        private void RefreshWorkersList()
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
                foreach(int existingID in WorkersIDs)
                {
                    if (!availableIDs.Contains(existingID))
                        WorkersIDs.Remove(existingID);
                }
            }
        }

        public int[] GiveJob(int workerID, int[] data)
        {
            string dataString = "";
            for (int i = 0; i < data.Length; i++)
            {
                dataString += data[i];
                if (i != data.Length - 1)
                    dataString += '|';
            }
            string command = "job " + workerID + " " + dataString + "\r\n";

            ServerSocket.Send(Encoding.ASCII.GetBytes(command));

            byte[] answer = new byte[1024];
            int ind = ServerSocket.Receive(answer);
            if (ind > 0)
            {
                string[] reply = Encoding.ASCII.GetString(answer).Substring(0, ind).Split(' ');
                if (reply[0] != "JobResult")
                    return new int[0];
                reply = reply[1].Split('|');
                int[] nums = new int[reply.Length];

                for (int i = 0; i < nums.Length; i++)
                    nums[i] = Convert.ToInt32(reply[i]);

                return nums;
            }

            return new int[0];
        }
    }
}
