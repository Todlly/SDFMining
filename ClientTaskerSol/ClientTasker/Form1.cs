using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientTasker
{
    public partial class Form1 : Form
    {
        static IPEndPoint endpoint = new IPEndPoint(3232235523, 7000);

        Recruiter recruiter;

        public Form1()
        {
            InitializeComponent();
            Timer timer = new Timer();
            recruiter = new Recruiter(endpoint);
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
           recruiter.GiveJob(0, new int[] { 2, 3 });
        }
    }
}
