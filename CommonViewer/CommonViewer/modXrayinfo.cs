using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
//
using CTAPI;
using CT30K.Common;

namespace CT30K
{
	public class modXrayinfo
	{
		public struct XrayInfoStruct
		{
			public int WarmupOk;        //Ｘ線ウォームアップ状態				
			public int WarmupRunning;   //Ｘ線ウォームアップ状態				
			public int xray_filament;   //Ｘ線フィラメント状態				
			public int xray_available;  //Ｘ線アベイラブル状態				
			public int xray_on;         //Ｘ線ＯＮ状態				
			public float targetCurrent; //v19.00 追加 by 長野 ターゲット電流 2012/5/12				
			public float fbCurrent;     //v19.00 追加 by 長野 フィードバック電流 2012/05/12
		}

		private const string XRAY_INFO_FILE = "c:\\ct\\common\\xrayinf";

        //
        //   Ｘ線情報の読込
        //
        public static bool ReadXrayInfo(ref XrayInfoStruct theInfo)
        {
            bool functionReturnValue = false;
            FileStream fs = null;
            BinaryReader br = null;
            //ファイルオープン
            fs = new FileStream(XRAY_INFO_FILE, FileMode.OpenOrCreate, FileAccess.Read);
            br = new BinaryReader(fs);

            theInfo.WarmupOk = br.ReadInt32();
            theInfo.WarmupRunning = br.ReadInt32();
            theInfo.xray_filament = br.ReadInt32();
            theInfo.xray_available = br.ReadInt32();
            theInfo.xray_on = br.ReadInt32();
            theInfo.fbCurrent = br.ReadSingle();
            theInfo.targetCurrent = br.ReadSingle();

            return (functionReturnValue);
        }

		//
		//   Ｘ線情報の書き込み
		//
		public static bool WriteXrayInfo(XrayInfoStruct theInfo)
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Dim fileNo  As Integer
*/
			#endregion

			//XrayInfoStruct theInfo = default(XrayInfoStruct);

			//戻り値初期化
			bool functionReturnValue = false;

			FileStream fs = null;
			BinaryWriter bw = null;

			//エラー時の扱い
			try
			{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				'エラー時の扱い
				On Error GoTo ExitHandler
    
				'ファイルオープン
				fileNo = FreeFile()
				Open XRAY_INFO_FILE For Binary Access Write As #fileNo
*/
#endregion

				//ファイルオープン
				fs = new FileStream(XRAY_INFO_FILE, FileMode.OpenOrCreate, FileAccess.Write);
				bw = new BinaryWriter(fs);


//                frmXrayControl frmXrayControl = frmXrayControl.Instance;
//                theInfo.WarmupOk = (frmXrayControl.lblWarmupStatus.BackColor == Color.Lime ? 1 : 0);

//#region         //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
//                //        .WarmupRunning = IIf(ViscomState1 And XST1_WUP_RUNNING, 1, 0)
//                //        .xray_filament = IIf(ViscomState1 And XST1_FLM_RUNNING, 0, 1)
//                //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
//#endregion

//                theInfo.xray_available = (int)frmXrayControl.MecaXrayAvailable;
//                theInfo.xray_on = (int)frmXrayControl.MecaXrayOn;			//v19.00 追加 by長野 2012/05/12

//                //浜ホト160kVと230kVのときのみターゲット電流取得可能
//                if ((modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL9191)
//                 || (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeHamaL10801))
//                {
//                    theInfo.targetCurrent = (float)frmXrayControl.ntbTargetCurrent.Value;
//                    theInfo.fbCurrent = (float)frmXrayControl.ntbActCurrent.Value;
//                }
//                else
//                {
//                    theInfo.targetCurrent = 0;
//                    theInfo.fbCurrent = (float)frmXrayControl.ntbActCurrent.Value;
//                }

      
				bw.Write(theInfo.WarmupOk);
				bw.Write(theInfo.WarmupRunning);
				bw.Write(theInfo.xray_filament);
				bw.Write(theInfo.xray_available);
				bw.Write(theInfo.xray_on);
				bw.Write(theInfo.targetCurrent);
				bw.Write(theInfo.fbCurrent);

				//戻り値初期化
				functionReturnValue = true;
			}
			catch
			{
				//Nothing
			}
			finally
			{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
				'ファイルオープン
				Close #fileNo
*/
#endregion

				//ファイルクローズ
				if (bw != null)
				{
					bw.Close();
					bw = null;
				}

				if (fs != null)
				{
					fs.Close();
					fs = null;
				}
			}
			return functionReturnValue;
		}
	}
}
