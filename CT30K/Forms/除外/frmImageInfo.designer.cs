namespace CT30K
{
    partial class frmImageInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImageInfo));
            this.cmdDetailMode = new System.Windows.Forms.Button();
            this.fraScale = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // cmdDetailMode
            // 
            this.cmdDetailMode.Enabled = false;
            this.cmdDetailMode.Location = new System.Drawing.Point(127, 415);
            this.cmdDetailMode.Name = "cmdDetailMode";
            this.cmdDetailMode.Size = new System.Drawing.Size(84, 25);
            this.cmdDetailMode.TabIndex = 0;
            this.cmdDetailMode.Text = "詳細表示>>";
            this.cmdDetailMode.UseVisualStyleBackColor = true;
            this.cmdDetailMode.Click += new System.EventHandler(this.cmdDetailMode_Click);
            // 
            // fraScale
            // 
            this.fraScale.BackColor = System.Drawing.SystemColors.Control;
            this.fraScale.Cursor = System.Windows.Forms.Cursors.Default;
            this.fraScale.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fraScale.Location = new System.Drawing.Point(7, 416);
            this.fraScale.Name = "fraScale";
            this.fraScale.Size = new System.Drawing.Size(105, 25);
            this.fraScale.TabIndex = 4;
            // 
            // frmImageInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(221, 450);
            this.Controls.Add(this.fraScale);
            this.Controls.Add(this.cmdDetailMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmImageInfo";
            this.ShowInTaskbar = false;
            this.Text = "画像情報";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmImageInfo_FormClosing);
            this.Load += new System.EventHandler(this.frmImageInfo_Load);
            this.ClientSizeChanged += new System.EventHandler(this.frmImageInfo_ClientSizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdDetailMode;
        private System.Windows.Forms.Panel fraScale;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}