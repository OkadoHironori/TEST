namespace Itc.Common.Controls.TXUCCtrl
{
    partial class FrmConfirm
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
            this.UC_DoStop = new Itc.Common.Controls.TXUCCtrl.UC_DoStop();
            this.SuspendLayout();
            // 
            // UC_DoStop
            // 
            this.UC_DoStop.Location = new System.Drawing.Point(319, 338);
            this.UC_DoStop.Name = "UC_DoStop";
            this.UC_DoStop.Size = new System.Drawing.Size(256, 64);
            this.UC_DoStop.TabIndex = 0;
            // 
            // FrmConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 424);
            this.ControlBox = false;
            this.Controls.Add(this.UC_DoStop);
            this.Font = new System.Drawing.Font("Meiryo UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmConfirm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "確認";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private Itc.Common.Controls.TXUCCtrl.UC_DoStop UC_DoStop;
    }
}