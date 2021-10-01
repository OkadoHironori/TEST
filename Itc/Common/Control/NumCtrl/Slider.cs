//---------------------------------------------------------------------------------------------------- 
// プログラム名： CT.Controls Ver1.0   
// ファイル名　： Slider.cs    
// 処理概要　　： スライダークラス
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
using System.Windows.Forms;

namespace Itc.Common.Controls
{
    /// <summary>
    /// スライダークラス
    /// </summary>
    public class Slider : TrackBar
    {
        private NumTextBox myNumTextBox;
        private NumUpDown myNumUpDown;
        private NumUpDownForDecimal myNumUpDownForDecimal;
        private Label myMaximumLabel;
        private Label myMinimumLabel;
        private float myScale = 1;

        /// <summary>
        /// 連動するNumTextBoxコントロール
        /// </summary>
        public NumTextBox BindingNumTextBox
        {
            get{ return myNumTextBox;  } 
            set{ myNumTextBox = value; }
        }

        /// <summary>
        /// 連動するNumUpDownコントロール
        /// </summary>
        public NumUpDown BindingNumUpDown
        {
            get { return myNumUpDown;  }
            set { myNumUpDown = value; }
        }

        public NumUpDownForDecimal BindingNumUpDownForDecimal
        {
            get { return myNumUpDownForDecimal; }
            set { myNumUpDownForDecimal = value; }
        }

        /// <summary>
        /// Maximumプロパティと連動するMaximumLabel
        /// </summary>
        public Label MaximumLabel
        {
            get { return myMaximumLabel;  }
            set { myMaximumLabel = value; }
        }

        /// <summary>
        /// Minimumプロパティと連動するMinimumLabel
        /// </summary>
        public Label MinimumLabel
        {
            get { return myMinimumLabel;  }
            set { myMinimumLabel = value; }
        }

        /// <summary>
        /// 最大値
        /// </summary>
        new public float Maximum
        {
            get { return myScale * base.Maximum; }
            set
            {
                base.Maximum = (int)Math.Round(value / myScale, 0);
                if (myMaximumLabel != null) myMaximumLabel.Text = value.ToString();
            }
        }

        /// <summary>
        /// 最小値
        /// </summary>
        new public float Minimum
        {
            get { return myScale * base.Minimum; }
            set
            {
                base.Minimum = (int)(value / myScale);
                if (myMinimumLabel != null) myMinimumLabel.Text = value.ToString();
            }
        }

        /// <summary>
        /// 値
        /// </summary>
        new public float Value
        {
            get { return myScale * base.Value; }
            set { base.Value = (int)Math.Round(value / myScale, 0); }
        }

        /// <summary>
        /// スケール値
        /// </summary>
        public float ValueScale
        {
            get { return myScale; }
            set { if (value > 0) myScale = value; }  
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

            //基本クラスの OnKeyDown メソッドを呼び出す
            base.OnKeyDown(e);
        }

        /// <summary>
        /// スクロール処理
        /// </summary>
        protected override void OnScroll(EventArgs e)
        {
            //このコントロールにNumTextBoxが連動している場合
            if (myNumTextBox != null)
            {
                myNumTextBox.Value = this.Value;
            }

            //このコントロールにNumUpDownが連動している場合
            if (myNumUpDown != null)
            {
                myNumUpDown.Value = (int)this.Value;
            }

            //このコントロールにNumUpDownForDecimalが連動している場合
            if (myNumUpDownForDecimal != null)
            {
                myNumUpDownForDecimal.Value = this.Value;
            }

            //基本クラスの OnScroll メソッドを呼び出す
            base.OnScroll(e); 
        }
    }
}
