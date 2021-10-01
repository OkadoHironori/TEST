namespace TXSMechaControl.MechaIntegrate
{
    partial class UC_OptFCD
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
            this.STS_TXT_MOVE = new Itc.Common.Controls.SpdTXTB();
            this.STS_TEXT = new Itc.Common.Controls.SpdTXTB();
            this.StopBtn = new Itc.Common.Controls.MoveBtn.OriginBtn();
            this.OriginBtn = new Itc.Common.Controls.MoveBtn.OriginBtn();
            this.LTable_TXTB = new Itc.Common.Controls.SpdTXTB();
            this.UC_SelCtrlSpd = new TXSMechaControl.CommonComp.UC_SelCtrlSpd();
            this.FCD_UC_BindSlider = new TXSMechaControl.CommonComp.UC_H_BindSlider();
            this.UC_HOME = new TXSMechaControl.CommonComp.UC_NUM_Update();
            this.UC_ORIGIN = new TXSMechaControl.CommonComp.UC_NUM_Update();
            this.UC_HAMA1 = new TXSMechaControl.CommonComp.UC_NUM_Update();
            this.UC_HAMA2 = new TXSMechaControl.CommonComp.UC_NUM_Update();
            this.SuspendLayout();
            // 
            // STS_TXT_MOVE
            // 
            this.STS_TXT_MOVE.BackColor = System.Drawing.SystemColors.Control;
            this.STS_TXT_MOVE.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.STS_TXT_MOVE.Location = new System.Drawing.Point(377, 35);
            this.STS_TXT_MOVE.Name = "STS_TXT_MOVE";
            this.STS_TXT_MOVE.Size = new System.Drawing.Size(136, 36);
            this.STS_TXT_MOVE.TabIndex = 13;
            this.STS_TXT_MOVE.TabStop = false;
            // 
            // STS_TEXT
            // 
            this.STS_TEXT.BackColor = System.Drawing.SystemColors.Control;
            this.STS_TEXT.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.STS_TEXT.Location = new System.Drawing.Point(19, 35);
            this.STS_TEXT.Name = "STS_TEXT";
            this.STS_TEXT.Size = new System.Drawing.Size(307, 36);
            this.STS_TEXT.TabIndex = 12;
            this.STS_TEXT.TabStop = false;
            // 
            // StopBtn
            // 
            this.StopBtn.BackColor = System.Drawing.SystemColors.Control;
            this.StopBtn.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.StopBtn.Limit = Itc.Common.TXEnum.LimitMode.OnLimit;
            this.StopBtn.Location = new System.Drawing.Point(861, 213);
            this.StopBtn.Name = "StopBtn";
            this.StopBtn.Size = new System.Drawing.Size(178, 95);
            this.StopBtn.TabIndex = 10;
            this.StopBtn.TabStop = false;
            this.StopBtn.Text = "ストップ";
            this.StopBtn.UseVisualStyleBackColor = false;
            this.StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            // 
            // OriginBtn
            // 
            this.OriginBtn.BackColor = System.Drawing.SystemColors.Control;
            this.OriginBtn.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.OriginBtn.Limit = Itc.Common.TXEnum.LimitMode.OnLimit;
            this.OriginBtn.Location = new System.Drawing.Point(861, 58);
            this.OriginBtn.Name = "OriginBtn";
            this.OriginBtn.Size = new System.Drawing.Size(178, 95);
            this.OriginBtn.TabIndex = 8;
            this.OriginBtn.TabStop = false;
            this.OriginBtn.Text = "リセット";
            this.OriginBtn.UseVisualStyleBackColor = false;
            this.OriginBtn.Click += new System.EventHandler(this.OriginBtn_Click);
            // 
            // LTable_TXTB
            // 
            this.LTable_TXTB.BackColor = System.Drawing.SystemColors.Control;
            this.LTable_TXTB.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LTable_TXTB.Location = new System.Drawing.Point(574, 35);
            this.LTable_TXTB.Name = "LTable_TXTB";
            this.LTable_TXTB.Size = new System.Drawing.Size(226, 36);
            this.LTable_TXTB.TabIndex = 20;
            this.LTable_TXTB.TabStop = false;
            // 
            // UC_SelCtrlSpd
            // 
            this.UC_SelCtrlSpd.Location = new System.Drawing.Point(29, 118);
            this.UC_SelCtrlSpd.Name = "UC_SelCtrlSpd";
            this.UC_SelCtrlSpd.Size = new System.Drawing.Size(516, 61);
            this.UC_SelCtrlSpd.TabIndex = 21;
            // 
            // FCD_UC_BindSlider
            // 
            this.FCD_UC_BindSlider.Location = new System.Drawing.Point(19, 560);
            this.FCD_UC_BindSlider.Name = "FCD_UC_BindSlider";
            this.FCD_UC_BindSlider.Size = new System.Drawing.Size(886, 134);
            this.FCD_UC_BindSlider.TabIndex = 22;
            // 
            // UC_HOME
            // 
            this.UC_HOME.Location = new System.Drawing.Point(29, 239);
            this.UC_HOME.Name = "UC_HOME";
            this.UC_HOME.Size = new System.Drawing.Size(494, 69);
            this.UC_HOME.TabIndex = 23;
            // 
            // UC_ORIGIN
            // 
            this.UC_ORIGIN.Location = new System.Drawing.Point(29, 314);
            this.UC_ORIGIN.Name = "UC_ORIGIN";
            this.UC_ORIGIN.Size = new System.Drawing.Size(494, 69);
            this.UC_ORIGIN.TabIndex = 24;
            // 
            // UC_HAMA1
            // 
            this.UC_HAMA1.Location = new System.Drawing.Point(29, 389);
            this.UC_HAMA1.Name = "UC_HAMA1";
            this.UC_HAMA1.Size = new System.Drawing.Size(494, 69);
            this.UC_HAMA1.TabIndex = 25;
            // 
            // UC_HAMA2
            // 
            this.UC_HAMA2.Location = new System.Drawing.Point(29, 464);
            this.UC_HAMA2.Name = "UC_HAMA2";
            this.UC_HAMA2.Size = new System.Drawing.Size(494, 69);
            this.UC_HAMA2.TabIndex = 26;
            // 
            // UC_OptFCD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.UC_HAMA2);
            this.Controls.Add(this.UC_HAMA1);
            this.Controls.Add(this.UC_ORIGIN);
            this.Controls.Add(this.UC_HOME);
            this.Controls.Add(this.FCD_UC_BindSlider);
            this.Controls.Add(this.UC_SelCtrlSpd);
            this.Controls.Add(this.LTable_TXTB);
            this.Controls.Add(this.STS_TXT_MOVE);
            this.Controls.Add(this.STS_TEXT);
            this.Controls.Add(this.StopBtn);
            this.Controls.Add(this.OriginBtn);
            this.Name = "UC_OptFCD";
            this.Size = new System.Drawing.Size(1207, 983);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Itc.Common.Controls.MoveBtn.OriginBtn OriginBtn;
        private Itc.Common.Controls.MoveBtn.OriginBtn StopBtn;
        private Itc.Common.Controls.SpdTXTB STS_TEXT;
        private Itc.Common.Controls.SpdTXTB STS_TXT_MOVE;
        //private FCD.UC_FCDPosiUpdate UC_FCDHome;
        //private FCD.UC_FCDPosiUpdate UC_FCDOrigin;
        //private FCD.UC_FCDPosiUpdate UC_HAMA_FCD1;
        //private FCD.UC_FCDPosiUpdate UC_HAMA_FCD2;
        private Itc.Common.Controls.SpdTXTB LTable_TXTB;
        private CommonComp.UC_SelCtrlSpd UC_SelCtrlSpd;
        private CommonComp.UC_H_BindSlider FCD_UC_BindSlider;
        private CommonComp.UC_NUM_Update UC_HOME;
        private CommonComp.UC_NUM_Update UC_ORIGIN;
        private CommonComp.UC_NUM_Update UC_HAMA1;
        private CommonComp.UC_NUM_Update UC_HAMA2;
    }
}
