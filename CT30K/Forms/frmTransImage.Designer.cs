namespace CT30K

{
    partial class frmTransImage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTransImage));
            this.mnuPopUp = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuRoiInput = new System.Windows.Forms.ToolStripMenuItem();
            this.vsbImage = new System.Windows.Forms.VScrollBar();
            this.hsbImage = new System.Windows.Forms.HScrollBar();
            this.fraSpace = new System.Windows.Forms.Panel();
            this.ctlTransImage = new CT30K.CTImageCanvas();
            this.mnuPopUp.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuPopUp
            // 
            this.mnuPopUp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRoiInput});
            this.mnuPopUp.Name = "mnuPopUp";
            this.mnuPopUp.Size = new System.Drawing.Size(169, 26);
            // 
            // mnuRoiInput
            // 
            this.mnuRoiInput.Name = "mnuRoiInput";
            this.mnuRoiInput.Size = new System.Drawing.Size(168, 22);
            this.mnuRoiInput.Text = "toolStripMenuItem1";
            // 
            // vsbImage
            // 
            this.vsbImage.LargeChange = 128;
            this.vsbImage.Location = new System.Drawing.Point(0, 0);
            this.vsbImage.Maximum = 12800;
            this.vsbImage.Name = "vsbImage";
            this.vsbImage.Size = new System.Drawing.Size(16, 300);
            this.vsbImage.SmallChange = 8;
            this.vsbImage.TabIndex = 4;
            this.vsbImage.ValueChanged += new System.EventHandler(this.vsbImage_ValueChanged);
            // 
            // hsbImage
            // 
            this.hsbImage.LargeChange = 128;
            this.hsbImage.Location = new System.Drawing.Point(0, 0);
            this.hsbImage.Maximum = 12800;
            this.hsbImage.Name = "hsbImage";
            this.hsbImage.Size = new System.Drawing.Size(300, 16);
            this.hsbImage.SmallChange = 8;
            this.hsbImage.TabIndex = 5;
            this.hsbImage.ValueChanged += new System.EventHandler(this.hsbImage_ValueChanged);
            // 
            // fraSpace
            // 
            this.fraSpace.Location = new System.Drawing.Point(107, 209);
            this.fraSpace.Name = "fraSpace";
            this.fraSpace.Size = new System.Drawing.Size(16, 16);
            this.fraSpace.TabIndex = 6;
            // 
            // ctlTransImage
            // 
            this.ctlTransImage.BackColor = System.Drawing.Color.Black;
            this.ctlTransImage.FCDFIDRate = 0D;
            this.ctlTransImage.Location = new System.Drawing.Point(3, 3);
            this.ctlTransImage.MirrorOn = false;
            this.ctlTransImage.Name = "ctlTransImage";
            this.ctlTransImage.Picture = ((System.Drawing.Bitmap)(resources.GetObject("ctlTransImage.Picture")));
            this.ctlTransImage.Size = new System.Drawing.Size(186, 160);
            this.ctlTransImage.SizeX = 1024;
            this.ctlTransImage.SizeY = 1024;
            this.ctlTransImage.TabIndex = 3;
            this.ctlTransImage.Paint += new System.Windows.Forms.PaintEventHandler(this.ctlTransImage_Paint);
            this.ctlTransImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ctlTransImage_MouseDown);
            this.ctlTransImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ctlTransImage_MouseMove);
            this.ctlTransImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ctlTransImage_MouseUp);
            // 
            // frmTransImage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(527, 487);
            this.ControlBox = false;
            this.Controls.Add(this.hsbImage);
            this.Controls.Add(this.fraSpace);
            this.Controls.Add(this.vsbImage);
            this.Controls.Add(this.ctlTransImage);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTransImage";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TransImage";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmTransImage_FormClosed);
            this.Load += new System.EventHandler(this.frmTransImage_Load);
            this.Resize += new System.EventHandler(this.frmTransImage_Resize);
            this.mnuPopUp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        //internal CTImageCanvas ctlTransImage;
        private System.Windows.Forms.ContextMenuStrip mnuPopUp;
        private System.Windows.Forms.ToolStripMenuItem mnuRoiInput;
        internal CTImageCanvas ctlTransImage;
        private System.Windows.Forms.VScrollBar vsbImage;
        private System.Windows.Forms.HScrollBar hsbImage;
        private System.Windows.Forms.Panel fraSpace;
        //private CTImageCanvas ctImageCanvas1;
        //private CTImageCanvas ctImageCanvas1;






    }
}