using Itc.Common.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Itc.Common.Controls
{
    /// <summary>
    /// Enum項目対象とするコンボボックス
    /// </summary>
    /// <remarks>Display属性が付与されている場合、表示名を取得する</remarks>
    [ToolboxItem(typeof(System.Drawing.Design.ToolboxItem))] //初期値が強制的に変更されるのを防ぐために付与。（例：AutoSize）
    public partial class EnumComboBox : ComboBox
    {
        const string valueMember = "Key";

        const string displayMember = "Value";

        /// <summary>
        /// 新しいインスタンスを生成します
        /// </summary>
        public EnumComboBox()
        {
            InitializeComponent();

            DropDownStyle = ComboBoxStyle.DropDownList;

            ValueMember = valueMember;
            DisplayMember = displayMember;
        }

        /// <summary>
        /// 新しいインスタンスを生成します
        /// </summary>
        /// <param name="container"></param>
        public EnumComboBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            DropDownStyle = ComboBoxStyle.DropDownList;

            ValueMember = valueMember;
            DisplayMember = displayMember;
        }

        /// <summary>
        /// Tの項目を全てデータソース化する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void SetDataSource<T>() 
            where T : Enum 
            => DataSource = new BindingSource(CreateDictionary<T>(), null);

        /// <summary>
        /// Tの指定した項目をデータソース化する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        public void SetDataSource<T>(IEnumerable<T> values)
            where T : Enum
            => DataSource = new BindingSource(CreateDictionary<T>(values), null);

        /// <summary>
        /// Dictionary作成
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="availables"></param>
        /// <returns></returns>
        private Dictionary<T, string> CreateDictionary<T>(IEnumerable<T> availables = null)
            where T : Enum => typeof(T).ToEnumerable<T>()
                .Where(t => availables?.Contains(t) ?? true)
                .ToDictionary(t => t, t => t?.GetAttribute<DisplayAttribute>()?.GetName() ?? t.ToString());
    }
}
