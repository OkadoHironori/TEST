namespace CT30K
{
	partial class frmMechaMoveWarning
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMechaMoveWarning));
            this.Image1 = new System.Windows.Forms.PictureBox();
            this.cmdContinue = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Image1)).BeginInit();
            this.SuspendLayout();
            // 
            // Image1
            // 
            this.Image1.Image = ((System.Drawing.Image)(resources.GetObject("Image1.Image")));
            this.Image1.Location = new System.Drawing.Point(8, 8);
            this.Image1.Name = "Image1";
            this.Image1.Size = new System.Drawing.Size(32, 32);
            this.Image1.TabIndex = 0;
            this.Image1.TabStop = false;
            // 
            // cmdContinue
            // 
            this.cmdContinue.Location = new System.Drawing.Point(300, 104);
            this.cmdContinue.Name = "cmdContinue";
            this.cmdContinue.Size = new System.Drawing.Size(93, 29);
            this.cmdContinue.TabIndex = 1;
            this.cmdContinue.Tag = "20134";
            this.cmdContinue.Text = "続行";
            this.cmdContinue.UseVisualStyleBackColor = true;
            this.cmdContinue.Click += new System.EventHandler(this.cmdContinue_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(409, 103);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(93, 29);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Tag = "10002";
            this.cmdCancel.Text = "キャンセル";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(63, 8);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(452, 78);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Tag = "9404";
            this.lblMessage.Text = "指定したエリアを回転中心位置に移動できませんでした。\r\n\r\nこのまま続行することも可能です。\r\n\r\n最適なスキャン位置でスキャンするためには、微調テーブルのＸ軸、" +
    "Ｙ軸をリセットした後、\r\nスキャンしたいエリアがテーブルの中心付近になるようにワークをセットし直してください。\r\n";
            // 
            // frmMechaMoveWarning
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(527, 147);
            this.ControlBox = false;
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdContinue);
            this.Controls.Add(this.Image1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(0, -7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMechaMoveWarning";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "";
            this.Text = "CT30K";
            this.Load += new System.EventHandler(this.frmMechaMoveWarning_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Image1)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox Image1;
		private System.Windows.Forms.Button cmdContinue;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Label lblMessage;
	}
}