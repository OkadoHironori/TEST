namespace MechaMaintCnt.MainPanel
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
            this.FDD = new MechaMaintCnt.FDD.UC_FDD();
            this.FCD = new MechaMaintCnt.FCD.UC_FCD();
            this.AUX = new MechaMaintCnt.AuxSel.UC_OptAUX();
            this.TBLY = new MechaMaintCnt.TblY.UC_TblY();
            this.SuspendLayout();
            // 
            // FDD
            // 
            this.FDD._FDD = null;
            this.FDD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FDD.Location = new System.Drawing.Point(16, 210);
            this.FDD.Name = "FDD";
            this.FDD.Size = new System.Drawing.Size(1088, 286);
            this.FDD.TabIndex = 5;
            // 
            // FCD
            // 
            this.FCD._FCD = null;
            this.FCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FCD.Location = new System.Drawing.Point(6, 46);
            this.FCD.Name = "FCD";
            this.FCD.Size = new System.Drawing.Size(1098, 432);
            this.FCD.TabIndex = 4;
            // 
            // AUX
            // 
            this.AUX._AuxSel = null;
            this.AUX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AUX.Location = new System.Drawing.Point(296, -1);
            this.AUX.Name = "AUX";
            this.AUX.Size = new System.Drawing.Size(572, 253);
            this.AUX.TabIndex = 3;
            // 
            // TBLY
            // 
            this.TBLY._TblY = null;
            this.TBLY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TBLY.Location = new System.Drawing.Point(6, 7);
            this.TBLY.Name = "TBLY";
            this.TBLY.Size = new System.Drawing.Size(1071, 350);
            this.TBLY.TabIndex = 6;
            // 
            // UC_MainPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.TBLY);
            this.Controls.Add(this.FDD);
            this.Controls.Add(this.FCD);
            this.Controls.Add(this.AUX);
            this.Name = "UC_MainPanel";
            this.Size = new System.Drawing.Size(1080, 360);
            this.ResumeLayout(false);

        }

        #endregion


        private AuxSel.UC_OptAUX AUX;
        private FCD.UC_FCD FCD;
        private FDD.UC_FDD FDD;
        private TblY.UC_TblY TBLY;
    }
}
