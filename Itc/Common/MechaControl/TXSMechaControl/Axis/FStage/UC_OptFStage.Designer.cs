namespace TXSMechaControl.FStage
{
    partial class UC_OptFStage
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
            this.UC_SelSpd = new Itc.Common.Controls.TXUCCtrl.UC_SelSpd();
            this.UC_FY_BindSlider = new TXSMechaControl.CommonComp.UC_H_BindSlider();
            this.UC_FX_BindSlider = new TXSMechaControl.CommonComp.UC_H_BindSlider();
            this.SuspendLayout();
            // 
            // Stop
            // 
            this.Stop.BackColor = System.Drawing.SystemColors.Control;
            this.Stop.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Stop.Limit = Itc.Common.TXEnum.LimitMode.OnLimit;
            this.Stop.Location = new System.Drawing.Point(88, 516);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(156, 116);
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
            this.OriginBtn.Location = new System.Drawing.Point(301, 516);
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
            this.STS_TEXT.Location = new System.Drawing.Point(78, 29);
            this.STS_TEXT.Name = "STS_TEXT";
            this.STS_TEXT.Size = new System.Drawing.Size(307, 36);
            this.STS_TEXT.TabIndex = 13;
            this.STS_TEXT.TabStop = false;
            // 
            // UC_SelSpd
            // 
            this.UC_SelSpd.Location = new System.Drawing.Point(405, 19);
            this.UC_SelSpd.Name = "UC_SelSpd";
            this.UC_SelSpd.Size = new System.Drawing.Size(416, 61);
            this.UC_SelSpd.TabIndex = 5;
            // 
            // UC_FY_BindSlider
            // 
            this.UC_FY_BindSlider.Location = new System.Drawing.Point(78, 270);
            this.UC_FY_BindSlider.Name = "UC_FY_BindSlider";
            this.UC_FY_BindSlider.Size = new System.Drawing.Size(936, 134);
            this.UC_FY_BindSlider.TabIndex = 21;
            this.UC_FY_BindSlider.Load += new System.EventHandler(this.UC_FY_BindSlider_Load);
            // 
            // UC_FX_BindSlider
            // 
            this.UC_FX_BindSlider.Location = new System.Drawing.Point(78, 112);
            this.UC_FX_BindSlider.Name = "UC_FX_BindSlider";
            this.UC_FX_BindSlider.Size = new System.Drawing.Size(936, 134);
            this.UC_FX_BindSlider.TabIndex = 20;
            // 
            // UC_OptFStage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.UC_FY_BindSlider);
            this.Controls.Add(this.UC_FX_BindSlider);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.OriginBtn);
            this.Controls.Add(this.STS_TEXT);
            this.Controls.Add(this.UC_SelSpd);
            this.Name = "UC_OptFStage";
            this.Size = new System.Drawing.Size(1197, 762);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Itc.Common.Controls.TXUCCtrl.UC_SelSpd UC_SelSpd;
        private Itc.Common.Controls.SpdTXTB STS_TEXT;
        private Itc.Common.Controls.MoveBtn.OriginBtn OriginBtn;
        private Itc.Common.Controls.MoveBtn.OriginBtn Stop;
        private CommonComp.UC_H_BindSlider UC_FX_BindSlider;
        private CommonComp.UC_H_BindSlider UC_FY_BindSlider;
    }
}
