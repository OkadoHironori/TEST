namespace Itc.Common.Controls
{
    partial class UC_NumUpdate
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.numData = new Itc.Common.Controls.NumUpDownForDecimal();
            this.update = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numData)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.numData, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.update, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(307, 65);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // numData
            // 
            this.numData.BindingSlider = null;
            this.numData.DecimalPlaces = 2;
            this.numData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numData.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numData.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numData.Location = new System.Drawing.Point(3, 8);
            this.numData.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.numData.Maximum = 5000F;
            this.numData.Minimum = -5000F;
            this.numData.Name = "numData";
            this.numData.Size = new System.Drawing.Size(147, 44);
            this.numData.TabIndex = 0;
            this.numData.Value = 0F;
            this.numData.ValueChanged += new System.EventHandler(this.numData_ValueChanged);
            // 
            // update
            // 
            this.update.Dock = System.Windows.Forms.DockStyle.Fill;
            this.update.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.update.Location = new System.Drawing.Point(156, 3);
            this.update.Name = "update";
            this.update.Size = new System.Drawing.Size(148, 59);
            this.update.TabIndex = 1;
            this.update.Text = "更新";
            this.update.UseVisualStyleBackColor = true;
            this.update.Click += new System.EventHandler(this.update_Click);
            // 
            // UC_NumUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UC_NumUpdate";
            this.Size = new System.Drawing.Size(307, 65);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Itc.Common.Controls.NumUpDownForDecimal numData;
        private System.Windows.Forms.Button update;
    }
}
