using Itc.Common.Controls;

namespace TXSMechaControl.CommonComp
{
    partial class UC_SelCtrlSpd
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
            this.SPD_CB = new Itc.Common.Controls.SpdSelCB();
            this.NTB_SPD = new Itc.Common.Controls.NumTextBoxEx.NumTextBoxEx();
            this.Unit = new Itc.Common.Controls.LabelEx.LabelEx();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(this.SPD_CB, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.NTB_SPD, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Unit, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(513, 61);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // SPD_CB
            // 
            this.SPD_CB.BackColor = System.Drawing.SystemColors.Control;
            this.SPD_CB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SPD_CB.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.SPD_CB.FormattingEnabled = true;
            this.SPD_CB.Location = new System.Drawing.Point(261, 10);
            this.SPD_CB.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.SPD_CB.Name = "SPD_CB";
            this.SPD_CB.Size = new System.Drawing.Size(249, 37);
            this.SPD_CB.TabIndex = 3;
            this.SPD_CB.TabStop = false;
            this.SPD_CB.SelectedIndexChanged += new System.EventHandler(this.SPD_CB_SelectedIndexChanged);
            // 
            // NTB_SPD
            // 
            this.NTB_SPD.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.NTB_SPD.Location = new System.Drawing.Point(3, 10);
            this.NTB_SPD.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.NTB_SPD.Name = "NTB_SPD";
            this.NTB_SPD.Size = new System.Drawing.Size(102, 36);
            this.NTB_SPD.TabIndex = 4;
            this.NTB_SPD.TabStop = false;
            this.NTB_SPD.ValueChanged += new System.EventHandler(this.NTB_SPD_ValueChanged);
            // 
            // Unit
            // 
            this.Unit.AutoSize = true;
            this.Unit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Unit.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Unit.Location = new System.Drawing.Point(111, 0);
            this.Unit.Name = "Unit";
            this.Unit.Size = new System.Drawing.Size(144, 61);
            this.Unit.TabIndex = 5;
            this.Unit.Text = "Unit";
            this.Unit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UC_SelCtrlSpd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UC_SelCtrlSpd";
            this.Size = new System.Drawing.Size(516, 61);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private SpdSelCB SPD_CB;
        private Itc.Common.Controls.NumTextBoxEx.NumTextBoxEx NTB_SPD;
        private Itc.Common.Controls.LabelEx.LabelEx Unit;
    }
}
