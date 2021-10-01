using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Itc.Common.Extensions;

namespace TXSMechaControl.MechaIntegrate
{
    public partial class UC_Confirm : UserControl
    {
        public UC_Confirm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 初期値入力
        /// </summary>
        public void SetIniValue(int decp, float max, float min, decimal incre,float prev)
        {
            NUD_EX.SetIniDeci(decp, max, min, incre, prev);
        }
        /// <summary>
        /// 切り上げ
        /// </summary>
        /// <param name="value"></param>
        /// <param name="multiple"></param>
        /// <returns></returns>
        private float MultipleCeil(float value, float multiple)
        {
            string tmp;

            switch (NUD_EX.ValueEx)
            {
                case ("0.000"):
                    tmp = ((float)(Math.Ceiling(value / multiple) * multiple)).ToString(NUD_EX.ValueEx);
                    break;
                default:

                    tmp = ((float)(Math.Round(value / multiple) * multiple)).ToString(NUD_EX.ValueEx);

                    //if (value > 0)
                    //{
                    //    tmp = ((float)(Math.Floor(value / multiple) * multiple)).ToString(NUD_EX.ValueEx);
                    //}
                    //else
                    //{
                    //    tmp = ((float)(Math.Ceiling(value / multiple) * multiple)).ToString(NUD_EX.ValueEx);
                    //}
                    break;
            }


        
            return float.Parse(tmp);
        }
        /// <summary>
        /// 値設定
        /// </summary>
        /// <param name="current"></param>
        public void SetValue(float newposi, float current)
        {
            var tmpdata = newposi.CorrectRange((float)NUD_EX.Minimum, (float)NUD_EX.Maximum);
            var data = MultipleCeil(tmpdata, float.Parse(NUD_EX.Increment.ToString()));

            NUD_EX.Value = (decimal)data;

            switch(NUD_EX.DecimalPlaces)
            {
                case (3):
                    TBReadOnly.Text = current.ToString("0.000");
                    break;
                case (2):
                    TBReadOnly.Text = current.ToString("0.00");
                    break;
                case (1):
                    TBReadOnly.Text = current.ToString("0.0");
                    break;
                case (0):
                    TBReadOnly.Text = current.ToString("0");
                    break;

                default:
                    throw new Exception("Undeveloped DecimalPlaces");
            }
        }
        /// <summary>
        /// 更新後の数値の取得
        /// </summary>
        /// <returns></returns>
        public float GetRes()
        {
            return (float)NUD_EX.Value;
        }
        /// <summary>
        /// 更新前の数値の取得
        /// </summary>
        /// <returns></returns>
        public float GetBeforeNum()
        {
            return float.Parse(TBReadOnly.Text);
        }
    }
}
