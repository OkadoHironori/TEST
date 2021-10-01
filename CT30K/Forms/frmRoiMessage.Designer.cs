namespace CT30K
{
    partial class frmRoiMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRoiMessage));
            this.cmdGo = new System.Windows.Forms.Button();
            this.cmdExit = new System.Windows.Forms.Button();
            this.lblComment = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmdGo
            // 
            this.cmdGo.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cmdGo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.cmdGo.Image = ((System.Drawing.Image)(resources.GetObject("cmdGo.Image")));
            this.cmdGo.Location = new System.Drawing.Point(160, 36);
            this.cmdGo.Name = "cmdGo";
            this.cmdGo.Size = new System.Drawing.Size(33, 33);
            this.cmdGo.TabIndex = 0;
            this.cmdGo.UseVisualStyleBackColor = true;
            this.cmdGo.Click += new System.EventHandler(this.cmdGo_Click);
            // 
            // cmdExit
            // 
            this.cmdExit.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cmdExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.cmdExit.Image = ((System.Drawing.Image)(resources.GetObject("cmdExit.Image")));
            this.cmdExit.Location = new System.Drawing.Point(204, 36);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(33, 33);
            this.cmdExit.TabIndex = 1;
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // lblComment
            // 
            this.lblComment.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblComment.Location = new System.Drawing.Point(2, 2);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(247, 35);
            this.lblComment.TabIndex = 2;
            this.lblComment.Text = "#断面像上にROIを描画してからGoボタンを押してください。";
            // 
            // frmRoiMessage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(249, 72);
            this.ControlBox = false;
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.cmdExit);
            this.Controls.Add(this.cmdGo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmRoiMessage";
            this.ShowInTaskbar = false;
            this.Text = "#ROIメッセージ";
            this.Load += new System.EventHandler(this.frmRoiMessage_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdGo;
        private System.Windows.Forms.Button cmdExit;
        private System.Windows.Forms.Label lblComment;
    }
}