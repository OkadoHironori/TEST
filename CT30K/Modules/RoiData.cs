using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using Microsoft.VisualBasic.Compatibility;
using Microsoft.VisualBasic.PowerPacks;
//
using CTAPI;
using CT30K.Common;

namespace CT30K
{
   
    //'/* *************************************************************************** */
    //'/* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver9.7                */
    //'/* 客先　　　　： ?????? 殿                                                    */
    //'/* プログラム名： RoiData.cls                                                  */
    //'/* 処理概要　　： ＲＯＩクラスモジュール                                     　*/
    //'/* 注意事項　　：                                                              */
    //'/* --------------------------------------------------------------------------- */
    //'/* ＯＳ　　　　： Windows XP Professional (SP1)                                */
    //'/* コンパイラ　： VB 6.0 (SP5)                                                 */
    //'/* --------------------------------------------------------------------------- */
    //'/* VERSION     DATE        BY                  CHANGE/COMMENT                  */
    //'/*                                                                             */
    //'/* V1.00       99/XX/XX    (TOSFEC) ????????   新規作成                        */
    //'/* V9.7        04/11/01    (SI4)間々田         リニューアル                    */
    //'/* V11.5       06/09/11    (WEB)間々田         不要なコメント文削除            */
    //'/*                                                                             */
    //'/* --------------------------------------------------------------------------- */
    //'/* ご注意：                                                                    */
    //'/* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。    */
    //'/*                                                                             */
    //'/* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2004                  */
    //'/* *************************************************************************** */
    public class RoiData : IDisposable
    {
        #region 定数データ宣言

        //'********************************************************************************
        //'  定数データ宣言
        //'********************************************************************************

        public enum RoiShape :int
        {
            NO_ROI,         //0
            ROI_CIRC,       //1
            ROI_RECT,       //2
            ROI_TRACE,      //3
            ROI_SQR,        //4
            ROI_LINE,       //5     'v9.7追加 by 間々田 2004/11/01
            ROI_POINT,      //6     'v9.7追加 by 間々田 2004/11/01
        }

        //編集用ハンドルの幅
        //private const int HANDLE_WIDTH = 6;
        private const int HANDLE_WIDTH = 4;
        
        //トレースモードの最大座票数
        private const int MaxTracePoints = 128;
        
        //点（十字）の半径
        private const int PointR = 10;  //v9.7追加 by 間々田 2004/11/01

        private enum ShapeHandleType
        {
            NO_EDIT_POINT,          //0
                                    //1-128  トレースのハンドル
            ON_LINE = 999,          //999   ライン上
            LEFT_UPPER = 1000,      //1000   (x1,y1)
            RIGHT_UPPER = 1001,     //1001   (x2,y1)
            LEFT_LOWER = 1002,      //1002   (x1,y2)
            RIGHT_LOWER = 1003,     //1003   (x2,y2)
            MIDDLE_UPPER = 1004,    //1004   (mpx,y1)
            MIDDLE_LOWER = 1006,    //1006   (mpx,y2)
            LEFT_MIDDLE = 1008,     //1008   (x1,mpy)
            RIGHT_MIDDLE = 1009     //1009   (x2,mpy)
        }

        private ShapeHandleType EditPoint;

        #endregion

        #region 構造体宣言

        //'********************************************************************************
        //'  構造体宣言
        //'********************************************************************************

        //'ROI構造体
        private struct RoiInfoType
        {
            public RoiShape shape;          //ROIの種類
            public bool Enabled;            //選択が可能か
            public bool Selected;           //選択されているか
            public bool Sizable;            //サイズ変更可能か
            //public Ipc32v5.RECT area;       //ROI領域
            //public Rectangle area;          //ROI領域
            public Winapi.RECT area;
            //public modLibrary.Points[] p;   //トレース用座標配列
            public Point[] p;   //トレース用座標配列
        }
        
        //Roi格納データ
        private RoiInfoType[] myRoi;
        
        //描画・編集時に表示する仮想ROI
        private RoiInfoType vRoi;
        
        //コピー・ペースト時に格納するROI格納データ    'v9.7追加 by 間々田 2004/11/01
        private RoiInfoType[] ClipRoi;

        #endregion

        #region 共通データ宣言

        //'********************************************************************************
        //'  共通データ宣言
        //'********************************************************************************

        private Form myForm;            //ROIが描画されているフォーム
        //private Form myDisplay;         //ROI表示対象オブジェクト
        private object myDisplay;       //ROI表示対象オブジェクト
        
        private int NumOfRoi;           //ROIの数
        private int myTargetRoi;        //ターゲットROI
        private RoiShape MakeShape;     //作成形状種別
        //private modLibrary.Points mp;   //（移動前の）マウス座標
        private Point mp;   //（移動前の）マウス座標

        //コメント                           'v15.0追加 by 間々田 2009/03/25
        private string myComment;

        //Roiテーブル                        'v15.0追加 by 間々田 2009/03/25
        private string myTable;

        //手動で削除可能                     'v15.0追加 by 間々田 2009/03/25
        private bool myManualCut;
        
        //Roiオブジェクト
        //Shapeｺﾝﾄﾛｰﾙは使わない_2014/09/18(検S1)hata
        //private object[] MyControl = new object[2];
        private int RoiCtrlType = 0;  //(0:ScanImage用(デフォルト)、1:TransImage用）
        private int IndicateRectHandlePointsExFlg = 0; //IndicateRectHandlePointsを実行するフラグ //基本的にはScanImage以外では実行しないが、それ以外でも実行したい場合にONにする。 Rev20.00 追加 by長野 2015/02/06

        //イベント宣言
        public delegate void ChangedEventHandler(string RoiInfo);   // イベントハンドラー
        public event ChangedEventHandler Changed;                   // イベントの宣言

        private int oldDrawRoiX1 = 0; //Rev26.00 change by chouno 2017/11/08
        private int oldDrawRoiX2 = 0; //Rev26.00 change by chouno 2017/11/08
        private int oldDrawRoiY1 = 0; //Rev26.00 change by chouno 2017/11/08
        private int oldDrawRoiY2 = 0; //Rev26.00 change by chouno 2017/11/08

        #endregion

        //'*******************************************************************************
        //'機　　能： クラス初期化処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V9.7　 04/11/01   (SI4)間々田   リニューアル
        //'*******************************************************************************
        private void Class_Initialize()
        {
            myRoi = new RoiInfoType[2]; // 'v15.0変更 by 間々田 2009/03/25

            //仮想ROIを初期化する
            vRoi = new RoiInfoType();

            //クリップ用のROI領域を初期化する
            ClipRoi = new RoiInfoType[1];

	        //登録されているROIの数を０とする
	        NumOfRoi = 0;
	        myTargetRoi = 0;

	        //仮想ROIの初期化
	        vRoi.shape = RoiShape.NO_ROI;

	        //描画するROI種の初期化
	        MakeShape = RoiShape.NO_ROI;

	        //コメント初期化                     'v15.0追加 by 間々田 2009/03/25
	        myComment = "";

	        //テーブル名                         'v15.0追加 by 間々田 2009/03/25
	        myTable = "";

	        //手動で削除可能                     'v15.0追加 by 間々田 2009/03/25
	        myManualCut = true;
        }

        /// <summary>
        /// RoiDataクラスのコンストラクタ
        /// </summary>
        public RoiData()
        {
            Class_Initialize();
        }

        //'*******************************************************************************
        //'機　　能： クラス終了時処理
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V9.7　 04/11/01   (SI4)間々田   リニューアル
        //'*******************************************************************************
        public void Dispose()
        {
            myDisplay = null;

            //Shapeｺﾝﾄﾛｰﾙ(MyControl)は使わない_2014/09/18(検S1)hata
            //MyControl[0] = null;
            //MyControl[1] = null;
            RoiCtrlType = 0;
        }


        //'*******************************************************************************
        //'機　　能： 描画対象フォームを設定
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： theForm         [I/ ] Form      描画対象フォーム
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V9.7　 04/11/01   (SI4)間々田   リニューアル
        //'*******************************************************************************
        //Shapeｺﾝﾄﾛｰﾙ(MyControl)は使わない_2014/09/18(検S1)hata
        //public void SetTarget(object DispObject, RectangleShape RoiControl = null, RectangleShape RoiControl1 = null)
        //public void SetTarget(object DispObject)
        public void SetTarget(object DispObject, int IndicateRectHandlePointsFlg = 0)
        {
            //描画対象オブジェクト
            myDisplay = DispObject;

            //roi設定中
            myRoiFlg = 0;

            //描画対象フォーム
            //try
            //{
            //    myForm = DispObject;
            //}
            //catch
            //{
            //    myForm = DispObject.Parent as Form;
            //}

 
            try
            {
                if (DispObject.GetType() == typeof(frmScanImage))
                {
                    myForm = DispObject as Form;
                }
                else if (DispObject.GetType() == typeof(CTImageCanvas))
                {
                    myForm = (DispObject as CTImageCanvas).Parent as Form;
                }
                else if (DispObject.GetType() == typeof(frmExObsCam)) //Rev23.30 追加 by長野 2016/02/06
                {
                    myForm = DispObject as Form;
                }

            }
            catch
            {
                myForm = null;
            }

            //Shapeｺﾝﾄﾛｰﾙ(MyControl)は使わない_2014/09/18(検S1)hata
            //MyControl[0] = null;
            //MyControl[1] = null;
            //if (RoiControl != null) MyControl[0] = RoiControl as RectangleShape;
            //if (RoiControl1 != null) MyControl[1] = RoiControl1 as RectangleShape;
            //if (myDisplay.Equals(frmTransImage.Instance.ctlTransImage))
            if (myDisplay.Equals(frmTransImage.Instance.ctlTransImage) || myDisplay.Equals(frmExObsCam.Instance))
            {
                RoiCtrlType = 1;
            }
            else
            {
                RoiCtrlType = 0;
            }
            if (IndicateRectHandlePointsFlg == 0)
            {
                IndicateRectHandlePointsExFlg = 0;
            }
            else
            {
                IndicateRectHandlePointsExFlg = 1;
            }

        }

        #region プロパティ


        //roi設定中？
        int myRoiFlg = 0;
        public int RoiFlg
        {
            get
            {
                return myRoiFlg;
            }
            set
            {
                myRoiFlg = RoiFlg;
            }
        }


        //'*******************************************************************************
        //'機　　能： ROI登録最大数
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： theValue        [I/ ] Integer   ROI最大数
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： V9.7　 04/11/01   (SI4)間々田   リニューアル
        //'*******************************************************************************
        public int RoiMaxNum
        {
            get
            {
                return myRoi.GetUpperBound(0);
            }
            set
            {
                Array.Resize(ref myRoi, value + 1);
            }
        }

        //'********************************************************************************
        //'機    能  ：  ROI登録数取得
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        public int NumOfRois
        {
	        get
            {
                return NumOfRoi;
            }
        }

        //'*******************************************************************************
        //'機　　能： コメントプロパティ
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v15.0　2009/03/25   (SI1)間々田   リニューアル
        //'*******************************************************************************
        public string Comment
        {
	        get
            {
                return myComment;
            }

	        set
            {
                myComment = value;
            }
        }

        //'*******************************************************************************
        //'機　　能： ROIテーブルプロパティ
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v15.0　2009/03/25   (SI1)間々田   リニューアル
        //'*******************************************************************************
        public string Table
        {
	        get
            {
                return myTable;
            }

	        set
            {
                myTable = value;
            }
        }

        //'*******************************************************************************
        //'機　　能： ROIテーブルプロパティ
        //'
        //'           変数名          [I/O] 型        内容
        //'引　　数： なし
        //'戻 り 値： なし
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v15.0　2009/03/25   (SI1)間々田   リニューアル
        //'*******************************************************************************
        public bool ManualCut
        {
            get
            {
                return myManualCut;
            }

            set
            {
                myManualCut = value;
            }
        }

        //'********************************************************************************
        //'機    能  ：  描画図形を指定したROIにする
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V9.7  04/10/08   (SI4)間々田  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        public void ModeToPaint(RoiShape theShape)
        {
            //v15.0追加 209/03/26
            if ((RoiMaxNum == 1) && (theShape != RoiShape.NO_ROI))
            {
                DeleteAllRoiData();
            }

            //    ModeState = INIT_STATE 'v9.7削除 by 間々田 2004/11/01
            MakeShape = theShape;
        }

        //'********************************************************************************
        //'機    能  ：  ターゲットROIを返す
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        public int TargetRoi
        {
            get
            {
                return myTargetRoi;
            }
        }
	
        //'********************************************************************************
        //'機    能  ：  選択されているROIが存在するか調べます
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：
        //'
        //'履    歴  ：  V9.7   04/10/08  (SI4)間々田     新規作成
        //'********************************************************************************
        public bool IsExistSelectedRoi
        {
            get
            {
                //戻り値初期化
                bool functionReturnValue = true;

                for (int RI = 1; RI <= NumOfRoi; RI++)
                {
                    if (myRoi[RI].Selected)
                    {
                        return functionReturnValue;
                    }
                }

                functionReturnValue = false;
                return functionReturnValue;
            }
        }

        #endregion

        //'********************************************************************************
        //'機    能  ：  ROIの追加
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        private bool AddNewRoi(RoiShape shape)  //v9.7変更 by 間々田 2004/11/01
        {
            //戻り値初期化
            bool functionReturnValue = false;

            if (NumOfRoi >= myRoi.GetUpperBound(0))
            {
                return functionReturnValue;
            }

            NumOfRoi = NumOfRoi + 1;

            myRoi[NumOfRoi].p = new Point[1];
            myRoi[NumOfRoi].shape = shape;
            myRoi[NumOfRoi].Enabled = true;
            myRoi[NumOfRoi].Selected = false;
            //    RI = NumOfRoi                      'v9.7削除 by 間々田 2004/11/01

            functionReturnValue = true;
            return functionReturnValue;
        }

        //'********************************************************************************
        //'機    能  ：  ROIの形状を調べる
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        public RoiShape GetRoiShape(int RI)
        {
            RoiShape functionReturnValue = default(RoiShape);
            //RoiShape functionReturnValue = RoiShape.NO_ROI;

            try
            {
                if (modLibrary.InRange(RI, 1, NumOfRoi))
                {
                    functionReturnValue = myRoi[RI].shape;
                }
                else
                {
                    functionReturnValue = RoiShape.NO_ROI;
                }

            }
            catch
            { 
            }


            return functionReturnValue;
        }

        //'********************************************************************************
        //'機    能  ：  ROIの形状を調べる
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        public bool IsSizable(int RI)
        {
            bool functionReturnValue = false;

            if (modLibrary.InRange(RI, 1, NumOfRoi))
            {
                functionReturnValue = myRoi[RI].Sizable;
            }
            else
            {
                functionReturnValue = false;
            }

            return functionReturnValue;
        }

        //'********************************************************************************
        //'機    能  ：  ROIの使用可能フラグをセットする
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V6.00  XX/XX/XX  （SI4）間々田  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        public void SetEnableShape(int RI, bool enableFlg)
        {
            if (modLibrary.InRange(RI, 1, NumOfRoi))
            {
                myRoi[RI].Enabled = enableFlg;
            }
        }
	
        //'********************************************************************************
        //'機    能  ：  円のデータを追加（この関数で追加）
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        public int AddCircleShape(int xc, int yc, int r, bool Sizable = true)
        {
            //戻り値初期化
            int functionReturnValue = 0;

	        //パラメータチェック：半径
	        if (!(r > 0))
            {
		        return functionReturnValue;
            }

            //パラメータチェック：描画範囲外
            if (!CheckCircleInArea(xc, yc, r))
            {
                return functionReturnValue;
            }

            if (!AddNewRoi(RoiShape.ROI_CIRC))
            {
                return functionReturnValue;
            }

            myRoi[NumOfRoi].Sizable = Sizable;
            SetCircleShape(NumOfRoi, xc, yc, r);

	        functionReturnValue = NumOfRoi;
	        return functionReturnValue;
        }
	
        //'********************************************************************************
        //'機    能  ：  長方形のデータを追加（この関数で追加）
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        public int AddRectangleShape(int x1, int y1, int x2, int y2, bool Sizable = true)
        {
            //戻り値初期化
            int functionReturnValue = 0;

            //パラメータチェック：描画範囲外
            if (!CheckRectInArea(x1, y1, x2, y2))
            {
                return functionReturnValue;
            }

            if (!AddNewRoi(RoiShape.ROI_RECT))
            {
                return functionReturnValue;
            }

            myRoi[NumOfRoi].Sizable = Sizable;
            SetRectangleShape(NumOfRoi, x1, y1, x2, y2);

	        functionReturnValue = NumOfRoi;
	        return functionReturnValue;
        }
	
        //'********************************************************************************
        //'機    能  ：  長方形のデータを追加（この関数で追加）
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  中心座標と辺までの長さを使用する
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        public int AddRectangleShape2(int xc, int yc, int xl, int yl, bool Sizable = true)
        {
            //戻り値初期化
            int functionReturnValue = 0;

	        if ((xl > 0) && (yl > 0))
            {
		        functionReturnValue = AddRectangleShape(xc - xl, yc - yl, xc + xl, yc + yl, Sizable);
	        }

	        return functionReturnValue;
        }
	
        //'********************************************************************************
        //'機    能  ：  正方形のROIを追加（この関数で追加）
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        public int AddSquareShape(int xc, int yc, int r, bool Sizable = true)
        {
            //戻り値初期化
            int functionReturnValue = 0;

            //パラメータチェック：描画範囲外
            if (!CheckCircleInArea(xc, yc, r))
            {
                return functionReturnValue;
            }

            if (!AddNewRoi(RoiShape.ROI_SQR))
            {
                return functionReturnValue;
            }

            myRoi[NumOfRoi].Sizable = Sizable;
            SetSquareShape(NumOfRoi, xc, yc, r);

	        functionReturnValue = NumOfRoi;
	        return functionReturnValue;
        }
	
        //'********************************************************************************
        //'機    能  ：  ﾄﾚｰｽのデータを追加（この関数で追加）
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        public int AddTraceShape(bool Sizable = true)
        {
            //戻り値初期化
            int functionReturnValue = 0;

            if (!AddNewRoi(RoiShape.ROI_TRACE))
            {
                return functionReturnValue;
            }

            myRoi[NumOfRoi].Sizable = Sizable;
            myRoi[NumOfRoi].p = new Point[1];

	        functionReturnValue = NumOfRoi;
	        return functionReturnValue;
        }
	
        //'********************************************************************************
        //'機    能  ：  線のデータを追加（この関数で追加）
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V9.7   04/11/01  (SI4)間々田     新規作成
        //'********************************************************************************
        public int AddLineShape(int x1, int y1, int x2, int y2, bool Sizable = true)
        {
            //戻り値初期化
            int functionReturnValue = 0;

            //パラメータチェック：描画範囲外
            if (!CheckRectInArea(x1, y1, x2, y2))
            {
                return functionReturnValue;
            }

            //    '始点と終点が同じ場合はＮＧ                     'v15.0追加 by 間々田 2009/03/27
            //    If (x1 = x2) And (y1 = y2) Then Exit Function
            if (!AddNewRoi(RoiShape.ROI_LINE))
            {
                return functionReturnValue;
            }

            myRoi[NumOfRoi].Sizable = Sizable;
            myRoi[NumOfRoi].p = new Point[3];
            SetLineShape(NumOfRoi, x1, y1, x2, y2);

	        functionReturnValue = NumOfRoi;
	        return functionReturnValue;
        }
	
        //'********************************************************************************
        //'機    能  ：  点のデータを追加（この関数で追加）
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V9.7   04/11/01  (SI4)間々田     新規作成
        //'********************************************************************************
        public int AddPointShape(int x, int y)
        {
            //戻り値初期化
            int functionReturnValue = 0;

            //パラメータチェック：描画範囲外
            if (!CheckRectInArea(x, y, x, y))
            {
                return functionReturnValue;
            }

            if (!AddNewRoi(RoiShape.ROI_POINT))
            {
                return functionReturnValue;
            }

            myRoi[NumOfRoi].Sizable = false;
            //myRoi[NumOfRoi].p = new modLibrary.Points[2];
            myRoi[NumOfRoi].p = new Point[2];

            SetPointShape(NumOfRoi, x, y);

	        functionReturnValue = NumOfRoi;
	        return functionReturnValue;
        }
	
        //'********************************************************************************
        //'機    能  ：  円のデータを設定（この関数で設定）
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        public void SetCircleShape(int RI, int xc, int yc, int ro)
        {
	        if (GetRoiShape(RI) == RoiShape.ROI_CIRC)
            {
                myRoi[RI].area.left = xc - ro;
                myRoi[RI].area.top = yc - ro;
                myRoi[RI].area.right = xc + ro;
                myRoi[RI].area.bottom = yc + ro;
            }
        }
	
        //'********************************************************************************
        //'機    能  ：  長方形のデータを設定（この関数で設定）
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        public bool SetRectangleShape(int RI, int x1, int y1, int x2, int y2)
        {
            //戻り値初期化
            bool functionReturnValue = false;

            if (GetRoiShape(RI) != RoiShape.ROI_RECT)
            {
                return functionReturnValue;
            }

	        //領域が範囲外の場合
            if (!CheckRectInArea(x1, y1, x2, y2))
            {
                return functionReturnValue;
            }

            myRoi[RI].area.left = modLibrary.MinVal(x1, x2);
            myRoi[RI].area.top = modLibrary.MinVal(y1, y2);
            myRoi[RI].area.right = modLibrary.MaxVal(x1, x2);
            myRoi[RI].area.bottom = modLibrary.MaxVal(y1, y2);
            
	        functionReturnValue = true;
	        return functionReturnValue;
        }
	
        //'********************************************************************************
        //'機    能  ：  長方形のデータを設定　その２（この関数で設定）
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        public bool SetRectangleShape2(int RI, int xc, int yc, int xl, int yl)
        {
	        bool functionReturnValue = false;

	        if (SetRectangleShape(RI, xc - xl, yc - yl, xc + xl, yc + yl))
            {
                //変更2014/07/14(検S1)hata
                //IndicateRoi();
                //roi表示
                myRoiFlg = 2;
                myForm.Refresh();

		        //イベント生成
		        if (Changed != null)
                {
			        Changed(GetRoiInfo(myTargetRoi));
		        }

		        functionReturnValue = true;
	        }
            else
            {
		        functionReturnValue = false;
	        }

	        return functionReturnValue;
        }
	
        //'********************************************************************************
        //'機    能  ：  正方形のデータを設定（この関数で設定）
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        public void SetSquareShape(int RI, int xc, int yc, int ro)
        {
	        if (GetRoiShape(RI) == RoiShape.ROI_SQR)
            {
		        myRoi[RI].area.left = xc - ro;
		        myRoi[RI].area.top = yc - ro;
		        myRoi[RI].area.right = xc + ro;
		        myRoi[RI].area.bottom = yc + ro;
            }
        }

        //'********************************************************************************
        //'機    能  ：  線のデータを設定（この関数で設定）
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V9.7   04/11/01  (SI4)間々田     新規作成
        //'********************************************************************************
        public bool SetLineShape(int RI, int x1, int y1, int x2, int y2)
        {
            //戻り値初期化
            bool functionReturnValue = false;

            if (GetRoiShape(RI) != RoiShape.ROI_LINE)
            {
                return functionReturnValue;
            }

	        //領域が範囲外の場合
            if (!CheckRectInArea(x1, y1, x2, y2))
            {
                return functionReturnValue;
            }

	        //    '始点と終点が同じ場合はＮＧ                     'v15.0追加 by 間々田 2009/03/27
	        //    If (x1 = x2) And (y1 = y2) Then Exit Function

	        myRoi[RI].p[1].X = x1;
	        myRoi[RI].p[1].Y = y1;
	        myRoi[RI].p[2].X = x2;
	        myRoi[RI].p[2].Y = y2;

	        UpdateTraceArea(ref myRoi[RI]);

	        functionReturnValue = true;
	        return functionReturnValue;
        }	
	
        //'********************************************************************************
        //'機    能  ：  点のデータを設定（この関数で設定）
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V9.7   04/11/01  (SI4)間々田     新規作成
        //'********************************************************************************
        private bool SetPointShape(int RI, int x, int y)
        {
            //戻り値初期化
            bool functionReturnValue = false;

            if (GetRoiShape(RI) != RoiShape.ROI_POINT)
            {
                return functionReturnValue;
            }

	        //座標が描画範囲内
            if (!CheckRectInArea(x, y, x, y))
            {
                return functionReturnValue;
            }

	        myRoi[RI].p[1].X = x;
	        myRoi[RI].p[1].Y = y;

	        UpdateTraceArea(ref myRoi[RI]);

	        functionReturnValue = true;
	        return functionReturnValue;
        }

        //'********************************************************************************
        //'機    能  ：  ﾄﾚｰｽのデータを設定（この関数で設定）
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        public void AddTracePoint(int RI, int x, int y)
        {
	        int no = 0;

            if (GetRoiShape(RI) != RoiShape.ROI_TRACE)
            {
                return;
            }

            no = myRoi[RI].p.GetUpperBound(0) + 1;

            if (no > MaxTracePoints)
            {
                return;
            }

            Array.Resize(ref myRoi[RI].p, no + 1);

            myRoi[RI].p[no].X = x;
            myRoi[RI].p[no].Y = y;

            UpdateTraceArea(ref myRoi[RI]);
        }

        //'********************************************************************************
        //'機    能  ：  トレースの範囲を得る
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.7   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        private void UpdateTraceArea(ref RoiInfoType theRoi)    //v9.7変更 by 間々田 2004/11/01
        {
	        int i = 0;
	        int x = 0;
	        int y = 0;

            for (i = 1; i <= theRoi.p.GetUpperBound(0); i++)
            {
                x = theRoi.p[i].X;
                y = theRoi.p[i].Y;

                if (i == 1)
                {
                    theRoi.area.left = x;
                    theRoi.area.top = y;
                    theRoi.area.right = x;
                    theRoi.area.bottom = y;
                }
                else
                {
                    theRoi.area.left = modLibrary.MinVal(theRoi.area.left, x);
                    theRoi.area.top = modLibrary.MinVal(theRoi.area.top, y);
                    theRoi.area.right = modLibrary.MaxVal(theRoi.area.right, x);
                    theRoi.area.bottom = modLibrary.MaxVal(theRoi.area.bottom, y);
                }
            }
        }
	
        //'********************************************************************************
        //'機    能  ：  ﾄﾚｰｽのﾎﾟｲﾝﾄ数を得る
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        public int NumOfTracePoints(int RI)
        {
            //戻り値初期化
            int functionReturnValue = 0;

            if (GetRoiShape(RI) != RoiShape.ROI_TRACE)
            {
                return functionReturnValue;
            }

            functionReturnValue = myRoi[RI].p.GetUpperBound(0);

	        return functionReturnValue;
        }

        //'********************************************************************************
        //'機    能  ：  指定されたROIの座標の取得：円
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        public bool GetCircleShape(int RI, ref int xc, ref int yc, ref int ro)
        {
            //戻り値初期化
            bool functionReturnValue = false;

            if (GetRoiShape(RI) != RoiShape.ROI_CIRC)
            {
                return functionReturnValue;
            }

            //2014/11/13hata キャストの修正
            //ro = (myRoi[RI].area.right - myRoi[RI].area.left) / 2;
            ro = Convert.ToInt32((myRoi[RI].area.right - myRoi[RI].area.left) / 2F);
            xc = myRoi[RI].area.left + ro;
            yc = myRoi[RI].area.top + ro;

	        functionReturnValue = true;
	        return functionReturnValue;
        }

        //'********************************************************************************
        //'機    能  ：  指定されたROIの座標の取得：長方形
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        public bool GetRectangleShape(int RI, ref int x1, ref int y1, ref int x2, ref int y2)
        {
            //戻り値初期化
            bool functionReturnValue = false;

            if (GetRoiShape(RI) != RoiShape.ROI_RECT)
            {
                return functionReturnValue;
            }

            x1 = myRoi[RI].area.left;
            y1 = myRoi[RI].area.top;
            x2 = myRoi[RI].area.right;
            y2 = myRoi[RI].area.bottom;

	        functionReturnValue = true;
	        return functionReturnValue;
        }

        //'********************************************************************************
        //'機    能  ：  指定されたROIの座標の取得：長方形（中心座標とＸ・Ｙ方向の大きさを取得）
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        public bool GetRectangleShape2(int i, ref int xc, ref int yc, ref int xl, ref int yl)
        {
	        int x1 = 0;
	        int y1 = 0;
	        int x2 = 0;
	        int y2 = 0;
            int remainder;  // 余り

	        //戻り値初期化
            bool functionReturnValue = false;

	        //GetRectangleShapeを呼び出す
            if (!GetRectangleShape(i, ref x1, ref y1, ref x2, ref y2))
            {
                return functionReturnValue;
            }

            xl = Math.DivRem((x2 - x1), 2, out remainder);
            yl = Math.DivRem((y2 - y1), 2, out remainder);
	        xc = x1 + xl;
	        yc = y1 + yl;

	        functionReturnValue = true;
	        return functionReturnValue;
        }

        //'********************************************************************************
        //'機    能  ：  指定されたROIの座標の取得：正方形
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        public bool GetSquareShape(int RI, ref int xc, ref int yc, ref int ro)
        {
            //戻り値初期化
            bool functionReturnValue = false;

            if (GetRoiShape(RI) != RoiShape.ROI_SQR)
            {
                return functionReturnValue;
            }

            int remainder;  // 余り
            ro = Math.DivRem((myRoi[RI].area.right - myRoi[RI].area.left), 2, out remainder);
            xc = myRoi[RI].area.left + ro;
            yc = myRoi[RI].area.top + ro;

	        functionReturnValue = true;
	        return functionReturnValue;
        }

        //'********************************************************************************
        //'機    能  ：  ﾄﾚｰｽのデータを得る
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        public bool GetTracePoint(int RI, int pno, ref int x, ref int y)
        {
            //戻り値初期化
            bool functionReturnValue = false;

            if (GetRoiShape(RI) != RoiShape.ROI_TRACE)
            {
                return functionReturnValue;
            }

            if (!modLibrary.InRange(pno, 1, myRoi[RI].p.GetUpperBound(0)))
            {
                return functionReturnValue;
            }
            x = myRoi[RI].p[pno].X;
            y = myRoi[RI].p[pno].Y;

	        functionReturnValue = true;
	        return functionReturnValue;
        }
		
        //'********************************************************************************
        //'機    能  ：  指定されたROIの座標の取得：線
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V9.7   04/11/01  (SI4)間々田     新規作成
        //'********************************************************************************
        public bool GetLineShape(int RI, ref int x1, ref int y1, ref int x2, ref int y2)
        {
            //戻り値初期化
            bool functionReturnValue = false;

            if (GetRoiShape(RI) != RoiShape.ROI_LINE)
            {
                return functionReturnValue;
            }

            x1 = myRoi[RI].p[1].X;
            y1 = myRoi[RI].p[1].Y;
            x2 = myRoi[RI].p[2].X;
            y2 = myRoi[RI].p[2].Y;

	        functionReturnValue = true;
	        return functionReturnValue;
        }

        //'********************************************************************************
        //'機    能  ：  指定されたROIの座標の取得：点
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V9.7   04/11/01  (SI4)間々田     新規作成
        //'********************************************************************************
        public bool GetPointShape(int RI, ref int x, ref int y)
        {
            //戻り値初期化
            bool functionReturnValue = false;

            if (GetRoiShape(RI) != RoiShape.ROI_POINT)
            {
                return functionReturnValue;
            }
	
	        x = myRoi[RI].p[1].X;
	        y = myRoi[RI].p[1].Y;

	        functionReturnValue = true;
	        return functionReturnValue;
        }

        //'********************************************************************************
        //'機    能  ：  矩形の中にある図形をすべて選択状態にする
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        //'Public Sub ChooseShapeInRectangle(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer)
        private void SelectRoiInRectangle(int x1, int y1, int x2, int y2)
        {
	        int RI = 0;

	        //選択ROIのクリア
	        //    ClearSelectedRoi False         'v9.7削除 by 間々田 2004/11/01

	        //v9.7追加 by 間々田 2004/11/01
	        for (RI = 1; RI <= NumOfRoi; RI++)
            {
		        myRoi[RI].Selected = false;
	        }

	        for (RI = 1; RI <= NumOfRoi; RI++)
            {
                if (Figure.PointInRectangle(myRoi[RI].area.left, myRoi[RI].area.top, x1, y1, x2, y2))
                {
                    if (Figure.PointInRectangle(myRoi[RI].area.right, myRoi[RI].area.bottom, x1, y1, x2, y2))
                    {
                        // AddChooseRoi RI
                        SelectRoi(RI, true);    //v9.7変更 by 間々田 2004/11/01
			        }
		        }
	        }

            //変更2014/07/14(検S1)hata
            //IndicateRoi(g);
            //roi表示
            myRoiFlg = 2;
            myForm.Refresh();

	        //イベント生成
	        if (Changed != null)
            {
		        Changed(GetRoiInfo(myTargetRoi));
	        }
        }

        //'********************************************************************************
        //'機    能  ：  すべてのROIデータを削除
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        public void DeleteAllRoiData()
        {
	        DeleteRoiData(false);
        }

        //'********************************************************************************
        //'機    能  ：  選択されているROIの切り取り
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        public void CurrentRoiCut()
        {
	        DataToClipBoard();
	        DeleteRoiData();
        }

        //'********************************************************************************
        //'機    能  ：  選択されているROIを削除
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        private void DeleteRoiData(bool SelectedOnly = true)
        {
            //データを削除
            int counter = NumOfRoi;

            for (int RI = 1; RI <= counter; RI++)
            {
                if (myRoi[RI].Selected || (!SelectedOnly))
                {
                    myRoi[RI].shape = RoiShape.NO_ROI;
                    NumOfRoi = NumOfRoi - 1;
                }
            }

            PackRoiRoom();

            //UpdateTargetRoi

            //変更2014/07/14(検S1)hata
            //IndicateRoi(g);      //v9.7変更 by 間々田 2004/11/01
            //roi表示
            myRoiFlg = 2;
            myForm.Refresh();

            //イベント生成
            if (Changed != null)
            {
                Changed(GetRoiInfo(myTargetRoi));
            }
        }

        //'********************************************************************************
        //'機    能  ：  ROI領域を詰める
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'********************************************************************************
        private void PackRoiRoom()
        {
	        int fi = 1;

            for (int i = 1; i <= myRoi.GetUpperBound(0); i++)
            {
		        if (myRoi[i].shape == RoiShape.NO_ROI)
                {
			        fi = i;
			        break;
		        }
	        }

            for (int i = (fi + 1); i <= myRoi.GetUpperBound(0); i++)
            {
		        if (myRoi[i].shape != RoiShape.NO_ROI)
                {
			        myRoi[fi] = myRoi[i];
			        myRoi[i].shape = RoiShape.NO_ROI;
			        fi = fi + 1;
		        }
	        }
        }
	
        //'********************************************************************************
        //'機    能  ：   ROIの貼り付け
        //'               変数名           [I/O] 型        内容
        //'引    数  ：   なし
        //'戻 り 値  ：   なし
        //'補    足  ：   クリップボードからROIデータを読みだし、選択されているROIに追加する
        //'
        //'履    歴  ：   V1.00  XX/XX/XX  ??????????????  新規作成
        //'               V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        public void RoiPaste()
        {
	        int RI;

            SelectRoi(0);   //いったんすべてのROIを非選択にする

            for (RI = 1; RI <= ClipRoi.GetUpperBound(0); RI++)
            {
		        //v11.2追加 by 間々田 2006/02/03 ペーストしてもROIが最大描画可能個数（普通は20個）を超えないようにした
		        //if (NumOfRoi < Information.UBound(myRoi))
                if (NumOfRoi < myRoi.GetUpperBound(0))
                {
			        NumOfRoi = NumOfRoi + 1;

                    //変更2015/01/23hata
                    //参照渡しではなく値渡しをする
                    //myRoi[NumOfRoi] = ClipRoi[RI];
                    myRoi[NumOfRoi] = RoiInfoCopy(ClipRoi[RI]);
                   
                    myRoi[NumOfRoi].Selected = true;
                }
                else   //v11.2追加 by 間々田 2006/02/03
                {
			        //メッセージ表示：                                                               'v11.2追加 by 間々田 2006/02/03
			        //MsgBox "描画できるROIは " & CStr(UBound(myRoi)) & "個までです。", vbExclamation 'v11.2追加 by 間々田 2006/02/03
			        //v17.60 ストリングテーブル化 by長野 2011/05/25
                    string msg = StringTable.GetResString(20117, (myRoi.GetUpperBound(0)).ToString());

                    MessageBox.Show(msg, "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

			        break;
                }   //v11.2追加 by 間々田 2006/02/03
            }   //v11.2追加 by 間々田 2006/02/03

            //変更2014/07/14(検S1)hata
            //IndicateRoi(g);
            //roi表示
            myRoiFlg = 2;
            myForm.Refresh();

	        //イベント生成
	        if (Changed != null)
            {
		        Changed(GetRoiInfo(myTargetRoi));
	        }
        }
	
        //'********************************************************************************
        //'機    能  ：  描画されている全ROIの切り取り
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        public void AllRoiCut()
        {
	        DataToClipBoard(false);
	        DeleteAllRoiData();
        }
	
        //'********************************************************************************
        //'機    能  ：  ROIをクリップボードにコピー
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  SelectedOnly     Boolean         True: 選択されているROIのみコピー
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        public void DataToClipBoard(bool SelectedOnly = true)
        {
	        //    Dim MyRoiText   As String   'コピー＆ペースト用領域
	        int RI;
	        int N;

	        //ROI情報格納スタート
	        //    MyRoiText = "TOSCANER" & vbCrLf    'v9.7削除 by 間々田 2004/11/01   
            ClipRoi = new RoiInfoType[1];   // 'v9.7追加 by 間々田 2004/11/0

	        for (RI = 1; RI <= NumOfRoi; RI++)
            {
		        if (myRoi[RI].Selected || (!SelectedOnly))
                {
                    N = ClipRoi.GetUpperBound(0) + 1;

			        Array.Resize(ref ClipRoi, N + 1);
			        
			        ClipRoi[N] = myRoi[RI];
		        }
	        }

	        //    'クリップボードにセット
	        //    Clipboard.SetText MyRoiText    'v9.7削除 by 間々田 2004/11/01

	        //v15.0追加 by 間々田 2009/02/20
	        //    If Not (myForm Is Nothing) Then
	        //        myForm.mnuROIEditPaste.Enabled = IsExistRoi() 'クリップボードにROIが存在するか？
	        //        myForm.Toolbar1.Buttons("Paste").Enabled = myForm.mnuROIEditPaste.Enabled
	        //    End If
	        //v15.0追加 by 間々田 2009/03/28

            //変更2014/07/14(検S1)hata
            //IndicateRoi(g);
            //roi表示
            myRoiFlg = 2;
            myForm.Refresh();

            //イベント生成
	        if (Changed != null)
            {
		        Changed(GetRoiInfo(myTargetRoi));
	        }
        }

        //'********************************************************************************
        //'機    能  ：  クリップボードにＲＯＩが存在するか調べる
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：                   [ /O] Boolean   結果 (True:有り,False:無し)
        //'補    足  ：
        //'
        //'履    歴  ：  V4.0   01/03/06  (SI1)鈴山       新規作成
        //'********************************************************************************
        public bool IsExistRoi()
        {
	        //    IsExistRoi = (GetFirstItem(Clipboard.GetText(), vbCrLf) = "TOSCANER")
            //return (Information.UBound(ClipRoi) > 0);   //v9.7変更 by 間々田 2004/11/01

            return (ClipRoi.GetUpperBound(0) > 0);   //v9.7変更 by 間々田 2004/11/01
        }

        //'********************************************************************************
        //'機    能  ：  ROIを表示する。
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        //public void IndicateRoi()
        public void IndicateRoi(Graphics g)
        {
	        int RI;
	        Color theColor = default(Color);

            myTargetRoi = 0;

            for (RI = 1; RI <= NumOfRoi; RI++)
            {
                if (!myRoi[RI].Selected)
                {
                }
                else if (myTargetRoi == 0)
                {
                    myTargetRoi = RI;
                }
                else
                {
                    myTargetRoi = 0;
                    break;
                }
            }

            //Shapeｺﾝﾄﾛｰﾙ(MyControl)は使わない_2014/09/18(検S1)hata
            ////ROIを消去
            //if (MyControl[1] == null)
            //{
            //    //これは不要
            //    //ここで消してはいけない。画像を消してしまう
            //    //Graphics graphic = myDisplay.CreateGraphics();
            //    //g.Clear(theColor);
            //}
            //else
            //{
            //    //MyControl[1].Visible = (NumOfRoi > 0);
            //    if (MyControl.GetType() == typeof(RectangleShape))
            //    {
            //        RectangleShape theControl0 = MyControl[1] as RectangleShape;
            //        theControl0.Visible = (NumOfRoi > 0);
            //    }
            //    else if (MyControl.GetType() == typeof(OvalShape))
            //    {
            //        OvalShape theControl1 = MyControl[1] as OvalShape;
            //        theControl1.Visible = (NumOfRoi > 0);
            //    }
            //}


            for (RI = 1; RI <= NumOfRoi; RI++)
            {
                //ROIの表示
                //theColor = DrawRoi(myRoi[RI], true, (RI == myTargetRoi) & myRoi[RI].Sizable, g);
                //Rev23.30 DrawRoi内でROI情報を変更するため引数変更 by長野 2016/02/08
                theColor = DrawRoi(ref myRoi[RI], true, (RI == myTargetRoi) & myRoi[RI].Sizable, g);

                //Roi No.の表示
                IndicateRoiNo(RI, theColor,g);
            }

            //Shapeｺﾝﾄﾛｰﾙ(MyControl)は使わない_2014/09/18(検S1)hata
            //if (MyControl[0] != null)
            //{
            //    //MyControl[0].Visible = vRoi.shape != RoiShape.NO_ROI;
            //    if (MyControl.GetType() == typeof(RectangleShape))
            //    {
            //        RectangleShape theControl0 = MyControl[0] as RectangleShape;
            //        theControl0.Visible = vRoi.shape != RoiShape.NO_ROI;
            //    }
            //    else if (MyControl.GetType() == typeof(OvalShape))
            //    {
            //        OvalShape theControl1 = MyControl[0] as OvalShape;
            //        theControl1.Visible = vRoi.shape != RoiShape.NO_ROI;
            //    } 
            //}

            //仮想ROIも表示      'v9.7追加 by 間々田 2004/11/01
            if (vRoi.shape != RoiShape.NO_ROI)
            {
                //DrawRoi(vRoi, false, g: g);
                //Rev23.30 DrawRoi内でROI情報を変更するため引数変更 by長野 2016/02/08
                DrawRoi(ref vRoi, false, g: g);
            }
        }


        //外部から表示するための関数
        public void DispRoi(Graphics g)
        {
            switch (myRoiFlg)
            { 
                //case 1:
                //    //作成(開始)の場合
                //    DrawRoi(vRoi, false, g: g);
                //    break;

                //case 2:
                //    //作成(終了)の場合
                //    IndicateRoi(g);
                //    break;

                //case 3:
                //    //編集中の場合
                //    IndicateRoi(g);
                //    DrawRoi(vRoi, false, g: g);
                //    break;
                //Rev23.30 DrawRoi内でROI情報を変更するため引数変更 by長野 2016/02/08
                case 1:
                    //作成(開始)の場合
                    DrawRoi(ref vRoi, false, g: g);
                    break;

                case 2:
                    //作成(終了)の場合
                    IndicateRoi(g);
                    break;

                case 3:
                    //編集中の場合
                    IndicateRoi(g);
                    DrawRoi(ref vRoi, false, g: g);
                    break;

            }
        }


        //'*************************************************************************************************
        //'機　　能： Roiの描画
        //'
        //'           変数名          [I/O] 型            内容
        //'引　　数：
        //'戻 り 値：
        //'
        //'補　　足： なし
        //'
        //'履　　歴： v13.0 2007/04/04 (WEB)間々田    新規作成
        //'*************************************************************************************************
        //private Color DrawRoi(RoiInfoType theRoi, bool IsReal = true, bool IsTarget = false)
        //private Color DrawRoi(RoiInfoType theRoi, bool IsReal = true, bool IsTarget = false, Graphics g = null)
        //Rev23.30 DrawRoi内でROI情報を変更するため引数変更 by長野 2016/02/08
        private Color DrawRoi(ref RoiInfoType theRoi, bool IsReal = true, bool IsTarget = false, Graphics g = null)
        {

            Color theColor = default(Color);

            int x1 = 0;
            int y1 = 0;
            int x2 = 0;
            int y2 = 0;
            int xc = 0;
            int yc = 0;
            int r = 0;
            int i = 0;
            int remainder;
            //bool modRevers = false; // 反転有無（反転なし：false、反転あり：true） 
            int cntrlIndex = -1;     // コントロールのインデックス


            #region //コントロール選択色
            //Shapeｺﾝﾄﾛｰﾙ(MyControl)は使わない_2014/09/18(検S1)hata
            //if ((MyControl[0] == null) || (MyControl[1] == null))
            //{
            //    cntrlIndex = -1;
            //}
            //else
            //{
            //    if (IsReal)
            //    {
            //        cntrlIndex = 1;
            //    }
            //    else
            //    {
            //        cntrlIndex = 0;
            //    }
            //}
            #endregion //コントロール選択色

            #region //色

            if (!IsReal)
            {
                //theColor = Color.White;
                theColor = Color.Magenta;
            }
            else if (theRoi.Selected)
            {
                theColor = Color.Lime;
            }
            else if (theRoi.Enabled)
            {
                theColor = Color.Cyan;
            }
            else
            {
                theColor = Color.Yellow;
            }
            Pen myPen = new Pen(theColor);
            #endregion

            #region //線幅

            ////if (theControl == null)
            //if(MyControl[cntrlIndex] == null)
            //{
            //    //myDisplay.DrawWidth = 1;
            //    myPen.Width = 1;
            //}
            //else
            //{
            //    //theControl.BorderWidth = 1;
            //    //borderWidth = 1;
            //    myPen.Width = 1;
            //}
            myPen.Width = 1;

            #endregion

            #region //線種
            ////if (theControl == null)
            ////if(MyControl[cntrlIndex] == null)
            //if (cntrlIndex == -1)
            //{
            //    if (!IsReal)
            //    {
            //        //myDisplay.DrawMode = vbXorPen;
            //        //myDisplay.DrawStyle = vbDash;

            //        //vbXorPenに相当するC#の処理が不明
            //        //ControlPaint.DrawReversibleFrameで対応する
            //        modRevers = true;    //反転あり
            //        myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            //    }
            //    else
            //    {
            //        //myDisplay.DrawMode = vbCopyPen;
            //        //myDisplay.DrawStyle = vbSolid;

            //        //vbCopyPenに相当するC#の処理が不明
            //        //ControlPaint.DrawReversibleFrameで対応する
            //        modRevers = false;   //反転なし
            //        myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            //    }
            //}
            //else
            //{
            //    if (!IsReal)
            //    {
            //        //theControl.DrawMode = vbXorPen
            //        //v17.60 上記の設定では見づらいため変更 by長野 2011/05/31
            //        //theControl.DrawMode = vbMergePenNot;
            //        //theControl.BorderStyle = vbBSDash;

            //        modRevers = true;    //反転あり
            //        //theControl.BorderStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            //        myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            //    }
            //    else
            //    {
            //        //theControl.DrawMode = vbCopyPen;
            //        //theControl.BorderStyle = vbBSSolid;
            //        modRevers = false;   //反転なし
            //        //theControl.BorderStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            //        myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            //    }
            //}
            if (!IsReal)
            {
                //myDisplay.DrawMode = vbXorPen;
                //myDisplay.DrawStyle = vbDash;
                //modRevers = true;    //反転あり
                myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                //破線幅の設定
                //myPen.DashPattern = new float[] { 8.0F, 4.0F, 2.0F, 6.0F };     //1点鎖線
                myPen.DashPattern = new float[] { 8.0F, 4.0F, 8.0F, 4.0F };     //破線

                ////2重線にする(0～1の間で数値設定)
                //myPen.CompoundArray = new float[] { 0.0f, 0.35f, 0.65f, 1.0f };
                ////2重線なので幅は"3"とする
                //myPen.Width = 3;
                
            }
            else
            {
                //myDisplay.DrawMode = vbCopyPen;
                //myDisplay.DrawStyle = vbSolid;
                //modRevers = false;   //反転なし
                myPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

            }

            #endregion

            #region //座標変換

            //if (theControl == null)
            //if(MyControl[cntrlIndex] == null)
            if (cntrlIndex == -1)
            {
                //Rev23.30 条件追加 by長野 2016/02/08
                if(myDisplay.Equals(frmScanImage.Instance))
                {
                    x1 = (int)frmScanImage.Instance.GetXOnForm(theRoi.area.left);
                    y1 = (int)frmScanImage.Instance.GetYOnForm(theRoi.area.top);
                    x2 = (int)frmScanImage.Instance.GetXOnForm(theRoi.area.right);
                    y2 = (int)frmScanImage.Instance.GetYOnForm(theRoi.area.bottom);
                }
                else if (myDisplay.Equals(frmExObsCam.Instance))
                {
                    x1 = (int)frmExObsCam.Instance.GetXOnForm(theRoi.area.left);
                    y1 = (int)frmExObsCam.Instance.GetYOnForm(theRoi.area.top);
                    x2 = (int)frmExObsCam.Instance.GetXOnForm(theRoi.area.right);
                    y2 = (int)frmExObsCam.Instance.GetYOnForm(theRoi.area.bottom);
              
                    //Rev23.00 制限画素数に合わせて座標調整 by長野 2016/02/06
                    if (frmExObsCam.Instance.ftableOn == false)
                    {
                        float center = (float)(1023.0 / 2.0);
                        float h_width = Math.Abs(x2 - x1) / 2;
                        float h_height = Math.Abs(y2 - y1) / 2;
                        if ((Math.Abs(x2 - x1)) >= frmExObsCam.Instance.LimitWidth)
                        {
                            h_width = frmExObsCam.Instance.LimitWidth / 2;
                        }
                        else
                        {
                            h_width = Math.Abs(x2 - x1) / 2;
                        }
                        if (Math.Abs(y2 - y1) >= frmExObsCam.Instance.LimitHeight)
                        {
                            h_height = frmExObsCam.Instance.LimitHeight / 2;
                        }
                        else
                        {
                            h_height = Math.Abs(y2 - y1) / 2;
                        }

                        x1 = (int)(center - h_width);
                        x2 = (int)(center + h_width);
                        y1 = (int)(center - h_height);
                        y2 = (int)(center + h_height);
                        theRoi.area.left = x1;
                        theRoi.area.top = y1;
                        theRoi.area.right = x2;
                        theRoi.area.bottom = y2;
                    }
                    else
                    {
                        int difX = frmExObsCam.Instance.LimitWidth - Math.Abs(x2 - x1);
                        int difY = frmExObsCam.Instance.LimitWidth - Math.Abs(y2 - y1);
                        if (difX < 0 )
                        {
                            if (x1 != oldDrawRoiX1 && x2 == oldDrawRoiX2)
                            {
                                x1 = x1 + Math.Abs(difX);
                            }
                            if (x1 == oldDrawRoiX1 && x2 != oldDrawRoiX2)
                            {
                                x2 = x2 - Math.Abs(difX);
                            }
                            else if(x1 != oldDrawRoiX1 && x2 != oldDrawRoiX2)
                            {
                                x1 = x1 + Math.Abs(difX) / 2 + 1;
                                x2 = x2 - Math.Abs(difX) / 2;
                            }
                            theRoi.area.left = x1;
                            theRoi.area.right = x2;
                        }
                        if (difY < 0)
                        {
                            if (y1 != oldDrawRoiY1 && y2 == oldDrawRoiY2)
                            {
                                y1 = y1 + Math.Abs(difY);
                            }
                            if (y1 == oldDrawRoiY1 && y2 != oldDrawRoiY2)
                            {
                                y2 = y2 - Math.Abs(difY);
                            }
                            else if(y1 != oldDrawRoiY1 && y2 != oldDrawRoiY2)
                            {
                                y1 = y1 + Math.Abs(difY) / 2 + 1;
                                y2 = y2 - Math.Abs(difY) / 2;
                            }
                            theRoi.area.top = y1;
                            theRoi.area.bottom = y2;
                        }
                    }
                    oldDrawRoiX1 = x1; //Rev26.00 change by chouno 2017/11/08
                    oldDrawRoiX2 = x2; //Rev26.00 change by chouno 2017/11/08
                    oldDrawRoiY1 = y1; //Rev26.00 change by chouno 2017/11/08
                    oldDrawRoiY2 = y2; //Rev26.00 change by chouno 2017/11/08
                }
                else //Rev23.31 修正 by長野 2016/03/30
                {
                    x1 = (int)frmScanImage.Instance.GetXOnForm(theRoi.area.left);
                    y1 = (int)frmScanImage.Instance.GetYOnForm(theRoi.area.top);
                    x2 = (int)frmScanImage.Instance.GetXOnForm(theRoi.area.right);
                    y2 = (int)frmScanImage.Instance.GetYOnForm(theRoi.area.bottom);                  
                }
            }
            else
            {
                //x1 = .Left * Screen.TwipsPerPixelX    // TwipsPerPixelX: 1ピクセルあたりのtwip数. Windows標準のシステム解像度である96dpiの環境下では15Twipsで1ピクセル
                //y1 = .Top * Screen.TwipsPerPixelY
                //x2 = .Right * Screen.TwipsPerPixelX
                //y2 = .Bottom * Screen.TwipsPerPixelY
                x1 = theRoi.area.left;
                y1 = theRoi.area.top;
                x2 = theRoi.area.right;
                y2 = theRoi.area.bottom;

            }
            r = Math.DivRem((x2 - x1), 2, out remainder);
            xc = x1 + r;
            yc = y1 + r;

            #endregion

            if ((cntrlIndex == -1) && (g == null)) return theColor;


            RoiShape roi = (RoiShape)Math.Abs((decimal)theRoi.shape);
 
            switch (roi)
            {
                //円ROI
                case RoiShape.ROI_CIRC:

                    //Shapeｺﾝﾄﾛｰﾙ(MyControl)は使わない_2014/09/18(検S1)hata
                    //if (cntrlIndex == -1)
                    //{
                    //    g.DrawEllipse(myPen, x1, y1, x2 - x1 + 1, y2 - y1 + 1);         //'描画
                    //    if (IsTarget)
                    //    {
                    //        //System.Drawing.Drawing2D.GraphicsPath Gp = new System.Drawing.Drawing2D.GraphicsPath();
                    //        //Gp.AddRectangle(new Rectangle( x1, y1, x2 - x1 + 1, y2 - y1 + 1));             
                    //        //g.DrawPath(myPen,Gp);

                    //        g.DrawRectangle(myPen, x1, y1, x2 - x1 + 1, y2 - y1 + 1);   //'（編集用の）正方形の枠を描画
                    //        IndicateRectHandlePoints(x1, y1, x2, y2, g);                //（編集用の）点を描画
                    //    }
                    //}
                    //else
                    //{
                        
                    //    //コントロールを円で表示
                    //    OvalShape theControl0 = MyControl[cntrlIndex] as OvalShape;
                    //    if (theControl0 != null)
                    //    {
                    //        theControl0.SetBounds(x1, y1, x2 - x1 + 1, y2 - y1 + 1);
                    //        theControl0.BorderColor = myPen.Color;
                    //        theControl0.BorderStyle = myPen.DashStyle;
                    //        theControl0.FillStyle = FillStyle.Transparent;
                    //    }
                    //}
                    g.DrawEllipse(myPen, x1, y1, x2 - x1 + 1, y2 - y1 + 1);
                    if (IsTarget)
                    {
                        g.DrawRectangle(myPen, x1, y1, x2 - x1 + 1, y2 - y1 + 1);
                        IndicateRectHandlePoints(x1, y1, x2, y2, g);     //（編集用の）点を描画
                    }
                    break;


                //'長方形ROI/正方形ROI
                case RoiShape.ROI_RECT:
                case RoiShape.ROI_SQR:

                    //Shapeｺﾝﾄﾛｰﾙ(MyControl)は使わない_2014/09/18(検S1)hata
                    //if (cntrlIndex == -1)
                    //{
                    //    g.DrawRectangle(myPen, x1, y1, x2 - x1 + 1, y2 - y1 + 1);
                    //    if (IsTarget)
                    //    {
                    //        IndicateRectHandlePoints(x1, y1, x2, y2, g);     //（編集用の）点を描画
                    //    }
                    //}
                    //else
                    //{   //コントロールを長方形で表示
                    //    RectangleShape theControl1 = MyControl[1] as RectangleShape;
                    //    if (theControl1 != null)
                    //    {
                    //        theControl1.SetBounds(x1, y1, x2 - x1 + 1, y2 - y1 + 1);
                    //        theControl1.BorderColor = myPen.Color;
                    //        theControl1.BorderStyle = myPen.DashStyle;
                    //        theControl1.FillStyle = FillStyle.Transparent;
                    //    }
                    //}
                    g.DrawRectangle(myPen, x1, y1, x2 - x1 + 1, y2 - y1 + 1);
                    if (IsTarget)
                    {
                        IndicateRectHandlePoints(x1, y1, x2, y2, g);     //（編集用の）点を描画
                    }

                    break;


                // 'トレースROI/線
                case RoiShape.ROI_LINE:

                    // '線を描画
                    if (myDisplay != null)
                    {
                        int Lx1 = (int)frmScanImage.Instance.GetXOnForm(theRoi.p[1].X);
                        int Ly1 = (int)frmScanImage.Instance.GetYOnForm(theRoi.p[1].Y);
                        int Lx2 = (int)frmScanImage.Instance.GetXOnForm(theRoi.p[2].X);
                        int Ly2 = (int)frmScanImage.Instance.GetYOnForm(theRoi.p[2].Y);

                        // 線を描画
                        myPen.Width = 1;
                        g.DrawLine(myPen, Lx1, Ly1, Lx2, Ly2);

                    }
                    break;                
                
                
                case RoiShape.ROI_TRACE:
 
                    int xpre = 0;
                    int ypre = 0;

                    Debug.WriteLine("len = " + theRoi.p.Length.ToString());


                    for (i = 1; i <= theRoi.p.GetUpperBound(0); i++)
                    {
                        //フォーム上の座標に変換
                        //xc = myDisplay.GetXOnForm(theRoi.p[i].X);
                        //yc = myDisplay.GetYOnForm(theRoi.p[i].Y);
                        xc = (int)frmScanImage.Instance.GetXOnForm(theRoi.p[i].X);
                        yc = (int)frmScanImage.Instance.GetYOnForm(theRoi.p[i].Y);
                        
                        // '始点の場合
                        if (i == 1)
                        {

                            //ラインの描画位置
                            //myDisplay.CurrentX = xc;
                            //myDisplay.CurrentY = yc;

                            // '座標を保存
                            x1 = xc;
                            y1 = yc;
                            xpre = xc;
                            ypre = yc;

                        }
                        // '始点以外の場合
                        else
                        {
                            // '線を描画
                            myPen.Width = 1;
                            if (myDisplay != null)
                            {
                                // 前回描画を終了した点から線を描画
                                //g = myDisplay.CreateGraphics();
                                g.DrawLine(myPen, xpre, ypre, xc, yc);

                                // '座標を保存
                                xpre = xc;
                                ypre = yc;
                            }
                        }

                        // 'カレントROIの場合、（編集用の）点を描画
                        if (IsTarget && (theRoi.shape == RoiShape.ROI_TRACE))
                        {
                            //myDisplay.DrawWidth = 5
                            //myDisplay.PSet (xc, yc), theColor
                            //myPen.Width = 5;
                            myPen.Width = HANDLE_WIDTH;
                            float lx = xc - myPen.Width / 2;
                            float ly = yc - myPen.Width / 2;
                            g.DrawEllipse(myPen, lx, ly, myPen.Width, myPen.Width);

                        }
                    }

                    //'最後に始点と終点を結ぶ
                    if (theRoi.shape == RoiShape.ROI_TRACE)
                    {
                        if (myDisplay != null)
                        {
                            myPen.Width = 1;
                            g.DrawLine(myPen, x1, y1, xc, yc);
                        }
                    }

                    break;

                #region '点ROI

                case RoiShape.ROI_POINT:
                    //'フォーム上の座標に変換
                    //xc = myDisplay.GetXOnForm(theRoi.p[1].X);
                    //yc = myDisplay.GetYOnForm(theRoi.p[1].Y);
                    xc = (int)frmScanImage.Instance.GetXOnForm(theRoi.p[1].X);
                    yc = (int)frmScanImage.Instance.GetYOnForm(theRoi.p[1].Y);

                    if (myDisplay != null)
                    {
                        g.DrawLine(myPen, xc - PointR, yc, xc + PointR, yc);    // '描画（横線）
                        g.DrawLine(myPen, xc, yc - PointR, xc, yc + PointR);    // '描画（縦線）
                    }

                    break;

                #endregion

            }

            return theColor;

        }


        //'********************************************************************************
        //'機    能  ：  長方形ROIのマウスハンドル表示
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        private void IndicateRectHandlePoints(int x1, int y1, int x2, int y2, Graphics g)
        {
            int wpx;
            int wpy;
            int remainder;  // 余り

            wpx =x1 + Math.DivRem((x2 - x1), 2, out remainder);
            wpy =y1 + Math.DivRem((y2 - y1), 2, out remainder);

            //Shapeｺﾝﾄﾛｰﾙ(MyControl)は使わない_2014/09/18(検S1)hata
            //if (MyControl[1] == null)
            //if (RoiCtrlType != 1)
            //Rev20.00 条件変更 by長野 2015/02/06
            if(RoiCtrlType != 1 || IndicateRectHandlePointsExFlg == 1)
            {
                Pen GreenPen = new Pen(Color.Lime);

                GreenPen.Width = HANDLE_WIDTH;
                g.DrawEllipse(GreenPen, new RectangleF((float)(x1 - HANDLE_WIDTH / 2.0), (float)(y1 - HANDLE_WIDTH / 2.0), GreenPen.Width, GreenPen.Width));
                g.DrawEllipse(GreenPen, new RectangleF((float)(x1 - HANDLE_WIDTH / 2.0), (float)(y2 - HANDLE_WIDTH / 2.0), GreenPen.Width, GreenPen.Width));
                g.DrawEllipse(GreenPen, new RectangleF((float)(x2 - HANDLE_WIDTH / 2.0), (float)(y1 - HANDLE_WIDTH / 2.0), GreenPen.Width, GreenPen.Width));
                g.DrawEllipse(GreenPen, new RectangleF((float)(x2 - HANDLE_WIDTH / 2.0), (float)(y2 - HANDLE_WIDTH / 2.0), GreenPen.Width, GreenPen.Width));
                g.DrawEllipse(GreenPen, new RectangleF((float)(wpx - HANDLE_WIDTH / 2.0), (float)(y1 - HANDLE_WIDTH / 2.0), GreenPen.Width, GreenPen.Width));
                g.DrawEllipse(GreenPen, new RectangleF((float)(x1 - HANDLE_WIDTH / 2.0), (float)(wpy - HANDLE_WIDTH / 2.0), GreenPen.Width, GreenPen.Width));
                g.DrawEllipse(GreenPen, new RectangleF((float)(x2 - HANDLE_WIDTH / 2.0), (float)(wpy - HANDLE_WIDTH / 2.0), GreenPen.Width, GreenPen.Width));
                g.DrawEllipse(GreenPen, new RectangleF((float)(wpx - HANDLE_WIDTH / 2.0), (float)(y2 - HANDLE_WIDTH / 2.0), GreenPen.Width, GreenPen.Width));
 

                //ROI中心に十字線を描く  'v16.30/v17.00 追加 by 山影 10-03-04
                //int backDrawWidth = HANDLE_WIDTH;
                //Pen GreenPen = new Pen(Color.Lime, 1);

                GreenPen.Width = 1;
                g.DrawLine(GreenPen, wpx - PointR, wpy, wpx + PointR, wpy);   // 描画（横線）
                g.DrawLine(GreenPen, wpx, wpy - PointR, wpx, wpy + PointR);   // 描画（縦線）
                //myDisplay.DrawWidth = backDrawWidth;

                // オブジェクト破棄
                GreenPen.Dispose();
                //LimeBrush.Dispose();
                //g.Dispose();
            }
        }

        //'********************************************************************************
        //'機    能  ：  ROI番号表示
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        private void IndicateRoiNo(int RI, Color col, Graphics g)
        {
	        // Dim theRect As RECT
	        int tx = 0;
	        int ty = 0;
            int remainder;  // 余り

            if (myRoi.GetUpperBound(0) < 2)
            {
                return;
            }

            //Shapeｺﾝﾄﾛｰﾙ(MyControl)は使わない_2014/09/18(検S1)hata
            //if (MyControl[1] != null)
            if (RoiCtrlType != 0)
            {
                return;
            }

	        switch (myRoi[RI].shape)
            {
		        //トレースROIの場合
		        case RoiShape.ROI_TRACE:    //v9.7変更（ROI_LINE追加）by 間々田 2004/11/01
		        case RoiShape.ROI_LINE:
			        //v9.7以下に変更 by 間々田 2004/11/01
			        GetLineRoiNoPosition(RI, ref tx, ref ty);
			        break;

		        default:

                    //高さを計測
                    //ty = myRoi[RI].area.top - myDisplay.TextHeight("X");
			        SizeF size = g.MeasureString("X",myForm.Font);  //MeasureStringは余白を含めた値のため、大き目になる。
                    //ty = myRoi[RI].area.top - (int)size.Height;
                    //2014/11/13hata キャストの修正
                    ty = Convert.ToInt32(myRoi[RI].area.top - size.Height);
                    
                    if (ty < 0)
                    {
				        ty = myRoi[RI].area.bottom;
			        }
			        if (myRoi[RI].shape == RoiShape.ROI_CIRC)
                    {
				        //サークルROIの場合
                        tx = Math.DivRem(myRoi[RI].area.right - myRoi[RI].area.left, 5, out remainder) * 2 + myRoi[RI].area.left;
			        }
                    else
                    {
                        //少し右に寄せる
                        //tx = myRoi[RI].area.left;
                        tx = myRoi[RI].area.left + 4;
			        }
			        break;
            }
            
            
            //フォーム上の座標位置に変換
            //myDisplay.CurrentX = myDisplay.GetXOnForm(tx);
            //myDisplay.CurrentY = myDisplay.GetYOnForm(ty);
            float cx = frmScanImage.Instance.GetXOnForm(tx);
            float cy = frmScanImage.Instance.GetXOnForm(ty);

	        //No表示
	        //myDisplay.ForeColor = col;
	        //myDisplay.Print(RI);
            SolidBrush myBrush = new SolidBrush(col);
            g.DrawString(Convert.ToString(RI), myForm.Font, myBrush, cx, cy);

            myBrush.Dispose();

        }
	
        //'********************************************************************************
        //'機    能  ：  トレースROI番号表示位置算出
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        private bool GetTraceRoiNoPosition(int RI, ref int xp, ref int yp)
        {
	        bool functionReturnValue = false;

	        int ofs = 0;
	        //    Dim p()  As Points             'v9.7削除 by 間々田 2004/11/01
	        int i = 0;

	        //トレース座標の取得
	        //    Call GetTracePoints(RI, p())   'v9.7削除 by 間々田 2004/11/01

            //Shapeｺﾝﾄﾛｰﾙ(MyControl)は使わない_2014/09/18(検S1)hata
            //if (MyControl[1] != null)
            if (RoiCtrlType != 0)
            {
                return functionReturnValue;
            }

	        //v9.7以下 p→.p に変更 by 間々田 2004/11/01
            if (myRoi[RI].p.GetUpperBound(0) > 0)
            {
		        //最も上のポイントの上側に表示
		        ofs = 1;
                for (i = 2; i <= myRoi[RI].p.GetUpperBound(0); i++)
                {
                    if (myRoi[RI].p[i].Y < myRoi[RI].p[ofs].Y)
                    {
                        ofs = i;
                    }
                }


                //高さを計測
                //yp = myRoi[RI].p[ofs].Y - myDisplay.TextHeight("X");
                Graphics g = myForm.CreateGraphics();
                SizeF size = g.MeasureString("X", myForm.Font);  //MeasureStringは余白を含めた値のため、大き目になる。
   
		        if (yp >= 0)
                {
                    //2014/11/13hata キャストの修正
                    //xp = myRoi[RI].p[ofs].X - (myDisplay.TextWidth("X") * 2);
                    xp = Convert.ToInt32(myRoi[RI].p[ofs].X - (size.Height * 2));   

			        functionReturnValue = true;
			        return functionReturnValue;
		        }

		        //最も左のポイントの左側に表示
		        ofs = 1;
                for (i = 2; i <= myRoi[RI].p.GetUpperBound(0); i++)
                {
                    if (myRoi[RI].p[i].X < myRoi[RI].p[ofs].X)
                    {
                        ofs = i;
                    }
		        }

                //xp = myRoi[RI].p[ofs].X - (myDisplay.TextWidth("X") * 2);
                //2014/11/13hata キャストの修正
                //xp = (int)(myRoi[RI].p[ofs].X - (size.Height * 2));
                xp = Convert.ToInt32(myRoi[RI].p[ofs].X - (size.Height * 2));

                yp = myRoi[RI].p[ofs].Y;
                

                functionReturnValue = true;
		        return functionReturnValue;
	        }   //v9.7追加 by 間々田 2004/11/01

	        functionReturnValue = false;
	        return functionReturnValue;
        }
	
        //'
        //'   線分に対応するラベルの位置を求めます v9.7追加 by 間々田 2004/11/01
        //'
        private void GetLineRoiNoPosition(int RI, ref int x, ref int y)
        {
	        int xLimit = 0;
	        int yLimit = 0;

            //Shapeｺﾝﾄﾛｰﾙ(MyControl)は使わない_2014/09/18(検S1)hata
            //if (MyControl[1] != null)
            if (RoiCtrlType != 0)
            {
                return;
            }

            int remainder;  // 余り

            //xLimit = Math.DivRem(myDisplay.PicWidth, myDisplay.Magnify, out remainder) - 1;
            //yLimit = Math.DivRem(myDisplay.PicHeight, myDisplay.Magnify, out remainder) - 1;
            xLimit = Math.DivRem(frmScanImage.Instance.PicWidth, frmScanImage.Instance.Magnify, out remainder) - 1;
            yLimit = Math.DivRem(frmScanImage.Instance.PicHeight, frmScanImage.Instance.Magnify, out remainder) - 1;

            x = Math.DivRem(myRoi[RI].p[1].X + myRoi[RI].p[2].X, 2, out remainder);
            y = Math.DivRem(myRoi[RI].p[1].Y + myRoi[RI].p[2].Y, 2, out remainder);

            if (Math.Abs(myRoi[RI].p[2].X - myRoi[RI].p[1].X) < Math.Abs(myRoi[RI].p[2].Y - myRoi[RI].p[1].Y))
            {
		        x = (modLibrary.InRange(x + 10, 0, xLimit) ? x + 10 : x - 10);
	        }
            else
            {
                y = (modLibrary.InRange(y + 10, 0, yLimit) ? y + 10 : y - 10);
	        }
        }
	
        //'********************************************************************************
        //'機    能  ： 指定座標位置のROIの判定
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        private int GetSelectRoi(int X, int Y)
        {
	        int functionReturnValue = 0;

	        int RI = 0;
	        int xc = 0;
	        int yc = 0;
	        int r = 0;
	        int x1 = 0;
	        int y1 = 0;
	        int x2 = 0;
	        int y2 = 0;
            int remainder;  // 余り

	        for (RI = NumOfRoi; RI >= 1; RI--)
            {
		        var _with1 = myRoi[RI];

                if (myRoi[RI].Enabled)
                {
                    x1 = myRoi[RI].area.left;
                    x2 = myRoi[RI].area.right;
                    y1 = myRoi[RI].area.top;
                    y2 = myRoi[RI].area.bottom;
                    r = Math.DivRem((x2 - x1), 2, out remainder);
			        xc = x1 + r;
			        yc = y1 + r;

			        functionReturnValue = RI;

                    switch (myRoi[RI].shape)
                    {
				        //円？
				        case RoiShape.ROI_CIRC:
                            if (Figure.PointOnCircle(X, Y, xc, yc, r))
                            {
                                return functionReturnValue;
                            }
					        break;

				        //長方形？
				        case RoiShape.ROI_RECT:
                            if (Figure.PointOnRectangle(X, Y, x1, y1, x2, y2))
                            {
                                return functionReturnValue;
                            }
					        break;

				        //トレース？
				        case RoiShape.ROI_TRACE:
                            if (Figure.PointOnTrace(X, Y, myRoi[RI].p))
                            {
                                return functionReturnValue;
                            }
					        break;

				        //正方形？
				        case RoiShape.ROI_SQR:
                            if (Figure.PointOnRectangle(X, Y, x1, y1, x2, y2))
                            {
                                return functionReturnValue;
                            }
					        break;

				        //線？
				        case RoiShape.ROI_LINE:
                            if (Figure.PointOnLine(X, Y, myRoi[RI].p[1].X, myRoi[RI].p[1].Y, myRoi[RI].p[2].X, myRoi[RI].p[2].Y))
                            {
                                return functionReturnValue;
                            }
					        break;

				        //点？
				        case RoiShape.ROI_POINT:
                            if (Figure.IsNear(X, Y, myRoi[RI].p[1].X, myRoi[RI].p[1].Y))
                            {
                                return functionReturnValue;
                            }
					        break;

                        default:
                            break;
			        }
		        }
	        }

	        functionReturnValue = 0;
	        return functionReturnValue;
        }
	
        //'********************************************************************************
        //'機    能  ：  マウスダウンイベントのコールバック。
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  Button           [I/ ] Integer   マウスのボタン
        //'              Shift            [I/ ] Integer   シフトキーの状態
        //'              x                [I/ ] Integer   ｘ座標
        //'              y                [I/ ] Integer   ｙ座標
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        //public void MouseDown(MouseButtons Button, int Shift, int X, int Y)
        public void MouseDown(MouseButtons Button, Keys Shift, int X, int Y)
        {
	        //仮想ROIが表示されている場合、何もしない
            if (vRoi.shape != RoiShape.NO_ROI)
            {
                return;
            }

	        switch (Button)
            {
                //左マウスボタン
                case MouseButtons.Left:
			        if (!StartEditShape(X, Y))
                    {
                        switch (MakeShape)
                        {
					        case RoiShape.NO_ROI:
						        StartMakeShape(RoiShape.ROI_RECT, X, Y);
						        break;

					        //トレース
					        case RoiShape.ROI_TRACE:
                                break;

					        default:
                                if (NumOfRoi < myRoi.GetUpperBound(0))
                                {
							        StartMakeShape(MakeShape, X, Y);
						        }
						        break;
				        }
			        }
			        break;

                //右マウスボタン
                case MouseButtons.Right:
			        if (myForm != null)
                    {
				        //ポップアップメニューを表示
                        if (myForm.Equals(frmScanImage.Instance))
                        {
                            ContextMenuStrip mnuPopUp = frmScanImage.Instance.mnuPopUp;
                            mnuPopUp.Show(myForm, new System.Drawing.Point(X, Y));
                        }
                        else if (myForm.Equals(frmExObsCam.Instance))//Rev23.30 追加 by長野 2016/02/08
                        {
                            ContextMenuStrip mnuPopup = frmExObsCam.Instance.mnuPopUp;
                            mnuPopup.Show(myForm, new System.Drawing.Point(X, Y));
                        }
			        }
			        break;

                default:
                    break;
            }
        }
	
        //'********************************************************************************
        //'機    能  ：  マウスムーブイベントのコールバック。
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  x                [I/ ] Integer   ｘ座標
        //'              y                [I/ ] Integer   ｙ座標
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        public void MouseMove(int X, int Y)
        {
	        if (vRoi.shape == RoiShape.NO_ROI)
            {
                //マウスカーソル形状を設定する
		        myForm.Cursor = GetMousePointer(GetHandlePoint(X, Y));
            }
            else
            {
		        //仮想ROIの編集
		        EditVirtualRoi(X, Y);
	        }
        }
	
        //'********************************************************************************
        //'機    能  ：  マウスアップイベントのコールバック。
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  Button           [I/ ] Integer   マウスのボタン
        //'              Shift            [I/ ] Integer   シフトキーの状態
        //'              x                [I/ ] Integer   ｘ座標
        //'              y                [I/ ] Integer   ｙ座標
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        //public void MouseUp(MouseButtons Button, int Shift, int X, int Y, bool DoubleClick = false)
        public void MouseUp(MouseButtons Button, Keys Shift, int X, int Y, bool DoubleClick = false)
        {
	        int N = 0;
	        int RI = 0;

            //追加2014/07/14(検S1)hata
            bool bShift = false;
            if ((Shift & Keys.Shift) == Keys.Shift)
            {
                //シフトキーを押された。
                bShift = true;
            }

            #region 右クリックされた場合

	        if (Button == MouseButtons.Right)
            {
		        //トレースROI作成中の場合
                if (vRoi.shape == (RoiShape)Enum.ToObject(typeof(RoiShape), -1 * (int)RoiShape.ROI_TRACE))
                {
                    N = vRoi.p.GetUpperBound(0);

			        if (N > 0)
                    {
                        //変更2014/07/14(検S1)hata
				        //DrawRoi(vRoi, false);
                        //roi設定中
                        myRoiFlg = 1;
                        myForm.Refresh();

				        Array.Resize(ref vRoi.p, N);
                        EditPoint = (ShapeHandleType)Enum.ToObject(typeof(ShapeHandleType), N - 1);

                        //変更2014/07/14(検S1)hata
                        //DrawRoi(vRoi, false);
                        //roi設定中
                        myRoiFlg = 1;
                        myForm.Refresh();

                        if ((int)EditPoint > 1) { return; }
			        }
		        }
		        vRoi.shape = RoiShape.NO_ROI;
            
            #endregion
	        
            //左クリックされた場合
	        } else {
                //円・長方形・正方形・線を描画中の場合
                if ((vRoi.shape == (RoiShape)Enum.ToObject(typeof(RoiShape), -1 * (int)RoiShape.ROI_CIRC)) ||
                    (vRoi.shape == (RoiShape)Enum.ToObject(typeof(RoiShape), -1 * (int)RoiShape.ROI_RECT)) ||
                    (vRoi.shape == (RoiShape)Enum.ToObject(typeof(RoiShape), -1 * (int)RoiShape.ROI_SQR)) ||
                    (vRoi.shape == (RoiShape)Enum.ToObject(typeof(RoiShape), -1 * (int)RoiShape.ROI_LINE)))
                {
                        //ROIとして有効でない
				        if (!IsAvailableRoi(ref vRoi))
                        {
					        vRoi.shape = RoiShape.NO_ROI;

                            //Roiのクリック処理
                            //変更2014/07/14(検S1)hata
                            //SelectRoi(GetSelectRoi(X, Y), (Shift & VB6.ShiftConstants.ShiftMask));
                            SelectRoi(GetSelectRoi(X, Y), bShift);
                            
				        }
                        else if (MakeShape == RoiShape.NO_ROI)
                        {
					        vRoi.shape = RoiShape.NO_ROI;

					        SelectRoiInRectangle(vRoi.area.left, vRoi.area.top, vRoi.area.right, vRoi.area.bottom);
				        }
                        else
                        {
					        AddRoiData();
				        }
                }
                //トレースを描画中
                else if (vRoi.shape == (RoiShape)Enum.ToObject(typeof(RoiShape), -1 * (int)RoiShape.ROI_TRACE))
                {
                    N = vRoi.p.GetUpperBound(0);

				    if (DoubleClick)
                    {
					    if (N > 4)
                        {
						    Array.Resize(ref vRoi.p, N - 1);
						    AddRoiData();

                            //追加2014/07/25(検S1)hata
                            //ここでareaを更新する
                            UpdateTraceArea(ref vRoi);
                            myRoi[myTargetRoi].area = vRoi.area;

                        }
				    }
                    else if (N < MaxTracePoints + 2)
                    {
					    vRoi.p[N].X = X;
					    vRoi.p[N].Y = Y;
					    Array.Resize(ref vRoi.p, N + 2);
                        EditPoint = (ShapeHandleType)Enum.ToObject(typeof(ShapeHandleType), N + 1); //EditPoint = N + 1;
					    vRoi.p[N + 1].X = X;
					    vRoi.p[N + 1].Y = Y;
				    }
                }
                else
                {
                    //図形修正時
                    if (vRoi.shape > 0)
                    {
				        if (IsAvailableRoi(ref vRoi))
                        {
                            myRoi[myTargetRoi] = vRoi;
					        vRoi.shape = RoiShape.NO_ROI;

                            //変更2014/07/14(検S1)hata
                            //IndicateRoi(g);
                            //roi表示
                            myRoiFlg = 2;
                            myForm.Refresh();

					        //イベント生成
					        if (Changed != null)
                            {
						        Changed(GetRoiInfo(myTargetRoi));
					        }
				        }
                    }
                    //その他
                    else
                    {
				        RI = GetSelectRoi(X, Y);
				        if ((RI == 0) & (MakeShape == RoiShape.ROI_TRACE))
                        {
                            //何も選択していなければトレースの一点目
					        StartMakeShape(MakeShape, X, Y);
				        }
                        else
                        {   
                            //Roiのクリック処理
                            //テスト追加2014/07/14hata
                            SelectRoi(RI, bShift);

                        }
                    }
                }
	        }
        }

        //'********************************************************************************
        //'機    能  ：  Roiの選択操作
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V9.7   04/11/01  (SI4)間々田     新規作成
        //'********************************************************************************
        public void SelectRoi(int RI, bool MultiSelect = false)
        {
	        int i = 0;

            //複数選択が許可されていない場合、すべての線分を非選択にする
            if (!MultiSelect)
            {
                for (i = 1; i <= NumOfRoi; i++)
                {
                    myRoi[i].Selected = false;
                }
            }

            //ROIを選択
            if (RI > 0)
            {
                myRoi[RI].Selected = true;
            }

            //ROIの表示
            //変更2014/07/14(検S1)hata
            //IndicateRoi(g);
            //roi表示
            myRoiFlg = 2;
            myForm.Refresh();

            //イベント生成
            if (Changed != null)
            {
                Changed(GetRoiInfo(myTargetRoi));
            }
        }

        //'********************************************************************************
        //'機    能  ：  図形描画開始
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        private void StartMakeShape(RoiShape theShape, int x, int y)
        {
            //追加2014/07/30(検S1)hata
            //vRoiを初期化する
            //if (theShape == RoiShape.ROI_LINE) vRoi = new RoiInfoType();
            vRoi = new RoiInfoType();
            
	        switch (theShape)
            {
		        case RoiShape.ROI_LINE:
		        case RoiShape.ROI_TRACE:
                    Array.Resize(ref vRoi.p, 3);
                    vRoi.p[1].X = x;
                    vRoi.p[1].Y = y;
                    vRoi.p[2].X = x;
                    vRoi.p[2].Y = y;
                    EditPoint = (ShapeHandleType)Enum.ToObject(typeof(ShapeHandleType), 2);
			        break;

		        default:
                    Array.Resize(ref vRoi.p, 1);
                    vRoi.area.left = x;
                    vRoi.area.top = y;
                    vRoi.area.right = x;
                    vRoi.area.bottom = y;
			        EditPoint = ShapeHandleType.RIGHT_LOWER;
			        break;
	        }

            vRoi.shape = (RoiShape)Enum.ToObject(typeof(RoiShape), -1 * (int)theShape); //vRoi.shape = -theShape;
            vRoi.Sizable = true;
            vRoi.Enabled = true;
            vRoi.Selected = false;


            //削除2014/07/14(検S1)hata
            //スタート時の描画は不要。
            //DrawRoi(vRoi, false);
 
        }
	
        //'********************************************************************************
        //'機    能  ：  図形編集（移動、変形）開始
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        private bool StartEditShape(int x, int y)
        {
	        //戻り値初期化
            bool functionReturnValue = false;

	        EditPoint = (ShapeHandleType)GetHandlePoint(x, y);
            if (EditPoint == ShapeHandleType.NO_EDIT_POINT)
            {
                return functionReturnValue;
            }

	        //現在の座標を保存
	        mp.X = x;
	        mp.Y = y;

	        //仮想ROIを作成
	        vRoi = myRoi[myTargetRoi];

            //変更2014/07/22(検S1)hata
            //DrawRoi(vRoi, false);
            //roi編集中
            myRoiFlg = 3;
            myForm.Refresh();

	        functionReturnValue = true;
	        return functionReturnValue;
        }
	
        //'
        //'   ROIの幅・高さを算出する             'v9.7新規作成 by 間々田 2004/11/01
        //'
        //'   mode =  1:高さを幅にあわせる
        //'           2:幅を高さにあわせる
        //'           3:絶対値の小さい方にあわせる
        //'          他:何もしない
        //'
        private void GetWidthAndHeight(int inWidth, int inHeight, ref int outWidth, ref int outHeight, int Mode)
        {
	        //幅・高さを２の倍数とする
            int remainder;
            outWidth = Math.DivRem(inWidth, 2, out remainder) * 2;
            outHeight = Math.DivRem(inHeight, 2, out remainder) * 2;

	        switch (Mode)
            {
		        case 1:
			        outHeight = Math.Abs(outWidth);
			        break;

		        case 2:
			        outWidth = Math.Abs(outHeight);
			        break;

		        case 3:
			        if (Math.Abs(outWidth) > Math.Abs(outHeight))
                    {
				        outWidth = Math.Abs(outHeight) * Math.Sign(outWidth);
			        }
                    else
                    {
				        outHeight = Math.Abs(outWidth) * Math.Sign(outHeight);
			        }
			        break;

                default:
                    break;
	        }
        }
	
        //'********************************************************************************
        //'機    能  ：  仮想ROIの編集処理
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        private void EditVirtualRoi(int X, int Y)
        {
	        int theWidth = 0;
	        int theHeight = 0;
	        bool IsSquare = false;
	        int dX = 0;
	        int dY = 0;
	        int i = 0;
	        int xLimit = 0;
	        int yLimit = 0;
            int remainder = 0;  // 余り

            //Ipc32v5.RECT DispRect = default(Ipc32v5.RECT);
            //Rectangle DispRect;// = new Rectangle();
            Winapi.RECT DispRect = default(Winapi.RECT);

            //Shapeｺﾝﾄﾛｰﾙ(MyControl)は使わない_2014/09/18(検S1)hata
	        //if (MyControl[1] == null)
            if (RoiCtrlType == 0)
            {
                DispRect.left = 0;
                DispRect.top = 0;

                DispRect.right = Math.DivRem(frmScanImage.Instance.PicWidth, frmScanImage.Instance.Magnify, out remainder) - 1;
                DispRect.bottom = Math.DivRem(frmScanImage.Instance.PicHeight, frmScanImage.Instance.Magnify, out remainder) - 1;
 
            }
            else
            {
		        DispRect.left = 0;
		        DispRect.top = 0;
		        
                //DispRect.right = myDisplay.Width - 1;
		        //DispRect.bottom = myDisplay.Height - 1;
                //if (myDisplay.Equals(myForm))
                //{
                //    DispRect.right = myForm.Width - 1;
                //    DispRect.bottom = myForm.Height - 1;
                //}
                //else if (myDisplay.Equals(frmTransImage.Instance.ctlTransImage))
                //{
                //    DispRect.right = frmTransImage.Instance.ctlTransImage.Width - 1;
                //    DispRect.bottom = frmTransImage.Instance.ctlTransImage.Height - 1;
                //}
                
                //Rev23.30 条件変更 by長野 2016/02/08
                if (myDisplay.Equals(frmExObsCam.Instance))
                {
                    //DispRect.right = 1024;
                    //DispRect.bottom = 1024;
                    //Rev26.00 change by chouno 2017/11/08
                    DispRect.right = 1023;
                    DispRect.bottom = 1023;
                }
                else if (myDisplay.Equals(frmTransImage.Instance.ctlTransImage))
                {
                    DispRect.right = frmTransImage.Instance.ctlTransImage.Width - 1;
                    DispRect.bottom = frmTransImage.Instance.ctlTransImage.Height - 1;
                }
                else if (myDisplay.Equals(myForm))
                {
                    DispRect.right = myForm.Width - 1;
                    DispRect.bottom = myForm.Height - 1;
                }

            }

	        xLimit = DispRect.right;
	        yLimit = DispRect.bottom;

            RoiShape vRoiShape = (RoiShape)Math.Abs((Decimal)vRoi.shape);
            switch (vRoiShape)
            {
		        case RoiShape.ROI_CIRC:
		        case RoiShape.ROI_SQR:
			        IsSquare = true;
			        break;

		        default:
			        IsSquare = false;
			        break;
	        }

            //削除2014/07/14(検S1)hata
            //ﾉXorPenは止める。二度書きしない。
            //DrawRoi(vRoi, false);

	        switch (EditPoint)
            {
		        case ShapeHandleType.LEFT_UPPER:
			        //幅・高さの取得
                    GetWidthAndHeight(vRoi.area.right - X, vRoi.area.bottom - Y, ref theWidth, ref theHeight, (IsSquare ? 3 : 0));

                    vRoi.area.left = vRoi.area.right - theWidth;
                    vRoi.area.top = vRoi.area.bottom - theHeight;
                    break;

		        case ShapeHandleType.RIGHT_UPPER:
			        //幅・高さの取得
                    GetWidthAndHeight(X - vRoi.area.left, vRoi.area.bottom - Y, ref theWidth, ref theHeight, (IsSquare ? 3 : 0));

                    vRoi.area.right = vRoi.area.left + theWidth;
                    vRoi.area.top = vRoi.area.bottom - theHeight;
			        break;

		        case ShapeHandleType.LEFT_LOWER:
			        //幅・高さの取得
                    GetWidthAndHeight(vRoi.area.right - X, Y - vRoi.area.top, ref theWidth, ref theHeight, (IsSquare ? 3 : 0));

                    vRoi.area.left = vRoi.area.right - theWidth;
                    vRoi.area.bottom = vRoi.area.top + theHeight;
                    break;

		        case ShapeHandleType.RIGHT_LOWER:
			        //幅・高さの取得
                    GetWidthAndHeight(X - vRoi.area.left, Y - vRoi.area.top, ref theWidth, ref theHeight, (IsSquare ? 3 : 0));

                    vRoi.area.right = vRoi.area.left + theWidth;
                    vRoi.area.bottom = vRoi.area.top + theHeight;
                     break;

		        case ShapeHandleType.MIDDLE_UPPER:
                    dX = (vRoi.area.top - Y) / 2;

			        //正方形・円の場合、移動距離を補正
			        if (IsSquare)
                    {
                        dX = modLibrary.CorrectInRange(dX, vRoi.area.left - xLimit, vRoi.area.left);
                        dX = modLibrary.CorrectInRange(dX, -vRoi.area.right, xLimit - vRoi.area.right);
			        }

			        //幅・高さの取得
                    GetWidthAndHeight(vRoi.area.right - vRoi.area.left, vRoi.area.bottom - vRoi.area.top + dX * 2, ref theWidth, ref theHeight, (IsSquare ? 2 : 0));

                    vRoi.area.top = vRoi.area.bottom - theHeight;
                    vRoi.area.left = (vRoi.area.left + vRoi.area.right) / 2 - theWidth / 2;
                    vRoi.area.right = vRoi.area.left + theWidth;
			        break;

		        case ShapeHandleType.RIGHT_MIDDLE:
                    dY = (X - vRoi.area.right) / 2;

			        //正方形・円の場合、移動距離を補正
			        if (IsSquare)
                    {
                        dY = modLibrary.CorrectInRange(dY, vRoi.area.top - yLimit, vRoi.area.top);
                        dY = modLibrary.CorrectInRange(dY, -vRoi.area.bottom, yLimit - vRoi.area.bottom);
			        }

			        //幅・高さの取得
                    GetWidthAndHeight(vRoi.area.right - vRoi.area.left + dY * 2, vRoi.area.bottom - vRoi.area.top, ref theWidth, ref theHeight, (IsSquare ? 1 : 0));

                    vRoi.area.right = vRoi.area.left + theWidth;
                    vRoi.area.top = (vRoi.area.top + vRoi.area.bottom) / 2 - theHeight / 2;
                    vRoi.area.bottom = vRoi.area.top + theHeight;
                    break;

		        case ShapeHandleType.MIDDLE_LOWER:
                    dX = (Y - vRoi.area.bottom) / 2;

			        //正方形・円の場合、移動距離を補正
			        if (IsSquare)
                    {
                        dX = modLibrary.CorrectInRange(dX, vRoi.area.left - xLimit, vRoi.area.left);
                        dX = modLibrary.CorrectInRange(dX, -vRoi.area.right, xLimit - vRoi.area.right);
			        }

			        //幅・高さの取得
                    GetWidthAndHeight(vRoi.area.right - vRoi.area.left, vRoi.area.bottom - vRoi.area.top + dX * 2, ref theWidth, ref theHeight, (IsSquare ? 2 : 0));

                    vRoi.area.bottom = vRoi.area.top + theHeight;
                    vRoi.area.left = (vRoi.area.left + vRoi.area.right) / 2 - theWidth / 2;
                    vRoi.area.right = vRoi.area.left + theWidth;
			        break;

		        case ShapeHandleType.LEFT_MIDDLE:
                    dY = (vRoi.area.left - X) / 2;

			        //正方形・円の場合、移動距離を補正
			        if (IsSquare)
                    {
                        dY = modLibrary.CorrectInRange(dY, vRoi.area.top - yLimit, vRoi.area.top);
                        dY = modLibrary.CorrectInRange(dY, -vRoi.area.bottom, yLimit - vRoi.area.bottom);
			        }

			        //幅・高さの取得
                    GetWidthAndHeight(vRoi.area.right - vRoi.area.left + dY * 2, vRoi.area.bottom - vRoi.area.top, ref theWidth, ref theHeight, (IsSquare ? 1 : 0));

                    vRoi.area.left = vRoi.area.right - theWidth;
                    vRoi.area.top = (vRoi.area.top + vRoi.area.bottom) / 2 - theHeight / 2;
                    vRoi.area.bottom = vRoi.area.top + theHeight;
                    break;

		        //移動時
		        case ShapeHandleType.ON_LINE:
			        //移動距離を算出（補正付き）
                    dX = modLibrary.CorrectInRange(X - mp.X, -vRoi.area.left, xLimit - vRoi.area.right);
                    dY = modLibrary.CorrectInRange(Y - mp.Y, -vRoi.area.top, yLimit - vRoi.area.bottom);

                    vRoi.area.left = vRoi.area.left + dX;
                    vRoi.area.top = vRoi.area.top + dY;
                    vRoi.area.right = vRoi.area.right + dX;
                    vRoi.area.bottom = vRoi.area.bottom + dY;
                    
			        for (i = 1; i <= vRoi.p.GetUpperBound(0); i++)
                    {
				        vRoi.p[i].X = vRoi.p[i].X + dX;
				        vRoi.p[i].Y = vRoi.p[i].Y + dY;
			        }

			        mp.X = X;
			        mp.Y = Y;
			        break;

                default:
                    if (((int)EditPoint >= 1) && ((int)EditPoint <= MaxTracePoints))
                    {
                        vRoi.p[(int)EditPoint].X = X;
                        vRoi.p[(int)EditPoint].Y = Y;
                    }
                    break;
	        }

	        //座標・ハンドルの調整
	        if (EditPoint >= ShapeHandleType.LEFT_UPPER)
            {
                if (vRoi.area.left > vRoi.area.right)
                {
                    modLibrary.Swap(ref vRoi.area.left, ref vRoi.area.right);    //座標の反転
                    EditPoint = (ShapeHandleType)((int)EditPoint ^ 1);          //ハンドルの反転
                }
                if (vRoi.area.top > vRoi.area.bottom)
                {
                    modLibrary.Swap(ref vRoi.area.top, ref  vRoi.area.bottom);   //座標の反転
                    EditPoint = (ShapeHandleType)((int)EditPoint ^ 2);          //ハンドルの反転
                }
	        }

            //変更2014/07/22(検S1)hata
            //DrawRoi(vRoi, false);
            //roi設定中（変更中）
            myRoiFlg = 3;
            myForm.Refresh();
        
        }

        //'********************************************************************************
        //'機    能  ：  ハンドルポイントに対するマウスカーソル形状を返す
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        private Cursor GetMousePointer(int theHandle)
        {
	        Cursor functionReturnValue = null;

	        switch (theHandle)
            {
		        case (int)ShapeHandleType.ON_LINE:
			        functionReturnValue = Cursors.SizeAll; //上下左右（移動用）
			        break;

                case (int)ShapeHandleType.LEFT_UPPER:
                case (int)ShapeHandleType.RIGHT_LOWER:
			        functionReturnValue = Cursors.SizeNWSE;
			        break;

                case (int)ShapeHandleType.MIDDLE_UPPER:
                case (int)ShapeHandleType.MIDDLE_LOWER:
			        functionReturnValue = Cursors.SizeNS;
			        break;

                case (int)ShapeHandleType.RIGHT_UPPER:
                case (int)ShapeHandleType.LEFT_LOWER:
			        functionReturnValue = Cursors.SizeNESW;
			        break;

                case (int)ShapeHandleType.RIGHT_MIDDLE:
                case (int)ShapeHandleType.LEFT_MIDDLE:
			        functionReturnValue = Cursors.SizeWE;
			        break;

		        default:
                    if (theHandle >= 1 && theHandle <= MaxTracePoints)
                    {
                        functionReturnValue = Cursors.UpArrow; //↑（トレース/線用）
                    }
                    else
                    {
                        functionReturnValue = Cursors.Arrow;
                    }
			        break;
	        }

	        return functionReturnValue;
        }
	
        //'********************************************************************************
        //'機    能  ：  座標のカレント図形上の位置（ハンドル）を返す
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        private int GetHandlePoint(int x, int y)
        {
	        //戻り値初期化
	        int functionReturnValue = (int)ShapeHandleType.NO_EDIT_POINT;

	        switch (GetRoiShape(myTargetRoi))
            {
                //v9.7 ROI_POINT追加 by 間々田 2004/11/01
                case RoiShape.ROI_CIRC:
		        case RoiShape.ROI_RECT:
		        case RoiShape.ROI_SQR:
		        case RoiShape.ROI_POINT:
			        functionReturnValue = (int)GetRectHandlePoint(x, y);
			        break;

                //v9.7 ROI_LINE追加  by 間々田 2004/11/01
                case RoiShape.ROI_TRACE: 
		        case RoiShape.ROI_LINE:
			        functionReturnValue = GetTraceHandlePoint(x, y);
			        break;

                default:
                    break;
	        }

	        return functionReturnValue;
        }
	
        //'********************************************************************************
        //'機    能  ：  図形のハンドルの操作
        //'              指定座標(x,y)が図形のどの部分をさしているかを得る
        //'              座標の長方形上の位置（ハンドル）
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        //'v9.7以下に変更 by 間々田 2004/11/01
        private ShapeHandleType GetRectHandlePoint(int X, int Y)
        {
	        //   LEFT_UPPER(.left,.top) -------- MIDDLE_UPPER(mpx,.top) -------- RIGHT_UPPER(.right,.top)
	        //       |                                                           |
	        //       |                                                           |
	        //   LEFT_MIDDLE(.left,mpy)                                          RIGHT_MIDDLE(.right,mpy)
	        //       |                                                           |
	        //       |                                                           |
	        //   LEFT_LOWER(.left,.bottom) ----- MIDDLE_LOWER(mpx,.bottom) ----- RIGHT_LOWER(.right,.bottom)

            ShapeHandleType functionReturnValue = default(ShapeHandleType);

            //modLibrary.Points p = default(modLibrary.Points); // 中心の座標
            Point p = new Point(); // 中心の座標
	        
            bool IsNormal = false;      // 通常サイズのROIかどうか
            int remainder;              // 余り

            p.X = myRoi[myTargetRoi].area.left + Math.DivRem((myRoi[myTargetRoi].area.right - myRoi[myTargetRoi].area.left), 2, out remainder);
            p.Y = myRoi[myTargetRoi].area.top + Math.DivRem((myRoi[myTargetRoi].area.bottom - myRoi[myTargetRoi].area.top), 2, out remainder);
            IsNormal = ((myRoi[myTargetRoi].area.right - myRoi[myTargetRoi].area.left) > 41) | ((myRoi[myTargetRoi].area.bottom - myRoi[myTargetRoi].area.top) > 41);

            if ((Figure.IsNear(X, Y, myRoi[myTargetRoi].area.left, myRoi[myTargetRoi].area.top) & myRoi[myTargetRoi].Sizable))
            {
		        functionReturnValue = ShapeHandleType.LEFT_UPPER;
	        }
            else if ((Figure.IsNear(X, Y, p.X, myRoi[myTargetRoi].area.top) & myRoi[myTargetRoi].Sizable))
            {
		        functionReturnValue = ShapeHandleType.MIDDLE_UPPER;
	        }
            else if ((Figure.IsNear(X, Y, myRoi[myTargetRoi].area.right, myRoi[myTargetRoi].area.top) & myRoi[myTargetRoi].Sizable))
            {
		        functionReturnValue = ShapeHandleType.RIGHT_UPPER;
	        }
            else if ((Figure.IsNear(X, Y, myRoi[myTargetRoi].area.right, p.Y) & myRoi[myTargetRoi].Sizable))
            {
		        functionReturnValue = ShapeHandleType.RIGHT_MIDDLE;
	        }
            else if ((Figure.IsNear(X, Y, myRoi[myTargetRoi].area.right, myRoi[myTargetRoi].area.bottom) & myRoi[myTargetRoi].Sizable))
            {
                //小さいROIの時移動用(ON_LINE)とする
		        functionReturnValue = ShapeHandleType.RIGHT_LOWER;
	        }
            else if ((Figure.IsNear(X, Y, p.X, myRoi[myTargetRoi].area.bottom) & IsNormal & myRoi[myTargetRoi].Sizable))
            {
                //小さいROIの時移動用(ON_LINE)とする
		        functionReturnValue = ShapeHandleType.MIDDLE_LOWER;
	        }
            else if ((Figure.IsNear(X, Y, myRoi[myTargetRoi].area.left, myRoi[myTargetRoi].area.bottom) & IsNormal & myRoi[myTargetRoi].Sizable))
            {
                //小さいROIの時移動用(ON_LINE)とする
		        functionReturnValue = ShapeHandleType.LEFT_LOWER;
	        }
            else if ((Figure.IsNear(X, Y, myRoi[myTargetRoi].area.left, p.Y) & IsNormal & myRoi[myTargetRoi].Sizable))
            {
		        functionReturnValue = ShapeHandleType.LEFT_MIDDLE;
	        }
            else if ((Figure.PointOnRectangle(X, Y, myRoi[myTargetRoi].area.left, myRoi[myTargetRoi].area.top, myRoi[myTargetRoi].area.right, myRoi[myTargetRoi].area.bottom)))
            {
		        functionReturnValue = ShapeHandleType.ON_LINE;
	        }
            else
            {
		        functionReturnValue = ShapeHandleType.NO_EDIT_POINT;
	        }

	        return functionReturnValue;
        }
	
        //'********************************************************************************
        //'機    能  ：  トレース/線上の位置（ハンドル）を返す
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  なし
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        private int GetTraceHandlePoint(int X, int Y)
        {
	        int functionReturnValue = 0;

	        int i = 0;
	        int N = 0;

            if (myRoi[myTargetRoi].Sizable)
            {
                for (i = 1; i <= myRoi[myTargetRoi].p.GetUpperBound(0); i++)
                {
                    if (Figure.IsNear(X, Y, myRoi[myTargetRoi].p[i].X, myRoi[myTargetRoi].p[i].Y))
                    {
				        functionReturnValue = i;
				        return functionReturnValue;
			        }
		        }
	        }

            for (i = 1; i <= myRoi[myTargetRoi].p.GetUpperBound(0); i++)
            {
                N = (i == myRoi[myTargetRoi].p.GetUpperBound(0) ? 1 : i + 1);

                if (Figure.PointOnLine(X, Y, myRoi[myTargetRoi].p[i].X, myRoi[myTargetRoi].p[i].Y, myRoi[myTargetRoi].p[N].X, myRoi[myTargetRoi].p[N].Y))
                {
			        functionReturnValue = (int)ShapeHandleType.ON_LINE;
			        return functionReturnValue;
		        }
	        }

	        functionReturnValue = (int)ShapeHandleType.NO_EDIT_POINT;
	        return functionReturnValue;
        }
	
        //'********************************************************************************
        //'機    能  ：  ROIが有効かどうか調べます
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  ROI データ操作
        //'
        //'履    歴  ：  V9.7   04/11/01  (SI4)間々田     新規作成
        //'********************************************************************************
        private bool IsAvailableRoi(ref RoiInfoType theRoi)
        {
	        //戻り値初期化
	        bool functionReturnValue = false;

            RoiShape roi = (RoiShape)Math.Abs((decimal)theRoi.shape);
            
            switch (roi)
            {
		        // 円・長方形・正方形
		        case RoiShape.ROI_CIRC:
		        case RoiShape.ROI_RECT:
		        case RoiShape.ROI_SQR:
                    if (!(theRoi.area.left < theRoi.area.right))
                    {
                        return functionReturnValue;
                    }
                    if (!(theRoi.area.top < theRoi.area.bottom))
                    {
                        return functionReturnValue;
                    }
			        break;

		        // 線
		        case RoiShape.ROI_LINE:
                    if ((theRoi.p[1].X == theRoi.p[2].X) && (theRoi.p[1].Y == theRoi.p[2].Y))
                    {
                        return functionReturnValue;
                    }
                    UpdateTraceArea(ref theRoi);
			        break;

		        // トレース
		        case RoiShape.ROI_TRACE:
                    if (theRoi.p.GetUpperBound(0) < 3)
                    {
                        return functionReturnValue;
                    }
			        UpdateTraceArea(ref theRoi);
			        break;

                default:
                    break;
	        }

	        //戻り値初期化
	        functionReturnValue = true;
	        return functionReturnValue;
        }

        //'********************************************************************************
        //'機    能  ：  ROIのデータベースへの追加
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：  ROI データ操作
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        private bool AddRoiData()
        {
 
            //マイナスの値を入れている。作成中を意味している。
            //AddNewRoi(-vRoi.shape);
            AddNewRoi((RoiShape)Enum.ToObject(typeof(RoiShape), -1 * (int)vRoi.shape));

 	        myRoi[NumOfRoi] = vRoi;

            //myRoi[NumOfRoi].area.left = vRoi.area.left;
            //myRoi[NumOfRoi].area.top = vRoi.area.top;
            //myRoi[NumOfRoi].area.right = vRoi.area.right;
            //myRoi[NumOfRoi].area.bottom = vRoi.area.bottom;
            //myRoi[NumOfRoi].Enabled = vRoi.Enabled;
            //myRoi[NumOfRoi].Selected = vRoi.Selected;
            //myRoi[NumOfRoi].shape = vRoi.shape;
            //myRoi[NumOfRoi].Sizable = vRoi.Sizable;

            //int len = vRoi.p.Length;
            //Array.Resize(ref  myRoi[NumOfRoi].p, len);

            //for (int i = 1; i <= myRoi[NumOfRoi].p.GetUpperBound(0); i++)
            //{
            //    myRoi[NumOfRoi].p[i].X = vRoi.p[i].X;
            //    myRoi[NumOfRoi].p[i].Y = vRoi.p[i].Y;
            //}

            //変更2014/07/14(検S1)hata
            //myRoi[NumOfRoi].shape = -vRoi.shape;
            myRoi[NumOfRoi].shape = (RoiShape)Enum.ToObject(typeof(RoiShape), -1 * (int)vRoi.shape);

	        vRoi.shape = RoiShape.NO_ROI;
	        
            SelectRoi(NumOfRoi, false);
            
            return true;
        }
	
        //'********************************************************************************
        //'機    能  ： 円のROI形状データが表示アリア内にあるかチェックします
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        public bool CheckCircleInArea(int CX, int CY, int r)
        {
	        return CheckRectInArea(CX - r, CY - r, CX + r, CY + r);
        }
	
        //'********************************************************************************
        //'機    能  ：  長方形のROI形状データが表示エリア内にあるかチェックします
        //'              変数名           [I/O] 型        内容
        //'引    数  ：  なし
        //'戻 り 値  ：  なし
        //'補    足  ：
        //'
        //'履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //'              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //'********************************************************************************
        public bool CheckRectInArea(int x1, int y1, int x2, int y2)
        {
	        //戻り値初期化
            bool functionReturnValue = false;

            Winapi.RECT DispRect = default(Winapi.RECT);

            //Shapeｺﾝﾄﾛｰﾙ(MyControl)は使わない_2014/09/18(検S1)hata
            //if (MyControl[1] == null)
            if (RoiCtrlType == 0)
            {
		        DispRect.left = 0;
		        DispRect.top = 0;

                DispRect.right = frmScanImage.Instance.PicWidth / frmScanImage.Instance.Magnify - 1;
                DispRect.bottom = frmScanImage.Instance.PicHeight / frmScanImage.Instance.Magnify - 1;

            }
            else
            {
		        DispRect.left = 0;
		        DispRect.top = 0;
		        
                //DispRect.right = myDisplay.Width - 1;
		        //DispRect.bottom = myDisplay.Height - 1;
                if (myDisplay.Equals(myForm))
                {
                    DispRect.right = myForm.Width - 1;
                    DispRect.bottom = myForm.Height - 1;
                }
                else if (myDisplay.Equals(frmTransImage.Instance.ctlTransImage))
                {
                    DispRect.right = frmTransImage.Instance.ctlTransImage.Width - 1;
                    DispRect.bottom = frmTransImage.Instance.ctlTransImage.Height - 1;
                }
                else if (myDisplay.Equals(frmExObsCam.Instance))
                {
                    DispRect.right = 1024;
                    DispRect.bottom = 1024;
                }
           
            }

            //表示エリア外
            if (x1 < DispRect.left)
            {
                return functionReturnValue;
            }
            if (x2 > DispRect.right)
            {
                return functionReturnValue;
            }
            if (y1 < DispRect.top)
            {
                return functionReturnValue;
            }
            if (y2 > DispRect.bottom)
            {
                return functionReturnValue;
            }

	        //表示エリア内
	        functionReturnValue = true;
	        return functionReturnValue;
        }
	
        /// <summary>
        /// 
        /// </summary>
        private string GetRoiInfo(int RoiNo)
        {
	        int xc = 0;
	        int yc = 0;
	        int xl = 0;
	        int yl = 0;
	        int r = 0;
	        int x1 = 0;
	        int y1 = 0;
	        int x2 = 0;
	        int y2 = 0;

	        //戻り値初期化
            string functionReturnValue = "";

	        //ROIの種別ごとの処理
	        switch (GetRoiShape(RoiNo))
            {
		        //円形
		        case RoiShape.ROI_CIRC:
			        //ROI座標取得
			        GetCircleShape(RoiNo, ref xc, ref yc, ref r);
			        //戻り値セット
                    functionReturnValue = CTResources.LoadResString(StringTable.IDS_RoiCircle2) + " " + " XC : " + Convert.ToString(xc) + " YC : " + Convert.ToString(yc) + " R : " + Convert.ToString(r);
			        break;

		        //矩形
		        case RoiShape.ROI_RECT:
			        //ROI座標取得
                    GetRectangleShape2(RoiNo, ref xc, ref yc, ref xl, ref yl);
			        //戻り値セット
                    functionReturnValue = CTResources.LoadResString(StringTable.IDS_RoiRectangle) + " " + " XC : " + Convert.ToString(xc) + " YC : " + Convert.ToString(yc) + " XL : " + Convert.ToString(xl) + " YL : " + Convert.ToString(yl);
			        break;

		        //正方形
		        case RoiShape.ROI_SQR:
			        //ROI座標取得
                    GetSquareShape(RoiNo, ref xc, ref yc, ref r);
			        //戻り値セット
                    functionReturnValue = CTResources.LoadResString(StringTable.IDS_RoiSquare) + " " + " XC : " + Convert.ToString(xc) + " YC : " + Convert.ToString(yc) + " RO : " + Convert.ToString(r);
			        break;

		        //トレース
		        case RoiShape.ROI_TRACE:
			        //戻り値セット
                    functionReturnValue = CTResources.LoadResString(StringTable.IDS_RoiTrace);
			        break;

		        //線
		        case RoiShape.ROI_LINE:
			        //ROI座標取得
                    GetLineShape(RoiNo, ref x1, ref y1, ref x2, ref y2);

			        //戻り値セット
                    functionReturnValue = CTResources.LoadResString(StringTable.IDS_LineSegment) + " (" + Convert.ToString(x1) + "," + Convert.ToString(y1) + ") - (" + Convert.ToString(x2) + "," + Convert.ToString(y2) + ")";
			        break;

		        //点
		        case RoiShape.ROI_POINT:
			        //ROI座標取得
                    GetPointShape(RoiNo, ref xc, ref yc);

			        //戻り値セット
                    functionReturnValue = CTResources.LoadResString(StringTable.IDS_Point) + " (" + Convert.ToString(xc) + ", " + Convert.ToString(yc) + ")";
			        break;

                default:
                    break;
	        }

	        return functionReturnValue;
        }


        //追加2015/01/23hata
        //Roi情報をコピーする
        private RoiInfoType RoiInfoCopy(RoiInfoType roi)
        {
            RoiInfoType newRoi = new RoiInfoType();
            
            newRoi.area.bottom = roi.area.bottom;
            newRoi.area.left = roi.area.left;
            newRoi.area.right = roi.area.right;
            newRoi.area.top = roi.area.top;
            newRoi.p = (Point[])roi.p.Clone();
            newRoi.Enabled = roi.Enabled;
            newRoi.shape = roi.shape;
            newRoi.Sizable = roi.Sizable;
            newRoi.Selected = roi.Selected;

            return newRoi;
       }

    }

}
