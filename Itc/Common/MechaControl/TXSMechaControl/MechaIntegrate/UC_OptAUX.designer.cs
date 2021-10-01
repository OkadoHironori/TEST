namespace TXSMechaControl.MechaIntegrate
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
            this.SuspendLayout();
            // 
            // CB_DoorInterlock
            // 
            this.CB_DoorInterlock.Appearance = System.Windows.Forms.Appearance.Button;
            this.CB_DoorInterlock.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.CB_DoorInterlock.Location = new System.Drawing.Point(115, 140);
            this.CB_DoorInterlock.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.CB_DoorInterlock.Name = "CB_DoorInterlock";
            this.CB_DoorInterlock.Size = new System.Drawing.Size(207, 127);
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
            this.CB_XrayColiSensor.Location = new System.Drawing.Point(115, 324);
            this.CB_XrayColiSensor.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.CB_XrayColiSensor.Name = "CB_XrayColiSensor";
            this.CB_XrayColiSensor.Size = new System.Drawing.Size(207, 126);
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
            this.CB_XraySourceColi.Location = new System.Drawing.Point(115, 508);
            this.CB_XraySourceColi.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.CB_XraySourceColi.Name = "CB_XraySourceColi";
            this.CB_XraySourceColi.Size = new System.Drawing.Size(207, 126);
            this.CB_XraySourceColi.TabIndex = 6;
            this.CB_XraySourceColi.TabStop = false;
            this.CB_XraySourceColi.Text = "浜松ホト干渉";
            this.CB_XraySourceColi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CB_XraySourceColi.UnCheckedColor = System.Drawing.SystemColors.Control;
            this.CB_XraySourceColi.UseVisualStyleBackColor = false;
            this.CB_XraySourceColi.CheckedChanged += new System.EventHandler(this.CB_XraySourceColi_CheckedChanged);
            // 
            // UC_OptAUX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.CB_XraySourceColi);
            this.Controls.Add(this.CB_XrayColiSensor);
            this.Controls.Add(this.CB_DoorInterlock);
            this.Name = "UC_OptAUX";
            this.Size = new System.Drawing.Size(612, 741);
            this.ResumeLayout(false);

        }

        #endregion
        private Itc.Common.Controls.CheckButtonYellow CB_DoorInterlock;
        private Itc.Common.Controls.CheckButtonYellow CB_XrayColiSensor;
        private Itc.Common.Controls.CheckButtonYellow CB_XraySourceColi;
    }
}
