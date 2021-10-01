namespace CT30K
{
    partial class frmLineInput
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
            this.fraCoordinate = new System.Windows.Forms.GroupBox();
            this.lblPoint2 = new System.Windows.Forms.Label();
            this.lblPoint1 = new System.Windows.Forms.Label();
            this.lblY1 = new System.Windows.Forms.Label();
            this.lblX1 = new System.Windows.Forms.Label();
            this.fraLineNo = new System.Windows.Forms.GroupBox();
            this.lblNo = new System.Windows.Forms.Label();
            this.cboDistLNo = new System.Windows.Forms.ComboBox();
            this.lblDistLNo = new System.Windows.Forms.Label();
            this.fraCoordinate.SuspendLayout();
            this.fraLineNo.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdDisp
            // 
            this.cmdDisp.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdDisp.Location = new System.Drawing.Point(210, 156);
            this.cmdDisp.Name = "cmdDisp";
            this.cmdDisp.Size = new System.Drawing.Size(89, 33);
            this.cmdDisp.TabIndex = 10;
            this.cmdDisp.Tag = "10011";
            this.cmdDisp.Text = "#表示";
            this.cmdDisp.UseVisualStyleBackColor = true;
            this.cmdDisp.Click += new System.EventHandler(this.cmdDisp_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdCancel.Location = new System.Drawing.Point(110, 156);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(89, 33);
            this.cmdCancel.TabIndex = 9;
            this.cmdCancel.Tag = "10002";
            this.cmdCancel.Text = "#キャンセル";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOk
            // 
            this.cmdOk.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdOk.Location = new System.Drawing.Point(10, 156);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(89, 33);
            this.cmdOk.TabIndex = 8;
            this.cmdOk.Tag = "10001";
            this.cmdOk.Text = "#ＯＫ";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // fraCoordinate
            // 
            this.fraCoordinate.Controls.Add(this.lblPoint2);
            this.fraCoordinate.Controls.Add(this.lblPoint1);
            this.fraCoordinate.Controls.Add(this.lblY1);
            this.fraCoordinate.Controls.Add(this.lblX1);
            this.fraCoordinate.Location = new System.Drawing.Point(10, 44);
            this.fraCoordinate.Name = "fraCoordinate";
            this.fraCoordinate.Size = new System.Drawing.Size(289, 100);
            this.fraCoordinate.TabIndex = 14;
            this.fraCoordinate.TabStop = false;
            // 
            // lblPoint2
            // 
            this.lblPoint2.AutoSize = true;
            this.lblPoint2.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblPoint2.Location = new System.Drawing.Point(27, 70);
            this.lblPoint2.Name = "lblPoint2";
            this.lblPoint2.Size = new System.Drawing.Size(83, 16);
            this.lblPoint2.TabIndex = 20;
            this.lblPoint2.Tag = "";
            this.lblPoint2.Text = "#指定点２：";
            // 
            // lblPoint1
            // 
            this.lblPoint1.AutoSize = true;
            this.lblPoint1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblPoint1.Location = new System.Drawing.Point(27, 36);
            this.lblPoint1.Name = "lblPoint1";
            this.lblPoint1.Size = new System.Drawing.Size(83, 16);
            this.lblPoint1.TabIndex = 19;
            this.lblPoint1.Tag = "";
            this.lblPoint1.Text = "#指定点１：";
            // 
            // lblY1
            // 
            this.lblY1.AutoSize = true;
            this.lblY1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblY1.Location = new System.Drawing.Point(214, 12);
            this.lblY1.Name = "lblY1";
            this.lblY1.Size = new System.Drawing.Size(17, 16);
            this.lblY1.TabIndex = 14;
            this.lblY1.Tag = "";
            this.lblY1.Text = "Y";
            // 
            // lblX1
            // 
            this.lblX1.AutoSize = true;
            this.lblX1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblX1.Location = new System.Drawing.Point(133, 12);
            this.lblX1.Name = "lblX1";
            this.lblX1.Size = new System.Drawing.Size(18, 16);
            this.lblX1.TabIndex = 13;
            this.lblX1.Tag = "";
            this.lblX1.Text = "X";
            // 
            // fraLineNo
            // 
            this.fraLineNo.Controls.Add(this.lblNo);
            this.fraLineNo.Controls.Add(this.cboDistLNo);
            this.fraLineNo.Controls.Add(this.lblDistLNo);
            this.fraLineNo.Location = new System.Drawing.Point(48, 5);
            this.fraLineNo.Name = "fraLineNo";
            this.fraLineNo.Size = new System.Drawing.Size(201, 38);
            this.fraLineNo.TabIndex = 15;
            this.fraLineNo.TabStop = false;
            // 
            // lblNo
            // 
            this.lblNo.AutoSize = true;
            this.lblNo.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblNo.Location = new System.Drawing.Point(87, 13);
            this.lblNo.Name = "lblNo";
            this.lblNo.Size = new System.Drawing.Size(29, 16);
            this.lblNo.TabIndex = 16;
            this.lblNo.Text = "No:";
            // 
            // cboDistLNo
            // 
            this.cboDistLNo.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.cboDistLNo.FormattingEnabled = true;
            this.cboDistLNo.Location = new System.Drawing.Point(116, 10);
            this.cboDistLNo.Name = "cboDistLNo";
            this.cboDistLNo.Size = new System.Drawing.Size(81, 24);
            this.cboDistLNo.TabIndex = 14;
            this.cboDistLNo.SelectedIndexChanged += new System.EventHandler(this.cboDistLNo_SelectedIndexChanged);
            this.cboDistLNo.Enter += new System.EventHandler(this.cboDistLNo_Enter);
            this.cboDistLNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboDistLNo_KeyPress);
            // 
            // lblDistLNo
            // 
            this.lblDistLNo.AutoSize = true;
            this.lblDistLNo.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 12F);
            this.lblDistLNo.Location = new System.Drawing.Point(3, 13);
            this.lblDistLNo.Name = "lblDistLNo";
            this.lblDistLNo.Size = new System.Drawing.Size(80, 16);
            this.lblDistLNo.TabIndex = 15;
            this.lblDistLNo.Tag = "12495";
            this.lblDistLNo.Text = "#線分番号";
            // 
            // frmLineInput
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(308, 203);
            this.ControlBox = false;
            this.Controls.Add(this.fraLineNo);
            this.Controls.Add(this.fraCoordinate);
            this.Controls.Add(this.cmdDisp);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOk);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmLineInput";
            this.Text = "線分座標入力";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmLineInput_FormClosed);
            this.Load += new System.EventHandler(this.frmLineInput_Load);
            this.Resize += new System.EventHandler(this.frmLineInput_Resize);
            this.fraCoordinate.ResumeLayout(false);
            this.fraCoordinate.PerformLayout();
            this.fraLineNo.ResumeLayout(false);
            this.fraLineNo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdDisp;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOk;
        private System.Windows.Forms.GroupBox fraCoordinate;
        private System.Windows.Forms.Label lblY1;
        private System.Windows.Forms.Label lblX1;
        private System.Windows.Forms.Label lblPoint2;
        private System.Windows.Forms.Label lblPoint1;
        private System.Windows.Forms.GroupBox fraLineNo;
        private System.Windows.Forms.Label lblNo;
        private System.Windows.Forms.ComboBox cboDistLNo;
        private System.Windows.Forms.Label lblDistLNo;
    }
}