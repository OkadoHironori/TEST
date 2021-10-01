namespace TXSMechaControl.CommonComp
{
    partial class UC_StsCheck
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
            this.LED = new Itc.Common.Controls.LabelEx.LabelLED();
            this.lblDisp = new Itc.Common.Controls.LabelEx.LabelEx();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            this.tableLayoutPanel1.Controls.Add(this.LED, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblDisp, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(180, 73);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // LED
            // 
            this.LED.AutoSize = true;
            this.LED.BackColor = System.Drawing.Color.Gray;
            this.LED.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LED.Dock = System.Windows.Forms.DockStyle.Left;
            this.LED.Enabled = false;
            this.LED.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LED.Location = new System.Drawing.Point(4, 4);
            this.LED.Margin = new System.Windows.Forms.Padding(4);
            this.LED.Name = "LED";
            this.LED.Size = new System.Drawing.Size(22, 65);
            this.LED.TabIndex = 0;
            this.LED.Text = "   ";
            this.LED.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDisp
            // 
            this.lblDisp.AutoSize = true;
            this.lblDisp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDisp.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDisp.Location = new System.Drawing.Point(33, 0);
            this.lblDisp.Name = "lblDisp";
            this.lblDisp.Size = new System.Drawing.Size(144, 73);
            this.lblDisp.TabIndex = 1;
            this.lblDisp.Text = "Ready";
            this.lblDisp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UC_StsCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UC_StsCheck";
            this.Size = new System.Drawing.Size(180, 73);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Itc.Common.Controls.LabelEx.LabelLED LED;
        private Itc.Common.Controls.LabelEx.LabelEx lblDisp;
    }
}
