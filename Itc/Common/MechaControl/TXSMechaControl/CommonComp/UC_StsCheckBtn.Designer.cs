namespace TXSMechaControl.CommonComp
{
    partial class UC_StsCheckBtn
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblDisp = new Itc.Common.Controls.LabelEx.LabelEx();
            this.LED = new Itc.Common.Controls.LabelEx.LabelLED();
            this.Reset = new Itc.Common.Controls.MoveBtn.OriginBtn();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 119F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 109F));
            this.tableLayoutPanel1.Controls.Add(this.lblDisp, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.LED, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Reset, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(278, 65);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblDisp
            // 
            this.lblDisp.AutoSize = true;
            this.lblDisp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDisp.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDisp.Location = new System.Drawing.Point(33, 0);
            this.lblDisp.Name = "lblDisp";
            this.lblDisp.Size = new System.Drawing.Size(113, 65);
            this.lblDisp.TabIndex = 2;
            this.lblDisp.Text = "Ready";
            this.lblDisp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LED
            // 
            this.LED.AutoSize = true;
            this.LED.BackColor = System.Drawing.Color.Gray;
            this.LED.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LED.Dock = System.Windows.Forms.DockStyle.Left;
            this.LED.Enabled = false;
            this.LED.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LED.Location = new System.Drawing.Point(4, 0);
            this.LED.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LED.Name = "LED";
            this.LED.Size = new System.Drawing.Size(22, 65);
            this.LED.TabIndex = 1;
            this.LED.Text = "   ";
            this.LED.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Reset
            // 
            this.Reset.BackColor = System.Drawing.SystemColors.Control;
            this.Reset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Reset.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Reset.Limit = Itc.Common.TXEnum.LimitMode.OnLimit;
            this.Reset.Location = new System.Drawing.Point(152, 3);
            this.Reset.Name = "Reset";
            this.Reset.Size = new System.Drawing.Size(123, 59);
            this.Reset.TabIndex = 3;
            this.Reset.TabStop = false;
            this.Reset.Text = "Reset";
            this.Reset.UseVisualStyleBackColor = false;
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // UC_StsCheckBtn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UC_StsCheckBtn";
            this.Size = new System.Drawing.Size(278, 65);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Itc.Common.Controls.LabelEx.LabelLED LED;
        private Itc.Common.Controls.LabelEx.LabelEx lblDisp;
        private Itc.Common.Controls.MoveBtn.OriginBtn Reset;
    }
}
