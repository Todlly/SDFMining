using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace serv
{
    public class Client
    {
        public int ID { get; set; }
        public Socket Socket { get; set; }

        public Client(Socket socket)
        {
            Socket = socket;
        }
    }
}
