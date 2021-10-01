namespace CT30K
{
    partial class frmSlicePlanCheck
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
            this.lstMessage = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lstMessage
            // 
            this.lstMessage.FormattingEnabled = true;
            this.lstMessage.ItemHeight = 12;
            this.lstMessage.Location = new System.Drawing.Point(28, 42);
            this.lstMessage.Name = "lstMessage";
            this.lstMessage.Size = new System.Drawing.Size(395, 112);
            this.lstMessage.TabIndex = 0;
            // 
            // frmSlicePlanCheck
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(477, 211);
            this.Controls.Add(this.lstMessage);
            this.Name = "frmSlicePlanCheck";
            this.ShowInTaskbar = false;
            this.Text = "frmSlicePlanCheck";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.ListBox lstMessage;

    }
}