namespace CT30K
{
	partial class frmImagePrint
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
            this.cmdPrint = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.chk1stMonitor = new System.Windows.Forms.CheckBox();
            this.chk2ndMonitor = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cmdPrint
            // 
            this.cmdPrint.Location = new System.Drawing.Point(34, 97);
            this.cmdPrint.Name = "cmdPrint";
            this.cmdPrint.Size = new System.Drawing.Size(100, 27);
            this.cmdPrint.TabIndex = 0;
            this.cmdPrint.Tag = "20131";
            this.cmdPrint.Text = "印刷";
            this.cmdPrint.UseVisualStyleBackColor = true;
            this.cmdPrint.Click += new System.EventHandler(this.cmdPrint_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(179, 96);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(100, 27);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Tag = "10002";
            this.cmdCancel.Text = "キャンセル";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // chk1stMonitor
            // 
            this.chk1stMonitor.Checked = true;
            this.chk1stMonitor.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.chk1stMonitor.Location = new System.Drawing.Point(51, 24);
            this.chk1stMonitor.Name = "chk1stMonitor";
            this.chk1stMonitor.Size = new System.Drawing.Size(197, 25);
            this.chk1stMonitor.TabIndex = 2;
            this.chk1stMonitor.Tag = "20129";
            this.chk1stMonitor.Text = "メイン画面";
            this.chk1stMonitor.UseVisualStyleBackColor = true;
            this.chk1stMonitor.CheckStateChanged += new System.EventHandler(this.chk1stMonitor_CheckStateChanged);
            // 
            // chk2ndMonitor
            // 
            this.chk2ndMonitor.Checked = true;
            this.chk2ndMonitor.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.chk2ndMonitor.Location = new System.Drawing.Point(51, 58);
            this.chk2ndMonitor.Name = "chk2ndMonitor";
            this.chk2ndMonitor.Size = new System.Drawing.Size(197, 24);
            this.chk2ndMonitor.TabIndex = 3;
            this.chk2ndMonitor.Tag = "20130";
            this.chk2ndMonitor.Text = "透視画面";
            this.chk2ndMonitor.UseVisualStyleBackColor = true;
            this.chk2ndMonitor.CheckStateChanged += new System.EventHandler(this.chk2ndMonitor_CheckStateChanged);
            // 
            // frmImagePrint
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(303, 138);
            this.ControlBox = false;
            this.Controls.Add(this.chk2ndMonitor);
            this.Controls.Add(this.chk1stMonitor);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdPrint);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(0, -7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmImagePrint";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "20131";
            this.Text = "画面印刷";
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button cmdPrint;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.CheckBox chk1stMonitor;
		private System.Windows.Forms.CheckBox chk2ndMonitor;
	}
}