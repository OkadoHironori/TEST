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
using TXSMechaControl.MechaIntegrate;
using TXSMechaControl.UpDown;
using Itc.Common.Extensions;

namespace TXSMechaControl.FStage
{
    public partial class UC_OptFStage : UserControl
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
            UC_FX_BindSlider.IsDoExecut = (val, mes) => IsDoExecut(val, Mecha._FStage.GetFXPosi(), mes);
            UC_FY_BindSlider.IsDoExecut = (val, mes) => IsDoExecut(val, Mecha._FStage.GetFYPosi(), mes);


            //微調の何かが変わった
            Mecha._FStage.FStageChanged += (s, e) =>
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        UpdateUCOptFStage(s, e);
                    });
                }
                else
                {
                    UpdateUCOptFStage(s, e);
                }
            };
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void UpdateUCOptFStage(object s, EventArgs e)
        {
            var tmpmax = (s as FStage).GetFXMax() + 10;
            var tmpmin = (s as FStage).GetFXMin() - 10;

            //微調X軸のバインドスライダー初期値
            UC_FX_BindSlider.SetInitValue(
                (s as FStage).GetFXMax(),
                (s as FStage).GetFXMin(),
                (s as FStage).GetFXDeip(),
                tmpmax,
                tmpmin,
                "0.01",
                "mm"
                );
            //微調X軸のバインドスライダー値入力
            UC_FX_BindSlider.SetValue((s as FStage).FStage_Provider.StsFXPosi.CorrectRange(tmpmin, tmpmax));


            UC_SelSpd.SetValue((s as FStage).Param.CurrentSpd, (s as FStage).Param.Speedlist);

            //微調Y軸のバインドスライダー初期値
            UC_FY_BindSlider.SetInitValue(
                (s as FStage).GetFYMax(),
                (s as FStage).GetFYMin(),
                (s as FStage).GetFYDeip(),
                tmpmax,
                tmpmin,
                "0.01",
                "mm"
                );

            //微調Y軸のバインドスライダー値入力
            UC_FY_BindSlider.SetValue((s as FStage).FStage_Provider.StsFYPosi.CorrectRange(tmpmin, tmpmax));


        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UC_OptFStage()
        {
            InitializeComponent();

            //速度変更
            UC_SelSpd.SelectChange += (s, e) =>
            {
                Mecha._FStage.UpdateSpeed(e.SelectName, SendMes);
            };
            //微調X軸のIndex移動
            UC_FX_BindSlider.MoveIdexChanged += async (s, e) =>
            {
                InvokeMoveEnabelSettings(false);

                cts = new CancellationTokenSource();
                var p = new Progress<CalProgress>(ShowProgress);

                bool result = await Task.Run(() => Mecha._FStage.Index_FX(e.NumValue, SendMes, p, cts));

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

            //微調X軸のJog移動
            UC_FX_BindSlider.ForwordMoveStsChanged += (s, e) =>
             {
                 if (e.Chk)
                 {
                     Mecha._FStage.Manual_FX(RevMode.CCW, SendMes);
                 }
                 else
                 {
                     Mecha._FStage.Stop(SendMes);
                 }
             };
            //微調X軸のJog移動
            UC_FX_BindSlider.BackwordMoveStsChanged += (s, e) =>
            {
                if (e.Chk)
                {
                    Mecha._FStage.Manual_FX(RevMode.CW, SendMes);
                }
                else
                {
                    Mecha._FStage.Stop(SendMes);
                }
            };

            //微調Y軸のIndex移動
            UC_FY_BindSlider.MoveIdexChanged += async (s, e) =>
            {
                InvokeMoveEnabelSettings(false);

                cts = new CancellationTokenSource();
                var p = new Progress<CalProgress>(ShowProgress);

                bool result = await Task.Run(() => Mecha._FStage.Index_FY(e.NumValue, SendMes, p, cts));

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

            //微調X軸のJog移動
            UC_FY_BindSlider.ForwordMoveStsChanged += (s, e) =>
            {
                if (e.Chk)
                {
                    Mecha._FStage.Manual_FY(RevMode.CCW, SendMes);
                }
                else
                {
                    Mecha._FStage.Stop(SendMes);
                }
            };
            //微調X軸のJog移動
            UC_FY_BindSlider.BackwordMoveStsChanged += (s, e) =>
            {
                if (e.Chk)
                {
                    Mecha._FStage.Manual_FY(RevMode.CW, SendMes);
                }
                else
                {
                    Mecha._FStage.Stop(SendMes);
                }
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
                    UC_FX_BindSlider.Enabled = set;
                    UC_FY_BindSlider.Enabled = set;
                    OriginBtn.Enabled = set;
                });
            }
            else
            {
                UC_FX_BindSlider.Enabled = set;
                UC_FY_BindSlider.Enabled = set;
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

            bool result = await Task.Run(() => Mecha._FStage.Origin(SendMes, p, cts));

            if (result)
            {
                InvokeMoveEnabelSettings(true);
            }
            else
            {
                InvokeMoveEnabelSettings(true);
            }
        }
        /// <summary>
        /// ストップクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
            InvokeMoveEnabelSettings(true);
        }

        private void UC_FY_BindSlider_Load(object sender, EventArgs e)
        {

        }
    }
}
