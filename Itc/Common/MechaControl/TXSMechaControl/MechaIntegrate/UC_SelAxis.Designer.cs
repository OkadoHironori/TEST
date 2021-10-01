namespace TXSMechaControl.MechaIntegrate
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
            this.UpDown = new Itc.Common.Controls.CRadioButton();
            this.TblY = new Itc.Common.Controls.CRadioButton();
            this.FStage = new Itc.Common.Controls.CRadioButton();
            this.Rot = new Itc.Common.Controls.CRadioButton();
            this.FCD = new Itc.Common.Controls.CRadioButton();
            this.FDD = new Itc.Common.Controls.CRadioButton();
            this.AUXSel = new Itc.Common.Controls.CRadioButton();
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
            this.tableLayoutPanel1.Controls.Add(this.UpDown, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.TblY, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.FStage, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Rot, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.FCD, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.FDD, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.AUXSel, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(793, 76);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // UpDown
            // 
            this.UpDown.Appearance = System.Windows.Forms.Appearance.Button;
            this.UpDown.AutoSize = true;
            this.UpDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UpDown.Location = new System.Drawing.Point(300, 3);
            this.UpDown.Name = "UpDown";
            this.UpDown.Size = new System.Drawing.Size(93, 70);
            this.UpDown.TabIndex = 6;
            this.UpDown.TabStop = true;
            this.UpDown.Text = "昇降";
            this.UpDown.UseVisualStyleBackColor = true;
            this.UpDown.CheckedChanged += new System.EventHandler(this.UpDown_CheckedChanged);
            // 
            // TblY
            // 
            this.TblY.Appearance = System.Windows.Forms.Appearance.Button;
            this.TblY.AutoSize = true;
            this.TblY.BackColor = System.Drawing.SystemColors.Control;
            this.TblY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TblY.Location = new System.Drawing.Point(399, 3);
            this.TblY.Name = "TblY";
            this.TblY.Size = new System.Drawing.Size(93, 70);
            this.TblY.TabIndex = 5;
            this.TblY.Text = "Y軸";
            this.TblY.UseVisualStyleBackColor = false;
            this.TblY.CheckedChanged += new System.EventHandler(this.TblY_CheckedChanged);
            // 
            // FStage
            // 
            this.FStage.Appearance = System.Windows.Forms.Appearance.Button;
            this.FStage.AutoSize = true;
            this.FStage.BackColor = System.Drawing.SystemColors.Control;
            this.FStage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FStage.Location = new System.Drawing.Point(201, 3);
            this.FStage.Name = "FStage";
            this.FStage.Size = new System.Drawing.Size(93, 70);
            this.FStage.TabIndex = 4;
            this.FStage.Text = "微調";
            this.FStage.UseVisualStyleBackColor = false;
            this.FStage.CheckedChanged += new System.EventHandler(this.FStage_CheckedChanged);
            // 
            // Rot
            // 
            this.Rot.Appearance = System.Windows.Forms.Appearance.Button;
            this.Rot.AutoSize = true;
            this.Rot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Rot.Location = new System.Drawing.Point(3, 3);
            this.Rot.Name = "Rot";
            this.Rot.Size = new System.Drawing.Size(93, 70);
            this.Rot.TabIndex = 3;
            this.Rot.TabStop = true;
            this.Rot.Text = "回転";
            this.Rot.UseVisualStyleBackColor = true;
            this.Rot.CheckedChanged += new System.EventHandler(this.Rot_CheckedChanged);
            // 
            // FCD
            // 
            this.FCD.Appearance = System.Windows.Forms.Appearance.Button;
            this.FCD.AutoSize = true;
            this.FCD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FCD.Location = new System.Drawing.Point(102, 3);
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
            this.FDD.Location = new System.Drawing.Point(498, 3);
            this.FDD.Name = "FDD";
            this.FDD.Size = new System.Drawing.Size(93, 70);
            this.FDD.TabIndex = 1;
            this.FDD.Text = "FDD";
            this.FDD.UseVisualStyleBackColor = false;
            this.FDD.CheckedChanged += new System.EventHandler(this.FDD_CheckedChanged);
            // 
            // AUXSel
            // 
            this.AUXSel.Appearance = System.Windows.Forms.Appearance.Button;
            this.AUXSel.AutoSize = true;
            this.AUXSel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AUXSel.Location = new System.Drawing.Point(597, 3);
            this.AUXSel.Name = "AUXSel";
            this.AUXSel.Size = new System.Drawing.Size(93, 70);
            this.AUXSel.TabIndex = 2;
            this.AUXSel.TabStop = true;
            this.AUXSel.Text = "その他";
            this.AUXSel.UseVisualStyleBackColor = true;
            this.AUXSel.CheckedChanged += new System.EventHandler(this.AUX_CheckedChanged);
            // 
            // UC_SelAxis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
        private Itc.Common.Controls.CRadioButton AUXSel;
        private Itc.Common.Controls.CRadioButton Rot;
        private Itc.Common.Controls.CRadioButton TblY;
        private Itc.Common.Controls.CRadioButton FStage;
        private Itc.Common.Controls.CRadioButton UpDown;
    }
}
