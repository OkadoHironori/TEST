using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CT30K
{

	///* ************************************************************************** */
	///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
	///* 客先　　　　： ?????? 殿                                                   */
	///* プログラム名： FormFCpara.frm                                              */
	///* 処理概要　　： ??????????????????????????????                              */
	///* 注意事項　　： なし                                                        */
	///* -------------------------------------------------------------------------- */
	///* 適用計算機　： DOS/V PC                                                    */
	///* ＯＳ　　　　： Windows 2000  (SP4)                                         */
	///* コンパイラ　： VB 6.0                                                      */
	///* -------------------------------------------------------------------------- */
	///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
	///*                                                                            */
	///* V1.00       99/XX/XX    (TOSFEC) ????????   新規作成                       */
	///*                                                                            */
	///* -------------------------------------------------------------------------- */
	///* ご注意：                                                                   */
	///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
	///*                                                                            */
	///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2001                 */
	///* ************************************************************************** */
	public partial class frmFCpara : Form
	{

		private ComboBox[] cmbFCfilterkind = null;
		private ComboBox[] cmbFCfftsize = null;
		private TextBox[] txtFCa = null;
		private GroupBox[] fraFC = null;

		private static frmFCpara _Instance = null;

		public frmFCpara()
		{
			InitializeComponent();

            //変更2014/10/07hata_v19.51反映
            //cmbFCfilterkind = new ComboBox[] { null, cmbFCfilterkind1, cmbFCfilterkind2, cmbFCfilterkind3,
            //                                         cmbFCfilterkind4, cmbFCfilterkind5, cmbFCfilterkind6,
            //                                         cmbFCfilterkind7, cmbFCfilterkind8, cmbFCfilterkind9 };
            //cmbFCfftsize = new ComboBox[]{ null, cmbFCfftsize1, cmbFCfftsize2, cmbFCfftsize3,
            //                                     cmbFCfftsize4, cmbFCfftsize5, cmbFCfftsize6,
            //                                     cmbFCfftsize7, cmbFCfftsize8, cmbFCfftsize9 };
            //txtFCa = new TextBox[] { null, txtFCa1, txtFCa2, txtFCa3,
            //                               txtFCa4, txtFCa5, txtFCa6,
            //                               txtFCa7, txtFCa8, txtFCa9 };
            //fraFC = new GroupBox[] { null, fraFC1, fraFC2, fraFC3,
            //                               fraFC4, fraFC5, fraFC6,
            //                               fraFC7, fraFC8, fraFC9 };

            cmbFCfilterkind = new ComboBox[] { null, cmbFCfilterkind1, cmbFCfilterkind2, cmbFCfilterkind3,
													 cmbFCfilterkind4, cmbFCfilterkind5, cmbFCfilterkind6,
													 cmbFCfilterkind7, cmbFCfilterkind8, cmbFCfilterkind9,
                                                     cmbFCfilterkind10, cmbFCfilterkind11, cmbFCfilterkind12};

            cmbFCfftsize = new ComboBox[]{ null, cmbFCfftsize1, cmbFCfftsize2, cmbFCfftsize3,
												 cmbFCfftsize4, cmbFCfftsize5, cmbFCfftsize6,
												 cmbFCfftsize7, cmbFCfftsize8, cmbFCfftsize9,
                                                 cmbFCfftsize10, cmbFCfftsize11, cmbFCfftsize12};

            txtFCa = new TextBox[] { null, txtFCa1, txtFCa2, txtFCa3,
										   txtFCa4, txtFCa5, txtFCa6,
										   txtFCa7, txtFCa8, txtFCa9, 
                                           txtFCa10, txtFCa11, txtFCa12 };

            fraFC = new GroupBox[] { null, fraFC1, fraFC2, fraFC3,
										   fraFC4, fraFC5, fraFC6,
										   fraFC7, fraFC8, fraFC9, 
                                           fraFC10, fraFC11, fraFC12};

        }

		public static frmFCpara Instance
		{
			get
			{
				if (_Instance == null || _Instance.IsDisposed)
				{
					_Instance = new frmFCpara();
				}

				return _Instance;
			}
		}


		//********************************************************************************
		//機    能  ：  FCﾊﾟﾗﾒｰﾀをﾊﾞｯﾌｧに読込む
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V1.00  99/09/14  J.IWASAWA       初版
		//                     00/02/17  J.IWASAWA       ｵﾌｾｯﾄｽｷｬﾝ、高解像度ｶﾒﾗ対応
		//                     03/07/11 (SI4)間々田      コントロール・変数の配列化
		//********************************************************************************
		private void FCparaRead()
		{
			//    On Error GoTo FileError

			//delete by 間々田 2003/07/11 Start
			//    Open "C:\Ct\filter\FC1para.dat" For Binary Access Read Lock Write As #1
			//    Open "C:\Ct\filter\FC2para.dat" For Binary Access Read Lock Write As #2
			//    Open "C:\Ct\filter\FC3para.dat" For Binary Access Read Lock Write As #3
			//    Open "C:\Ct\filter\FC4para.dat" For Binary Access Read Lock Write As #4
			//    Open "C:\Ct\filter\FC5para.dat" For Binary Access Read Lock Write As #5
			//    Open "C:\Ct\filter\FC6para.dat" For Binary Access Read Lock Write As #6
			//    Open "C:\Ct\filter\FC7para.dat" For Binary Access Read Lock Write As #7
			//    Open "C:\Ct\filter\FC8para.dat" For Binary Access Read Lock Write As #8
			//    Open "C:\Ct\filter\FC9para.dat" For Binary Access Read Lock Write As #9
			//
			//    Get #1, , FC1pr
			//    Get #2, , FC2pr
			//    Get #3, , FC3pr
			//    Get #4, , FC4pr
			//    Get #5, , FC5pr
			//    Get #6, , FC6pr
			//    Get #7, , FC7pr
			//    Get #8, , FC8pr
			//    Get #9, , FC9pr
			//
			//    Get #1, , FC1a
			//    Get #2, , FC2a
			//    Get #3, , FC3a
			//    Get #4, , FC4a
			//    Get #5, , FC5a
			//    Get #6, , FC6a
			//    Get #7, , FC7a
			//    Get #8, , FC8a
			//    Get #9, , FC9a
			//
			//
			//    Close #1
			//    Close #2
			//    Close #3
			//    Close #4
			//    Close #5
			//    Close #6
			//    Close #7
			//    Close #8
			//    Close #9
			//
			//    'ﾌｨﾙﾀ種類'ﾌｨﾙﾀ種類
			//    If FC1pr(0) = 0 Then
			//        cmbFC1filterkind.text = "Laks"
			//    ElseIf FC1pr(0) = 1 Then
			//        cmbFC1filterkind.text = "Shepp"
			//    Else
			//        cmbFC1filterkind.text = "HighReso"
			//    End If
			//
			//    If FC2pr(0) = 0 Then
			//        cmbFC2filterkind.text = "Laks"
			//    ElseIf FC2pr(0) = 1 Then
			//        cmbFC2filterkind.text = "Shepp"
			//    Else
			//        cmbFC2filterkind.text = "HighReso"
			//    End If
			//
			//    If FC3pr(0) = 0 Then
			//        cmbFC3filterkind.text = "Laks"
			//    ElseIf FC3pr(0) = 1 Then
			//        cmbFC3filterkind.text = "Shepp"
			//    Else
			//        cmbFC3filterkind.text = "HighReso"
			//    End If
			//
			//    If FC4pr(0) = 0 Then
			//        cmbFC4filterkind.text = "Laks"
			//    ElseIf FC4pr(0) = 1 Then
			//        cmbFC4filterkind.text = "Shepp"
			//    Else
			//        cmbFC4filterkind.text = "HighReso"
			//    End If
			//
			//    If FC5pr(0) = 0 Then
			//        cmbFC5filterkind.text = "Laks"
			//    ElseIf FC5pr(0) = 1 Then
			//        cmbFC5filterkind.text = "Shepp"
			//    Else
			//        cmbFC5filterkind.text = "HighReso"
			//    End If
			//
			//    If FC6pr(0) = 0 Then
			//        cmbFC6filterkind.text = "Laks"
			//    ElseIf FC6pr(0) = 1 Then
			//        cmbFC6filterkind.text = "Shepp"
			//    Else
			//        cmbFC6filterkind.text = "HighReso"
			//    End If
			//
			//    If FC7pr(0) = 0 Then
			//        cmbFC7filterkind.text = "Laks"
			//    ElseIf FC7pr(0) = 1 Then
			//        cmbFC7filterkind.text = "Shepp"
			//    Else
			//        cmbFC7filterkind.text = "HighReso"
			//    End If
			//
			//    If FC8pr(0) = 0 Then
			//        cmbFC8filterkind.text = "Laks"
			//    ElseIf FC8pr(0) = 1 Then
			//        cmbFC8filterkind.text = "Shepp"
			//    Else
			//        cmbFC8filterkind.text = "HighReso"
			//    End If
			//
			//    If FC9pr(0) = 0 Then
			//        cmbFC9filterkind.text = "Laks"
			//    ElseIf FC9pr(0) = 1 Then
			//        cmbFC9filterkind.text = "Shepp"
			//    Else
			//        cmbFC9filterkind.text = "HighReso"
			//    End If
			//
			//
			//    'FFTｻｲｽﾞ
			//    cmbFC1fftsize.text = Str(FC1pr(1))
			//    cmbFC2fftsize.text = Str(FC2pr(1))
			//    cmbFC3fftsize.text = Str(FC3pr(1))
			//    cmbFC4fftsize.text = Str(FC4pr(1))
			//    cmbFC5fftsize.text = Str(FC5pr(1))
			//    cmbFC6fftsize.text = Str(FC6pr(1))
			//    cmbFC7fftsize.text = Str(FC7pr(1))
			//    cmbFC8fftsize.text = Str(FC8pr(1))
			//    cmbFC9fftsize.text = Str(FC9pr(1))
			//
			//    'a
			//    txtFC1a.text = Str(FC1a)
			//    txtFC2a.text = Str(FC2a)
			//    txtFC3a.text = Str(FC3a)
			//    txtFC4a.text = Str(FC4a)
			//    txtFC5a.text = Str(FC5a)
			//    txtFC6a.text = Str(FC6a)
			//    txtFC7a.text = Str(FC7a)
			//    txtFC8a.text = Str(FC8a)
			//    txtFC9a.text = Str(FC9a)
			//delete by 間々田 2003/07/11 End

			//append by 間々田 2003/07/11 Start

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
			Dim fileNo  As Integer
			Dim i       As Integer

			For i = 1 To 9
				fileNo = FreeFile
				Open "C:\Ct\filter\FC" & CStr(i) & "para.dat" For Binary Access Read Lock Write As #fileNo
				Get #fileNo, , FCRec(i)
				Close #fileNo
			Next

			For i = 1 To 9
				Select Case FCRec(i).Kind                   'ﾌｨﾙﾀ種類
					Case 0 To 2
						cmbFCfilterkind(i).ListIndex = FCRec(i).Kind
				End Select
				cmbFCfftsize(i).Text = Str(FCRec(i).SIZE)   'FFTｻｲｽﾞ
				txtFCa(i).Text = Str(FCRec(i).a)            'a
			Next
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			FileStream fs = null;
			BinaryReader br = null;
            string FileName = null; //v18.00追加 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

            //変更2015/01/17hata_for文を外側に移動した
            for (int i = 1 ; i <= Maint.FCRec.GetUpperBound(0); i++)
            {

			    try
			    {
				    //for (int i = 1; i <= 9; i++)
                    //v18.00変更 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05//変更2014/10/07hata_v19.51反映

                    //変更2015/01/17hata_for文を外側に移動した
                    ////for (int i = Maint.FCRec.GetLowerBound(0); i <= Maint.FCRec.GetUpperBound(0); i++)
                    //for (int i = 1 ; i <= Maint.FCRec.GetUpperBound(0); i++)
                    //{
                        //変更2014/10/07hata_v19.51反映
         			    //ファイル名
                        FileName = ("C:\\Ct\\filter\\FC*para.dat").Replace("*", Convert.ToString(i));   //v18.00追加 2011/03/10 by 間々田

                        //追加2014/10/07hata_v19.51反映
                        ////Open "C:\Ct\filter\FC" & CStr(i) & "para.dat" For Binary Access Read Lock Write As #fileNo
                        //fs = new FileStream("C:\\Ct\\filter\\FC" + Convert.ToString(i) + "para.dat", FileMode.Open, FileAccess.Read, FileShare.Read);
                        //br = new BinaryReader(fs);
                        //Maint.FCRec[i].Kind = br.ReadInt32();
                        //Maint.FCRec[i].SIZE = br.ReadInt32();
                        //Maint.FCRec[i].a = br.ReadSingle();
				        //ファイル有無チェック
				        //v18.00追加 2011/03/10 by 間々田
				        if (File.Exists(FileName))
                        {
                            fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
					        br = new BinaryReader(fs);

					        Maint.FCRec[i].Kind = br.ReadInt32();
					        Maint.FCRec[i].SIZE = br.ReadInt32();
					        Maint.FCRec[i].a = br.ReadSingle();

                        }



                    //変更2015/01/17hata_for文を外側に移動した
                    //v18.00追加 2011/03/10 by 間々田
                    //}
			    }
			    catch { }
			    finally
			    {
				    if (br != null)
				    {
					    br.Close();
					    br = null;
				    }

				    if (fs != null)
				    {
					    fs.Close();
					    fs = null;
				    }
			    }


            }   //変更2015/01/17hata_for文を外側に移動した

			//for (int i = 1; i <= 9; i++)
            //v18.00変更 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05 //変更2014/10/07hata_v19.51反映
            //for (int i = Maint.FCRec.GetLowerBound(0); i <= Maint.FCRec.GetUpperBound(0); i++)
            for (int i = 1 ; i <= Maint.FCRec.GetUpperBound(0); i++)
            {
				switch (Maint.FCRec[i].Kind)		//ﾌｨﾙﾀ種類
				{
					case 0:
					case 1:
					case 2:
						cmbFCfilterkind[i].SelectedIndex = Maint.FCRec[i].Kind;
						break;
				}
				cmbFCfftsize[i].Text = Convert.ToString(Maint.FCRec[i].SIZE);		//FFTｻｲｽﾞ
				txtFCa[i].Text = Convert.ToString(Maint.FCRec[i].a);				//a
			}
			//append by 間々田 2003/07/11 End

			//FileError:
			//    MsgBox "ﾌｧｲﾙｴﾗｰ", vbExclamation, "ﾌｨﾙﾀ関数ﾊﾟﾗﾒｰﾀ"     'ﾌｧｲﾙｴﾗｰﾒｯｾｰｼﾞ
			//   Resume Next

			//    Exit Sub
		}


		//********************************************************************************
		//機    能  ：  FCﾊﾟﾗﾒｰﾀﾌｧｲﾙ書き込み
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V1.00  99/09/13  J.IWASAWA       初版
		//                     00/02/17  J.IWASAWA       ｵﾌｾｯﾄｽｷｬﾝ、高解像度ｶﾒﾗ対応
		//                     00/04/21  J.IWASAWA       ﾌｨﾙﾀ関数追加
		//                     03/07/11 (SI4)間々田      変数の配列化
		//********************************************************************************
		private void FCparawrite()
		{
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
 *			On Error GoTo FileError
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			//delete by 間々田 2003/07/11 Start
			//    Open "C:\Ct\filter\FC1para.dat" For Binary Access Write Lock Write As #1
			//    Open "C:\Ct\filter\FC2para.dat" For Binary Access Write Lock Write As #2
			//    Open "C:\Ct\filter\FC3para.dat" For Binary Access Write Lock Write As #3
			//    Open "C:\Ct\filter\FC4para.dat" For Binary Access Write Lock Write As #4
			//    Open "C:\Ct\filter\FC5para.dat" For Binary Access Write Lock Write As #5
			//    Open "C:\Ct\filter\FC6para.dat" For Binary Access Write Lock Write As #6
			//    Open "C:\Ct\filter\FC7para.dat" For Binary Access Write Lock Write As #7
			//    Open "C:\Ct\filter\FC8para.dat" For Binary Access Write Lock Write As #8
			//    Open "C:\Ct\filter\FC9para.dat" For Binary Access Write Lock Write As #9
			//
			//    Put #1, , FC1pr
			//    Put #2, , FC2pr
			//    Put #3, , FC3pr
			//    Put #4, , FC4pr
			//    Put #5, , FC5pr
			//    Put #6, , FC6pr
			//    Put #7, , FC7pr
			//    Put #8, , FC8pr
			//    Put #9, , FC9pr
			//
			//    Put #1, , FC1a
			//    Put #2, , FC2a
			//    Put #3, , FC3a
			//    Put #4, , FC4a
			//    Put #5, , FC5a
			//    Put #6, , FC6a
			//    Put #7, , FC7a
			//    Put #8, , FC8a
			//    Put #9, , FC9a
			//
			//    Close #1
			//    Close #2
			//    Close #3
			//    Close #4
			//    Close #5
			//    Close #6
			//    Close #7
			//    Close #8
			//    Close #9
			//delete by 間々田 2003/07/11 End

			//append by 間々田 2003/07/11 Start

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
		'append by 間々田 2003/07/11 Start
			Dim fileNo  As Integer
			Dim i       As Integer

			For i = 1 To 9
				fileNo = FreeFile
				Open "C:\Ct\filter\FC" & CStr(i) & "para.dat" For Binary Access Write Lock Write As #fileNo
				Put #fileNo, , FCRec(i)
				Close #fileNo
			Next
		'append by 間々田 2003/07/11 End

			Exit Sub

		FileError:
		'    MsgBox "ﾌｧｲﾙｴﾗｰ", vbExclamation, "ﾌｨﾙﾀ関数パラメータ"    'ﾌｧｲﾙｴﾗｰﾒｯｾｰｼﾞ
			MsgBox Err.Description, vbExclamation               'v7.0 change by 間々田 2003/08/12
			Resume Next
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

			FileStream fs = null;
			BinaryWriter bw = null;

			//for (int i = 1; i <= 9; i++)
            //v18.00変更 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05 //変更2014/10/07hata_v19.51反映
            //for (int i = Maint.FCRec.GetLowerBound(0); i <= Maint.FCRec.GetUpperBound(0); i++)
            for (int i = 1 ; i <= Maint.FCRec.GetUpperBound(0); i++)
            {
				try
				{
					fs = new FileStream("C:\\Ct\\filter\\FC" + Convert.ToString(i) + "para.dat", FileMode.OpenOrCreate, FileAccess.Write);
					bw = new BinaryWriter(fs);

					bw.Write(Maint.FCRec[i].Kind);
					bw.Write(Maint.FCRec[i].SIZE);
					bw.Write(Maint.FCRec[i].a);
				}
				catch (Exception ex)
				{
					//    MsgBox "ﾌｧｲﾙｴﾗｰ", vbExclamation, "ﾌｨﾙﾀ関数パラメータ"    'ﾌｧｲﾙｴﾗｰﾒｯｾｰｼﾞ
					MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);		//v7.0 change by 間々田 2003/08/12
				}
				finally
				{
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
			}
			//append by 間々田 2003/07/11 End

		}


		//********************************************************************************
		//機    能  ：  FCﾊﾟﾗﾒｰﾀ設定
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V1.00  99/09/13  J.IWASAWA       初版
		//                     00/02/17  J.IWASAWA       ｵﾌｾｯﾄｽｷｬﾝ、高解像度ｶﾒﾗ対応
		//                     00/04/21  J.IWASAWA       ﾌｨﾙﾀ種類追加
		//                     03/07/11 (SI4)間々田      コントロール・変数の配列化
		//********************************************************************************
		public void FCparaset()
		{
			//delete by 間々田 2003/07/11 Start
			//    'ﾌｨﾙﾀ種類
			//    If cmbFC1filterkind.text = "Laks" Then
			//        FC1pr(0) = 0
			//    ElseIf cmbFC1filterkind.text = "Shepp" Then
			//        FC1pr(0) = 1    'Shepp
			//    Else
			//        FC1pr(0) = 2    'Y=aX2
			//    End If
			//
			//    If cmbFC2filterkind.text = "Laks" Then
			//        FC2pr(0) = 0
			//    ElseIf cmbFC2filterkind.text = "Shepp" Then
			//        FC2pr(0) = 1    'Shepp
			//    Else
			//        FC2pr(0) = 2    'Y=aX2
			//    End If
			//
			//    If cmbFC3filterkind.text = "Laks" Then
			//        FC3pr(0) = 0
			//    ElseIf cmbFC3filterkind.text = "Shepp" Then
			//        FC3pr(0) = 1    'Shepp
			//    Else
			//        FC3pr(0) = 2    'Y=aX2
			//    End If
			//
			//'ｵﾌｾｯﾄｽｷｬﾝ、高解像度ｶﾒﾗ対応追加
			//    If cmbFC4filterkind.text = "Laks" Then
			//        FC4pr(0) = 0
			//    ElseIf cmbFC4filterkind.text = "Shepp" Then
			//        FC4pr(0) = 1    'Shepp
			//    Else
			//        FC4pr(0) = 2    'Y=aX2
			//    End If
			//
			//    If cmbFC5filterkind.text = "Laks" Then
			//        FC5pr(0) = 0
			//    ElseIf cmbFC5filterkind.text = "Shepp" Then
			//        FC5pr(0) = 1    'Shepp
			//    Else
			//        FC5pr(0) = 2    'Y=aX2
			//    End If
			//
			//    If cmbFC6filterkind.text = "Laks" Then
			//        FC6pr(0) = 0
			//    ElseIf cmbFC6filterkind.text = "Shepp" Then
			//        FC6pr(0) = 1    'Shepp
			//    Else
			//        FC6pr(0) = 2    'Y=aX2
			//    End If
			//
			//   'ﾌｨﾙﾀ種類追加
			//    If cmbFC7filterkind.text = "Laks" Then
			//        FC7pr(0) = 0
			//    ElseIf cmbFC7filterkind.text = "Shepp" Then
			//        FC7pr(0) = 1    'Shepp
			//    Else
			//        FC7pr(0) = 2    'Y=aX2
			//    End If
			//
			//    If cmbFC8filterkind.text = "Laks" Then
			//        FC8pr(0) = 0
			//    ElseIf cmbFC7filterkind.text = "Shepp" Then     'cmbFC8filterkind.textの間違いと思われる by 間々田 2003/07/11
			//        FC8pr(0) = 1    'Shepp
			//    Else
			//        FC8pr(0) = 2    'Y=aX2
			//    End If
			//
			//    If cmbFC9filterkind.text = "Laks" Then
			//        FC9pr(0) = 0
			//    ElseIf cmbFC7filterkind.text = "Shepp" Then     'cmbFC9filterkind.textの間違いと思われる by 間々田 2003/07/11
			//        FC9pr(0) = 1    'Shepp
			//    Else
			//        FC9pr(0) = 2    'Y=aX2
			//    End If
			//
			//    'a
			//    FC1a = Val(txtFC1a)
			//    FC2a = Val(txtFC2a)
			//    FC3a = Val(txtFC3a)
			//    FC4a = Val(txtFC4a)
			//    FC5a = Val(txtFC5a)
			//    FC6a = Val(txtFC6a)
			//    FC7a = Val(txtFC7a)
			//    FC8a = Val(txtFC8a)
			//    FC9a = Val(txtFC9a)
			//
			//    'FFTｻｲｽﾞ
			//    FC1pr(1) = Val(cmbFC1fftsize.text)
			//    FC2pr(1) = Val(cmbFC2fftsize.text)
			//    FC3pr(1) = Val(cmbFC3fftsize.text)
			//    'ｵﾌｾｯﾄｽｷｬﾝ、高解像度ｶﾒﾗ対応追加
			//    FC4pr(1) = Val(cmbFC4fftsize.text)
			//    FC5pr(1) = Val(cmbFC5fftsize.text)
			//    FC6pr(1) = Val(cmbFC6fftsize.text)
			//    'ﾌｨﾙﾀ種類追加
			//    FC7pr(1) = Val(cmbFC7fftsize.text)
			//    FC8pr(1) = Val(cmbFC8fftsize.text)
			//    FC9pr(1) = Val(cmbFC9fftsize.text)
			//delete by 間々田 2003/07/11 End

			//append by 間々田 2003/07/11 Start
			int size = 0;
			float a = 0;

            //test!!11/5
            //仮にロードする
            //フォームロード処理
            //frmFCpara_Load(this, EventArgs.Empty);

			//for (int i = 1; i <= 9; i++)
            //v18.00変更 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05 //変更2014/10/07hata_v19.51反映
            //for (int i = Maint.FCRec.GetLowerBound(0); i <= Maint.FCRec.GetUpperBound(0); i++)
            for (int i = 1 ; i <= Maint.FCRec.GetUpperBound(0); i++)
            {
                Maint.FCRec[i].Kind = cmbFCfilterkind[i].SelectedIndex;		//ﾌｨﾙﾀ種類
				int.TryParse(cmbFCfftsize[i].Text, out size);
				Maint.FCRec[i].SIZE = size;									//FFTｻｲｽﾞ
				float.TryParse(txtFCa[i].Text, out a);
				Maint.FCRec[i].a = a;										//a
			}
			//append by 間々田 2003/07/11 End

		}


		//********************************************************************************
		//機    能  ：  キャンセルボタンクリック時の処理
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V1.0   ?          ?              新規作成
		//********************************************************************************
		private void cmdCancel_Click(object sender, EventArgs e)
		{
			//フォームのアンロード
			this.Close();
		}


		//********************************************************************************
		//機    能  ：  ＯＫボタンクリック時の処理
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V1.0   ?          ?              新規作成
		//********************************************************************************
		private void cmdOk_Click(object sender, EventArgs e)
		{
			FCparaset();		//FCパラメータ設定
			FCparawrite();		//FCパラメータﾌｧｲﾙ書き込み
		}


		//********************************************************************************
		//機    能  ：  フォームロード時の処理
		//              変数名           [I/O] 型        内容
		//引    数  ：  なし
		//戻 り 値  ：  なし
		//補    足  ：  なし
		//
		//履    歴  ：  V1.0   ?          ?              新規作成
		//              v7.0   2003/07/11  (SI4)間々田   リソース化対応
		//********************************************************************************
		private void frmFCpara_Load(object sender, EventArgs e)
		{
			//Tagプロパティに保持されたリソースIDに基づいて文字列をロードする
			StringTable.LoadResStrings(this);

			int i = 0;
			//for (i = 1; i <= 9; i++)
            //v18.00変更 FFTサイズ8192対応 2011/03/10 by 間々田 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05 //変更2014/10/07hata_v19.51反映
            //for (i = Maint.FCRec.GetLowerBound(0); i <= Maint.FCRec.GetUpperBound(0); i++)
            for (i = 1 ; i <= Maint.FCRec.GetUpperBound(0); i++)
            {
				fraFC[i].Text = StringTable.GetResString(StringTable.IDS_fraFCpara, i.ToString());		//FCxパラメータ(x=1～9)
			}

			//表示位置：メンテナンス画面の右横
			frmMaint frmMaint = frmMaint.Instance;
			this.Location = new Point(frmMaint.Left + frmMaint.Width, frmMaint.Top);

			//    FCgridset
			FCparaRead();
		}


	}
}
