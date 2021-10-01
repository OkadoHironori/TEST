using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MechaMaintCnt.AuxSel;
using Itc.Common.Basic;
using Itc.Common.Extensions;
using Itc.Common.TXEnum;

namespace MechaMaintCnt.FDD
{
    public partial class UC_FDD : UserControl
    {
        /// <summary>
        /// メッセージ送信
        /// </summary>
        public Action<CalProgress> ProcMes { private get; set; }
        /// <summary>
        /// メッセージ送信
        /// </summary>
        public Action<string> SendMes { private get; set; }
        /// <summary>
        /// 実行可否
        /// </summary>
        public Func<NumBase, float, string, ResBase> IsDoExecut { private get; set; }
        /// <summary>
        /// FCD
        /// </summary>
        public IFDD _FDD { get; set; }
        /// <summary>
        /// DI注入
        /// </summary>
        public void Inject(IFDD fcd)
        {
            _FDD = fcd;
            _FDD.FDDChanged += (s, e) =>
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        UpdateUC(s, e);
                    });
                }
                else
                {
                    UpdateUC(s, e);
                }
            };

            UC_ORIGIN.IsDoExecut = (val, mes) => IsDoExecut(val, _FDD.GetFDDOrigin(), mes);
            //原点位置変更イベント
            UC_ORIGIN.NumUpdateArg += (s, e) =>
            {
                _FDD.UpdateOrigin(e.NumValue, SendMes);
            };
            //原点位置からの更新イベント
            UC_ORIGIN.RequestUpdate += (s, e) =>
            {
                _FDD.UpdateAll();
            };

            //変更の問い合わせ画面
            UC_BindSlider.IsDoExecut = (val, mes) => IsDoExecut(val, _FDD.GetFDD(), mes);

            //前進ボタンのOn/Offが変わった
            UC_BindSlider.BackwordMoveStsChanged += (s, e) =>
            {
                if (e.Chk)
                {
                    _FDD.Manual(MDirMode.Backward, SendMes);
                }
                else
                {
                    _FDD.Stop(SendMes);
                }
            };
            //後退ボタンのOn/Offが変わった
            UC_BindSlider.ForwordMoveStsChanged += (s, e) =>
            {
                if (e.Chk)
                {
                    _FDD.Manual(MDirMode.Forward, SendMes);
                }
                else
                {
                    _FDD.Stop(SendMes);
                }
            };

            ////インデックス移動
            UC_BindSlider.MoveIdexChanged += async (s, e) =>
            {
                ComProgress com = new ComProgress()
                {
                    prog = new Progress<CalProgress>(ProcMes),
                    Message = "Index"
                };
                bool result = await Task.Run(() => _FDD.Index(e.NumValue, SendMes, com));
            };

            //値の更新要求
            UC_BindSlider.RequestUpdate += (s, e) =>
            {
                _FDD?.UpdateAll();
            };
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void UpdateUC(object s, EventArgs e)
        {
            var tmpmax = (s as FDD).Param.Max_fdd_forNumUD;
            var tmpmin = (s as FDD).Param.Min_fdd_forNumUD;

            var maxfdd = (s as FDD).Param.Max_fdd;
            var minfdd = (s as FDD).Param.Min_fdd;

            //FDDのバインドスライダー初期値
            UC_BindSlider.SetInitValue(
                maxfdd,
                minfdd,
                (s as FDD).GetFDDDeip(),
                tmpmax,
                tmpmin,
                "0.1",
                "mm"
                );
            //FDDのバインドスライダー値入力
            UC_BindSlider.SetValue((s as FDD).FddProvider.StsFDD.CorrectRange(tmpmin, tmpmax));

            UC_BindSlider.SetValueBLimit((s as FDD).FddProvider.StsBLimit);

            UC_BindSlider.SetValueFLimit((s as FDD).FddProvider.StsFLimit);


            //FDD原点位置初期化
            UC_ORIGIN.SetInitValue(
                "FDD原点位置",
                (s as FDD).GetFDDOriginDeip(),
                -tmpmin,
                -tmpmax
                );
            //FDD原点位置更新
            UC_ORIGIN.SetValue(
                (s as FDD).FddProvider.FDDOrigin.CorrectRange(-tmpmax, -tmpmin)
                );
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UC_FDD()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 原点復帰ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OriginBtn_Click(object sender, EventArgs e)
        {
            ComProgress com = new ComProgress()
            {
                prog = new Progress<CalProgress>(ProcMes),
                Message = "原点復帰"
            };
            bool result = await Task.Run(() => _FDD.Origin(SendMes, com));
        }
    }
}
