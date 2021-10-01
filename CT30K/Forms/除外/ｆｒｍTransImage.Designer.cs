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
            this.ctlTransImage = new CT30K.CTImageCanvas();
            this.ctTransImage = new CT30K.CTImageCanvas();
            this.SuspendLayout();
            // 
            // ctlTransImage
            // 
            this.ctlTransImage.BackColor = System.Drawing.Color.Black;
            this.ctlTransImage.FCDFIDRate = 0D;
            this.ctlTransImage.Location = new System.Drawing.Point(12, 11);
            this.ctlTransImage.Name = "ctlTransImage";
            this.ctlTransImage.Size = new System.Drawing.Size(257, 250);
            this.ctlTransImage.TabIndex = 0;
            // 
            // ctTransImage
            // 
            this.ctTransImage.BackColor = System.Drawing.Color.Black;
            this.ctTransImage.FCDFIDRate = 0D;
            this.ctTransImage.Location = new System.Drawing.Point(12, 11);
            this.ctTransImage.Name = "ctTransImage";
            this.ctTransImage.Size = new System.Drawing.Size(257, 250);
            this.ctTransImage.TabIndex = 0;
            // 
            // frmTransImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.ctlTransImage);
            this.Name = "frmTransImage";
            this.Text = "frmTransImage";
            this.ResumeLayout(false);

        }

        #endregion

        internal CTImageCanvas ctlTransImage;
        internal CTImageCanvas ctTransImage;

    }
}