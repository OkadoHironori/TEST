using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using XrayCtrl;
using System.Runtime.InteropServices;

using CTAPI;
using CT30K.Common;
using CT30K.Properties;
using TransImage;

namespace CT30K
{
    public static class  modXrayAutoVI
    {

        //追加2017/01/19hata
        private static bool bXrayAutoVI = false;
        private static bool bXrayAutoVICancel = false;

        /// <summary>
        /// 自動X線VI設定(Mode指定）
        /// </summary>
        /// <param name="mode">開始時に設定する電圧電流設定モード (0:現在値 1:High 2:Mid 3:Low)</param>
        public static void XrayAutoVIModeProc(int mode)
        {
             int x1 = CTSettings.iniValue.AutoviStartX;
             int x2 = CTSettings.iniValue.AutoviEndX;
             int y1 = CTSettings.iniValue.AutoviStartY;
             int y2 = CTSettings.iniValue.AutoviEndY;

             Rectangle rect = new Rectangle(x1, y1, x2 - x1 + 1, y2 - y1 + 1);
             XrayAutoVIModeProc(mode, rect);

        }

        
        /// <summary>
        /// 自動X線VI設定(Mode指定、エリア指定）
        /// </summary>
        /// <param name="mode">開始時に設定する電圧電流設定モード (0:現在値 1:High 2:Mid 3:Low)</param>
        /// <param name="rect">グレイ値調査エリア</param>
        public static void XrayAutoVIModeProc(int mode, Rectangle rect)
        {
            float volt = 0f;
            float current = 0f;

            float XvoltMax = (float)frmXrayControl.Instance.cwneKV.Maximum;
            float XcurrentMax = (float)frmXrayControl.Instance.cwneMA.Maximum;

            switch (mode)
            {
                case 0:　//現在の設定値
                    volt = (float)frmXrayControl.Instance.ntbSetVolt.Value;
                    current = (float)frmXrayControl.Instance.ntbSetCurrent.Value;
                    break;

                case 1:  //High
                    volt = CTSettings.iniValue.AutoviVoltHigh;
                    current = CTSettings.iniValue.AutoviCurrentHigh;
                    break;

                case 2:  //Mid
                    volt = CTSettings.iniValue.AutoviVoltMid;
                    current = CTSettings.iniValue.AutoviCurrentMid;
                    break;

                case 3:  //Low
                    volt = CTSettings.iniValue.AutoviVoltLow;
                    current = CTSettings.iniValue.AutoviCurrentLow;
                    break;

                default:
                    return;

            }
            XrayAutoVIProc(volt, current, XvoltMax, XcurrentMax, rect);
        }

       
        /// <summary>
        /// 自動X線VI処理中止
        /// </summary>
        public static void XrayAutoVICancel()
        {
            bXrayAutoVICancel = true;
        }

         /// <summary>
        /// 自動X線VI設定(デフォルトX線電圧電流指定、エリア指定）
        /// </summary>
        /// <param name="StartVolt">デフォルトX線電圧</param>
        /// <param name="StartCurrent">デフォルトX線電流</param>
        /// <param name="MaxVolt">X線の設定最大電圧</param>
        /// <param name="MaxCurrent">X線の設定最大電流</param>
        /// <param name="rect">グレイ値調査エリア</param>
        public static void XrayAutoVIProc(float StartVolt, float StartCurrent, float MaxVolt, float MaxCurrent, Rectangle rect)
        {
            if (bXrayAutoVI)
            {
                //処理中なら抜ける
                return;
            }
            bXrayAutoVICancel = false;

            int minVal = 0;
            int maxVal = 0;
            float div = 0.0f;
            float mean = 0.0f;
            //int GrayMax = 65535;
            //Rev26.00 オフセット量を考慮しGrayMaxを場合分け
            int GrayMax = 0;
            if(CTSettings.detectorParam.DetType == DetectorConstants.DetTypeII || CTSettings.detectorParam.DetType == DetectorConstants.DetTypeHama)
            {
                GrayMax = 4096;
            }
            else
            {
                if (CTSettings.t20kinf.Data.pki_fpd_type == 2)
                {
                    GrayMax = 55535; //GigE 8inch 10000程度 オフセットが乗るため
                }
                else
                {
                    GrayMax = 60535; //それ以外だと4,5000程度
                }
            }
            
            int GrayRange = 0;
            //int Upvolt = 10;
            //int Dwnevolt = 10;

            int minValCenter = 0;
            int maxValCenter = 0;
            float divCenter = 0.0f;
            float meanCenter = 0.0f;
            int GrayRangeCenter = 0;

            int Cnt = 0;
            int EndCnt = 5;
            bool bXampChange = false;
            bool bXvoltChange = false;
            //int intXvoltCnt = 0;
            //float BefMeanGray = 0f;
            //float BefSetVolt = 0f;
            //float BefSetAmp = 0f;

            float HistoMaxThresholdRate = 0f;
            float HistoMinThresholdRate = 0f;
            float HistoRangeThresholdRate = 0f;
            float VoltUpStep = 0f;
            float VoltDownStep = 0f;
            int LoopNumber = 0;
            float UpRate = 0f;
            float DownRate = 0f;
            int step = 0;

            float HistoMeanThresholdRate = 0f;
            float VoltMaxLimitRate = 0f;
            float CurrentMaxLimitRate = 0f;

            float VoltLimitRate = 0f;
            float CurrentLimitRate = 0f;
            bool bLaststep = false;
            bool bWithSetImageOn = false;
  
            int h_size = frmTransImage.Instance.TransImageCtrl.ImageSize.Width;
            int v_size = frmTransImage.Instance.TransImageCtrl.ImageSize.Height;
            ushort[] WordImage = new ushort[h_size * v_size];

            //最小値・最大値の探索範囲を決める
            //PkeFPDの場合の額縁幅を計算する 'v17.53追加 byやまおか 2011/05/13
            int modX = 0;
            int modY = 0;
            if ((CTSettings.detectorParam.DetType == DetectorConstants.DetTypePke) && (!CTSettings.detectorParam.Use_FpdAllpix))
            {
                modX = Convert.ToInt32((frmTransImage.Instance.ctlTransImage.SizeX % 100) / 2F);
                modY = Convert.ToInt32((frmTransImage.Instance.ctlTransImage.SizeY % 100) / 2F);
            }
            else
            {
                modX = 0;
                modY = 0;
            }
            int StartXIndex = rect.X + modX;
            int EndXIndex = rect.Right - modX;
            int StartYIndex = rect.Y + modY;
            int EndYIndex = rect.Bottom - modY;

            if (StartXIndex > h_size - 1 || StartYIndex > v_size - 1)
                return;
            if (EndXIndex > h_size - 1)
                EndXIndex = h_size - 1;

            if (EndYIndex > v_size - 1)
                EndYIndex = v_size - 1;

            HistoMaxThresholdRate = CTSettings.iniValue.AutoviHistoMaxThresholdRate;//ヒストグラム最大閾値(率)（レンジ幅のの20%)
            HistoMinThresholdRate = CTSettings.iniValue.AutoviHistoMinThresholdRate;//ヒストグラム最少閾値(率)（レンジ幅のの20%)
            HistoRangeThresholdRate = CTSettings.iniValue.AutoviHistoRangeThresholdRate;//ヒストグラムレンジ最少閾値(率)（レンジ幅のの20%)
            VoltUpStep = CTSettings.iniValue.AutoviVoltUpStep;//電圧アップステップ
            VoltDownStep = CTSettings.iniValue.AutoviVoltDownStep;//電圧ダウンステップ
            LoopNumber = CTSettings.iniValue.AutoviLoopNumber;//繰り返し回数
            UpRate = CTSettings.iniValue.AutoviCurrentUpRate;//電流アップ(率)1.5倍）
            DownRate = CTSettings.iniValue.AutoviCurrentDownRate;//電流ダウン(率)（0.5倍）

            HistoMeanThresholdRate = CTSettings.iniValue.AutoviHistoMeanThresholdRate;        //グレイ平均値の閾値(率)（レンジ幅のの50%)
            VoltMaxLimitRate = CTSettings.iniValue.AutoviVoltMaxLimitRate;               //最大電圧の制限(率)（最大電圧の90%)
            CurrentMaxLimitRate = CTSettings.iniValue.AutoviCurrentMaxLimitRate;         //最大電流の制限(率)（最大電流の80%)
            EndCnt = LoopNumber;
 
            //FPDの使用有無でﾋﾞｯﾄを判断
            //FPD は16bit
            //I.I.は12bit
            if (!CTSettings.detectorParam.Use_FlatPanel)
                GrayMax = 4095;

            //X線設定値
            float Xvolt = StartVolt;
            float Xcurrent = StartCurrent;
            float XvoltMax = MaxVolt;
            float XcurrentMax = MaxCurrent;
            string XraySts = frmXrayControl.Instance.XrayStatus;

            //準備完了でない場合は抜ける
            //X線ON中
            if ((XraySts != StringTable.GC_STS_STANDBY_OK) &&
                 (XraySts != StringTable.GC_Xray_On))
                return;

            bXrayAutoVI = true;

            //デフォルト値をセットする
            if((float)frmXrayControl.Instance.ntbSetVolt.Value != StartVolt)
                modXrayControl.SetVolt(modLibrary.CorrectInRange(Xvolt, 0, XvoltMax));
            modCT30K.PauseForDoEvents(1f);
            if((float)frmXrayControl.Instance.ntbSetCurrent.Value != StartCurrent)
                modXrayControl.SetCurrent(modLibrary.CorrectInRange(Xcurrent, 0, XcurrentMax));

            //WithSetImageOnをOnにする
            bWithSetImageOn = frmTransImage.Instance.TransImageCtrl.WithSetImageOn;
            if (!frmTransImage.Instance.TransImageCtrl.WithSetImageOn)
            {
                frmTransImage.Instance.TransImageCtrl.WithSetImageOn = true;

                if (frmTransImage.Instance.CaptureOn)
                    modCT30K.PauseForDoEvents(1f);
            }

            //ライブ画像オンにする
            if (!frmTransImage.Instance.CaptureOn)
                frmTransImage.Instance.CaptureOn = true;

            while (bXrayAutoVI)
            {
                //繰り返しカウントEndなら抜ける
                if (Cnt >= EndCnt)
                    break;

                //キャンセルならなら抜ける
                if (bXrayAutoVICancel)
                    break;

                //エラーなら抜ける（中断）
                XraySts = frmXrayControl.Instance.XrayStatus;
                if ((XraySts != StringTable.GC_STS_STANDBY_OK) &&
                    (XraySts != StringTable.GC_Xray_On))
                    break;


                //アベイラブル監視
                if ((frmXrayControl.Instance.MecaXrayAvailable == modCT30K.OnOffStatusConstants.OnStatus))
                {
                    //画像データ取得
                    WordImage = frmTransImage.Instance.TransImageCtrl.GetImage();

                    //グレイMax、グレイMin、グレイ平均、標準偏差　を取得
                    //画像全体のデータを取得(20pixel内側にする)
                    ScanCorrect.GetStatisticalInfo(ref WordImage[0],
                                    h_size,
                                    v_size,
                                    20 + modX,
                                    h_size -1 - 20 - modX,
                                    20 + modY,
                                    v_size -1 - 20 - modY,
                                    ref minVal,
                                    ref maxVal,
                                    ref div,
                                    ref mean);

                    //BefSetVolt = Xvolt;
                    //BefSetAmp = Xcurrent;

                    bXvoltChange = false;
                    bXampChange = false;
                    GrayRange = maxVal - minVal;

                    //Debug.Print("GrayMax = " + maxVal + " GrayMin = " + minVal + " GrayMean = " + mean + " Graydiv = " + div);

                    //指定位置で実施
                    //グレイMax、グレイMin、グレイ平均、標準偏差　を取得
                    ScanCorrect.GetStatisticalInfo(ref WordImage[0],
                                    h_size,
                                    v_size,
                                    StartXIndex,
                                    EndXIndex,
                                    StartYIndex,
                                    EndYIndex,
                                    ref minValCenter,
                                    ref maxValCenter,
                                    ref divCenter,
                                    ref meanCenter);

                    GrayRangeCenter = maxValCenter - minValCenter;

                    //Debug.Print("C_Max = " + maxValCenter + " C_Min = " + minValCenter + " C_Range = " + GrayRangeCenter + " C_Mean = " + meanCenter + " C_Div = " + divCenter);


                    if ((divCenter / (float)maxValCenter) < 0.2f)
                    {
                        VoltLimitRate =  VoltMaxLimitRate * 0.9f;
                        CurrentLimitRate = CurrentMaxLimitRate * 0.9f;
                    }
                    else
                    {
                        VoltLimitRate = VoltMaxLimitRate;
                        CurrentLimitRate = CurrentMaxLimitRate;
                    }

                    if (step == 0)
                    {
                        for (int i = 0; i < 5; ++i)
                        {
                            switch (i)
                            {
                                case 0:
                                    //これでは機能しない
                                    //ヒストグラムレンジ最大閾値 > ヒストグラム最大値　or
                                    //ヒストグラムレンジ最少閾値 < ヒストグラム最小値
                                    //if ((maxValCenter < GrayMax * HistoMaxThresholdRate) ||
                                    //    (minValCenter > GrayMax * HistoMinThresholdRate))
                                    //{
                                    //    //どっちか一方だけだったら次へ
                                    //}
                                    //else
                                    //{
                                    //    //両側の範囲を超えていたら、電流を下げる
                                    //    //電流を下げる
                                    //    bXampChange = true;
                                    //    Xcurrent = (int)(Xcurrent * DownRate);
                                    //}

                                    //ヒストグラムレンジ最大閾値 < ヒストグラム最大値
                                    if (XvoltMax * VoltLimitRate > Xvolt)
                                    {
                                        if (maxValCenter > GrayMax * HistoMaxThresholdRate)
                                        {
                                            //ここでは大きめに下げる
                                            //電流を下げる（ DownRate * 0.9とする）
                                            bXampChange = true;
                                            Xcurrent = (int)(Xcurrent * DownRate * 0.9);
                                        }
                                    }

                                    break;

                                case 1:
                                    ////ヒストグラムレンジ最少閾値 < ヒストグラムレンジ
                                    //if (!bXampChange && (GrayRangeCenter > GrayMax * HistoRangeThresholdRate))
                                    //{
                                    //    //次へ
                                    //}
                                    //else
                                    //{
                                    //    if (XcurrentMax * 0.8 >= Xcurrent)
                                    //    {
                                    //        //電流を上げる
                                    //        bXampChange = true;
                                    //        Xcurrent = (int)(Xcurrent * UpRate);
                                    //    }
                                    //}
                                    break;

                                case 2:
                                    //ヒストグラムレンジ最大閾値 > ヒストグラム最大値
                                    if (!bXampChange && (maxValCenter < GrayMax * HistoMaxThresholdRate))
                                    {
                                        //次へ
                                    }
                                    else
                                    {
                                        //電圧を下げる
                                        bXvoltChange = true;
                                        Xvolt -= VoltDownStep;
                                    }
                                    break;

                                case 3:

                                    //ヒストグラムレンジ最少閾値 < ヒストグラム最小値
                                    if (!bXampChange && !bXvoltChange && (minValCenter > GrayMax * HistoMinThresholdRate))
                                    {
                                        //次へ
                                    }
                                    else
                                    {
                                        //if (XvoltMax * 0.8 > Xvolt)VoltLimitRate
                                        //if (XvoltMax * VoltMaxLimitRate * 0.9 > Xvolt)
                                        if (XvoltMax * VoltLimitRate * 0.9 > Xvolt)
                                        {
                                            //電圧を上げる
                                            bXvoltChange = true;
                                            Xvolt += VoltUpStep;
                                        }
                                    }
                                    break;

                                case 4:
                                    //ヒストグラムレンジ最少閾値 < ヒストグラムレンジ
                                    //if (!bXampChange && !bXvoltChange && (mean > GrayMax * 0.35))
                                    if (!bXampChange && !bXvoltChange && (GrayRangeCenter > GrayMax * HistoRangeThresholdRate))
                                    {
                                        //次へ
                                    }
                                    else
                                    {
                                        // if (XcurrentMax * 0.5 > Xcurrent)
                                        //if (XcurrentMax * CurrentMaxLimitRate * 0.8 > Xcurrent)
                                        if (XcurrentMax * CurrentLimitRate * 0.8 > Xcurrent)
                                        {
                                            //電流を上げる
                                            bXampChange = true;
                                            Xcurrent = (int)(Xcurrent * UpRate);
                                        }
                                    }
                                    break;
                            }

                            if (bXvoltChange || bXampChange)
                                break;
                        }

                        if (!bXvoltChange && !bXampChange)
                            step = 1;
                    }

                    if (step == 1)
                    {
                        float fval = 0;
                        int val = 0;
                        if ((int)(GrayMax * (HistoMaxThresholdRate + 0.1f)) < (int)(GrayMax * 0.9f))
                            val = (int)(GrayMax * (HistoMaxThresholdRate + 0.1f));
                        else
                            val = (int)(GrayMax * 0.9f);

                        // if (maxVal > GrayMax * 0.9) 
                        if (maxValCenter >  val)
                        {
                            //電流を下げる
                            bXampChange = true;
                            fval = DownRate * 1.05f;
                            if (fval > 0.95f)
                                fval = 0.95f;

                            Xcurrent = (int)(Xcurrent * fval);
                            bLaststep = true;
                        }

                        //if ((maxVal < GrayMax * HistoMaxThresholdRate)
                        //else if ((maxVal > GrayMax * 0.7 || mean > GrayMax * 0.35))
                        else if ((maxValCenter > GrayMax * HistoMaxThresholdRate || meanCenter > GrayMax * HistoMeanThresholdRate))
                        {
                            Cnt = EndCnt;
                            break;
                        }
                        else
                        {
                            if (bLaststep)
                            {
                                Cnt = EndCnt;
                                break;
                            }

                            //if (XvoltMax * 0.9 > Xvolt)
                            //if (XvoltMax * VoltMaxLimitRate > Xvolt)
                            if (XvoltMax * VoltLimitRate > Xvolt)
                            {
                                //電圧を上げる
                                bXvoltChange = true;
                                Xvolt += VoltUpStep;

                            }
                            //else if (XcurrentMax * 0.8 > Xcurrent)
                            //else if (XcurrentMax * CurrentMaxLimitRate > Xcurrent)
                            else if (XcurrentMax * CurrentLimitRate > Xcurrent)
                            {
                                //電流を上げる
                                bXampChange = true;
                                Xcurrent = (int)(Xcurrent * UpRate);
                            }
                        }

                        if (!bXvoltChange && !bXampChange)
                            break;
                    }

                    //if ((div / (float)maxVal) < 0.02f && (div / (float)mean) < 0.02f)
                    //{
                    //    //対象サンプルがない状態
                    //    break;
                    //}

                    //BefMeanGray = mean;

                    //ステータス確認
                    XraySts = frmXrayControl.Instance.XrayStatus;
                    if ((XraySts != StringTable.GC_STS_STANDBY_OK) &&
                        (XraySts != StringTable.GC_Xray_On))
                        break;

                    if (bXvoltChange)
                    {
                        //電圧を設定
                        Xvolt = (float)modLibrary.CorrectInRange(Xvolt, 0, XvoltMax);
                        modXrayControl.SetVolt(Xvolt);
                    }
                    else if (bXampChange)
                    {
                        //電流を設定
                        Xcurrent = (float)modLibrary.CorrectInRange(Xcurrent, 0, XcurrentMax);
                        modXrayControl.SetCurrent(Xcurrent);
                    }

                    //Debug.Print("SetVolt = " + Xvolt + " SetCurrent = " + Xcurrent);

                    Cnt += 1;
                    modCT30K.PauseForDoEvents(1f);
                }

                modCT30K.PauseForDoEvents(0.5f);
            }

            //撮影を止めるか？
            //if (frmTransImage.Instance.CaptureOn)
            //    frmTransImage.Instance.CaptureOn = false;

            //WithSetImageOnを設定前の戻す
            frmTransImage.Instance.TransImageCtrl.WithSetImageOn = bWithSetImageOn;
            
            WordImage = null;
            bXrayAutoVI = false;
            bXrayAutoVICancel = false;
        }



    }
}
