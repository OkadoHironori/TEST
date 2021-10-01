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
using Itc.Common.Extensions;

namespace TXSMechaControl.Rotation
{
    /// <summary>
    /// 回転操作のクラス
    /// </summary>
    public partial class UC_OptRot : UserControl
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
            UC_ROT_BindSlider.IsDoExecut = (val, mes) => IsDoExecut(val, Mecha._Rot.GetRot(), mes);

            //Rotationの何かが変わった
            Mecha._Rot.RotChanged += (s, e) =>
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
            //PLCからROTのIndex指示があった
            Mecha._Rot.PLCRotRequest += async (s, e) =>
            {
                InvokeMoveEnabelSettings(false);

                cts = new CancellationTokenSource();
                ComProgress com = new ComProgress()
                {
                    prog = new Progress<CalProgress>(ShowProgress),
                    ctoken = cts,
                    Message = "Index"
                };

                bool result = await Task.Run(() => Mecha._Rot.Index((s as Rotation).RotProvider.StsRotIndexPos, SendMes, com));

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

            //前進ボタンのOn/Offが変わった
            UC_ROT_BindSlider.BackwordMoveStsChanged += (s, e) =>
            {
                if (e.Chk)
                {
                    Mecha._Rot.Manual(RevMode.CCW, SendMes);
                }
                else
                {
                    Mecha._Rot.Stop(SendMes);
                }
            };
            //後退ボタンのOn/Offが変わった
            UC_ROT_BindSlider.ForwordMoveStsChanged += (s, e) =>
            {
                if (e.Chk)
                {
                    Mecha._Rot.Manual(RevMode.CW, SendMes);
                }
                else
                {
                    Mecha._Rot.Stop(SendMes);
                }
            };

            //値の更新要求
            UC_ROT_BindSlider.RequestUpdate += (s, e) =>
            {
                Mecha._Rot?.UpdateAll();
            };
        }
        /// <summary>
        /// 表示更新
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void UpdateUCOptRot(object s, EventArgs e)
        {
            var tmpmax = (s as Rotation).GetRotMax() ;
            var tmpmin = (s as Rotation).GetRotMin() ;

            //回転のバインドスライダー初期値
            UC_ROT_BindSlider.SetInitValue(
                tmpmax,
                tmpmin,
                (s as Rotation).GetRotDeip(),
                tmpmax,
                tmpmin,
                "0.01",
                "deg"
                );

            //回転のバインドスライダー値入力
            UC_ROT_BindSlider.SetValue((s as Rotation).RotProvider.StsRot.CorrectRange(tmpmin, tmpmax));

            UC_SelCtrlSpd.SetValue((s as Rotation).Param.CurrentSpd, (s as Rotation).Param.Speedlist, (s as Rotation).GetRotSpeedMax(), (s as Rotation).GetRotSpeedMin(),"deg/sec");

            UC_Alarm.SetText("", true, !(s as Rotation).RotProvider.StsAlarm);

            UC_Ready.SetText("", true, !(s as Rotation).RotProvider.StsReady);

        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UC_OptRot()
        {
            InitializeComponent();

            ////インデックス移動
            UC_ROT_BindSlider.MoveIdexChanged += async (s, e) =>
            {
                InvokeMoveEnabelSettings(false);

                cts = new CancellationTokenSource();
                ComProgress com = new ComProgress()
                {
                    prog = new Progress<CalProgress>(ShowProgress),
                    ctoken = cts,
                    Message = "Index"
                };

                bool result = await Task.Run(() => Mecha._Rot.Index(e.NumValue, SendMes, com));

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
                Mecha._Rot?.UpdateSpeed(e.SelectName, SendMes);
            };
            UC_SelCtrlSpd.NumChanged += (s, e) =>
            {
                Mecha._Rot?.UpdateSpeed(e.NumValue, SendMes);
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
                    UC_ROT_BindSlider.Enabled = set;
                    OriginBtn.Enabled = set;
                });
            }
            else
            {
                UC_ROT_BindSlider.Enabled = set;
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

            bool result = await Task.Run(() => Mecha._Rot.Origin(SendMes, p, cts));

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
        /// 停止ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
            InvokeMoveEnabelSettings(true);
        }

        private void UC_ROT_BindSlider_Load(object sender, EventArgs e)
        {

        }
    }
}
