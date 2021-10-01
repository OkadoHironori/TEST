namespace CT30K
{
    partial class HoriVertForm
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
            this.lblHeader = new System.Windows.Forms.Label();
            this.cwneLine = new System.Windows.Forms.NumericUpDown();
            this.lblRange = new System.Windows.Forms.Label();
            this.cmdDisp = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOk = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.cwneLine)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(104, 32);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(39, 24);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "X : ";
            // 
            // cwneLine
            // 
            this.cwneLine.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneLine.Location = new System.Drawing.Point(142, 34);
            this.cwneLine.Name = "cwneLine";
            this.cwneLine.Size = new System.Drawing.Size(89, 23);
            this.cwneLine.TabIndex = 1;
            this.cwneLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblRange
            // 
            this.lblRange.AutoSize = true;
            this.lblRange.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRange.Location = new System.Drawing.Point(160, 72);
            this.lblRange.Name = "lblRange";
            this.lblRange.Size = new System.Drawing.Size(66, 16);
            this.lblRange.TabIndex = 2;
            this.lblRange.Text = "0 - 1023";
            // 
            // cmdDisp
            // 
            this.cmdDisp.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdDisp.Location = new System.Drawing.Point(227, 117);
            this.cmdDisp.Name = "cmdDisp";
            this.cmdDisp.Size = new System.Drawing.Size(89, 33);
            this.cmdDisp.TabIndex = 13;
            this.cmdDisp.Tag = "10011";
            this.cmdDisp.Text = "#表　示";
            this.cmdDisp.UseVisualStyleBackColor = true;
            this.cmdDisp.Click += new System.EventHandler(this.cmdDisp_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdCancel.Location = new System.Drawing.Point(127, 117);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(89, 33);
            this.cmdCancel.TabIndex = 12;
            this.cmdCancel.Tag = "10002";
            this.cmdCancel.Text = "#キャンセル";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOk
            // 
            this.cmdOk.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdOk.Location = new System.Drawing.Point(27, 117);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(89, 33);
            this.cmdOk.TabIndex = 11;
            this.cmdOk.Tag = "10001";
            this.cmdOk.Text = "#Ｏ　Ｋ";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // HoriVertForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(341, 162);
            this.ControlBox = false;
            this.Controls.Add(this.cmdDisp);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOk);
            this.Controls.Add(this.lblRange);
            this.Controls.Add(this.cwneLine);
            this.Controls.Add(this.lblHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "HoriVertForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "#HoriVertForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HoriVertForm_FormClosed);
            this.Load += new System.EventHandler(this.HoriVertForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cwneLine)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.NumericUpDown cwneLine;
        private System.Windows.Forms.Label lblRange;
        private System.Windows.Forms.Button cmdDisp;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOk;
    }
}