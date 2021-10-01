//---------------------------------------------------------------------------------------------------- 
// プログラム名： CT.Controls Ver1.0   
// ファイル名　： CheckButton.cs    
// 処理概要　　： On/Off値を持つボタンクラス
// 注意事項　　：   
//  
// ＯＳ　　　　： Windows XP Professional (SP2) 
// コンパイラ　： Microsoft Visual C# 2005  
//  
// VERSION  DATE        BY              CHANGE/COMMENT     
// v1.00    2008/04/16  (SS1)間々田     新規作成
//
// (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2008
//----------------------------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Itc.Common.Controls
{
    /// <summary>
    /// On/Off値を持つボタン（CheckBoxから派生）
    /// </summary>
    public partial class CheckButtonYellow : CheckBox
    {
        //オン時（チェック時）のボタンの色：規定値は「黄色」
        private Color myCheckedColor = Color.FromArgb(0xff, 0xff, 0x7f);

        //オフ時（アンチェック時）のボタンの色：規定値は「灰色」
        private Color myUnCheckedColor = Color.FromKnownColor(KnownColor.Control);
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CheckButtonYellow()
            : base()
        {
            //アピアランスを「ボタン」にする
            this.Appearance = Appearance.Button;

            //タブによるフォーカス移動を禁止
            this.TabStop = false;
        }

        /// <summary>
        /// オン時（チェック時）の色
        /// </summary>
        public Color CheckedColor
        {
            get { return myCheckedColor; }
            set { myCheckedColor = value; }
        }

        /// <summary>
        /// オフ時（アンチェック時）の色
        /// </summary>
        public Color UnCheckedColor
        {
            get { return myUnCheckedColor; }
            set { myUnCheckedColor = value; }
        }

        /// <summary>
        /// 値変更時処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCheckedChanged(EventArgs e)
        {
            //背景色の設定
            this.BackColor = this.Checked ? myCheckedColor : myUnCheckedColor;

            //継承元の処理を実行
            base.OnCheckedChanged(e);
        }
    }
}
