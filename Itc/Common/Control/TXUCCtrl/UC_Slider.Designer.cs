namespace Itc.Common.Controls.TXUCCtrl
{
    partial class UC_Slider
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
            this.bindSliderEx1 = new Itc.Common.Controls.BindSlider.BindSliderEx();
            this.floatUpDown1 = new Itc.Common.Controls.BindSlider.FloatUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.bindSliderEx1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.floatUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // bindSliderEx1
            // 
            this.bindSliderEx1.BindFloatUpdown = null;
            this.bindSliderEx1.Location = new System.Drawing.Point(164, 72);
            this.bindSliderEx1.Maximum = 10F;
            this.bindSliderEx1.Minimum = 0F;
            this.bindSliderEx1.Name = "bindSliderEx1";
            this.bindSliderEx1.Size = new System.Drawing.Size(185, 69);
            this.bindSliderEx1.TabIndex = 0;
            this.bindSliderEx1.ValueScale = 1F;
            // 
            // floatUpDown1
            // 
            this.floatUpDown1.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.floatUpDown1.Location = new System.Drawing.Point(19, 72);
            this.floatUpDown1.Maximum = 100F;
            this.floatUpDown1.Minimum = 0F;
            this.floatUpDown1.Name = "floatUpDown1";
            this.floatUpDown1.Size = new System.Drawing.Size(120, 36);
            this.floatUpDown1.Slider = null;
            this.floatUpDown1.TabIndex = 1;
            this.floatUpDown1.Value = 0F;
            // 
            // UC_Slider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.floatUpDown1);
            this.Controls.Add(this.bindSliderEx1);
            this.Name = "UC_Slider";
            this.Size = new System.Drawing.Size(439, 260);
            ((System.ComponentModel.ISupportInitialize)(this.bindSliderEx1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.floatUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BindSlider.BindSliderEx bindSliderEx1;
        private BindSlider.FloatUpDown floatUpDown1;
    }
}
