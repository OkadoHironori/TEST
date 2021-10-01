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
//Rev22.00 追加 by長野 2015/07/03

// Add Start 2018/08/31 M.Oyama 中国語対応
using System.Threading;
// Add End 2018/08/31
using TransImage;

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

            //CTBusyのイベント化はやらない
            /*
            // CTBusyステータス変更時処理
            modCTBusy.StatusChanged += delegate
            {
                // 透視画像ボタン
                cmdTransImage.Enabled = !Convert.ToBoolean(modCTBusy.CTBusy);

                // ダブルオブリークボタン：スキャン中・再構成リトライ中・コーン後再構成中・ズーミング中は操作不可
                cmdDoubleOblique.Enabled = !Convert.ToBoolean(modCTBusy.CTBusy & (modCTBusy.CTScanStart | modCTBusy.CTReconstruct | modCTBusy.CTZooming));
            };
            */
            
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
            cmdTransImage.Click += (sender, e) =>
            {
                //「透視」タブに切り替え
                //frmScanControl.Instance.SSTab1.SelectedIndex = 2;
                //Rev26.10 条件追加 by chouno 2018/01/18 
                if (CTSettings.scaninh.Data.guide_mode == 0)
                {
                    frmScanControl.Instance.SSTab1.SelectedIndex = 3; //Rev26.00 change by chouno 2017/10/20
                }
                else
                {
                    frmScanControl.Instance.SSTab1.SelectedIndex = 2;
                }
                //マウスポインタも移動
                int x = 0;
                int y = 0;
                //2014/11/07hata キャストの修正
                //x = frmScanControl.Instance.ClientRectangle.Left + frmScanControl.Instance.ClientRectangle.Width / 2;
                //y = frmScanControl.Instance.ClientRectangle.Top + frmScanControl.Instance.ClientRectangle.Height / 2;
                //変更2015/01/28hata
                //x = frmScanControl.Instance.ClientRectangle.Left + Convert.ToInt32(frmScanControl.Instance.ClientRectangle.Width / 2F);
                //y = frmScanControl.Instance.ClientRectangle.Top + Convert.ToInt32(frmScanControl.Instance.ClientRectangle.Height / 2F);
                x = frmScanControl.Instance.Left + Convert.ToInt32(frmScanControl.Instance.ClientRectangle.Width / 2F);
                y = frmScanControl.Instance.Top + Convert.ToInt32(frmScanControl.Instance.ClientRectangle.Height / 2F);
                
                Winapi.SetCursorPos(x, y);
            };

            //*******************************************************************************
            //機　　能： 「コメント編集」ボタンクリック時処理
            //
            //           変数名          [I/O] 型        内容
            //引　　数： なし
            //戻 り 値： なし
            //
            //補　　足： なし
            //
            //履　　歴： V22.00 2015/07/02 (検S1)長野   新規作成
            //*******************************************************************************
            cmdCommentEdit.Click += (sender, e) =>
            {
                ////マウスポインタも移動
                //int x = 0;
                //int y = 0;
                ////変更2015/01/28hata
                //x = Screen.PrimaryScreen.Bounds.Width / 2 - 300;
                //y = Screen.PrimaryScreen.Bounds.Height / 2 - 100;

                //Winapi.SetCursorPos(x, y);

                //コメント編集画面を表示する
                //frmAddCommentFImage.Instance.ShowDialog(frmCTMenu.Instance);
                frmAddCommentFImage.Instance.ShowDialog();

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

                //表示位置
                var _with1 = frmTransImage.Instance;
                var _with2 = frmTransImageInfo.Instance;
                if (_with2.WindowState == FormWindowState.Normal)
                {
                    // Mod Start 2018/12/10 M.Oyama V26.40 Windows10対応の不具合対応
                    // Mod Start 2018/10/29 M.Oyama V26.40 Windows10対応 
                    //SetBounds(_with2.Left, _with2.Top + _with2.Height, _with2.Width, _with1.Height - _with2.Height);
                    //SetBounds(_with2.Left, _with2.Top + _with2.Height - 3, _with2.Width - 1, _with1.Height - _with2.Height + 3);
                    // Mod End 2018/10/29
                    SetBounds(_with2.Left, _with2.Top + _with2.Height - 2, _with2.Width - 1, _with1.Height - _with2.Height + 2);
                    // Mod End 2018/12/10
                }

                //空白を入れてタイトルバーを表示させる
                this.Text = "   ";

                //v17.60 キャプションのストリングテーブル化 by 長野 2011/05/25
                //直接設定するので、LoadResStrings(From)は使用しない
                StringTable.LoadResStrings(this);
               
                //v17.60 キャプションのストリングテーブル化 by 長野 2011/05/25
                //ToolbarからBuutonに変更
                //Toolbar1.Items[1].Text = Resources.STR_17479;
                cmdDoubleOblique.Text = CTResources.LoadResString(17479);

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

                //2014/11/07hata キャストの修正
                //cmdDoubleOblique.SetBounds(ClientRectangle.Width / 2 - cmdDoubleOblique.Width / 2, 
                //                           ClientRectangle.Height - cmdDoubleOblique.Height - 20, 
                //                           0, 
                //                           0, 
                //                           System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
                //Rev22.00 コメント編集ボタンを追加するため変更 by長野 2015/07/02
                //cmdDoubleOblique.SetBounds(Convert.ToInt32(ClientRectangle.Width / 2F - cmdDoubleOblique.Width / 2F),
                //                           ClientRectangle.Height - cmdDoubleOblique.Height - 20,
                //                           0,
                //                           0,
                //                           System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);
                
                //cmdTransImage.SetBounds(cmdDoubleOblique.Left, 
                //              cmdDoubleOblique.Top - cmdTransImage.Height - 20, 
                //                   0, 
                //                      0, 
                //                        System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);

                cmdDoubleOblique.SetBounds(Convert.ToInt32(ClientRectangle.Width / 2F - cmdDoubleOblique.Width / 2F),
                                           ClientRectangle.Height - cmdDoubleOblique.Height - 2,
                                           0,
                                           0,
                                           System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);

                cmdTransImage.SetBounds(cmdDoubleOblique.Left,
                              cmdDoubleOblique.Top - cmdTransImage.Height - 2,
                                   0,
                                      0,
                                        System.Windows.Forms.BoundsSpecified.X | System.Windows.Forms.BoundsSpecified.Y);

                fraCommentEdit.SetBounds(cmdDoubleOblique.Left,
                              cmdTransImage.Top - fraCommentEdit.Height - 2,
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
            cmdDoubleOblique.Click += (sender, e) =>
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
			cmdDoubleOblique.Width = 140;
			cmdTransImage.Width = 140;
		}
        //*******************************************************************************
        //機　　能： コメント追加チェックボックスクリック時の処理
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V22.00  2015/07/03  (検S１)長野   新規作成
        //*******************************************************************************
        private void chkCommentEdit_CheckedChanged(object sender, EventArgs e)
        {
            AddCommentInit();

            if (frmTransImage.Instance.CaptureOn == false)
            {
                CTSettings.transImageControl.Update();
            }
        }
        //*******************************************************************************
        //機　　能： コメント追加処理の準備
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： v22.00  2015-07-06  (検S1)長野      新規作成
        //*******************************************************************************
        public void AddCommentInit()
        {
            CTSettings.transImageControl.Str1 = frmAddCommentFImage.Comment1;
            CTSettings.transImageControl.Str2 = frmAddCommentFImage.Comment2;
            CTSettings.transImageControl.Str3 = frmAddCommentFImage.Comment3;
            CTSettings.transImageControl.Str4 = frmAddCommentFImage.Comment4;
            CTSettings.transImageControl.PosNum = frmAddCommentFImage.StrPos;
            CTSettings.transImageControl.CommentColor = frmAddCommentFImage.FontColor;
            CTSettings.transImageControl.CommentSize = frmAddCommentFImage.FontSize;
            if (chkCommentEdit.CheckState == CheckState.Checked)
            {

                CTSettings.transImageControl.CommentFlg = true;

                int LenPix = 0;

                CTSettings.transImageControl.marginPix = (int)(frmAddCommentFImage.FontSize);

                switch (frmAddCommentFImage.StrPos)
                {
                    case 1:

                        CTSettings.transImageControl.PosX = 50;
                        CTSettings.transImageControl.PosY = 50;
                        break;

                    case 2:
                    case 3:
                        CTSettings.transImageControl.Str1 = string.Format("{0,-20}", frmAddCommentFImage.Comment1);
                        CTSettings.transImageControl.Str2 = string.Format("{0,-20}", frmAddCommentFImage.Comment2);
                        CTSettings.transImageControl.Str3 = string.Format("{0,-20}", frmAddCommentFImage.Comment3);
                        CTSettings.transImageControl.Str4 = string.Format("{0,-20}", frmAddCommentFImage.Comment4);

                        //int[] CommenLength = new int[4] { frmAddCommentFImage.Comment1.Length, frmAddCommentFImage.Comment2.Length, frmAddCommentFImage.Comment3.Length, frmAddCommentFImage.Comment4.Length };
                        //int MaxLength = 0;
                        //int Max = 0;
                        //MaxLength = CommenLength[0];
                        //for (int i = 0; i < 4; i++)
                        //{
                        //    if (MaxLength < CommenLength[i])
                        //    {
                        //        MaxLength = CommenLength[i];
                        //        Max = i;
                        //    }
                        //}

                        string[] Comment = new string[4] { frmAddCommentFImage.Comment1, frmAddCommentFImage.Comment2, frmAddCommentFImage.Comment3, frmAddCommentFImage.Comment4 };
                        int MaxByte = 0;
                        int Max = 0;
                        MaxByte = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(Comment[0]);
                        for (int i = 0; i < 4; i++)
                        {
                            if (MaxByte < System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(Comment[i]))
                            {
                                MaxByte = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(Comment[i]);
                                Max = i;
                            }
                        }


                        //LenPix = frmAddCommentFImage.FontSize * 20;
                        //LenPix = frmAddCommentFImage.FontSize  * MaxLength * 0.75;
                        LenPix = (int)((double)frmAddCommentFImage.FontSize * (double)MaxByte * (double)0.85);

                        CTSettings.transImageControl.PosX = frmTransImage.Instance.ctlTransImage.SizeX - LenPix - 80;

                        if (frmAddCommentFImage.StrPos == 2)
                        {
                            CTSettings.transImageControl.PosY = 50;
                        }
                        else if (frmAddCommentFImage.StrPos == 3)
                        {
                            LenPix = CTSettings.transImageControl.marginPix * 4;
                            CTSettings.transImageControl.PosY = frmTransImage.Instance.ctlTransImage.SizeY - LenPix - 50;
                        }
             
                        break;


                    case 4:

                        CTSettings.transImageControl.PosX = 50;

                        LenPix = CTSettings.transImageControl.marginPix * 4;
                        CTSettings.transImageControl.PosY = frmTransImage.Instance.ctlTransImage.SizeY - LenPix - 50;

                        break;

                    default:

                        CTSettings.transImageControl.PosX = 50;
                        CTSettings.transImageControl.PosY = 50;
                        break;
                }

#if !NoCamera
                Pulsar.Mil9AddCommentInit(frmAddCommentFImage.FontSize, frmAddCommentFImage.FontColor);
#endif
                }
            else
            {
                CTSettings.transImageControl.CommentFlg = false ;  
            }
        }

        // Add Start 2018/08/31 M.Oyama 中国語対応
        private void frmTransImageControl_Load(object sender, EventArgs e)
        {
            if (Thread.CurrentThread.CurrentCulture.Name.StartsWith("zh-CN") == true)
            {
                cmdDoubleOblique.TextAlign = ContentAlignment.MiddleCenter;
            }
        }
        // Add End 2018/08/31
       }
}
