namespace TXSMechaControl.MechaIntegrate
{
    partial class UC_MainPanel
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
            this.FDD = new TXSMechaControl.FDD.UC_OptFDD();
            this.ROT = new TXSMechaControl.Rotation.UC_OptRot();
            this.FCD = new TXSMechaControl.MechaIntegrate.UC_OptFCD();
            this.AUX = new TXSMechaControl.MechaIntegrate.UC_OptAUX();
            this.UPDOWN = new TXSMechaControl.UpDown.UC_OptUD();
            this.FSTAGE = new TXSMechaControl.FStage.UC_OptFStage();
            this.TBLY = new TXSMechaControl.TblY.UC_OptTblY();
            this.SuspendLayout();
            // 
            // FDD
            // 
            this.FDD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FDD.Location = new System.Drawing.Point(27, 3);
            this.FDD.Mecha = null;
            this.FDD.Name = "FDD";
            this.FDD.Size = new System.Drawing.Size(1290, 827);
            this.FDD.TabIndex = 1;
            // 
            // ROT
            // 
            this.ROT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ROT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ROT.Location = new System.Drawing.Point(0, 0);
            this.ROT.Mecha = null;
            this.ROT.Name = "ROT";
            this.ROT.Size = new System.Drawing.Size(1352, 859);
            this.ROT.TabIndex = 0;
            // 
            // FCD
            // 
            this.FCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FCD.Location = new System.Drawing.Point(0, 0);
            this.FCD.Mecha = null;
            this.FCD.Name = "FCD";
            this.FCD.Size = new System.Drawing.Size(1352, 859);
            this.FCD.TabIndex = 2;
            // 
            // AUX
            // 
            this.AUX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AUX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AUX.Location = new System.Drawing.Point(0, 0);
            this.AUX.Mecha = null;
            this.AUX.Name = "AUX";
            this.AUX.Size = new System.Drawing.Size(1352, 859);
            this.AUX.TabIndex = 3;
            // 
            // UPDOWN
            // 
            this.UPDOWN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UPDOWN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UPDOWN.Location = new System.Drawing.Point(0, 0);
            this.UPDOWN.Mecha = null;
            this.UPDOWN.Name = "UPDOWN";
            this.UPDOWN.Size = new System.Drawing.Size(1352, 859);
            this.UPDOWN.TabIndex = 4;
            // 
            // FSTAGE
            // 
            this.FSTAGE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FSTAGE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FSTAGE.Location = new System.Drawing.Point(0, 0);
            this.FSTAGE.Mecha = null;
            this.FSTAGE.Name = "FSTAGE";
            this.FSTAGE.Size = new System.Drawing.Size(1352, 859);
            this.FSTAGE.TabIndex = 5;
            // 
            // TBLY
            // 
            this.TBLY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TBLY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TBLY.Location = new System.Drawing.Point(0, 0);
            this.TBLY.Mecha = null;
            this.TBLY.Name = "TBLY";
            this.TBLY.Size = new System.Drawing.Size(1352, 859);
            this.TBLY.TabIndex = 6;
            // 
            // UC_MainPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.AUX);
            this.Controls.Add(this.FCD);
            this.Controls.Add(this.FDD);
            this.Controls.Add(this.FSTAGE);
            this.Controls.Add(this.UPDOWN);
            this.Controls.Add(this.ROT);
            this.Controls.Add(this.TBLY);
            this.Name = "UC_MainPanel";
            this.Size = new System.Drawing.Size(1352, 859);
            this.ResumeLayout(false);

        }

        #endregion

        private Rotation.UC_OptRot ROT;
        private FDD.UC_OptFDD FDD;
        private UC_OptFCD FCD;
        private UC_OptAUX AUX;
        private UpDown.UC_OptUD UPDOWN;
        private FStage.UC_OptFStage FSTAGE;
        private TblY.UC_OptTblY TBLY;
    }
}
