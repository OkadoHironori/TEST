namespace CT30K
{
    partial class CTSliderV
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
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.lblBottom = new System.Windows.Forms.Label();
            this.lblTop = new System.Windows.Forms.Label();
            this.lblMid = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineTicksBot = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineTicksMid = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineTicksTop = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.pnlDummy = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar1.LargeChange = 5;
            this.vScrollBar1.Location = new System.Drawing.Point(0, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(21, 200);
            this.vScrollBar1.TabIndex = 4;
            this.vScrollBar1.TabStop = true;
            // 
            // lblBottom
            // 
            this.lblBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBottom.AutoSize = true;
            this.lblBottom.BackColor = System.Drawing.Color.Transparent;
            this.lblBottom.Location = new System.Drawing.Point(31, 179);
            this.lblBottom.Margin = new System.Windows.Forms.Padding(0);
            this.lblBottom.Name = "lblBottom";
            this.lblBottom.Size = new System.Drawing.Size(23, 12);
            this.lblBottom.TabIndex = 3;
            this.lblBottom.Text = "100";
            this.lblBottom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblBottom.AutoSizeChanged += new System.EventHandler(this.lblTop_AutoSizeChanged);
            this.lblBottom.Resize += new System.EventHandler(this.lblTop_Resize);
            // 
            // lblTop
            // 
            this.lblTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTop.AutoSize = true;
            this.lblTop.BackColor = System.Drawing.Color.Transparent;
            this.lblTop.Location = new System.Drawing.Point(31, 12);
            this.lblTop.Margin = new System.Windows.Forms.Padding(0);
            this.lblTop.Name = "lblTop";
            this.lblTop.Size = new System.Drawing.Size(11, 12);
            this.lblTop.TabIndex = 1;
            this.lblTop.Text = "0";
            this.lblTop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTop.AutoSizeChanged += new System.EventHandler(this.lblTop_AutoSizeChanged);
            this.lblTop.Resize += new System.EventHandler(this.lblTop_Resize);
            // 
            // lblMid
            // 
            this.lblMid.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblMid.AutoSize = true;
            this.lblMid.BackColor = System.Drawing.Color.Transparent;
            this.lblMid.Location = new System.Drawing.Point(31, 99);
            this.lblMid.Margin = new System.Windows.Forms.Padding(0);
            this.lblMid.Name = "lblMid";
            this.lblMid.Size = new System.Drawing.Size(17, 12);
            this.lblMid.TabIndex = 2;
            this.lblMid.Text = "50";
            this.lblMid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMid.AutoSizeChanged += new System.EventHandler(this.lblTop_AutoSizeChanged);
            this.lblMid.Resize += new System.EventHandler(this.lblTop_Resize);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineTicksBot,
            this.lineTicksMid,
            this.lineTicksTop});
            this.shapeContainer1.Size = new System.Drawing.Size(62, 200);
            this.shapeContainer1.TabIndex = 9;
            this.shapeContainer1.TabStop = false;
            // 
            // lineTicksBot
            // 
            this.lineTicksBot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lineTicksBot.Name = "lineTicksBot";
            this.lineTicksBot.X1 = 22;
            this.lineTicksBot.X2 = 27;
            this.lineTicksBot.Y1 = 184;
            this.lineTicksBot.Y2 = 184;
            // 
            // lineTicksMid
            // 
            this.lineTicksMid.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lineTicksMid.Name = "lineTicksMid";
            this.lineTicksMid.X1 = 22;
            this.lineTicksMid.X2 = 27;
            this.lineTicksMid.Y1 = 103;
            this.lineTicksMid.Y2 = 103;
            // 
            // lineTicksTop
            // 
            this.lineTicksTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lineTicksTop.Name = "lineTicksTop";
            this.lineTicksTop.X1 = 22;
            this.lineTicksTop.X2 = 27;
            this.lineTicksTop.Y1 = 17;
            this.lineTicksTop.Y2 = 17;
            // 
            // pnlDummy
            // 
            this.pnlDummy.Location = new System.Drawing.Point(27, 42);
            this.pnlDummy.Name = "pnlDummy";
            this.pnlDummy.Size = new System.Drawing.Size(21, 14);
            this.pnlDummy.TabIndex = 10;
            // 
            // CTSliderV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlDummy);
            this.Controls.Add(this.lblMid);
            this.Controls.Add(this.lblBottom);
            this.Controls.Add(this.lblTop);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.shapeContainer1);
            this.Name = "CTSliderV";
            this.Size = new System.Drawing.Size(62, 200);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.Label lblBottom;
        private System.Windows.Forms.Label lblTop;
        private System.Windows.Forms.Label lblMid;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineTicksBot;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineTicksMid;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineTicksTop;
        private System.Windows.Forms.Panel pnlDummy;
    }
}
