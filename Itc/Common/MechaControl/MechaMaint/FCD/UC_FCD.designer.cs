namespace MechaMaintCnt.FCD
{
    partial class UC_FCD
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
            this.OriginBtn = new Itc.Common.Controls.MoveBtn.OriginBtn();
            this.UC_HOME = new TXSMechaControl.CommonComp.UC_NUM_Update();
            this.UC_ORIGIN = new TXSMechaControl.CommonComp.UC_NUM_Update();
            this.UC_HAMA1 = new TXSMechaControl.CommonComp.UC_NUM_Update();
            this.UC_HAMA2 = new TXSMechaControl.CommonComp.UC_NUM_Update();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.UC_BindSlider = new MechaMaintCnt.CommonUC.UC_MM_BindSlider();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // OriginBtn
            // 
            this.OriginBtn.BackColor = System.Drawing.SystemColors.Control;
            this.OriginBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.OriginBtn.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.OriginBtn.Limit = Itc.Common.TXEnum.LimitMode.OnLimit;
            this.OriginBtn.Location = new System.Drawing.Point(897, 3);
            this.OriginBtn.Name = "OriginBtn";
            this.OriginBtn.Size = new System.Drawing.Size(174, 75);
            this.OriginBtn.TabIndex = 9;
            this.OriginBtn.TabStop = false;
            this.OriginBtn.Text = "リセット";
            this.OriginBtn.UseVisualStyleBackColor = false;
            this.OriginBtn.Click += new System.EventHandler(this.OriginBtn_Click);
            // 
            // UC_HOME
            // 
            this.UC_HOME.Location = new System.Drawing.Point(3, 3);
            this.UC_HOME.Name = "UC_HOME";
            this.UC_HOME.Size = new System.Drawing.Size(494, 63);
            this.UC_HOME.TabIndex = 24;
            // 
            // UC_ORIGIN
            // 
            this.UC_ORIGIN.Location = new System.Drawing.Point(540, 3);
            this.UC_ORIGIN.Name = "UC_ORIGIN";
            this.UC_ORIGIN.Size = new System.Drawing.Size(494, 63);
            this.UC_ORIGIN.TabIndex = 25;
            // 
            // UC_HAMA1
            // 
            this.UC_HAMA1.Location = new System.Drawing.Point(3, 77);
            this.UC_HAMA1.Name = "UC_HAMA1";
            this.UC_HAMA1.Size = new System.Drawing.Size(494, 63);
            this.UC_HAMA1.TabIndex = 26;
            // 
            // UC_HAMA2
            // 
            this.UC_HAMA2.Location = new System.Drawing.Point(540, 77);
            this.UC_HAMA2.Name = "UC_HAMA2";
            this.UC_HAMA2.Size = new System.Drawing.Size(494, 63);
            this.UC_HAMA2.TabIndex = 27;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1080, 380);
            this.tableLayoutPanel1.TabIndex = 28;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.UC_HOME, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.UC_HAMA2, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.UC_HAMA1, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.UC_ORIGIN, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 153);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1074, 224);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tableLayoutPanel2.Controls.Add(this.UC_BindSlider, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.OriginBtn, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1074, 144);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // UC_BindSlider
            // 
            this.UC_BindSlider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UC_BindSlider.Location = new System.Drawing.Point(3, 3);
            this.UC_BindSlider.Name = "UC_BindSlider";
            this.UC_BindSlider.Size = new System.Drawing.Size(888, 138);
            this.UC_BindSlider.TabIndex = 0;
            // 
            // UC_FCD
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UC_FCD";
            this.Size = new System.Drawing.Size(1080, 380);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CommonUC.UC_MM_BindSlider UC_BindSlider;
        private Itc.Common.Controls.MoveBtn.OriginBtn OriginBtn;
        private TXSMechaControl.CommonComp.UC_NUM_Update UC_HOME;
        private TXSMechaControl.CommonComp.UC_NUM_Update UC_ORIGIN;
        private TXSMechaControl.CommonComp.UC_NUM_Update UC_HAMA1;
        private TXSMechaControl.CommonComp.UC_NUM_Update UC_HAMA2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}
