namespace CT30K
{
	partial class frmAdjAlignment
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
			this.components = new System.ComponentModel.Container();
			this.fraFocus = new System.Windows.Forms.GroupBox();
			this.lblFocusFeedBack = new System.Windows.Forms.Label();
			this.lblFocus = new System.Windows.Forms.Label();
			this.lblFeedBack = new System.Windows.Forms.Label();
			this.lblStep = new System.Windows.Forms.Label();
			this.lblSetValue = new System.Windows.Forms.Label();
			this.cmdSave = new System.Windows.Forms.Button();
			this.cmdAutoGet = new System.Windows.Forms.Button();
			this.cboStep = new System.Windows.Forms.ComboBox();
			this.cwneFocusSetValue = new System.Windows.Forms.NumericUpDown();
			this.cmdClose = new System.Windows.Forms.Button();
			this.cmdAlignment = new System.Windows.Forms.Button();
			this.cmdAlignmentAll = new System.Windows.Forms.Button();
			this.cmdStop = new System.Windows.Forms.Button();
			this.cmdWUP0 = new System.Windows.Forms.Button();
			this.cmdWUP1 = new System.Windows.Forms.Button();
			this.cmdWUP2 = new System.Windows.Forms.Button();
			this.cmdReset = new System.Windows.Forms.Button();
			this.fraBeamAlignment = new System.Windows.Forms.GroupBox();
			this.txtAlignmentAll = new System.Windows.Forms.TextBox();
			this.txtAlignment = new System.Windows.Forms.TextBox();
			this.lblAlignmentY = new System.Windows.Forms.Label();
			this.lblAlignmentX = new System.Windows.Forms.Label();
			this.lblAlignment = new System.Windows.Forms.Label();
			this.lblDirectionY = new System.Windows.Forms.Label();
			this.lblDirectionX = new System.Windows.Forms.Label();
			this.fraWarmup = new System.Windows.Forms.GroupBox();
			this.cmdWarmup = new System.Windows.Forms.Button();
			this.tmrAdjAlignment = new System.Windows.Forms.Timer(this.components);
			this.ToolTipText = new System.Windows.Forms.ToolTip(this.components);
			this.fraFocus.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cwneFocusSetValue)).BeginInit();
			this.fraBeamAlignment.SuspendLayout();
			this.fraWarmup.SuspendLayout();
			this.SuspendLayout();
			// 
			// fraFocus
			// 
			this.fraFocus.Controls.Add(this.lblFocusFeedBack);
			this.fraFocus.Controls.Add(this.lblFocus);
			this.fraFocus.Controls.Add(this.lblFeedBack);
			this.fraFocus.Controls.Add(this.lblStep);
			this.fraFocus.Controls.Add(this.lblSetValue);
			this.fraFocus.Controls.Add(this.cmdSave);
			this.fraFocus.Controls.Add(this.cmdAutoGet);
			this.fraFocus.Controls.Add(this.cboStep);
			this.fraFocus.Controls.Add(this.cwneFocusSetValue);
			this.fraFocus.Location = new System.Drawing.Point(6, 6);
			this.fraFocus.Name = "fraFocus";
			this.fraFocus.Size = new System.Drawing.Size(301, 97);
			this.fraFocus.TabIndex = 0;
			this.fraFocus.TabStop = false;
			this.fraFocus.Tag = "14201";
			this.fraFocus.Text = "#フォーカス調整";
			// 
			// lblFocusFeedBack
			// 
			this.lblFocusFeedBack.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblFocusFeedBack.Location = new System.Drawing.Point(212, 36);
			this.lblFocusFeedBack.Name = "lblFocusFeedBack";
			this.lblFocusFeedBack.Size = new System.Drawing.Size(73, 21);
			this.lblFocusFeedBack.TabIndex = 8;
			this.lblFocusFeedBack.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblFocus
			// 
			this.lblFocus.AutoSize = true;
			this.lblFocus.Location = new System.Drawing.Point(10, 40);
			this.lblFocus.Name = "lblFocus";
			this.lblFocus.Size = new System.Drawing.Size(55, 12);
			this.lblFocus.TabIndex = 7;
			this.lblFocus.Tag = "14202";
			this.lblFocus.Text = "#フォーカス";
			this.lblFocus.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblFeedBack
			// 
			this.lblFeedBack.AutoSize = true;
			this.lblFeedBack.Location = new System.Drawing.Point(212, 20);
			this.lblFeedBack.Name = "lblFeedBack";
			this.lblFeedBack.Size = new System.Drawing.Size(70, 12);
			this.lblFeedBack.TabIndex = 6;
			this.lblFeedBack.Tag = "14205";
			this.lblFeedBack.Text = "#フィードバック";
			// 
			// lblStep
			// 
			this.lblStep.AutoSize = true;
			this.lblStep.Location = new System.Drawing.Point(152, 20);
			this.lblStep.Name = "lblStep";
			this.lblStep.Size = new System.Drawing.Size(45, 12);
			this.lblStep.TabIndex = 5;
			this.lblStep.Tag = "14204";
			this.lblStep.Text = "#ステップ";
			// 
			// lblSetValue
			// 
			this.lblSetValue.AutoSize = true;
			this.lblSetValue.Location = new System.Drawing.Point(72, 20);
			this.lblSetValue.Name = "lblSetValue";
			this.lblSetValue.Size = new System.Drawing.Size(47, 12);
			this.lblSetValue.TabIndex = 4;
			this.lblSetValue.Tag = "14203";
			this.lblSetValue.Text = "#設定値";
			// 
			// cmdSave
			// 
			this.cmdSave.Location = new System.Drawing.Point(208, 64);
			this.cmdSave.Name = "cmdSave";
			this.cmdSave.Size = new System.Drawing.Size(77, 25);
			this.cmdSave.TabIndex = 3;
			this.cmdSave.Tag = "10012";
			this.cmdSave.Text = "#保　存";
			this.cmdSave.UseVisualStyleBackColor = true;
			this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
			// 
			// cmdAutoGet
			// 
			this.cmdAutoGet.Location = new System.Drawing.Point(116, 64);
			this.cmdAutoGet.Name = "cmdAutoGet";
			this.cmdAutoGet.Size = new System.Drawing.Size(85, 25);
			this.cmdAutoGet.TabIndex = 2;
			this.cmdAutoGet.Tag = "14206";
			this.cmdAutoGet.Text = "#自動取得";
			this.cmdAutoGet.UseVisualStyleBackColor = true;
			this.cmdAutoGet.Click += new System.EventHandler(this.cmdAutoGet_Click);
			// 
			// cboStep
			// 
			this.cboStep.FormattingEnabled = true;
			this.cboStep.Items.AddRange(new object[] {
            "0.1",
            "1",
            "10"});
			this.cboStep.Location = new System.Drawing.Point(148, 36);
			this.cboStep.Name = "cboStep";
			this.cboStep.Size = new System.Drawing.Size(57, 20);
			this.cboStep.TabIndex = 1;
			this.cboStep.Text = "cboStep";
			this.cboStep.Click += new System.EventHandler(this.cboStep_Click);
			// 
			// cwneFocusSetValue
			// 
			this.cwneFocusSetValue.Location = new System.Drawing.Point(72, 36);
			this.cwneFocusSetValue.Name = "cwneFocusSetValue";
			this.cwneFocusSetValue.Size = new System.Drawing.Size(65, 19);
			this.cwneFocusSetValue.TabIndex = 0;
			this.cwneFocusSetValue.ValueChanged += new System.EventHandler(this.cwneFocusSetValue_ValueChanged);
			// 
			// cmdClose
			// 
			this.cmdClose.Location = new System.Drawing.Point(218, 282);
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size(89, 25);
			this.cmdClose.TabIndex = 1;
			this.cmdClose.Tag = "10008";
			this.cmdClose.Text = "#閉じる";
			this.cmdClose.UseVisualStyleBackColor = true;
			this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
			// 
			// cmdAlignment
			// 
			this.cmdAlignment.Location = new System.Drawing.Point(5, 56);
			this.cmdAlignment.Name = "cmdAlignment";
			this.cmdAlignment.Size = new System.Drawing.Size(97, 34);
			this.cmdAlignment.TabIndex = 2;
			this.cmdAlignment.Tag = "14211";
			this.cmdAlignment.Text = "#ｱﾗｲﾒﾝﾄ";
			this.cmdAlignment.UseVisualStyleBackColor = true;
			this.cmdAlignment.Click += new System.EventHandler(this.cmdAlignment_Click);
			// 
			// cmdAlignmentAll
			// 
			this.cmdAlignmentAll.Location = new System.Drawing.Point(107, 56);
			this.cmdAlignmentAll.Name = "cmdAlignmentAll";
			this.cmdAlignmentAll.Size = new System.Drawing.Size(97, 34);
			this.cmdAlignmentAll.TabIndex = 3;
			this.cmdAlignmentAll.Tag = "14212";
			this.cmdAlignmentAll.Text = "　#一括ｱﾗｲﾒﾝﾄ";
			this.cmdAlignmentAll.UseVisualStyleBackColor = true;
			this.cmdAlignmentAll.Click += new System.EventHandler(this.cmdAlignmentAll_Click);
			// 
			// cmdStop
			// 
			this.cmdStop.Enabled = false;
			this.cmdStop.Location = new System.Drawing.Point(208, 56);
			this.cmdStop.Name = "cmdStop";
			this.cmdStop.Size = new System.Drawing.Size(85, 34);
			this.cmdStop.TabIndex = 4;
			this.cmdStop.Tag = "14213";
			this.cmdStop.Text = "#中断";
			this.cmdStop.UseVisualStyleBackColor = true;
			this.cmdStop.Click += new System.EventHandler(this.cmdStop_Click);
			// 
			// cmdWUP0
			// 
			this.cmdWUP0.Location = new System.Drawing.Point(16, 20);
			this.cmdWUP0.Name = "cmdWUP0";
			this.cmdWUP0.Size = new System.Drawing.Size(73, 25);
			this.cmdWUP0.TabIndex = 5;
			this.cmdWUP0.Tag = "20001";
			this.cmdWUP0.Text = "#簡易";
			this.cmdWUP0.UseVisualStyleBackColor = true;
			this.cmdWUP0.Click += new System.EventHandler(this.cmdWUP_Click);
			// 
			// cmdWUP1
			// 
			this.cmdWUP1.Location = new System.Drawing.Point(117, 20);
			this.cmdWUP1.Name = "cmdWUP1";
			this.cmdWUP1.Size = new System.Drawing.Size(73, 25);
			this.cmdWUP1.TabIndex = 6;
			this.cmdWUP1.Text = "#簡易安定";
			this.cmdWUP1.UseVisualStyleBackColor = true;
			this.cmdWUP1.Click += new System.EventHandler(this.cmdWUP_Click);
			// 
			// cmdWUP2
			// 
			this.cmdWUP2.Location = new System.Drawing.Point(216, 20);
			this.cmdWUP2.Name = "cmdWUP2";
			this.cmdWUP2.Size = new System.Drawing.Size(73, 25);
			this.cmdWUP2.TabIndex = 7;
			this.cmdWUP2.Tag = "20002";
			this.cmdWUP2.Text = "#通常";
			this.cmdWUP2.UseVisualStyleBackColor = true;
			this.cmdWUP2.Click += new System.EventHandler(this.cmdWUP_Click);
			// 
			// cmdReset
			// 
			this.cmdReset.Location = new System.Drawing.Point(114, 282);
			this.cmdReset.Name = "cmdReset";
			this.cmdReset.Size = new System.Drawing.Size(89, 25);
			this.cmdReset.TabIndex = 8;
			this.cmdReset.Tag = "10022";
			this.cmdReset.Text = "#リセット";
			this.cmdReset.UseVisualStyleBackColor = true;
			this.cmdReset.Click += new System.EventHandler(this.cmdReset_Click);
			// 
			// fraBeamAlignment
			// 
			this.fraBeamAlignment.Controls.Add(this.txtAlignmentAll);
			this.fraBeamAlignment.Controls.Add(this.txtAlignment);
			this.fraBeamAlignment.Controls.Add(this.lblAlignmentY);
			this.fraBeamAlignment.Controls.Add(this.lblAlignmentX);
			this.fraBeamAlignment.Controls.Add(this.lblAlignment);
			this.fraBeamAlignment.Controls.Add(this.lblDirectionY);
			this.fraBeamAlignment.Controls.Add(this.lblDirectionX);
			this.fraBeamAlignment.Controls.Add(this.cmdStop);
			this.fraBeamAlignment.Controls.Add(this.cmdAlignmentAll);
			this.fraBeamAlignment.Controls.Add(this.cmdAlignment);
			this.fraBeamAlignment.Location = new System.Drawing.Point(6, 110);
			this.fraBeamAlignment.Name = "fraBeamAlignment";
			this.fraBeamAlignment.Size = new System.Drawing.Size(301, 96);
			this.fraBeamAlignment.TabIndex = 9;
			this.fraBeamAlignment.TabStop = false;
			this.fraBeamAlignment.Tag = "14207";
			this.fraBeamAlignment.Text = "#ビームアライメント";
			// 
			// txtAlignmentAll
			// 
			this.txtAlignmentAll.BackColor = System.Drawing.SystemColors.Control;
			this.txtAlignmentAll.Enabled = false;
			this.txtAlignmentAll.Location = new System.Drawing.Point(111, 64);
			this.txtAlignmentAll.Name = "txtAlignmentAll";
			this.txtAlignmentAll.Size = new System.Drawing.Size(11, 19);
			this.txtAlignmentAll.TabIndex = 11;
			// 
			// txtAlignment
			// 
			this.txtAlignment.BackColor = System.Drawing.SystemColors.Control;
			this.txtAlignment.Enabled = false;
			this.txtAlignment.Location = new System.Drawing.Point(9, 64);
			this.txtAlignment.Name = "txtAlignment";
			this.txtAlignment.Size = new System.Drawing.Size(11, 19);
			this.txtAlignment.TabIndex = 10;
			// 
			// lblAlignmentY
			// 
			this.lblAlignmentY.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblAlignmentY.Location = new System.Drawing.Point(192, 32);
			this.lblAlignmentY.Name = "lblAlignmentY";
			this.lblAlignmentY.Size = new System.Drawing.Size(61, 21);
			this.lblAlignmentY.TabIndex = 9;
			this.lblAlignmentY.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblAlignmentX
			// 
			this.lblAlignmentX.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblAlignmentX.Location = new System.Drawing.Point(120, 32);
			this.lblAlignmentX.Name = "lblAlignmentX";
			this.lblAlignmentX.Size = new System.Drawing.Size(61, 21);
			this.lblAlignmentX.TabIndex = 8;
			this.lblAlignmentX.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblAlignment
			// 
			this.lblAlignment.AutoSize = true;
			this.lblAlignment.Location = new System.Drawing.Point(16, 36);
			this.lblAlignment.Name = "lblAlignment";
			this.lblAlignment.Size = new System.Drawing.Size(74, 12);
			this.lblAlignment.TabIndex = 7;
			this.lblAlignment.Tag = "14208";
			this.lblAlignment.Text = "#アライメント値";
			// 
			// lblDirectionY
			// 
			this.lblDirectionY.AutoSize = true;
			this.lblDirectionY.Location = new System.Drawing.Point(220, 16);
			this.lblDirectionY.Name = "lblDirectionY";
			this.lblDirectionY.Size = new System.Drawing.Size(43, 12);
			this.lblDirectionY.TabIndex = 6;
			this.lblDirectionY.Tag = "14210";
			this.lblDirectionY.Text = "#Ｙ方向";
			// 
			// lblDirectionX
			// 
			this.lblDirectionX.AutoSize = true;
			this.lblDirectionX.Location = new System.Drawing.Point(148, 16);
			this.lblDirectionX.Name = "lblDirectionX";
			this.lblDirectionX.Size = new System.Drawing.Size(43, 12);
			this.lblDirectionX.TabIndex = 5;
			this.lblDirectionX.Tag = "14209";
			this.lblDirectionX.Text = "#Ｘ方向";
			// 
			// fraWarmup
			// 
			this.fraWarmup.Controls.Add(this.cmdWUP2);
			this.fraWarmup.Controls.Add(this.cmdWUP1);
			this.fraWarmup.Controls.Add(this.cmdWUP0);
			this.fraWarmup.Location = new System.Drawing.Point(6, 213);
			this.fraWarmup.Name = "fraWarmup";
			this.fraWarmup.Size = new System.Drawing.Size(301, 57);
			this.fraWarmup.TabIndex = 10;
			this.fraWarmup.TabStop = false;
			this.fraWarmup.Tag = "10524";
			this.fraWarmup.Text = "#ｳｫｰﾑｱｯﾌﾟ";
			// 
			// cmdWarmup
			// 
			this.cmdWarmup.Location = new System.Drawing.Point(10, 282);
			this.cmdWarmup.Name = "cmdWarmup";
			this.cmdWarmup.Size = new System.Drawing.Size(89, 25);
			this.cmdWarmup.TabIndex = 11;
			this.cmdWarmup.Tag = "12156";
			this.cmdWarmup.Text = "#ｳｫｰﾑｱｯﾌﾟ";
			this.cmdWarmup.UseVisualStyleBackColor = true;
			this.cmdWarmup.Click += new System.EventHandler(this.cmdWarmup_Click);
			// 
			// tmrAdjAlignment
			// 
			this.tmrAdjAlignment.Interval = 1000;
			this.tmrAdjAlignment.Tick += new System.EventHandler(this.tmrAdjAlignment_Timer);
			// 
			// frmAdjAlignment
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(313, 324);
			this.ControlBox = false;
			this.Controls.Add(this.cmdWarmup);
			this.Controls.Add(this.fraWarmup);
			this.Controls.Add(this.fraBeamAlignment);
			this.Controls.Add(this.cmdReset);
			this.Controls.Add(this.cmdClose);
			this.Controls.Add(this.fraFocus);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "frmAdjAlignment";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Tag = "14200";
			this.Text = "#アライメント調整";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAdjAlignment_FormClosed);
			this.Load += new System.EventHandler(this.frmAdjAlignment_Load);
			this.fraFocus.ResumeLayout(false);
			this.fraFocus.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.cwneFocusSetValue)).EndInit();
			this.fraBeamAlignment.ResumeLayout(false);
			this.fraBeamAlignment.PerformLayout();
			this.fraWarmup.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox fraFocus;
		private System.Windows.Forms.Label lblFocusFeedBack;
		private System.Windows.Forms.Label lblFocus;
		private System.Windows.Forms.Label lblFeedBack;
		private System.Windows.Forms.Label lblStep;
		private System.Windows.Forms.Label lblSetValue;
		private System.Windows.Forms.Button cmdSave;
		private System.Windows.Forms.Button cmdAutoGet;
		private System.Windows.Forms.ComboBox cboStep;
		private System.Windows.Forms.NumericUpDown cwneFocusSetValue;
		private System.Windows.Forms.Button cmdClose;
		private System.Windows.Forms.Button cmdAlignment;
		private System.Windows.Forms.Button cmdAlignmentAll;
		private System.Windows.Forms.Button cmdStop;
		private System.Windows.Forms.Button cmdWUP0;
		private System.Windows.Forms.Button cmdWUP1;
		private System.Windows.Forms.Button cmdWUP2;
		private System.Windows.Forms.Button cmdReset;
		private System.Windows.Forms.GroupBox fraBeamAlignment;
		private System.Windows.Forms.TextBox txtAlignmentAll;
		private System.Windows.Forms.TextBox txtAlignment;
		private System.Windows.Forms.Label lblAlignmentY;
		private System.Windows.Forms.Label lblAlignmentX;
		private System.Windows.Forms.Label lblAlignment;
		private System.Windows.Forms.Label lblDirectionY;
		private System.Windows.Forms.Label lblDirectionX;
		private System.Windows.Forms.GroupBox fraWarmup;
		private System.Windows.Forms.Button cmdWarmup;
		private System.Windows.Forms.Timer tmrAdjAlignment;
		private System.Windows.Forms.ToolTip ToolTipText;
	}
}