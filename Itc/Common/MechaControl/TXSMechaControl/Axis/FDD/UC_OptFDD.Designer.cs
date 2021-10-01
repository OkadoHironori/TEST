namespace TXSMechaControl.FDD
{
    partial class UC_OptFDD
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
            this.label2 = new System.Windows.Forms.Label();
            this.STS_TEXT = new Itc.Common.Controls.SpdTXTB();
            this.OriginBtn = new Itc.Common.Controls.MoveBtn.OriginBtn();
            this.Stop = new Itc.Common.Controls.MoveBtn.OriginBtn();
            this.STS_TXT_MOVE = new Itc.Common.Controls.SpdTXTB();
            this.UC_ORIGIN = new TXSMechaControl.CommonComp.UC_NUM_Update();
            this.UC_SelSpdCtrl = new TXSMechaControl.CommonComp.UC_SelCtrlSpd();
            this.FDD_UC_BindSlider = new TXSMechaControl.CommonComp.UC_H_BindSlider();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(94, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "FDDです";
            // 
            // STS_TEXT
            // 
            this.STS_TEXT.BackColor = System.Drawing.SystemColors.Control;
            this.STS_TEXT.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.STS_TEXT.Location = new System.Drawing.Point(61, 85);
            this.STS_TEXT.Name = "STS_TEXT";
            this.STS_TEXT.Size = new System.Drawing.Size(307, 36);
            this.STS_TEXT.TabIndex = 13;
            this.STS_TEXT.TabStop = false;
            // 
            // OriginBtn
            // 
            this.OriginBtn.BackColor = System.Drawing.SystemColors.Control;
            this.OriginBtn.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.OriginBtn.Limit = Itc.Common.TXEnum.LimitMode.OnLimit;
            this.OriginBtn.Location = new System.Drawing.Point(897, 115);
            this.OriginBtn.Name = "OriginBtn";
            this.OriginBtn.Size = new System.Drawing.Size(169, 123);
            this.OriginBtn.TabIndex = 14;
            this.OriginBtn.TabStop = false;
            this.OriginBtn.Text = "原点復帰";
            this.OriginBtn.UseVisualStyleBackColor = false;
            this.OriginBtn.Click += new System.EventHandler(this.OriginBtn_Click);
            // 
            // Stop
            // 
            this.Stop.BackColor = System.Drawing.SystemColors.Control;
            this.Stop.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Stop.Limit = Itc.Common.TXEnum.LimitMode.OnLimit;
            this.Stop.Location = new System.Drawing.Point(897, 285);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(169, 123);
            this.Stop.TabIndex = 15;
            this.Stop.TabStop = false;
            this.Stop.Text = "停止";
            this.Stop.UseVisualStyleBackColor = false;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // STS_TXT_MOVE
            // 
            this.STS_TXT_MOVE.BackColor = System.Drawing.SystemColors.Control;
            this.STS_TXT_MOVE.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.STS_TXT_MOVE.Location = new System.Drawing.Point(422, 85);
            this.STS_TXT_MOVE.Name = "STS_TXT_MOVE";
            this.STS_TXT_MOVE.Size = new System.Drawing.Size(140, 36);
            this.STS_TXT_MOVE.TabIndex = 35;
            this.STS_TXT_MOVE.TabStop = false;
            // 
            // UC_ORIGIN
            // 
            this.UC_ORIGIN.Location = new System.Drawing.Point(61, 261);
            this.UC_ORIGIN.Name = "UC_ORIGIN";
            this.UC_ORIGIN.Size = new System.Drawing.Size(494, 69);
            this.UC_ORIGIN.TabIndex = 38;
            // 
            // UC_SelSpdCtrl
            // 
            this.UC_SelSpdCtrl.Location = new System.Drawing.Point(58, 149);
            this.UC_SelSpdCtrl.Name = "UC_SelSpdCtrl";
            this.UC_SelSpdCtrl.Size = new System.Drawing.Size(516, 61);
            this.UC_SelSpdCtrl.TabIndex = 37;
            // 
            // FDD_UC_BindSlider
            // 
            this.FDD_UC_BindSlider.Location = new System.Drawing.Point(58, 465);
            this.FDD_UC_BindSlider.Name = "FDD_UC_BindSlider";
            this.FDD_UC_BindSlider.Size = new System.Drawing.Size(886, 134);
            this.FDD_UC_BindSlider.TabIndex = 39;
            // 
            // UC_OptFDD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.FDD_UC_BindSlider);
            this.Controls.Add(this.UC_ORIGIN);
            this.Controls.Add(this.UC_SelSpdCtrl);
            this.Controls.Add(this.STS_TXT_MOVE);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.OriginBtn);
            this.Controls.Add(this.STS_TEXT);
            this.Controls.Add(this.label2);
            this.Name = "UC_OptFDD";
            this.Size = new System.Drawing.Size(1197, 762);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private Itc.Common.Controls.SpdTXTB STS_TEXT;
        private Itc.Common.Controls.MoveBtn.OriginBtn OriginBtn;
        private Itc.Common.Controls.MoveBtn.OriginBtn Stop;
        private Itc.Common.Controls.SpdTXTB STS_TXT_MOVE;
        private CommonComp.UC_SelCtrlSpd UC_SelSpdCtrl;
        private CommonComp.UC_NUM_Update UC_ORIGIN;
        private CommonComp.UC_H_BindSlider FDD_UC_BindSlider;
    }
}
