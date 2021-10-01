namespace Itc.Common.Controls
{
    partial class UserControl_EnumCombobox
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.enumCombobox1 = new Itc.Common.Controls.EnumComboBox(this.components);
            this.sampleBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.checkButton1 = new Itc.Common.Controls.CheckButton();
            this.enumRadioGroup1 = new Itc.Common.Controls.EnumRadioGroup(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.dropdownButton1 = new Itc.Common.Controls.DropdownButton(this.components);
            this.dropdownButton2 = new Itc.Common.Controls.DropdownButton(this.components);
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.sampleBindingSource)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // enumCombobox1
            // 
            this.enumCombobox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.sampleBindingSource, "Value", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.enumCombobox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.enumCombobox1.FormattingEnabled = true;
            this.enumCombobox1.Location = new System.Drawing.Point(14, 19);
            this.enumCombobox1.Name = "enumCombobox1";
            this.enumCombobox1.Size = new System.Drawing.Size(121, 20);
            this.enumCombobox1.TabIndex = 0;
            this.enumCombobox1.ValueMember = "Key";
            // 
            // sampleBindingSource
            // 
            this.sampleBindingSource.DataSource = typeof(Itc.Common.Controls.Sample);
            // 
            // checkButton1
            // 
            this.checkButton1.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkButton1.CheckedColor = System.Drawing.Color.Lime;
            this.checkButton1.IndeterminateColor = System.Drawing.Color.Empty;
            this.checkButton1.Location = new System.Drawing.Point(14, 58);
            this.checkButton1.Name = "checkButton1";
            this.checkButton1.Size = new System.Drawing.Size(121, 24);
            this.checkButton1.TabIndex = 1;
            this.checkButton1.Text = "checkButton1";
            this.checkButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkButton1.UncheckedColor = System.Drawing.Color.Green;
            this.checkButton1.UseVisualStyleBackColor = false;
            // 
            // enumRadioGroup1
            // 
            this.enumRadioGroup1.Location = new System.Drawing.Point(14, 104);
            this.enumRadioGroup1.Name = "enumRadioGroup1";
            this.enumRadioGroup1.Size = new System.Drawing.Size(121, 100);
            this.enumRadioGroup1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(146, 107);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(146, 136);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(146, 165);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // dropdownButton1
            // 
            this.dropdownButton1.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dropdownButton1.Location = new System.Drawing.Point(146, 19);
            this.dropdownButton1.Name = "dropdownButton1";
            this.dropdownButton1.Size = new System.Drawing.Size(99, 29);
            this.dropdownButton1.TabIndex = 6;
            this.dropdownButton1.Text = "Basic";
            this.dropdownButton1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dropdownButton1.UseVisualStyleBackColor = true;
            // 
            // dropdownButton2
            // 
            this.dropdownButton2.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dropdownButton2.Location = new System.Drawing.Point(146, 54);
            this.dropdownButton2.Name = "dropdownButton2";
            this.dropdownButton2.Size = new System.Drawing.Size(99, 29);
            this.dropdownButton2.TabIndex = 8;
            this.dropdownButton2.Text = "IIR";
            this.dropdownButton2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dropdownButton2.UseVisualStyleBackColor = true;
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem3.Text = "Shen";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(101, 26);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(179, 22);
            this.toolStripMenuItem4.Text = "toolStripMenuItem4";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(32, 19);
            this.toolStripMenuItem5.Text = "toolStripMenuItem5";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuItem1.Text = "TEST";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(104, 22);
            this.toolStripMenuItem2.Text = "TEST2";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem6,
            this.toolStripMenuItem7});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(116, 48);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(115, 22);
            this.toolStripMenuItem6.Text = "Smooth";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(115, 22);
            this.toolStripMenuItem7.Text = "Median";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
            // 
            // UserControl_EnumCombobox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dropdownButton2);
            this.Controls.Add(this.dropdownButton1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.enumRadioGroup1);
            this.Controls.Add(this.checkButton1);
            this.Controls.Add(this.enumCombobox1);
            this.Name = "UserControl_EnumCombobox";
            this.Size = new System.Drawing.Size(271, 223);
            ((System.ComponentModel.ISupportInitialize)(this.sampleBindingSource)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private EnumComboBox enumCombobox1;
        private CheckButton checkButton1;
        private EnumRadioGroup enumRadioGroup1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private DropdownButton dropdownButton1;
        private DropdownButton dropdownButton2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.BindingSource sampleBindingSource;
    }
}
