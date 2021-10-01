using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.ComponentModel;

namespace Xs.DioMonitor
{
    public class CheckButton : CheckBox, ICloneable
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        public DioModel DioModel
        {
            get { return dioModelBindingSource.DataSource as DioModel; }
            set
            {
                if (value != dioModelBindingSource.DataSource)
                {
                    if (null == value)
                        dioModelBindingSource.DataSource = typeof(DioModel);
                    else
                        dioModelBindingSource.DataSource = value;
                }
            }
        }

        /// <summary>
        /// Checked=true時の背景色
        /// </summary>
        [EditorBrowsable]
        public System.Drawing.Color OnColor { get; set; }

        /// <summary>
        /// Checked=false時の背景色
        /// </summary>
        [EditorBrowsable]
        public System.Drawing.Color OffColor { get; set; }

        //
        public CheckButton()
            : base()
        {
            InitializeComponent();

            this.Appearance = System.Windows.Forms.Appearance.Button;
            this.AutoSize = false;
            this.Size = new System.Drawing.Size(50, 50);
            this.Font = new System.Drawing.Font("Meiryo UI", 10);
            this.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.CheckedChanged += CheckButton_CheckedChanged_ChangeColor;
            this.CheckedChanged += CheckButton_CheckedChanged_Execute;

            OnColor = System.Drawing.Color.Lime;
            OffColor = System.Drawing.Color.Green;
            
            this.BackColor = OffColor;
            this.UseVisualStyleBackColor = false;
        }

        /// <summary>
        /// 背景色変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CheckButton_CheckedChanged_ChangeColor(object sender, EventArgs e)
        {
            if(InvokeRequired)
            {
                Invoke(new EventHandler(CheckButton_CheckedChanged_ChangeColor), sender, e);
                return;
            }

            this.BackColor = Checked ? OnColor : OffColor;
        }

        /// <summary>
        /// 処理実行イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CheckButton_CheckedChanged_Execute(object sender, EventArgs e)
        {
            if (null != DioModel)
            {
                DioModel.Execute(this.Checked);
            }
        }

        
        public object Clone()
        {
            return new CheckButton()
            {
                Size = this.Size,
                OnColor = this.OnColor,
                OffColor = this.OffColor,
            };
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dioModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dioModelBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dioModelBindingSource
            // 
            this.dioModelBindingSource.DataSource = typeof(Xs.DioMonitor.DioModel);
            // 
            // CheckButton
            // 
            this.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.dioModelBindingSource, "Checked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dioModelBindingSource, "Text", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.dioModelBindingSource, "Controlable", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            ((System.ComponentModel.ISupportInitialize)(this.dioModelBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        private BindingSource dioModelBindingSource;
    }
}
