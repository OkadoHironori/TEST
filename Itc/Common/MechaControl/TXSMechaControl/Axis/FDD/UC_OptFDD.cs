using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Itc.Common.Basic;
using Itc.Common.TXEnum;
using Itc.Common.Extensions;
using TXSMechaControl.MechaIntegrate;

namespace TXSMechaControl.FDD
{

    public partial class UC_OptFDD : UserControl
    {
        /// <summary>
        /// キャンセルトークンソース
        /// </summary>
        private CancellationTokenSource cts;
        /// <summary>
        /// メッセージ送信
        /// </summary>
        public Action<string> SendMes { private get; set; }
        /// <summary>
        /// 実行可否
        /// </summary>
        public Func<NumBase, float, string, ResBase> IsDoExecut { private get; set; }
        /// <summary>
        /// メカインターフェイス
        /// </summary>
        public IMechaIntegrate Mecha { get; set; }
        /// <summary>
        /// DI注入
        /// </summary>
        public void Inject(IMechaIntegrate mecha)
        {
            Mecha = mecha;

            //変更の問い合わせ画面
            FDD_UC_BindSlider.IsDoExecut = (val, mes) => IsDoExecut(val, Mecha._FDD.GetFDD(), mes);
            UC_ORIGIN.IsDoExecut = (val, mes) => IsDoExecut(val, Mecha._FDD.GetFDDOrigin(), mes);       

            //FDDの何かが変わった
            Mecha._FDD.FDDChanged += (s, e) =>
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        UpdateUCOptFDD(s, e);
                    });
                }
                else
                {
                    UpdateUCOptFDD(s, e);
                }
            };
            //前進ボタンのOn/Offが変わった
            FDD_UC_BindSlider.BackwordMoveStsChanged += (s, e) =>
            {
                if (e.Chk)
                {
                    Mecha._FDD.Manual(MDirMode.Backward, SendMes);
                }
                else
                {
                    Mecha._FDD.Stop(SendMes);
                }
            };
            //後退ボタンのOn/Offが変わった
            FDD_UC_BindSlider.ForwordMoveStsChanged += (s, e) =>
            {
                if (e.Chk)
                {
                    Mecha._FDD.Manual(MDirMode.Forward, SendMes);
                }
                else
                {
                    Mecha._FDD.Stop(SendMes);
                }
            };

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void UpdateUCOptFDD(object s, EventArgs e)
        {
            
            var tmpmax = (s as FDD).Param.Max_fdd_forNumUD;
            var tmpmin = (s as FDD).Param.Min_fdd_forNumUD;
            var maxfdd = (s as FDD).Param.Max_fdd;
            var minfdd = (s as FDD).Param.Min_fdd;

            //FCDのバインドスライダー初期値
            FDD_UC_BindSlider.SetInitValue(
                maxfdd,
                minfdd,
                (s as FDD).GetFDDDeip(),
                tmpmax,
                tmpmin,
                "0.1",
                "mm"
                );

            ////FCDのバインドスライダー値入力
            FDD_UC_BindSlider.SetValue((s as FDD).FddProvider.StsFDD.CorrectRange(tmpmin, tmpmax));
            ////スライダーリミット
            FDD_UC_BindSlider.SetValueBLimit((s as FDD).FddProvider.StsBLimit);
            ////スライダーリミット
            FDD_UC_BindSlider.SetValueFLimit((s as FDD).FddProvider.StsFLimit);

            //TblY原点位置初期化
            UC_ORIGIN.SetInitValue(
                "Origin Posi",
                (s as FDD).GetFDDOriginDeip(),
                tmpmax,
                tmpmin
                );

            //原点位置更新
            UC_ORIGIN.SetValue(
                (s as FDD).FddProvider.FDDOrigin.CorrectRange(tmpmin, tmpmax)
                );


            //スピード
            if ((s as FDD).FddProvider.IsMaxSpeedFix && (s as FDD).FddProvider.IsMinSpeedFix)
            {
                UC_SelSpdCtrl.SetValue((s as FDD).Param.CurrentSpd, (s as FDD).Param.Speedlist, (s as FDD).FddProvider.StsMaxSpeed, (s as FDD).FddProvider.StsMinSpeed, "mm/sec");
            }

            //動作中
            this.STS_TXT_MOVE.SetValue((s as FDD).FddProvider.StsBusy ? MechaRes.ON_MOVE : MechaRes.ON_STOP);

        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UC_OptFDD()
        {
            InitializeComponent();

            ////インデックス移動
            FDD_UC_BindSlider.MoveIdexChanged += async (s, e) =>
            {
                InvokeMoveEnabelSettings(false);

                cts = new CancellationTokenSource();
                var p = new Progress<CalProgress>(ShowProgress);

                bool result = await Task.Run(() => Mecha._FDD.Index(e.NumValue, SendMes, p, cts.Token));

                if (result)
                {
                    InvokeMoveEnabelSettings(true);
                }
                else
                {
                    InvokeMoveEnabelSettings(true);
                }
            };

            //値の更新要求
            FDD_UC_BindSlider.RequestUpdate += (s, e) =>
            {
                Mecha._FDD?.UpdateAll();
            };

            //速度変更
            UC_SelSpdCtrl.SelectChange += (s, e) =>
            {
                Mecha._FDD?.UpdateSpeed(e.SelectName, SendMes);
            };
            UC_SelSpdCtrl.NumChanged += (s, e) =>
            {
                Mecha._FDD?.UpdateSpeed(e.NumValue, SendMes);
            };
        }
        /// <summary>
        /// 動作時のEnabel処理
        /// </summary>
        /// <param name="set"></param>
        private void InvokeMoveEnabelSettings(bool set)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    FDD_UC_BindSlider.Enabled = set;
                    OriginBtn.Enabled = set;
                });
            }
            else
            {
                FDD_UC_BindSlider.Enabled = set;
                OriginBtn.Enabled = set;
            }
        }
        /// <summary>
        /// 進捗表示・状態表示
        /// </summary>
        /// <param name="inf"></param>
        private void ShowProgress(CalProgress inf)
        {
            this.STS_TEXT.SetValue(inf.Status);
        }
        /// <summary>
        /// 原点復帰
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OriginBtn_Click(object sender, EventArgs e)
        {
            InvokeMoveEnabelSettings(false);

            var p = new Progress<CalProgress>(ShowProgress);

            cts = new CancellationTokenSource();

            bool result = await Task.Run(() => Mecha._FDD.Origin(SendMes, p, cts.Token));

            if (result)
            {
                this.STS_TEXT.SetValue(MechaRes.PROCESS_COMPLEAT);
                InvokeMoveEnabelSettings(true);
            }
            else
            {
                this.STS_TEXT.SetValue(MechaRes.PROCESS_STOP);
                InvokeMoveEnabelSettings(true);
            }
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
            InvokeMoveEnabelSettings(true);
        }
    }
}
