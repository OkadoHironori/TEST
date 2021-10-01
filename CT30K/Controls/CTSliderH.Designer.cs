namespace CT30K
{
    partial class CTSliderH
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
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
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.lblMax = new System.Windows.Forms.Label();
            this.lblMin = new System.Windows.Forms.Label();
            this.lineTicksMid = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineTicksMax = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineTicksMin = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lblMid = new System.Windows.Forms.Label();
            this.pnlDummy = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar1.LargeChange = 5;
            this.hScrollBar1.Location = new System.Drawing.Point(0, 0);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(300, 20);
            this.hScrollBar1.TabIndex = 4;
            this.hScrollBar1.TabStop = true;
            // 
            // lblMax
            // 
            this.lblMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMax.AutoSize = true;
            this.lblMax.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMax.Location = new System.Drawing.Point(272, 32);
            this.lblMax.Margin = new System.Windows.Forms.Padding(0);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new System.Drawing.Size(23, 12);
            this.lblMax.TabIndex = 3;
            this.lblMax.Text = "100";
            this.lblMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMax.AutoSizeChanged += new System.EventHandler(this.lblTicks_AutoSizeChanged);
            this.lblMax.Resize += new System.EventHandler(this.lblTicks_Resize);
            // 
            // lblMin
            // 
            this.lblMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMin.AutoSize = true;
            this.lblMin.Location = new System.Drawing.Point(11, 32);
            this.lblMin.Margin = new System.Windows.Forms.Padding(0);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(11, 12);
            this.lblMin.TabIndex = 1;
            this.lblMin.Text = "0";
            this.lblMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMin.AutoSizeChanged += new System.EventHandler(this.lblTicks_AutoSizeChanged);
            this.lblMin.Resize += new System.EventHandler(this.lblTicks_Resize);
            // 
            // lineTicksMid
            // 
            this.lineTicksMid.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lineTicksMid.Name = "lineTicksMid";
            this.lineTicksMid.X1 = 149;
            this.lineTicksMid.X2 = 149;
            this.lineTicksMid.Y1 = 23;
            this.lineTicksMid.Y2 = 28;
            // 
            // lineTicksMax
            // 
            this.lineTicksMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lineTicksMax.Name = "lineTicksMax";
            this.lineTicksMax.X1 = 283;
            this.lineTicksMax.X2 = 283;
            this.lineTicksMax.Y1 = 23;
            this.lineTicksMax.Y2 = 28;
            // 
            // lineTicksMin
            // 
            this.lineTicksMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lineTicksMin.Name = "lineTicksMin";
            this.lineTicksMin.X1 = 16;
            this.lineTicksMin.X2 = 16;
            this.lineTicksMin.Y1 = 23;
            this.lineTicksMin.Y2 = 28;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineTicksMin,
            this.lineTicksMax,
            this.lineTicksMid});
            this.shapeContainer1.Size = new System.Drawing.Size(300, 46);
            this.shapeContainer1.TabIndex = 8;
            this.shapeContainer1.TabStop = false;
            // 
            // lblMid
            // 
            this.lblMid.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblMid.AutoSize = true;
            this.lblMid.Location = new System.Drawing.Point(141, 32);
            this.lblMid.Margin = new System.Windows.Forms.Padding(0);
            this.lblMid.Name = "lblMid";
            this.lblMid.Size = new System.Drawing.Size(17, 12);
            this.lblMid.TabIndex = 2;
            this.lblMid.Text = "50";
            this.lblMid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlDummy
            // 
            this.pnlDummy.Location = new System.Drawing.Point(40, 29);
            this.pnlDummy.Name = "pnlDummy";
            this.pnlDummy.Size = new System.Drawing.Size(21, 14);
            this.pnlDummy.TabIndex = 9;
            // 
            // CTSliderH
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlDummy);
            this.Controls.Add(this.lblMid);
            this.Controls.Add(this.lblMax);
            this.Controls.Add(this.lblMin);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.shapeContainer1);
            this.Name = "CTSliderH";
            this.Size = new System.Drawing.Size(300, 46);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.Label lblMax;
        private System.Windows.Forms.Label lblMin;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineTicksMid;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineTicksMax;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineTicksMin;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private System.Windows.Forms.Label lblMid;
        private System.Windows.Forms.Panel pnlDummy;
    }
}
