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
            this.fraCommentEdit = new System.Windows.Forms.GroupBox();
            this.lblOn = new System.Windows.Forms.Label();
            this.chkCommentEdit = new System.Windows.Forms.CheckBox();
            this.cmdCommentEdit = new System.Windows.Forms.Button();
            this.fraCommentEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdTransImage
            // 
            this.cmdTransImage.Location = new System.Drawing.Point(136, 173);
            this.cmdTransImage.Name = "cmdTransImage";
            this.cmdTransImage.Size = new System.Drawing.Size(106, 36);
            this.cmdTransImage.TabIndex = 0;
            this.cmdTransImage.Tag = "12596";
            this.cmdTransImage.Text = "透視画像処理";
            this.cmdTransImage.UseVisualStyleBackColor = true;
            // 
            // cmdDoubleOblique
            // 
            this.cmdDoubleOblique.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdDoubleOblique.Image = ((System.Drawing.Image)(resources.GetObject("cmdDoubleOblique.Image")));
            this.cmdDoubleOblique.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cmdDoubleOblique.Location = new System.Drawing.Point(136, 225);
            this.cmdDoubleOblique.Name = "cmdDoubleOblique";
            this.cmdDoubleOblique.Size = new System.Drawing.Size(106, 36);
            this.cmdDoubleOblique.TabIndex = 1;
            this.cmdDoubleOblique.Text = "ダブルオブリーク";
            this.cmdDoubleOblique.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdDoubleOblique.UseVisualStyleBackColor = true;
            // 
            // fraCommentEdit
            // 
            this.fraCommentEdit.Controls.Add(this.lblOn);
            this.fraCommentEdit.Controls.Add(this.chkCommentEdit);
            this.fraCommentEdit.Controls.Add(this.cmdCommentEdit);
            this.fraCommentEdit.Location = new System.Drawing.Point(136, 105);
            this.fraCommentEdit.Name = "fraCommentEdit";
            this.fraCommentEdit.Size = new System.Drawing.Size(106, 49);
            this.fraCommentEdit.TabIndex = 4;
            this.fraCommentEdit.TabStop = false;
            this.fraCommentEdit.Tag = "12816";
            this.fraCommentEdit.Text = "#コメント";
            // 
            // lblOn
            // 
            this.lblOn.AutoSize = true;
            this.lblOn.Location = new System.Drawing.Point(27, 22);
            this.lblOn.Name = "lblOn";
            this.lblOn.Size = new System.Drawing.Size(21, 12);
            this.lblOn.TabIndex = 6;
            this.lblOn.Text = "ON";
            // 
            // chkCommentEdit
            // 
            this.chkCommentEdit.AutoSize = true;
            this.chkCommentEdit.Location = new System.Drawing.Point(9, 22);
            this.chkCommentEdit.Name = "chkCommentEdit";
            this.chkCommentEdit.Size = new System.Drawing.Size(15, 14);
            this.chkCommentEdit.TabIndex = 5;
            this.chkCommentEdit.UseVisualStyleBackColor = true;
            this.chkCommentEdit.CheckedChanged += new System.EventHandler(this.chkCommentEdit_CheckedChanged);
            // 
            // cmdCommentEdit
            // 
            this.cmdCommentEdit.Location = new System.Drawing.Point(53, 10);
            this.cmdCommentEdit.Name = "cmdCommentEdit";
            this.cmdCommentEdit.Size = new System.Drawing.Size(46, 36);
            this.cmdCommentEdit.TabIndex = 4;
            this.cmdCommentEdit.Tag = "22016";
            this.cmdCommentEdit.Text = "#編集";
            this.cmdCommentEdit.UseVisualStyleBackColor = true;
            // 
            // frmTransImageControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.ControlBox = false;
            this.Controls.Add(this.cmdDoubleOblique);
            this.Controls.Add(this.cmdTransImage);
            this.Controls.Add(this.fraCommentEdit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTransImageControl";
            this.ShowInTaskbar = false;
            this.Text = "frmTransImageControl";
            this.Load += new System.EventHandler(this.frmTransImageControl_Load);
            this.fraCommentEdit.ResumeLayout(false);
            this.fraCommentEdit.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Button cmdTransImage;
        internal System.Windows.Forms.Button cmdDoubleOblique;
        private System.Windows.Forms.GroupBox fraCommentEdit;
        private System.Windows.Forms.Label lblOn;
        private System.Windows.Forms.CheckBox chkCommentEdit;
        private System.Windows.Forms.Button cmdCommentEdit;

    }
}