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
            this.btnTransImage = new System.Windows.Forms.Button();
            this.btnDoubleOblique = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTransImage
            // 
            this.btnTransImage.Location = new System.Drawing.Point(136, 151);
            this.btnTransImage.Name = "btnTransImage";
            this.btnTransImage.Size = new System.Drawing.Size(106, 31);
            this.btnTransImage.TabIndex = 0;
            this.btnTransImage.Text = "透視画像処理";
            this.btnTransImage.UseVisualStyleBackColor = true;
            // 
            // btnDoubleOblique
            // 
            this.btnDoubleOblique.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnDoubleOblique.Image = ((System.Drawing.Image)(resources.GetObject("btnDoubleOblique.Image")));
            this.btnDoubleOblique.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDoubleOblique.Location = new System.Drawing.Point(136, 225);
            this.btnDoubleOblique.Name = "btnDoubleOblique";
            this.btnDoubleOblique.Size = new System.Drawing.Size(106, 36);
            this.btnDoubleOblique.TabIndex = 1;
            this.btnDoubleOblique.Text = "ダブルオブリーク";
            this.btnDoubleOblique.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDoubleOblique.UseVisualStyleBackColor = true;
            // 
            // frmTransImageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.btnDoubleOblique);
            this.Controls.Add(this.btnTransImage);
            this.Name = "frmTransImageControl";
            this.Text = "frmTransImageControl";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTransImage;
        private System.Windows.Forms.Button btnDoubleOblique;
    }
}