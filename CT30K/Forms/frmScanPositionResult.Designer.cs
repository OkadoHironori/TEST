namespace CT30K
{
    partial class frmScanPositionResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScanPositionResult));
            this.fraControl = new System.Windows.Forms.GroupBox();
            this.cwsldWidth = new CT30K.CTSliderH();
            this.cwsldLevel = new CT30K.CTSliderH();
            this.lblWidth = new System.Windows.Forms.Label();
            this.lblWW = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblWL = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.cmdEnd = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.fraScale = new System.Windows.Forms.GroupBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.lblPosiAB = new System.Windows.Forms.Label();
            this.lblSa = new System.Windows.Forms.Label();
            this.lblScanPosiB = new System.Windows.Forms.Label();
            this.lblPosibtitle = new System.Windows.Forms.Label();
            this.lblScanPosiA = new System.Windows.Forms.Label();
            this.lblPosiaTitle = new System.Windows.Forms.Label();
            this.lblColon0 = new System.Windows.Forms.Label();
            this.lblColon1 = new System.Windows.Forms.Label();
            this.lblColon3 = new System.Windows.Forms.Label();
            this.ctlTransImage = new CT30K.CTImageCanvas();
            this.fraControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // fraControl
            // 
            this.fraControl.Controls.Add(this.cwsldWidth);
            this.fraControl.Controls.Add(this.cwsldLevel);
            this.fraControl.Controls.Add(this.lblWidth);
            this.fraControl.Controls.Add(this.lblWW);
            this.fraControl.Controls.Add(this.lblLevel);
            this.fraControl.Controls.Add(this.lblWL);
            this.fraControl.Controls.Add(this.lblMessage);
            this.fraControl.Controls.Add(this.cmdEnd);
            this.fraControl.Controls.Add(this.cmdOK);
            this.fraControl.Controls.Add(this.fraScale);
            this.fraControl.Location = new System.Drawing.Point(1, 216);
            this.fraControl.Name = "fraControl";
            this.fraControl.Size = new System.Drawing.Size(605, 121);
            this.fraControl.TabIndex = 0;
            this.fraControl.TabStop = false;
            // 
            // cwsldWidth
            // 
            this.cwsldWidth.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwsldWidth.LargeChange = 10;
            this.cwsldWidth.Location = new System.Drawing.Point(20, 83);
            this.cwsldWidth.Maximum = 256;
            this.cwsldWidth.Minimum = 1;
            this.cwsldWidth.Name = "cwsldWidth";
            this.cwsldWidth.Reverse = false;
            this.cwsldWidth.Size = new System.Drawing.Size(197, 33);
            this.cwsldWidth.SmallChange = 1;
            this.cwsldWidth.TabIndex = 131;
            this.cwsldWidth.TicksLabel = CT30K.CTSliderH.TicksLabelType.TicksLabelMidOff;
            this.cwsldWidth.TicksLine = false;
            this.cwsldWidth.Value = 1;
            this.cwsldWidth.ValueChanged += new System.EventHandler(this.cwsldWidth_PointerValueChanged);
            // 
            // cwsldLevel
            // 
            this.cwsldLevel.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwsldLevel.LargeChange = 10;
            this.cwsldLevel.Location = new System.Drawing.Point(20, 28);
            this.cwsldLevel.Maximum = 255;
            this.cwsldLevel.Minimum = 0;
            this.cwsldLevel.Name = "cwsldLevel";
            this.cwsldLevel.Reverse = false;
            this.cwsldLevel.Size = new System.Drawing.Size(197, 33);
            this.cwsldLevel.SmallChange = 1;
            this.cwsldLevel.TabIndex = 132;
            this.cwsldLevel.TicksLabel = CT30K.CTSliderH.TicksLabelType.TicksLabelMidOff;
            this.cwsldLevel.TicksLine = false;
            this.cwsldLevel.Value = 1;
            this.cwsldLevel.ValueChanged += new System.EventHandler(this.cwsldLevel_PointerValueChanged);
            // 
            // lblWidth
            // 
            this.lblWidth.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 10F);
            this.lblWidth.Location = new System.Drawing.Point(140, 63);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(49, 14);
            this.lblWidth.TabIndex = 9;
            this.lblWidth.Text = "256";
            this.lblWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWW
            // 
            this.lblWW.AutoSize = true;
            this.lblWW.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 10F);
            this.lblWW.Location = new System.Drawing.Point(28, 60);
            this.lblWW.Name = "lblWW";
            this.lblWW.Size = new System.Drawing.Size(85, 14);
            this.lblWW.TabIndex = 8;
            this.lblWW.Text = "ウィンドウ幅：";
            // 
            // lblLevel
            // 
            this.lblLevel.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 10F);
            this.lblLevel.Location = new System.Drawing.Point(140, 9);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(49, 14);
            this.lblLevel.TabIndex = 7;
            this.lblLevel.Text = "128";
            this.lblLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblWL
            // 
            this.lblWL.AutoSize = true;
            this.lblWL.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 10F);
            this.lblWL.Location = new System.Drawing.Point(28, 10);
            this.lblWL.Name = "lblWL";
            this.lblWL.Size = new System.Drawing.Size(110, 14);
            this.lblWL.TabIndex = 6;
            this.lblWL.Text = "ウィンドウレベル：";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblMessage.Location = new System.Drawing.Point(375, 24);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(167, 16);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.Tag = "12108";
            this.lblMessage.Text = "#結果を保存しますか？";
            // 
            // cmdEnd
            // 
            this.cmdEnd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdEnd.Location = new System.Drawing.Point(468, 64);
            this.cmdEnd.Name = "cmdEnd";
            this.cmdEnd.Size = new System.Drawing.Size(105, 33);
            this.cmdEnd.TabIndex = 2;
            this.cmdEnd.Tag = "10004";
            this.cmdEnd.Text = "#いいえ";
            this.cmdEnd.UseVisualStyleBackColor = true;
            this.cmdEnd.Click += new System.EventHandler(this.cmdEnd_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdOK.Location = new System.Drawing.Point(344, 64);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(105, 33);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Tag = "10003";
            this.cmdOK.Text = "#はい";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // fraScale
            // 
            this.fraScale.Location = new System.Drawing.Point(224, 20);
            this.fraScale.Name = "fraScale";
            this.fraScale.Size = new System.Drawing.Size(97, 85);
            this.fraScale.TabIndex = 0;
            this.fraScale.TabStop = false;
            this.fraScale.Tag = "20183";
            this.fraScale.Text = "#倍率";
            // 
            // Label1
            // 
            this.Label1.BackColor = System.Drawing.Color.Transparent;
            this.Label1.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(0)))), ((int)(((byte)(198)))));
            this.Label1.Location = new System.Drawing.Point(12, 58);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(93, 40);
            this.Label1.TabIndex = 21;
            this.Label1.Tag = "12599";
            this.Label1.Text = "Vertical diffrende";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Label1.Visible = false;
            // 
            // lblPosiAB
            // 
            this.lblPosiAB.BackColor = System.Drawing.Color.White;
            this.lblPosiAB.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblPosiAB.ForeColor = System.Drawing.Color.Black;
            this.lblPosiAB.Location = new System.Drawing.Point(111, 65);
            this.lblPosiAB.Name = "lblPosiAB";
            this.lblPosiAB.Size = new System.Drawing.Size(81, 17);
            this.lblPosiAB.TabIndex = 20;
            this.lblPosiAB.Text = "-000.0000";
            this.lblPosiAB.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSa
            // 
            this.lblSa.BackColor = System.Drawing.Color.Transparent;
            this.lblSa.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblSa.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(0)))), ((int)(((byte)(198)))));
            this.lblSa.Location = new System.Drawing.Point(31, 65);
            this.lblSa.Name = "lblSa";
            this.lblSa.Size = new System.Drawing.Size(65, 40);
            this.lblSa.TabIndex = 18;
            this.lblSa.Tag = "12599";
            this.lblSa.Text = "#上下差";
            this.lblSa.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblScanPosiB
            // 
            this.lblScanPosiB.BackColor = System.Drawing.Color.White;
            this.lblScanPosiB.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblScanPosiB.ForeColor = System.Drawing.Color.Black;
            this.lblScanPosiB.Location = new System.Drawing.Point(111, 41);
            this.lblScanPosiB.Name = "lblScanPosiB";
            this.lblScanPosiB.Size = new System.Drawing.Size(81, 17);
            this.lblScanPosiB.TabIndex = 17;
            this.lblScanPosiB.Text = "-000.0000";
            this.lblScanPosiB.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPosibtitle
            // 
            this.lblPosibtitle.AutoSize = true;
            this.lblPosibtitle.BackColor = System.Drawing.Color.Transparent;
            this.lblPosibtitle.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblPosibtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(0)))), ((int)(((byte)(198)))));
            this.lblPosibtitle.Location = new System.Drawing.Point(33, 41);
            this.lblPosibtitle.Name = "lblPosibtitle";
            this.lblPosibtitle.Size = new System.Drawing.Size(63, 15);
            this.lblPosibtitle.TabIndex = 15;
            this.lblPosibtitle.Tag = "12600";
            this.lblPosibtitle.Text = "#切　片";
            this.lblPosibtitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblScanPosiA
            // 
            this.lblScanPosiA.BackColor = System.Drawing.Color.White;
            this.lblScanPosiA.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblScanPosiA.ForeColor = System.Drawing.Color.Black;
            this.lblScanPosiA.Location = new System.Drawing.Point(111, 17);
            this.lblScanPosiA.Name = "lblScanPosiA";
            this.lblScanPosiA.Size = new System.Drawing.Size(81, 17);
            this.lblScanPosiA.TabIndex = 14;
            this.lblScanPosiA.Text = "-000.0000";
            this.lblScanPosiA.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPosiaTitle
            // 
            this.lblPosiaTitle.AutoSize = true;
            this.lblPosiaTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblPosiaTitle.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblPosiaTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(0)))), ((int)(((byte)(198)))));
            this.lblPosiaTitle.Location = new System.Drawing.Point(33, 17);
            this.lblPosiaTitle.Name = "lblPosiaTitle";
            this.lblPosiaTitle.Size = new System.Drawing.Size(63, 15);
            this.lblPosiaTitle.TabIndex = 12;
            this.lblPosiaTitle.Tag = "12598";
            this.lblPosiaTitle.Text = "#傾　き";
            this.lblPosiaTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblColon0
            // 
            this.lblColon0.AutoSize = true;
            this.lblColon0.BackColor = System.Drawing.Color.Transparent;
            this.lblColon0.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblColon0.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(0)))), ((int)(((byte)(198)))));
            this.lblColon0.Location = new System.Drawing.Point(95, 17);
            this.lblColon0.Name = "lblColon0";
            this.lblColon0.Size = new System.Drawing.Size(15, 15);
            this.lblColon0.TabIndex = 22;
            this.lblColon0.Tag = "";
            this.lblColon0.Text = ":";
            this.lblColon0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblColon1
            // 
            this.lblColon1.AutoSize = true;
            this.lblColon1.BackColor = System.Drawing.Color.Transparent;
            this.lblColon1.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblColon1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(0)))), ((int)(((byte)(198)))));
            this.lblColon1.Location = new System.Drawing.Point(95, 41);
            this.lblColon1.Name = "lblColon1";
            this.lblColon1.Size = new System.Drawing.Size(15, 15);
            this.lblColon1.TabIndex = 23;
            this.lblColon1.Tag = "";
            this.lblColon1.Text = ":";
            this.lblColon1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblColon3
            // 
            this.lblColon3.AutoSize = true;
            this.lblColon3.BackColor = System.Drawing.Color.Transparent;
            this.lblColon3.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F);
            this.lblColon3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(0)))), ((int)(((byte)(198)))));
            this.lblColon3.Location = new System.Drawing.Point(95, 65);
            this.lblColon3.Name = "lblColon3";
            this.lblColon3.Size = new System.Drawing.Size(15, 15);
            this.lblColon3.TabIndex = 24;
            this.lblColon3.Tag = "";
            this.lblColon3.Text = ":";
            this.lblColon3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ctlTransImage
            // 
            this.ctlTransImage.BackColor = System.Drawing.Color.Black;
            this.ctlTransImage.FCDFIDRate = 0D;
            this.ctlTransImage.Location = new System.Drawing.Point(5, 7);
            this.ctlTransImage.MirrorOn = false;
            this.ctlTransImage.Name = "ctlTransImage";
            this.ctlTransImage.Picture = ((System.Drawing.Bitmap)(resources.GetObject("ctlTransImage.Picture")));
            this.ctlTransImage.Size = new System.Drawing.Size(262, 140);
            this.ctlTransImage.SizeX = 1024;
            this.ctlTransImage.SizeY = 1024;
            this.ctlTransImage.TabIndex = 25;
            this.ctlTransImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ctlTransImage_MouseDown);
            this.ctlTransImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ctlTransImage_MouseMove);
            this.ctlTransImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ctlTransImage_MouseUp);
            // 
            // frmScanPositionResult
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(629, 356);
            this.ControlBox = false;
            this.Controls.Add(this.fraControl);
            this.Controls.Add(this.lblColon3);
            this.Controls.Add(this.lblSa);
            this.Controls.Add(this.lblColon1);
            this.Controls.Add(this.lblColon0);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.lblPosiAB);
            this.Controls.Add(this.lblScanPosiB);
            this.Controls.Add(this.lblPosibtitle);
            this.Controls.Add(this.lblScanPosiA);
            this.Controls.Add(this.lblPosiaTitle);
            this.Controls.Add(this.ctlTransImage);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmScanPositionResult";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmScanPositionResult";
            this.Load += new System.EventHandler(this.frmScanPositionResult_Load);
            this.fraControl.ResumeLayout(false);
            this.fraControl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox fraControl;
        private System.Windows.Forms.GroupBox fraScale;
        private System.Windows.Forms.Button cmdEnd;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Label lblWL;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.Label lblWW;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Label lblPosiAB;
        private System.Windows.Forms.Label lblSa;
        private System.Windows.Forms.Label lblScanPosiB;
        private System.Windows.Forms.Label lblPosibtitle;
        private System.Windows.Forms.Label lblScanPosiA;
        private System.Windows.Forms.Label lblPosiaTitle;
        private System.Windows.Forms.Label lblColon0;
        private System.Windows.Forms.Label lblColon1;
        private System.Windows.Forms.Label lblColon3;
        private CTImageCanvas ctlTransImage;
        private CTSliderH cwsldWidth;
        private CTSliderH cwsldLevel;
    }
}