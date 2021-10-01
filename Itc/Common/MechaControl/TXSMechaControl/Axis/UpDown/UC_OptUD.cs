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

namespace TXSMechaControl.UpDown
{
    public partial class UC_OptUD : UserControl
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

            UC_UD_BindSlider.IsDoExecut = (val, mes) => IsDoExecut(val, Mecha._Ud.GetUPPosi(), mes);

            //昇降の何かが変わった
            Mecha._Ud.UdChanged += (s, e) =>
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        UpdateUCOptRot(s, e);
                    });
                }
                else
                {
                    UpdateUCOptRot(s, e);
                }
            };
            //前進ボタンのOn/Offが変わった
            UC_UD_BindSlider.BackwordMoveStsChanged += (s, e) =>
            {
                if (e.Chk)
                {
                    Mecha._Ud.Manual(RevMode.CW, SendMes);
                }
                else
                {
                    Mecha._Ud.Stop(SendMes);
                }
            };
            //後退ボタンのOn/Offが変わった
            UC_UD_BindSlider.ForwordMoveStsChanged += (s, e) =>
            {
                if (e.Chk)
                {
                    Mecha._Ud.Manual(RevMode.CCW, SendMes);
                }
                else
                {
                    Mecha._Ud.Stop(SendMes);
                }
            };
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void UpdateUCOptRot(object s, EventArgs e)
        {
            var tmpmax = (s as UdStage).GetUDMax() + 5.0F;
            var tmpmin = (s as UdStage).GetUDMin() - 5.0F;

            //バインドスライダー初期値
            UC_UD_BindSlider.SetInitValue(
                (s as UdStage).GetUDMax(),
                (s as UdStage).GetUDMin(),
                (s as UdStage).GetUDDeip(),
                tmpmax,
                tmpmin,
                "0.002",
                "mm"
                );
            //FCDのバインドスライダー値入力
            UC_UD_BindSlider.SetValue((s as UdStage).Ud_Provider.StsUDPosi.CorrectRange(tmpmin, tmpmax));

            UC_SelCtrlSpd.SetValue((s as UdStage).Param.CurrentSpd, (s as UdStage).Param.Speedlist, (s as UdStage).GetUDSpeedMax(), (s as UdStage).GetUDSpeedMin(),"mm/sec");


        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UC_OptUD()
        {
            InitializeComponent();

            ////インデックス移動
            UC_UD_BindSlider.MoveIdexChanged += async (s, e) =>
            {
                InvokeMoveEnabelSettings(false);

                cts = new CancellationTokenSource();
                ComProgress com = new ComProgress()
                {
                    prog = new Progress<CalProgress>(ShowProgress),
                    ctoken = cts,
                    Message = "Index"
                };

                bool result = await Task.Run(() => Mecha._Ud.Index(e.NumValue, SendMes, com));

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

            //速度変更
            UC_SelCtrlSpd.SelectChange += (s, e) =>
            {
                Mecha._Ud.UpdateSpeed(e.SelectName, SendMes);
            };
            UC_SelCtrlSpd.NumChanged += (s, e) =>
            {
                Mecha._Ud.UpdateSpeed(e.NumValue, SendMes);
            };

            //更新要求
            UC_UD_BindSlider.RequestUpdate += (s, e) =>
            {
                Mecha._Ud?.UpdateAll();
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
                    UC_UD_BindSlider.Enabled = set;
                    OriginBtn.Enabled = set;
                });
            }
            else
            {
                UC_UD_BindSlider.Enabled = set;
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

            bool result = await Task.Run(() => Mecha._Ud.Origin(SendMes, p, cts));

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
        /// キャンセル
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
            InvokeMoveEnabelSettings(true);
        }

        private void AramReset_Click(object sender, EventArgs e)
        {
            Mecha._Ud?.UpdateAll();
        }
    }
}
