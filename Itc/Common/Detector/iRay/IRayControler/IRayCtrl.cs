using iDetector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IRayControler
{
    /// <summary>
    /// iRay検出器制御クラス
    /// </summary>
    public class IRayCtrl: IIRayCtrl, IDisposable
    {
        /// <summary>
        /// GUIへのメッセージ
        /// </summary>
        public string GUIMessage { get; private set; } = string.Empty;
        /// <summary>
        /// ステータス変更時のイベント
        /// </summary>
        public event EventHandler StsChanged;
        /// <summary>
        /// 初期化
        /// </summary>
        public bool IsInitilized { get; private set; }
        /// <summary>
        /// 選択しているモード
        /// </summary>
        public string SelectMode { get; private set; }
        /// <summary>
        /// 校正設定パラメータ 0x000形式
        /// </summary>
        public int CorrectionParam { get; private set; }
        /// <summary>
        /// オフセットフレーム数
        /// </summary>
        public int TotalDarkFrames { get; private set; }
        /// <summary>
        /// ゲインフレーム数
        /// </summary>
        public int TotalLightFrames { get; private set; }
        /// <summary>
        /// IRAYのID
        /// </summary>
        public int DetectorID { get; private set; }
        /// <summary>
        /// 収集データ
        /// </summary>
        public IntPtr AcqData { get; private set; }
        /// <summary>
        /// Dll確認完了
        /// </summary>
        public event EventHandler EndConnetIRay;
        /// <summary>
        /// 
        /// </summary>
        private readonly IIRayDetector _IRayDet;
        /// <summary>
        /// モード変更完了イベント
        /// </summary>
        public event EventHandler EndChageMode;
        /// <summary>
        /// 校正初期化完了イベント
        /// </summary>
        public event EventHandler EndCorrectInit;
        /// <summary>
        /// オフセットデータ収集完了イベント
        /// </summary>
        public event EventHandler EndAcqOffset;
        /// <summary>
        /// データ収集完了イベント
        /// </summary>
        public event EventHandler EndAcqData;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_iraydet"></param>
        public IRayCtrl(IIRayDetector iraydet)
        {
            _IRayDet = iraydet;
            _IRayDet.EndStartupIRayClass += (s, e) =>
            {
                var ird = s as IRayDetector;
                IsInitilized = ird.IsInitilized;
                DetectorID = ird.DetectorID;
                if (IsInitilized)
                {
                    int ret = _IRayDet.SyncInvoke(SdkInterface.Cmd_Connect, 20000);
                    if (SdkInterface.Err_OK != ret)
                    {
                        GUIMessage = _IRayDet.GetErrorInfo(ret);
                        Debug.WriteLine($"エラーメッセージ{GUIMessage}");
                        StsChanged?.Invoke(this, new EventArgs());
                        return;
                    }
                    EndConnetIRay?.Invoke(this, new EventArgs());
                }
            };
            _IRayDet.IRayClassStart(OnSdkCallback);
        }
        /// <summary>
        /// IRayCallback
        /// </summary>
        /// <param name="nEventID"></param>
        /// <param name="nEventLevel"></param>
        /// <param name="strMsg"></param>
        /// <param name="nParam1"></param>
        /// <param name="nParam2"></param>
        /// <param name="nPtrParamLen"></param>
        /// <param name="pParam"></param>
        public void OnSdkCallback(
            int nEventID,
            int nEventLevel,
            string strMsg,
            int nParam1,
            int nParam2,
            int nPtrParamLen,
            IntPtr pParam)
        {
            switch (nEventID)
            {
                case SdkInterface.Evt_Exp_Enable:
                    break;
                case SdkInterface.Evt_Image:
                    Debug.Write("Got image\n");
                    {
                        //must make deep copies of pParam
                        IRayImage image = (IRayImage)Marshal.PtrToStructure(pParam, typeof(IRayImage));

                        AcqData = image.pData;

                        EndAcqData?.Invoke(this, new EventArgs());

                        var vars = new IRayVariantMapItem[image.propList.nItemCount];
                        SdkParamConvertor<IRayVariantMapItem>.IntPtrToStructArray(image.propList.pItems, ref vars);
                        foreach (IRayVariantMapItem item in vars)
                        {
                            //parse the image frame no
                            if ((int)Enm_ImageTag.Enm_ImageTag_FrameNo == item.nMapKey)
                            {
                                Debug.WriteLine("Frame no:{0}", item.varMapVal.val.nVal);
                                break;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// モード変更
        /// </summary>
        /// <param name="mode"></param>
        public void ChangeMode(string mode)
        {
            //"Mode1" defined in DynamicApplicationMode.ini subset=Mode1
            var ret = _IRayDet.SyncInvoke(SdkInterface.Cmd_SetCaliSubset, mode, 5000);
            if (SdkInterface.Err_OK != ret)
            {
                GUIMessage = _IRayDet.GetErrorInfo(ret);
                StsChanged?.Invoke(this, new EventArgs());
                return;
            }
            SelectMode = mode;

            EndChageMode?.Invoke(this, new EventArgs());

        }
        /// <summary>
        /// 校正の設定
        /// </summary>
        public void SetCorrection()
        {
            int correction = (int)(Enm_CorrectOption.Enm_CorrectOp_SW_PreOffset | Enm_CorrectOption.Enm_CorrectOp_SW_Gain | Enm_CorrectOption.Enm_CorrectOp_SW_Defect);
            var ret = _IRayDet.SyncInvoke(SdkInterface.Cmd_SetCorrectOption, correction, 5000);
            if (SdkInterface.Err_OK != ret)
            {
                GUIMessage = _IRayDet.GetErrorInfo(ret);
                StsChanged?.Invoke(this, new EventArgs());
                return;
            }

        }
        /// <summary>
        /// データ収集
        /// </summary>
        public void DoDataAcq()
        {
            _IRayDet.Invoke(SdkInterface.Cmd_StartAcq);
        }
        /// <summary>
        /// データ収集完了
        /// </summary>
        public void DoStopDataAcq()
        {
            _IRayDet.SyncInvoke(SdkInterface.Cmd_StopAcq, 2000);
        }

        /// <summary>
        /// 校正初期化
        /// </summary>
        public void DoInitCorrection()
        {
            int ret = _IRayDet.SetAttr(SdkInterface.Cfg_CalibrationFlow, 1);//none zero
            if (SdkInterface.Err_OK != ret)
            {
                Debug.WriteLine($"InitCalibration failed!err={_IRayDet.GetErrorInfo(ret)}");
            }

            ret = _IRayDet.SyncInvoke(SdkInterface.Cmd_CalibrationInit, 5000);
            if (SdkInterface.Err_OK == ret)
            {
                TotalDarkFrames = _IRayDet.GetAttrInt(SdkInterface.Attr_OffsetTotalFrames);
                TotalLightFrames = _IRayDet.GetAttrInt(SdkInterface.Attr_GainTotalFrames);
            }
            else
            {
                throw new Exception("校正データ初期化失敗のお知らせ");
            }

            EndCorrectInit?.Invoke(this, new EventArgs());
        }

        public void DoAcqOffset()
        {
            var ret = _IRayDet.Invoke(SdkInterface.Cmd_ForceDarkContinuousAcq, 0);
            
            //オフセット
            while (true)
            {
                var offsetframe = _IRayDet.GetAttrInt(SdkInterface.Attr_OffsetValidFrames);
                if (offsetframe == TotalDarkFrames)
                {
                    break;
                }

                Debug.WriteLine($"オフセット校正用のデータ収集中..{offsetframe}/{TotalDarkFrames}");

                Task.WaitAll(Task.Delay(500));
            }

            ret = _IRayDet.Invoke(SdkInterface.Cmd_OffsetGeneration);
            if (SdkInterface.Err_OK != ret)
            {
                Debug.WriteLine($"Generate offset map failed! err={_IRayDet.GetErrorInfo(ret)}");
            }

            int cnt = 0;
            int cntmax = 20;
            while (true)
            {
                if (cnt > cntmax)
                {
                    break;
                }

                Debug.WriteLine($"X線をONしてください{cnt}/{cntmax}");

                Task.WaitAll(Task.Delay(500));

                cnt++;
            }

            ret = _IRayDet.Invoke(SdkInterface.Cmd_StartAcq);
            if (SdkInterface.Err_OK != ret)
            {
                Debug.WriteLine($"Generate offset map failed! err={_IRayDet.GetErrorInfo(ret)}");
            }            
            //ゲイン
            while (true)
            {
                var gainframe = _IRayDet.GetAttrInt(SdkInterface.Attr_GainValidFrames);
                if (gainframe == TotalLightFrames)
                {
                    break;
                }

                Debug.WriteLine($"ゲイン校正用のデータ収集中..{gainframe}/{TotalLightFrames}");

                Task.WaitAll(Task.Delay(500));
            }

            ret = _IRayDet.Invoke(SdkInterface.Cmd_GainGeneration);
            if (SdkInterface.Err_OK != ret)
            {
                Debug.WriteLine($"InitCalibration failed!err={_IRayDet.GetErrorInfo(ret)}");
            }

            ret = _IRayDet.Invoke(SdkInterface.Cmd_DefectGeneration);
            if (SdkInterface.Err_OK != ret)
            {
                Debug.WriteLine($"InitCalibration failed!err={_IRayDet.GetErrorInfo(ret)}");
            }


            //完了
            ret = _IRayDet.Invoke(SdkInterface.Cmd_FinishGenerationProcess);
            if (SdkInterface.Err_OK != ret)
            {
                Debug.WriteLine($"InitCalibration failed!err={_IRayDet.GetErrorInfo(ret)}");
            }


            EndAcqOffset?.Invoke(this, new EventArgs());
        }

        public void OnAcquiringDarkTimer(object state)
        {
            //int nValid = GetValidDarkFrames();
            //mProgressBar.SetProgress(nValid, TotalDarkFrames);
            //if (nValid == TotalDarkFrames)
            //{
            //    mMonitorTimer.Dispose();
            //    Console.Write("\n");
            //    mNextStepEvent.Set();
            //}
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            var ret = _IRayDet.Abort();
            if (SdkInterface.Err_OK != ret)
            {
                Debug.WriteLine($"InitCalibration failed!err={_IRayDet.GetErrorInfo(ret)}");
            }

            _IRayDet.DestroyDetector(DetectorID);

            ////完了
            //ret = _IRayDet.Invoke(SdkInterface.Cmd_Disconnect);
            //if (SdkInterface.Err_OK != ret)
            //{
            //    Debug.WriteLine($"InitCalibration failed!err={_IRayDet.GetErrorInfo(ret)}");
            //}


        }
    }
    /// <summary>
    /// iRay検出器制御I/F
    /// </summary>
    public interface IIRayCtrl
    {
        event EventHandler EndConnetIRay;
        /// <summary>
        /// モード変更
        /// </summary>
        void ChangeMode(string mode);
        /// <summary>
        /// モード変更完了イベント
        /// </summary>
        event EventHandler EndChageMode;
        /// <summary>
        /// 校正初期化
        /// </summary>
        void DoInitCorrection();
        /// <summary>
        /// 校正初期化完了イベント
        /// </summary>
        event EventHandler EndCorrectInit;
        /// <summary>
        /// オフセットデータ収集
        /// </summary>
        void DoAcqOffset();
        /// <summary>
        /// データ収集
        /// </summary>
        void DoDataAcq();
        /// <summary>
        /// データ収集完了
        /// </summary>
        void DoStopDataAcq();
        /// <summary>
        /// 校正の設定
        /// </summary>
        void SetCorrection();
        /// <summary>
        /// データ収集完了イベント
        /// </summary>
        event EventHandler EndAcqData;

    }
    
}
