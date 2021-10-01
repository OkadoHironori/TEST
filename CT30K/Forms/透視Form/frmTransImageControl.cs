using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//
//using CT30K.Modules;
using CTAPI;
using System.Resources;
using CT30K.Properties;

namespace CT30K
{
    public partial class frmTransImageControl : Form
    {
        #region メンバ変数
        /// <summary>
        /// フォームのインスタンス変数（シングルトン用）
        /// </summary>
        private static frmTransImageControl myForm = null;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public frmTransImageControl()
        {
            InitializeComponent();

            // イベント定義
            InitializeEventHandler();
        }
        #endregion

        #region インスタンス（シングルトン用）
        /// <summary>
        /// インスタンス（シングルトン用）
        /// </summary>
        public static frmTransImageControl Instance
        {
            get
            {
                if (myForm == null || myForm.IsDisposed)
                {
                    myForm = new frmTransImageControl();
                }

                return myForm;
            }
        }
        #endregion

        #region イベント定義
        /// <summary>
        /// イベント定義
        /// </summary>
        private void InitializeEventHandler()
        {
            // CTBusyステータス変更時処理
            modCTBusy.StatusChanged += delegate
            {
                // 透視画像ボタン
                btnTransImage.Enabled = !Convert.ToBoolean(modCTBusy.CTBusy);

                // ダブルオブリークボタン：スキャン中・再構成リトライ中・コーン後再構成中・ズーミング中は操作不可
                btnDoubleOblique.Enabled = !Convert.ToBoolean(modCTBusy.CTBusy & (modCTBusy.CTScanStart | modCTBusy.CTReconstruct | modCTBusy.CTZooming));
            };

            //*******************************************************************************
            //機　　能： 「透視画像処理」ボタンクリック時処理
            //
            //           変数名          [I/O] 型        内容
            //引　　数： なし
            //戻 り 値： なし
            //
            //補　　足： なし
            //
            //履　　歴： V15.00 2009/08/17 (SI1)間々田   新規作成
            //*******************************************************************************
            btnTransImage.Click += (sender, e) =>
            {

                //「透視」タブに切り替え
                frmScanControl.Instance.tabControl.SelectedIndex = 2;

                //マウスポインタも移動
                int x = 0;
                int y = 0;
                x = frmScanControl.Instance.ClientRectangle.Left + frmScanControl.Instance.ClientRectangle.Width / 2;
                y = frmScanControl.Instance.ClientRectangle.Top + frmScanControl.Instance.ClientRectangle.Height / 2;
                
                Winapi.SetCursorPos(x, y);
            };
 
     
            //*******************************************************************************
            //機　　能： フォームロード処理
            //
            //           変数名          [I/O] 型        内容
            //引　　数： なし
            //戻 り 値： なし
            //
            //補　　足： なし
            //
            //履　　歴： V17.02  2010/07/22  やまおか   新規作成
            //*******************************************************************************
            this.Load += (sender, e) =>
            {

                //空白を入れてタイトルバーを表示させる
                this.Text = "   ";

                //v17.60 キャプションのストリングテーブル化 by 長野 2011/05/25
                //直接設定するので、LoadResStrings(From)は使用しない
                //StringTable.LoadResStrings(ref this);
               
                //v17.60 キャプションのストリングテーブル化 by 長野 2011/05/25
                //ToolbarからBuutonに変更
                //Toolbar1.Items[1].Text = Resources.STR_17479;
                btnDoubleOblique.Text = Resources.STR_17479;

                //v17.60 英語用レイアウト
                if (modCT30K.IsEnglish == true)
                {
                    EnglishAdjustLayout();
                }

            };

            
            //*******************************************************************************
            //機　　能： フォームリサイズ処理
            //
            //           変数名          [I/O] 型        内容
            //引　　数： なし
            //戻 り 値： なし
            //
            //補　　足： なし
            //
            //履　　歴： V15.00 2009/08/17 (SI1)間々田   新規作成
            //*******************************************************************************
            this.Resize += (sender, e) =>
            {

                btnDoubleOblique.SetBounds(ClientRectangle.Width / 2 - btnDoubleOblique.Width / 2, 
                                           ClientRectangle.Height - btnDoubleOblique.Height - 20, 
                                           0, 
                                           0, 
                                           System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
                
                btnTransImage.SetBounds(btnDoubleOblique.Left, 
                    　　　　　　　　　　btnDoubleOblique.Top - btnTransImage.Height - 20, 
                              　　　　　0, 
                                   　　 0, 
                                        System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);

            };

            //*******************************************************************************
            //機　　能： 「ダブルオブリーク」ボタン選択時処理
            //
            //           変数名          [I/O] 型        内容
            //引　　数： なし
            //戻 り 値： なし
            //
            //補　　足： なし
            //
            //履　　歴： V1.00  99/XX/XX   ????????      新規作成
            //           v13.0  2007/03/19 (WEB)間々田   ダブルオブリーク対応
            //*******************************************************************************
            btnDoubleOblique.Click += (sender, e) =>
            {
				//ダブルオブリークをアクティブにする
				modDoubleOblique.ActivateDoubleOblique();
            };
 
        }
        #endregion
        
        
        //*******************************************************************************
        //機　　能： 英語用レイアウト調整
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V17.60  2011/05/25  (検S１)長野   新規作成
        //*******************************************************************************
		private void EnglishAdjustLayout()
		{
			btnDoubleOblique.Width = 140;
			btnTransImage.Width = 140;
		}

    }
}
