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

namespace MechaMaintCnt.TblY
{
    public partial class UC_TblY : UserControl
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
        /// テーブルY軸I/F
        /// </summary>
        public ITblY _TblY { get; set; }
        /// <summary>
        /// DI注入
        /// </summary>
        public void Inject(ITblY tblY)
        {
            _TblY = tblY;
            _TblY.TblYChanged += (s, e) =>
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
            UC_BindSlider.IsDoExecut = (val, mes) => IsDoExecut(val, _TblY.GetTblY(), mes);

            //原点位置変更
            UC_ORIGIN.IsDoExecut = (val, mes) => IsDoExecut(val, _TblY.GetTblYOrigin(), mes);
            //原点位置変更イベント
            UC_ORIGIN.NumUpdateArg += (s, e) =>
            {
                _TblY.UpdateOrigin(e.NumValue, SendMes);
            };
            //原点位置からの更新イベント
            UC_ORIGIN.RequestUpdate += (s, e) =>
            {
                _TblY.UpdateAll();
            };

            //浜松ホト変更
            UC_HAMA1.IsDoExecut = (val, mes) => IsDoExecut(val, _TblY.GetTblYHama1(), mes);
            //浜松ホトニクスFCD1イベント
            UC_HAMA1.NumUpdateArg += (s, e) =>
            {
                _TblY.UpdateHamamatu1(e.NumValue, SendMes);
            };
            //浜松ホトニクスFCD1からの更新イベント
            UC_HAMA1.RequestUpdate += (s, e) =>
            {
                _TblY.UpdateAll();
            };

            //浜松ホト変更
            UC_HAMA2.IsDoExecut = (val, mes) => IsDoExecut(val, _TblY.GetTblYHama2(), mes);
            //浜松ホトニクスFCD2変更イベント
            UC_HAMA2.NumUpdateArg += (s, e) =>
            {
                _TblY.UpdateHamamatu2(e.NumValue, SendMes);
            };
            //浜松ホトニクスFCD2からの更新イベント
            UC_HAMA2.RequestUpdate += (s, e) =>
            {
                _TblY.UpdateAll();
            };
            //前進ボタンのOn/Offが変わった//TbleYは逆
            UC_BindSlider.BackwordMoveStsChanged += (s, e) =>
            {
                if (e.Chk)
                {
                    _TblY.Manual(MDirMode.Forward, SendMes);
                }
                else
                {
                    _TblY.Stop(SendMes);
                }
            };
            //後退ボタンのOn/Offが変わった
            UC_BindSlider.ForwordMoveStsChanged += (s, e) =>
            {
                if (e.Chk)
                {
                    _TblY.Manual(MDirMode.Backward, SendMes);
                }
                else
                {
                    _TblY.Stop(SendMes);
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
                bool result = await Task.Run(() => _TblY.Index(e.NumValue, SendMes, com));
            };

            //値の更新要求
            UC_BindSlider.RequestUpdate += (s, e) =>
            {
                _TblY?.UpdateAll();
            };
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void UpdateUC(object s, EventArgs e)
        {
            var tmpmax = (s as TblY).Param.Max_tbly_forNumUD;
            var tmpmin = (s as TblY).Param.Min_tbly_forNumUD;

            var maxtbly = (s as TblY).Param.Max_tbly;
            var mintbly = (s as TblY).Param.Min_tbly;

            //FCDのバインドスライダー初期値
            UC_BindSlider.SetInitValue(
                maxtbly,
                mintbly,
                (s as TblY).GetTblYDeip(),
                tmpmax,
                tmpmin,
                "0.1",
                "mm"
                );
            //FCDのバインドスライダー値入力
            UC_BindSlider.SetValue((s as TblY).TblYProvider.StsTblY.CorrectRange(tmpmin, tmpmax));

            UC_BindSlider.SetValueBLimit((s as TblY).TblYProvider.StsLLimit);

            UC_BindSlider.SetValueFLimit((s as TblY).TblYProvider.StsRLimit);


            //原点位置初期化
            UC_ORIGIN.SetInitValue(
                "テーブルY軸原点位置",
                (s as TblY).GetTblYOriginDeip(),
                maxtbly,
                mintbly
                );
            //原点位置更新
            UC_ORIGIN.SetValue(
                (s as TblY).TblYProvider.TblYOrigin.CorrectRange(mintbly, maxtbly)
                );

            //浜松ホトニクスTblY1位置初期化
            UC_HAMA1.SetInitValue(
                "浜ホトテーブルY軸 1",
                (s as TblY).GetTblYHama1Deip(),
                maxtbly,
                mintbly
                );
            //浜松ホトニクスTblY1位置更新
            UC_HAMA1.SetValue(
                (s as TblY).TblYProvider.TblY_Hama1.CorrectRange(tmpmin, tmpmax)
                );

            //浜松ホトニクスTblY2位置初期化
            UC_HAMA2.SetInitValue(
                "浜ホトテーブルY軸 2",
                (s as TblY).GetTblYHama2Deip(),
                maxtbly,
                mintbly
                );
            //浜松ホトニクスTblY2位置更新
            UC_HAMA2.SetValue(
                (s as TblY).TblYProvider.TblY_Hama2.CorrectRange(tmpmin, tmpmax)
                );

        }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UC_TblY()
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
            bool result = await Task.Run(() => _TblY.Origin(SendMes, com));
        }
    }
}
