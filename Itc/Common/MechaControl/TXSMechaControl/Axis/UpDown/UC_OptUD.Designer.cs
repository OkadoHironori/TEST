namespace TXSMechaControl.UpDown
{
    partial class UC_OptUD
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
            this.label2 = new System.Windows.Forms.Label();
            this.UC_UD_BindSlider = new TXSMechaControl.CommonComp.UC_H_BindSlider();
            this.UC_SelCtrlSpd = new TXSMechaControl.CommonComp.UC_SelCtrlSpd();
            this.AramReset = new Itc.Common.Controls.MoveBtn.OriginBtn();
            this.SuspendLayout();
            // 
            // Stop
            // 
            this.Stop.BackColor = System.Drawing.SystemColors.Control;
            this.Stop.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Stop.Limit = Itc.Common.TXEnum.LimitMode.OnLimit;
            this.Stop.Location = new System.Drawing.Point(157, 519);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(194, 116);
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
            this.OriginBtn.Location = new System.Drawing.Point(435, 516);
            this.OriginBtn.Name = "OriginBtn";
            this.OriginBtn.Size = new System.Drawing.Size(169, 123);
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
            this.STS_TEXT.Location = new System.Drawing.Point(77, 79);
            this.STS_TEXT.Name = "STS_TEXT";
            this.STS_TEXT.Size = new System.Drawing.Size(307, 36);
            this.STS_TEXT.TabIndex = 13;
            this.STS_TEXT.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(812, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 18);
            this.label2.TabIndex = 17;
            this.label2.Text = "昇降";
            // 
            // UC_UD_BindSlider
            // 
            this.UC_UD_BindSlider.Location = new System.Drawing.Point(132, 288);
            this.UC_UD_BindSlider.Name = "UC_UD_BindSlider";
            this.UC_UD_BindSlider.Size = new System.Drawing.Size(940, 134);
            this.UC_UD_BindSlider.TabIndex = 20;
            // 
            // UC_SelCtrlSpd
            // 
            this.UC_SelCtrlSpd.Location = new System.Drawing.Point(132, 158);
            this.UC_SelCtrlSpd.Name = "UC_SelCtrlSpd";
            this.UC_SelCtrlSpd.Size = new System.Drawing.Size(516, 61);
            this.UC_SelCtrlSpd.TabIndex = 21;
            // 
            // AramReset
            // 
            this.AramReset.BackColor = System.Drawing.SystemColors.Control;
            this.AramReset.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.AramReset.Limit = Itc.Common.TXEnum.LimitMode.OnLimit;
            this.AramReset.Location = new System.Drawing.Point(687, 516);
            this.AramReset.Name = "AramReset";
            this.AramReset.Size = new System.Drawing.Size(169, 123);
            this.AramReset.TabIndex = 22;
            this.AramReset.TabStop = false;
            this.AramReset.Text = "アラームリセット";
            this.AramReset.UseVisualStyleBackColor = false;
            this.AramReset.Click += new System.EventHandler(this.AramReset_Click);
            // 
            // UC_OptUD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.AramReset);
            this.Controls.Add(this.UC_SelCtrlSpd);
            this.Controls.Add(this.UC_UD_BindSlider);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.OriginBtn);
            this.Controls.Add(this.STS_TEXT);
            this.Name = "UC_OptUD";
            this.Size = new System.Drawing.Size(1197, 762);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Itc.Common.Controls.SpdTXTB STS_TEXT;
        private Itc.Common.Controls.MoveBtn.OriginBtn OriginBtn;
        private Itc.Common.Controls.MoveBtn.OriginBtn Stop;
        private System.Windows.Forms.Label label2;
        private CommonComp.UC_H_BindSlider UC_UD_BindSlider;
        private CommonComp.UC_SelCtrlSpd UC_SelCtrlSpd;
        private Itc.Common.Controls.MoveBtn.OriginBtn AramReset;
    }
}
