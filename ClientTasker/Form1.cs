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
          //    timer.Tick += Timer_Tick;
          //   timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            recruiter.GiveJob(0, new int[] { 2, 3 });
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            string[] stringData = txt_JobData.Text.Split(' ');
            int[] intData = new int[stringData.Length];
            for (int i = 0; i < intData.Length; i++)
                intData[i] = Convert.ToInt32(stringData[i]);

            int[] result = recruiter.GiveJob(0, intData);
            lbl_JobResult.Text = "";
            foreach(int num in result)
            {
                lbl_JobResult.Text += num + " ";
            }
        }
    }
}
