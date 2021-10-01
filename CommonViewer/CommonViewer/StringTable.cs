using System;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

//
using CTAPI;
using CT30K.Common;

namespace CT30K
{
    ///* *************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver9.7                */
    ///* 客先　　　　： ?????? 殿                                                    */
    ///* プログラム名： StringTable.bas                                              */
    ///* 処理概要　　： ストリングテーブルのＩＤ定数の定義モジュール                 */
    ///* 注意事項　　：                                                              */
    ///* --------------------------------------------------------------------------- */
    ///* ＯＳ　　　　： Windows XP Professional (SP1)                                */
    ///* コンパイラ　： VB 6.0 (SP5)                                                 */
    ///* --------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                  */
    ///*                                                                             */
    ///* V7.00       99/XX/XX    (SI4)間々田         新規作成                        */
    ///* V19.00      12/02/20    H.Nagai             BHC対応                         */
    ///*                                                                             */
    ///* --------------------------------------------------------------------------- */
    ///* ご注意：                                                                    */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。    */
    ///*                                                                             */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2004                  */
    ///* *************************************************************************** */

    
    
    internal static class StringTable
	{
        //ストリングテーブル用の定数

        //付帯情報項目
        public const int IDS_ProductName = 12801;		    //プロダクト名
        public const int IDS_SliceName = 12802;		        //スライス名
        public const int IDS_ScanPos = 12803;		        //スキャン位置
        public const int IDS_ScanDate = 12804;		        //スキャン年月日
        public const int IDS_ScanTime = 12805;		        //スキャン時刻
        public const int IDS_TubeVoltage = 12806;		    //管電圧
        public const int IDS_TubeCurrent = 12807;		    //管電流
        public const int IDS_ViewNum = 12808;		        //ビュー数
        public const int IDS_IntegNum = 12809;		        //積算枚数
        public const int IDS_IIField = 12810;		        //I.I.視野
        public const int IDS_MaxScanArea = 12811;		    //最大ｽｷｬﾝｴﾘｱ
        public const int IDS_SliceWidth = 12812;		    //スライス厚
        public const int IDS_SystemName = 12813;		    //システム名
        public const int IDS_Matrix = 12814;		        //マトリクスサイズ
        public const int IDS_WorkShopName = 12815;		    //事業所名
        public const int IDS_Comment = 12816;		        //コメント
        public const int IDS_ScanMode = 12817;		        //スキャンモード
        public const int IDS_FilterFunc = 12818;		    //フィルタ関数
        public const int IDS_ImageBias = 12819;		        //画像バイアス
        public const int IDS_ImageSlope = 12820;		    //画像スロープ
        public const int IDS_ConeBeam = 12821;		        //コーンビーム
        public const int IDS_ImageDirection = 12822;		//断面像方向
        //v17.30 FDDに統一する by 長野 2010-09-26
        //Public Const IDS_FID                    As Long = 12823     'FID
        public const int IDS_FID = 12823;		            //FID
        public const int IDS_FCD = 12824;		            //FCD
        public const int IDS_WindowLevel = 12825;		    //ウィンドウレベル
        public const int IDS_WindowWidth = 12826;		    //ウィンドウ幅
        public const int IDS_PixelSize = 12827;		        //1画素サイズ(mm)
        public const int IDS_Magnification = 12828;		    //拡大倍率
        public const int IDS_Scale = 12829;		            //スケール(mm)
        public const int IDS_DataAcqTime = 12830;		    //ﾃﾞｰﾀ収集時間(秒)
        public const int IDS_ReconTime = 12831;		        //再構成時間(秒)
        public const int IDS_RFC = 12832;		            //RFC
        public const int IDS_FilterProc = 12833;		    //フィルタ処理
        public const int IDS_Gamma = 12841;		            //ガンマ補正 by長野 v19.00 2012/02/21

        //メカ・スキャン関連用語
        public const int IDS_SliceNumber = 9059;		    //スライス枚数/Slice number
        public const int IDS_DataCollectViews = 9068;		//データ収集ビュー数
        public const int IDS_ScanArea = 12029;		        //スキャンエリア
        public const int IDS_SliceNum = 12035;		        //スライス数
        public const int IDS_Vias = 12037;		            //バイアス
        public const int IDS_Slope = 12038;		            //スロープ
        public const int IDS_SlicePitch = 12069;		    //スライスピッチ
        public const int IDS_Helical = 12078;		        //ヘリカル
        public const int IDS_NonHelical = 12079;		    //非ヘリカル
        public const int IDS_Date = 12094;		            //年月日

        public const int IDS_Pitch = 12197;		            //ピッチ
        public const int IDS_Slice = 10822;		            //%1スライス
        public const int IDS_ComInit = 12133;			    //コモン初期化
        public const int IDS_Collimator = 12157;		    //コリメータ
        public const int IDS_SliceLight = 12158;		    //スライスライト
        public const int IDS_Tilt = 12159;		            //チルト
        public const int IDS_Phantom = 12162;		        //ファントム
        public const int IDS_Filter = 12163;		        //フィルタ
        
        //追加2014/10/07hata_v19.51反映
        public const int IDS_Focus = 12164;                 //焦点       'v18.00追加 byやまおか 2011/03/11 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここまで
        
        public const int IDS_ScanAndView = 12210;		    //スキャン中再構成
        public const int IDS_ReconMask = 12211;		        //再構成形状
        public const int IDS_XrayTube = 12218;		        //Ｘ線管
        public const int IDS_AutoCentering = 12221;		    //オートセンタリング
        public const int IDS_AutoPrint = 12222;		        //オートプリント
        public const int IDS_ScanCondition = 12224;		    //スキャン条件
        public const int IDS_DataMode = 9014;		        //データモード
        public const int IDS_MultiScanMode = 12232;		    //マルチスキャンモード
        public const int IDS_MultiSlice = 12233;		    //マルチスライス
        public const int IDS_ContrastFitting = 12237;		//画像階調最適化
        public const int IDS_ImageIntegNum = 12238;		    //画像積算枚数
        public const int IDS_ImageRotAngle = 12251;		    //画像回転角度
        public const int IDS_Xray = 12298;		            //Ｘ線
        public const int IDS_Table = 12334;		            //テーブル

        //追加2014/10/07hata_v19.51反映
        public const int IDS_XrayDetector = 12344;          //X線・検出器

        public const int IDS_OverScan = 12326;		        //オーバースキャン
        public const int IDS_SlicePos = 12605;		        //スライス位置
        public const int IDS_SendMail = 13006;		        //メール送信
        public const int IDS_AutoZooming = 12219;		    //オートズーミング

        public const int IDS_FTable = 12122;		        //微調テーブル
        public const int IDS_II = 12333;		            //I.I.
        public const int IDS_SampleTable = 12335;		    //試料テーブル
        public const int IDS_MoveErr = 9953;		        //指定された%1位置まで%2を移動させることができませんでした。
        public const int IDS_TableAxis = 10175;		        //テーブル%1
        public const int IDS_Details = 10176;		        //%1－詳細

        public const int IDS_MAX = 12021;		            //最大

        public const int IDS_Value = 12099;		            //値
        public const int IDS_RawData = 12100;		        //生データ
        public const int IDS_RawDataName = 12101;		    //生データ名
        public const int IDS_ParameterName = 12395;		    //パラメータ名
        public const int IDS_TableName = 12225;		        //テーブル名
        public const int IDS_DirName = 12226;		        //ディレクトリ名
        public const int IDS_FileName = 12227;		        //ファイル名
        public const int IDS_DirNameOf = 12228;		        //%1のディレクトリ名
        public const int IDS_FileNameOf = 12229;		    //%1のファイル名
        public const int IDS_TableNameOf = 12230;		    //%1のテーブル名

        public const int IDS_msgZoomingError1 = 9359;		//ROIが1個も描画されていないので、ズーミングを実行できません。
        public const int IDS_msgZoomingError2 = 9398;		//コーンビーム画像でズーミングを行う場合は、ROIを1個にしてください。
        public const int IDS_msgZoomingError4 = 9527;		//生データが一式ありません。
        public const int IDS_msgZoomingError5 = 9526;		//生データがありません。

        //エラー関連
        public const int IDS_msgFluoroIPOpenErr = 9348;		//BMP､JPG､TIF以外の画像を開くことはできません｡
        public const int IDS_msgCaptureBoardErr = 9430;		//ビデオキャプチャボードエラー
        public const int IDS_msgCaptureErr = 9431;		    //ビデオ信号取込みエラー
        public const int IDS_UnkownError = 9900;		    //予想外のエラーが発生しました。
        public const int IDS_ErrorNum = 9901;		        //エラー番号:
        public const int IDS_CompletedNormally = 9908;		//%1が正常に終了しました。
        public const int IDS_WentWrong = 9909;		        //%1に失敗しました。
        public const int IDS_Saved = 9910;		            //%1を保存しました。
        public const int IDS_QuerySave = 9911;		        //%1を保存しますか？
        public const int IDS_NotFound = 9913;		        //%1が見つかりません。
        public const int IDS_NotSpecified = 9914;		    //%1が指定されていません。
        public const int IDS_Interrupted = 9916;		    //%1を中止します。
        public const int IDS_DoSelectImages = 9947;		    //対象画像が２枚以上指定されていません。
        public const int IDS_NotFoundROI = 9974;		    //ROIが設定されていません。
        public const int IDS_msgQueryEndCT30K = 9999;		//CT30K中止の問い合わせメッセージＩＤ

        //ボタン関係
        public const int IDS_btnOK = 10001;		            //ＯＫ
        public const int IDS_btnCancel = 10002;		        //キャンセル
        public const int IDS_btnYes = 10003;		        //はい
        public const int IDS_btnNo = 10004;		            //いいえ
        public const int IDS_btnExe = 10005;		        //実　行
        public const int IDS_btnEnd = 10006;		        //終　了
        public const int IDS_btnOpen = 10007;		        //開く
        public const int IDS_btnClose = 10008;		        //閉じる
        public const int IDS_btnRef = 10009;		        //参照
        public const int IDS_btnDel = 10010;		        //削　除
        public const int IDS_btnDisp = 10011;		        //表　示
        public const int IDS_btnSave = 10012;		        //保　存
        public const int IDS_btnSave2 = 10013;		        //保存
        public const int IDS_btnRegister = 10014;		    //登録
        public const int IDS_btnChange = 10015;		        //変更
        public const int IDS_btnUpdate = 10016;		        //更　新
        public const int IDS_btnUndo = 10017;		        //元に戻す
        public const int IDS_btnStart = 10018;		        //開始
        public const int IDS_btnStop = 10019;		        //停止
        public const int IDS_btnNext = 10020;		        //次へ＞
        public const int IDS_btnBack = 10021;		        //＜戻る
        public const int IDS_btnReset = 10022;		        //リセット
        public const int IDS_btnAllReset = 10023;		    //オールリセット
        public const int IDS_btnSetting = 10024;		    //設定
        public const int IDS_btnManual = 10030;		        //手動
        public const int IDS_btnSingle = 10031;		        //単独
        public const int IDS_btnAll = 10032;		        //一括
        public const int IDS_btnDetails = 10033;		    //詳細
        public const int IDS_btnScanStart = 10041;		    //スキャンスタート
        public const int IDS_btnScanStop = 10042;		    //スキャンストップ

        public const int IDS_btnEnlarge = 12583;		    //拡大
        public const int IDS_btnReduction = 12582;		    //縮小
        
        //追加2014/10/07hata_v19.51反映
        public const int IDA_btnResize = 12584;             //サイズ変更 'v17.4X/v18.00追加 byやまおか 2011/04/28 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07 ここまで
       
        public const int IDS_Open = 10101;			        //%1を開く
        public const int IDS_Save = 10102;		            //%1の保存
        public const int IDS_Select = 10103;		        //%1の選択
        public const int IDS_FileSpecify = 10104;		    //%1の指定
        public const int IDS_DoSave = 10105;		        //%1を保存する
        public const int IDS_Reading = 10111;		        //%1の読み込み
        public const int IDS_Writing = 10112;		        //%1の書き込み

        public const int IDS_SelectFolder = 10122;		    //%1を行うフォルダを指定

        public const int IDS_Copy = 10131;		            //%1のコピー
        public const int IDS_Paste = 10132;		            //%1の貼り付け
        public const int IDS_Cut = 10133;		            //%1の切り取り

        public const int IDS_Exe = 10141;		            //%1実行
        public const int IDS_Result = 10142;		        //%1結果
        public const int IDS_Conditions = 10143;		    //%1条件
        public const int IDS_Ing = 10144;		            //%1中
        public const int IDS_Processing = 10145;		    //%1処理
        public const int IDS_PowerSupply = 10147;		    //%1電源

        public const int IDS_Input = 10151;		            //%1入力
        public const int IDS_CoordinateInput = 10152;		//%1座標入力
        public const int IDS_InputCommentOf = 10153;		//%1のコメント入力
        public const int IDS_BinaryImage = 10154;		    //～２値化画像
        public const int IDS_Range = 10155;		            //(%1～%2)
        public const int IDS_RangeMM = 10156;		        //(%1～%2mm)
        public const int IDS_MaxMM = 10157;		            //最大 %1mm
        public const int IDS_Frames = 10158;		        //%1 枚

        public const int IDS_Rotate = 10160;		        //%1回転
        public const int IDS_RotateCW = 10161;		        //%1右回転
        public const int IDS_RotateCCW = 10162;		        //%1左回転
        public const int IDS_Move = 10163;		            //%1移動
        public const int IDS_MoveR = 10164;		            //%1右移動
        public const int IDS_MoveL = 10165;		            //%1左移動
        public const int IDS_UpDown = 10166;		        //%1昇降
        public const int IDS_Up = 10167;		            //%1上昇
        public const int IDS_Down = 10168;		            //%1下降
        public const int IDS_Forward = 10169;		        //%1前進
        public const int IDS_Back = 10170;		            //%1後退
        public const int IDS_FWDEnlarge = 10171;		    //%1前進（拡大）
        public const int IDS_BCKReduction = 10172;		    //%1後退（縮小）
        public const int IDS_SpeedOf = 10173;		        //%1速度
        public const int IDS_MoveSpeed = 10174;		        //%1移動速度
        public const int IDS_Height = 12170;		        //高さ
        public const int IDS_Moving = 10177;		        //%1移動中
        public const int IDS_NotReady = 10178;		        //%1未完
        public const int IDS_Failed = 10179;		        //%1失敗

        public const int IDS_FilerAll = 10300;		        //すべてのファイル(*.*)|*.*

        //ファイル関係
        public const int IDS_CTImage = 10301;		        //画像ファイル
        public const int IDS_InfoFile = 10302;		        //付帯情報ファイル
        public const int IDS_RawFile = 10303;		        //生データファイル
        public const int IDS_SlicePlanTable = 10304;		//スライスプランテーブル
        public const int IDS_ZoomingTable = 10305;		    //ズーミングテーブル
        public const int IDS_ROITable = 10306;		        //ROIテーブル
        public const int IDS_PDTable = 10307;		        //プロフィールディスタンステーブル
        public const int IDS_ConeRawFile = 10308;		    //コーンビームＣＴ用生データファイル
        public const int IDS_FilerFIMG = 10309;		        //BMP(*.BMP)|*.BMP|JPG(*.JPG)|*.JPG|TIF(*.TIF)|*.TIF
        public const int IDS_CondFile = 10310;		        //スキャン条件ファイル
        public const int IDS_AutoZoomFile = 10313;		    //オートズームファイル

        public const int IDS_Single = 10500;		        //シングル
        public const int IDS_Multi = 10501;		            //マルチ
        public const int IDS_SlicePlan = 10504;		        //スライスプラン
        public const int IDS_ScanCorrect = 10509;		    //スキャン校正
        public const int IDS_GradConvert = 10512;		    //階調変換
        public const int IDS_FormatConvert = 10518;		    //画像フォーマット変換
        public const int IDS_Zooming = 10514;		        //ズーミング
        public const int IDS_Reconst = 10515;		        //再構成リトライ
        public const int IDS_PostConeRetry = 10516;		    //コーン後再構成
        public const int IDS_TransImage = 10517;		    //透視画像

        //単位・記号関係
        public const int IDS_Xtheta = 10802;		        //Xθ
        public const int IDS_Ytheta = 10803;		        //Yθ
        public const int IDS_Colon = 10808;		            //：
        public const int IDS_Frame = 10818;		            //枚
        public const int IDS_FramesWithMax = 10819;		    //枚 (最大%1枚)
        public const int IDS_Pixels = 10820;		        //画素

        //ROI
        public const int IDS_RoiTrace = 12000;		        //トレース
        public const int IDS_RoiCircle = 12001;		        //円
        public const int IDS_RoiCircle2 = 12002;		    //円形
        public const int IDS_RoiRectangle = 12003;		    //矩形
        public const int IDS_SlantingLine = 12004;		    //斜線
        public const int IDS_VerticalLine = 12005;		    //垂直線
        public const int IDS_Horizon = 12006;		        //水平線
        public const int IDS_RoiSquare = 12007;		        //正方形
        public const int IDS_RoiRect = 12008;		        //長方形
        public const int IDS_Radius = 12009;		        //半径
        public const int IDS_LineSegment = 12010;		    //線分
        public const int IDS_Point = 12011;		            //点

        //印刷
        public const int IDS_lblNowPrinting = 12200;		//印刷中...(出力先：%1)
        public const int IDS_lblNowPrinter = 12382;		    //%1 に書込み中

        //CT30K共通
        //追加2014/10/07hata_v19.51反映
        public const int IDS_TosMicroCT = 17000;	        //東芝 マイクロＣＴ
        public const int IDS_TosIndustrialCT = 17001;       //東芝 産業用ＣＴ

        public const int IDS_ImageInfo = 12576;		        //付帯情報
        public const int IDS_None = 12020;		            //なし
        public const int IDS_ON = 12019;			        //あり
        public const int IDS_OFF = 12020;		            //なし
        public const int IDS_Scan = 12028;		            //スキャン
        public const int IDS_Status = 12033;		        //ステータス

        public const int IDS_StrNumLimit = 12342;		    //%1文字以内 (全角：２文字、半角１文字）
        public const int IDS_ImageProcessing = 12450;		//画像処理

        public const int IDS_Number = 12616;		        //番号
        public const int IDS_RoiNo = 12608;		            //ROI番号
        public const int IDS_RoiShape = 12606;		        //ROI形状
        public const int IDS_RoiCount = 12615;		        //ROI個数
        public const int IDS_RoiCoordinate = 12614;		    //ROI座標
        public const int IDS_LineCount = 12625;		        //線分数
        public const int IDS_OneScale = 12401;		        //１目盛
        public const int IDS_Median = 12403;		        //中央値
        public const int IDS_OnePointLower = 12627;		    //１点指定下限閾値
        public const int IDS_OnePointUpper = 12628;		    //１点指定上限閾値
        public const int IDS_PointCount = 12626;		    //指定点数
        public const int IDS_Length = 12624;		        //長さ(mm)

        public const int IDS_lblInputXL = 12609;		    //X方向の大きさ XL
        public const int IDS_lblInputYL = 12610;		    //Y方向の大きさ YL
        public const int IDS_lblInputRo = 12611;		    //大きさ     Ro
        public const int IDS_InputRoiOut = 9573;		    //描画範囲に入りません。
        public const int IDS_InputRoiInvalidX = 9380;		//X方向の大きさが不正です
        public const int IDS_InputRoiInvalidY = 9382;		//Y方向の大きさが不正です

        public const int IDS_Left = 12407;		            //左
        public const int IDS_Right = 12408;		            //右

        public const int IDS_FImageInfo0 = 12593;		    //ＭＳ ゴシック

        public const int IDS_LiveImage = 12595;		        //ライブ画像

        //画像処理
        public const int IDS_AddImage = 12458;		        //和画像
        public const int IDS_SubImage = 12459;		        //差画像
        public const int IDS_EnlargeSimple = 12460;		    //単純拡大
        public const int IDS_SizeMeasurement = 12461;		//寸法測定
        public const int IDS_PseudoColor = 12451;		    //疑似カラー
        public const int IDS_MultiFrame = 12457;		    //マルチフレーム
        public const int IDS_EditImageInfo = 12462;		    //付帯情報修正
        public const int IDS_ProfileDistance = 12434;		//プロフィールディスタンス
        public const int IDS_CTNumberDisp = 12452;		    //ＣＴ値表示
        public const int IDS_BoneAnalysis = 12453;		    //骨塩定量解析
        public const int IDS_ROIProcessing = 12454;		    //ROI処理
        public const int IDS_Histogram = 12455;		        //ヒストグラム
        public const int IDS_Profile = 12456;		        //プロフィール
        public const int IDS_AngleMeasurement = 12463;		//角度測定(非表示)   'v16.01/v17.00追加 byやまおか 2010/02/24

        public const int IDS_Enlarge = 12583;		        //拡大
        public const int IDS_Return = 12097;		        //戻る

        public const int IDS_Quality = 12039;		        //画質
        public const int IDS_Standard = 12040;		        //標準
        public const int IDS_HighSpeed = 12041;		        //高速
        public const int IDS_HighQuality = 12042;		    //精細

        public const int IDS_Connection = 12328;		    //接続

        public const int IDS_Direction = 12214;		        //画像方向
        public const int IDS_DirectionTop = 12103;		    //上から見た画像
        public const int IDS_DirectionBottom = 12102;		//下から見た画像

        public const int IDS_Name = 12045;		            //名称
        public const int IDS_XCenter = 12602;		        //Ｘセンター
        public const int IDS_YCenter = 12603;		        //Ｙセンター
        public const int IDS_RoiSize = 12601;		        //ROIサイズ
        public const int IDS_ZoomingCount = 12604;		    //ｽﾞｰﾐﾝｸﾞ個数

        public const int IDS_fraFCpara = 12523;		        //FC%1パラメータ

        public const int IDS_ProfileDistance2 = 12449;		//ﾌﾟﾛﾌｨｰﾙ&&ﾃﾞｨｽﾀﾝｽ

        public const int IDS_MaxThreshold = 12506;		    //上限閾値
        public const int IDS_MinThreshold = 12505;		    //下限閾値
        public const int IDS_ConnectionWidth = 12507;		//連結幅

        public const int IDS_FCDAddValue = 12387;		    //FCD加算値(mm)
        public const int IDS_FIDAddValue = 12388;		    //FID加算値(mm)

        //メンテナンス画面
        public const int IDS_Maintenance = 12700;		    //メンテナンス
        public const int IDS_FilterSetup = 12701;		    //フィルタ関数設定
        public const int IDS_Parameter = 12703;		        //パラメータ
        public const int IDS_FilterTypeError = 12705;		//フィルタ種類エラー
        public const int IDS_YAxisAngleMeas = 12721;		//%1傾斜角度測定     v14.24変更 by 間々田 2009/03/10 Y軸傾斜角度測定→%1傾斜角度測定
        public const int IDS_YAxisAngle = 12722;		    //%1傾斜角度         v14.24変更 by 間々田 2009/03/10 Ｙ軸傾斜角度→%1傾斜角度
        //public const int IDS_Offset = 12723;		        //%1オフセット       v14.24変更 by 間々田 2009/03/10 Ｘオフセット→%1オフセット
        public const int IDS_AxisOffset = 12723;            //%1オフセット       'v18.00変更 IDS_Offset→IDS_AxisOffset byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        
        public const int IDS_RefFid = 12724;		        //基準fid

        //v7.0 フラットパネル対応
        public const int IDS_FlatPanel = 12731;		        //フラットパネル
        public const int IDS_BinningMode = 12733;		    //ビニングモード
        public const int IDS_DefImage = 12734;		        //欠陥画像
        public const int IDS_Binning = 12737;		        //ビニング

        public const int IDS_FileSizeError = 12745;		    //

        public const int IDS_AlignmentNow = 16201;		    //調整中
        public const int IDS_AlignmentFailure = 16202;		//調整失敗
        public const int IDS_AlignmentCheck = 16203;		//チェック

        //校正関連
        public const int IDS_Correction = 10900;		    //校正
        public const int IDS_CorScanPos = 10901;		    //スキャン位置校正
        public const int IDS_CorDistortion = 10902;		    //幾何歪校正
        public const int IDS_CorRot = 10903;		        //回転中心校正
        public const int IDS_CorGain = 10904;		        //ゲイン校正
        public const int IDS_CorOffset = 10905;		        //オフセット校正
        public const int IDS_CorSize = 10906;		        //寸法校正
        public const int IDS_CorMulti = 10907;		        //マルチスライス校正
        public const int IDS_CorAuto = 10908;		        //全自動校正

        //v14.24追加 by 間々田 2009/03/10 なおリソース12160の中身も Ｘ軸→%1軸に変更
        public const int IDS_Axis = 12160;		            //%1軸

        //v14.24追加 by 間々田 2009/03/10 なおリソース12325はv14.24にて新規追加
        public const int IDS_XrayMove = 12325;		        //Ｘ線管%1移動

        public const int IDS_ArtifactReduction = 15001;		//アーティファクト低減

        public const int IDS_DoubleOblique = 17479;		    //ダブルオブリーク
        public const int IDS_Transmission = 12208;		    //透視
        public const int IDS_Reconstruction = 12212;		//再々構成
        public const int IDS_Preset = 15207;		        //プリセット
        public const int IDS_Destination = 12240;		    //画像保存先
        public const int IDS_CorStatus = 12092;		        //校正ステータス
        public const int IDS_Contrast = 12205;		        //コントラスト
        public const int IDS_TimesFactor = 12749;		    //倍率
        public const int IDS_Times = 10829;		            //%1 倍

        public const int IDS_TurnOn = 9955;		            //%1をオンしてください。
        public const int IDS_TurnOff = 9956;		        //%1をオフしてください。

        public const int IDS_ClickOK = 9905;		        //よろしければＯＫをクリックしてください。
        public const int IDS_ClickOKIfReady = 9906;		    //準備ができたらＯＫをクリックしてください。
        public const int IDS_CorNotReady = 9958;		    //～が準備完了でないため、処理を中止します。
                                                            //事前に～を実施してください。
        public const int IDS_CorReadyAlready = 9959;		//実行しようとする校正がすべて完了しているので、処理を中止します。
        public const int IDS_ErrCorAuto = 9960;		        //%1に失敗しましたので、全自動校正を中止します。
                                                            //%1は手動で行ってください。

        public const int IDS_Retry = 9938;		            //%1を再度行ってください。
        public const int IDS_ErrCalPara = 9939;		        //%1パラメータ計算に失敗しました。

        public const int IDS_DontPutOnTable = 12058;		//試料テーブルに何も載せないでください。

        public const int IDS_QueryResultSave = 12108;		//結果を保存しますか？

        public const int IDS_Warmup = 10524;		        //ウォームアップ
        public const int IDS_XrayInfo = 14100;		        //Ｘ線情報

        //v15.03追加 byやまおか 2009/11/17
        public const int IDS_CannotDo = 9617;		        //%1が%2のため、%3できません。
        public const int IDS_MagLock = 9618;		        //電磁ロック
        public const int IDS_Door = 9619;		            //扉
        public const int IDS_XrayON = 9620;		            //Ｘ線オン
        public const int IDS_GoOnProcess = 9621;		    //処理続行
        public const int IDS_StandbyMode = 9622;		    //スタンバイモード
        public const int IDS_WarmupStart = 9623;		    //ウォームアップ開始
        public const int IDS_OpenOnly = 12134;		        //開

        //v16.01 追加 by 山影 2010/02/16
        public const int IDS_CloseOnly = 12135;		        //閉
        public const int IDS_WarmUpNow = 12193;		        //ｳｫｰﾑｱｯﾌﾟ中
        public const int IDS_XrayWarning = 17484;		    //X線警告
        public const int IDS_XrayWarningMessage = 17485;	//X線ON後、%1秒経過しました。 X線ONを継続しますか？
        public const int IDS_ConflictScanInhibit = 17486;	//%1と%2がともに有効なため、CT30Kを起動できません。
        //どちらかを無効にするように設定ファイル scaninhibit.csv を修正した後、CT30Kを再起動してください。
        public const int IDS_XrayRotate = 17487;		    //X線管回転
        public const int IDS_HighSpeedCamera = 17488;		//高速度透視撮影
        public const int IDS_MultiTube = 17489;		        //複数X線管
        public const int IDS_RotateSelect = 17490;		    //回転選択
        public const int IDS_CTIIDrive = 17491;		        //CT用I.I.切替中
        public const int IDS_TVIIDrive = 17492;		        //高速用I.I.切替中
        public const int IDS_CTIIPos = 17493;		        //CTI.I.位置
        public const int IDS_TVIIPos = 17494;		        //高速用I.I.位置
        public const int IDS_UnknownIIPos = 17495;		    //I.I.位置不定
        public const int IDS_IIOkMove = 17496;		        //I.I.切替可否
        public const int IDS_CTmode = 17497;		        //CT
        public const int IDS_HSCmode = 17498;		        //高速撮影
        public const int IDS_Iris = 17499;		            //X線I.I.絞り

        public const int IDS_XrayStopMessage = 17500;		//X線を強制停止しました
        //追加2014/10/07hata_v19.51反映
        public const int IDS_DetChange = 17508;             //検出器切替     'v18.00追加 byやまおか 2011/02/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        public const int IDS_DetShift = 17509;              //検出器シフト   'v18.00追加 byやまおか 2011/02/04 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        public const int IDS_Shift = 17514;                 //シフト         'v18.00追加 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        public const int IDS_Offset = 12312;                //オフセット     'v18.00追加 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        public const int IDS_Full = 17515;                  //フル           'v18.00追加 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        public const int IDS_Half = 17516;                  //ハーフ         'v18.00追加 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        public const int IDS_ScanCollection = 17517;        //スキャン収集   'v18.00追加 byやまおか 2011/03/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
        public const int IDS_ShiftScan = 17518;             //シフトスキャン 'v18.00追加 byやまおか 2011/07/09 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
 
        //ステータス文字列   'V4.0 append by 鈴山 2001/03/14 'change リソース対応 by 2003/07/26 間々田
        public static string GC_STS_IGNORE;		            //対象外
        public static string GC_STS_STOP;		            //停止中
        public static string GC_STS_X_AVAIL;		        //X線アベイラブル待ち
        public static string GC_STS_CAPTURE;		        //データ収集中
        public static string GC_STS_WAITCAPTURE;            //データ収集待ち     'v18.00追加 byやまおか 2011/07/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        public static string GC_STS_CAPT_OK;		        //データ収集完了
        public static string GC_STS_CAPT_NG;		        //データ収集異常終了
        public static string GC_STS_STANDBY_OK;		        //準備完了
        public static string GC_STS_STANDBY_NG;		        //準備未完了
        public static string GC_STS_BUSY;		            //動作中             '新規追加 by 間々田 2003/08/23
        public static string GC_STS_CPU_BUSY;		        //処理中             '新規追加 by 間々田 2003/08/23
        public static string GC_Xray_WarmUp;		        //ｳｫｰﾑｱｯﾌﾟ中         '新規追加 by 間々田 2003/08/23
        public static string GC_Xray_On;		            //Ｘ線ＯＮ中         '新規追加 by 間々田 2003/08/23
        public static string GC_Xray_Error;		            //異常               '新規追加 by 間々田 2003/08/23
        public static string GC_Xray_WarmUp_NG;		        //ｳｫｰﾑｱｯﾌﾟ未完了     '新規追加 by 間々田 2003/08/23
        public static string GC_STS_STANDBY_NG2;		    //準備未完           '新規追加 by 間々田 2003/08/25
        public static string GC_STS_MovementLimit;		    //動作限             '新規追加 by 間々田 2003/08/25
        public static string GC_STS_Scan;		            //動作中/Scanning    '新規追加 by 間々田 2003/09/16
        public static string GC_STS_AutoCentering;		    //ｵｰﾄｾﾝﾀﾘﾝｸﾞあり
        public static string GC_STS_NORMAL_OK;		        //ﾉｰﾏﾙ準備完了
        public static string GC_STS_CONE_OK;		        //ｺｰﾝ準備完了
        public static string GC_STS_WUP_FAILED;		        //ｳｫｰﾑｱｯﾌﾟ失敗
        public static string GC_STS_WUP_NOTREADY;		    //ｳｫｰﾑｱｯﾌﾟ未完
        public static string GC_STS_FLM_FAILED;		        //ﾌｨﾗﾒﾝﾄ調整失敗
        public static string GC_STS_FLM_RUNNING;		    //ﾌｨﾗﾒﾝﾄ調整中
        public static string GC_STS_FLM_NOTREADY;		    //ﾌｨﾗﾒﾝﾄ調整未完
        public static string GC_STS_CNT_RUNNING;		    //ｾﾝﾀﾘﾝｸﾞ中
        public static string GC_STS_CNT_FAILED;		        //ｾﾝﾀﾘﾝｸﾞ失敗        'added by 山本 2006-12-14
        public static string GC_STS_INT;		            //中断               'v12.01追加 by 間々田 2006/12/14
        public static string GC_STS_STOPPED;		        //停止               'v12.01追加 by 間々田 2006/12/14
        public static string GC_STS_VAC_NOTREADY;		    //真空未完           'v12.01追加 by 間々田 2006/12/14
        public static string GC_STS_TABLE_MOVING;		    //テーブル移動中     'v15.0追加 by 間々田 2009/02/09
        public static string GC_STS_PHANTOM_MOVING;		    //ファントム移動中  'v15.0追加 by 間々田 2009/02/09
        public static string GC_STS_COOLANT_ERROR;		    //冷却水異常         'v17.10追加 byやまおか 2010/08/25
        public static string GC_STS_REMOTE;                 //外部制御           'v18.00追加 byやまおか 2011/03/06 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        public static string GC_STS_MANUAL;                 //手動               'v18.00追加 byやまおか 2011/03/06 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        public static string GC_STS_PANEL_ON;               //パネルON           'v18.00追加 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/05
        

        //v19.00 ->（電S2）永井
        //BHC関連定数
        public const int IDS_BHCTable = 21000;
        public const int IDS_BHCTableDir = 21001;		    //CTUSER\BHCﾃｰﾌﾞﾙ
        public const int IDS_BHCFile = 21005;		        //BHCファイル
        public const int IDS_BHCImageDir = 21006;		    //CTUSER\画像

        public const int IDS_Specify = 21100;		        //指定
        public const int IDS_File = 21101;		            //ファイル
        public const int IDS_Folder = 21102;		        //フォルダ
        public const int IDS_SelectedImageNum = 21103;		//入力枚数

        public const int IDS_DoubleStart = 9349;            // 2重起動

        //*******************************************************************************
        //機　　能： パラメータ付きのリソース取得関数
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Id              [I/ ] Long      ストリングテーブルのＩＤ番号
        //           p()             [I/ ] String    置換文字列（可変）
        //戻 り 値：                 [ /O] String    取得文字列
        //
        //補　　足： なし
        //
        //履　　歴： V7.00  XX/XX/XX  (SI4)間々田    新規作成
        //           v15.0  2009/03/18 (SS1)間々田   任意のパラメータ個数に対応
        //*******************************************************************************
        public static string GetResString(int Id, params string[] p)    //TODO object[] → string[]
		{
			string Result = null;

			//基本のリソース文字列
            Result = CTResources.LoadResString(Id);
			
            //%1, %2, %3...の部分を指定した引数に置換する
			int i = 0;
            foreach (string param in p)
            {
                i++;
                Result = Result.Replace("%" + i.ToString(), param);
            }

			//戻り値をセット
			return Result;
		}


        //*******************************************************************************
        //機　　能： パラメータ付きのリソース取得関数
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Id              [I/ ] Long      ストリングテーブルのＩＤ番号
        //           p()             [I/ ] String    置換文字列のＩＤ番号（可変）
        //戻 り 値：                 [ /O] String    取得文字列
        //
        //補　　足： なし
        //
        //履　　歴： V7.00  XX/XX/XX  (SI4)間々田    新規作成
        //           v15.0  2009/03/18 (SS1)間々田   任意のパラメータ個数に対応
        //*******************************************************************************
        public static string BuildResStr(int Id, params int[] p)   //TODO object[] → int[]
		{
			string Result = null;

			//基本のリソース文字列
			Result = CTResources.LoadResString(Id);

            //%1, %2, %3...の部分を指定した引数に置換する
            for (int i = 0; i < p.Length; i++)
            {
                Result = Result.Replace("%" +(i + 1).ToString(), CTResources.LoadResString(p[i]));
            }

			//戻り値をセット
			return Result;
		}


        public static string FormatStr(string expression, params int[] p)  //TODO object[] → int[]
		{
			string Result = null;

			//基本の文字列
			Result = expression;

			//%1, %2, %3...の部分を指定した引数に置換する
            for (int i = 0; i < p.Length; i++)
            {
                Result = Result.Replace("%" + (i + 1).ToString(), p[i].ToString());
            }

			//戻り値をセット
			return Result;
		}


        //*******************************************************************************
        //機　　能： リソースから取得した文字列にコロン（：）を付加する
        //
        //           変数名          [I/O] 型        内容
        //引　　数： Id              [I/ ] Long      ストリングテーブルのＩＤ番号
        //戻 り 値：                 [ /O] String    取得文字列
        //
        //補　　足： なし
        //
        //履　　歴： v15.0  2009/03/18 (SS1)間々田   新規作成
        //*******************************************************************************
		public static string LoadResStringWithColon(int ResID)
		{
            return CTResources.LoadResString(ResID) + CTResources.LoadResString(IDS_Colon);
		}


        //*******************************************************************************
        //機　　能： ステータス文字列の設定
        //
        //           変数名          [I/O] 型        内容
        //引　　数： なし
        //戻 り 値： なし
        //
        //補　　足： この関数は起動時にのみ呼び出される。ここで設定される文字列変数は、設定後不変。
        //
        //履　　歴： V1.00  99/XX/XX   ????????      新規作成
        //*******************************************************************************
		public static void SetConstString()
		{
			//各校正画面のステータス文字列
            GC_STS_IGNORE = CTResources.LoadResString(12398);		            //対象外
            GC_STS_STOP = CTResources.LoadResString(12115);		            //停止中
            GC_STS_X_AVAIL = CTResources.LoadResString(12389);		        //X線アベイラブル待ち
            GC_STS_CAPTURE = CTResources.LoadResString(12394);	            //データ収集中

            //追加2014/10/07hata_v19.51反映
            GC_STS_WAITCAPTURE = CTResources.LoadResString(12391);          //データ収集待ち 'v18.00追加 byやまおか 2011/07/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07

            GC_STS_CAPT_OK = CTResources.LoadResString(12393);	            //データ収集完了
            GC_STS_CAPT_NG = CTResources.LoadResString(12392);	            //データ収集異常終了
            GC_STS_STANDBY_OK = CTResources.LoadResString(12044);		        //準備完了
            GC_STS_STANDBY_NG = CTResources.LoadResString(12310);		        //準備未完了
            GC_STS_BUSY = CTResources.LoadResString(12309);		            //動作中                     '新規追加 by 間々田 2003/08/23
            GC_STS_CPU_BUSY = CTResources.LoadResString(12307);		        //処理中                     '新規追加 by 間々田 2003/08/23
            GC_Xray_WarmUp = CTResources.LoadResString(12193);	            //ｳｫｰﾑｱｯﾌﾟ中                 '新規追加 by 間々田 2003/08/23
            GC_Xray_On = CTResources.LoadResString(12192);	                //Ｘ線ＯＮ中                 '新規追加 by 間々田 2003/08/23
            GC_Xray_Error = CTResources.LoadResString(12308);		            //異常                       '新規追加 by 間々田 2003/08/23
            GC_Xray_WarmUp_NG = CTResources.LoadResString(12194);		        //ｳｫｰﾑｱｯﾌﾟ未完了             '新規追加 by 間々田 2003/08/23
            GC_STS_STANDBY_NG2 = CTResources.LoadResString(12188);	        //準備未完                   '新規追加 by 間々田 2003/08/25
            GC_STS_MovementLimit = CTResources.LoadResString(12189);		    //動作限                     '新規追加 by 間々田 2003/08/25
            GC_STS_Scan = CTResources.LoadResString(12288);		            //動作中（英語はScanning）   '新規追加 by 間々田 2003/09/16
            GC_STS_AutoCentering = CTResources.LoadResString(12173);		    //ｵｰﾄｾﾝﾀﾘﾝｸﾞあり v11.2追加 by 間々田 2006/01/13
            GC_STS_NORMAL_OK = CTResources.LoadResString(12174);		        //ﾉｰﾏﾙ準備完了   v11.2追加 by 間々田 2006/01/17
            GC_STS_CONE_OK = CTResources.LoadResString(12175);	            //ｺｰﾝ準備完了    v11.2追加 by 間々田 2006/01/17

            GC_STS_WUP_FAILED = BuildResStr(IDS_Failed, 12156);		        //ｳｫｰﾑｱｯﾌﾟ失敗       v11.5追加 by 間々田 2006/06/14
            GC_STS_FLM_FAILED = BuildResStr(IDS_Failed, 16111);		        //ﾌｨﾗﾒﾝﾄ調整失敗     v11.5追加 by 間々田 2006/06/14
            GC_STS_FLM_RUNNING = BuildResStr(IDS_Ing, 16111);		        //ﾌｨﾗﾒﾝﾄ調整中...    v11.5追加 by 間々田 2006/06/14
            GC_STS_CNT_RUNNING = BuildResStr(IDS_Ing, 16112);		        //ｾﾝﾀﾘﾝｸﾞ中...       v11.5追加 by 間々田 2006/06/14
            GC_STS_CNT_FAILED = BuildResStr(IDS_Failed, 16112);		        //ｾﾝﾀﾘﾝｸﾞ失敗        added by 山本 2006-12-14
            GC_STS_FLM_NOTREADY = BuildResStr(IDS_NotReady, 16111);		    //ﾌｨﾗﾒﾝﾄ調整未完     v11.5追加 by 間々田 2006/06/14
            GC_STS_WUP_NOTREADY = BuildResStr(IDS_NotReady, 12156);		    //ｳｫｰﾑｱｯﾌﾟ未完       v11.5追加 by 間々田 2006/06/14

            GC_STS_INT = CTResources.LoadResString(14213);	                //中断               v12.01追加 by 間々田 2006/12/14
            GC_STS_STOPPED = CTResources.LoadResString(IDS_btnStop);          //停止               v12.01追加 by 間々田 2006/12/14
            GC_STS_VAC_NOTREADY = BuildResStr(IDS_NotReady, 14002);		    //真空未完           v12.01追加 by 間々田 2006/12/14
            GC_STS_TABLE_MOVING = BuildResStr(IDS_Moving, IDS_Table);		//テーブル移動中     v15.0追加 by 間々田 2009/02/09
            GC_STS_PHANTOM_MOVING = BuildResStr(IDS_Moving, IDS_Phantom);   //ファントム移動中   v15.0追加 by 間々田 2009/02/09
            GC_STS_COOLANT_ERROR = CTResources.LoadResString(14215);		//冷却水異常         v17.10追加 byやまおか 2010/08/25

            //追加2014/10/07hata_v19.51反映
            GC_STS_REMOTE = CTResources.LoadResString(16230);               //外部制御           v18.00追加 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            GC_STS_MANUAL = CTResources.LoadResString(16213);               //手動               v18.00追加 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            GC_STS_PANEL_ON = CTResources.LoadResString(16231);             //パネルON           v18.00追加 byやまおか 2011/03/21 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07


			//管電流の単位   'v11.4追加 by 間々田 2006/03/02
            //変更2014/10/07hata_v19.51反映
            //modXrayControl.CurrentUni = (modXrayControl.XrayType == modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150? "mA" : CTResources.LoadResString(10815));   //μA
            //switch (modXrayControl.XrayType)
            //{
            //    //v18.00変更 byやまおか 2011/02/03 'v19.50 v19.41とv18.02の統合 by長野 2013/11/07
            //    case modXrayControl.XrayTypeConstants.XrayTypeToshibaEXM2_150:
            //    case modXrayControl.XrayTypeConstants.XrayTypeGeTitan:
            //        modXrayControl.CurrentUni = "mA";
            //        break;
            //    default:
            //        modXrayControl.CurrentUni = CTResources.LoadResString(10815);
            //        break;
            //}
        
        }


        // このプロシージャはコントロールの Tag プロパティに保持されたリソース ID に基づいて
        // フォーム上のコントロールに関連付けされたリソース文字列をロードします。
        //
        // ﾘｿｰｽ文字列は各ｵﾌﾞｼﾞｪｸﾄの次のﾌﾟﾛﾊﾟﾃｨにﾛｰﾄﾞされます:
        // ｵﾌﾞｼﾞｪｸﾄ    ﾌﾟﾛﾊﾟﾃｨ
        // Form        Caption
        // Menu        Caption
        // TabStrip    Caption, ToolTipText
        // Toolbar     ToolTipText
        // ListView    ColumnHeader.Text

		public static void LoadResStrings(Control control)
		{
            //string[] Cell = null;
            //int i = 0;

            int _resourceNo = 0;

            try
            {
			    //フォームのキャプションを設定します。
			    if (int.TryParse(Convert.ToString(control.Tag), out _resourceNo))
                {
                    ////削除2014/10/31hata
                    ////control.Text = CTResources.LoadResString(_resourceNo);
                    
                    ////次のコントロールはCapitonプロパティに入れる
                    //if (control is CTStatus)
                    //{
                    //    CTStatus _CTStatus = (CTStatus)control;
                    //    _CTStatus.Caption = CTResources.LoadResString(_resourceNo);
                    //}
                    //else if (control is CTButton)
                    //{
                    //    CTButton _CTButton = (CTButton)control;
                    //    _CTButton.Caption = CTResources.LoadResString(_resourceNo);
                    //}
                    ////追加2014/10/31hata --- ここから ---
                    //else if (control is NumTextBox)
                    //{
                    //    NumTextBox _NumTextBox = (NumTextBox)control;
                    //    _NumTextBox.Caption = CTResources.LoadResString(_resourceNo);
                    //}
                    //else if (control is CTLabel)
                    //{
                    //    CTLabel _CTLabel = (CTLabel)control;
                    //    _CTLabel.Caption = CTResources.LoadResString(_resourceNo);
                    //}
                    //else if (control is CWButton)
                    //{
                    //    CWButton _CWButton = (CWButton)control;
                    //    _CWButton.Caption = CTResources.LoadResString(_resourceNo);
                    //}
                    //else
                    //{
                    //    control.Text = CTResources.LoadResString(_resourceNo);
                    //}
                    ////追加2014/10/31hata --- ここまで ---
                }
            }
            catch
            {
                // Nothing
            }

			try
			{
				PropertyInfo info = control.GetType().GetProperty("ToolTipText");

				if (info != null)
				{
					string s = info.GetValue(control, null) as string;

					//ツールヒントを調べます。
					if (int.TryParse(s, out _resourceNo))
					{           
						string res = CTResources.LoadResString(_resourceNo);

						info.SetValue(control, res, null);
					}
				}
			}
			catch
			{
			}

			//メニュー項目の Captionプロパティおよびその他の
			//すべてのコントロールの Tagプロパティを使用して
			//コントロールのキャプションを設定します。
			foreach (Control ctl in control.Controls) 
            {
                try
                {
                    switch (ctl.GetType().Name) //TODO Formのタイプ
                    {
                        case "Menu":
                            if (int.TryParse(ctl.Text, out _resourceNo))
                            {
                                ctl.Text = CTResources.LoadResString(_resourceNo);
                            }
                            break;

                        case "TabControl":

							TabControl tab = (TabControl)ctl;

                            foreach (TabPage page in tab.TabPages)
                            {
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
								Err.Clear
								If IsNumeric(obj.tag) Then
									obj.Caption = LoadResString(CInt(obj.tag))
								End If
                    
								'ツールヒントを調べます。
								If IsNumeric(obj.ToolTipText) Then
									If Err = 0 Then
										obj.ToolTipText = LoadResString(CInt(obj.ToolTipText))
									End If
								End If
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

								LoadResStrings(page);
                            }
                            break;

                        case "ToolStrip":

							ToolStrip tool = (ToolStrip)ctl;

                            foreach (ToolStripItem item in tool.Items)
                            {
                                if (int.TryParse(Convert.ToString(item.Tag), out _resourceNo))
                                {
                                    item.ToolTipText = CTResources.LoadResString(_resourceNo);
                                }
                            }
                            break;

                        case "ListView":

							ListView list = (ListView)ctl;

                            foreach (ColumnHeader header in list.Columns)
                            {
                                if (int.TryParse(Convert.ToString(header.Tag), out _resourceNo))
                                {
                                    header.Text = CTResources.LoadResString(_resourceNo);
                                }
                            }
                            break;

#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
						Case "SSTab"
            
							Cell = Split(ctl.tag, ",")
							For i = LBound(Cell) To UBound(Cell)
								If IsNumeric(Cell(i)) Then
									ctl.TabCaption(i) = LoadResString(CInt(Cell(i)))
								End If
							Next
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

                        default:
#region CT30Kv19.13_64bit 化不要コメントアウト_完全版
/*
							If IsNumeric(ctl.tag) Then
								If Err = 0 Then
									ctl.Caption = LoadResString(CInt(ctl.tag)) & IIf(Right$(ctl.Caption, 3) = "...", "...", "")
								End If
							End If
                
							'ツールヒントを調べます。
							If IsNumeric(ctl.ToolTipText) Then
								If Err = 0 Then
									ctl.ToolTipText = LoadResString(CInt(ctl.ToolTipText))
								End If
							End If
*/
#endregion CT30Kv19.13_64bit 化不要コメントアウト_完全版

							LoadResStrings(ctl);
                            break;
                    }
                }
                catch
                {                    
                    // Nothing
                }                
			}
		}


//v19.00 追加 ->（電S2）永井
        //ver8.30本間 2008/03/04
        //行頭の"・"を追加する
		public static string LoadResStringWithBeginLineLetter(int ResID)
		{
            return CTResources.LoadResString(17542) + CTResources.LoadResString(ResID);
		}
//<- v19.00
	}
}
