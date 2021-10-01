//---------------------------------------------------------------------------------------------------- 
// プログラム名： CT.Controls Ver1.0   
// ファイル名　： NumTextBox.cs    
// 処理概要　　： 
// 注意事項　　：   
//  
// ＯＳ　　　　： Windows XP Professional (SP2) 
// コンパイラ　： Microsoft Visual C# 2005  
//  
// VERSION  DATE        BY              CHANGE/COMMENT     
// v2.00    2008/04/16  (SS1)間々田     新規作成
//
// (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2008
//----------------------------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Itc.Common.Controls
{
    /// <summary>
    /// 数値入力用テキストボックスクラス
    /// </summary>
    public class NumTextBox : TextBox
    {
        //イベントの宣言
        public event EventHandler ValueChanged;     //値変更時に発生するイベント

        //プロパティ用内部変数
        //private float myMinimum = 0;              //最小値
        private float myMinimum = (float)short.MinValue;   //最小値    //v2.0変更 by 間々田 2008/01/29
        
        //private float myMaximum = 100;            //最大値
        private float myMaximum = (float)short.MaxValue;   //最大値    //v2.0変更 by 間々田 2008/01/29
        
        private int myDecimalPlaces = 0;            //小数点以下の桁数
        private Slider mySlider;                    //連動するSliderコントロール
        private Label myMaximumLabel;
        private Label myMinimumLabel;

        //ValueChangedイベントを生じさせないためのフラグ
        private bool IgnoreValueChange = false;     

        //入力されたキーが数字キー以外ならば無効にするためのフラグ
        private bool nonNumberEntered = false;

        private string myLastValueText = "0";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NumTextBox()
            : base()
        {            
            //テキストを０にしておく
            this.Text = myLastValueText;
        }

        /// <summary>
        /// 最小値
        /// </summary>
        public float Minimum
        {
            //取得
            get { return myMinimum; }

            //設定
            set
            {
                //最大値よりも大きければ無視
                if (value > myMaximum) return;

                //最小値を設定
                myMinimum = value;

                //連動するSliderコントロールにも反映させる
                if (mySlider != null) mySlider.Minimum = myMinimum;

                //連動するラベルコントロールにも反映させる
                //if (myMinimumLabel != null) myMinimumLabel.Text = myMinimum.ToString();
                if (myMinimumLabel != null) myMinimumLabel.Text = myMinimum.ToString("f" + DecimalPlaces.ToString()); //v2.0変更 by 間々田 2007/12/13 DecimalPlacesを反映させる

                //現在の値が新たに設定された最小値よりも小さい場合
                if (this.Value < myMinimum)
                {
                    //現在の値を最小値に更新する。ただしValueChangedイベントを生じさせない。→この場合でもValueChangedイベントを生じさせる v2.0変更 by 間々田 2008/01/10
                    //IgnoreValueChange = true;     //v2.0削除 by 間々田 2008/01/10
                    this.Value = myMinimum;
                    //IgnoreValueChange = false;    //v2.0削除 by 間々田 2008/01/10
                }
            }
        }

        /// <summary>
        /// 最大値
        /// </summary>
        public float Maximum
        {
            //取得
            get { return myMaximum; }

            //設定
            set
            {
                //最小値よりも小さければ無視
                if (value < myMinimum) return;

                //最大値を設定
                myMaximum = value;

                //連動するSliderコントロールにも反映させる
                if (mySlider != null) mySlider.Maximum = myMaximum;

                //連動するラベルコントロールにも反映させる
                //if (myMaximumLabel != null) myMaximumLabel.Text = myMaximum.ToString();
                if (myMaximumLabel != null) myMaximumLabel.Text = myMaximum.ToString("f" + DecimalPlaces.ToString()); //v2.0変更 by 間々田 2007/12/13 DecimalPlacesを反映させる

                //現在の値が新たに設定された最大値よりも大きい場合
                if (this.Value > myMaximum)
                {
                    //現在の値を最大値に更新する。ただしValueChangedイベントを生じさせない。→この場合でもValueChangedイベントを生じさせる v2.0変更 by 間々田 2008/01/10
                    //IgnoreValueChange = true;     //v2.0削除 by 間々田 2008/01/10
                    this.Value = myMaximum;
                    //IgnoreValueChange = false;    //v2.0削除 by 間々田 2008/01/10
                }
                
            }
        }

        /// <summary>
        /// Maximumプロパティと連動するMaximumLabel
        /// </summary>
        public Label MaximumLabel
        {
            get { return myMaximumLabel; }
            set { myMaximumLabel = value; }
        }

        /// <summary>
        /// Minimumプロパティと連動するMinimumLabel
        /// </summary>
        public Label MinimumLabel
        {
            get { return myMinimumLabel; }
            set { myMinimumLabel = value; }
        }

        /// <summary>
        /// 小数点以下の桁数
        /// </summary>
        public int DecimalPlaces
        {
            //取得
            get { return myDecimalPlaces; }
            
            //設定
            set
            { 
                myDecimalPlaces = value;
                this.Maximum = (float)Math.Round((decimal)myMaximum, myDecimalPlaces);
                this.Minimum = (float)Math.Round((decimal)myMinimum, myDecimalPlaces);
                IgnoreValueChange = true;
                this.Value = (float)Math.Round(this.Value, myDecimalPlaces);
                IgnoreValueChange = false;
            }
        }

        /// <summary>
        /// 数値
        /// </summary>
        public float Value
        {
            //取得
            get
            {
                return float.Parse(this.Text);
            }

            //設定
            set
            {
                ////前回の値を保持
                //float lastValue = this.Value;             //v2.0削除 by 間々田 2007/12/11

                //範囲チェック
                float val = Math.Max(myMinimum, Math.Min(value, myMaximum));
                
                //v2.1.0 1e+6以上は、指数表示とする。 by 長野 10-05-07
                //float val = this.Value;
                if (val < 1e+6)
                {
                    //設定されている小数点以下桁数を考慮してテキストボックスに表示
                    string f = (myDecimalPlaces > 0) ? "f" + myDecimalPlaces.ToString() : "";
                    this.Text = val.ToString(f);
                    this.Refresh();
                }
                else
                {
                    this.Text = val.ToString("e6");
                }
                //前回値と同じならば以後の処理はしない
                if (this.Text == myLastValueText) return;   //v2.0追加 by 間々田 2007/12/11
                    myLastValueText = this.Text;                //v2.0追加 by 間々田 2007/12/11

                //連動するスライダーSliderコントロールにも反映させる
                if (mySlider != null)
                {
                    if (!mySlider.Focused)
                    {
                        mySlider.Value = val;
                    }
                }

                //イベントを発生させる
                if (ValueChanged != null && (!IgnoreValueChange)) ValueChanged(this, EventArgs.Empty);
                //if (ValueChanged != null) ValueChanged(this, EventArgs.Empty); //v2.0変更 by 間々田 2007/12/12 //v2.0元に戻す by 間々田 2008/01/10
            }
        }

        /// <summary>
        /// 連動するSliderコントロール
        /// </summary>
        public Slider BindingSlider
        {
            //取得
            get { return mySlider; }

            //設定
            set
            {
                //if (mySlider != null) mySlider.Leave -= new EventHandler(mySlider_Leave);
                if (mySlider != null) mySlider.LostFocus -= new EventHandler(mySlider_LostFocus);//v1.09 LeaveイベントではなくLostFocusイベントを使用することにした by 間々田 2007/12/14
                mySlider = value;
                //if (mySlider != null) mySlider.Leave += new EventHandler(mySlider_Leave);
                if (mySlider != null) mySlider.LostFocus += new EventHandler(mySlider_LostFocus);//v1.09 LeaveイベントではなくLostFocusイベントを使用することにした by 間々田 2007/12/14
            }
        }

        ///// <summary>
        ///// 連動するSliderコントロールのLeaveイベントをこのコントロールのLeaveイベントに結びつける
        ///// </summary>
        //private void mySlider_Leave(object sender, EventArgs e)
        //{
        //    this.OnLeave(e);
        //}

        //v1.09 LeaveイベントではなくLostFocusイベントを使用することにした by 間々田 2007/12/14
        /// <summary>
        /// 連動するSliderコントロールのLostFocusイベントをこのコントロールのLostFocusイベントに結びつける
        /// </summary>
        private void mySlider_LostFocus(object sender, EventArgs e)
        {
            this.OnLostFocus(e);
        }

        /// <summary>
        /// キーダウン処理
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            nonNumberEntered = false;

            //入力されたキーをチェック
            //if (Tool.InRange(e.KeyCode, Keys.D0, Keys.D9)) return;
            //if (Tool.InRange(e.KeyCode, Keys.NumPad0, Keys.NumPad9)) return;
            if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) return;
            if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9) return;

            if (e.KeyCode == Keys.Back) return;
            if ((e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod) && this.Text.IndexOf(".") < 0 && myDecimalPlaces > 0) return;
            if ((e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus) && this.SelectionStart == 0) return;

            //Enterの場合、Tabキーと同じ扱いとするための措置
            if (e.KeyCode == Keys.Enter) this.ProcessDialogKey(Keys.Tab);

            //v2.1.0 指数eもしくはEを入力可能にする。10-05-06 by 長野
            if ((e.KeyCode == Keys.E)) return;

            //v2.1.0 ＋を入力可能にする。 (μmの場合，入力の最小値は1μmなので－は先頭入力以外無視することとする。) 10-05-06 by 長野
            if ((e.KeyCode == Keys.Add) | (e.KeyCode == Keys.Oemplus)) return;

            nonNumberEntered = true;

            //基本クラスの OnKeyDown メソッドを呼び出す
            base.OnKeyDown(e);
        }

        /// <summary>
        /// キープレス処理：キーダウン処理のあとに実行される
        /// </summary>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            //キーダウン時に数字（小数点ありの場合はピリオドも含む）, BackSpace
            //以外の文字が入力された場合、ここで無視する
            if (nonNumberEntered) e.Handled = true;

            //基本クラスの OnKeyPress メソッドを呼び出す
            base.OnKeyPress(e);
        }

        /// <summary>
        /// 妥当性チェック
        /// </summary>
        protected override void OnValidating(CancelEventArgs e)
        {
            try
            {
                //数値に変換して格納
                //Value = float.Parse(this.Text);
                float dummy = float.Parse(this.Text);   //v2.0変更 by 間々田 2008/01/07
            }
            catch
            {
                //元に戻す
                this.Undo();
                e.Cancel = true;
            }

            //基本クラスの OnValidating メソッドを呼び出す
            base.OnValidating(e);
        }

        /// <summary>
        /// 妥当性チェック  //v2.0追加 by 間々田 2008/01/07
        /// </summary>
        /// <param name="e"></param>
        protected override void OnValidated(EventArgs e)
        {
            //数値に変換して格納
            this.Value = float.Parse(this.Text);

            //基本クラスの OnValidated メソッドを呼び出す
            base.OnValidated(e);
        }

        /// <summary>
        /// Enabled プロパティ変更時処理
        /// </summary>
        protected override void OnEnabledChanged(EventArgs e)
        {
            //連動するSliderコントロールにも反映させる
            if (mySlider != null) mySlider.Enabled = this.Enabled;

            //基本クラスの OnEnabledChanged メソッドを呼び出す
            base.OnEnabledChanged(e);
        }

    }
}
