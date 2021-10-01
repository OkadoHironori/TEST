using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace spc53004
{
	/// <summary>
	/// Form1 の概要の説明です。
	/// </summary>
	public class DlgInx : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.GroupBox GroupBox1;
		internal System.Windows.Forms.RadioButton OprMode7;
		internal System.Windows.Forms.RadioButton OprMode6;
		internal System.Windows.Forms.RadioButton OprMode5;
		internal System.Windows.Forms.RadioButton OprMode4;
		internal System.Windows.Forms.RadioButton OprMode3;
		internal System.Windows.Forms.RadioButton OprMode2;
		internal System.Windows.Forms.RadioButton OprMode1;
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// プログラム選択画面
		/// </summary>
		public DlgInx()
		{
			//
			// Windows フォーム デザイナ サポートに必要です。
			//
			InitializeComponent();

			//
			// TODO: InitializeComponent 呼び出しの後に、コンストラクタ コードを追加してください。
			//
		}

		/// <summary>
		/// 使用されているリソースに後処理を実行します。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows フォーム デザイナで生成されたコード 
		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.GroupBox1 = new System.Windows.Forms.GroupBox();
			this.OprMode7 = new System.Windows.Forms.RadioButton();
			this.OprMode6 = new System.Windows.Forms.RadioButton();
			this.OprMode5 = new System.Windows.Forms.RadioButton();
			this.OprMode4 = new System.Windows.Forms.RadioButton();
			this.OprMode3 = new System.Windows.Forms.RadioButton();
			this.OprMode2 = new System.Windows.Forms.RadioButton();
			this.OprMode1 = new System.Windows.Forms.RadioButton();
			this.GroupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// GroupBox1
			// 
			this.GroupBox1.Controls.Add(this.OprMode7);
			this.GroupBox1.Controls.Add(this.OprMode6);
			this.GroupBox1.Controls.Add(this.OprMode5);
			this.GroupBox1.Controls.Add(this.OprMode4);
			this.GroupBox1.Controls.Add(this.OprMode3);
			this.GroupBox1.Controls.Add(this.OprMode2);
			this.GroupBox1.Controls.Add(this.OprMode1);
			this.GroupBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.GroupBox1.Location = new System.Drawing.Point(20, 14);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.Size = new System.Drawing.Size(512, 408);
			this.GroupBox1.TabIndex = 11;
			this.GroupBox1.TabStop = false;
			this.GroupBox1.Text = "動作選択";

			// 
			// OprMode7
			// 
			this.OprMode7.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.OprMode7.Location = new System.Drawing.Point(48, 352);
			this.OprMode7.Name = "OprMode7";
			this.OprMode7.Size = new System.Drawing.Size(144, 24);
			this.OprMode7.TabIndex = 8;
			this.OprMode7.Text = "プログラム終了";
			// 
			// OprMode6
			// 
			this.OprMode6.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.OprMode6.Location = new System.Drawing.Point(48, 280);
			this.OprMode6.Name = "OprMode6";
			this.OprMode6.Size = new System.Drawing.Size(136, 24);
			this.OprMode6.TabIndex = 6;
			this.OprMode6.Text = "円弧補間動作";
			// 
			// OprMode5
			// 
			this.OprMode5.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.OprMode5.Location = new System.Drawing.Point(48, 232);
			this.OprMode5.Name = "OprMode5";
			this.OprMode5.Size = new System.Drawing.Size(136, 24);
			this.OprMode5.TabIndex = 5;
			this.OprMode5.Text = "直線補間動作";
			// 
			// OprMode4
			// 
			this.OprMode4.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.OprMode4.Location = new System.Drawing.Point(48, 184);
			this.OprMode4.Name = "OprMode4";
			this.OprMode4.Size = new System.Drawing.Size(136, 24);
			this.OprMode4.TabIndex = 4;
			this.OprMode4.Text = "位置決め動作";
			// 
			// OprMode3
			// 
			this.OprMode3.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.OprMode3.Location = new System.Drawing.Point(48, 136);
			this.OprMode3.Name = "OprMode3";
			this.OprMode3.Size = new System.Drawing.Size(136, 24);
			this.OprMode3.TabIndex = 3;
			this.OprMode3.Text = "連続送り動作";
			// 
			// OprMode2
			// 
			this.OprMode2.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.OprMode2.Location = new System.Drawing.Point(48, 88);
			this.OprMode2.Name = "OprMode2";
			this.OprMode2.Size = new System.Drawing.Size(136, 24);
			this.OprMode2.TabIndex = 2;
			this.OprMode2.Text = "原点復帰動作";
			// 
			// OprMode1
			// 
			this.OprMode1.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(128)));
			this.OprMode1.Location = new System.Drawing.Point(48, 40);
			this.OprMode1.Name = "OprMode1";
			this.OprMode1.Size = new System.Drawing.Size(232, 24);
			this.OprMode1.TabIndex = 1;
			this.OprMode1.Text = "デバイスオープン/クローズ";
			// 
			// DlgInx
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(552, 437);
			this.Controls.Add(this.GroupBox1);
			this.Name = "DlgInx";
			this.Text = "[HVT] HPCI-CPD530シリーズ　サンプルプログラム";
			this.Load += new System.EventHandler(this.DlgInx_Load);
			this.Closed += new System.EventHandler(this.DlgInx_Closed);
			this.GroupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new DlgInx());
		}

		private System.Windows.Forms.RadioButton[] OprModes;

		// フォームロード処理
		private void DlgInx_Load(object sender, System.EventArgs e)
		{
			//ボタンコントロール配列の作成
			this.OprModes = new System.Windows.Forms.RadioButton[7];

			//ボタンコントロールの配列にすでに作成されているインスタンスを代入
			this.OprModes[0] = this.OprMode1;
			this.OprModes[1] = this.OprMode2;
			this.OprModes[2] = this.OprMode3;
			this.OprModes[3] = this.OprMode4;
			this.OprModes[4] = this.OprMode5;
			this.OprModes[5] = this.OprMode6;
			this.OprModes[6] = this.OprMode7;

			this.OprMode1.Checked = true;

			//イベントハンドラに関連付け（必要な時のみ）
			for (int i = 0; i < this.OprModes.Length; i++)
			{
				this.OprModes[i].MouseUp +=
					new System.Windows.Forms.MouseEventHandler(this.OprModes_MouseUp);
			}
		}

		//Buttonのクリックイベントハンドラ
		private void OprModes_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			int i = 0;
			int nIx = 8;
			uint unRet = 0;
			DlgInx f0 = new DlgInx();
			DlgDev f1 = new DlgDev();
			DlgOrg f2 = new DlgOrg();
			DlgCnt f3 = new DlgCnt();
			DlgPos f4 = new DlgPos();
			DlgLin f5 = new DlgLin();
			DlgCir f6 = new DlgCir();

			for (i = 0; i < OprModes.Length; i++)
			{
				if (sender.Equals(OprModes[i])==true)
				{
					nIx = i;
					break;
				}
			}
			switch(nIx)
			{
				// デバイス情報取得
				case 0:
					// デバイスオープン／クローズのダイアログ表示
					f1.Show();
					// メインのウィンドウを隠す
					this.Hide();
					break;
				// 原点復帰動作
				case 1: 
					// 原点復帰方法の初期値はOLS+Z相原点復帰
					// デバイスの初期化
					unRet = spc53004Class.IniDev();
					if(0==unRet)
					{
						// 原点復帰動作のダイアログ表示
						f2.Show();
						// メインのウィンドウを隠す
						this.Hide();
					} 
					else 
					{
						break;
					}
					break;
				// 連続送り動作
				case 2: 
					// デバイスの初期化
					unRet = spc53004Class.IniDev();
					if(0==unRet)
					{
						// 連続送り動作のダイアログ作成
						f3.Show(); 
						// メインのウィンドウを隠す
						this.Hide();
					}
					break;
				// 位置決め動作
				case 3: 
					// デバイスの初期化
					unRet = spc53004Class.IniDev();
					if(0==unRet)
					{
						// 位置決め動作のダイアログ作成
						f4.Show(); 
						// メインのウィンドウを隠す
						this.Hide();
					} 
					break;
				// 直線補間動作
				case 4: 
					// デバイスの初期化
					unRet = spc53004Class.IniDev();
					if(0==unRet)
					{
						// 直線補間動作のダイアログ作成
						f5.Show(); 
						// メインのウィンドウを隠す
						this.Hide();
					} 
					break;
				// 円弧補間動作
				case 5: 
					// デバイスの初期化
					unRet = spc53004Class.IniDev();
					if(0==unRet)
					{
						// 円弧補間動作のダイアログ作成
						f6.Show(); 
						// メインのウィンドウを隠す
						this.Hide();
					} 
					break;
				// プログラム終了
				case 6: 
					this.Close();
					break;
				default: 
					break;
			}
		}

		private void DlgInx_Closed(object sender, System.EventArgs e)
		{
			DlgInx f0 = new DlgInx();
			DlgDev f1 = new DlgDev();
			DlgOrg f2 = new DlgOrg();
			DlgCnt f3 = new DlgCnt();
			DlgPos f4 = new DlgPos();
			DlgLin f5 = new DlgLin();
			DlgCir f6 = new DlgCir();

			f0.Dispose();
			f1.Dispose();
			f2.Dispose();
			f3.Dispose();
			f4.Dispose();
			f5.Dispose();
			f6.Dispose();
			System.Environment.Exit(0);
		}
	}
}
