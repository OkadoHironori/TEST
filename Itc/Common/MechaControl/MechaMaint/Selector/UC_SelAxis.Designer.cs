namespace MechaMaintCnt.Selector
{
    partial class UC_SelAxis
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
            this.TblY = new Itc.Common.Controls.CRadioButton();
            this.FCD = new Itc.Common.Controls.CRadioButton();
            this.FDD = new Itc.Common.Controls.CRadioButton();
            this.AUX = new Itc.Common.Controls.CRadioButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.Controls.Add(this.TblY, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.FCD, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.FDD, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.AUX, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(793, 76);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // TblY
            // 
            this.TblY.Appearance = System.Windows.Forms.Appearance.Button;
            this.TblY.AutoSize = true;
            this.TblY.BackColor = System.Drawing.SystemColors.Control;
            this.TblY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TblY.Location = new System.Drawing.Point(102, 3);
            this.TblY.Name = "TblY";
            this.TblY.Size = new System.Drawing.Size(93, 70);
            this.TblY.TabIndex = 5;
            this.TblY.Text = "Y軸";
            this.TblY.UseVisualStyleBackColor = false;
            this.TblY.CheckedChanged += new System.EventHandler(this.TblY_CheckedChanged);
            // 
            // FCD
            // 
            this.FCD.Appearance = System.Windows.Forms.Appearance.Button;
            this.FCD.AutoSize = true;
            this.FCD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FCD.Location = new System.Drawing.Point(3, 3);
            this.FCD.Name = "FCD";
            this.FCD.Size = new System.Drawing.Size(93, 70);
            this.FCD.TabIndex = 0;
            this.FCD.Text = "FCD";
            this.FCD.UseVisualStyleBackColor = true;
            this.FCD.CheckedChanged += new System.EventHandler(this.FCD_CheckedChanged);
            // 
            // FDD
            // 
            this.FDD.Appearance = System.Windows.Forms.Appearance.Button;
            this.FDD.AutoSize = true;
            this.FDD.BackColor = System.Drawing.SystemColors.Control;
            this.FDD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FDD.Location = new System.Drawing.Point(201, 3);
            this.FDD.Name = "FDD";
            this.FDD.Size = new System.Drawing.Size(93, 70);
            this.FDD.TabIndex = 1;
            this.FDD.Text = "FDD";
            this.FDD.UseVisualStyleBackColor = false;
            this.FDD.CheckedChanged += new System.EventHandler(this.FDD_CheckedChanged);
            // 
            // AUX
            // 
            this.AUX.Appearance = System.Windows.Forms.Appearance.Button;
            this.AUX.AutoSize = true;
            this.AUX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AUX.Location = new System.Drawing.Point(300, 3);
            this.AUX.Name = "AUX";
            this.AUX.Size = new System.Drawing.Size(93, 70);
            this.AUX.TabIndex = 2;
            this.AUX.TabStop = true;
            this.AUX.Text = "その他";
            this.AUX.UseVisualStyleBackColor = true;
            this.AUX.CheckedChanged += new System.EventHandler(this.AUX_CheckedChanged);
            // 
            // UC_SelAxis
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UC_SelAxis";
            this.Size = new System.Drawing.Size(793, 76);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Itc.Common.Controls.CRadioButton FCD;
        private Itc.Common.Controls.CRadioButton FDD;
        private Itc.Common.Controls.CRadioButton AUX;
        private Itc.Common.Controls.CRadioButton TblY;
    }
}
