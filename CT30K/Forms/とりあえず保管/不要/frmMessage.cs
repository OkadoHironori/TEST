using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CT30K
{
	public partial class frmMessage : Form
	{
		private static frmMessage _Instance = null;

		public frmMessage()
		{
			InitializeComponent();
		}

		public static frmMessage Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmMessage();
				}

				return _Instance;
			}
		}


		//Private Sub Form_Load()
		//
		//'    Dim sts As Long
		//'
		//'    'ウィンドウを常に最前面表示にする
		//'    'SetWindowPosの呼び出し
		//'    '   第１引数は目的のウィンドウのハンドル(hwndプロパティ）
		//'    '   第２引数はウィンドウのＺオーダーを指定
		//'    '   第３～第６引数はウィンドウの位置、サイズ（Left, Top, Width, Height）を指定するが，
		//'    '   第７引数のフラグSWP_NOMOVE によって位置が、 SWP_NOSIZEによってサイズが無視される。
		//'    '   位置およびサイズを変更したいときは第７引数に 0 をセットする。
		//'    '   関数が正常に終了した場合0以外を、そうでない場合0を返す。
		//'    'sts = SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE Or SWP_NOSIZE)
		//'    sts = SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE)
		//'
		//'    '表示
		//'    Me.Move Screen.width / 2 - Me.width / 2, Screen.Height / 2 - 2 * Me.Height
		//    Me.Show , frmCTMenu
		//
		//End Sub

	}
}
