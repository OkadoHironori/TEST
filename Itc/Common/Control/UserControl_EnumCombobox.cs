using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations;

namespace Itc.Common.Controls
{
    //デバッグ用
    internal partial class UserControl_EnumCombobox : UserControl
    {
        Sample Sample;

        public UserControl_EnumCombobox()
        {
            InitializeComponent();

            enumCombobox1.SetDataSource<TestEnum>();

            //var bs = new BindingSource();

            Sample = new Sample() { Value = TestEnum.Beta };

            //bs.DataSource = Sample;

            enumRadioGroup1.SetDataSource<TestEnum>(nameof(Sample.Value), sampleBindingSource);

            dropdownButton1.ContextMenuStrip = contextMenuStrip1;
            dropdownButton2.ContextMenuStrip = contextMenuStrip2;

            sampleBindingSource.DataSource = Sample;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Sample.Value = TestEnum.Alpha;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Sample.Value = TestEnum.Beta;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Sample.Value = TestEnum.Gamma;
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (sender is Control c)
            {
                Console.WriteLine(c.Text);
            }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (sender is Control c)
            {
                Console.WriteLine(c.Text);
            }
        }
    }

    internal enum TestEnum
    {
        [Display(Name ="α")]
        Alpha,

        [Display(Name = nameof(Itc.Common.Controls.Properties.Resources.Beta), ResourceType = typeof(Itc.Common.Controls.Properties.Resources))]
        Beta,

        Gamma, 
    }

    internal class Sample : INotifyPropertyChanged
    {
        TestEnum _Value;
        public TestEnum Value
        {
            get => _Value;
            set 
            {
                if (_Value != value)
                {
                    _Value = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
