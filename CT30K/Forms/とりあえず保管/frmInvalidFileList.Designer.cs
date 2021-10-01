namespace CT30K
{
	partial class frmInvalidFileList
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInvalidFileList));
			this.lblMessage = new System.Windows.Forms.Label();
			this.cmdClose = new System.Windows.Forms.Button();
			this.lvwInvalidFile = new System.Windows.Forms.ListView();
			this.Image1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.Image1)).BeginInit();
			this.SuspendLayout();
			// 
			// lblMessage
			// 
			this.lblMessage.AutoSize = true;
			this.lblMessage.Location = new System.Drawing.Point(64, 20);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(183, 12);
			this.lblMessage.TabIndex = 0;
			this.lblMessage.Text = "以下のファイルは追加されませんでした";
			// 
			// cmdClose
			// 
			this.cmdClose.Location = new System.Drawing.Point(464, 264);
			this.cmdClose.Name = "cmdClose";
			this.cmdClose.Size = new System.Drawing.Size(81, 25);
			this.cmdClose.TabIndex = 1;
			this.cmdClose.Tag = "10008";
			this.cmdClose.Text = "閉じる";
			this.cmdClose.UseVisualStyleBackColor = true;
			this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
			// 
			// lvwInvalidFile
			// 
			this.lvwInvalidFile.Font = new System.Drawing.Font("MS UI Gothic", 9.75F);
			this.lvwInvalidFile.FullRowSelect = true;
			this.lvwInvalidFile.GridLines = true;
			this.lvwInvalidFile.Location = new System.Drawing.Point(16, 52);
			this.lvwInvalidFile.Name = "lvwInvalidFile";
			this.lvwInvalidFile.Size = new System.Drawing.Size(541, 193);
			this.lvwInvalidFile.TabIndex = 2;
			this.lvwInvalidFile.UseCompatibleStateImageBehavior = false;
			this.lvwInvalidFile.View = System.Windows.Forms.View.Details;
			// 
			// Image1
			// 
			this.Image1.Image = ((System.Drawing.Image)(resources.GetObject("Image1.Image")));
			this.Image1.Location = new System.Drawing.Point(16, 12);
			this.Image1.Name = "Image1";
			this.Image1.Size = new System.Drawing.Size(32, 32);
			this.Image1.TabIndex = 3;
			this.Image1.TabStop = false;
			// 
			// frmInvalidFileList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(569, 313);
			this.ControlBox = false;
			this.Controls.Add(this.Image1);
			this.Controls.Add(this.lvwInvalidFile);
			this.Controls.Add(this.cmdClose);
			this.Controls.Add(this.lblMessage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Location = new System.Drawing.Point(0, -7);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmInvalidFileList";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CT30K";
			this.Load += new System.EventHandler(this.frmInvalidFileList_Load);
			((System.ComponentModel.ISupportInitialize)(this.Image1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.Button cmdClose;
		private System.Windows.Forms.ListView lvwInvalidFile;
		private System.Windows.Forms.PictureBox Image1;
	}
}