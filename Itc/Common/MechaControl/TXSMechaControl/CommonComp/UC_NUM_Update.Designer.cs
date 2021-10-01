namespace TXSMechaControl.CommonComp
{
    partial class UC_NUM_Update
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
            this.TXT_BOX = new Itc.Common.Controls.TextBoxReadOnly.TextBoxReadOnly();
            this.CONF_DATA = new TXSMechaControl.CommonComp.BindNUD_WOSld();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CONF_DATA)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 252F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.TXT_BOX, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.CONF_DATA, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(488, 63);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // TXT_BOX
            // 
            this.TXT_BOX.BackColor = System.Drawing.SystemColors.Control;
            this.TXT_BOX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TXT_BOX.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TXT_BOX.Location = new System.Drawing.Point(3, 10);
            this.TXT_BOX.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.TXT_BOX.Name = "TXT_BOX";
            this.TXT_BOX.ReadOnly = true;
            this.TXT_BOX.Size = new System.Drawing.Size(246, 38);
            this.TXT_BOX.TabIndex = 1;
            this.TXT_BOX.TabStop = false;
            // 
            // CONF_DATA
            // 
            this.CONF_DATA.DirectChanged = false;
            this.CONF_DATA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CONF_DATA.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.CONF_DATA.Location = new System.Drawing.Point(255, 12);
            this.CONF_DATA.Margin = new System.Windows.Forms.Padding(3, 12, 3, 3);
            this.CONF_DATA.Name = "CONF_DATA";
            this.CONF_DATA.Size = new System.Drawing.Size(230, 36);
            this.CONF_DATA.TabIndex = 2;
            this.CONF_DATA.TabStop = false;
            // 
            // UC_NUM_Update
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UC_NUM_Update";
            this.Size = new System.Drawing.Size(494, 69);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CONF_DATA)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Itc.Common.Controls.TextBoxReadOnly.TextBoxReadOnly TXT_BOX;
        private BindNUD_WOSld CONF_DATA;
    }
}
