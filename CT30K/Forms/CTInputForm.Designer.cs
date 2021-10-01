namespace CT30K
{
    partial class CTInputForm
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
            this.cmdOk = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdDisp = new System.Windows.Forms.Button();
            this.ntbBias = new CT30K.NumTextBox();
            this.ntbInterval = new CT30K.NumTextBox();
            this.SuspendLayout();
            // 
            // cmdOk
            // 
            this.cmdOk.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdOk.Location = new System.Drawing.Point(20, 108);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(97, 33);
            this.cmdOk.TabIndex = 2;
            this.cmdOk.Tag = "10001";
            this.cmdOk.Text = "#Ｏ　Ｋ";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdCancel.Location = new System.Drawing.Point(126, 108);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(97, 33);
            this.cmdCancel.TabIndex = 3;
            this.cmdCancel.Tag = "10002";
            this.cmdCancel.Text = "#キャンセル";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdDisp
            // 
            this.cmdDisp.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdDisp.Location = new System.Drawing.Point(232, 108);
            this.cmdDisp.Name = "cmdDisp";
            this.cmdDisp.Size = new System.Drawing.Size(97, 33);
            this.cmdDisp.TabIndex = 4;
            this.cmdDisp.Tag = "10011";
            this.cmdDisp.Text = "#表　示";
            this.cmdDisp.UseVisualStyleBackColor = true;
            this.cmdDisp.Click += new System.EventHandler(this.cmdDisp_Click);
            // 
            // ntbBias
            // 
            this.ntbBias.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ntbBias.Caption = "#中央値";
            this.ntbBias.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.ntbBias.CaptionFont = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbBias.CaptionWidth = 65;
            this.ntbBias.DiscreteInterval = 1F;
            this.ntbBias.IncDecButton = false;
            this.ntbBias.Location = new System.Drawing.Point(96, 20);
            this.ntbBias.Max = new decimal(new int[] {
            16000,
            0,
            0,
            0});
            this.ntbBias.Min = new decimal(new int[] {
            16000,
            0,
            0,
            -2147483648});
            this.ntbBias.Name = "ntbBias";
            this.ntbBias.Size = new System.Drawing.Size(145, 24);
            this.ntbBias.TabIndex = 5;
            this.ntbBias.Tag = "12403";
            this.ntbBias.Unit = "";
            this.ntbBias.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // ntbInterval
            // 
            this.ntbInterval.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ntbInterval.Caption = "#１ 目盛";
            this.ntbInterval.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.ntbInterval.CaptionFont = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbInterval.CaptionWidth = 65;
            this.ntbInterval.DiscreteInterval = 1F;
            this.ntbInterval.IncDecButton = false;
            this.ntbInterval.Location = new System.Drawing.Point(96, 52);
            this.ntbInterval.Max = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.ntbInterval.Min = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ntbInterval.Name = "ntbInterval";
            this.ntbInterval.Size = new System.Drawing.Size(145, 24);
            this.ntbInterval.TabIndex = 6;
            this.ntbInterval.Tag = "12401";
            this.ntbInterval.Unit = "";
            this.ntbInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // CTInputForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(348, 156);
            this.ControlBox = false;
            this.Controls.Add(this.ntbInterval);
            this.Controls.Add(this.ntbBias);
            this.Controls.Add(this.cmdDisp);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOk);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CTInputForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Tag = "12446";
            this.Text = "#CT値目盛入力";
            this.Load += new System.EventHandler(this.Form_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdOk;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdDisp;
        private NumTextBox ntbBias;
        internal NumTextBox ntbInterval;
    }
}