namespace CT30K
{
    partial class frmResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmResult));
            this.Toolbar1 = new System.Windows.Forms.ToolStrip();
            this.tsbtnSave = new System.Windows.Forms.ToolStripButton();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.Toolbar1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Toolbar1
            // 
            this.Toolbar1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.Toolbar1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnSave});
            this.Toolbar1.Location = new System.Drawing.Point(0, 0);
            this.Toolbar1.Name = "Toolbar1";
            this.Toolbar1.Size = new System.Drawing.Size(161, 25);
            this.Toolbar1.TabIndex = 0;
            this.Toolbar1.Text = "toolStrip1";
            this.Toolbar1.MouseEnter += new System.EventHandler(this.Toolbar1_MouseEnter);
            // 
            // tsbtnSave
            // 
            this.tsbtnSave.AutoToolTip = false;
            this.tsbtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnSave.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnSave.Image")));
            this.tsbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSave.Name = "tsbtnSave";
            this.tsbtnSave.Size = new System.Drawing.Size(23, 22);
            this.tsbtnSave.Click += new System.EventHandler(this.Save_Click);
            // 
            // txtResult
            // 
            this.txtResult.BackColor = System.Drawing.Color.White;
            this.txtResult.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtResult.Location = new System.Drawing.Point(0, 32);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(161, 573);
            this.txtResult.TabIndex = 1;
            // 
            // frmResult
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(161, 605);
            this.ControlBox = false;
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.Toolbar1);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "frmResult";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "測定結果";
            this.Load += new System.EventHandler(this.frmResult_Load);
            this.Resize += new System.EventHandler(this.frmResult_Resize);
            this.Toolbar1.ResumeLayout(false);
            this.Toolbar1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip Toolbar1;
        private System.Windows.Forms.ToolStripButton tsbtnSave;
        private System.Windows.Forms.TextBox txtResult;
    }
}