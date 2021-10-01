namespace CT30K
{
    partial class frmGainCorResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGainCorResult));
            this.fraControl = new System.Windows.Forms.GroupBox();
            this.lblWidth = new System.Windows.Forms.Label();
            this.lblWW = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblWL = new System.Windows.Forms.Label();
            this.cwsldWidth = new CT30K.CTSliderH();
            this.cwsldLevel = new CT30K.CTSliderH();
            this.cmdEnd = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.fraScale = new System.Windows.Forms.GroupBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.ctlTransImage = new CT30K.CTImageCanvas();
            this.fraControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // fraControl
            // 
            this.fraControl.Controls.Add(this.lblWidth);
            this.fraControl.Controls.Add(this.lblWW);
            this.fraControl.Controls.Add(this.lblLevel);
            this.fraControl.Controls.Add(this.lblWL);
            this.fraControl.Controls.Add(this.cwsldWidth);
            this.fraControl.Controls.Add(this.cwsldLevel);
            this.fraControl.Controls.Add(this.cmdEnd);
            this.fraControl.Controls.Add(this.cmdOK);
            this.fraControl.Controls.Add(this.fraScale);
            this.fraControl.Controls.Add(this.lblMessage);
            this.fraControl.Location = new System.Drawing.Point(8, 524);
            this.fraControl.Name = "fraControl";
            this.fraControl.Size = new System.Drawing.Size(605, 125);
            this.fraControl.TabIndex = 1;
            this.fraControl.TabStop = false;
            // 
            // lblWidth
            // 
            this.lblWidth.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 10F);
            this.lblWidth.Location = new System.Drawing.Point(140, 65);
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
            this.lblWW.Location = new System.Drawing.Point(28, 65);
            this.lblWW.Name = "lblWW";
            this.lblWW.Size = new System.Drawing.Size(85, 14);
            this.lblWW.TabIndex = 8;
            this.lblWW.Tag = "12826";
            this.lblWW.Text = "ウィンドウ幅：";
            // 
            // lblLevel
            // 
            this.lblLevel.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 10F);
            this.lblLevel.Location = new System.Drawing.Point(140, 10);
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
            this.lblWL.Tag = "12825";
            this.lblWL.Text = "ウィンドウレベル：";
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
            this.cwsldWidth.TabIndex = 4;
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
            this.cwsldLevel.TabIndex = 3;
            this.cwsldLevel.TicksLabel = CT30K.CTSliderH.TicksLabelType.TicksLabelMidOff;
            this.cwsldLevel.TicksLine = false;
            this.cwsldLevel.Value = 1;
            this.cwsldLevel.ValueChanged += new System.EventHandler(this.cwsldLevel_PointerValueChanged);
            // 
            // cmdEnd
            // 
            this.cmdEnd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdEnd.Location = new System.Drawing.Point(468, 79);
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
            this.cmdOK.Location = new System.Drawing.Point(344, 79);
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
            this.fraScale.Location = new System.Drawing.Point(228, 16);
            this.fraScale.Name = "fraScale";
            this.fraScale.Size = new System.Drawing.Size(97, 85);
            this.fraScale.TabIndex = 0;
            this.fraScale.TabStop = false;
            this.fraScale.Tag = "20183";
            this.fraScale.Text = "#倍率";
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11F);
            this.lblMessage.Location = new System.Drawing.Point(340, 12);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(243, 72);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.Tag = "12117";
            this.lblMessage.Text = "#ゲイン校正データ収集が完了しました。ゲイン校正画像結果を保存しますか？";
            // 
            // ctlTransImage
            // 
            this.ctlTransImage.BackColor = System.Drawing.Color.Black;
            this.ctlTransImage.FCDFIDRate = 0D;
            this.ctlTransImage.Location = new System.Drawing.Point(0, 0);
            this.ctlTransImage.MirrorOn = false;
            this.ctlTransImage.Name = "ctlTransImage";
            this.ctlTransImage.Picture = ((System.Drawing.Bitmap)(resources.GetObject("ctlTransImage.Picture")));
            this.ctlTransImage.Size = new System.Drawing.Size(722, 484);
            this.ctlTransImage.SizeX = 0;
            this.ctlTransImage.SizeY = 0;
            this.ctlTransImage.TabIndex = 0;
            // 
            // frmGainCorResult
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(849, 673);
            this.ControlBox = false;
            this.Controls.Add(this.fraControl);
            this.Controls.Add(this.ctlTransImage);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmGainCorResult";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "10904";
            this.Text = "ゲイン校正結果";
            this.Load += new System.EventHandler(this.frmGainCorResult_Load);
            this.fraControl.ResumeLayout(false);
            this.fraControl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox fraControl;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.Label lblWW;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Label lblWL;
        private System.Windows.Forms.Button cmdEnd;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.GroupBox fraScale;
        private System.Windows.Forms.Label lblMessage;
        private CTImageCanvas ctlTransImage;
        private CTSliderH cwsldWidth;
        private CTSliderH cwsldLevel;
    }
}