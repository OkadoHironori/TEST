namespace CT30K
{
    partial class frmRetryCondition
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
            ((System.ComponentModel.ISupportInitialize)(this.cwneImageRotateAngle)).BeginInit();
            this.SuspendLayout();
            // 
            // cwneImageRotateAngle
            // 
            this.cwneImageRotateAngle.Location = new System.Drawing.Point(93, 49);
            this.cwneImageRotateAngle.Name = "cwneImageRotateAngle";
            this.cwneImageRotateAngle.Size = new System.Drawing.Size(115, 19);
            this.cwneImageRotateAngle.TabIndex = 0;
            // 
            // frmRetryCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.cwneImageRotateAngle);
            this.Name = "frmRetryCondition";
            this.Text = "frmRetryCondition";
            ((System.ComponentModel.ISupportInitialize)(this.cwneImageRotateAngle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.NumericUpDown cwneImageRotateAngle;


    }
}