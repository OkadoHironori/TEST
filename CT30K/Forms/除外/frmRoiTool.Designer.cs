namespace CT30K
{
    partial class frmRoiTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRoiTool));
            this.lblProcess = new System.Windows.Forms.Label();
            this.Toolbar1 = new System.Windows.Forms.ToolStrip();
            this.tbRectangle = new System.Windows.Forms.ToolStripButton();
            this.Toolbar1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblProcess
            // 
            this.lblProcess.AutoSize = true;
            this.lblProcess.Location = new System.Drawing.Point(35, 91);
            this.lblProcess.Name = "lblProcess";
            this.lblProcess.Size = new System.Drawing.Size(35, 12);
            this.lblProcess.TabIndex = 0;
            this.lblProcess.Text = "label1";
            // 
            // Toolbar1
            // 
            this.Toolbar1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbRectangle});
            this.Toolbar1.Location = new System.Drawing.Point(0, 0);
            this.Toolbar1.Name = "Toolbar1";
            this.Toolbar1.Size = new System.Drawing.Size(292, 25);
            this.Toolbar1.TabIndex = 1;
            this.Toolbar1.Text = "toolStrip1";
            // 
            // tbRectangle
            // 
            this.tbRectangle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbRectangle.Image = ((System.Drawing.Image)(resources.GetObject("tbRectangle.Image")));
            this.tbRectangle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbRectangle.Name = "tbRectangle";
            this.tbRectangle.Size = new System.Drawing.Size(23, 22);
            this.tbRectangle.Text = "toolStripButton1";
            // 
            // frmRoiTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.Toolbar1);
            this.Controls.Add(this.lblProcess);
            this.Name = "frmRoiTool";
            this.Text = "frmRoiTool";
            this.Toolbar1.ResumeLayout(false);
            this.Toolbar1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label lblProcess;
        internal System.Windows.Forms.ToolStrip Toolbar1;
        private System.Windows.Forms.ToolStripButton tbRectangle;

    }
}