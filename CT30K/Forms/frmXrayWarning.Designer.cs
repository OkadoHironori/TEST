namespace CT30K
{
    partial class frmXrayWarning
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
            this.components = new System.ComponentModel.Container();
            this.cmdYes = new System.Windows.Forms.Button();
            this.cmdNo = new System.Windows.Forms.Button();
            this.lblXWarnMessage = new System.Windows.Forms.Label();
            this.tmrSuperVisoryTimer = new System.Windows.Forms.Timer(this.components);
            this.tmrBeep = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // cmdYes
            // 
            this.cmdYes.Location = new System.Drawing.Point(96, 48);
            this.cmdYes.Name = "cmdYes";
            this.cmdYes.Size = new System.Drawing.Size(77, 30);
            this.cmdYes.TabIndex = 0;
            this.cmdYes.Tag = "10003";
            this.cmdYes.Text = "Yes";
            this.cmdYes.UseVisualStyleBackColor = true;
            this.cmdYes.Click += new System.EventHandler(this.cmdYes_Click);
            // 
            // cmdNo
            // 
            this.cmdNo.Location = new System.Drawing.Point(228, 48);
            this.cmdNo.Name = "cmdNo";
            this.cmdNo.Size = new System.Drawing.Size(77, 30);
            this.cmdNo.TabIndex = 1;
            this.cmdNo.Tag = "10004";
            this.cmdNo.Text = "No";
            this.cmdNo.UseVisualStyleBackColor = true;
            this.cmdNo.Click += new System.EventHandler(this.cmdNo_Click);
            // 
            // lblXWarnMessage
            // 
            this.lblXWarnMessage.Location = new System.Drawing.Point(12, 8);
            this.lblXWarnMessage.Name = "lblXWarnMessage";
            this.lblXWarnMessage.Size = new System.Drawing.Size(381, 40);
            this.lblXWarnMessage.TabIndex = 2;
            this.lblXWarnMessage.Text = "label1";
            // 
            // tmrSuperVisoryTimer
            // 
            this.tmrSuperVisoryTimer.Tick += new System.EventHandler(this.tmrSuperVisoryTimer_Tick);
            // 
            // tmrBeep
            // 
            this.tmrBeep.Tick += new System.EventHandler(this.tmrBeep_Tick);
            // 
            // frmXrayWarning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 90);
            this.ControlBox = false;
            this.Controls.Add(this.lblXWarnMessage);
            this.Controls.Add(this.cmdNo);
            this.Controls.Add(this.cmdYes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmXrayWarning";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "17484";
            this.Text = "#警告メッセージ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmXrayWarning_FormClosed);
            this.Load += new System.EventHandler(this.frmXrayWarning_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdYes;
        private System.Windows.Forms.Button cmdNo;
        private System.Windows.Forms.Label lblXWarnMessage;
        private System.Windows.Forms.Timer tmrSuperVisoryTimer;
        private System.Windows.Forms.Timer tmrBeep;
    }
}