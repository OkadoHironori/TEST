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

namespace MechaMaintCnt.FCD
{
    public partial class UC_FCD : UserControl
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
        public IFCD _FCD { get; set; }
        /// <summary>
        /// DI注入
        /// </summary>
        public void Inject(IFCD fcd)
        {
            _FCD = fcd;
            _FCD.FCDChanged += (s, e) =>
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

            //変更の問い合わせ画面
            UC_BindSlider.IsDoExecut = (val, mes) => IsDoExecut(val, _FCD.GetFCD(), mes);

            UC_HOME.IsDoExecut = (val, mes) => IsDoExecut(val, _FCD.GetFCDHome(), mes);
            //原点位置変更イベント
            UC_HOME.NumUpdateArg += (s, e) =>
            {
                _FCD.UpdateHome(e.NumValue, SendMes);
            };
            //UC_HOME
            UC_ORIGIN.RequestUpdate += (s, e) =>
            {
                _FCD.UpdateAll();
            };

            UC_ORIGIN.IsDoExecut = (val, mes) => IsDoExecut(val,_FCD.GetFCDOrigin(), mes);
            //原点位置変更イベント
            UC_ORIGIN.NumUpdateArg += (s, e) =>
            {
                _FCD.UpdateOrigin(e.NumValue, SendMes);
            };
            //原点位置からの更新イベント
            UC_ORIGIN.RequestUpdate += (s, e) =>
            {
                _FCD.UpdateAll();
            };

            UC_HAMA1.IsDoExecut = (val, mes) => IsDoExecut(val, _FCD.GetFCDHama1(), mes);
            //浜松ホトニクスFCD1イベント
            UC_HAMA1.NumUpdateArg += (s, e) =>
            {
                _FCD.UpdateHamamatu1(e.NumValue, SendMes);
            };
            //浜松ホトニクスFCD1からの更新イベント
            UC_HAMA1.RequestUpdate += (s, e) =>
            {
                _FCD.UpdateAll();
            };

            UC_HAMA2.IsDoExecut = (val, mes) => IsDoExecut(val, _FCD.GetFCDHama2(), mes);
            //浜松ホトニクスFCD2変更イベント
            UC_HAMA2.NumUpdateArg += (s, e) =>
            {
                _FCD.UpdateHamamatu2(e.NumValue, SendMes);
            };
            //浜松ホトニクスFCD2からの更新イベント
            UC_HAMA2.RequestUpdate += (s, e) =>
            {
                _FCD.UpdateAll();
            };


            //前進ボタンのOn/Offが変わった
            UC_BindSlider.BackwordMoveStsChanged += (s, e) =>
            {
                if (e.Chk)
                {
                    _FCD.Manual(MDirMode.Backward, SendMes);
                }
                else
                {
                    _FCD.Stop(SendMes);
                }
            };
            //後退ボタンのOn/Offが変わった
            UC_BindSlider.ForwordMoveStsChanged += (s, e) =>
            {
                if (e.Chk)
                {
                    _FCD.Manual(MDirMode.Forward, SendMes);
                }
                else
                {
                    _FCD.Stop(SendMes);
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
                bool result = await Task.Run(() => _FCD.Index(e.NumValue, SendMes, com));
            };

            //値の更新要求
            UC_BindSlider.RequestUpdate += (s, e) =>
            {
                _FCD?.UpdateAll();
            };
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void UpdateUC(object s, EventArgs e)
        {
            var tmpmax = (s as FCD).Param.Max_fcd_forNumUD;
            var tmpmin = (s as FCD).Param.Min_fcd_forNumUD;

            var maxfcd = (s as FCD).Param.Max_fcd;
            var minfcd = (s as FCD).Param.Min_fcd;

            //FCDのバインドスライダー初期値
            UC_BindSlider.SetInitValue(
                maxfcd,
                minfcd,
                (s as FCD).GetFCDDeip(),
                tmpmax,
                tmpmin,
                "0.1",
                "mm"
                );
            //FCDのバインドスライダー値入力
            UC_BindSlider.SetValue((s as FCD).FcdProvider.StsFCD.CorrectRange(tmpmin, tmpmax));

            UC_BindSlider.SetValueBLimit((s as FCD).FcdProvider.StsBLimit);

            UC_BindSlider.SetValueFLimit((s as FCD).FcdProvider.StsFLimit);

            //FCDホーム位置初期化
            UC_HOME.SetInitValue(
                "FCDホーム位置",
                (s as FCD).GetFCDHomeDeip(),
                maxfcd,
                minfcd
                );

            //FCDホーム位置更新
            UC_HOME.SetValue(
                (s as FCD).FcdProvider.FCDHome.CorrectRange(minfcd, maxfcd)
                );

            //FCD原点位置初期化
            UC_ORIGIN.SetInitValue(
                "FCD原点位置",
                (s as FCD).GetFCDOriginDeip(),
                maxfcd,
                minfcd
                );
            //FCDホーム位置更新
            UC_ORIGIN.SetValue(
                (s as FCD).FcdProvider.FCDOrigin.CorrectRange(minfcd, maxfcd)
                );

            //浜松ホトニクスFCD1位置初期化
            UC_HAMA1.SetInitValue(
                "浜ホトFCD 1",
                (s as FCD).GetFCDHama1Deip(),
                maxfcd,
                minfcd
                );
            //浜松ホトニクスFCD1位置更新
            UC_HAMA1.SetValue(
                (s as FCD).FcdProvider.FCD_Hama1.CorrectRange(minfcd, maxfcd)
                );

            //浜松ホトニクスFCD2位置初期化
            UC_HAMA2.SetInitValue(
                "浜ホトFCD 2",
                (s as FCD).GetFCDHama2Deip(),
                maxfcd,
                minfcd
                );
            //浜松ホトニクスFCD2位置更新
            UC_HAMA2.SetValue(
                (s as FCD).FcdProvider.FCD_Hama2.CorrectRange(minfcd, maxfcd)
                );
        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UC_FCD()
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
            bool result = await Task.Run(() => _FCD.Origin(SendMes, com));
        }
    }
}
