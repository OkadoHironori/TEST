using Itc.Common.Event;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TXSMechaControl.CommonComp
{
    using NumUpdateEventHandler = Action<object, NumUpdateEventArgs>;

    /// <summary>
    /// FCD用のスライダークラス
    /// </summary>
    public partial class BindSlider : TrackBar
    {
        /// <summary>
        /// スライダーでデータを変化させたか？
        /// </summary>
        public bool IsSliderChanged { get; set; } = false;
        /// <summary>
        /// スライダー手動動作イベント監視用
        /// </summary>
        private Subject<float> Source { get; } = new Subject<float>();
        /// <summary>
        /// スライダー手動動作イベント監視用
        /// </summary>
        public List<float> Data { get; set; } = new List<float>();
        /// <summary>
        /// イベント
        /// </summary>
        public NumUpdateEventHandler ChangeBindSlider;
        /// <summary>
        /// 連動するNumUpDown
        /// </summary>
        public BindNUD FCD_BindNUD { get; set; }
        /// <summary>
        /// 最大値
        /// </summary>
        new public float Maximum
        {
            get { return ValueScale * base.Maximum; }
            set
            {
                base.Maximum = (int)Math.Round(value / ValueScale, 0);
            }
        }
        /// <summary>
        /// 最小値
        /// </summary>
        new public float Minimum
        {
            get { return ValueScale * base.Minimum; }
            set
            {
                base.Minimum = (int)(value / ValueScale);
            }
        }
        /// <summary>
        /// 値
        /// </summary>
        new public float Value
        {
            get
            {
                return ValueScale * base.Value;
            }
            set
            {
                var data = (int)Math.Round(value / ValueScale, 0);

                if (base.Maximum < data)
                {
                    base.Value = (int)Maximum;
                }
                else if (base.Minimum > data)
                {
                    base.Value = (int)Minimum;
                }
                else
                {

                    base.Value = (int)Math.Round(data / ValueScale, 0);
                }
            }
        }
        /// <summary>
        /// スケール値
        /// </summary>
        public float ValueScale { get; set; } = 1F;
        /// <summary>
        /// スライダーコンストラクタ　Throttolイベント登録
        /// </summary>
        public BindSlider() : base()
        {
            Source.Throttle(TimeSpan.FromMilliseconds(500))
                .Subscribe(i =>
                {
                    var firstdata = Data.First();
                    ChangeBindSlider?.Invoke(this, new NumUpdateEventArgs(firstdata));
                });
        }
        /// <summary>
        /// キーダウン処理
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            //上下矢印キーを無効にする
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Up:
                    e.Handled = true;
                    break;
            }
            base.OnKeyDown(e);
        }
        /// <summary>
        /// スクロール処理
        /// </summary>
        protected override void OnScroll(EventArgs e)
        {
            //このコントロールにNumUpDownForDecimalが連動している場合
            if (FCD_BindNUD != null)
            {
                IsSliderChanged = true;

                Data.Add(this.Value);

                Source.OnNext(this.Value);

                FCD_BindNUD.Value = this.Value;
            }

            base.OnScroll(e);
        }
    }
}
