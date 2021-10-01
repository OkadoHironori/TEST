namespace CT30K
{
    partial class NumTextBox
    {
        /// <summary> 
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
			this.lblCaption = new System.Windows.Forms.Label();
			this.lblColon = new System.Windows.Forms.Label();
			this.lblUnit = new System.Windows.Forms.Label();
			this.nudValue = new System.Windows.Forms.NumericUpDown();
			this.panel1 = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.nudValue)).BeginInit();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblCaption
			// 
			this.lblCaption.Location = new System.Drawing.Point(0, 0);
			this.lblCaption.Name = "lblCaption";
			this.lblCaption.Size = new System.Drawing.Size(61, 21);
			this.lblCaption.TabIndex = 0;
			this.lblCaption.Text = "Caption";
			// 
			// lblColon
			// 
			this.lblColon.AutoSize = true;
			this.lblColon.Location = new System.Drawing.Point(58, 3);
			this.lblColon.Name = "lblColon";
			this.lblColon.Size = new System.Drawing.Size(16, 16);
			this.lblColon.TabIndex = 1;
			this.lblColon.Text = "：";
			// 
			// lblUnit
			// 
			this.lblUnit.Location = new System.Drawing.Point(159, 0);
			this.lblUnit.Name = "lblUnit";
			this.lblUnit.Size = new System.Drawing.Size(32, 23);
			this.lblUnit.TabIndex = 3;
			this.lblUnit.Text = "mm";
			this.lblUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// nudValue
			// 
			this.nudValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.nudValue.Location = new System.Drawing.Point(0, 0);
			this.nudValue.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.nudValue.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
			this.nudValue.Name = "nudValue";
			this.nudValue.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.nudValue.Size = new System.Drawing.Size(78, 19);
			this.nudValue.TabIndex = 2;
			this.nudValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.nudValue.ValueChanged += new System.EventHandler(this.nudValue_TextChanged);
			this.nudValue.Enter += new System.EventHandler(this.nudValue_Enter);
			this.nudValue.Validating += new System.ComponentModel.CancelEventHandler(this.nudValue_Validating);
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.nudValue);
			this.panel1.Location = new System.Drawing.Point(72, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(67, 22);
			this.panel1.TabIndex = 4;
			// 
			// NumTextBox
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.lblUnit);
			this.Controls.Add(this.lblColon);
			this.Controls.Add(this.lblCaption);
			this.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Name = "NumTextBox";
			this.Size = new System.Drawing.Size(194, 31);
			this.Resize += new System.EventHandler(this.NumTextBox_Resize);
			((System.ComponentModel.ISupportInitialize)(this.nudValue)).EndInit();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.Label lblColon;
        private System.Windows.Forms.Label lblUnit;
		private System.Windows.Forms.NumericUpDown nudValue;
		private System.Windows.Forms.Panel panel1;
    }
}
