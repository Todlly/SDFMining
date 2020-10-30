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
            this.btn_Send = new System.Windows.Forms.Button();
            this.txt_JobData = new System.Windows.Forms.TextBox();
            this.lbl_JobResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_Send
            // 
            this.btn_Send.AccessibleName = "";
            this.btn_Send.Location = new System.Drawing.Point(100, 230);
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
            this.txt_JobData.Location = new System.Drawing.Point(84, 124);
            this.txt_JobData.Name = "txt_JobData";
            this.txt_JobData.Size = new System.Drawing.Size(100, 20);
            this.txt_JobData.TabIndex = 1;
            // 
            // lbl_JobResult
            // 
            this.lbl_JobResult.AccessibleName = "";
            this.lbl_JobResult.AutoSize = true;
            this.lbl_JobResult.Location = new System.Drawing.Point(249, 124);
            this.lbl_JobResult.Name = "lbl_JobResult";
            this.lbl_JobResult.Size = new System.Drawing.Size(52, 13);
            this.lbl_JobResult.TabIndex = 2;
            this.lbl_JobResult.Text = "Job result";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 401);
            this.Controls.Add(this.lbl_JobResult);
            this.Controls.Add(this.txt_JobData);
            this.Controls.Add(this.btn_Send);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Send;
        private System.Windows.Forms.TextBox txt_JobData;
        private System.Windows.Forms.Label lbl_JobResult;
    }
}

