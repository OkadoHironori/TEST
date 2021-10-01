using Itc.Common.Controls.MoveBtn;

namespace TXSMechaControl.CommonComp
{
    partial class UC_H_BindSlider
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UC_H_BindSlider));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.NumUDDecimalSlider = new TXSMechaControl.CommonComp.BindNUD();
            this.SliderBind = new TXSMechaControl.CommonComp.BindSlider();
            this.MoveBackword = new Itc.Common.Controls.MoveBtn.MoveBtn();
            this.MoveForword = new Itc.Common.Controls.MoveBtn.MoveBtn();
            this.Unit = new Itc.Common.Controls.LabelEx.LabelEx();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.Minlbl = new System.Windows.Forms.Label();
            this.Maxlbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUDDecimalSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SliderBind)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.Controls.Add(this.NumUDDecimalSlider, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.MoveBackword, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.SliderBind, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.MoveForword, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.Unit, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(877, 69);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // NumUDDecimalSlider
            // 
            this.NumUDDecimalSlider.BindSlider = this.SliderBind;
            this.NumUDDecimalSlider.DirectChanged = false;
            this.NumUDDecimalSlider.Font = new System.Drawing.Font("Meiryo UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.NumUDDecimalSlider.IsDirectDecition = false;
            this.NumUDDecimalSlider.IsSliderDecition = false;
            this.NumUDDecimalSlider.Location = new System.Drawing.Point(3, 9);
            this.NumUDDecimalSlider.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.NumUDDecimalSlider.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.NumUDDecimalSlider.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.NumUDDecimalSlider.Name = "NumUDDecimalSlider";
            this.NumUDDecimalSlider.Size = new System.Drawing.Size(194, 48);
            this.NumUDDecimalSlider.TabIndex = 1;
            this.NumUDDecimalSlider.Value = 0F;
            this.NumUDDecimalSlider.ValueChanged += new System.EventHandler(this.NumUDDecimalSlider_ValueChanged);
            // 
            // SliderBind
            // 
            this.SliderBind.Data = ((System.Collections.Generic.List<float>)(resources.GetObject("SliderBind.Data")));
            this.SliderBind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SliderBind.FCD_BindNUD = this.NumUDDecimalSlider;
            this.SliderBind.IsSliderChanged = false;
            this.SliderBind.Location = new System.Drawing.Point(360, 3);
            this.SliderBind.Maximum = 5000F;
            this.SliderBind.Minimum = -1000F;
            this.SliderBind.Name = "SliderBind";
            this.SliderBind.Size = new System.Drawing.Size(444, 63);
            this.SliderBind.TabIndex = 0;
            this.SliderBind.TickFrequency = 1000;
            this.SliderBind.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.SliderBind.Value = 1F;
            this.SliderBind.ValueScale = 1F;
            // 
            // MoveBackword
            // 
            this.MoveBackword.BackColor = System.Drawing.SystemColors.Control;
            this.MoveBackword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MoveBackword.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.MoveBackword.Limit = Itc.Common.TXEnum.LimitMode.OnLimit;
            this.MoveBackword.Location = new System.Drawing.Point(810, 3);
            this.MoveBackword.Name = "MoveBackword";
            this.MoveBackword.Size = new System.Drawing.Size(64, 63);
            this.MoveBackword.TabIndex = 3;
            this.MoveBackword.TabStop = false;
            this.MoveBackword.Text = "B";
            this.MoveBackword.UseVisualStyleBackColor = false;
            this.MoveBackword.MoveStateCtrl += new System.Action<object, Itc.Common.Event.ChkChangeEventArgs>(this.MoveBackword_MoveStateCtrl);
            // 
            // MoveForword
            // 
            this.MoveForword.BackColor = System.Drawing.SystemColors.Control;
            this.MoveForword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MoveForword.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.MoveForword.Limit = Itc.Common.TXEnum.LimitMode.OnLimit;
            this.MoveForword.Location = new System.Drawing.Point(289, 3);
            this.MoveForword.Name = "MoveForword";
            this.MoveForword.Size = new System.Drawing.Size(65, 63);
            this.MoveForword.TabIndex = 2;
            this.MoveForword.TabStop = false;
            this.MoveForword.Text = "F";
            this.MoveForword.UseVisualStyleBackColor = false;
            this.MoveForword.MoveStateCtrl += new System.Action<object, Itc.Common.Event.ChkChangeEventArgs>(this.MoveForword_MoveStateCtrl);
            // 
            // Unit
            // 
            this.Unit.AutoSize = true;
            this.Unit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Unit.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Unit.Location = new System.Drawing.Point(203, 0);
            this.Unit.Name = "Unit";
            this.Unit.Size = new System.Drawing.Size(80, 69);
            this.Unit.TabIndex = 4;
            this.Unit.Text = "Unit";
            this.Unit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(883, 134);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 286F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 136F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 139F));
            this.tableLayoutPanel3.Controls.Add(this.Minlbl, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.Maxlbl, 3, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 78);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(877, 60);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // Minlbl
            // 
            this.Minlbl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Minlbl.AutoSize = true;
            this.Minlbl.Font = new System.Drawing.Font("Meiryo UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Minlbl.Location = new System.Drawing.Point(289, 0);
            this.Minlbl.Name = "Minlbl";
            this.Minlbl.Size = new System.Drawing.Size(130, 60);
            this.Minlbl.TabIndex = 0;
            this.Minlbl.Text = "label1";
            this.Minlbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Maxlbl
            // 
            this.Maxlbl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Maxlbl.AutoSize = true;
            this.Maxlbl.Font = new System.Drawing.Font("Meiryo UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Maxlbl.Location = new System.Drawing.Point(741, 0);
            this.Maxlbl.Name = "Maxlbl";
            this.Maxlbl.Size = new System.Drawing.Size(133, 60);
            this.Maxlbl.TabIndex = 1;
            this.Maxlbl.Text = "label2";
            this.Maxlbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // UC_H_BindSlider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "UC_H_BindSlider";
            this.Size = new System.Drawing.Size(886, 134);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumUDDecimalSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SliderBind)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BindSlider SliderBind;
        private BindNUD NumUDDecimalSlider;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label Minlbl;
        private System.Windows.Forms.Label Maxlbl;
        private MoveBtn MoveForword;
        private MoveBtn MoveBackword;
        private Itc.Common.Controls.LabelEx.LabelEx Unit;
    }
}
