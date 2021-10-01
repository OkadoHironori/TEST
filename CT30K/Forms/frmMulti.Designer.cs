namespace CT30K
{
    partial class frmMulti
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
            this.lblSliceName = new System.Windows.Forms.Label();
            this.cmdEnd = new System.Windows.Forms.Button();
            this.cmdExe = new System.Windows.Forms.Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblMaxNum = new System.Windows.Forms.Label();
            this.lblNumTitle = new System.Windows.Forms.Label();
            this.cmdDel = new System.Windows.Forms.Button();
            this.cmdRef = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblSliceName
            // 
            this.lblSliceName.AutoSize = true;
            this.lblSliceName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblSliceName.Location = new System.Drawing.Point(16, 13);
            this.lblSliceName.Name = "lblSliceName";
            this.lblSliceName.Size = new System.Drawing.Size(94, 16);
            this.lblSliceName.TabIndex = 0;
            this.lblSliceName.Tag = "12762";
            this.lblSliceName.Text = "#スライス名：";
            // 
            // cmdEnd
            // 
            this.cmdEnd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdEnd.Location = new System.Drawing.Point(352, 288);
            this.cmdEnd.Name = "cmdEnd";
            this.cmdEnd.Size = new System.Drawing.Size(121, 33);
            this.cmdEnd.TabIndex = 14;
            this.cmdEnd.Tag = "10006";
            this.cmdEnd.Text = "#終　了";
            this.cmdEnd.UseVisualStyleBackColor = true;
            this.cmdEnd.Click += new System.EventHandler(this.cmdEnd_Click);
            // 
            // cmdExe
            // 
            this.cmdExe.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdExe.Location = new System.Drawing.Point(208, 288);
            this.cmdExe.Name = "cmdExe";
            this.cmdExe.Size = new System.Drawing.Size(121, 33);
            this.cmdExe.TabIndex = 13;
            this.cmdExe.Tag = "10005";
            this.cmdExe.Text = "#実　行";
            this.cmdExe.UseVisualStyleBackColor = true;
            this.cmdExe.Click += new System.EventHandler(this.cmdExe_Click);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCount.Location = new System.Drawing.Point(255, 256);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(16, 16);
            this.lblCount.TabIndex = 12;
            this.lblCount.Text = "0";
            // 
            // lblMaxNum
            // 
            this.lblMaxNum.AutoSize = true;
            this.lblMaxNum.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMaxNum.Location = new System.Drawing.Point(278, 256);
            this.lblMaxNum.Name = "lblMaxNum";
            this.lblMaxNum.Size = new System.Drawing.Size(104, 16);
            this.lblMaxNum.TabIndex = 11;
            this.lblMaxNum.Text = "枚（最大25枚）";
            // 
            // lblNumTitle
            // 
            this.lblNumTitle.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblNumTitle.Location = new System.Drawing.Point(28, 256);
            this.lblNumTitle.Name = "lblNumTitle";
            this.lblNumTitle.Size = new System.Drawing.Size(207, 16);
            this.lblNumTitle.TabIndex = 10;
            this.lblNumTitle.Tag = "12052";
            this.lblNumTitle.Text = "#入力枚数:";
            this.lblNumTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmdDel
            // 
            this.cmdDel.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdDel.Location = new System.Drawing.Point(384, 80);
            this.cmdDel.Name = "cmdDel";
            this.cmdDel.Size = new System.Drawing.Size(89, 33);
            this.cmdDel.TabIndex = 9;
            this.cmdDel.Tag = "10010";
            this.cmdDel.Text = "#削　除";
            this.cmdDel.UseVisualStyleBackColor = true;
            // 
            // cmdRef
            // 
            this.cmdRef.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdRef.Location = new System.Drawing.Point(384, 32);
            this.cmdRef.Name = "cmdRef";
            this.cmdRef.Size = new System.Drawing.Size(89, 33);
            this.cmdRef.TabIndex = 8;
            this.cmdRef.Tag = "10009";
            this.cmdRef.Text = "#参照...";
            this.cmdRef.UseVisualStyleBackColor = true;
            // 
            // frmMulti
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(491, 341);
            this.ControlBox = false;
            this.Controls.Add(this.cmdEnd);
            this.Controls.Add(this.cmdExe);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lblMaxNum);
            this.Controls.Add(this.lblNumTitle);
            this.Controls.Add(this.cmdDel);
            this.Controls.Add(this.cmdRef);
            this.Controls.Add(this.lblSliceName);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmMulti";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Tag = "12457";
            this.Text = "#マルチフレーム";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMulti_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMulti_FormClosed);
            this.Load += new System.EventHandler(this.frmMulti_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSliceName;
        private System.Windows.Forms.Button cmdEnd;
        private System.Windows.Forms.Button cmdExe;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label lblMaxNum;
        private System.Windows.Forms.Label lblNumTitle;
        private System.Windows.Forms.Button cmdDel;
        private System.Windows.Forms.Button cmdRef;
    }
}