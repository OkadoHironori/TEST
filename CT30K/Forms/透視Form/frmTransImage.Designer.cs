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
            this.ctImageCanvas1 = new CT30K.CTImageCanvas();
            this.SuspendLayout();
            // 
            // ctImageCanvas1
            // 
            this.ctImageCanvas1.BackColor = System.Drawing.Color.Black;
            this.ctImageCanvas1.FCDFIDRate = 0D;
            this.ctImageCanvas1.Location = new System.Drawing.Point(0, 0);
            this.ctImageCanvas1.Name = "ctImageCanvas1";
            this.ctImageCanvas1.Size = new System.Drawing.Size(306, 263);
            this.ctImageCanvas1.TabIndex = 0;
            // 
            // frmTransImage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(309, 266);
            this.Controls.Add(this.ctImageCanvas1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmTransImage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TransImage";
            this.ResumeLayout(false);

        }

        #endregion

        private CTImageCanvas ctImageCanvas1;





    }
}