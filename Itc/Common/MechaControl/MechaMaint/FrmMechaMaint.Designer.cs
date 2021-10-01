namespace MechaMaintCnt
{
    partial class FrmMechaMaint
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.UC_SelElement = new MechaMaintCnt.Selector.UC_SelAxis();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.CloseCmd = new Itc.Common.Controls.ButtonEx.ButtonEx();
            this.BtnStop = new Itc.Common.Controls.ButtonEx.ButtonEx();
            this.ProgMes = new Itc.Common.Controls.SpdTXTB();
            this.UC_MainPanel = new MechaMaintCnt.MainPanel.UC_MainPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.UC_SelElement, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.UC_MainPanel, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1116, 575);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // UC_SelElement
            // 
            this.UC_SelElement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UC_SelElement.Location = new System.Drawing.Point(4, 6);
            this.UC_SelElement.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.UC_SelElement.Name = "UC_SelElement";
            this.UC_SelElement.Size = new System.Drawing.Size(1108, 50);
            this.UC_SelElement.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 211F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.tableLayoutPanel2.Controls.Add(this.CloseCmd, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.BtnStop, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.ProgMes, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 498);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1110, 65);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // CloseCmd
            // 
            this.CloseCmd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseCmd.Font = new System.Drawing.Font("Meiryo UI", 11.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.CloseCmd.Location = new System.Drawing.Point(943, 3);
            this.CloseCmd.Name = "CloseCmd";
            this.CloseCmd.Size = new System.Drawing.Size(162, 59);
            this.CloseCmd.TabIndex = 0;
            this.CloseCmd.TabStop = false;
            this.CloseCmd.Text = "閉じる";
            this.CloseCmd.UseVisualStyleBackColor = true;
            this.CloseCmd.Click += new System.EventHandler(this.CloseCmd_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnStop.Font = new System.Drawing.Font("Meiryo UI", 11.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.BtnStop.Location = new System.Drawing.Point(773, 3);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(162, 59);
            this.BtnStop.TabIndex = 1;
            this.BtnStop.TabStop = false;
            this.BtnStop.Text = "停止";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // ProgMes
            // 
            this.ProgMes.BackColor = System.Drawing.SystemColors.Control;
            this.ProgMes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProgMes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ProgMes.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ProgMes.Location = new System.Drawing.Point(3, 26);
            this.ProgMes.Name = "ProgMes";
            this.ProgMes.Size = new System.Drawing.Size(553, 36);
            this.ProgMes.TabIndex = 2;
            this.ProgMes.TabStop = false;
            // 
            // UC_MainPanel
            // 
            this.UC_MainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UC_MainPanel.Location = new System.Drawing.Point(3, 65);
            this.UC_MainPanel.Name = "UC_MainPanel";
            this.UC_MainPanel.Size = new System.Drawing.Size(1110, 427);
            this.UC_MainPanel.TabIndex = 4;
            // 
            // FrmMechaMaint
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1116, 577);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "FrmMechaMaint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PLCメンテナンス";
            this.Shown += new System.EventHandler(this.FrmMechaMaint_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Selector.UC_SelAxis UC_SelElement;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Itc.Common.Controls.ButtonEx.ButtonEx CloseCmd;
        private MainPanel.UC_MainPanel UC_MainPanel;
        private Itc.Common.Controls.ButtonEx.ButtonEx BtnStop;
        private Itc.Common.Controls.SpdTXTB ProgMes;
    }
}