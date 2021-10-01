namespace TXSMechaControl.Rotation
{
    partial class UC_OptRot
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
            this.Stop = new Itc.Common.Controls.MoveBtn.OriginBtn();
            this.OriginBtn = new Itc.Common.Controls.MoveBtn.OriginBtn();
            this.STS_TEXT = new Itc.Common.Controls.SpdTXTB();
            this.UC_Ready = new TXSMechaControl.CommonComp.UC_StsCheck();
            this.UC_Alarm = new TXSMechaControl.CommonComp.UC_StsCheckBtn();
            this.UC_SelCtrlSpd = new TXSMechaControl.CommonComp.UC_SelCtrlSpd();
            this.UC_ROT_BindSlider = new TXSMechaControl.CommonComp.UC_H_BindSlider();
            this.SuspendLayout();
            // 
            // Stop
            // 
            this.Stop.BackColor = System.Drawing.SystemColors.Control;
            this.Stop.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Stop.Limit = Itc.Common.TXEnum.LimitMode.OnLimit;
            this.Stop.Location = new System.Drawing.Point(104, 472);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(147, 66);
            this.Stop.TabIndex = 15;
            this.Stop.TabStop = false;
            this.Stop.Text = "停止";
            this.Stop.UseVisualStyleBackColor = false;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // OriginBtn
            // 
            this.OriginBtn.BackColor = System.Drawing.SystemColors.Control;
            this.OriginBtn.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.OriginBtn.Limit = Itc.Common.TXEnum.LimitMode.OnLimit;
            this.OriginBtn.Location = new System.Drawing.Point(310, 469);
            this.OriginBtn.Name = "OriginBtn";
            this.OriginBtn.Size = new System.Drawing.Size(141, 72);
            this.OriginBtn.TabIndex = 14;
            this.OriginBtn.TabStop = false;
            this.OriginBtn.Text = "原点復帰";
            this.OriginBtn.UseVisualStyleBackColor = false;
            this.OriginBtn.Click += new System.EventHandler(this.OriginBtn_Click);
            // 
            // STS_TEXT
            // 
            this.STS_TEXT.BackColor = System.Drawing.SystemColors.Control;
            this.STS_TEXT.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.STS_TEXT.Location = new System.Drawing.Point(104, 79);
            this.STS_TEXT.Name = "STS_TEXT";
            this.STS_TEXT.Size = new System.Drawing.Size(307, 36);
            this.STS_TEXT.TabIndex = 13;
            this.STS_TEXT.TabStop = false;
            // 
            // UC_Ready
            // 
            this.UC_Ready.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UC_Ready.Location = new System.Drawing.Point(549, 476);
            this.UC_Ready.Name = "UC_Ready";
            this.UC_Ready.Size = new System.Drawing.Size(150, 65);
            this.UC_Ready.TabIndex = 22;
            // 
            // UC_Alarm
            // 
            this.UC_Alarm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UC_Alarm.Location = new System.Drawing.Point(729, 476);
            this.UC_Alarm.Name = "UC_Alarm";
            this.UC_Alarm.Size = new System.Drawing.Size(278, 65);
            this.UC_Alarm.TabIndex = 21;
            // 
            // UC_SelCtrlSpd
            // 
            this.UC_SelCtrlSpd.Location = new System.Drawing.Point(104, 159);
            this.UC_SelCtrlSpd.Name = "UC_SelCtrlSpd";
            this.UC_SelCtrlSpd.Size = new System.Drawing.Size(516, 61);
            this.UC_SelCtrlSpd.TabIndex = 20;
            // 
            // UC_ROT_BindSlider
            // 
            this.UC_ROT_BindSlider.Location = new System.Drawing.Point(104, 263);
            this.UC_ROT_BindSlider.Name = "UC_ROT_BindSlider";
            this.UC_ROT_BindSlider.Size = new System.Drawing.Size(903, 134);
            this.UC_ROT_BindSlider.TabIndex = 19;
            this.UC_ROT_BindSlider.Load += new System.EventHandler(this.UC_ROT_BindSlider_Load);
            // 
            // UC_OptRot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.UC_Ready);
            this.Controls.Add(this.UC_Alarm);
            this.Controls.Add(this.UC_SelCtrlSpd);
            this.Controls.Add(this.UC_ROT_BindSlider);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.OriginBtn);
            this.Controls.Add(this.STS_TEXT);
            this.Name = "UC_OptRot";
            this.Size = new System.Drawing.Size(1197, 762);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Itc.Common.Controls.SpdTXTB STS_TEXT;
        private Itc.Common.Controls.MoveBtn.OriginBtn OriginBtn;
        private Itc.Common.Controls.MoveBtn.OriginBtn Stop;
        private CommonComp.UC_H_BindSlider UC_ROT_BindSlider;
        private CommonComp.UC_SelCtrlSpd UC_SelCtrlSpd;
        private CommonComp.UC_StsCheckBtn UC_Alarm;
        private CommonComp.UC_StsCheck UC_Ready;
    }
}
