namespace CT30K
{
    partial class frmTitanXrayInfomation
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
            this.lblErrNo = new System.Windows.Forms.Label();
            this.lblErrContents = new System.Windows.Forms.Label();
            this.lblErrNoTxt = new System.Windows.Forms.Label();
            this.lblErrContentsTxt = new System.Windows.Forms.Label();
            this.cmdUpdate = new System.Windows.Forms.Button();
            this.cmdErrReset = new System.Windows.Forms.Button();
            this.cmdClose = new System.Windows.Forms.Button();
            this.lblCommErrNo = new System.Windows.Forms.Label();
            this.lblCommErrNoTxt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblErrNo
            // 
            this.lblErrNo.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblErrNo.Location = new System.Drawing.Point(25, 56);
            this.lblErrNo.Name = "lblErrNo";
            this.lblErrNo.Size = new System.Drawing.Size(65, 17);
            this.lblErrNo.TabIndex = 0;
            this.lblErrNo.Tag = "20013";
            this.lblErrNo.Text = "#エラ－番号";
            // 
            // lblErrContents
            // 
            this.lblErrContents.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblErrContents.Location = new System.Drawing.Point(46, 93);
            this.lblErrContents.Name = "lblErrContents";
            this.lblErrContents.Size = new System.Drawing.Size(35, 12);
            this.lblErrContents.TabIndex = 1;
            this.lblErrContents.Tag = "12511";
            this.lblErrContents.Text = "#原因";
            // 
            // lblErrNoTxt
            // 
            this.lblErrNoTxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblErrNoTxt.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblErrNoTxt.Location = new System.Drawing.Point(96, 50);
            this.lblErrNoTxt.Name = "lblErrNoTxt";
            this.lblErrNoTxt.Size = new System.Drawing.Size(115, 25);
            this.lblErrNoTxt.TabIndex = 2;
            this.lblErrNoTxt.Text = "label3";
            this.lblErrNoTxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblErrContentsTxt
            // 
            this.lblErrContentsTxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblErrContentsTxt.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblErrContentsTxt.Location = new System.Drawing.Point(96, 88);
            this.lblErrContentsTxt.Name = "lblErrContentsTxt";
            this.lblErrContentsTxt.Size = new System.Drawing.Size(209, 113);
            this.lblErrContentsTxt.TabIndex = 3;
            this.lblErrContentsTxt.Text = "label4";
            // 
            // cmdUpdate
            // 
            this.cmdUpdate.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdUpdate.Location = new System.Drawing.Point(56, 224);
            this.cmdUpdate.Name = "cmdUpdate";
            this.cmdUpdate.Size = new System.Drawing.Size(81, 25);
            this.cmdUpdate.TabIndex = 4;
            this.cmdUpdate.Tag = "20120";
            this.cmdUpdate.Text = "#更新";
            this.cmdUpdate.UseVisualStyleBackColor = true;
            this.cmdUpdate.Click += new System.EventHandler(this.cmdUpdate_Click);
            // 
            // cmdErrReset
            // 
            this.cmdErrReset.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdErrReset.Location = new System.Drawing.Point(144, 224);
            this.cmdErrReset.Name = "cmdErrReset";
            this.cmdErrReset.Size = new System.Drawing.Size(81, 25);
            this.cmdErrReset.TabIndex = 5;
            this.cmdErrReset.Tag = "10022";
            this.cmdErrReset.Text = "#リセット";
            this.cmdErrReset.UseVisualStyleBackColor = true;
            this.cmdErrReset.Click += new System.EventHandler(this.cmdErrReset_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdClose.Location = new System.Drawing.Point(232, 224);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(81, 25);
            this.cmdClose.TabIndex = 6;
            this.cmdClose.Tag = "10008";
            this.cmdClose.Text = "#閉じる";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // lblCommErrNo
            // 
            this.lblCommErrNo.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCommErrNo.Location = new System.Drawing.Point(2, 22);
            this.lblCommErrNo.Name = "lblCommErrNo";
            this.lblCommErrNo.Size = new System.Drawing.Size(88, 20);
            this.lblCommErrNo.TabIndex = 7;
            this.lblCommErrNo.Tag = "24006";
            this.lblCommErrNo.Text = "#通信エラ－番号";
            // 
            // lblCommErrNoTxt
            // 
            this.lblCommErrNoTxt.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCommErrNoTxt.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCommErrNoTxt.Location = new System.Drawing.Point(96, 13);
            this.lblCommErrNoTxt.Name = "lblCommErrNoTxt";
            this.lblCommErrNoTxt.Size = new System.Drawing.Size(115, 25);
            this.lblCommErrNoTxt.TabIndex = 8;
            this.lblCommErrNoTxt.Text = "label3";
            this.lblCommErrNoTxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // frmTitanXrayInfomation
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(322, 262);
            this.Controls.Add(this.lblCommErrNoTxt);
            this.Controls.Add(this.lblCommErrNo);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.cmdErrReset);
            this.Controls.Add(this.cmdUpdate);
            this.Controls.Add(this.lblErrContentsTxt);
            this.Controls.Add(this.lblErrNoTxt);
            this.Controls.Add(this.lblErrContents);
            this.Controls.Add(this.lblErrNo);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmTitanXrayInfomation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "14100";
            this.Text = "#X線情報";
            this.Load += new System.EventHandler(this.frmTitanXrayInfomation_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblErrNo;
        private System.Windows.Forms.Label lblErrContents;
        private System.Windows.Forms.Label lblErrNoTxt;
        private System.Windows.Forms.Label lblErrContentsTxt;
        private System.Windows.Forms.Button cmdUpdate;
        private System.Windows.Forms.Button cmdErrReset;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Label lblCommErrNo;
        private System.Windows.Forms.Label lblCommErrNoTxt;
    }
}