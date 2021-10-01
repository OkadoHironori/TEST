using System;

using System.Windows.Forms;
using System.ComponentModel;

namespace Itc.Common.Controls
{
    public class NumericUpDownCell : DataGridViewTextBoxCell
    {
        /// <summary>
        /// 添付し、ホストされる編集コントロールを初期化します。
        /// </summary>
        /// <param name="rowIndex">編集されている行のインデックス。</param>
        /// <param name="initialFormattedValue">コントロールに表示される初期値。</param>
        /// <param name="dataGridViewCellStyle">ホストされるコントロールの外観を決定するために使用するセルのスタイル。</param>
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            NumericUpDownEditingControl ctrl = DataGridView.EditingControl as NumericUpDownEditingControl;

            if (this.Value != null)
            {
                ctrl.Value = decimal.Parse(this.Value.ToString());
            }
        }

        public override Type EditType
        {
            get
            {
                return typeof(NumericUpDownEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(decimal);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return default(decimal);
            }
        }

        /// <summary>
        /// 表示用に書式設定された値を、実際のセル値に変換します。
        /// </summary>
        /// <param name="formattedValue">セルの表示値。</param>
        /// <param name="cellStyle">System.Windows.Forms.DataGridViewCellStyle セルに対して有効です。</param>
        /// <param name="formattedValueTypeConverter">A System.ComponentModel.TypeConverter 表示値の種類または null 既定のコンバーターを使用します。</param>
        /// <param name="valueTypeConverter">A System.ComponentModel.TypeConverter セル値の種類または null 既定のコンバーターを使用します。</param>
        /// <returns>セル値。</returns>
        public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
        {
            //return base.ParseFormattedValue(formattedValue, cellStyle, formattedValueTypeConverter, valueTypeConverter);
            //例外回避策として、formattedValueをstringにしている。
            return base.ParseFormattedValue(formattedValue.ToString(), cellStyle, formattedValueTypeConverter, valueTypeConverter);
        }
    }
}
