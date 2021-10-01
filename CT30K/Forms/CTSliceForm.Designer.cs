namespace CT30K
{
    partial class CTSliceForm
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
            this.cmdDisp = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdDisp
            // 
            this.cmdDisp.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdDisp.Location = new System.Drawing.Point(234, 136);
            this.cmdDisp.Name = "cmdDisp";
            this.cmdDisp.Size = new System.Drawing.Size(97, 33);
            this.cmdDisp.TabIndex = 7;
            this.cmdDisp.Tag = "10011";
            this.cmdDisp.Text = "#表　示";
            this.cmdDisp.UseVisualStyleBackColor = true;
            this.cmdDisp.Click += new System.EventHandler(this.cmdDisp_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdCancel.Location = new System.Drawing.Point(128, 136);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(97, 33);
            this.cmdCancel.TabIndex = 6;
            this.cmdCancel.Tag = "10002";
            this.cmdCancel.Text = "#キャンセル";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOk
            // 
            this.cmdOk.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cmdOk.Location = new System.Drawing.Point(22, 136);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(97, 33);
            this.cmdOk.TabIndex = 5;
            this.cmdOk.Tag = "10001";
            this.cmdOk.Text = "#Ｏ　Ｋ";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // CTSliceForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(353, 186);
            this.ControlBox = false;
            this.Controls.Add(this.cmdDisp);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOk);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CTSliceForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Tag = "12509";
            this.Text = "#閾値入力";
            this.Load += new System.EventHandler(this.CTSliceForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdDisp;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOk;
    }
}