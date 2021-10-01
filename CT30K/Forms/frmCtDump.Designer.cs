namespace CT30K
{
    partial class frmCtDump
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCtDump));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblCtDump = new System.Windows.Forms.Label();
            this.CTExecFrame = new System.Windows.Forms.GroupBox();
            this.cmdMove1 = new System.Windows.Forms.Button();
            this.cmdMove3 = new System.Windows.Forms.Button();
            this.cmdMove2 = new System.Windows.Forms.Button();
            this.cmdMove0 = new System.Windows.Forms.Button();
            this.Shape1 = new System.Windows.Forms.Label();
            this.cwneMove = new System.Windows.Forms.NumericUpDown();
            this.lblMove = new System.Windows.Forms.Label();
            this.cmdEnd = new System.Windows.Forms.Button();
            this.MSFlexGrid1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CTExecFrame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneMove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MSFlexGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCtDump
            // 
            this.lblCtDump.AutoSize = true;
            this.lblCtDump.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F);
            this.lblCtDump.Location = new System.Drawing.Point(4, 4);
            this.lblCtDump.Name = "lblCtDump";
            this.lblCtDump.Size = new System.Drawing.Size(136, 12);
            this.lblCtDump.TabIndex = 0;
            this.lblCtDump.Text = "ROI座標：(x1, y1)-(x2, y2)";
            // 
            // CTExecFrame
            // 
            this.CTExecFrame.Controls.Add(this.cmdMove1);
            this.CTExecFrame.Controls.Add(this.cmdMove3);
            this.CTExecFrame.Controls.Add(this.cmdMove2);
            this.CTExecFrame.Controls.Add(this.cmdMove0);
            this.CTExecFrame.Controls.Add(this.Shape1);
            this.CTExecFrame.Controls.Add(this.cwneMove);
            this.CTExecFrame.Controls.Add(this.lblMove);
            this.CTExecFrame.Location = new System.Drawing.Point(8, 388);
            this.CTExecFrame.Name = "CTExecFrame";
            this.CTExecFrame.Size = new System.Drawing.Size(433, 101);
            this.CTExecFrame.TabIndex = 2;
            this.CTExecFrame.TabStop = false;
            // 
            // cmdMove1
            // 
            this.cmdMove1.Image = ((System.Drawing.Image)(resources.GetObject("cmdMove1.Image")));
            this.cmdMove1.Location = new System.Drawing.Point(92, 68);
            this.cmdMove1.Name = "cmdMove1";
            this.cmdMove1.Size = new System.Drawing.Size(27, 27);
            this.cmdMove1.TabIndex = 6;
            this.cmdMove1.UseVisualStyleBackColor = true;
            this.cmdMove1.Click += new System.EventHandler(this.cmdMove_Click);
            // 
            // cmdMove3
            // 
            this.cmdMove3.Image = ((System.Drawing.Image)(resources.GetObject("cmdMove3.Image")));
            this.cmdMove3.Location = new System.Drawing.Point(120, 40);
            this.cmdMove3.Name = "cmdMove3";
            this.cmdMove3.Size = new System.Drawing.Size(27, 27);
            this.cmdMove3.TabIndex = 5;
            this.cmdMove3.UseVisualStyleBackColor = true;
            this.cmdMove3.Click += new System.EventHandler(this.cmdMove_Click);
            // 
            // cmdMove2
            // 
            this.cmdMove2.Image = ((System.Drawing.Image)(resources.GetObject("cmdMove2.Image")));
            this.cmdMove2.Location = new System.Drawing.Point(64, 40);
            this.cmdMove2.Name = "cmdMove2";
            this.cmdMove2.Size = new System.Drawing.Size(27, 27);
            this.cmdMove2.TabIndex = 4;
            this.cmdMove2.UseVisualStyleBackColor = true;
            this.cmdMove2.Click += new System.EventHandler(this.cmdMove_Click);
            // 
            // cmdMove0
            // 
            this.cmdMove0.Image = ((System.Drawing.Image)(resources.GetObject("cmdMove0.Image")));
            this.cmdMove0.Location = new System.Drawing.Point(92, 12);
            this.cmdMove0.Name = "cmdMove0";
            this.cmdMove0.Size = new System.Drawing.Size(27, 27);
            this.cmdMove0.TabIndex = 3;
            this.cmdMove0.UseVisualStyleBackColor = true;
            this.cmdMove0.Click += new System.EventHandler(this.cmdMove_Click);
            // 
            // Shape1
            // 
            this.Shape1.BackColor = System.Drawing.SystemColors.Control;
            this.Shape1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Shape1.Location = new System.Drawing.Point(98, 45);
            this.Shape1.Name = "Shape1";
            this.Shape1.Size = new System.Drawing.Size(14, 18);
            this.Shape1.TabIndex = 10;
            this.Shape1.Paint += new System.Windows.Forms.PaintEventHandler(this.Shape1_Paint);
            // 
            // cwneMove
            // 
            this.cwneMove.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cwneMove.Location = new System.Drawing.Point(300, 40);
            this.cwneMove.Maximum = new decimal(new int[] {
            2047,
            0,
            0,
            0});
            this.cwneMove.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneMove.Name = "cwneMove";
            this.cwneMove.Size = new System.Drawing.Size(65, 23);
            this.cwneMove.TabIndex = 8;
            this.cwneMove.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblMove
            // 
            this.lblMove.AutoSize = true;
            this.lblMove.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMove.Location = new System.Drawing.Point(209, 44);
            this.lblMove.Name = "lblMove";
            this.lblMove.Size = new System.Drawing.Size(98, 15);
            this.lblMove.TabIndex = 7;
            this.lblMove.Tag = "12406";
            this.lblMove.Text = "#移動画素数：";
            // 
            // cmdEnd
            // 
            this.cmdEnd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11F);
            this.cmdEnd.Location = new System.Drawing.Point(464, 428);
            this.cmdEnd.Name = "cmdEnd";
            this.cmdEnd.Size = new System.Drawing.Size(77, 29);
            this.cmdEnd.TabIndex = 9;
            this.cmdEnd.Tag = "10006";
            this.cmdEnd.Text = "#終　了";
            this.cmdEnd.UseVisualStyleBackColor = true;
            this.cmdEnd.Click += new System.EventHandler(this.cmdEnd_Click);
            // 
            // MSFlexGrid1
            // 
            this.MSFlexGrid1.AllowUserToAddRows = false;
            this.MSFlexGrid1.AllowUserToDeleteRows = false;
            this.MSFlexGrid1.AllowUserToResizeColumns = false;
            this.MSFlexGrid1.AllowUserToResizeRows = false;
            this.MSFlexGrid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.MSFlexGrid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.MSFlexGrid1.ColumnHeadersVisible = false;
            this.MSFlexGrid1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column11,
            this.Column12,
            this.Column13,
            this.Column14,
            this.Column15,
            this.Column16});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.MSFlexGrid1.DefaultCellStyle = dataGridViewCellStyle3;
            this.MSFlexGrid1.Location = new System.Drawing.Point(4, 20);
            this.MSFlexGrid1.Name = "MSFlexGrid1";
            this.MSFlexGrid1.ReadOnly = true;
            this.MSFlexGrid1.RowHeadersVisible = false;
            this.MSFlexGrid1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.MSFlexGrid1.RowTemplate.Height = 21;
            this.MSFlexGrid1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MSFlexGrid1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.MSFlexGrid1.Size = new System.Drawing.Size(546, 363);
            this.MSFlexGrid1.TabIndex = 1;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Column3";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Column4";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Column5";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Column6";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Column7";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Column8";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Column9";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "Column10";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            this.Column10.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "Column11";
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            this.Column11.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column12
            // 
            this.Column12.HeaderText = "Column12";
            this.Column12.Name = "Column12";
            this.Column12.ReadOnly = true;
            this.Column12.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column13
            // 
            this.Column13.HeaderText = "Column13";
            this.Column13.Name = "Column13";
            this.Column13.ReadOnly = true;
            this.Column13.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column14
            // 
            this.Column14.HeaderText = "Column14";
            this.Column14.Name = "Column14";
            this.Column14.ReadOnly = true;
            this.Column14.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column15
            // 
            this.Column15.HeaderText = "Column15";
            this.Column15.Name = "Column15";
            this.Column15.ReadOnly = true;
            this.Column15.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column16
            // 
            this.Column16.HeaderText = "Column16";
            this.Column16.Name = "Column16";
            this.Column16.ReadOnly = true;
            this.Column16.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // frmCtDump
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(553, 501);
            this.ControlBox = false;
            this.Controls.Add(this.MSFlexGrid1);
            this.Controls.Add(this.cmdEnd);
            this.Controls.Add(this.CTExecFrame);
            this.Controls.Add(this.lblCtDump);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(107, 73);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCtDump";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Tag = "12452";
            this.Text = "#ＣＴ値表示";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCtDump_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCtDump_FormClosed);
            this.Load += new System.EventHandler(this.frmCtDump_Load);
            this.CTExecFrame.ResumeLayout(false);
            this.CTExecFrame.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneMove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MSFlexGrid1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.Label lblCtDump;
		private System.Windows.Forms.GroupBox CTExecFrame;
        private System.Windows.Forms.NumericUpDown cwneMove;
        private System.Windows.Forms.Label lblMove;
        private System.Windows.Forms.Button cmdEnd;
        private System.Windows.Forms.Label Shape1;
		private System.Windows.Forms.Button cmdMove1;
		private System.Windows.Forms.Button cmdMove3;
		private System.Windows.Forms.Button cmdMove2;
		private System.Windows.Forms.Button cmdMove0;
		private System.Windows.Forms.DataGridView MSFlexGrid1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column14;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column15;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column16;

    }
}