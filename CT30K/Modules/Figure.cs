using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;

//
using CTAPI;
using CT30K.Common;

namespace CT30K
{
    ///* *************************************************************************** */
    ///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver9.7                */
    ///* 客先　　　　： ?????? 殿                                                    */
    ///* プログラム名： Figure.bas                                                   */
    ///* 処理概要　　： 図形描画用関数                                               */
    ///* 注意事項　　：                                                              */
    ///* --------------------------------------------------------------------------- */
    ///* ＯＳ　　　　： Windows XP Professional (SP1)                                */
    ///* コンパイラ　： VB 6.0 (SP5)                                                 */
    ///* --------------------------------------------------------------------------- */
    ///* VERSION     DATE        BY                  CHANGE/COMMENT                  */
    ///*                                                                             */
    ///* V1.00       99/XX/XX    (TOSFEC) ????????   新規作成                        */
    ///*                                                                             */
    ///* --------------------------------------------------------------------------- */
    ///* ご注意：                                                                    */
    ///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。    */
    ///*                                                                             */
    ///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2004                  */
    ///* *************************************************************************** */
    internal static class Figure
    {      
        //********************************************************************************
        //  定数データ宣言
        //********************************************************************************
        //Private Const diff  As Integer = 5
        //v17.10変更 byやまおか 2010/08/20
        private const int diff = 15;

        //********************************************************************************
        //機    能  ：  ???????
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //********************************************************************************
		private static bool coPointOnLine(int px, int py, int x1, int y1, int x2, int y2)
		{
			bool functionReturnValue = false;

			double y = 0;
			double a = 0;
			double b = 0;

			if ((x2 - x1) != 0)
            {
				a = (double)(y2 - y1) / (double)(x2 - x1);
                b = (double)y1 - a * (double)x1;
                y = (double)px * a + b;
				functionReturnValue = ((y - diff) <= py) & (py <= (y + diff));
			}
            else
            {
				functionReturnValue = ((x1 - diff) <= px) & (px <= (x1 + diff));
			}

			return functionReturnValue;
		}

        //********************************************************************************
        //機    能  ：  ???????
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //********************************************************************************
		public static bool PointOnLine(int px, int py, int x1, int y1, int x2, int y2)
		{
			bool functionReturnValue = false;

			//戻り値初期化
			functionReturnValue = false;

            if (!PointInRectangle(px, py, x1, y1, x2, y2))
            {
                return functionReturnValue;
            }
			//追加 by 間々田 2004/10/12

			if (coPointOnLine(px, py, x1, y1, x2, y2))
            {
				functionReturnValue = true;
			}
            else if (coPointOnLine(py, px, y1, x1, y2, x2))
            {
				functionReturnValue = true;
			}

			return functionReturnValue;
		}

        //********************************************************************************
        //機    能  ：  ???????
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //********************************************************************************
		public static bool PointOnRectangle(int px, int py, int x1, int y1, int x2, int y2)
		{
			bool functionReturnValue = false;

			if (PointInRectangle(px, py, x1, y1, x2, y2))
            {
				if (PointOnLine(px, py, x1, y1, x1, y2))
                {
					functionReturnValue = true;
				}
                else if (PointOnLine(px, py, x1, y1, x2, y1))
                {
					functionReturnValue = true;
				}
                else if (PointOnLine(px, py, x2, y1, x2, y2))
                {
					functionReturnValue = true;
				}
                else if (PointOnLine(px, py, x1, y2, x2, y2))
                {
					functionReturnValue = true;
				}
                else
                {
					functionReturnValue = false;
				}
			}

			return functionReturnValue;
		}

        //********************************************************************************
        //機    能  ：  ???????
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //********************************************************************************
        public static bool PointInRectangle(int px, int py, int x1, int y1, int x2, int y2)
        {
            bool functionReturnValue = false;

            //Win32api.RECT theRect = new Win32api.RECT();

            //theRect.Left = modLibrary.MinVal(x1, x2);
            //theRect.Top = modLibrary.MinVal(y1, y2);
            //theRect.Right = modLibrary.MaxVal(x1, x2);
            //theRect.Bottom = modLibrary.MaxVal(y1, y2);
            //functionReturnValue = modLibrary.InRange(px, theRect.Left - diff, theRect.Right + diff) &
            //                     modLibrary.InRange(py, theRect.Top - diff, theRect.Bottom + diff);
            
            int left = Math.Min(x1, x2);
            int top = Math.Min(y1, y2);
            int right = Math.Max(x1, x2);
            int bottom = Math.Max(y1, y2);
            
            functionReturnValue = modLibrary.InRange(px, left - diff, right + diff) & 
                                  modLibrary.InRange(py, top - diff, bottom + diff);
            
            return functionReturnValue;
		}

        //********************************************************************************
        //機    能  ：  ２つの座標が近傍かどうか調べます
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //********************************************************************************
		public static bool IsNear(int x1, int y1, int x2, int y2)
		{
			return (DistanceOf2Point(x1, y1, x2, y2) < diff);
		}
        public static bool IsNear(Point p1, Point p2)
        {
            return (DistanceOf2Point(p1.X, p1.Y, p2.X, p2.Y) < diff);
        }

        //********************************************************************************
        //機    能  ：  指定された点が円上にあるか調べます
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //********************************************************************************
		public static bool PointOnCircle(int x, int y, int CX, int CY, int r)
		{
            return modLibrary.InRange(DistanceOf2Point(x, y, CX, CY), r - diff, r + diff);
		}

        //********************************************************************************
        //機    能  ：  ２点間の距離を求める
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：  なし
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //********************************************************************************
        private static double DistanceOf2Point(int x1, int y1, int x2, int y2)
        {
			long dX = 0;
            long dY = 0;

			dX = x2 - x1;
			dY = y2 - y1;

            return Math.Sqrt(dX * dX + dY * dY);
        }

        //********************************************************************************
        //機    能  ： 指定座標にトレースROIがあるか調べる
        //              変数名           [I/O] 型        内容
        //引    数  ：  なし
        //戻 り 値  ：  なし
        //補    足  ：
        //
        //履    歴  ：  V1.00  XX/XX/XX  ??????????????  新規作成
        //              V9.6   04/10/08  (SI4)間々田     ロジック修正
        //********************************************************************************
        public static bool PointOnTrace(int x, int y, Point[] p)
		{
			bool functionReturnValue = false;

			int num = 0;
			int i = 0;
			int N = 0;

			//戻り値初期化
			functionReturnValue = false;

            num = p.GetUpperBound(0);

            for (i = 1; i <= num; i++)
            {
                N = (i == num ? 1 : i + 1);

                if (PointOnLine(x, y, p[i].X, p[i].Y, p[N].X, p[N].Y))
                {
                    functionReturnValue = true;
                    return functionReturnValue;
                }
            }

			return functionReturnValue;
		}
	}
}
