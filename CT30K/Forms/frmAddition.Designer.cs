namespace CT30K
{
    partial class frmAddition
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
            this.cmdRef = new System.Windows.Forms.Button();
            this.cmdImgDelete = new System.Windows.Forms.Button();
            this.lblNumTitle = new System.Windows.Forms.Label();
            this.lblMaxNum = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.AddGo = new System.Windows.Forms.Button();
            this.AddEnd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblSliceName
            // 
            this.lblSliceName.AutoSize = true;
            this.lblSliceName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblSliceName.Location = new System.Drawing.Point(16, 13);
            this.lblSliceName.Name = "lblSliceName";
            this.lblSliceName.Size = new System.Drawing.Size(94, 16);
            this.lblSliceName.TabIndex = 0;
            this.lblSliceName.Tag = "12762";
            this.lblSliceName.Text = "#スライス名：";
            // 
            // cmdRef
            // 
            this.cmdRef.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdRef.Location = new System.Drawing.Point(384, 32);
            this.cmdRef.Name = "cmdRef";
            this.cmdRef.Size = new System.Drawing.Size(89, 33);
            this.cmdRef.TabIndex = 1;
            this.cmdRef.Tag = "10009";
            this.cmdRef.Text = "#参照...";
            this.cmdRef.UseVisualStyleBackColor = true;
            // 
            // cmdImgDelete
            // 
            this.cmdImgDelete.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdImgDelete.Location = new System.Drawing.Point(384, 80);
            this.cmdImgDelete.Name = "cmdImgDelete";
            this.cmdImgDelete.Size = new System.Drawing.Size(89, 33);
            this.cmdImgDelete.TabIndex = 2;
            this.cmdImgDelete.Tag = "10010";
            this.cmdImgDelete.Text = "#削　除";
            this.cmdImgDelete.UseVisualStyleBackColor = true;
            // 
            // lblNumTitle
            // 
            this.lblNumTitle.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblNumTitle.Location = new System.Drawing.Point(33, 256);
            this.lblNumTitle.Name = "lblNumTitle";
            this.lblNumTitle.Size = new System.Drawing.Size(207, 16);
            this.lblNumTitle.TabIndex = 3;
            this.lblNumTitle.Tag = "12052";
            this.lblNumTitle.Text = "#入力枚数：";
            this.lblNumTitle.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblMaxNum
            // 
            this.lblMaxNum.AutoSize = true;
            this.lblMaxNum.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMaxNum.Location = new System.Drawing.Point(275, 256);
            this.lblMaxNum.Name = "lblMaxNum";
            this.lblMaxNum.Size = new System.Drawing.Size(112, 16);
            this.lblMaxNum.TabIndex = 4;
            this.lblMaxNum.Text = "枚（最大100枚）";
            // 
            // lblCount
            // 
            this.lblCount.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCount.Location = new System.Drawing.Point(240, 256);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(34, 16);
            this.lblCount.TabIndex = 5;
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // AddGo
            // 
            this.AddGo.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.AddGo.Location = new System.Drawing.Point(208, 288);
            this.AddGo.Name = "AddGo";
            this.AddGo.Size = new System.Drawing.Size(121, 33);
            this.AddGo.TabIndex = 6;
            this.AddGo.Tag = "10005";
            this.AddGo.Text = "#実　行";
            this.AddGo.UseVisualStyleBackColor = true;
            this.AddGo.Click += new System.EventHandler(this.AddGo_Click);
            // 
            // AddEnd
            // 
            this.AddEnd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.AddEnd.Location = new System.Drawing.Point(352, 288);
            this.AddEnd.Name = "AddEnd";
            this.AddEnd.Size = new System.Drawing.Size(121, 33);
            this.AddEnd.TabIndex = 7;
            this.AddEnd.Tag = "10006";
            this.AddEnd.Text = "#終　了";
            this.AddEnd.UseVisualStyleBackColor = true;
            this.AddEnd.Click += new System.EventHandler(this.addEnd_Click);
            // 
            // frmAddition
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(491, 341);
            this.ControlBox = false;
            this.Controls.Add(this.AddEnd);
            this.Controls.Add(this.AddGo);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lblMaxNum);
            this.Controls.Add(this.lblNumTitle);
            this.Controls.Add(this.cmdImgDelete);
            this.Controls.Add(this.cmdRef);
            this.Controls.Add(this.lblSliceName);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmAddition";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Tag = "12458";
            this.Text = "#和画像";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAddition_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAddition_FormClosed);
            this.Load += new System.EventHandler(this.frmAddition_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSliceName;
        private System.Windows.Forms.Button cmdRef;
        private System.Windows.Forms.Button cmdImgDelete;
        private System.Windows.Forms.Label lblNumTitle;
        private System.Windows.Forms.Label lblMaxNum;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Button AddGo;
        private System.Windows.Forms.Button AddEnd;
    }
}