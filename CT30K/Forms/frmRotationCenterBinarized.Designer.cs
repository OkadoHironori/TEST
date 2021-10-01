namespace CT30K
{
    partial class frmRotationCenterBinarized
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRotationCenterBinarized));
            this.fraControl = new System.Windows.Forms.GroupBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.lblBi = new System.Windows.Forms.Label();
            this.sldImg1 = new System.Windows.Forms.HScrollBar();
            this.picFrame = new System.Windows.Forms.PictureBox();
            this.sldVimg1 = new System.Windows.Forms.VScrollBar();
            this.lblVtop = new System.Windows.Forms.Label();
            this.lblVbtm = new System.Windows.Forms.Label();
            this.ctlTransImage = new CT30K.CTImageCanvas();
            this.fraControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // fraControl
            // 
            this.fraControl.Controls.Add(this.cmdCancel);
            this.fraControl.Controls.Add(this.cmdOK);
            this.fraControl.Controls.Add(this.lblBi);
            this.fraControl.Controls.Add(this.sldImg1);
            this.fraControl.Location = new System.Drawing.Point(0, 96);
            this.fraControl.Name = "fraControl";
            this.fraControl.Size = new System.Drawing.Size(601, 77);
            this.fraControl.TabIndex = 1;
            this.fraControl.TabStop = false;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdCancel.Location = new System.Drawing.Point(468, 28);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(105, 33);
            this.cmdCancel.TabIndex = 3;
            this.cmdCancel.Tag = "10002";
            this.cmdCancel.Text = "キャンセル";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdOK.Location = new System.Drawing.Point(344, 28);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(105, 33);
            this.cmdOK.TabIndex = 2;
            this.cmdOK.Tag = "10001";
            this.cmdOK.Text = "ＯＫ";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // lblBi
            // 
            this.lblBi.BackColor = System.Drawing.SystemColors.Window;
            this.lblBi.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F);
            this.lblBi.Location = new System.Drawing.Point(92, 12);
            this.lblBi.Name = "lblBi";
            this.lblBi.Size = new System.Drawing.Size(41, 17);
            this.lblBi.TabIndex = 1;
            this.lblBi.Text = "0";
            this.lblBi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sldImg1
            // 
            this.sldImg1.Location = new System.Drawing.Point(16, 36);
            this.sldImg1.Maximum = 255;
            this.sldImg1.Name = "sldImg1";
            this.sldImg1.Size = new System.Drawing.Size(209, 25);
            this.sldImg1.TabIndex = 4;
            this.sldImg1.ValueChanged += new System.EventHandler(this.sldImg1_ValueChanged);
            // 
            // picFrame
            // 
            this.picFrame.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picFrame.Location = new System.Drawing.Point(0, 1);
            this.picFrame.Name = "picFrame";
            this.picFrame.Size = new System.Drawing.Size(281, 81);
            this.picFrame.TabIndex = 2;
            this.picFrame.TabStop = false;
            // 
            // sldVimg1
            // 
            this.sldVimg1.LargeChange = 100;
            this.sldVimg1.Location = new System.Drawing.Point(261, 4);
            this.sldVimg1.Maximum = 32767;
            this.sldVimg1.Name = "sldVimg1";
            this.sldVimg1.Size = new System.Drawing.Size(17, 73);
            this.sldVimg1.TabIndex = 3;
            this.sldVimg1.ValueChanged += new System.EventHandler(this.sldVimg1_ValueChanged);
            // 
            // lblVtop
            // 
            this.lblVtop.BackColor = System.Drawing.SystemColors.Window;
            this.lblVtop.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F);
            this.lblVtop.Location = new System.Drawing.Point(283, 2);
            this.lblVtop.Name = "lblVtop";
            this.lblVtop.Size = new System.Drawing.Size(41, 17);
            this.lblVtop.TabIndex = 4;
            this.lblVtop.Text = "0";
            this.lblVtop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVbtm
            // 
            this.lblVbtm.BackColor = System.Drawing.SystemColors.Window;
            this.lblVbtm.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F);
            this.lblVbtm.Location = new System.Drawing.Point(283, 65);
            this.lblVbtm.Name = "lblVbtm";
            this.lblVbtm.Size = new System.Drawing.Size(41, 17);
            this.lblVbtm.TabIndex = 5;
            this.lblVbtm.Text = "0";
            this.lblVbtm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ctlTransImage
            // 
            this.ctlTransImage.BackColor = System.Drawing.Color.Black;
            this.ctlTransImage.FCDFIDRate = 0D;
            this.ctlTransImage.Location = new System.Drawing.Point(0, 0);
            this.ctlTransImage.MirrorOn = false;
            this.ctlTransImage.Name = "ctlTransImage";
            this.ctlTransImage.Picture = ((System.Drawing.Bitmap)(resources.GetObject("ctlTransImage.Picture")));
            this.ctlTransImage.Size = new System.Drawing.Size(67, 48);
            this.ctlTransImage.SizeX = 1024;
            this.ctlTransImage.SizeY = 1024;
            this.ctlTransImage.TabIndex = 6;
            // 
            // frmRotationCenterBinarized
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(617, 179);
            this.ControlBox = false;
            this.Controls.Add(this.ctlTransImage);
            this.Controls.Add(this.lblVbtm);
            this.Controls.Add(this.lblVtop);
            this.Controls.Add(this.sldVimg1);
            this.Controls.Add(this.picFrame);
            this.Controls.Add(this.fraControl);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmRotationCenterBinarized";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "回転中心校正２値化画像";
            this.Load += new System.EventHandler(this.frmRotationCenterBinarized_Load);
            this.fraControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picFrame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox fraControl;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label lblBi;
        private System.Windows.Forms.HScrollBar sldImg1;
        private System.Windows.Forms.PictureBox picFrame;
        private System.Windows.Forms.VScrollBar sldVimg1;
        private System.Windows.Forms.Label lblVtop;
        private System.Windows.Forms.Label lblVbtm;
        private CTImageCanvas ctlTransImage;
    }
}