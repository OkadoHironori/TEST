namespace TXSMechaControl.MechaIntegrate
{
    partial class UC_Confirm
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
            this.NUD_EX = new Itc.Common.Controls.NumCtrl.NumUpDownEx();
            this.TBReadOnly = new Itc.Common.Controls.TextBoxReadOnly.TextBoxReadOnly();
            this.labelEx1 = new Itc.Common.Controls.LabelEx.LabelEx();
            this.labelEx2 = new Itc.Common.Controls.LabelEx.LabelEx();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_EX)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.NUD_EX, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.TBReadOnly, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelEx1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelEx2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(341, 106);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // NUD_EX
            // 
            this.NUD_EX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NUD_EX.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.NUD_EX.Location = new System.Drawing.Point(173, 63);
            this.NUD_EX.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.NUD_EX.Name = "NUD_EX";
            this.NUD_EX.Size = new System.Drawing.Size(165, 36);
            this.NUD_EX.TabIndex = 0;
            this.NUD_EX.TabStop = false;
            // 
            // TBReadOnly
            // 
            this.TBReadOnly.BackColor = System.Drawing.SystemColors.Control;
            this.TBReadOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBReadOnly.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TBReadOnly.Location = new System.Drawing.Point(173, 10);
            this.TBReadOnly.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.TBReadOnly.Name = "TBReadOnly";
            this.TBReadOnly.ReadOnly = true;
            this.TBReadOnly.Size = new System.Drawing.Size(165, 36);
            this.TBReadOnly.TabIndex = 1;
            this.TBReadOnly.TabStop = false;
            // 
            // labelEx1
            // 
            this.labelEx1.AutoSize = true;
            this.labelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEx1.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelEx1.Location = new System.Drawing.Point(3, 53);
            this.labelEx1.Name = "labelEx1";
            this.labelEx1.Size = new System.Drawing.Size(164, 53);
            this.labelEx1.TabIndex = 2;
            this.labelEx1.Text = "指定位置";
            this.labelEx1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelEx2
            // 
            this.labelEx2.AutoSize = true;
            this.labelEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEx2.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelEx2.Location = new System.Drawing.Point(3, 0);
            this.labelEx2.Name = "labelEx2";
            this.labelEx2.Size = new System.Drawing.Size(164, 53);
            this.labelEx2.TabIndex = 3;
            this.labelEx2.Text = "現在位置";
            this.labelEx2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UC_Confirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UC_Confirm";
            this.Size = new System.Drawing.Size(341, 106);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_EX)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Itc.Common.Controls.NumCtrl.NumUpDownEx NUD_EX;
        private Itc.Common.Controls.TextBoxReadOnly.TextBoxReadOnly TBReadOnly;
        private Itc.Common.Controls.LabelEx.LabelEx labelEx1;
        private Itc.Common.Controls.LabelEx.LabelEx labelEx2;
    }
}
