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

namespace TXSMechaControl.TblY
{
    public partial class UC_OptTblY : UserControl
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
            UC_TblY_BindSlider.IsDoExecut = (val, mes) => IsDoExecut(val, Mecha._TableY.GetTblY(), mes);
            UC_ORIGIN.IsDoExecut = (val, mes) => IsDoExecut(val, Mecha._TableY.GetTblYOrigin(), mes);
            UC_HAMA1.IsDoExecut = (val, mes) => IsDoExecut(val, Mecha._TableY.GetTblYHama1(), mes);
            UC_HAMA2.IsDoExecut = (val, mes) => IsDoExecut(val, Mecha._TableY.GetTblYHama2(), mes);

            //FDDの何かが変わった
            Mecha._TableY.TblYChanged += (s, e) =>
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        UpdateUCOptTblY(s, e);
                    });
                }
                else
                {
                    UpdateUCOptTblY(s, e);
                }
            };

            //前進ボタンのOn/Offが変わった
            UC_TblY_BindSlider.BackwordMoveStsChanged += (s, e) =>
            {
                if (e.Chk)
                {
                    Mecha._TableY.Manual(MDirMode.Forward, SendMes);
                }
                else
                {
                    Mecha._TableY.Stop(SendMes);
                }
            };
            //後退ボタンのOn/Offが変わった
            UC_TblY_BindSlider.ForwordMoveStsChanged += (s, e) =>
            {
                if (e.Chk)
                {
                    Mecha._TableY.Manual(MDirMode.Backward, SendMes);
                }
                else
                {
                    Mecha._TableY.Stop(SendMes);
                }
            };

            //浜松ホトニクスFCD1イベント
            UC_HAMA1.NumUpdateArg += (s, e) =>
            {
                Mecha._TableY.UpdateHamamatu1(e.NumValue, SendMes);
                Mecha._TableY.UpdateAll();
            };
            //浜松ホトニクスFCD1からの更新イベント
            UC_HAMA1.RequestUpdate += (s, e) =>
            {
                Mecha._TableY.UpdateAll();
            };

            //浜松ホトニクスFCD2変更イベント
            UC_HAMA2.NumUpdateArg += (s, e) =>
            {
                Mecha._TableY.UpdateHamamatu2(e.NumValue, SendMes);
                Mecha._TableY.UpdateAll();
            };
            //浜松ホトニクスFCD2からの更新イベント
            UC_HAMA2.RequestUpdate += (s, e) =>
            {
                Mecha._TableY.UpdateAll();
            };
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void UpdateUCOptTblY(object s, EventArgs e)
        {
            var tmpmax = (s as TblY.TableY).Param.Max_tbly_forNumUD;
            var tmpmin = (s as TblY.TableY).Param.Min_tbly_forNumUD;

            var maxtbly = (s as TblY.TableY).Param.Max_tbly;
            var mintbly = (s as TblY.TableY).Param.Min_tbly;

            ////TblYのバインドスライダー初期値
            UC_TblY_BindSlider.SetInitValue(
                maxtbly,
                mintbly,
                (s as TblY.TableY).GetTblYDeip(),
                tmpmax,
                tmpmin,
                "0.01",
                "mm"
                );

            ////FCDのバインドスライダー値入力
            UC_TblY_BindSlider.SetValue((s as TableY).TblYProvider.StsTblY.CorrectRange(tmpmin, tmpmax));
            //スライダーリミット
            UC_TblY_BindSlider.SetValueBLimit((s as TableY).TblYProvider.StsRLimit);
            //スライダーリミット
            UC_TblY_BindSlider.SetValueFLimit((s as TableY).TblYProvider.StsLLimit);

            //TblY原点位置初期化
            UC_ORIGIN.SetInitValue(
                "Origin Posi",
                (s as TblY.TableY).GetTblYOriginDeip(),
                maxtbly,
                mintbly
                );
            //TblY原点位置更新
            UC_ORIGIN.SetValue(
                (s as TblY.TableY).TblYProvider.TblYOrigin.CorrectRange(tmpmin, tmpmax)
                );


            //浜松ホトニクスTblY1位置初期化
            UC_HAMA1.SetInitValue(
                "Hamamatu Posi 1",
                (s as TblY.TableY).GetTblYHama1Deip(),
                maxtbly,
                mintbly
                );
            //浜松ホトニクスTblY1位置更新
            UC_HAMA1.SetValue(
                (s as TblY.TableY).TblYProvider.TblY_Hama1.CorrectRange(tmpmin, tmpmax)
                );

            //浜松ホトニクスTblY2位置初期化
            UC_HAMA2.SetInitValue(
                "Hamamatu Posi 2",
                (s as TblY.TableY).GetTblYHama2Deip(),
                maxtbly,
                mintbly
                );
            //浜松ホトニクスTblY2位置更新
            UC_HAMA2.SetValue(
                (s as TblY.TableY).TblYProvider.TblY_Hama2.CorrectRange(tmpmin, tmpmax)
                );
            
            //スピード
            if ((s as TableY).TblYProvider.IsMaxSpeedFix && (s as TableY).TblYProvider.IsMinSpeedFix)
            {
                UC_SelSpdCtrl.SetValue((s as TableY).Param.CurrentSpd, (s as TableY).Param.Speedlist, (s as TableY).TblYProvider.StsMaxSpeed, (s as TableY).TblYProvider.StsMinSpeed, "mm/sec");
            }


            //干渉
            //this.STS_TEXT.SetValue((s as TblY.TableY).TblYProvider.IsTouch ? MechaRes.COLITION : "", (s as TblY.TableY).TblYProvider.IsTouch);
            //動作中
            this.STS_TXT_MOVE.SetValue((s as TblY.TableY).TblYProvider.StsBusy ? MechaRes.ON_MOVE : MechaRes.ON_STOP);
            //Busy
            this.STS_TEXT_BUSY.SetValue((s as TblY.TableY).TblYProvider.StsBusy ? "On Busy" : "Off Busy");


        }



        public UC_OptTblY()
        {
            InitializeComponent();

            ////インデックス移動
            UC_TblY_BindSlider.MoveIdexChanged += async (s, e) =>
            {
                InvokeMoveEnabelSettings(false);

                cts = new CancellationTokenSource();
                var p = new Progress<CalProgress>(ShowProgress);

                bool result = await Task.Run(() => Mecha._TableY.Index(e.NumValue, SendMes, p, cts.Token));

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
            UC_TblY_BindSlider.RequestUpdate += (s, e) =>
            {
                Mecha._TableY?.UpdateAll();
            };

            //速度変更
            UC_SelSpdCtrl.SelectChange += (s, e) =>
            {
                Mecha._TableY?.UpdateSpeed(e.SelectName, SendMes);
            };
            UC_SelSpdCtrl.NumChanged += (s, e) =>
            {
                Mecha._TableY?.UpdateSpeed(e.NumValue, SendMes);
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
                    UC_TblY_BindSlider.Enabled = set;
                    OriginBtn.Enabled = set;
                });
            }
            else
            {
                UC_TblY_BindSlider.Enabled = set;
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

            bool result = await Task.Run(() => Mecha._TableY.Origin(SendMes, p, cts.Token));

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
