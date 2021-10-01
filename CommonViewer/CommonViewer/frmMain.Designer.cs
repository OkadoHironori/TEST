namespace CommonViewer
{
    partial class frmMain
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
            this.MainMenu1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileSub_1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileSub_2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileSub_3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFileSub_4 = new System.Windows.Forms.ToolStripMenuItem();
            this.MainMenu1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu1
            // 
            this.MainMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile});
            this.MainMenu1.Location = new System.Drawing.Point(0, 0);
            this.MainMenu1.Name = "MainMenu1";
            this.MainMenu1.Size = new System.Drawing.Size(532, 24);
            this.MainMenu1.TabIndex = 0;
            this.MainMenu1.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileSub_1,
            this.mnuFileSub_2,
            this.mnuFileSub_3,
            this.mnuFileSub_4});
            this.mnuFile.MergeAction = System.Windows.Forms.MergeAction.Remove;
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(56, 20);
            this.mnuFile.Text = "処理(&S)";
            this.mnuFile.Click += new System.EventHandler(this.mnuFile_Click);
            // 
            // mnuFileSub_1
            // 
            this.mnuFileSub_1.Name = "mnuFileSub_1";
            this.mnuFileSub_1.Size = new System.Drawing.Size(152, 22);
            this.mnuFileSub_1.Text = "&1 コモン";
            this.mnuFileSub_1.Click += new System.EventHandler(this.mnuFileSub_1_Click);
            // 
            // mnuFileSub_2
            // 
            this.mnuFileSub_2.Name = "mnuFileSub_2";
            this.mnuFileSub_2.Size = new System.Drawing.Size(152, 22);
            this.mnuFileSub_2.Text = "&2 付帯情報";
            this.mnuFileSub_2.Click += new System.EventHandler(this.mnuFileSub_2_Click);
            // 
            // mnuFileSub_3
            // 
            this.mnuFileSub_3.Name = "mnuFileSub_3";
            this.mnuFileSub_3.Size = new System.Drawing.Size(149, 6);
            // 
            // mnuFileSub_4
            // 
            this.mnuFileSub_4.Name = "mnuFileSub_4";
            this.mnuFileSub_4.Size = new System.Drawing.Size(152, 22);
            this.mnuFileSub_4.Text = "終了(&X)";
            this.mnuFileSub_4.Click += new System.EventHandler(this.mnuFileSub_4_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(532, 393);
            this.Controls.Add(this.MainMenu1);
            this.IsMdiContainer = true;
            this.Location = new System.Drawing.Point(131, 121);
            this.MainMenuStrip = this.MainMenu1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "コモンと付帯情報のテスト";
            this.MainMenu1.ResumeLayout(false);
            this.MainMenu1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        public System.Windows.Forms.ToolStripMenuItem mnuFileSub_1;
        public System.Windows.Forms.ToolStripMenuItem mnuFileSub_2;
        public System.Windows.Forms.ToolStripSeparator mnuFileSub_3;
        public System.Windows.Forms.ToolStripMenuItem mnuFileSub_4;
    }
}