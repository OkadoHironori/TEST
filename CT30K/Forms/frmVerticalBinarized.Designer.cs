namespace CT30K
{
    partial class frmVerticalBinarized
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmVerticalBinarized));
            this.fraControl = new System.Windows.Forms.GroupBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.lblBi = new System.Windows.Forms.Label();
            this.sldImg1 = new System.Windows.Forms.HScrollBar();
            this.ctlTransImage = new CT30K.CTImageCanvas();
            this.fraControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // fraControl
            // 
            this.fraControl.Controls.Add(this.cmdCancel);
            this.fraControl.Controls.Add(this.cmdOK);
            this.fraControl.Controls.Add(this.lblBi);
            this.fraControl.Controls.Add(this.sldImg1);
            this.fraControl.Location = new System.Drawing.Point(40, 144);
            this.fraControl.Name = "fraControl";
            this.fraControl.Size = new System.Drawing.Size(533, 65);
            this.fraControl.TabIndex = 0;
            this.fraControl.TabStop = false;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdCancel.Location = new System.Drawing.Point(412, 16);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(105, 33);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Tag = "10002";
            this.cmdCancel.Text = "キャンセル";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdOK.Location = new System.Drawing.Point(284, 16);
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
            this.lblBi.Location = new System.Drawing.Point(89, 8);
            this.lblBi.Name = "lblBi";
            this.lblBi.Size = new System.Drawing.Size(41, 17);
            this.lblBi.TabIndex = 4;
            this.lblBi.Text = "0";
            this.lblBi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sldImg1
            // 
            this.sldImg1.Location = new System.Drawing.Point(8, 28);
            this.sldImg1.Maximum = 255;
            this.sldImg1.Name = "sldImg1";
            this.sldImg1.Size = new System.Drawing.Size(209, 25);
            this.sldImg1.TabIndex = 3;
            this.sldImg1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sldImg1_Scroll);
            // 
            // ctlTransImage
            // 
            this.ctlTransImage.BackColor = System.Drawing.Color.Black;
            this.ctlTransImage.FCDFIDRate = 0D;
            this.ctlTransImage.Location = new System.Drawing.Point(0, 0);
            this.ctlTransImage.MirrorOn = false;
            this.ctlTransImage.Name = "ctlTransImage";
            this.ctlTransImage.Picture = ((System.Drawing.Bitmap)(resources.GetObject("ctlTransImage.Picture")));
            this.ctlTransImage.Size = new System.Drawing.Size(170, 109);
            this.ctlTransImage.SizeX = 1024;
            this.ctlTransImage.SizeY = 1024;
            this.ctlTransImage.TabIndex = 1;
            // 
            // frmVerticalBinarized
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(608, 217);
            this.ControlBox = false;
            this.Controls.Add(this.ctlTransImage);
            this.Controls.Add(this.fraControl);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmVerticalBinarized";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "幾何歪校正２値化画像";
            this.Load += new System.EventHandler(this.frmVerticalBinarized_Load);
            this.fraControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox fraControl;
        private System.Windows.Forms.HScrollBar sldImg1;
        private System.Windows.Forms.Label lblBi;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private CTImageCanvas ctlTransImage;
    }
}