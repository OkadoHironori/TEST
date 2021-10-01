namespace CT30K
{
    partial class frmInputRoiData
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
            this.lblRoiNo = new System.Windows.Forms.Label();
            this.CWRoiNo = new System.Windows.Forms.NumericUpDown();
            this.cmbRoiShape = new System.Windows.Forms.ComboBox();
            this.lblRoiShape = new System.Windows.Forms.Label();
            this.fraRoiInf = new System.Windows.Forms.GroupBox();
            this.ntbValue3 = new CT30K.NumTextBox();
            this.ntbValue2 = new CT30K.NumTextBox();
            this.ntbValue1 = new CT30K.NumTextBox();
            this.ntbValue0 = new CT30K.NumTextBox();
            this.cmdIndicate = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.CWRoiNo)).BeginInit();
            this.fraRoiInf.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblRoiNo
            // 
            this.lblRoiNo.AutoSize = true;
            this.lblRoiNo.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblRoiNo.Location = new System.Drawing.Point(92, 20);
            this.lblRoiNo.Name = "lblRoiNo";
            this.lblRoiNo.Size = new System.Drawing.Size(73, 16);
            this.lblRoiNo.TabIndex = 0;
            this.lblRoiNo.Text = "ROI番号：";
            // 
            // CWRoiNo
            // 
            this.CWRoiNo.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 13F);
            this.CWRoiNo.Location = new System.Drawing.Point(160, 16);
            this.CWRoiNo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CWRoiNo.Name = "CWRoiNo";
            this.CWRoiNo.Size = new System.Drawing.Size(73, 25);
            this.CWRoiNo.TabIndex = 1;
            this.CWRoiNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.CWRoiNo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CWRoiNo.ValueChanged += new System.EventHandler(this.CWRoiNo_ValueChanged);
            // 
            // cmbRoiShape
            // 
            this.cmbRoiShape.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmbRoiShape.FormattingEnabled = true;
            this.cmbRoiShape.Location = new System.Drawing.Point(160, 48);
            this.cmbRoiShape.Name = "cmbRoiShape";
            this.cmbRoiShape.Size = new System.Drawing.Size(133, 24);
            this.cmbRoiShape.TabIndex = 2;
            this.cmbRoiShape.SelectedIndexChanged += new System.EventHandler(this.cmbRoiShape_SelectedIndexChanged);
            // 
            // lblRoiShape
            // 
            this.lblRoiShape.AutoSize = true;
            this.lblRoiShape.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblRoiShape.Location = new System.Drawing.Point(92, 51);
            this.lblRoiShape.Name = "lblRoiShape";
            this.lblRoiShape.Size = new System.Drawing.Size(73, 16);
            this.lblRoiShape.TabIndex = 3;
            this.lblRoiShape.Text = "ROI形状：";
            // 
            // fraRoiInf
            // 
            this.fraRoiInf.Controls.Add(this.ntbValue3);
            this.fraRoiInf.Controls.Add(this.ntbValue2);
            this.fraRoiInf.Controls.Add(this.ntbValue1);
            this.fraRoiInf.Controls.Add(this.ntbValue0);
            this.fraRoiInf.Location = new System.Drawing.Point(16, 80);
            this.fraRoiInf.Name = "fraRoiInf";
            this.fraRoiInf.Size = new System.Drawing.Size(301, 142);
            this.fraRoiInf.TabIndex = 4;
            this.fraRoiInf.TabStop = false;
            // 
            // ntbValue3
            // 
            this.ntbValue3.BackColor = System.Drawing.SystemColors.Control;
            this.ntbValue3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ntbValue3.Caption = "#Y方向の大きさ YL";
            this.ntbValue3.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.ntbValue3.CaptionFont = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbValue3.CaptionWidth = 200;
            this.ntbValue3.DiscreteInterval = 1F;
            this.ntbValue3.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbValue3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbValue3.IncDecButton = false;
            this.ntbValue3.Location = new System.Drawing.Point(16, 108);
            this.ntbValue3.Max = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.ntbValue3.Min = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ntbValue3.Name = "ntbValue3";
            this.ntbValue3.ReadOnly = false;
            this.ntbValue3.Size = new System.Drawing.Size(267, 24);
            this.ntbValue3.TabIndex = 3;
            this.ntbValue3.Tag = "12609";
            this.ntbValue3.TextBackColor = System.Drawing.SystemColors.Window;
            this.ntbValue3.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbValue3.Unit = "";
            this.ntbValue3.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ntbValue2
            // 
            this.ntbValue2.BackColor = System.Drawing.SystemColors.Control;
            this.ntbValue2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ntbValue2.Caption = "#大きさ     Ro";
            this.ntbValue2.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.ntbValue2.CaptionFont = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbValue2.CaptionWidth = 200;
            this.ntbValue2.DiscreteInterval = 1F;
            this.ntbValue2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbValue2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbValue2.IncDecButton = false;
            this.ntbValue2.Location = new System.Drawing.Point(16, 78);
            this.ntbValue2.Max = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.ntbValue2.Min = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ntbValue2.Name = "ntbValue2";
            this.ntbValue2.ReadOnly = false;
            this.ntbValue2.Size = new System.Drawing.Size(267, 24);
            this.ntbValue2.TabIndex = 2;
            this.ntbValue2.Tag = "12611";
            this.ntbValue2.TextBackColor = System.Drawing.SystemColors.Window;
            this.ntbValue2.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbValue2.Unit = "";
            this.ntbValue2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ntbValue1
            // 
            this.ntbValue1.BackColor = System.Drawing.SystemColors.Control;
            this.ntbValue1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ntbValue1.Caption = "YC";
            this.ntbValue1.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.ntbValue1.CaptionFont = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbValue1.CaptionWidth = 200;
            this.ntbValue1.DiscreteInterval = 1F;
            this.ntbValue1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbValue1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbValue1.IncDecButton = false;
            this.ntbValue1.Location = new System.Drawing.Point(16, 48);
            this.ntbValue1.Max = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.ntbValue1.Min = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ntbValue1.Name = "ntbValue1";
            this.ntbValue1.ReadOnly = false;
            this.ntbValue1.Size = new System.Drawing.Size(267, 24);
            this.ntbValue1.TabIndex = 1;
            this.ntbValue1.TextBackColor = System.Drawing.SystemColors.Window;
            this.ntbValue1.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbValue1.Unit = "";
            this.ntbValue1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ntbValue0
            // 
            this.ntbValue0.BackColor = System.Drawing.SystemColors.Control;
            this.ntbValue0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ntbValue0.Caption = "#中心座標 XC";
            this.ntbValue0.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.ntbValue0.CaptionFont = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbValue0.CaptionWidth = 200;
            this.ntbValue0.DiscreteInterval = 1F;
            this.ntbValue0.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ntbValue0.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ntbValue0.IncDecButton = false;
            this.ntbValue0.Location = new System.Drawing.Point(16, 16);
            this.ntbValue0.Max = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.ntbValue0.Min = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ntbValue0.Name = "ntbValue0";
            this.ntbValue0.ReadOnly = false;
            this.ntbValue0.Size = new System.Drawing.Size(267, 24);
            this.ntbValue0.TabIndex = 0;
            this.ntbValue0.Tag = "12612";
            this.ntbValue0.TextBackColor = System.Drawing.SystemColors.Window;
            this.ntbValue0.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.ntbValue0.Unit = "";
            this.ntbValue0.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cmdIndicate
            // 
            this.cmdIndicate.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdIndicate.Location = new System.Drawing.Point(222, 240);
            this.cmdIndicate.Name = "cmdIndicate";
            this.cmdIndicate.Size = new System.Drawing.Size(97, 33);
            this.cmdIndicate.TabIndex = 7;
            this.cmdIndicate.Tag = "10011";
            this.cmdIndicate.Text = "#表　示";
            this.cmdIndicate.UseVisualStyleBackColor = true;
            this.cmdIndicate.Click += new System.EventHandler(this.cmdIndicate_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdCancel.Location = new System.Drawing.Point(116, 240);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(97, 33);
            this.cmdCancel.TabIndex = 6;
            this.cmdCancel.Tag = "10002";
            this.cmdCancel.Text = "#キャンセル";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdOK.Location = new System.Drawing.Point(10, 240);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(97, 33);
            this.cmdOK.TabIndex = 5;
            this.cmdOK.Tag = "10001";
            this.cmdOK.Text = "#Ｏ　Ｋ";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // frmInputRoiData
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(328, 289);
            this.ControlBox = false;
            this.Controls.Add(this.cmdIndicate);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.fraRoiInf);
            this.Controls.Add(this.cmbRoiShape);
            this.Controls.Add(this.CWRoiNo);
            this.Controls.Add(this.lblRoiNo);
            this.Controls.Add(this.lblRoiShape);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmInputRoiData";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ROI数値入力";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmInputRoiData_FormClosed);
            this.Load += new System.EventHandler(this.frmInputRoiData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CWRoiNo)).EndInit();
            this.fraRoiInf.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblRoiNo;
        private System.Windows.Forms.NumericUpDown CWRoiNo;
        private System.Windows.Forms.ComboBox cmbRoiShape;
        private System.Windows.Forms.Label lblRoiShape;
        private System.Windows.Forms.GroupBox fraRoiInf;
        private System.Windows.Forms.Button cmdIndicate;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private NumTextBox ntbValue0;
        private NumTextBox ntbValue1;
        private NumTextBox ntbValue3;
        private NumTextBox ntbValue2;
    }
}