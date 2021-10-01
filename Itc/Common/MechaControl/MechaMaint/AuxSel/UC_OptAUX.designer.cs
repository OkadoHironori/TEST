namespace MechaMaintCnt.AuxSel
{
    partial class UC_OptAUX
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
            this.CB_DoorInterlock = new Itc.Common.Controls.CheckButtonYellow();
            this.CB_XrayColiSensor = new Itc.Common.Controls.CheckButtonYellow();
            this.CB_XraySourceColi = new Itc.Common.Controls.CheckButtonYellow();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CB_DoorInterlock
            // 
            this.CB_DoorInterlock.Appearance = System.Windows.Forms.Appearance.Button;
            this.CB_DoorInterlock.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.CB_DoorInterlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CB_DoorInterlock.Location = new System.Drawing.Point(5, 4);
            this.CB_DoorInterlock.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.CB_DoorInterlock.Name = "CB_DoorInterlock";
            this.CB_DoorInterlock.Size = new System.Drawing.Size(260, 87);
            this.CB_DoorInterlock.TabIndex = 4;
            this.CB_DoorInterlock.TabStop = false;
            this.CB_DoorInterlock.Text = "扉インターロック";
            this.CB_DoorInterlock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CB_DoorInterlock.UnCheckedColor = System.Drawing.SystemColors.Control;
            this.CB_DoorInterlock.UseVisualStyleBackColor = false;
            this.CB_DoorInterlock.CheckedChanged += new System.EventHandler(this.CB_DoorInterlock_CheckedChanged);
            // 
            // CB_XrayColiSensor
            // 
            this.CB_XrayColiSensor.Appearance = System.Windows.Forms.Appearance.Button;
            this.CB_XrayColiSensor.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.CB_XrayColiSensor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CB_XrayColiSensor.Location = new System.Drawing.Point(275, 4);
            this.CB_XrayColiSensor.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.CB_XrayColiSensor.Name = "CB_XrayColiSensor";
            this.CB_XrayColiSensor.Size = new System.Drawing.Size(260, 87);
            this.CB_XrayColiSensor.TabIndex = 5;
            this.CB_XrayColiSensor.TabStop = false;
            this.CB_XrayColiSensor.Text = "X線接触センサ";
            this.CB_XrayColiSensor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CB_XrayColiSensor.UnCheckedColor = System.Drawing.SystemColors.Control;
            this.CB_XrayColiSensor.UseVisualStyleBackColor = false;
            this.CB_XrayColiSensor.CheckedChanged += new System.EventHandler(this.CB_XrayColiSensor_CheckedChanged);
            // 
            // CB_XraySourceColi
            // 
            this.CB_XraySourceColi.Appearance = System.Windows.Forms.Appearance.Button;
            this.CB_XraySourceColi.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.CB_XraySourceColi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CB_XraySourceColi.Location = new System.Drawing.Point(545, 4);
            this.CB_XraySourceColi.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.CB_XraySourceColi.Name = "CB_XraySourceColi";
            this.CB_XraySourceColi.Size = new System.Drawing.Size(260, 87);
            this.CB_XraySourceColi.TabIndex = 6;
            this.CB_XraySourceColi.TabStop = false;
            this.CB_XraySourceColi.Text = "浜松ホト干渉";
            this.CB_XraySourceColi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CB_XraySourceColi.UnCheckedColor = System.Drawing.SystemColors.Control;
            this.CB_XraySourceColi.UseVisualStyleBackColor = false;
            this.CB_XraySourceColi.CheckedChanged += new System.EventHandler(this.CB_XraySourceColi_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.CB_DoorInterlock, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.CB_XraySourceColi, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.CB_XrayColiSensor, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1080, 380);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // UC_OptAUX
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UC_OptAUX";
            this.Size = new System.Drawing.Size(1080, 380);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Itc.Common.Controls.CheckButtonYellow CB_DoorInterlock;
        private Itc.Common.Controls.CheckButtonYellow CB_XrayColiSensor;
        private Itc.Common.Controls.CheckButtonYellow CB_XraySourceColi;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
