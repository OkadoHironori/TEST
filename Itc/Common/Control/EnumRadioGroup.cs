using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

using Itc.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Itc.Common.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EnumRadioGroup : FlowLayoutPanel
    {
        [Browsable(true)]
        public List<RadioButton> Items { get; private set; }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        public EnumRadioGroup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 新しいインスタンスを生成します。
        /// </summary>
        /// <param name="container"></param>
        public EnumRadioGroup(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dateMember"></param>
        /// <param name="datasource"></param>
        public void SetDataSource<T>(string dateMember, object datasource)
            where T : Enum
        {
            Items = new List<RadioButton>();

            foreach (T v in Enum.GetValues(typeof(T)))
            {
                string text = v.GetAttribute<DisplayAttribute>()?.GetName() ?? v.ToString();

                var radio = new RadioButton()
                {
                    Text = text,
                    Tag = v
                };

                Binding binding = radio.DataBindings.Add(nameof(radio.Checked), datasource, dateMember, true, DataSourceUpdateMode.OnPropertyChanged);

                binding.Format += (s, e) => e.Value = ((Binding)s).Control.Tag.Equals(e.Value);

                binding.Parse += (s, e) => 
                {
                    if ((bool)e.Value)
                    {
                        e.Value = ((Binding)s).Control.Tag;
                    }
                };

                this.Controls.Add(radio);

                Items.Add(radio);
            }
        }
    }
}
