namespace CT30K
{
    partial class frmSubtraction
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
            this.cmdEnd = new System.Windows.Forms.Button();
            this.cmdExe = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdEnd
            // 
            this.cmdEnd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdEnd.Location = new System.Drawing.Point(345, 115);
            this.cmdEnd.Name = "cmdEnd";
            this.cmdEnd.Size = new System.Drawing.Size(120, 33);
            this.cmdEnd.TabIndex = 11;
            this.cmdEnd.Tag = "10006";
            this.cmdEnd.Text = "#終　了";
            this.cmdEnd.UseVisualStyleBackColor = true;
            this.cmdEnd.Click += new System.EventHandler(this.cmdEnd_Click);
            // 
            // cmdExe
            // 
            this.cmdExe.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdExe.Location = new System.Drawing.Point(201, 115);
            this.cmdExe.Name = "cmdExe";
            this.cmdExe.Size = new System.Drawing.Size(120, 33);
            this.cmdExe.TabIndex = 10;
            this.cmdExe.Tag = "10005";
            this.cmdExe.Text = "#実　行";
            this.cmdExe.UseVisualStyleBackColor = true;
            this.cmdExe.Click += new System.EventHandler(this.cmdExe_Click);
            // 
            // frmSubtraction
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(524, 170);
            this.ControlBox = false;
            this.Controls.Add(this.cmdEnd);
            this.Controls.Add(this.cmdExe);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmSubtraction";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Tag = "12459";
            this.Text = "#差画像";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSubtraction_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSubtraction_FormClosed);
            this.Load += new System.EventHandler(this.frmSubtraction_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdEnd;
        private System.Windows.Forms.Button cmdExe;
    }
}