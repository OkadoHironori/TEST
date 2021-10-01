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
using Itc.Common.Event;
using Itc.Common.TXEnum;
using Itc.Common.FormObject;
using TXSMechaControl.FCD;
using Itc.Common.Basic;
using Itc.Common.Extensions;

namespace TXSMechaControl.MechaIntegrate
{
    using NumUpdateEventHandler = Action<object, NumUpdateEventArgs>;
    using ChkChangeEventHandler = Action<object, ChkChangeEventArgs>;
    /// <summary>
    /// FCD制御用のユーザーコントロール
    /// </summary>
    public partial class UC_OptFCD : UserControl
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
        /// I/F注入
        /// </summary>
        public void Inject(IMechaIntegrate mecha)
        {
            Mecha = mecha;

            //変更の問い合わせ画面
            FCD_UC_BindSlider.IsDoExecut = (val, mes) => IsDoExecut(val, Mecha._FCD.GetFCD(), mes);
            UC_HOME.IsDoExecut = (val, mes) => IsDoExecut(val, Mecha._FCD.GetFCDHome(), mes);
            UC_ORIGIN.IsDoExecut = (val, mes) => IsDoExecut(val, Mecha._FCD.GetFCDOrigin(), mes);
            UC_HAMA1.IsDoExecut = (val, mes) => IsDoExecut(val, Mecha._FCD.GetFCDHama1(), mes);
            UC_HAMA2.IsDoExecut = (val, mes) => IsDoExecut(val, Mecha._FCD.GetFCDHama2(), mes);

            //FCDの何かが変わった
            Mecha._FCD.FCDChanged += (s, e) =>
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        UpdateUCOptFCD(s, e);
                    });
                }
                else
                {
                    UpdateUCOptFCD(s, e);
                }
            };
            //前進ボタンのOn/Offが変わった
            FCD_UC_BindSlider.BackwordMoveStsChanged += (s,e) =>
            {
                if (e.Chk)
                {
                    Mecha._FCD.Manual(MDirMode.Backward, SendMes);
                }
                else
                {
                    Mecha._FCD.Stop(SendMes);
                }
            };
            //後退ボタンのOn/Offが変わった
            FCD_UC_BindSlider.ForwordMoveStsChanged += (s, e) => 
            {
                if (e.Chk)
                {
                    Mecha._FCD.Manual(MDirMode.Forward, SendMes);
                }
                else
                {
                    Mecha._FCD.Stop(SendMes);
                }
            };

            //ホーム位置変更イベント
            UC_HOME.NumUpdateArg += (s, e) =>
            {
                Mecha._FCD.UpdateHome(e.NumValue, SendMes);
            };
            //ホーム位置からの更新イベント
            UC_HOME.RequestUpdate += (s, e) =>
            {
                Mecha._FCD.UpdateAll();
            };

            //原点位置変更イベント
            UC_ORIGIN.NumUpdateArg += (s, e) =>
            {
                Mecha._FCD.UpdateOrigin(e.NumValue, SendMes);
            };
            //原点位置からの更新イベント
            UC_ORIGIN.RequestUpdate += (s, e) =>
            {
                Mecha._FCD.UpdateAll();
            };

            //浜松ホトニクスFCD1イベント
            UC_HAMA1.NumUpdateArg += (s, e) =>
            {
                Mecha._FCD.UpdateHamamatu1(e.NumValue, SendMes);
            };
            //浜松ホトニクスFCD1からの更新イベント
            UC_HAMA1.RequestUpdate += (s, e) =>
            {
                Mecha._FCD.UpdateAll();
            };

            //浜松ホトニクスFCD2変更イベント
            UC_HAMA2.NumUpdateArg += (s, e) =>
            {
                Mecha._FCD.UpdateHamamatu2(e.NumValue, SendMes);
            };
            //浜松ホトニクスFCD2からの更新イベント
            UC_HAMA2.RequestUpdate += (s, e) =>
            {
                Mecha._FCD.UpdateAll();
            };
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void UpdateUCOptFCD(object s, EventArgs e)
        {
            var tmpmax = (s as FCD.FCD).Param.Max_fcd_forNumUD;
            var tmpmin = (s as FCD.FCD).Param.Min_fcd_forNumUD;

            var maxfcd = (s as FCD.FCD).Param.Max_fcd;
            var minfcd = (s as FCD.FCD).Param.Min_fcd;

            //FCDのバインドスライダー初期値
            FCD_UC_BindSlider.SetInitValue(
                maxfcd,
                minfcd,
                (s as FCD.FCD).GetFCDDeip(),
                tmpmax,
                tmpmin,
                "0.1",
                "mm"
                );
            //FCDのバインドスライダー値入力
            FCD_UC_BindSlider.SetValue((s as FCD.FCD).FcdProvider.StsFCD.CorrectRange(tmpmin, tmpmax));

            FCD_UC_BindSlider.SetValueBLimit((s as FCD.FCD).FcdProvider.StsBLimit);

            FCD_UC_BindSlider.SetValueFLimit((s as FCD.FCD).FcdProvider.StsFLimit);

            //FCDホーム位置初期化
            UC_HOME.SetInitValue(
                "Home Posi",
                (s as FCD.FCD).GetFCDHomeDeip(),
                maxfcd,
                minfcd
                );

            //FCDホーム位置更新
            UC_HOME.SetValue(
                (s as FCD.FCD).FcdProvider.FCDHome.CorrectRange(minfcd, maxfcd)
                );

            //FCD原点位置初期化
            UC_ORIGIN.SetInitValue(
                "Origin Posi",
                (s as FCD.FCD).GetFCDOriginDeip(),
                maxfcd,
                minfcd
                );
            //FCDホーム位置更新
            UC_ORIGIN.SetValue(
                (s as FCD.FCD).FcdProvider.FCDOrigin.CorrectRange(minfcd, maxfcd)
                );

            //浜松ホトニクスFCD1位置初期化
            UC_HAMA1.SetInitValue(
                "Hamamatu Posi 1",
                (s as FCD.FCD).GetFCDHama1Deip(),
                maxfcd,
                minfcd
                );
            //浜松ホトニクスFCD1位置更新
            UC_HAMA1.SetValue(
                (s as FCD.FCD).FcdProvider.FCD_Hama1.CorrectRange(minfcd, maxfcd)
                );

            //浜松ホトニクスFCD2位置初期化
            UC_HAMA2.SetInitValue(
                "Hamamatu Posi 2",
                (s as FCD.FCD).GetFCDHama2Deip(),
                maxfcd,
                minfcd
                );
            //浜松ホトニクスFCD2位置更新
            UC_HAMA2.SetValue(
                (s as FCD.FCD).FcdProvider.FCD_Hama2.CorrectRange(minfcd, maxfcd)
                );

            //スピード
            if ((s as FCD.FCD).FcdProvider.IsMaxSpeedFix && (s as FCD.FCD).FcdProvider.IsMinSpeedFix)
            {
                UC_SelCtrlSpd.SetValue((s as FCD.FCD).Param.CurrentSpd, (s as FCD.FCD).Param.Speedlist, (s as FCD.FCD).FcdProvider.StsMaxSpeed, (s as FCD.FCD).FcdProvider.StsMinSpeed, "mm/sec");
            }
            //干渉
            this.STS_TEXT.SetValue((s as FCD.FCD).FcdProvider.IsTouch ? MechaRes.COLITION : "", (s as FCD.FCD).FcdProvider.IsTouch);

            //移動中のボタンの色
            this.STS_TXT_MOVE.SetValue((s as FCD.FCD).FcdProvider.StsBusy ? MechaRes.ON_MOVE : MechaRes.ON_STOP, (s as FCD.FCD).FcdProvider.StsBusy);

            //大型テーブル装着？
            this.LTable_TXTB.SetValue((s as FCD.FCD).FcdProvider.StsRotLargeTable ? MechaRes.LARGE_TABLE_MODE : "", (s as FCD.FCD).FcdProvider.StsRotLargeTable);

        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UC_OptFCD()
        {
            InitializeComponent();

            ////インデックス移動
            FCD_UC_BindSlider.MoveIdexChanged += async (s, e) =>
            {
                InvokeMoveEnabelSettings(false);

                cts = new CancellationTokenSource();
                var p = new Progress<CalProgress>(this.ShowProgress);

                bool result = await Task.Run(() => Mecha._FCD.Index(e.NumValue, SendMes, p, cts.Token));

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
            };

            //値の更新要求
            FCD_UC_BindSlider.RequestUpdate += (s, e) =>
             {
                 Mecha._FCD?.UpdateAll();
             };

            //速度変更
            UC_SelCtrlSpd.SelectChange += (s, e) =>
             {
                 Mecha._FCD?.UpdateSpeed(e.SelectName, SendMes);
             };
            UC_SelCtrlSpd.NumChanged += (s, e) =>
            {
                Mecha._FCD?.UpdateSpeed(e.NumValue, SendMes);
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
                    FCD_UC_BindSlider.Enabled = set;
                    OriginBtn.Enabled = set;
                });
            }
            else
            {
                FCD_UC_BindSlider.Enabled = set;
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
        /// 原点復帰ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OriginBtn_Click(object sender, EventArgs e)
        {

            InvokeMoveEnabelSettings(false);

            cts = new CancellationTokenSource();

            var p = new Progress<CalProgress>(ShowProgress);

            bool result = await Task.Run(() => Mecha._FCD.Origin(SendMes, p, cts.Token));

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
        /// <summary>
        /// ストップボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopBtn_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
        }
    }
}