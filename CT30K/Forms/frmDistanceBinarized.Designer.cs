namespace CT30K
{
    partial class frmDistanceBinarized
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDistanceBinarized));
            this.fraControl = new System.Windows.Forms.GroupBox();
            this.lblBi = new System.Windows.Forms.Label();
            this.sldImg1 = new System.Windows.Forms.HScrollBar();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.ＯＫ = new System.Windows.Forms.Button();
            this.ctlTransImage = new CT30K.CTImageCanvas();
            this.fraControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // fraControl
            // 
            this.fraControl.Controls.Add(this.lblBi);
            this.fraControl.Controls.Add(this.sldImg1);
            this.fraControl.Controls.Add(this.cmdCancel);
            this.fraControl.Controls.Add(this.ＯＫ);
            this.fraControl.Location = new System.Drawing.Point(8, 528);
            this.fraControl.Name = "fraControl";
            this.fraControl.Size = new System.Drawing.Size(512, 52);
            this.fraControl.TabIndex = 1;
            this.fraControl.TabStop = false;
            // 
            // lblBi
            // 
            this.lblBi.BackColor = System.Drawing.Color.White;
            this.lblBi.Location = new System.Drawing.Point(87, 8);
            this.lblBi.Name = "lblBi";
            this.lblBi.Size = new System.Drawing.Size(41, 17);
            this.lblBi.TabIndex = 10;
            this.lblBi.Text = "0";
            this.lblBi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sldImg1
            // 
            this.sldImg1.Location = new System.Drawing.Point(3, 24);
            this.sldImg1.Name = "sldImg1";
            this.sldImg1.Size = new System.Drawing.Size(209, 25);
            this.sldImg1.TabIndex = 9;
            this.sldImg1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sldImg1_Scroll);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdCancel.Location = new System.Drawing.Point(408, 13);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(97, 33);
            this.cmdCancel.TabIndex = 8;
            this.cmdCancel.Tag = "10002";
            this.cmdCancel.Text = "キャンセル";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // ＯＫ
            // 
            this.ＯＫ.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ＯＫ.Location = new System.Drawing.Point(302, 13);
            this.ＯＫ.Name = "ＯＫ";
            this.ＯＫ.Size = new System.Drawing.Size(97, 33);
            this.ＯＫ.TabIndex = 7;
            this.ＯＫ.Tag = "10001";
            this.ＯＫ.Text = "#Ｏ　Ｋ";
            this.ＯＫ.UseVisualStyleBackColor = true;
            this.ＯＫ.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // ctlTransImage
            // 
            this.ctlTransImage.BackColor = System.Drawing.Color.Black;
            this.ctlTransImage.FCDFIDRate = 0D;
            this.ctlTransImage.Location = new System.Drawing.Point(8, 9);
            this.ctlTransImage.MirrorOn = false;
            this.ctlTransImage.Name = "ctlTransImage";
            this.ctlTransImage.Picture = ((System.Drawing.Bitmap)(resources.GetObject("ctlTransImage.Picture")));
            this.ctlTransImage.Size = new System.Drawing.Size(512, 512);
            this.ctlTransImage.SizeX = 512;
            this.ctlTransImage.SizeY = 512;
            this.ctlTransImage.TabIndex = 2;
            // 
            // frmDistanceBinarized
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(527, 588);
            this.ControlBox = false;
            this.Controls.Add(this.ctlTransImage);
            this.Controls.Add(this.fraControl);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmDistanceBinarized";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "寸法校正２値化画像";
            this.Load += new System.EventHandler(this.frmDistanceBinarized_Load);
            this.fraControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox fraControl;
        private System.Windows.Forms.Label lblBi;
        private System.Windows.Forms.HScrollBar sldImg1;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button ＯＫ;
        private CTImageCanvas ctlTransImage;
    }
}