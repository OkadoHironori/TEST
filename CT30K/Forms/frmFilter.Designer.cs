namespace CT30K
{
    partial class frmFilter
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
            this.fraFilter = new System.Windows.Forms.GroupBox();
            this.cmdExe = new System.Windows.Forms.Button();
            this.fraParameter = new System.Windows.Forms.GroupBox();
            this.lblSizeRange = new System.Windows.Forms.Label();
            this.cwneSize = new System.Windows.Forms.NumericUpDown();
            this.lblSize = new System.Windows.Forms.Label();
            this.lblPassesRange = new System.Windows.Forms.Label();
            this.lblPasses = new System.Windows.Forms.Label();
            this.cwnePasses = new System.Windows.Forms.NumericUpDown();
            this.fraStrength = new System.Windows.Forms.Panel();
            this.lblStrengthRange = new System.Windows.Forms.Label();
            this.lblStrength = new System.Windows.Forms.Label();
            this.cwneStrength = new System.Windows.Forms.NumericUpDown();
            this.fraMatrix = new System.Windows.Forms.Panel();
            this.lblMatrix = new System.Windows.Forms.Label();
            this.cmdUndo = new System.Windows.Forms.Button();
            this.cmdEnd = new System.Windows.Forms.Button();
            this.fraParameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwnePasses)).BeginInit();
            this.fraStrength.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneStrength)).BeginInit();
            this.fraMatrix.SuspendLayout();
            this.SuspendLayout();
            // 
            // fraFilter
            // 
            this.fraFilter.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraFilter.Location = new System.Drawing.Point(12, 12);
            this.fraFilter.Name = "fraFilter";
            this.fraFilter.Size = new System.Drawing.Size(381, 85);
            this.fraFilter.TabIndex = 0;
            this.fraFilter.TabStop = false;
            this.fraFilter.Tag = "12163";
            this.fraFilter.Text = "#フィルタ";
            // 
            // cmdExe
            // 
            this.cmdExe.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdExe.Location = new System.Drawing.Point(16, 232);
            this.cmdExe.Name = "cmdExe";
            this.cmdExe.Size = new System.Drawing.Size(105, 33);
            this.cmdExe.TabIndex = 3;
            this.cmdExe.Tag = "10005";
            this.cmdExe.Text = "#実　行";
            this.cmdExe.UseVisualStyleBackColor = true;
            this.cmdExe.Click += new System.EventHandler(this.cmdExe_Click);
            // 
            // fraParameter
            // 
            this.fraParameter.Controls.Add(this.lblSizeRange);
            this.fraParameter.Controls.Add(this.cwneSize);
            this.fraParameter.Controls.Add(this.lblSize);
            this.fraParameter.Controls.Add(this.lblPassesRange);
            this.fraParameter.Controls.Add(this.lblPasses);
            this.fraParameter.Controls.Add(this.cwnePasses);
            this.fraParameter.Controls.Add(this.fraStrength);
            this.fraParameter.Controls.Add(this.fraMatrix);
            this.fraParameter.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.fraParameter.Location = new System.Drawing.Point(11, 104);
            this.fraParameter.Name = "fraParameter";
            this.fraParameter.Size = new System.Drawing.Size(381, 122);
            this.fraParameter.TabIndex = 4;
            this.fraParameter.TabStop = false;
            this.fraParameter.Tag = "12703";
            this.fraParameter.Text = "#パラメータ";
            // 
            // lblSizeRange
            // 
            this.lblSizeRange.AutoSize = true;
            this.lblSizeRange.Location = new System.Drawing.Point(285, 58);
            this.lblSizeRange.Name = "lblSizeRange";
            this.lblSizeRange.Size = new System.Drawing.Size(58, 16);
            this.lblSizeRange.TabIndex = 14;
            this.lblSizeRange.Text = "(3～31)";
            // 
            // cwneSize
            // 
            this.cwneSize.Location = new System.Drawing.Point(214, 56);
            this.cwneSize.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.cwneSize.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.cwneSize.Name = "cwneSize";
            this.cwneSize.Size = new System.Drawing.Size(65, 23);
            this.cwneSize.TabIndex = 13;
            this.cwneSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneSize.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Location = new System.Drawing.Point(141, 58);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(68, 16);
            this.lblSize.TabIndex = 12;
            this.lblSize.Tag = "16011";
            this.lblSize.Text = "#サイズ：";
            // 
            // lblPassesRange
            // 
            this.lblPassesRange.AutoSize = true;
            this.lblPassesRange.Location = new System.Drawing.Point(285, 27);
            this.lblPassesRange.Name = "lblPassesRange";
            this.lblPassesRange.Size = new System.Drawing.Size(58, 16);
            this.lblPassesRange.TabIndex = 11;
            this.lblPassesRange.Text = "(1～10)";
            // 
            // lblPasses
            // 
            this.lblPasses.AutoSize = true;
            this.lblPasses.Location = new System.Drawing.Point(142, 27);
            this.lblPasses.Name = "lblPasses";
            this.lblPasses.Size = new System.Drawing.Size(56, 16);
            this.lblPasses.TabIndex = 10;
            this.lblPasses.Tag = "16012";
            this.lblPasses.Text = "#回数：";
            // 
            // cwnePasses
            // 
            this.cwnePasses.Location = new System.Drawing.Point(214, 24);
            this.cwnePasses.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.cwnePasses.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwnePasses.Name = "cwnePasses";
            this.cwnePasses.Size = new System.Drawing.Size(65, 23);
            this.cwnePasses.TabIndex = 9;
            this.cwnePasses.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwnePasses.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // fraStrength
            // 
            this.fraStrength.Controls.Add(this.lblStrengthRange);
            this.fraStrength.Controls.Add(this.lblStrength);
            this.fraStrength.Controls.Add(this.cwneStrength);
            this.fraStrength.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.fraStrength.Location = new System.Drawing.Point(116, 71);
            this.fraStrength.Name = "fraStrength";
            this.fraStrength.Size = new System.Drawing.Size(259, 45);
            this.fraStrength.TabIndex = 7;
            // 
            // lblStrengthRange
            // 
            this.lblStrengthRange.AutoSize = true;
            this.lblStrengthRange.Location = new System.Drawing.Point(169, 19);
            this.lblStrengthRange.Name = "lblStrengthRange";
            this.lblStrengthRange.Size = new System.Drawing.Size(58, 16);
            this.lblStrengthRange.TabIndex = 5;
            this.lblStrengthRange.Text = "(1～10)";
            // 
            // lblStrength
            // 
            this.lblStrength.AutoSize = true;
            this.lblStrength.Location = new System.Drawing.Point(27, 19);
            this.lblStrength.Name = "lblStrength";
            this.lblStrength.Size = new System.Drawing.Size(52, 16);
            this.lblStrength.TabIndex = 4;
            this.lblStrength.Tag = "16013";
            this.lblStrength.Text = "#強さ：";
            // 
            // cwneStrength
            // 
            this.cwneStrength.Location = new System.Drawing.Point(98, 16);
            this.cwneStrength.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.cwneStrength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneStrength.Name = "cwneStrength";
            this.cwneStrength.Size = new System.Drawing.Size(65, 23);
            this.cwneStrength.TabIndex = 3;
            this.cwneStrength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneStrength.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // fraMatrix
            // 
            this.fraMatrix.Controls.Add(this.lblMatrix);
            this.fraMatrix.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.fraMatrix.Location = new System.Drawing.Point(6, 14);
            this.fraMatrix.Name = "fraMatrix";
            this.fraMatrix.Size = new System.Drawing.Size(104, 102);
            this.fraMatrix.TabIndex = 7;
            // 
            // lblMatrix
            // 
            this.lblMatrix.AutoSize = true;
            this.lblMatrix.Location = new System.Drawing.Point(5, 13);
            this.lblMatrix.Name = "lblMatrix";
            this.lblMatrix.Size = new System.Drawing.Size(76, 16);
            this.lblMatrix.TabIndex = 0;
            this.lblMatrix.Tag = "16014";
            this.lblMatrix.Text = "#ﾏﾄﾘｯｸｽ：";
            // 
            // cmdUndo
            // 
            this.cmdUndo.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdUndo.Location = new System.Drawing.Point(144, 232);
            this.cmdUndo.Name = "cmdUndo";
            this.cmdUndo.Size = new System.Drawing.Size(105, 33);
            this.cmdUndo.TabIndex = 5;
            this.cmdUndo.Tag = "10017";
            this.cmdUndo.Text = "#元に戻す";
            this.cmdUndo.UseVisualStyleBackColor = true;
            this.cmdUndo.Click += new System.EventHandler(this.cmdUndo_Click);
            // 
            // cmdEnd
            // 
            this.cmdEnd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdEnd.Location = new System.Drawing.Point(272, 232);
            this.cmdEnd.Name = "cmdEnd";
            this.cmdEnd.Size = new System.Drawing.Size(105, 33);
            this.cmdEnd.TabIndex = 6;
            this.cmdEnd.Tag = "10006";
            this.cmdEnd.Text = "#終　了";
            this.cmdEnd.UseVisualStyleBackColor = true;
            this.cmdEnd.Click += new System.EventHandler(this.cmdEnd_Click);
            // 
            // frmFilter
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(402, 279);
            this.ControlBox = false;
            this.Controls.Add(this.cmdEnd);
            this.Controls.Add(this.cmdUndo);
            this.Controls.Add(this.fraParameter);
            this.Controls.Add(this.cmdExe);
            this.Controls.Add(this.fraFilter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmFilter";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Tag = "12833";
            this.Text = "#フィルタ処理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmFilter_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmFilter_FormClosed);
            this.Load += new System.EventHandler(this.frmFilter_Load);
            this.fraParameter.ResumeLayout(false);
            this.fraParameter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwnePasses)).EndInit();
            this.fraStrength.ResumeLayout(false);
            this.fraStrength.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneStrength)).EndInit();
            this.fraMatrix.ResumeLayout(false);
            this.fraMatrix.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox fraFilter;
        private System.Windows.Forms.Button cmdExe;
        private System.Windows.Forms.GroupBox fraParameter;
        private System.Windows.Forms.Button cmdUndo;
        private System.Windows.Forms.Button cmdEnd;
        private System.Windows.Forms.Label lblMatrix;
        private System.Windows.Forms.Label lblStrengthRange;
        private System.Windows.Forms.Label lblStrength;
        private System.Windows.Forms.NumericUpDown cwneStrength;
        private System.Windows.Forms.Label lblSizeRange;
        private System.Windows.Forms.NumericUpDown cwneSize;
        private System.Windows.Forms.Label lblSize;
        private System.Windows.Forms.Label lblPassesRange;
        private System.Windows.Forms.Label lblPasses;
        private System.Windows.Forms.NumericUpDown cwnePasses;
        private System.Windows.Forms.Panel fraMatrix;
        private System.Windows.Forms.Panel fraStrength;
    }
}