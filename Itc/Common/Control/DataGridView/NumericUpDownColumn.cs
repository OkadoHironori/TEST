
using System.Windows.Forms;

namespace Itc.Common.Controls
{
    public class NumericUpDownColumn : DataGridViewColumn
    {
        public NumericUpDownColumn() : base(new NumericUpDownCell()) { }
    }
}
