using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
//
using CT30K.Common.Library;

namespace CT30K.Common
{
    public class IniValue
    {
        //ダブルオブリーク実行ファイル名
        public string DoubleObliquePath { get; private set; }

        //CT30Kメッセージ受信用ポート番号：デフォルトは7010 
        public int CT30KPort { get; private set; }

        //イメージプロ実行ファイル名
        public string ImageProExe { get; private set; }

        //イメージプロ起動待ち時間
        public float ImageProPauseTime { get; private set; }

        //CT30K起動時にイメージプロを起動するかどうかのフラグ
        public bool ImageProStartup { get; private set; }

        //スキャン・リトライの使用するデータ用のメモリ 追加 by長野 2014/09/11
        public int SharedMemSize { get; private set; }

        //動画保存に使用可能なメモリ 追加 by長野 2015/08/18
        public int MovieMemSize { get; private set; }
        
        //段階ウォームアップ設定ステップ数
        public int STEPWU_NUM { get; private set; }

        //段階ウォームアップ設定パラメータ
        public int[] STEPWU_KV
        {
            get
            { 
                return  steppwuKV; 
            }
            set
            {
                 steppwuKV = value;
            }
        }
 
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
        //Ｘ線ドライバ（Viscom）のバージョン
        //public string DLLVERSION { get; private set; }
        //X線警告音WAVファイルパス
        //public string WavWarningPath { get; private set; }
        //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''

        //Rev23.00 X線beep音 .NET対応 by長野 2015/10/06
        public string WavWarningPath { get; private set; }

        //Rev26.10 Wスキャン・シフトスキャン用mcl係数
        public float mclMagnify { get; private set; }
       
        //Rev23.30 外観カメラ設定 by長野 2016/02/06
        public int ExObsCamOffsetMaskX { get; private set; } //画像切り出し開始画素X
        public int ExObsCamOffsetMaskY { get; private set; } //画像切り出し終了画素Y
        public int ExObsCamOffsetMaskWidth { get; private set; }//画像切り出し横サイズ
        public int ExObsCamOffsetMaskHeight { get; private set; } //画像切り出し縦サイズ
        public float ExObsCamZoomMagnify { get; private set; } //拡大時の倍率
        public int ExObsCamMaintCrossLine { get; private set; } //メンテ時の十字描画

        //Rev25.00 透視画像へのラインプロファイル  by長野 2016/08/08 --->
        public int FImageLProfileVPos { get; private set; }//垂直位置
        public int FImageLProfileV0Pos { get; private set; } //垂直プロファイル0%位置
        public int FImageLProfileV100Pos { get; private set; } //垂直プロファイル100%位置
        public int FImageLProfileHPos { get; private set; }//水平位置
        public int FImageLProfileH0Pos { get; private set; } //水平プロファイル0%位置
        public int FImageLProfileH100Pos { get;private set;}//水平プロファイル100%位置
        //<---

        //Rev26.00 AutoVI用設定値  by Hata 2017/02/01 --->
        public int AutoviStartX { get; private set; }              　   //グレイ値の調査エリア　開始X
        public int AutoviEndX { get; private set; }                  　 //グレイ値の調査エリア　終了X
        public int AutoviStartY { get; private set; }                   //グレイ値の調査エリア　開始Y
        public int AutoviEndY { get; private set; }                     //グレイ値の調査エリア　終了Y
        public float AutoviVoltHigh { get; private set; }               //電圧High（高）
        public float AutoviCurrentHigh { get; private set; }            //電流High（高）
        public float AutoviVoltMid { get; private set; }                //電圧Mid（中）
        public float AutoviCurrentMid { get; private set; }             //電流Mid（中）
        public float AutoviVoltLow { get; private set; }                //電圧Low（低）
        public float AutoviCurrentLow { get; private set; }             //電流Low（低）
        public float AutoviHistoMaxThresholdRate { get; private set; }    //ヒストグラム最大閾値(率)（レンジ幅のの20%)
        public float AutoviHistoMinThresholdRate { get; private set; }    //ヒストグラム最少閾値(率)（レンジ幅のの20%)
        public float AutoviHistoRangeThresholdRate { get; private set; }  //ヒストグラムレンジ最少閾値(率)（レンジ幅のの20%)
        public float AutoviVoltUpStep { get; private set; }               //電圧アップステップ
        public float AutoviVoltDownStep { get; private set; }             //電圧ダウンステップ
        public int AutoviLoopNumber { get; private set; }                 //繰り返し回数
        public float AutoviCurrentUpRate { get; private set; }            //電流アップ(率)
        public float AutoviCurrentDownRate { get; private set; }          //電流ダウン(率)

        public float AutoviHistoMeanThresholdRate { get; private set; }    //グレイ平均値の閾値(率)（レンジ幅のの50%)
        public float AutoviVoltMaxLimitRate { get; private set; }           //最大電圧の制限(率)（最大電圧の90%)
        public float AutoviCurrentMaxLimitRate { get; private set; }        //最大電流の制限(率)（最大電流の80%)

        //Rev26.00 [ガイド]タブ用スキャンエリア by長野 2016/12/27 --->
        public float ScanArea8inchFPD_SS { get; private set; }
        public float ScanArea8inchFPD_S { get; private set; }
        public float ScanArea8inchFPD_M { get; private set; }
        public float ScanArea8inchFPD_L { get; private set; }
        public float ScanArea8inchFPD_LL { get; private set; }
        public float[] ScanArea8inchFPD;

        public float ScanArea16inchFPD_SS { get; private set; }
        public float ScanArea16inchFPD_S { get; private set; }
        public float ScanArea16inchFPD_M { get; private set; }
        public float ScanArea16inchFPD_L { get; private set; }
        public float ScanArea16inchFPD_LL { get; private set; }
        public float[] ScanArea16inchFPD;

        public float ScanArea9inchII_SS { get; private set; }
        public float ScanArea9inchII_S { get; private set; }
        public float ScanArea9inchII_M { get; private set; }
        public float ScanArea9inchII_L { get; private set; }
        public float ScanArea9inchII_LL { get; private set; }
        public float[] ScanArea9inchII;

        public float ScanArea6inchII_SS { get; private set; }
        public float ScanArea6inchII_S { get; private set; }
        public float ScanArea6inchII_M { get; private set; }
        public float ScanArea6inchII_L { get; private set; }
        public float ScanArea6inchII_LL { get; private set; }
        public float[] ScanArea6inchII;

        public float ScanArea4inchII_SS { get; private set; }
        public float ScanArea4inchII_S { get; private set; }
        public float ScanArea4inchII_M { get; private set; }
        public float ScanArea4inchII_L { get; private set; }
        public float ScanArea4inchII_LL { get; private set; }
        public float[] ScanArea4inchII;

        public float ScanArea4_5inchII_SS { get; private set; }
        public float ScanArea4_5inchII_S { get; private set; }
        public float ScanArea4_5inchII_M { get; private set; }
        public float ScanArea4_5inchII_L { get; private set; }
        public float ScanArea4_5inchII_LL { get; private set; }
        public float[] ScanArea4_5inchII;

        public float ScanArea2inchII_SS { get; private set; }
        public float ScanArea2inchII_S { get; private set; }
        public float ScanArea2inchII_M { get; private set; }
        public float ScanArea2inchII_L { get; private set; }
        public float ScanArea2inchII_LL { get; private set; }
        public float[] ScanArea2inchII;
        //<---

        //Rev26.00 add スキャンエリア設定に対応したスキャンモードとWスキャン有効有無 by chouno 2017/01/17
        public int ScanMode8inchFPD_SS { get; private set; }
        public int WScanMode8inchFPD_SS { get; private set; }
        public int ScanMode8inchFPD_S { get; private set; }
        public int WScanMode8inchFPD_S { get; private set; }
        public int ScanMode8inchFPD_M { get; private set; }
        public int WScanMode8inchFPD_M { get; private set; }
        public int ScanMode8inchFPD_L { get; private set; }
        public int WScanMode8inchFPD_L { get; private set; }
        public int ScanMode8inchFPD_LL { get; private set; }
        public int WScanMode8inchFPD_LL { get; private set; }
        public int[] ScanMode8inchFPD;
        public int[] WScanMode8inchFPD;

        public int ScanMode16inchFPD_SS { get; private set; }
        public int WScanMode16inchFPD_SS { get; private set; }
        public int ScanMode16inchFPD_S { get; private set; }
        public int WScanMode16inchFPD_S { get; private set; }
        public int ScanMode16inchFPD_M { get; private set; }
        public int WScanMode16inchFPD_M { get; private set; }
        public int ScanMode16inchFPD_L { get; private set; }
        public int WScanMode16inchFPD_L { get; private set; }
        public int ScanMode16inchFPD_LL { get; private set; }
        public int WScanMode16inchFPD_LL { get; private set; }
        public int[] ScanMode16inchFPD;
        public int[] WScanMode16inchFPD;

        public int ScanMode9inchII_SS { get; private set; }
        public int WScanMode9inchII_SS { get; private set; }
        public int ScanMode9inchII_S { get; private set; }
        public int WScanMode9inchII_S { get; private set; }
        public int ScanMode9inchII_M { get; private set; }
        public int WScanMode9inchII_M { get; private set; }
        public int ScanMode9inchII_L { get; private set; }
        public int WScanMode9inchII_L { get; private set; }
        public int ScanMode9inchII_LL { get; private set; }
        public int WScanMode9inchII_LL { get; private set; }
        public int[] ScanMode9inchII;
        public int[] WScanMode9inchII;

        public int ScanMode6inchII_SS { get; private set; }
        public int WScanMode6inchII_SS { get; private set; }
        public int ScanMode6inchII_S { get; private set; }
        public int WScanMode6inchII_S { get; private set; }
        public int ScanMode6inchII_M { get; private set; }
        public int WScanMode6inchII_M { get; private set; }
        public int ScanMode6inchII_L { get; private set; }
        public int WScanMode6inchII_L { get; private set; }
        public int ScanMode6inchII_LL { get; private set; }
        public int WScanMode6inchII_LL { get; private set; }
        public int[] ScanMode6inchII;
        public int[] WScanMode6inchII;

        public int ScanMode4_5inchII_SS { get; private set; }
        public int WScanMode4_5inchII_SS { get; private set; }
        public int ScanMode4_5inchII_S { get; private set; }
        public int WScanMode4_5inchII_S { get; private set; }
        public int ScanMode4_5inchII_M { get; private set; }
        public int WScanMode4_5inchII_M { get; private set; }
        public int ScanMode4_5inchII_L { get; private set; }
        public int WScanMode4_5inchII_L { get; private set; }
        public int ScanMode4_5inchII_LL { get; private set; }
        public int WScanMode4_5inchII_LL { get; private set; }
        public int[] ScanMode4_5inchII;
        public int[] WScanMode4_5inchII;

        public int ScanMode4inchII_SS { get; private set; }
        public int WScanMode4inchII_SS { get; private set; }
        public int ScanMode4inchII_S { get; private set; }
        public int WScanMode4inchII_S { get; private set; }
        public int ScanMode4inchII_M { get; private set; }
        public int WScanMode4inchII_M { get; private set; }
        public int ScanMode4inchII_L { get; private set; }
        public int WScanMode4inchII_L { get; private set; }
        public int ScanMode4inchII_LL { get; private set; }
        public int WScanMode4inchII_LL { get; private set; }
        public int[] ScanMode4inchII;
        public int[] WScanMode4inchII;        

        public int ScanMode2inchII_SS { get; private set; }
        public int WScanMode2inchII_SS { get; private set; }
        public int ScanMode2inchII_S { get; private set; }
        public int WScanMode2inchII_S { get; private set; }
        public int ScanMode2inchII_M { get; private set; }
        public int WScanMode2inchII_M { get; private set; }
        public int ScanMode2inchII_L { get; private set; }
        public int WScanMode2inchII_L { get; private set; }
        public int ScanMode2inchII_LL { get; private set; }
        public int WScanMode2inchII_LL { get; private set; }
        public int[] ScanMode2inchII;
        public int[] WScanMode2inchII;    
        //<---

        //スキャンエリア設定時の機構部位置 Rev26.00 add by chouno 2017/01/18 --->
        //8inchFPD
        public float fdd8inchFPD_SS { get; private set; }
        public float tableZ8inchFPD_SS { get; private set; }
        public float fdd8inchFPD_S { get; private set; }
        public float tableZ8inchFPD_S { get; private set; }
        public float fdd8inchFPD_M { get; private set; }
        public float tableZ8inchFPD_M { get; private set; }
        public float fdd8inchFPD_L { get; private set; }
        public float tableZ8inchFPD_L { get; private set; }
        public float fdd8inchFPD_LL { get; private set; }
        public float tableZ8inchFPD_LL { get; private set; }
        public float[] fdd8inchFPD;
        public float[] tableZ8inchFPD;
        //16inchFPD
        public float fdd16inchFPD_SS { get; private set; }
        public float tableZ16inchFPD_SS { get; private set; }
        public float fdd16inchFPD_S { get; private set; }
        public float tableZ16inchFPD_S { get; private set; }
        public float fdd16inchFPD_M { get; private set; }
        public float tableZ16inchFPD_M { get; private set; }
        public float fdd16inchFPD_L { get; private set; }
        public float tableZ16inchFPD_L { get; private set; }
        public float fdd16inchFPD_LL { get; private set; }
        public float tableZ16inchFPD_LL { get; private set; }
        public float[] fdd16inchFPD;
        public float[] tableZ16inchFPD;
        //9inchI.I.
        public float fdd9inchII_SS { get; private set; }
        public float tableZ9inchII_SS { get; private set; }
        public float fdd9inchII_S { get; private set; }
        public float tableZ9inchII_S { get; private set; }
        public float fdd9inchII_M { get; private set; }
        public float tableZ9inchII_M { get; private set; }
        public float fdd9inchII_L { get; private set; }
        public float tableZ9inchII_L { get; private set; }
        public float fdd9inchII_LL { get; private set; }
        public float tableZ9inchII_LL { get; private set; }
        public float[] fdd9inchII;
        public float[] tableZ9inchII;
        //6inchI.I.
        public float fdd6inchII_SS { get; private set; }
        public float tableZ6inchII_SS { get; private set; }
        public float fdd6inchII_S { get; private set; }
        public float tableZ6inchII_S { get; private set; }
        public float fdd6inchII_M { get; private set; }
        public float tableZ6inchII_M { get; private set; }
        public float fdd6inchII_L { get; private set; }
        public float tableZ6inchII_L { get; private set; }
        public float fdd6inchII_LL { get; private set; }
        public float tableZ6inchII_LL { get; private set; }
        public float[] fdd6inchII;
        public float[] tableZ6inchII;
        //4.5inchI.I.
        public float fdd4_5inchII_SS { get; private set; }
        public float tableZ4_5inchII_SS { get; private set; }
        public float fdd4_5inchII_S { get; private set; }
        public float tableZ4_5inchII_S { get; private set; }
        public float fdd4_5inchII_M { get; private set; }
        public float tableZ4_5inchII_M { get; private set; }
        public float fdd4_5inchII_L { get; private set; }
        public float tableZ4_5inchII_L { get; private set; }
        public float fdd4_5inchII_LL { get; private set; }
        public float tableZ4_5inchII_LL { get; private set; }
        public float[] fdd4_5inchII;
        public float[] tableZ4_5inchII;
        //4inchI.I.
        public float fdd4inchII_SS { get; private set; }
        public float tableZ4inchII_SS { get; private set; }
        public float fdd4inchII_S { get; private set; }
        public float tableZ4inchII_S { get; private set; }
        public float fdd4inchII_M { get; private set; }
        public float tableZ4inchII_M { get; private set; }
        public float fdd4inchII_L { get; private set; }
        public float tableZ4inchII_L { get; private set; }
        public float fdd4inchII_LL { get; private set; }
        public float tableZ4inchII_LL { get; private set; }
        public float[] fdd4inchII;
        public float[] tableZ4inchII;
        //2inchI.I.
        public float fdd2inchII_SS { get; private set; }
        public float tableZ2inchII_SS { get; private set; }
        public float fdd2inchII_S { get; private set; }
        public float tableZ2inchII_S { get; private set; }
        public float fdd2inchII_M { get; private set; }
        public float tableZ2inchII_M { get; private set; }
        public float fdd2inchII_L { get; private set; }
        public float tableZ2inchII_L { get; private set; }
        public float fdd2inchII_LL { get; private set; }
        public float tableZ2inchII_LL { get; private set; }
        public float[] fdd2inchII;
        public float[] tableZ2inchII;
        //<---

        //Rev26.00 各スキャンエリアに対応した画質マトリクスのラベル(スキャン時間(min)) add by chouno 2017/01/19 --->
        public int scanTime_0 { get; private set; }
        public int scanTime_1 { get; private set; }
        public int scanTime_2 { get; private set; }
        public int scanTime_3 { get; private set; }
        public int scanTime_4 { get; private set; }
        public int scanTime_5 { get; private set; }
        public int scanTime_6 { get; private set; }
        public int scanTime_7 { get; private set; }
        public int scanTime_8 { get; private set; }
        public int scanTime_9 { get; private set; }
        public int scanTime_10 { get; private set; }
        public int scanTime_11 { get; private set; }
        public int scanTime_12 { get; private set; }
        public int scanTime_13 { get; private set; }
        public int scanTime_14 { get; private set; }
        public int scanTime_15 { get; private set; }
        public int[] scanTime;
        //<---

        //Rev26.00 校正自動判定用パラメータ add by chouno 2017/01/06 --->
        public float offsetCorDevRangeMin8inchFPD { get; private set; }
        public float offsetCorDevRangeMax8inchFPD { get; private set; }
        public float offsetCorMeanRangeMin8inchFPD { get; private set; }
        public float offsetCorMeanRangeMax8inchFPD { get; private set; }

        public float offsetCorDevRangeMin16inchFPD { get; private set; }
        public float offsetCorDevRangeMax16inchFPD { get; private set; }
        public float offsetCorMeanRangeMin16inchFPD { get; private set; }
        public float offsetCorMeanRangeMax16inchFPD { get; private set; }

        public float offsetCorDevRangeMin9inchII { get; private set; }
        public float offsetCorDevRangeMax9inchII { get; private set; }
        public float offsetCorMeanRangeMin9inchII { get; private set; }
        public float offsetCorMeanRangeMax9inchII { get; private set; }

        public float offsetCorDevRangeMin6inchII { get; private set; }
        public float offsetCorDevRangeMax6inchII { get; private set; }
        public float offsetCorMeanRangeMin6inchII { get; private set; }
        public float offsetCorMeanRangeMax6inchII { get; private set; }

        public float offsetCorDevRangeMin4inchII { get; private set; }
        public float offsetCorDevRangeMax4inchII { get; private set; }
        public float offsetCorMeanRangeMin4inchII { get; private set; }
        public float offsetCorMeanRangeMax4inchII { get; private set; }

        public float offsetCorDevRangeMin4_5inchII { get; private set; }
        public float offsetCorDevRangeMax4_5inchII { get; private set; }
        public float offsetCorMeanRangeMin4_5inchII { get; private set; }
        public float offsetCorMeanRangeMax4_5inchII { get; private set; }

        public float offsetCorDevRangeMin2inchII { get; private set; }
        public float offsetCorDevRangeMax2inchII { get; private set; }
        public float offsetCorMeanRangeMin2inchII { get; private set; }
        public float offsetCorMeanRangeMax2inchII { get; private set; }

        public float gainCorDevRangeMin8inchFPD { get; private set; }
        public float gainCorDevRangeMax8inchFPD { get; private set; }

        public float gainCorDevRangeMin16inchFPD { get; private set; }
        public float gainCorDevRangeMax16inchFPD { get; private set; }

        public float gainCorDevRangeMin9inchII { get; private set; }
        public float gainCorDevRangeMax9inchII { get; private set; }

        public float gainCorDevRangeMin6inchII { get; private set; }
        public float gainCorDevRangeMax6inchII { get; private set; }

        public float gainCorDevRangeMin4inchII { get; private set; }
        public float gainCorDevRangeMax4inchII { get; private set; }

        public float gainCorDevRangeMin4_5inchII { get; private set; }
        public float gainCorDevRangeMax4_5inchII { get; private set; }

        public float gainCorDevRangeMin2inchII { get; private set; }
        public float gainCorDevRangeMax2inchII { get; private set; }

        public float interceptRangeMin8inchFPD { get; private set; }
        public float interceptRangeMax8inchFPD { get; private set; }
        public float slopeRangeMin8inchFPD { get; private set; }
        public float slopeRangeMax8inchFPD { get; private set; }

        public float interceptRangeMin16inchFPD { get; private set; }
        public float interceptRangeMax16inchFPD { get; private set; }
        public float slopeRangeMin16inchFPD { get; private set; }
        public float slopeRangeMax16inchFPD { get; private set; }

        public float interceptRangeMin9inchII { get; private set; }
        public float interceptRangeMax9inchII { get; private set; }
        public float slopeRangeMin9inchII { get; private set; }
        public float slopeRangeMax9inchII { get; private set; }

        public float interceptRangeMin6inchII { get; private set; }
        public float interceptRangeMax6inchII { get; private set; }
        public float slopeRangeMin6inchII { get; private set; }
        public float slopeRangeMax6inchII { get; private set; }

        public float interceptRangeMin4inchII { get; private set; }
        public float interceptRangeMax4inchII { get; private set; }
        public float slopeRangeMin4inchII { get; private set; }
        public float slopeRangeMax4inchII { get; private set; }

        public float interceptRangeMin4_5inchII { get; private set; }
        public float interceptRangeMax4_5inchII { get; private set; }
        public float slopeRangeMin4_5inchII { get; private set; }
        public float slopeRangeMax4_5inchII { get; private set; }

        public float interceptRangeMin2inchII { get; private set; }
        public float interceptRangeMax2inchII { get; private set; }
        public float slopeRangeMin2inchII { get; private set; }
        public float slopeRangeMax2inchII { get; private set; }
        //<---

        //ゲイン校正時の機構部位置 Rev26.00 add by chouno 2017/01/17 --->
        public float GainCorTableY { get; private set; }
        public float GainCorFCD { get; private set; }
        public float GainCorFDD { get; private set; }
        //<---

        // Rev26.00 ファントムレスBHC用 追加by井上 2017/04/05
        public string BHCMaterialListFileName { get; private set; }

        // Rev26.00 ガイド(画質)に使用するガイド(画質)のインデックス add by chouno 2017/10/16
        public int guideImgQualityIndex0 { get; private set; }
        public int guideImgQualityIndex1 { get; private set; }
        public int guideImgQualityIndex2 { get; private set; }
        public int guideImgQualityIndex3 { get; private set; }
        public int[] guideImgQualityIndex;

        // Rev26.40 外観カメラパラメータ by chouno 2019/02/12
        public int FixedCamPosition { get; private set; }
        public float FixedCamFCD { get; private set; }
        public float FixedCamTableY { get; private set; }
        public float FixedCamTableZ { get; private set; }

        // Rev26.40 高速透視パラメータ by chouno 2019/02/12
        /// <summary>高速透視切り替え方法(0:PC,1:タッチパネル)</summary>
        public int HSCSettingType { get; private set; }
        /// <summary>高速透視運用タイプ(0: CTと高速,1:CTと高速と落下試験器</summary>
        public int HSCModeType { get; private set; }
        /// <summary>高速透視X線警告音出力方法(0:PC,1:タッチパネル)</summary>
        public int HSCWarningSoundMode { get; private set; }
        /// <summary>高速透視X線警告音持続時間(sec)</summary>
        public int HSCWarningSoundTime { get; private set; }
        ///<summar> X線警告メッセージ表示方式</summary>
        public int HSCWarningMessageMethod { get; private set; }

        //Rev26.40 X線・高速度カメラ
        ///<summary>昇降軸最大値(mm)</summary>
        public int XrayAndHSCMaxPos { get; private set; }
        ///<summary>昇降軸最小値(mm)</summary>
        public int XrayAndHSCMinPos { get; private set; }
        ///<summary>昇降移動タイムアウト(sec)</summary>
        public int XrayAndHSCTimeOut { get; private set; }

        #region 静的コンストラクタ
        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static IniValue()
        {
            //int[] steppwuKV = new int[5];
        }
        #endregion

        /// <summary>
        /// 読込み
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Load(string fileName)
        {
            IniFile ini = new IniFile(fileName);
            if (!ini.Exists)
            {
                return false;
            }

            
            //DoubleObliquePath = ini.GetIniString("DoubleOblique", "ExeFileName", "");
            //WavWarningPath = ini.GetIniString("WavWarning", "WavFileName", "");
            //CT30KPort = ini.GetIniInt("SocketPort", "CT30KPort", 7010);
            GetCtIni(ini);

            return true;
        }

        private void GetCtIni(IniFile inifile)
        {

            //イメージプロ実行ファイル名取得
            ImageProExe = inifile.GetIniString("Image-Pro Plus", "ExeFileName", "");

            //イメージプロ起動待ち時間取得
            ImageProPauseTime = Convert.ToSingle(inifile.GetIniInt("Image-Pro Plus", "PauseTime", 1));

            //CT30K起動時にイメージプロを起動するか
            ImageProStartup =Convert.ToBoolean(inifile.GetIniInt("Image-Pro Plus", "Startup", 1));

            //ダブルオブリーク実行ファイル名取得
            DoubleObliquePath = inifile.GetIniString("DoubleOblique", "ExeFileName", "");

            //CT30Kメッセージ受信用ポート番号：デフォルトは7010 
            CT30KPort = inifile.GetIniInt("SocketPort", "CT30KPort", 7010);

            //使用しない
            //ramMemSize = inifile.GetIniInt("RamDisk", "RamMem", 16000);
            //追加 by長野 2014/9/11
            SharedMemSize = inifile.GetIniInt("Memory", "Memory", 16000);

            //Rev22.00 追加 by長野 2015/08/18
            MovieMemSize = inifile.GetIniInt("Memory","MovieMemory",1024);

            //段階ウォームアップ設定パラメータを取得     'v17.72/v19.02追加 byやまおか 2012/05/16
            //ステップ数
            //変更2014/11/28hata_v19.51_dnet
            //STEPWU_NUM = inifile.GetIniInt("StepWarmUP", "StepNUM", 1);
            STEPWU_NUM = Convert.ToInt32(inifile.GetFileIniString("StepWarmUP", "StepNUM", "1", AppValue.STEPWUP_INI));
            //値の調整
            if ((STEPWU_NUM < 1))
            {
                STEPWU_NUM = 1;
            }
            if ((STEPWU_NUM > 5))
            {
                STEPWU_NUM = 5;
            }

            //第1段階           
            //変更2014/11/28hata_v19.51_dnet
            //steppwuKV[1] = inifile.GetIniInt("StepWarmUP", "StepKV1", 1);
            steppwuKV[1] = Convert.ToInt32(inifile.GetFileIniString("StepWarmUP", "StepKV1", "1", AppValue.STEPWUP_INI));
            //値の調整
            if ((steppwuKV[1] < 1))
            {
                steppwuKV[1] = 1;
            }
            if ((steppwuKV[1] > 1000))
            {
                steppwuKV[1] = 1000;
            }

            //第2段階
            //変更2014/11/28hata_v19.51_dnet
            //steppwuKV[1] = inifile.GetIniInt("StepWarmUP", "StepKV2", 1);
            steppwuKV[2] = Convert.ToInt32(inifile.GetFileIniString("StepWarmUP", "StepKV2", "1", AppValue.STEPWUP_INI));
            //値の調整
            if ((steppwuKV[2] < 1))
            {
                steppwuKV[2] = 1;
            }
            if ((steppwuKV[2] > 1000))
            {
                steppwuKV[2] = 1000;
            }

            //第3段階
            //変更2014/11/28hata_v19.51_dnet
            //steppwuKV[3] = inifile.GetIniInt("StepWarmUP", "StepKV3", 1);
            steppwuKV[3] = Convert.ToInt32(inifile.GetFileIniString("StepWarmUP", "StepKV3", "1", AppValue.STEPWUP_INI));
            //値の調整
            if ((steppwuKV[3] < 1))
            {
                steppwuKV[3] = 1;
            }
            if ((steppwuKV[3] > 1000))
            {
                steppwuKV[3] = 1000;
            }

            //第4段階
            //変更2014/11/28hata_v19.51_dnet
            //steppwuKV[4] = inifile.GetIniInt("StepWarmUP", "StepKV4", 1);
            steppwuKV[4] = Convert.ToInt32(inifile.GetFileIniString("StepWarmUP", "StepKV4", "1", AppValue.STEPWUP_INI));
            //値の調整
            if ((steppwuKV[4] < 1))
            {
                steppwuKV[4] = 1;
            }
            if ((steppwuKV[4] > 1000))
            {
                steppwuKV[4] = 1000;
            }

            //第5段階
            //変更2014/11/28hata_v19.51_dnet
            //steppwuKV[5] = inifile.GetIniInt("StepWarmUP", "StepKV5", 1);
            steppwuKV[5] = Convert.ToInt32(inifile.GetFileIniString("StepWarmUP", "StepKV5", "1", AppValue.STEPWUP_INI));
            //値の調整
            if ((steppwuKV[5] < 1))
            {
                steppwuKV[5] = 1;
            }
            if ((steppwuKV[5] > 1000))
            {
                steppwuKV[5] = 1000;
            }

            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //Ｘ線ドライバ（Viscom）のバージョン
            //DLLVERSION = inifile.GetIniString("Viscom Driver", "DllVersion", "");
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
           
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここから'''''
            //X線警告音WAVファイルパスを取得
            //WavWarningPath = inifile.GetIniString("WavWarning", "WavFileName", "");
            //v29.99 今のところ不要 by長野 2013/04/08'''''ここまで'''''
            
            //Rev23.00 X線ON beep音 .NET対応 2015/09/24
            WavWarningPath = inifile.GetIniString("WavWarning", "WavFileName", "");

            //Rev26.10 Wスキャン・シフトスキャン用mcl係数
            mclMagnify = Convert.ToSingle(inifile.GetFileIniString("ScanPara", "mclMagnify", "0.65", AppValue.CT30KProcessParaIniFileName));

            //Rev23.30 外観カメラ --->
            ExObsCamOffsetMaskX = Convert.ToInt32(inifile.GetFileIniString("Size", "OffsetMaskX", "0", AppValue.ExObsCamIniFileName));
            ExObsCamOffsetMaskY = Convert.ToInt32(inifile.GetFileIniString("Size", "OffsetMaskY", "0", AppValue.ExObsCamIniFileName));
            ExObsCamOffsetMaskWidth = Convert.ToInt32(inifile.GetFileIniString("Size", "OffsetMaskWidht", "960", AppValue.ExObsCamIniFileName));
            ExObsCamOffsetMaskHeight = Convert.ToInt32(inifile.GetFileIniString("Size", "OffsetMaskHeight", "960", AppValue.ExObsCamIniFileName));
            ExObsCamZoomMagnify = Convert.ToSingle(inifile.GetFileIniString("Zoom", "ZoomMagnify", "2.0", AppValue.ExObsCamIniFileName));
            ExObsCamMaintCrossLine = Convert.ToInt32(inifile.GetFileIniString("Maintenance", "WriteCrossLine", "0", AppValue.ExObsCamIniFileName));
            //<---

            //Rev25.00 透視画像へのラインプロファイル 追加 by長野 2016/08/08 --->
            //FImageLProfileVPos = Convert.ToInt32(inifile.GetIniString("FImageLineProfile","FImageLProfileVPos",""));
            //FImageLProfileV0Pos = Convert.ToInt32(inifile.GetIniString("FImageLineProfile","FImageLProfileV0Pos",""));
            //FImageLProfileV100Pos = Convert.ToInt32(inifile.GetIniString("FImageLineProfile","FImageLProfileV100Pos",""));
            //FImageLProfileHPos = Convert.ToInt32(inifile.GetIniString("FImageLineProfile","FImageLProfileHPos",""));
            //FImageLProfileH0Pos = Convert.ToInt32(inifile.GetIniString("FImageLineProfile","FImageLProfileH0Pos",""));
            //FImageLProfileH100Pos = Convert.ToInt32(inifile.GetIniString("FImageLineProfile","FImageLProfileH100Pos",""));
            //<---

            //Rev26.00 透視画像へのラインプロファイルパラメータを、CT30KPara.iniへ変更 ---> 2016/12/27
            FImageLProfileVPos = Convert.ToInt32(inifile.GetFileIniString("FImageLineProfile", "FImageLProfileVPos", "", AppValue.CT30KUIParaIniFileName));
            FImageLProfileV0Pos = Convert.ToInt32(inifile.GetFileIniString("FImageLineProfile", "FImageLProfileV0Pos", "", AppValue.CT30KUIParaIniFileName));
            FImageLProfileV100Pos = Convert.ToInt32(inifile.GetFileIniString("FImageLineProfile", "FImageLProfileV100Pos", "", AppValue.CT30KUIParaIniFileName));
            FImageLProfileHPos = Convert.ToInt32(inifile.GetFileIniString("FImageLineProfile", "FImageLProfileHPos", "", AppValue.CT30KUIParaIniFileName));
            FImageLProfileH0Pos = Convert.ToInt32(inifile.GetFileIniString("FImageLineProfile", "FImageLProfileH0Pos", "", AppValue.CT30KUIParaIniFileName));
            FImageLProfileH100Pos = Convert.ToInt32(inifile.GetFileIniString("FImageLineProfile", "FImageLProfileH100Pos", "", AppValue.CT30KUIParaIniFileName));
            //<---

            //Rev26.00 XrayAutoVI設定 追加 by Hata 2017/02/01 --->
            AutoviStartX = Convert.ToInt32(inifile.GetFileIniString("XrayAutoVI", "StartX", "20", AppValue.XrayAutoVIIniFileName));                                     //グレイ値の調査エリア　開始X
            AutoviEndX = Convert.ToInt32(inifile.GetFileIniString("XrayAutoVI", "EndX", "100", AppValue.XrayAutoVIIniFileName));                                        //グレイ値の調査エリア　終了X
            AutoviStartY = Convert.ToInt32(inifile.GetFileIniString("XrayAutoVI", "StartY", "20", AppValue.XrayAutoVIIniFileName));                                     //グレイ値の調査エリア　開始Y
            AutoviEndY = Convert.ToInt32(inifile.GetFileIniString("XrayAutoVI", "EndY", "100", AppValue.XrayAutoVIIniFileName));                                        //グレイ値の調査エリア　終了Y
            AutoviVoltHigh = Convert.ToSingle(inifile.GetFileIniString("XrayAutoVI", "HighVolt", "200", AppValue.XrayAutoVIIniFileName));                               //電圧High（高）
            AutoviCurrentHigh = Convert.ToSingle(inifile.GetFileIniString("XrayAutoVI", "HighCurrent", "30", AppValue.XrayAutoVIIniFileName));                          //電流High（高）
            AutoviVoltMid = Convert.ToSingle(inifile.GetFileIniString("XrayAutoVI", "MidVolt", "160", AppValue.XrayAutoVIIniFileName));                                 //電圧Mid（中）
            AutoviCurrentMid = Convert.ToSingle(inifile.GetFileIniString("XrayAutoVI", "MidCurrent", "30", AppValue.XrayAutoVIIniFileName));                            //電流Mid（中）
            AutoviVoltLow = Convert.ToSingle(inifile.GetFileIniString("XrayAutoVI", "LowVolt", "100", AppValue.XrayAutoVIIniFileName));                                 //電圧Low（低）
            AutoviCurrentLow = Convert.ToSingle(inifile.GetFileIniString("XrayAutoVI", "LowCurrent", "30", AppValue.XrayAutoVIIniFileName));                            //電流Low（低）

            AutoviHistoMaxThresholdRate = Convert.ToSingle(inifile.GetFileIniString("XrayAutoVI", "HistoMaxThresholdRate", "0.2", AppValue.XrayAutoVIIniFileName));     //ヒストグラム最大閾値(率)（レンジ幅のの20%)
            AutoviHistoMinThresholdRate = Convert.ToSingle(inifile.GetFileIniString("XrayAutoVI", "HistoMinThresholdRate", "0.2", AppValue.XrayAutoVIIniFileName));     //ヒストグラム最少閾値(率)（レンジ幅のの20%)
            AutoviHistoRangeThresholdRate = Convert.ToSingle(inifile.GetFileIniString("XrayAutoVI", "HistoRangeThresholdRate", "0.2", AppValue.XrayAutoVIIniFileName)); //ヒストグラム最少閾値(率)（レンジ幅のの20%)

            AutoviVoltUpStep = Convert.ToSingle(inifile.GetFileIniString("XrayAutoVI", "VoltUpStep", "10", AppValue.XrayAutoVIIniFileName));                            //電圧アップステップ
            AutoviVoltDownStep = Convert.ToSingle(inifile.GetFileIniString("XrayAutoVI", "VoltDownStep", "10", AppValue.XrayAutoVIIniFileName));                        //電圧ダウンステップ

            AutoviCurrentUpRate = Convert.ToSingle(inifile.GetFileIniString("XrayAutoVI", "CurrentUpRate", "1.1", AppValue.XrayAutoVIIniFileName));                            //電流アップ(率)1.5倍）
            AutoviCurrentDownRate = Convert.ToSingle(inifile.GetFileIniString("XrayAutoVI", "CurrentDownRate", "0.9", AppValue.XrayAutoVIIniFileName));                        //電流ダウン(率)（0.5倍）
            AutoviHistoMeanThresholdRate = Convert.ToSingle(inifile.GetFileIniString("XrayAutoVI", "HistoMeanThresholdRate", "0.5", AppValue.XrayAutoVIIniFileName));     //グレイ平均値の閾値(率)（レンジ幅のの50%)

            AutoviVoltMaxLimitRate = Convert.ToSingle(inifile.GetFileIniString("XrayAutoVI", "VoltMaxLimitRate", "0.9", AppValue.XrayAutoVIIniFileName));               //最大電圧の制限(率)（最大電圧の90%)
            AutoviCurrentMaxLimitRate = Convert.ToSingle(inifile.GetFileIniString("XrayAutoVI", "CurrentMaxLimitRate", "0.8", AppValue.XrayAutoVIIniFileName));         //最大電流の制限(率)（最大電流の80%)

            AutoviLoopNumber = Convert.ToInt32(inifile.GetFileIniString("XrayAutoVI", "LoopNumber", "5", AppValue.XrayAutoVIIniFileName));                              //繰り返し回数
            //<---

            //Rev26.00 [ガイド]タブ スキャンエリア設定用 2016/12/27 by長野
            ScanArea16inchFPD_SS = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea16inchFPD_SS", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea16inchFPD_S = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea16inchFPD_S", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea16inchFPD_M = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea16inchFPD_M", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea16inchFPD_L = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea16inchFPD_L", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea16inchFPD_LL = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea16inchFPD_LL", "0", AppValue.CT30KProcessParaIniFileName));

            ScanArea8inchFPD_SS = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea8inchFPD_SS", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea8inchFPD_S = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea8inchFPD_S", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea8inchFPD_M = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea8inchFPD_M", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea8inchFPD_L = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea8inchFPD_L", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea8inchFPD_LL = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea8inchFPD_LL", "0", AppValue.CT30KProcessParaIniFileName));

            ScanArea9inchII_SS = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea9inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea9inchII_S = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea9inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea9inchII_M = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea9inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea9inchII_L = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea9inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea9inchII_LL = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea9inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));

            ScanArea6inchII_SS = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea6inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea6inchII_S = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea6inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea6inchII_M = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea6inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea6inchII_L = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea6inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea6inchII_LL = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea6inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));

            ScanArea4inchII_SS = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea4inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea4inchII_S = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea4inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea4inchII_M = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea4inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea4inchII_L = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea4inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea4inchII_LL = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea4inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));

            ScanArea4_5inchII_SS = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea4_5inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea4_5inchII_S = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea4_5inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea4_5inchII_M = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea4_5inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea4_5inchII_L = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea4_5inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea4_5inchII_LL = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea4_5inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));

            ScanArea2inchII_SS = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea2inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea2inchII_S = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea2inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea2inchII_M = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea2inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea2inchII_L = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea2inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            ScanArea2inchII_LL = Convert.ToSingle(inifile.GetFileIniString("ScanAreaForGuideTab", "ScanArea2inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));
            
            //配列化
            ScanArea16inchFPD = new float[] { ScanArea16inchFPD_SS,ScanArea16inchFPD_S, ScanArea16inchFPD_M, ScanArea16inchFPD_L, ScanArea16inchFPD_LL };
            ScanArea8inchFPD = new float[] { ScanArea8inchFPD_SS,ScanArea8inchFPD_S, ScanArea8inchFPD_M, ScanArea8inchFPD_L, ScanArea8inchFPD_LL };
            ScanArea9inchII = new float[] { ScanArea9inchII_SS, ScanArea9inchII_S, ScanArea9inchII_M, ScanArea9inchII_L, ScanArea9inchII_LL };
            ScanArea6inchII = new float[] { ScanArea6inchII_SS, ScanArea6inchII_S, ScanArea6inchII_M, ScanArea6inchII_L, ScanArea6inchII_LL };
            ScanArea4inchII = new float[] { ScanArea4inchII_SS, ScanArea4inchII_S, ScanArea4inchII_M, ScanArea4inchII_L, ScanArea4inchII_LL };
            ScanArea4_5inchII = new float[] { ScanArea4_5inchII_SS, ScanArea4_5inchII_S, ScanArea4_5inchII_M, ScanArea4_5inchII_L, ScanArea4_5inchII_LL };
            //<---

            //Rev26.00 スキャンエリアに対応したスキャンモード・Wスキャン有無の設定 by chouno 2017/01/17 --->
            ScanMode16inchFPD_SS = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode16inchFPD_SS", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode16inchFPD_SS = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode16inchFPD_SS", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode16inchFPD_S = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode16inchFPD_S", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode16inchFPD_S = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode16inchFPD_S", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode16inchFPD_M = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode16inchFPD_M", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode16inchFPD_M = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode16inchFPD_M", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode16inchFPD_L = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode16inchFPD_L", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode16inchFPD_L = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode16inchFPD_L", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode16inchFPD_LL = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode16inchFPD_LL", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode16inchFPD_LL = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode16inchFPD_LL", "0", AppValue.CT30KProcessParaIniFileName));

            ScanMode8inchFPD_SS = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode8inchFPD_SS", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode8inchFPD_SS = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode8inchFPD_SS", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode8inchFPD_S = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode8inchFPD_S", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode8inchFPD_S = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode8inchFPD_S", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode8inchFPD_M = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode8inchFPD_M", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode8inchFPD_M = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode8inchFPD_M", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode8inchFPD_L = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode8inchFPD_L", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode8inchFPD_L = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode8inchFPD_L", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode8inchFPD_LL = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode8inchFPD_LL", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode8inchFPD_LL = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode8inchFPD_LL", "0", AppValue.CT30KProcessParaIniFileName));

            ScanMode9inchII_SS = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode9inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode9inchII_SS = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode9inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode9inchII_S = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode9inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode9inchII_S = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode9inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode9inchII_M = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode9inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode9inchII_M = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode9inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode9inchII_L = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode9inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode9inchII_L = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode9inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode9inchII_LL = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode9inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode9inchII_LL = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode9inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));

            ScanMode6inchII_SS = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode6inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode6inchII_SS = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode6inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode6inchII_S = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode6inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode6inchII_S = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode6inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode6inchII_M = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode6inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode6inchII_M = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode6inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode6inchII_L = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode6inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode6inchII_L = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode6inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode6inchII_LL = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode6inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode6inchII_LL = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode6inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));

            ScanMode4_5inchII_SS = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode4_5inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode4_5inchII_SS = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode4_5inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode4_5inchII_S = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode4_5inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode4_5inchII_S = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode4_5inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode4_5inchII_M = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode4_5inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode4_5inchII_M = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode4_5inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode4_5inchII_L = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode4_5inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode4_5inchII_L = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode4_5inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode4_5inchII_LL = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode4_5inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode4_5inchII_LL = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode4_5inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));

            ScanMode4inchII_SS = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode4inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode4inchII_SS = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode4inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode4inchII_S = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode4inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode4inchII_S = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode4inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode4inchII_M = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode4inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode4inchII_M = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode4inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode4inchII_L = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode4inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode4inchII_L = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode4inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode4inchII_LL = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode4inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode4inchII_LL = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode4inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));

            ScanMode2inchII_SS = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode2inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode2inchII_SS = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode2inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode2inchII_S = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode2inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode2inchII_S = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode2inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode2inchII_M = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode2inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode2inchII_M = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode2inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode2inchII_L = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode2inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode2inchII_L = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode2inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            ScanMode2inchII_LL = Convert.ToInt32(inifile.GetFileIniString("ScanModeForScanArea", "ScanMode2inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));
            WScanMode2inchII_LL = Convert.ToInt32(inifile.GetFileIniString("WScanModeForScanArea", "WScanMode2inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));
            
            //配列化
            ScanMode16inchFPD = new int[] { ScanMode16inchFPD_SS, ScanMode16inchFPD_S, ScanMode16inchFPD_M, ScanMode16inchFPD_L, ScanMode16inchFPD_LL };
            WScanMode16inchFPD = new int[] { WScanMode16inchFPD_SS, WScanMode16inchFPD_S, WScanMode16inchFPD_M, WScanMode16inchFPD_L, WScanMode16inchFPD_LL };
            ScanMode8inchFPD = new int[] { ScanMode8inchFPD_SS, ScanMode8inchFPD_S, ScanMode8inchFPD_M, ScanMode8inchFPD_L, ScanMode8inchFPD_LL };
            WScanMode8inchFPD = new int[] { WScanMode8inchFPD_SS, WScanMode8inchFPD_S, WScanMode8inchFPD_M, WScanMode8inchFPD_L, WScanMode8inchFPD_LL };
            ScanMode9inchII = new int[] { ScanMode9inchII_SS, ScanMode9inchII_S, ScanMode9inchII_M, ScanMode9inchII_L, ScanMode9inchII_LL };
            WScanMode9inchII = new int[] { WScanMode9inchII_SS, WScanMode9inchII_S, WScanMode9inchII_M, WScanMode9inchII_L, WScanMode9inchII_LL };
            ScanMode6inchII = new int[] { ScanMode6inchII_SS, ScanMode6inchII_S, ScanMode6inchII_M, ScanMode6inchII_L, ScanMode6inchII_LL };
            WScanMode6inchII = new int[] { WScanMode6inchII_SS, WScanMode6inchII_S, WScanMode6inchII_M, WScanMode6inchII_L, WScanMode6inchII_LL };
            ScanMode4_5inchII = new int[] { ScanMode4_5inchII_SS, ScanMode4_5inchII_S, ScanMode4_5inchII_M, ScanMode4_5inchII_L, ScanMode4_5inchII_LL };
            WScanMode4_5inchII = new int[] { WScanMode4inchII_SS, WScanMode4inchII_S, WScanMode4inchII_M, WScanMode4inchII_L, WScanMode4inchII_LL };
            ScanMode4inchII = new int[] { ScanMode4inchII_SS, ScanMode4inchII_S, ScanMode4inchII_M, ScanMode4inchII_L, ScanMode4inchII_LL };
            WScanMode4inchII = new int[] { WScanMode4inchII_SS, WScanMode4inchII_S, WScanMode4inchII_M, WScanMode4inchII_L, WScanMode4inchII_LL };
            ScanMode2inchII = new int[] { ScanMode2inchII_SS, ScanMode2inchII_S, ScanMode2inchII_M, ScanMode2inchII_L, ScanMode2inchII_LL };
            WScanMode2inchII = new int[] { WScanMode2inchII_SS, WScanMode2inchII_S, WScanMode2inchII_M, WScanMode2inchII_L, WScanMode2inchII_LL };
            //<---

            //Rev26.00 スキャンエリア設定時のFDD、Z軸位置 add by chouno --->
            //16inchFPD
            fdd16inchFPD_SS = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd16inchFPD_SS", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ16inchFPD_SS = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ16inchFPD_SS", "0", AppValue.CT30KProcessParaIniFileName));
            fdd16inchFPD_S = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd16inchFPD_S", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ16inchFPD_S = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ16inchFPD_S", "0", AppValue.CT30KProcessParaIniFileName));
            fdd16inchFPD_M = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd16inchFPD_M", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ16inchFPD_M = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ16inchFPD_M", "0", AppValue.CT30KProcessParaIniFileName));
            fdd16inchFPD_L = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd16inchFPD_L", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ16inchFPD_L = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ16inchFPD_L", "0", AppValue.CT30KProcessParaIniFileName));
            fdd16inchFPD_LL = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd16inchFPD_LL", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ16inchFPD_LL = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ16inchFPD_LL", "0", AppValue.CT30KProcessParaIniFileName));
            //8inchFPD
            fdd8inchFPD_SS = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd8inchFPD_SS", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ8inchFPD_SS = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ8inchFPD_SS", "0", AppValue.CT30KProcessParaIniFileName));
            fdd8inchFPD_S = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd8inchFPD_S", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ8inchFPD_S = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ8inchFPD_S", "0", AppValue.CT30KProcessParaIniFileName));
            fdd8inchFPD_M = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd8inchFPD_M", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ8inchFPD_M = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ8inchFPD_M", "0", AppValue.CT30KProcessParaIniFileName));
            fdd8inchFPD_L = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd8inchFPD_L", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ8inchFPD_L = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ8inchFPD_L", "0", AppValue.CT30KProcessParaIniFileName));
            fdd8inchFPD_LL = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd8inchFPD_LL", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ8inchFPD_LL = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ8inchFPD_LL", "0", AppValue.CT30KProcessParaIniFileName));
            //9inchI.I.
            fdd9inchII_SS = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd9inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ9inchII_SS = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ9inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            fdd9inchII_S = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd9inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ9inchII_S = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ9inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            fdd9inchII_M = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd9inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ9inchII_M = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ9inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            fdd9inchII_L = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd9inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ9inchII_L = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ9inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            fdd9inchII_LL = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd9inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ9inchII_LL = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ9inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));
            //6inchI.I.
            fdd6inchII_SS = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd6inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ6inchII_SS = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ6inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            fdd6inchII_S = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd6inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ6inchII_S = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ6inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            fdd6inchII_M = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd6inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ6inchII_M = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ6inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            fdd6inchII_L = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd6inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ6inchII_L = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ6inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            fdd6inchII_LL = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd6inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ6inchII_LL = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ6inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));
            //4.5inchI.I.
            fdd4_5inchII_SS = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd4_5inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ4_5inchII_SS = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ4_5inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            fdd4_5inchII_S = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd4_5inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ4_5inchII_S = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ4_5inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            fdd4_5inchII_M = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd4_5inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ4_5inchII_M = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ4_5inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            fdd4_5inchII_L = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd4_5inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ4_5inchII_L = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ4_5inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            fdd4_5inchII_LL = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd4_5inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ4_5inchII_LL = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ4_5inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));
            //4inchI.I.
            fdd4inchII_SS = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd4inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ4inchII_SS = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ4inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            fdd4inchII_S = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd4inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ4inchII_S = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ4inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            fdd4inchII_M = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd4inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ4inchII_M = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ4inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            fdd4inchII_L = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd4inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ4inchII_L = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ4inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            fdd4inchII_LL = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd4inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ4inchII_LL = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ4inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));
            //2inchI.I.
            fdd2inchII_SS = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd2inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ2inchII_SS = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ2inchII_SS", "0", AppValue.CT30KProcessParaIniFileName));
            fdd2inchII_S = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd2inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ2inchII_S = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ2inchII_S", "0", AppValue.CT30KProcessParaIniFileName));
            fdd2inchII_M = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd2inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ2inchII_M = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ2inchII_M", "0", AppValue.CT30KProcessParaIniFileName));
            fdd2inchII_L = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd2inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ2inchII_L = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ2inchII_L", "0", AppValue.CT30KProcessParaIniFileName));
            fdd2inchII_LL = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "fdd2inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));
            tableZ2inchII_LL = Convert.ToSingle(inifile.GetFileIniString("MechaPosForScanArea", "TableZ2inchII_LL", "0", AppValue.CT30KProcessParaIniFileName));
            //配列化
            fdd8inchFPD = new float[] { fdd8inchFPD_SS, fdd8inchFPD_S, fdd8inchFPD_M, fdd8inchFPD_L, fdd8inchFPD_LL };
            fdd16inchFPD = new float[] { fdd16inchFPD_SS, fdd16inchFPD_S, fdd16inchFPD_M, fdd16inchFPD_L, fdd16inchFPD_LL };
            fdd9inchII = new float[] { fdd9inchII_SS, fdd9inchII_S, fdd9inchII_M, fdd9inchII_L, fdd9inchII_LL };
            fdd6inchII = new float[] { fdd6inchII_SS, fdd6inchII_S, fdd6inchII_M, fdd6inchII_L, fdd6inchII_LL };
            fdd4_5inchII = new float[] { fdd4_5inchII_SS, fdd4_5inchII_S, fdd4_5inchII_M, fdd4_5inchII_L, fdd4_5inchII_LL };
            fdd4inchII = new float[] { fdd4inchII_SS, fdd4inchII_S, fdd4inchII_M, fdd4inchII_L, fdd4inchII_LL };
            fdd2inchII = new float[] { fdd2inchII_SS, fdd2inchII_S, fdd2inchII_M, fdd2inchII_L, fdd2inchII_LL };
            tableZ8inchFPD = new float[] { tableZ8inchFPD_SS, tableZ8inchFPD_S, tableZ8inchFPD_M, tableZ8inchFPD_L, tableZ8inchFPD_LL };
            tableZ16inchFPD = new float[] { tableZ16inchFPD_SS, tableZ16inchFPD_S, tableZ16inchFPD_M, tableZ16inchFPD_L, tableZ16inchFPD_LL };
            tableZ9inchII = new float[] { tableZ9inchII_SS, tableZ9inchII_S, tableZ9inchII_M, tableZ9inchII_L, tableZ9inchII_LL };
            tableZ6inchII = new float[] { tableZ6inchII_SS, tableZ6inchII_S, tableZ6inchII_M, tableZ6inchII_L, tableZ6inchII_LL };
            tableZ4_5inchII = new float[] { tableZ4_5inchII_SS, tableZ4_5inchII_S, tableZ4_5inchII_M, tableZ4_5inchII_L, tableZ4_5inchII_LL };
            tableZ4inchII = new float[] { tableZ4inchII_SS, tableZ4inchII_S, tableZ4inchII_M, tableZ4inchII_L, tableZ4inchII_LL };
            tableZ2inchII = new float[] { tableZ2inchII_SS, tableZ2inchII_S, tableZ2inchII_M, tableZ2inchII_L, tableZ2inchII_LL };

            //<---

            //Rev26.00 各スキャンエリアに対応した画質マトリクスのラベル(スキャン時間(min)) add by chouno 2017/01/19 --->
            scanTime_0 = Convert.ToInt32(inifile.GetFileIniString("ScanTimeForGuideTab", "scanTime_0", "0", AppValue.CT30KProcessParaIniFileName));
            scanTime_1 = Convert.ToInt32(inifile.GetFileIniString("ScanTimeForGuideTab", "scanTime_1", "0", AppValue.CT30KProcessParaIniFileName));
            scanTime_2 = Convert.ToInt32(inifile.GetFileIniString("ScanTimeForGuideTab", "scanTime_2", "0", AppValue.CT30KProcessParaIniFileName));
            scanTime_3 = Convert.ToInt32(inifile.GetFileIniString("ScanTimeForGuideTab", "scanTime_3", "0", AppValue.CT30KProcessParaIniFileName));
            scanTime_4 = Convert.ToInt32(inifile.GetFileIniString("ScanTimeForGuideTab", "scanTime_4", "0", AppValue.CT30KProcessParaIniFileName));
            scanTime_5 = Convert.ToInt32(inifile.GetFileIniString("ScanTimeForGuideTab", "scanTime_5", "0", AppValue.CT30KProcessParaIniFileName));
            scanTime_6 = Convert.ToInt32(inifile.GetFileIniString("ScanTimeForGuideTab", "scanTime_6", "0", AppValue.CT30KProcessParaIniFileName));
            scanTime_7 = Convert.ToInt32(inifile.GetFileIniString("ScanTimeForGuideTab", "scanTime_7", "0", AppValue.CT30KProcessParaIniFileName));
            scanTime_8 = Convert.ToInt32(inifile.GetFileIniString("ScanTimeForGuideTab", "scanTime_8", "0", AppValue.CT30KProcessParaIniFileName));
            scanTime_9 = Convert.ToInt32(inifile.GetFileIniString("ScanTimeForGuideTab", "scanTime_9", "0", AppValue.CT30KProcessParaIniFileName));
            scanTime_10 = Convert.ToInt32(inifile.GetFileIniString("ScanTimeForGuideTab", "scanTime_10", "0", AppValue.CT30KProcessParaIniFileName));
            scanTime_11 = Convert.ToInt32(inifile.GetFileIniString("ScanTimeForGuideTab", "scanTime_11", "0", AppValue.CT30KProcessParaIniFileName));
            scanTime_12 = Convert.ToInt32(inifile.GetFileIniString("ScanTimeForGuideTab", "scanTime_12", "0", AppValue.CT30KProcessParaIniFileName));
            scanTime_13 = Convert.ToInt32(inifile.GetFileIniString("ScanTimeForGuideTab", "scanTime_13", "0", AppValue.CT30KProcessParaIniFileName));
            scanTime_14 = Convert.ToInt32(inifile.GetFileIniString("ScanTimeForGuideTab", "scanTime_14", "0", AppValue.CT30KProcessParaIniFileName));
            scanTime_15 = Convert.ToInt32(inifile.GetFileIniString("ScanTimeForGuideTab", "scanTime_15", "0", AppValue.CT30KProcessParaIniFileName));

            //配列化
            scanTime = new int[]{scanTime_0,scanTime_1,scanTime_2,scanTime_3,scanTime_4,
                                 scanTime_5,scanTime_6,scanTime_7,scanTime_8,scanTime_9,
                                 scanTime_10,scanTime_11,scanTime_12,scanTime_13,scanTime_14,scanTime_15};
       
            //Rev26.00 校正結果判定用パラメータ add by chouno 2017/01/06
            offsetCorDevRangeMin16inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "DevRangeMin16inchFPD", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorDevRangeMax16inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "DevRangeMax16inchFPD", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorMeanRangeMin16inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "MeanRangeMin16inchFPD", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorMeanRangeMax16inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "MeanRangeMax16inchFPD", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorDevRangeMin8inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "DevRangeMin8inchFPD", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorDevRangeMax8inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "DevRangeMax8inchFPD", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorMeanRangeMin8inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "MeanRangeMin8inchFPD", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorMeanRangeMax8inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "MeanRangeMax8inchFPD", "0", AppValue.CT30KProcessParaIniFileName));

            offsetCorDevRangeMin9inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "DevRangeMin9inchII", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorDevRangeMax9inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "DevRangeMax9inchII", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorMeanRangeMin9inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "MeanRangeMin9inchII", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorMeanRangeMax9inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "MeanRangeMax9inchII", "0", AppValue.CT30KProcessParaIniFileName));

            offsetCorDevRangeMin6inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "DevRangeMin6inchII", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorDevRangeMax6inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "DevRangeMax6inchII", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorMeanRangeMin6inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "MeanRangeMin6inchII", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorMeanRangeMax6inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "MeanRangeMax6inchII", "0", AppValue.CT30KProcessParaIniFileName));

            offsetCorDevRangeMin4_5inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "DevRangeMin4_5inchII", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorDevRangeMax4_5inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "DevRangeMax4_5inchII", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorMeanRangeMin4_5inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "MeanRangeMin4_5inchII", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorMeanRangeMax4_5inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "MeanRangeMax4_5inchII", "0", AppValue.CT30KProcessParaIniFileName));

            offsetCorDevRangeMin4inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "DevRangeMin4inchII", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorDevRangeMax4inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "DevRangeMax4inchII", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorMeanRangeMin4inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "MeanRangeMin4inchII", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorMeanRangeMax4inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "MeanRangeMax4inchII", "0", AppValue.CT30KProcessParaIniFileName));

            offsetCorDevRangeMin2inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "DevRangeMin2inchII", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorDevRangeMax2inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "DevRangeMax2inchII", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorMeanRangeMin2inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "MeanRangeMin2inchII", "0", AppValue.CT30KProcessParaIniFileName));
            offsetCorMeanRangeMax2inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgOffsetCorrect", "MeanRangeMax2inchII", "0", AppValue.CT30KProcessParaIniFileName));

            gainCorDevRangeMin16inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgGainCorrect", "DevRangeMin16inchFPD", "0", AppValue.CT30KProcessParaIniFileName));
            gainCorDevRangeMax16inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgGainCorrect", "DevRangeMax16inchFPD", "0", AppValue.CT30KProcessParaIniFileName));

            gainCorDevRangeMin8inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgGainCorrect", "DevRangeMin8inchFPD", "0", AppValue.CT30KProcessParaIniFileName));
            gainCorDevRangeMax8inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgGainCorrect", "DevRangeMax8inchFPD", "0", AppValue.CT30KProcessParaIniFileName));

            gainCorDevRangeMin9inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgGainCorrect", "DevRangeMin9inchII", "0", AppValue.CT30KProcessParaIniFileName));
            gainCorDevRangeMax9inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgGainCorrect", "DevRangeMax9inchII", "0", AppValue.CT30KProcessParaIniFileName));

            gainCorDevRangeMin6inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgGainCorrect", "DevRangeMin6inchII", "0", AppValue.CT30KProcessParaIniFileName));
            gainCorDevRangeMax6inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgGainCorrect", "DevRangeMax6inchII", "0", AppValue.CT30KProcessParaIniFileName));

            gainCorDevRangeMin4_5inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgGainCorrect", "DevRangeMin4_5inchII", "0", AppValue.CT30KProcessParaIniFileName));
            gainCorDevRangeMax4_5inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgGainCorrect", "DevRangeMax4_5inchII", "0", AppValue.CT30KProcessParaIniFileName));

            gainCorDevRangeMin4inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgGainCorrect", "DevRangeMin4inchII", "0", AppValue.CT30KProcessParaIniFileName));
            gainCorDevRangeMax4inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgGainCorrect", "DevRangeMax4inchII", "0", AppValue.CT30KProcessParaIniFileName));

            gainCorDevRangeMin2inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgGainCorrect", "DevRangeMin2inchII", "0", AppValue.CT30KProcessParaIniFileName));
            gainCorDevRangeMax2inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgGainCorrect", "DevRangeMax2inchII", "0", AppValue.CT30KProcessParaIniFileName));

            interceptRangeMin8inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "InterceptRangeMin8inchFPD", "0", AppValue.CT30KProcessParaIniFileName));
            interceptRangeMax8inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "InterceptRangeMax8inchFPD", "0", AppValue.CT30KProcessParaIniFileName));
            slopeRangeMin8inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "SlopeRangeMin8inchFPD", "0", AppValue.CT30KProcessParaIniFileName));
            slopeRangeMax8inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "SlopeRangeMax8inchFPD", "0", AppValue.CT30KProcessParaIniFileName));

            interceptRangeMin16inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "InterceptRangeMin16inchFPD", "0", AppValue.CT30KProcessParaIniFileName));
            interceptRangeMax16inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "InterceptRangeMax16inchFPD", "0", AppValue.CT30KProcessParaIniFileName));
            slopeRangeMin16inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "SlopeRangeMin16inchFPD", "0", AppValue.CT30KProcessParaIniFileName));
            slopeRangeMax16inchFPD = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "SlopeRangeMax16inchFPD", "0", AppValue.CT30KProcessParaIniFileName));

            interceptRangeMin9inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "InterceptRangeMin9inchII", "0", AppValue.CT30KProcessParaIniFileName));
            interceptRangeMax9inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "InterceptRangeMax9inchII", "0", AppValue.CT30KProcessParaIniFileName));
            slopeRangeMin9inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "SlopeRangeMin9inchII", "0", AppValue.CT30KProcessParaIniFileName));
            slopeRangeMax9inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "SlopeRangeMax9inchII", "0", AppValue.CT30KProcessParaIniFileName));

            interceptRangeMin6inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "InterceptRangeMin6inchII", "0", AppValue.CT30KProcessParaIniFileName));
            interceptRangeMax6inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "InterceptRangeMax6inchII", "0", AppValue.CT30KProcessParaIniFileName));
            slopeRangeMin6inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "SlopeRangeMin6inchII", "0", AppValue.CT30KProcessParaIniFileName));
            slopeRangeMax6inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "SlopeRangeMax6inchII", "0", AppValue.CT30KProcessParaIniFileName));

            interceptRangeMin4_5inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "InterceptRangeMin4_5inchII", "0", AppValue.CT30KProcessParaIniFileName));
            interceptRangeMax4_5inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "InterceptRangeMax4_5inchII", "0", AppValue.CT30KProcessParaIniFileName));
            slopeRangeMin4_5inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "SlopeRangeMin4_5inchII", "0", AppValue.CT30KProcessParaIniFileName));
            slopeRangeMax4_5inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "SlopeRangeMax4_5inchII", "0", AppValue.CT30KProcessParaIniFileName));

            interceptRangeMin4inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "InterceptRangeMin4inchII", "0", AppValue.CT30KProcessParaIniFileName));
            interceptRangeMax4inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "InterceptRangeMax4inchII", "0", AppValue.CT30KProcessParaIniFileName));
            slopeRangeMin4inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "SlopeRangeMin4inchII", "0", AppValue.CT30KProcessParaIniFileName));
            slopeRangeMax4inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "SlopeRangeMax4inchII", "0", AppValue.CT30KProcessParaIniFileName));

            interceptRangeMin2inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "InterceptRangeMin2inchII", "0", AppValue.CT30KProcessParaIniFileName));
            interceptRangeMax2inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "InterceptRangeMax2inchII", "0", AppValue.CT30KProcessParaIniFileName));
            slopeRangeMin2inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "SlopeRangeMin2inchII", "0", AppValue.CT30KProcessParaIniFileName));
            slopeRangeMax2inchII = Convert.ToSingle(inifile.GetFileIniString("AutoJdgScanPosCorrect", "SlopeRangeMax2inchII", "0", AppValue.CT30KProcessParaIniFileName));
            //<---

            //ゲイン校正時の機構部位置 Rev26.00 2017/01/17 --->
            GainCorTableY = Convert.ToSingle(inifile.GetFileIniString("GainCorrectionMechaPos","TableY","0",AppValue.CT30KProcessParaIniFileName));
            GainCorFCD = Convert.ToSingle(inifile.GetFileIniString("GainCorrectionMechaPos","FCD","0",AppValue.CT30KProcessParaIniFileName));
            GainCorFDD = Convert.ToSingle(inifile.GetFileIniString("GainCorrectionMechaPos","FDD","0",AppValue.CT30KProcessParaIniFileName));
            //<---

            //Rev26.00 ファントムレスBHC用プロパティ追加by井上 2017/04/05
            BHCMaterialListFileName = inifile.GetFileIniString("PhantomlessBHC", "BHCMaterialList", "", AppValue.CT30KProcessParaIniFileName);

            guideImgQualityIndex0 = Convert.ToInt32(inifile.GetFileIniString("ScanQualityIndexForGuideTab", "guideImgQualityIndex0", "0", AppValue.CT30KProcessParaIniFileName));
            guideImgQualityIndex1 = Convert.ToInt32(inifile.GetFileIniString("ScanQualityIndexForGuideTab", "guideImgQualityIndex1", "0", AppValue.CT30KProcessParaIniFileName));
            guideImgQualityIndex2 = Convert.ToInt32(inifile.GetFileIniString("ScanQualityIndexForGuideTab", "guideImgQualityIndex2", "0", AppValue.CT30KProcessParaIniFileName));
            guideImgQualityIndex3 = Convert.ToInt32(inifile.GetFileIniString("ScanQualityIndexForGuideTab", "guideImgQualityIndex3", "0", AppValue.CT30KProcessParaIniFileName));
            guideImgQualityIndex = new int[] { guideImgQualityIndex0, guideImgQualityIndex1, guideImgQualityIndex2, guideImgQualityIndex3 };

            //Rev26.40 外観カメラパラメータ by chouno 2019/02/12
            FixedCamPosition = Convert.ToInt32(inifile.GetFileIniString("MonitorCam", "FixedCamPosition", "1", AppValue.CT30KProcessParaIniFileName));
            FixedCamFCD = Convert.ToSingle(inifile.GetFileIniString("MonitorCam", "FixedCamFCD", "300", AppValue.CT30KProcessParaIniFileName));
            FixedCamTableY = Convert.ToSingle(inifile.GetFileIniString("MonitorCam", "FixedCamTableY", "0", AppValue.CT30KProcessParaIniFileName));
            FixedCamTableZ = Convert.ToSingle(inifile.GetFileIniString("MonitorCam", "FixedCamTableZ", "0", AppValue.CT30KProcessParaIniFileName));

            //Rev26.40 高速透視パラメータ by chouno 2019/02/12
            HSCSettingType = Convert.ToInt32(inifile.GetFileIniString("HighSpeedCamera","HSCSettingType","0",AppValue.CT30KProcessParaIniFileName));
            HSCModeType = Convert.ToInt32(inifile.GetFileIniString("HighSpeedCamera", "HSCModeType", "0", AppValue.CT30KProcessParaIniFileName));
            HSCWarningSoundMode = Convert.ToInt32(inifile.GetFileIniString("HighSpeedCamera", "HSCWarningSoundMode","0",AppValue.CT30KProcessParaIniFileName));
            HSCWarningSoundTime = Convert.ToInt32(inifile.GetFileIniString("HighSpeedCamera", "HSCWarningSoundTime", "1", AppValue.CT30KProcessParaIniFileName));
            HSCWarningMessageMethod = Convert.ToInt32(inifile.GetFileIniString("HighSpeedCamera", "HSCWarningMessageMethod", "0", AppValue.CT30KProcessParaIniFileName));

            //Rev26.40 X線・高速度カメラ昇降ストローク 2019/03/04
            XrayAndHSCMaxPos = Convert.ToInt32(inifile.GetFileIniString("DropTestMode", "XrayAndHSCMaxPos", "0", AppValue.CT30KProcessParaIniFileName));
            XrayAndHSCMinPos = Convert.ToInt32(inifile.GetFileIniString("DropTestMode", "XrayAndHSCMinPos", "0", AppValue.CT30KProcessParaIniFileName));

            //Rev26.40 X線・高速度カメラ昇降タイムアウト 2019/03/04
            XrayAndHSCTimeOut = Convert.ToInt32(inifile.GetFileIniString("DropTestMode", "XrayAndHSCTimeOut", "0", AppValue.CT30KProcessParaIniFileName));
        }

        private int[] steppwuKV = new int[6];

    }
}
