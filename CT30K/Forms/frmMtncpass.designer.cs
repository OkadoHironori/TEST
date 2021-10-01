namespace CT30K
{
    partial class frmMtncpass
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMtncpass));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtPasswd = new System.Windows.Forms.TextBox();
            this.lblMainte = new System.Windows.Forms.Label();
            this.lblPasswd = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnOK.Location = new System.Drawing.Point(392, 16);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(97, 33);
            this.btnOK.TabIndex = 0;
            this.btnOK.Tag = "10001";
            this.btnOK.Text = "#OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnCancel.Location = new System.Drawing.Point(392, 63);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(97, 33);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Tag = "10002";
            this.btnCancel.Text = "#キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtPasswd
            // 
            this.txtPasswd.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtPasswd.Location = new System.Drawing.Point(104, 73);
            this.txtPasswd.Name = "txtPasswd";
            this.txtPasswd.PasswordChar = '*';
            this.txtPasswd.Size = new System.Drawing.Size(257, 23);
            this.txtPasswd.TabIndex = 0;
            // 
            // lblMainte
            // 
            this.lblMainte.AutoSize = true;
            this.lblMainte.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMainte.Location = new System.Drawing.Point(72, 16);
            this.lblMainte.Name = "lblMainte";
            this.lblMainte.Size = new System.Drawing.Size(308, 16);
            this.lblMainte.TabIndex = 3;
            this.lblMainte.Tag = "12567";
            this.lblMainte.Text = "#ﾒﾝﾃﾅﾝｽﾂｰﾙへのﾊﾟｽﾜｰﾄﾞを入力してください。";
            // 
            // lblPasswd
            // 
            this.lblPasswd.AutoSize = true;
            this.lblPasswd.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblPasswd.Location = new System.Drawing.Point(32, 77);
            this.lblPasswd.Name = "lblPasswd";
            this.lblPasswd.Size = new System.Drawing.Size(78, 16);
            this.lblPasswd.TabIndex = 4;
            this.lblPasswd.Tag = "12565";
            this.lblPasswd.Text = "#パスワード";
            // 
            // label1
            // 
            this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 32);
            this.label1.TabIndex = 5;
            // 
            // frmMtncpass
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(505, 112);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblPasswd);
            this.Controls.Add(this.lblMainte);
            this.Controls.Add(this.txtPasswd);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmMtncpass";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "12566";
            this.Text = "#ﾒﾝﾃﾅﾝｽ ﾊﾟｽﾜｰﾄﾞ入力";
            this.Load += new System.EventHandler(this.frmMtncpass_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtPasswd;
        private System.Windows.Forms.Label lblMainte;
        private System.Windows.Forms.Label lblPasswd;
        private System.Windows.Forms.Label label1;
    }
}