namespace CT30K
{
    partial class frmScanCondition
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
            this.cwneImageRotateAngle = new System.Windows.Forms.NumericUpDown();
            this.cwneSlice = new System.Windows.Forms.NumericUpDown();
            this.cwneDelta_z = new System.Windows.Forms.NumericUpDown();
            this.cwneK = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.cwneImageRotateAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSlice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneDelta_z)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneK)).BeginInit();
            this.SuspendLayout();
            // 
            // cwneImageRotateAngle
            // 
            this.cwneImageRotateAngle.Location = new System.Drawing.Point(40, 63);
            this.cwneImageRotateAngle.Name = "cwneImageRotateAngle";
            this.cwneImageRotateAngle.Size = new System.Drawing.Size(98, 19);
            this.cwneImageRotateAngle.TabIndex = 0;
            // 
            // cwneSlice
            // 
            this.cwneSlice.Location = new System.Drawing.Point(40, 100);
            this.cwneSlice.Name = "cwneSlice";
            this.cwneSlice.Size = new System.Drawing.Size(98, 19);
            this.cwneSlice.TabIndex = 1;
            // 
            // cwneDelta_z
            // 
            this.cwneDelta_z.Location = new System.Drawing.Point(40, 135);
            this.cwneDelta_z.Name = "cwneDelta_z";
            this.cwneDelta_z.Size = new System.Drawing.Size(98, 19);
            this.cwneDelta_z.TabIndex = 2;
            // 
            // cwneK
            // 
            this.cwneK.Location = new System.Drawing.Point(40, 174);
            this.cwneK.Name = "cwneK";
            this.cwneK.Size = new System.Drawing.Size(98, 19);
            this.cwneK.TabIndex = 3;
            // 
            // frmScanCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.cwneK);
            this.Controls.Add(this.cwneDelta_z);
            this.Controls.Add(this.cwneSlice);
            this.Controls.Add(this.cwneImageRotateAngle);
            this.Name = "frmScanCondition";
            this.Text = "frmScanCondition";
            ((System.ComponentModel.ISupportInitialize)(this.cwneImageRotateAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSlice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneDelta_z)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneK)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.NumericUpDown cwneImageRotateAngle;
        internal System.Windows.Forms.NumericUpDown cwneSlice;
        internal System.Windows.Forms.NumericUpDown cwneDelta_z;
        internal System.Windows.Forms.NumericUpDown cwneK;


    }
}