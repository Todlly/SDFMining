namespace ClientTasker
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btn_Send = new System.Windows.Forms.Button();
            this.txt_JobData = new System.Windows.Forms.TextBox();
            this.lbl_JobResult = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.txt_ServerAddress = new System.Windows.Forms.ToolStripTextBox();
            this.txt_ServerPort = new System.Windows.Forms.ToolStripTextBox();
            this.btn_ServerConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Send
            // 
            this.btn_Send.AccessibleName = "";
            this.btn_Send.Location = new System.Drawing.Point(109, 256);
            this.btn_Send.Name = "btn_Send";
            this.btn_Send.Size = new System.Drawing.Size(75, 23);
            this.btn_Send.TabIndex = 0;
            this.btn_Send.Text = "Send";
            this.btn_Send.UseVisualStyleBackColor = true;
            this.btn_Send.Click += new System.EventHandler(this.btn_Send_Click);
            // 
            // txt_JobData
            // 
            this.txt_JobData.AccessibleDescription = "";
            this.txt_JobData.AccessibleName = "";
            this.txt_JobData.Location = new System.Drawing.Point(109, 189);
            this.txt_JobData.Name = "txt_JobData";
            this.txt_JobData.Size = new System.Drawing.Size(100, 20);
            this.txt_JobData.TabIndex = 1;
            // 
            // lbl_JobResult
            // 
            this.lbl_JobResult.AccessibleName = "";
            this.lbl_JobResult.AutoSize = true;
            this.lbl_JobResult.Location = new System.Drawing.Point(258, 150);
            this.lbl_JobResult.Name = "lbl_JobResult";
            this.lbl_JobResult.Size = new System.Drawing.Size(52, 13);
            this.lbl_JobResult.TabIndex = 2;
            this.lbl_JobResult.Text = "Job result";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txt_ServerAddress,
            this.txt_ServerPort,
            this.btn_ServerConnect});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(717, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolMenu";
            // 
            // txt_ServerAddress
            // 
            this.txt_ServerAddress.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txt_ServerAddress.Name = "txt_ServerAddress";
            this.txt_ServerAddress.Size = new System.Drawing.Size(100, 25);
            // 
            // txt_ServerPort
            // 
            this.txt_ServerPort.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txt_ServerPort.Name = "txt_ServerPort";
            this.txt_ServerPort.Size = new System.Drawing.Size(100, 25);
            // 
            // btn_ServerConnect
            // 
            this.btn_ServerConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_ServerConnect.Image = ((System.Drawing.Image)(resources.GetObject("btn_ServerConnect.Image")));
            this.btn_ServerConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_ServerConnect.Name = "btn_ServerConnect";
            this.btn_ServerConnect.Size = new System.Drawing.Size(56, 22);
            this.btn_ServerConnect.Text = "Connect";
            this.btn_ServerConnect.Click += new System.EventHandler(this.btn_ServerConnect_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 401);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lbl_JobResult);
            this.Controls.Add(this.txt_JobData);
            this.Controls.Add(this.btn_Send);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.TextBox txt_JobData;
        private System.Windows.Forms.Label lbl_JobResult;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox txt_ServerAddress;
        private System.Windows.Forms.ToolStripTextBox txt_ServerPort;
        private System.Windows.Forms.ToolStripButton btn_ServerConnect;
    }
}

