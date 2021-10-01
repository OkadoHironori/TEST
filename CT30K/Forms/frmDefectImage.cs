using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CT30K
{
    public partial class frmDefectImage : Form
    {
        private static frmDefectImage _Instance = null;

        public static frmDefectImage Instance
        {
            get
            {
                if (_Instance == null || _Instance.IsDisposed)
                {
                    _Instance = new frmDefectImage();
                }

                return _Instance;
            }
        }
        
        public frmDefectImage()
        {
            InitializeComponent();
        }

        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //Option Explicit
        //
        //'*******************************************************************************
        //'機　　能： 「はい」ボタンクリック時処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //'*******************************************************************************
        //Private Sub cmdYes_Click()
        //
        //    Select Case ImageSave(Def_IMAGE(0), DEF_CORRECT, h_size, v_size)
        //
        //        Case 1
        //            '欠陥画像を保存した旨のメッセージを表示
        //            MsgBox GetResString(IDS_Saved, LoadResString(IDS_DefImage)), vbInformation
        //
        //    End Select
        //
        //    'フォームをアンロード
        //    Unload Me
        //
        //End Sub
        //
        //'*******************************************************************************
        //'機　　能： 「いいえ」ボタンクリック時処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //'*******************************************************************************
        //Private Sub cmdNo_Click()
        //
        //    'フォームをアンロード
        //    Unload Me
        //
        //End Sub
        //
        //'*******************************************************************************
        //'機　　能： フォームロード時の処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //'*******************************************************************************
        //Private Sub Form_Load()
        //
        //    'キャプションのセット
        //    SetCaption
        //
        //    'コントロールの初期化
        //    InitControls
        //
        //    '欠陥画像表示
        //    ctlTransImage.SetImage Def_IMAGE()
        //
        //End Sub
        //
        //'*******************************************************************************
        //'機　　能： フォームアンロード時の処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //'*******************************************************************************
        //Private Sub Form_Unload(Cancel As Integer)
        //
        //    'ゲイン校正画像フォームもアンロード
        //    Unload frmGainImageForMaint
        //
        //    'メンテナンス画面を再表示
        //    frmMaint.Show , frmCTMenu
        //
        //End Sub
        //
        //'*******************************************************************************
        //'機　　能： 各コントロールのキャプションに文字列をセットする
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //'*******************************************************************************
        //Private Sub SetCaption()
        //
        //    'Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
        //    LoadResStrings Me
        //
        //    lblMessage.Caption = BuildResStr(IDS_QuerySave, IDS_DefImage)   '欠陥画像を保存しますか？
        //
        //End Sub
        //
        //'*******************************************************************************
        //'機　　能： コントロールの初期化
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //'*******************************************************************************
        //Private Sub InitControls()
        //
        //    '透視画像表示コントロール
        //    With ctlTransImage
        //
        //        .SizeX = h_size
        //        .SizeY = v_size
        //        .Width = h_size / fphm
        //        .Height = v_size / fpvm
        //
        //        '階調変更コントロールフレームの位置
        //        fraMessage.Left = .Width
        //
        //        'フォームのサイズと位置の設定
        //        Me.Move 0, 0, _
        //'            (.Width + fraMessage.Width + 4) * Screen.TwipsPerPixelX, _
        //'            (.Height + 25) * Screen.TwipsPerPixelY
        //
        //    End With
        //
        //End Sub
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''    
    
    
    }
}
