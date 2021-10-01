namespace TXSMechaControl.MechaIntegrate
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
            this.selAxis = new TXSMechaControl.MechaIntegrate.UC_SelAxis();
            this.UC_MainPanel = new TXSMechaControl.MechaIntegrate.UC_MainPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.selAxis, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.UC_MainPanel, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1426, 1103);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // selAxis
            // 
            this.selAxis.Location = new System.Drawing.Point(3, 3);
            this.selAxis.Name = "selAxis";
            this.selAxis.Size = new System.Drawing.Size(793, 54);
            this.selAxis.TabIndex = 0;
            // 
            // UC_MainPanel
            // 
            this.UC_MainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UC_MainPanel.Location = new System.Drawing.Point(3, 63);
            this.UC_MainPanel.Name = "UC_MainPanel";
            this.UC_MainPanel.Size = new System.Drawing.Size(1420, 925);
            this.UC_MainPanel.TabIndex = 1;
            // 
            // FrmMechaMaint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1426, 1103);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FrmMechaMaint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmMechaMaint";
            this.Shown += new System.EventHandler(this.FrmMechaMaint_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private UC_SelAxis selAxis;
        private UC_MainPanel UC_MainPanel;
    }
}