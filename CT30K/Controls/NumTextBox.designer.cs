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
            this.txtValue = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.nudValue)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCaption
            // 
            this.lblCaption.BackColor = System.Drawing.SystemColors.Control;
            this.lblCaption.Location = new System.Drawing.Point(0, 3);
            this.lblCaption.Margin = new System.Windows.Forms.Padding(0);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(61, 23);
            this.lblCaption.TabIndex = 0;
            this.lblCaption.Text = "Caption";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblColon
            // 
            this.lblColon.AutoSize = true;
            this.lblColon.Location = new System.Drawing.Point(58, 3);
            this.lblColon.Margin = new System.Windows.Forms.Padding(0);
            this.lblColon.Name = "lblColon";
            this.lblColon.Size = new System.Drawing.Size(16, 16);
            this.lblColon.TabIndex = 1;
            this.lblColon.Text = "：";
            this.lblColon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUnit
            // 
            this.lblUnit.AutoSize = true;
            this.lblUnit.BackColor = System.Drawing.SystemColors.Control;
            this.lblUnit.Location = new System.Drawing.Point(155, 0);
            this.lblUnit.Margin = new System.Windows.Forms.Padding(0);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(32, 16);
            this.lblUnit.TabIndex = 3;
            this.lblUnit.Text = "mm";
            this.lblUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudValue
            // 
            this.nudValue.BackColor = System.Drawing.SystemColors.Window;
            this.nudValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nudValue.Location = new System.Drawing.Point(65, 0);
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
            this.nudValue.Size = new System.Drawing.Size(15, 19);
            this.nudValue.TabIndex = 2;
            this.nudValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudValue.ValueChanged += new System.EventHandler(this.nudValue_ValueChanged);
            // 
            // txtValue
            // 
            this.txtValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtValue.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtValue.Location = new System.Drawing.Point(0, 0);
            this.txtValue.Margin = new System.Windows.Forms.Padding(0);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(65, 16);
            this.txtValue.TabIndex = 5;
            this.txtValue.Text = "00";
            this.txtValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            this.txtValue.Enter += new System.EventHandler(this.txtValue_Enter);
            this.txtValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValue_KeyPress);
            this.txtValue.Validating += new System.ComponentModel.CancelEventHandler(this.txtValue_Validating);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.txtValue);
            this.panel1.Controls.Add(this.nudValue);
            this.panel1.Location = new System.Drawing.Point(72, 3);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(81, 21);
            this.panel1.TabIndex = 4;
            // 
            // NumTextBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblColon);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.lblCaption);
            this.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "NumTextBox";
            this.Size = new System.Drawing.Size(194, 26);
            this.Load += new System.EventHandler(this.NumTextBox_Load);
            this.Resize += new System.EventHandler(this.NumTextBox_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.nudValue)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.Label lblColon;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.NumericUpDown nudValue;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Panel panel1;
    }
}
