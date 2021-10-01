using System;
using System.IO;
//
using CTAPI;
using CT30K.Common;


namespace CT30K
{
	internal static class modReconst
	{
        //*******************************************************************************
        //機　　能： コモンへ条件を設定
        //
        //           変数名          [I/O] 型        内容
        //引　　数： RawDir          [I/ ] String    生データファイルのディレクトリ名
        //           RawName         [I/ ] String    生データファイルのファイル名
        //           IsZooming       [I/ ] Boolean   ズーミングか？
        //
        //戻 り 値： なし
        //
        //補　　足： なし
        //
        //履　　歴： V9.6   04/10/07   (SI4)間々田   リニューアル
        //           v11.4  06/03/15   (SI3)間々田   scansel構造体を使用
        //           v11.5  06/04/24   (WEB)間々田   reconinf構造体を使用
        //           v19.00 12/02/21 　H.Nagai       BHC対応
        //*******************************************************************************
		public static void SetCommonForRetry(string RawBaseName, bool IsZooming = false)
		{
			//スキャン条件の一時保存
			//BackupScansel(); 
            CTSettings.scansel.Backup();
            
            //modScansel.scanselType theScansel = default(modScansel.scanselType);
			//modScansel.GetScansel(ref theScansel);
            ScanSel theScansel = new ScanSel();
            theScansel.Data.Initialize();
            theScansel.Load();

			//reconinf（コモン）の取得   'v11.5追加 by 間々田 2006/04/24
			//modReconinf.GetReconinf(ref modReconinf.reconinf);
            CTSettings.reconinf.Data.Initialize();
            CTSettings.reconinf.Load();

			//ズーミング時
			if (IsZooming) 
            {
				//ズーミングテンポラリファイルのディレクトリ名を設定
				//SetCommonStr "reconinf", "zooming_dir", CtTmpDir
				//SetField CtTmpDir, reconinf.zooming_dir             'v11.5変更 by 間々田 2006/04/24
	            //modLibrary.SetField(modLibrary.AddExtension(modFileIO.FSO.GetParentFolderName(modFileIO.ZOOMTMPCSV), "\\"), ref modReconinf.reconinf.zooming_dir);
                CTSettings.reconinf.Data.zooming_dir.SetString(modLibrary.AddExtension(Path.GetDirectoryName(AppValue.ZOOMTMPCSV), "\\"));
 
				//ズーミングテンポラリファイルのファイル名を設定
				//SetCommonStr "reconinf", "zooming", ZoominTmpName
				//SetField ZoominTmpName, reconinf.zooming            'v11.5変更 by 間々田 2006/04/24
				//modLibrary.SetField(modFileIO.FSO.GetFileName(modFileIO.ZOOMTMPCSV), ref CTSettings.reconinf.Data.zooming);
                CTSettings.reconinf.Data.zooming.SetString(Path.GetFileName(AppValue.ZOOMTMPCSV));
 
				//再構成形状：正方形にする
                CTSettings.gImageinfo.Data.recon_mask = 0;

				//画像回転角度：0度にする
                CTSettings.gImageinfo.Data.recon_start_angle = 0;

				//scanselにスキャンエリアを設定(高周波カットフィルタ用）
				//theScansel.scan_area = GetScanAreaNo(.scan_area)
                //theScansel.scan_area = modLibrary.GetIndexByStr(modLibrary.RemoveNull(new String(modImageInfo.gImageInfo.scan_area)),ref modCommon.MyCtinfdef.scan_area, 2) + 1;	//v11.4変更 by 間々田 2006/03/16
                theScansel.Data.scan_area = modCommon.MyCtinfdef.scan_area.GetIndexByStr(modLibrary.RemoveNull(CTSettings.gImageinfo.Data.scan_area.GetString()), 2) + 1;

            }

            //生データディレクトリ名をコモンに書き込む
			//modLibrary.SetField(modLibrary.AddExtension(modFileIO.FSO.GetParentFolderName(RawBaseName), "\\"), ref modReconinf.reconinf.raw_dir);
            CTSettings.reconinf.Data.raw_dir.SetString(modLibrary.AddExtension(Path.GetDirectoryName(RawBaseName), "\\"));
 
			//生データファイル名をコモンに書き込む
			//modLibrary.SetField(modFileIO.FSO.GetFileName(RawBaseName), ref modReconinf.reconinf.raw_name);
            CTSettings.reconinf.Data.raw_name.SetString(Path.GetFileName(RawBaseName));
            
			//動作ﾓｰﾄﾞ
			//    構造体名：scansel
			//    コモン名：operation_mode
			//    コモン値：1:SCAN 2:SCANO 3:RECON 4:ZOOM 5:GAIN 6:CONEBEAM 7:CONERECON
            theScansel.Data.operation_mode = (IsZooming ? 4 : (CTSettings.gImageinfo.Data.bhc == 1 ? (int)ScanSel.OperationModeConstants.OP_CONERECON : (int)ScanSel.OperationModeConstants.OP_RECON));

			//再構成形状
            theScansel.Data.recon_mask = CTSettings.gImageinfo.Data.recon_mask;

			//画像回転角度
			//theScansel.image_rotate_angle = CLng(.recon_start_angle)
            theScansel.Data.image_rotate_angle = CTSettings.gImageinfo.Data.recon_start_angle;	    //v11.5変更 by 間々田 2006/06/27
            
            //回転中心ch調整量(ch) //Rev23.00 追加 by長野 2015/09/07
            theScansel.Data.numOfAdjCenterCh = CTSettings.gImageinfo.Data.numOfAdjCenterCh;

			//ズーミング有無を設定
			//putcommon_long "reconinf", "zoomflag", IIf(OperationMode = 4, 1, 0)
            CTSettings.reconinf.Data.zoomflag = (IsZooming ? 1 : 0);			//v11.5変更 by 間々田 2006/04/24

			//スキャン条件画面のオートズームフラッグをオフする
			theScansel.Data.auto_zoomflag = 0;

			//画像方向設定
            theScansel.Data.image_direction = CTSettings.gImageinfo.Data.image_direction;

			//バイアス設定
            theScansel.Data.mscan_bias = Convert.ToSingle(CTSettings.gImageinfo.Data.image_bias);

			//スロープ設定
            theScansel.Data.mscan_slope = CTSettings.gImageinfo.Data.image_slope;

			//フィルタ関数設定
			//theScansel.filter = GetFilterNo(Trim$(.fc))
			//theScansel.filter = modLibrary.GetIndexByStr(modLibrary.RemoveNull(new String(modImageInfo.gImageInfo.fc)), ref modCommon.MyCtinfdef.fc) + 1;	//v11.4変更 by 間々田 2006/03/16
            theScansel.Data.filter = modCommon.MyCtinfdef.fc.GetIndexByStr(modLibrary.RemoveNull(CTSettings.gImageinfo.Data.fc.GetString()), 0) + 1;

			//マトリクスサイズ設定
			//theScansel.matrix_size = GetMatrixNo(.matsiz)
			//theScansel.matrix_size = modLibrary.GetIndexByStr(modLibrary.RemoveNull(new String(modImageInfo.gImageInfo.matsiz)), ref modCommon.MyCtinfdef.matsiz, 2);	//v11.4変更 by 間々田 2006/03/16
            theScansel.Data.matrix_size = modCommon.MyCtinfdef.matsiz.GetIndexByStr(modLibrary.RemoveNull(CTSettings.gImageinfo.Data.matsiz.GetString()), 2);	//v11.4変更 by 間々田 2006/03/16


			//フィルタ処理設定                                                                   
			//theScansel.filter_process = modLibrary.GetIndexByStr(modLibrary.RemoveNull(new String(modImageInfo.gImageInfo.filter_process)), ref modCommon.MyCtinfdef.filter_process, 0);'v13.00変更 by Ohkado 2007/04/16
            theScansel.Data.filter_process = modCommon.MyCtinfdef.filter_process.GetIndexByStr(modLibrary.RemoveNull(CTSettings.gImageinfo.Data.filter_process.GetString()), 0);

			//RFC処理可否                                                                        'v14.00追加 by Ohkado 2007/06/25
            theScansel.Data.rfc = CTSettings.gImageinfo.Data.rfc;

			//コーンビームフラグ=1の場合、ｺｰﾝﾋﾞｰﾑCT用再構成ﾘﾄﾗｲ条件をｺﾓﾝに書込む    V3.0 append by 鈴山
            if (CTSettings.gImageinfo.Data.bhc == 1)
            {
                //ヘリカルモード：0(非ﾍﾘｶﾙ),1(ﾍﾘｶﾙ)
                theScansel.Data.inh = CTSettings.gImageinfo.Data.inh;

                //画質：0(標準),1(精細),2(高速)
                theScansel.Data.cone_image_mode = CTSettings.gImageinfo.Data.cone_image_mode;

                //スライス厚(mm)
                theScansel.Data.cone_scan_width = Convert.ToSingle(CTSettings.gImageinfo.Data.width.GetString());

                //スライスピッチ(mm)=軸方向Boxelｻｲｽﾞ(mm)
                theScansel.Data.delta_z = CTSettings.gImageinfo.Data.delta_z;

                //スライス枚数
                theScansel.Data.k = CTSettings.gImageinfo.Data.k;

                //再構成開始位置(mm)
                theScansel.Data.zs = CTSettings.gImageinfo.Data.zs0;

                //再構成終了位置(mm)
                theScansel.Data.ze = CTSettings.gImageinfo.Data.ze0;

                //画面上のｽﾗｲｽ幅(mm)     'v11.5追加 by 間々田 2006/06/28 frmConeRetryConditionから移動
                //2014/11/13hata キャストの修正
                //theScansel.Data.delta_msw = (float)(theScansel.Data.cone_scan_width
                //                            * (CTSettings.gImageinfo.Data.fid / CTSettings.gImageinfo.Data.fcd)
                //                            * (CTSettings.gImageinfo.Data.b1 / 10)
                //                            * (Math.Sqrt(1 + CTSettings.gImageinfo.Data.scan_posi_a * CTSettings.gImageinfo.Data.scan_posi_a) / (CTSettings.gImageinfo.Data.kv == 0 ? 1 : CTSettings.gImageinfo.Data.kv)));
                theScansel.Data.delta_msw = (float)(theScansel.Data.cone_scan_width
                                            * (CTSettings.gImageinfo.Data.fid / CTSettings.gImageinfo.Data.fcd)
                                            * (CTSettings.gImageinfo.Data.b1 / 10)
                                            * (Math.Sqrt(1 + CTSettings.gImageinfo.Data.scan_posi_a * CTSettings.gImageinfo.Data.scan_posi_a) 
                                            / (CTSettings.gImageinfo.Data.kv == 0 ? 1F : (float)CTSettings.gImageinfo.Data.kv)));
            }

			//v19.00 BHC対応 ->(電S2)永井
			//scansel
            theScansel.Data.mbhc_flag = CTSettings.gImageinfo.Data.mbhc_flag;
            theScansel.Data.mbhc_dir = CTSettings.gImageinfo.Data.mbhc_dir;
            theScansel.Data.mbhc_name = CTSettings.gImageinfo.Data.mbhc_name;
          
			//reconinf
            CTSettings.reconinf.Data.mbhc_flag = CTSettings.gImageinfo.Data.mbhc_flag;
            CTSettings.reconinf.Data.mbhc_dir = CTSettings.gImageinfo.Data.mbhc_dir;
            CTSettings.reconinf.Data.mbhc_name = CTSettings.gImageinfo.Data.mbhc_name;
			//<- v19.00

			//19.10 修正 by長野 2012/07/31
            CTSettings.reconinf.Data.mbhc_airLogValue = CTSettings.gImageinfo.Data.mbhc_AirLogValue;

            //Rev26.00 ファントムレスBHC追加 by井上 2017/01/19
            CTSettings.reconinf.Data.mbhc_phantomless = CTSettings.gImageinfo.Data.mbhc_phantomless;
            theScansel.Data.mbhc_phantomless = CTSettings.gImageinfo.Data.mbhc_phantomless;
            theScansel.Data.mbhc_phantomless_c = CTSettings.gImageinfo.Data.mbhc_phantomless_c;
            theScansel.Data.mbhc_phantomless_colli_on = CTSettings.gImageinfo.Data.mbhc_phantomless_colli_on;
            theScansel.Data.mbhc_method = CTSettings.gImageinfo.Data.mbhc_method;

			//scanselの書き込み
			//modScansel.PutScansel(ref theScansel);
            CTSettings.scansel.Put(theScansel.Data);

			//reconinfの書き込み
			//modReconinf.PutReconinf(ref CTSettings.reconinf);	//v11.5追加 by 間々田 2006/04/24
            CTSettings.reconinf.Write();
		}
	}
}
