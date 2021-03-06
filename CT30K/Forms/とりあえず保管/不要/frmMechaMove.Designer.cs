namespace CT30K
{
	partial class frmMechaMove
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
			this.Label1 = new System.Windows.Forms.Label();
			this.ntbMove0 = new CT30K.NumTextBox();
			this.ntbMove1 = new CT30K.NumTextBox();
			this.ntbMove2 = new CT30K.NumTextBox();
			this.ntbMove3 = new CT30K.NumTextBox();
			this.ntbMove4 = new CT30K.NumTextBox();
			this.ntbMove5 = new CT30K.NumTextBox();
			this.chkXYMove = new System.Windows.Forms.CheckBox();
			this.lblPrompt = new System.Windows.Forms.Label();
			this.cmdClose = new System.Windows.Forms.Button();
			this.ntbFCD1st = new CT30K.NumTextBox();
			this.ntbY1st = new CT30K.NumTextBox();
			this.Image1 = new System.Windows.Forms.PictureBox();
			this.cwbtnMove = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.Image1)).BeginInit();
			this.SuspendLayout();
			// 
			// Label1
			// 
			this.Label1.Location = new System.Drawing.Point(44, 8);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(232, 28);
			this.Label1.TabIndex = 0;
			this.Label1.Tag = "12140";
			this.Label1.Text = "#下記指定位置までテーブル等を移動します。";
			// 
			// ntbMove0
			// 
			this.ntbMove0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.ntbMove0.Caption = "微調X軸";
			this.ntbMove0.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.ntbMove0.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F);
			this.ntbMove0.CaptionWidth = 100;
			this.ntbMove0.DiscreteInterval = 0.01F;
			this.ntbMove0.Font = new System.Drawing.Font("MS UI Gothic", 10F);
			this.ntbMove0.IncDecButton = false;
			this.ntbMove0.Location = new System.Drawing.Point(4, 40);
			this.ntbMove0.Max = new decimal(new int[] {
            -10,
            4,
            0,
            196608});
			this.ntbMove0.Min = new decimal(new int[] {
            0,
            5,
            0,
            -2147287040});
			this.ntbMove0.Name = "ntbMove0";
			this.ntbMove0.Size = new System.Drawing.Size(203, 20);
			this.ntbMove0.TabIndex = 1;
			this.ntbMove0.Unit = "mm";
			this.ntbMove0.Value = new decimal(new int[] {
            999999,
            0,
            0,
            131072});
			// 
			// ntbMove1
			// 
			this.ntbMove1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.ntbMove1.Caption = "微調Y軸";
			this.ntbMove1.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.ntbMove1.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F);
			this.ntbMove1.CaptionWidth = 100;
			this.ntbMove1.DiscreteInterval = 0.01F;
			this.ntbMove1.Font = new System.Drawing.Font("MS UI Gothic", 10F);
			this.ntbMove1.IncDecButton = false;
			this.ntbMove1.Location = new System.Drawing.Point(4, 60);
			this.ntbMove1.Max = new decimal(new int[] {
            -10,
            4,
            0,
            196608});
			this.ntbMove1.Min = new decimal(new int[] {
            0,
            5,
            0,
            -2147287040});
			this.ntbMove1.Name = "ntbMove1";
			this.ntbMove1.Size = new System.Drawing.Size(203, 20);
			this.ntbMove1.TabIndex = 2;
			this.ntbMove1.Unit = "mm";
			this.ntbMove1.Value = new decimal(new int[] {
            999999,
            0,
            0,
            131072});
			// 
			// ntbMove2
			// 
			this.ntbMove2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.ntbMove2.Caption = "FCD";
			this.ntbMove2.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.ntbMove2.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F);
			this.ntbMove2.CaptionWidth = 100;
			this.ntbMove2.DiscreteInterval = 0.1F;
			this.ntbMove2.Font = new System.Drawing.Font("MS UI Gothic", 10F);
			this.ntbMove2.IncDecButton = false;
			this.ntbMove2.Location = new System.Drawing.Point(4, 80);
			this.ntbMove2.Max = new decimal(new int[] {
            2147483647,
            0,
            0,
            65536});
			this.ntbMove2.Min = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147418112});
			this.ntbMove2.Name = "ntbMove2";
			this.ntbMove2.Size = new System.Drawing.Size(203, 20);
			this.ntbMove2.TabIndex = 3;
			this.ntbMove2.Unit = "mm";
			this.ntbMove2.Value = new decimal(new int[] {
            999999,
            0,
            0,
            65536});
			// 
			// ntbMove3
			// 
			this.ntbMove3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.ntbMove3.Caption = "FDD";
			this.ntbMove3.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.ntbMove3.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F);
			this.ntbMove3.CaptionWidth = 100;
			this.ntbMove3.DiscreteInterval = 0.1F;
			this.ntbMove3.Font = new System.Drawing.Font("MS UI Gothic", 10F);
			this.ntbMove3.IncDecButton = false;
			this.ntbMove3.Location = new System.Drawing.Point(4, 100);
			this.ntbMove3.Max = new decimal(new int[] {
            2147483647,
            0,
            0,
            65536});
			this.ntbMove3.Min = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147418112});
			this.ntbMove3.Name = "ntbMove3";
			this.ntbMove3.Size = new System.Drawing.Size(203, 20);
			this.ntbMove3.TabIndex = 4;
			this.ntbMove3.Unit = "mm";
			this.ntbMove3.Value = new decimal(new int[] {
            999999,
            0,
            0,
            65536});
			// 
			// ntbMove4
			// 
			this.ntbMove4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.ntbMove4.Caption = "Y軸";
			this.ntbMove4.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.ntbMove4.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F);
			this.ntbMove4.CaptionWidth = 100;
			this.ntbMove4.DiscreteInterval = 0.01F;
			this.ntbMove4.Font = new System.Drawing.Font("MS UI Gothic", 10F);
			this.ntbMove4.IncDecButton = false;
			this.ntbMove4.Location = new System.Drawing.Point(4, 120);
			this.ntbMove4.Max = new decimal(new int[] {
            -10,
            4,
            0,
            196608});
			this.ntbMove4.Min = new decimal(new int[] {
            0,
            5,
            0,
            -2147287040});
			this.ntbMove4.Name = "ntbMove4";
			this.ntbMove4.Size = new System.Drawing.Size(203, 20);
			this.ntbMove4.TabIndex = 5;
			this.ntbMove4.Unit = "mm";
			this.ntbMove4.Value = new decimal(new int[] {
            999999,
            0,
            0,
            131072});
			// 
			// ntbMove5
			// 
			this.ntbMove5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.ntbMove5.Caption = "高さ";
			this.ntbMove5.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.ntbMove5.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F);
			this.ntbMove5.CaptionWidth = 100;
			this.ntbMove5.DiscreteInterval = 0.001F;
			this.ntbMove5.Font = new System.Drawing.Font("MS UI Gothic", 10F);
			this.ntbMove5.IncDecButton = false;
			this.ntbMove5.Location = new System.Drawing.Point(4, 140);
			this.ntbMove5.Max = new decimal(new int[] {
            2147483647,
            0,
            0,
            196608});
			this.ntbMove5.Min = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147287040});
			this.ntbMove5.Name = "ntbMove5";
			this.ntbMove5.Size = new System.Drawing.Size(203, 20);
			this.ntbMove5.TabIndex = 6;
			this.ntbMove5.Unit = "mm";
			this.ntbMove5.Value = new decimal(new int[] {
            999999,
            0,
            0,
            196608});
			// 
			// chkXYMove
			// 
			this.chkXYMove.Location = new System.Drawing.Point(40, 168);
			this.chkXYMove.Name = "chkXYMove";
			this.chkXYMove.Size = new System.Drawing.Size(253, 16);
			this.chkXYMove.TabIndex = 7;
			this.chkXYMove.Text = "FCDとY軸を同時に移動させる（デバッグ用）";
			this.chkXYMove.UseVisualStyleBackColor = true;
			this.chkXYMove.Visible = false;
			// 
			// lblPrompt
			// 
			this.lblPrompt.Location = new System.Drawing.Point(44, 184);
			this.lblPrompt.Name = "lblPrompt";
			this.lblPrompt.Size = new System.Drawing.Size(251, 104);
			this.lblPrompt.TabIndex = 8;
			this.lblPrompt.Text = "よろしければ「移動」ボタンをクリックしてください。";
			// 
			// cmdClose
			// 
			this.cmdClose.Location = new System.Drawing.Point(176, 296);
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size(105, 25);
			this.cmdClose.TabIndex = 10;
			this.cmdClose.Tag = "10008";
			this.cmdClose.Text = "#閉じる";
			this.cmdClose.UseVisualStyleBackColor = true;
			this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
			// 
			// ntbFCD1st
			// 
			this.ntbFCD1st.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.ntbFCD1st.Caption = "";
			this.ntbFCD1st.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.ntbFCD1st.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F);
			this.ntbFCD1st.CaptionWidth = 1;
			this.ntbFCD1st.DiscreteInterval = 0.1F;
			this.ntbFCD1st.Font = new System.Drawing.Font("MS UI Gothic", 10F);
			this.ntbFCD1st.IncDecButton = false;
			this.ntbFCD1st.Location = new System.Drawing.Point(212, 80);
			this.ntbFCD1st.Max = new decimal(new int[] {
            2147483647,
            0,
            0,
            65536});
			this.ntbFCD1st.Min = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147418112});
			this.ntbFCD1st.Name = "ntbFCD1st";
			this.ntbFCD1st.Size = new System.Drawing.Size(87, 20);
			this.ntbFCD1st.TabIndex = 11;
			this.ntbFCD1st.Unit = "mm";
			this.ntbFCD1st.Value = new decimal(new int[] {
            999999,
            0,
            0,
            65536});
			this.ntbFCD1st.Visible = false;
			// 
			// ntbY1st
			// 
			this.ntbY1st.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.ntbY1st.Caption = "";
			this.ntbY1st.CaptionAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.ntbY1st.CaptionFont = new System.Drawing.Font("MS UI Gothic", 9F);
			this.ntbY1st.CaptionWidth = 1;
			this.ntbY1st.DiscreteInterval = 0.01F;
			this.ntbY1st.Font = new System.Drawing.Font("MS UI Gothic", 10F);
			this.ntbY1st.IncDecButton = false;
			this.ntbY1st.Location = new System.Drawing.Point(212, 120);
			this.ntbY1st.Max = new decimal(new int[] {
            -10,
            4,
            0,
            196608});
			this.ntbY1st.Min = new decimal(new int[] {
            0,
            5,
            0,
            -2147287040});
			this.ntbY1st.Name = "ntbY1st";
			this.ntbY1st.Size = new System.Drawing.Size(87, 20);
			this.ntbY1st.TabIndex = 12;
			this.ntbY1st.Unit = "mm";
			this.ntbY1st.Value = new decimal(new int[] {
            999999,
            0,
            0,
            131072});
			this.ntbY1st.Visible = false;
			// 
			// Image1
			// 
			this.Image1.Location = new System.Drawing.Point(4, 4);
			this.Image1.Name = "Image1";
			this.Image1.Size = new System.Drawing.Size(32, 32);
			this.Image1.TabIndex = 13;
			this.Image1.TabStop = false;
			// 
			// cwbtnMove
			// 
			this.cwbtnMove.Location = new System.Drawing.Point(32, 296);
			this.cwbtnMove.Name = "cwbtnMove";
			this.cwbtnMove.Size = new System.Drawing.Size(104, 25);
			this.cwbtnMove.TabIndex = 9;
			this.cwbtnMove.Text = "移動";
			this.cwbtnMove.UseVisualStyleBackColor = true;
			this.cwbtnMove.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cwbtnMove_KeyDown);
			this.cwbtnMove.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cwbtnMove_KeyUp);
			this.cwbtnMove.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cwbtnMove_MouseClick);
			this.cwbtnMove.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cwbtnMove_MouseDown);
			this.cwbtnMove.MouseUp += new System.Windows.Forms.MouseEventHandler(this.cwbtnMove_MouseUp);
			// 
			// frmMechaMove
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(312, 335);
			this.ControlBox = false;
			this.Controls.Add(this.cwbtnMove);
			this.Controls.Add(this.lblPrompt);
			this.Controls.Add(this.Image1);
			this.Controls.Add(this.cmdClose);
			this.Controls.Add(this.chkXYMove);
			this.Controls.Add(this.ntbMove5);
			this.Controls.Add(this.ntbY1st);
			this.Controls.Add(this.ntbMove4);
			this.Controls.Add(this.ntbMove3);
			this.Controls.Add(this.ntbFCD1st);
			this.Controls.Add(this.ntbMove2);
			this.Controls.Add(this.ntbMove1);
			this.Controls.Add(this.ntbMove0);
			this.Controls.Add(this.Label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Location = new System.Drawing.Point(0, -7);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmMechaMove";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Tag = "20168";
			this.Text = "メカ移動";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMechaMove_FormClosed);
			this.Load += new System.EventHandler(this.frmMechaMove_Load);
			((System.ComponentModel.ISupportInitialize)(this.Image1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label Label1;
		private NumTextBox ntbMove0;
		private NumTextBox ntbMove1;
		private NumTextBox ntbMove2;
		private NumTextBox ntbMove3;
		private NumTextBox ntbMove4;
		private NumTextBox ntbMove5;
		private System.Windows.Forms.CheckBox chkXYMove;
		private System.Windows.Forms.Label lblPrompt;
		private System.Windows.Forms.Button cmdClose;
		private NumTextBox ntbFCD1st;
		private NumTextBox ntbY1st;
		private System.Windows.Forms.PictureBox Image1;
		private System.Windows.Forms.Button cwbtnMove;
	}
}