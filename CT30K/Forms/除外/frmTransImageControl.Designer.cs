namespace CT30K
{
    partial class frmTransImageControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTransImageControl));
            this.cmdTransImage = new System.Windows.Forms.Button();
            this.cmdDoubleOblique = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdTransImage
            // 
            this.cmdTransImage.Location = new System.Drawing.Point(76, 48);
            this.cmdTransImage.Name = "cmdTransImage";
            this.cmdTransImage.Size = new System.Drawing.Size(106, 34);
            this.cmdTransImage.TabIndex = 0;
            this.cmdTransImage.Text = "透視画像処理";
            this.cmdTransImage.UseVisualStyleBackColor = true;
            // 
            // cmdDoubleOblique
            // 
            this.cmdDoubleOblique.Image = ((System.Drawing.Image)(resources.GetObject("cmdDoubleOblique.Image")));
            this.cmdDoubleOblique.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdDoubleOblique.Location = new System.Drawing.Point(76, 88);
            this.cmdDoubleOblique.Name = "cmdDoubleOblique";
            this.cmdDoubleOblique.Size = new System.Drawing.Size(106, 36);
            this.cmdDoubleOblique.TabIndex = 2;
            this.cmdDoubleOblique.Text = "ダブルオブリーク";
            this.cmdDoubleOblique.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdDoubleOblique.UseVisualStyleBackColor = true;
            // 
            // frmTransImageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.cmdDoubleOblique);
            this.Controls.Add(this.cmdTransImage);
            this.Name = "frmTransImageControl";
            this.Text = "frmTransImageControl";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button cmdTransImage;
        internal System.Windows.Forms.Button cmdDoubleOblique;

    }
}