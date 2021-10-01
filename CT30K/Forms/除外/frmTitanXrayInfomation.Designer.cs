using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
namespace CT30K
{
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class frmTitanXrayInfomation
	{
		#region "Windows フォーム デザイナによって生成されたコード "
		[System.Diagnostics.DebuggerNonUserCode()]
		public frmTitanXrayInfomation() : base()
		{
			Load += frmTitanXrayInfomation_Load;
			//この呼び出しは、Windows フォーム デザイナで必要です。
			InitializeComponent();
		}
//Form は、コンポーネント一覧に後処理を実行するために dispose をオーバーライドします。
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool Disposing)
		{
			if (Disposing) {
				if ((components != null)) {
					components.Dispose();
				}
			}
			base.Dispose(Disposing);
		}
//Windows フォーム デザイナで必要です。
		private System.ComponentModel.IContainer components;
		public System.Windows.Forms.ToolTip ToolTip1;
		private System.Windows.Forms.Button withEventsField_cmdUpdate;
		public System.Windows.Forms.Button cmdUpdate {
			get { return withEventsField_cmdUpdate; }
			set {
				if (withEventsField_cmdUpdate != null) {
					withEventsField_cmdUpdate.Click -= cmdUpdate_Click;
				}
				withEventsField_cmdUpdate = value;
				if (withEventsField_cmdUpdate != null) {
					withEventsField_cmdUpdate.Click += cmdUpdate_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_cmdClose;
		public System.Windows.Forms.Button cmdClose {
			get { return withEventsField_cmdClose; }
			set {
				if (withEventsField_cmdClose != null) {
					withEventsField_cmdClose.Click -= cmdClose_Click;
				}
				withEventsField_cmdClose = value;
				if (withEventsField_cmdClose != null) {
					withEventsField_cmdClose.Click += cmdClose_Click;
				}
			}
		}
		private System.Windows.Forms.Button withEventsField_cmdErrReset;
		public System.Windows.Forms.Button cmdErrReset {
			get { return withEventsField_cmdErrReset; }
			set {
				if (withEventsField_cmdErrReset != null) {
					withEventsField_cmdErrReset.Click -= cmdErrReset_Click;
				}
				withEventsField_cmdErrReset = value;
				if (withEventsField_cmdErrReset != null) {
					withEventsField_cmdErrReset.Click += cmdErrReset_Click;
				}
			}
		}
		public System.Windows.Forms.Label lblErrContentsTxt;
		public System.Windows.Forms.Label lblErrNoTxt;
		public System.Windows.Forms.Label lblErrContents;
		public System.Windows.Forms.Label lblErrNo;
//メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
//Windows フォーム デザイナを使って変更できます。
//コード エディタを使用して、変更しないでください。
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmTitanXrayInfomation));
			this.components = new System.ComponentModel.Container();
			this.ToolTip1 = new System.Windows.Forms.ToolTip(components);
			this.cmdUpdate = new System.Windows.Forms.Button();
			this.cmdClose = new System.Windows.Forms.Button();
			this.cmdErrReset = new System.Windows.Forms.Button();
			this.lblErrContentsTxt = new System.Windows.Forms.Label();
			this.lblErrNoTxt = new System.Windows.Forms.Label();
			this.lblErrContents = new System.Windows.Forms.Label();
			this.lblErrNo = new System.Windows.Forms.Label();
			this.SuspendLayout();
			this.ToolTip1.Active = true;
			this.Text = "#X線情報";
			this.ClientSize = new System.Drawing.Size(322, 255);
			this.Location = new System.Drawing.Point(4, 23);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Tag = "14100";
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
			this.ControlBox = true;
			this.Enabled = true;
			this.KeyPreview = false;
			this.MaximizeBox = true;
			this.MinimizeBox = true;
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.ShowInTaskbar = true;
			this.HelpButton = false;
			this.WindowState = System.Windows.Forms.FormWindowState.Normal;
			this.Name = "frmTitanXrayInfomation";
			this.cmdUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cmdUpdate.Text = "#更新";
			this.cmdUpdate.Size = new System.Drawing.Size(81, 25);
			this.cmdUpdate.Location = new System.Drawing.Point(56, 224);
			this.cmdUpdate.TabIndex = 4;
			this.cmdUpdate.Tag = "20120";
			this.cmdUpdate.BackColor = System.Drawing.SystemColors.Control;
			this.cmdUpdate.CausesValidation = true;
			this.cmdUpdate.Enabled = true;
			this.cmdUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdUpdate.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdUpdate.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdUpdate.TabStop = true;
			this.cmdUpdate.Name = "cmdUpdate";
			this.cmdClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cmdClose.Text = "#閉じる";
			this.cmdClose.Size = new System.Drawing.Size(81, 25);
			this.cmdClose.Location = new System.Drawing.Point(232, 224);
			this.cmdClose.TabIndex = 1;
			this.cmdClose.Tag = "10008";
			this.cmdClose.BackColor = System.Drawing.SystemColors.Control;
			this.cmdClose.CausesValidation = true;
			this.cmdClose.Enabled = true;
			this.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdClose.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdClose.TabStop = true;
			this.cmdClose.Name = "cmdClose";
			this.cmdErrReset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.cmdErrReset.Text = "#リセット";
			this.cmdErrReset.Size = new System.Drawing.Size(81, 25);
			this.cmdErrReset.Location = new System.Drawing.Point(144, 224);
			this.cmdErrReset.TabIndex = 0;
			this.cmdErrReset.Tag = "10022";
			this.cmdErrReset.BackColor = System.Drawing.SystemColors.Control;
			this.cmdErrReset.CausesValidation = true;
			this.cmdErrReset.Enabled = true;
			this.cmdErrReset.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cmdErrReset.Cursor = System.Windows.Forms.Cursors.Default;
			this.cmdErrReset.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.cmdErrReset.TabStop = true;
			this.cmdErrReset.Name = "cmdErrReset";
			this.lblErrContentsTxt.Text = "Error Contents";
			this.lblErrContentsTxt.Size = new System.Drawing.Size(209, 161);
			this.lblErrContentsTxt.Location = new System.Drawing.Point(96, 40);
			this.lblErrContentsTxt.TabIndex = 6;
			this.lblErrContentsTxt.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.lblErrContentsTxt.BackColor = System.Drawing.SystemColors.Control;
			this.lblErrContentsTxt.Enabled = true;
			this.lblErrContentsTxt.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblErrContentsTxt.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblErrContentsTxt.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblErrContentsTxt.UseMnemonic = true;
			this.lblErrContentsTxt.Visible = true;
			this.lblErrContentsTxt.AutoSize = false;
			this.lblErrContentsTxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblErrContentsTxt.Name = "lblErrContentsTxt";
			this.lblErrNoTxt.Text = "0";
			this.lblErrNoTxt.Size = new System.Drawing.Size(65, 25);
			this.lblErrNoTxt.Location = new System.Drawing.Point(96, 8);
			this.lblErrNoTxt.TabIndex = 5;
			this.lblErrNoTxt.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.lblErrNoTxt.BackColor = System.Drawing.SystemColors.Control;
			this.lblErrNoTxt.Enabled = true;
			this.lblErrNoTxt.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblErrNoTxt.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblErrNoTxt.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblErrNoTxt.UseMnemonic = true;
			this.lblErrNoTxt.Visible = true;
			this.lblErrNoTxt.AutoSize = false;
			this.lblErrNoTxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblErrNoTxt.Name = "lblErrNoTxt";
			this.lblErrContents.Text = "#原因";
			this.lblErrContents.Size = new System.Drawing.Size(33, 25);
			this.lblErrContents.Location = new System.Drawing.Point(48, 40);
			this.lblErrContents.TabIndex = 3;
			this.lblErrContents.Tag = "12511";
			this.lblErrContents.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.lblErrContents.BackColor = System.Drawing.SystemColors.Control;
			this.lblErrContents.Enabled = true;
			this.lblErrContents.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblErrContents.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblErrContents.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblErrContents.UseMnemonic = true;
			this.lblErrContents.Visible = true;
			this.lblErrContents.AutoSize = false;
			this.lblErrContents.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lblErrContents.Name = "lblErrContents";
			this.lblErrNo.Text = "#エラー番号";
			this.lblErrNo.Size = new System.Drawing.Size(65, 17);
			this.lblErrNo.Location = new System.Drawing.Point(16, 16);
			this.lblErrNo.TabIndex = 2;
			this.lblErrNo.Tag = "20013";
			this.lblErrNo.TextAlign = System.Drawing.ContentAlignment.TopLeft;
			this.lblErrNo.BackColor = System.Drawing.SystemColors.Control;
			this.lblErrNo.Enabled = true;
			this.lblErrNo.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblErrNo.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblErrNo.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblErrNo.UseMnemonic = true;
			this.lblErrNo.Visible = true;
			this.lblErrNo.AutoSize = false;
			this.lblErrNo.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lblErrNo.Name = "lblErrNo";
			this.Controls.Add(cmdUpdate);
			this.Controls.Add(cmdClose);
			this.Controls.Add(cmdErrReset);
			this.Controls.Add(lblErrContentsTxt);
			this.Controls.Add(lblErrNoTxt);
			this.Controls.Add(lblErrContents);
			this.Controls.Add(lblErrNo);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		#endregion
	}
}
