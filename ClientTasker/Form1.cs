using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientTasker
{
    public struct Job
    {
        public int JobID;
        public int[] JobData;

        public Job(int jobID, int[] jobData)
        {
            JobID = jobID;
            JobData = jobData;
        }
    }

    public partial class Form1 : Form
    {
        static IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("192.168.56.1"), 7000);
        Random random = new Random();
        public List<Job> CompletedJobs { get; set; }

        Recruiter recruiter;

        public void ShowJobResult()
        {
            foreach (Job job in CompletedJobs)
            {
                lbl_JobResult.Text = "";
                foreach (int num in job.JobData)
                    lbl_JobResult.Text += num + " ";
            }
        }

        public Form1()
        {
            InitializeComponent();
            recruiter = new Recruiter(this);
            CompletedJobs = new List<Job>();
            Timer timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ShowJobResult();
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            string[] stringData = txt_JobData.Text.Split(' ');
            int[] intData = new int[stringData.Length];
            for (int i = 0; i < intData.Length; i++)
                intData[i] = Convert.ToInt32(stringData[i]);
            int ID = random.Next(0, 10000);


            recruiter.GiveJob(ID, intData);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (recruiter.ServerSocket.Connected)
                recruiter.Disconnect();
        }

        private void btn_ServerConnect_Click(object sender, EventArgs e)
        {
            string ipAddress = txt_ServerAddress.Text;
            int port = Convert.ToInt32(txt_ServerPort.Text);
            endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            recruiter.TryConnect(endpoint);
        }
    }
}
