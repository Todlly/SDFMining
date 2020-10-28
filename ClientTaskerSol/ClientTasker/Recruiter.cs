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
        public List<int> WorkersIPs { get; private set; }

        public Recruiter(EndPoint endpoint)
        {
            ServerSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            Server = endpoint;

            try
            {
                ServerSocket.Connect("192.168.0.3", 7000);
            } catch(Exception e)
            {
                
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
            //int ind = ServerSocket.Receive(answer);
            //if (ind > 0)
            //{
            //    string[] strNums = Encoding.ASCII.GetString(answer).Substring(0, ind).Split('|');
            //    int[] output = new int[strNums.Length];

            //    for (int i = 0; i < output.Length; i++)
            //        output[i] = Convert.ToInt32(strNums[i]);

            //    return output;
            //}

            return new int[0];
        }
    }
}
