namespace TXSMechaControl.MechaIntegrate
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
            this.UC_Conf = new TXSMechaControl.MechaIntegrate.UC_Confirm();
            this.SuspendLayout();
            // 
            // UC_DoStop
            // 
            this.UC_DoStop.Location = new System.Drawing.Point(90, 148);
            this.UC_DoStop.Name = "UC_DoStop";
            this.UC_DoStop.Size = new System.Drawing.Size(262, 64);
            this.UC_DoStop.TabIndex = 0;
            // 
            // UC_Conf
            // 
            this.UC_Conf.Location = new System.Drawing.Point(13, 14);
            this.UC_Conf.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.UC_Conf.Name = "UC_Conf";
            this.UC_Conf.Size = new System.Drawing.Size(413, 116);
            this.UC_Conf.TabIndex = 1;
            // 
            // FrmConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 224);
            this.ControlBox = false;
            this.Controls.Add(this.UC_Conf);
            this.Controls.Add(this.UC_DoStop);
            this.Font = new System.Drawing.Font("Meiryo UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
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
        private UC_Confirm UC_Conf;
    }
}