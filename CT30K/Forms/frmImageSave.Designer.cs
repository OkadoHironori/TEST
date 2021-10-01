namespace CT30K
{
    partial class frmImageSave
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
            this.lblPrompt = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.cmdRef = new System.Windows.Forms.Button();
            this.cmdYes = new System.Windows.Forms.Button();
            this.cmdNo = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblPrompt
            // 
            this.lblPrompt.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11F);
            this.lblPrompt.Location = new System.Drawing.Point(12, 16);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(500, 20);
            this.lblPrompt.TabIndex = 0;
            this.lblPrompt.Tag = "12466";
            this.lblPrompt.Text = "#結果画像を，以下のパスファイル名に保存しますか？";
            // 
            // txtFileName
            // 
            this.txtFileName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtFileName.Location = new System.Drawing.Point(12, 40);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(469, 22);
            this.txtFileName.TabIndex = 1;
            // 
            // cmdRef
            // 
            this.cmdRef.Location = new System.Drawing.Point(488, 37);
            this.cmdRef.Name = "cmdRef";
            this.cmdRef.Size = new System.Drawing.Size(33, 25);
            this.cmdRef.TabIndex = 2;
            this.cmdRef.Text = ">>";
            this.cmdRef.UseVisualStyleBackColor = true;
            this.cmdRef.Click += new System.EventHandler(this.cmdRef_Click);
            // 
            // cmdYes
            // 
            this.cmdYes.Location = new System.Drawing.Point(256, 80);
            this.cmdYes.Name = "cmdYes";
            this.cmdYes.Size = new System.Drawing.Size(81, 25);
            this.cmdYes.TabIndex = 3;
            this.cmdYes.Tag = "10003";
            this.cmdYes.Text = "#はい";
            this.cmdYes.UseVisualStyleBackColor = true;
            this.cmdYes.Click += new System.EventHandler(this.cmdYes_Click);
            // 
            // cmdNo
            // 
            this.cmdNo.Location = new System.Drawing.Point(348, 80);
            this.cmdNo.Name = "cmdNo";
            this.cmdNo.Size = new System.Drawing.Size(81, 25);
            this.cmdNo.TabIndex = 4;
            this.cmdNo.Tag = "10004";
            this.cmdNo.Text = "#いいえ";
            this.cmdNo.UseVisualStyleBackColor = true;
            this.cmdNo.Click += new System.EventHandler(this.cmdNo_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(440, 80);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(81, 25);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Tag = "10002";
            this.cmdCancel.Text = "#キャンセル";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // frmImageSave
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(532, 119);
            this.ControlBox = false;
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdNo);
            this.Controls.Add(this.cmdYes);
            this.Controls.Add(this.cmdRef);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.lblPrompt);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmImageSave";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "12465";
            this.Text = "#結果画像の保存";
            this.Load += new System.EventHandler(this.Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Button cmdRef;
        private System.Windows.Forms.Button cmdYes;
        private System.Windows.Forms.Button cmdNo;
        private System.Windows.Forms.Button cmdCancel;
    }
}