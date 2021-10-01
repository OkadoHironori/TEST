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
    public delegate void SdkCallbackEventHandler(int nEventID, int nEventLevel,
    string strMsg, int nParam1, int nParam2, int nPtrParamLen, IntPtr pParam);
    public struct AttrResult
    {
        public int nVal;
        public float fVal;
        public String strVal;
    }
    /// <summary>
    /// iRay社製メインクラス
    /// </summary>
    public class IRayDetector : IIRayDetector, IDisposable
    {
        /// <summary>
        /// IRayクラススタート完了イベント
        /// </summary>
        public event EventHandler EndStartupIRayClass;
        /// <summary>
        /// ID
        /// </summary>
        public int DetectorID { get; private set; } = 0;
        /// <summary>
        /// 作業ディレクトリ
        /// </summary>
        public string WorkDir { get; private set; }
        /// <summary>
        /// 初期化
        /// </summary>
        public bool IsInitilized { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int mCurCmdId { get; private set; }

        //private readonly int _mCurCmdId=0;
        //private int mDetectorID = 0;
        private int mLastError;
        //private bool mIsInitilized = false;
        private AutoResetEvent mWaitAckEvent;
        private List<SdkCallbackEventHandler> mHandlerList;
        private SdkInterface.SdkCallbackHandler sdkHandler;
        /// <summary>
        /// iRay社設定I/F
        /// </summary>
        private readonly IIRayConfig _IRayConfig;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iRayConfig"></param>
        public IRayDetector(IIRayConfig irayconfig)
        {
            _IRayConfig = irayconfig;
            _IRayConfig.EndAddEvent += (s, e) => 
            {
                IRayConfig ircg = s as IRayConfig;
                WorkDir = ircg.NDT0505JPath;
            };
            _IRayConfig.RequestConf();

            mWaitAckEvent = new AutoResetEvent(false);
            mHandlerList = new List<SdkCallbackEventHandler>();
            sdkHandler = new SdkInterface.SdkCallbackHandler(OnSdkCallback);
        }


        /// <summary>
        /// コールバック
        /// </summary>
        /// <param name="nDetectorID"></param>
        /// <param name="nEventID"></param>
        /// <param name="nEventLevel"></param>
        /// <param name="pszMsg"></param>
        /// <param name="nParam1"></param>
        /// <param name="nParam2"></param>
        /// <param name="nPtrParamLen"></param>
        /// <param name="pParam"></param>
        public void OnSdkCallback(
            int nDetectorID,
            int nEventID,
            int nEventLevel,
            IntPtr pszMsg,
            int nParam1,
            int nParam2,
            int nPtrParamLen,
            IntPtr pParam)
        {
            if ((SdkInterface.Evt_TaskResult_Succeed == nEventID) || (SdkInterface.Evt_TaskResult_Failed == nEventID))
            {
                if (SdkInterface.Evt_TaskResult_Succeed == nEventID)
                    mLastError = SdkInterface.Err_OK;
                else
                    mLastError = nParam2;
                if (mCurCmdId == nParam1)
                {
                    mWaitAckEvent.Set();
                }
            }
            string strMsg = Marshal.PtrToStringAnsi(pszMsg);
            mHandlerList.ForEach(processor =>
            {
                processor(nEventID, nEventLevel, strMsg, nParam1, nParam2, nPtrParamLen, pParam);
            });
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            Debug.WriteLine($"{nameof(IRayDetector)} is Disposeing");
            mWaitAckEvent.Close();
            SdkInterface.Destroy(DetectorID);
            mHandlerList.Clear();
        }
        /// <summary>
        /// IRayクラススタート
        /// </summary>
        public void IRayClassStart(SdkCallbackEventHandler sdkCallback)
        {
            int nResult = 0;
            try
            {
                //作業ディレクトリ登録とdll確認（例外だし）
                int tmpid = 0;

                nResult = SdkInterface.Create(WorkDir, sdkHandler, ref tmpid);

                DetectorID = tmpid;

                IsInitilized = true;

                mHandlerList.Add(sdkCallback);
            }
            catch
            {
                nResult = SdkInterface.Err_DllCreateObjFailed;
                throw new Exception($"{WorkDir} isn't exist!{Environment.NewLine}iRaydlls isn't exist!");
            }

            EndStartupIRayClass?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// ディテクタ破棄
        /// </summary>
        /// <param name="id"></param>
        public void DestroyDetector(int id)
        {
            var ret = SdkInterface.Destroy(id);
            if(ret!=0)
            {
                throw new Exception($"{WorkDir} isn't exist!{Environment.NewLine}iRaydlls isn't exist!");
            }

        }
        /// <summary>
        /// 終了
        /// </summary>
        /// <returns></returns>
        public int Abort()
        {
            return SdkInterface.Abort(DetectorID);
        }
        /// <summary>
        /// 情報取得
        /// </summary>
        /// <param name="nId"></param>
        /// <param name="nValue"></param>
        /// <returns></returns>
        public int SetAttr(int nId, int nValue)
        {
            if (!IsInitilized)
                return SdkInterface.Err_NotInitialized;

            IRayVariant var = new IRayVariant();
            var.vt = IRAY_VAR_TYPE.IVT_INT;
            var.val.nVal = nValue;
            return SdkInterface.SetAttr(DetectorID, nId, ref var);
        }
        public int SetAttr(int nId, float fValue)
        {
            if (!IsInitilized)
                return SdkInterface.Err_NotInitialized;
            IRayVariant var = new IRayVariant();
            var.vt = IRAY_VAR_TYPE.IVT_FLT;
            var.val.fVal = fValue;
            return SdkInterface.SetAttr(DetectorID, nId, ref var);
        }
        public int SetAttr(int nId, String str)
        {
            if (!IsInitilized)
                return SdkInterface.Err_NotInitialized;

            IRayVariant var = new IRayVariant();
            var.vt = IRAY_VAR_TYPE.IVT_STR;
            var.val.strVal = str;
            return SdkInterface.SetAttr(DetectorID, nId, ref var);
        }
        public int GetAttr(int nId, ref AttrResult result)
        {
            if (!IsInitilized)
                return SdkInterface.Err_NotInitialized;

            IRayVariant var = new IRayVariant();
            int nRet = SdkInterface.GetAttr(DetectorID, nId, ref var);
            if (nRet == SdkInterface.Err_OK)
            {
                if (var.vt == IRAY_VAR_TYPE.IVT_INT)
                {
                    result.nVal = var.val.nVal;
                }
                else if (var.vt == IRAY_VAR_TYPE.IVT_FLT)
                {
                    result.fVal = var.val.fVal;
                }
                else if (var.vt == IRAY_VAR_TYPE.IVT_STR)
                {
                    result.strVal = var.val.strVal;
                }
            }
            return nRet;
        }
        public int GetAttrInt(int nId)
        {
            int result = 0;
            if (!IsInitilized)
                return result;

            IRayVariant var = new IRayVariant();
            int nRet = SdkInterface.GetAttr(DetectorID, nId, ref var);
            if (nRet == SdkInterface.Err_OK)
            {
                if (var.vt == IRAY_VAR_TYPE.IVT_INT)
                {
                    result = var.val.nVal;
                }

            }
            return result;
        }
        /// <summary>
        /// 呼出
        /// </summary>
        /// <param name="cmdId"></param>
        /// <returns></returns>
        public int Invoke(int cmdId)
        {
            if (!IsInitilized)
                return SdkInterface.Err_NotInitialized;

            mWaitAckEvent.Reset();
            mCurCmdId = cmdId;
            return SdkInterface.Invoke(DetectorID, cmdId, null, 0);
        }
        public int Invoke(int cmdId, int nValue)
        {
            if (!IsInitilized)
                return SdkInterface.Err_NotInitialized;

            mWaitAckEvent.Reset();
            mCurCmdId = cmdId;
            IRayCmdParam[] param = new IRayCmdParam[1];
            param[0].pt = IRAY_PARAM_TYPE.IPT_VARIANT;
            param[0].var.vt = IRAY_VAR_TYPE.IVT_INT;
            param[0].var.val.nVal = nValue;
            return SdkInterface.Invoke(DetectorID, cmdId, param, 1);
        }
        /// <summary>
        /// 呼び出し
        /// </summary>
        /// <param name="cmdId"></param>
        /// <param name="nPar1"></param>
        /// <param name="nPar2"></param>
        /// <returns></returns>
        public int Invoke(int cmdId, int nPar1, int nPar2)
        {
            if (!IsInitilized)
                return SdkInterface.Err_NotInitialized;

            mWaitAckEvent.Reset();
            mCurCmdId = cmdId;
            IRayCmdParam[] param = new IRayCmdParam[2];
            param[0].pt = IRAY_PARAM_TYPE.IPT_VARIANT;
            param[0].var.vt = IRAY_VAR_TYPE.IVT_INT;
            param[0].var.val.nVal = nPar1;

            param[1].pt = IRAY_PARAM_TYPE.IPT_VARIANT;
            param[1].var.vt = IRAY_VAR_TYPE.IVT_INT;
            param[1].var.val.nVal = nPar2;

            return SdkInterface.Invoke(DetectorID, cmdId, param, param.Length);
        }
        public int Invoke(int cmdId, float fValue)
        {
            if (!IsInitilized)
                return SdkInterface.Err_NotInitialized;

            mWaitAckEvent.Reset();
            mCurCmdId = cmdId;
            IRayCmdParam[] param = new IRayCmdParam[1];
            param[0].pt = IRAY_PARAM_TYPE.IPT_VARIANT;
            param[0].var.vt = IRAY_VAR_TYPE.IVT_FLT;
            param[0].var.val.fVal = fValue;
            return SdkInterface.Invoke(DetectorID, cmdId, param, 1);
        }
        public int Invoke(int cmdId, String strValue)
        {
            if (!IsInitilized)
                return SdkInterface.Err_NotInitialized;

            mWaitAckEvent.Reset();
            mCurCmdId = cmdId;
            IRayCmdParam[] param = new IRayCmdParam[1];
            param[0].pt = IRAY_PARAM_TYPE.IPT_VARIANT;
            param[0].var.vt = IRAY_VAR_TYPE.IVT_STR;
            param[0].var.val.strVal = strValue;
            return SdkInterface.Invoke(DetectorID, cmdId, param, 1);
        }
        /// <summary>
        /// 非同期読出
        /// </summary>
        /// <param name="cmdId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public int SyncInvoke(int cmdId, int timeout)
        {
            mLastError = SdkInterface.Err_TaskTimeOut;
            int result = Invoke(cmdId);
            if (SdkInterface.Err_TaskPending == result)
            {
                result = WaitEvent(timeout);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdId"></param>
        /// <param name="nValue"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public int SyncInvoke(int cmdId, int nValue, int timeout)
        {
            mLastError = SdkInterface.Err_TaskTimeOut;
            int result = Invoke(cmdId, nValue);
            if (SdkInterface.Err_TaskPending == result)
            {
                result = WaitEvent(timeout);
            }
            return result;
        }
        public int SyncInvoke(int cmdId, int nPar1, int nPar2, int timeout)
        {
            mLastError = SdkInterface.Err_TaskTimeOut;
            int result = Invoke(cmdId, nPar1, nPar2);
            if (SdkInterface.Err_TaskPending == result)
            {
                result = WaitEvent(timeout);
            }
            return result;
        }
        public int SyncInvoke(int cmdId, float fValue, int timeout)
        {
            mLastError = SdkInterface.Err_TaskTimeOut;
            int result = Invoke(cmdId, fValue);
            if (SdkInterface.Err_TaskPending == result)
            {
                result = WaitEvent(timeout);
            }
            return result;
        }
        public int SyncInvoke(int cmdId, string strValue, int timeout)
        {
            mLastError = SdkInterface.Err_TaskTimeOut;
            int result = Invoke(cmdId, strValue);
            if (SdkInterface.Err_TaskPending == result)
            {
                result = WaitEvent(timeout);
            }
            return result;
        }
        /// <summary>
        /// イベント待ち
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public int WaitEvent(int timeout)
        {
            if (mWaitAckEvent.WaitOne(timeout))
                return mLastError;
            else
                return SdkInterface.Err_TaskTimeOut;
        }
        /// <summary>
        /// エラー情報を出力する
        /// </summary>
        /// <param name="nErrorCode"></param>
        /// <returns></returns>
        public string GetErrorInfo(int nErrorCode)
        {
            ErrorInfo info = new ErrorInfo();
            SdkInterface.GetErrInfo(nErrorCode, ref info);
            return info.strDescription;
        }
    }
    /// <summary>
    /// iRay社製メインクラスI/F
    /// </summary>
    public interface IIRayDetector
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler EndStartupIRayClass;
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="sdkCallback"></param>
        void IRayClassStart(SdkCallbackEventHandler sdkCallback);
        /// <summary>
        /// Det呼出：接続確認で利用
        /// </summary>
        /// <param name="nErrorCode"></param>
        /// <returns></returns>
        int SyncInvoke(int cmdId, int timeout);
        /// <summary>
        /// Det呼出：モード選択
        /// </summary>
        /// <param name="cmdId"></param>
        /// <param name="strValue"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        int SyncInvoke(int cmdId, string strValue, int timeout);
        /// <summary>
        /// Det呼出:校正選択
        /// </summary>
        /// <param name="cmdId"></param>
        /// <param name="nValue"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        int SyncInvoke(int cmdId, int nValue, int timeout);
        /// <summary>
        /// オフセット校正後のデータ確認
        /// </summary>
        /// <param name="cmdId"></param>
        /// <param name="nValue"></param>
        /// <returns></returns>
        int Invoke(int cmdId);
        /// <summary>
        /// オフセット校正
        /// </summary>
        /// <param name="cmdId"></param>
        /// <param name="nValue"></param>
        /// <returns></returns>
        int Invoke(int cmdId, int nValue);
        /// <summary>
        /// エラー情報を出力する
        /// </summary>
        /// <param name="nErrorCode"></param>
        /// <returns></returns>
        string GetErrorInfo(int nErrorCode);
        /// <summary>
        /// 本体情報の取得
        /// </summary>
        /// <param name="nId"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        int GetAttr(int nId, ref AttrResult result);
        /// <summary>
        /// 校正情報の取得
        /// </summary>
        /// <param name="nId"></param>
        /// <returns></returns>
        int GetAttrInt(int nId);
        /// <summary>
        /// 校正初期化時に
        /// </summary>
        /// <param name="nId"></param>
        /// <param name="nValue"></param>
        /// <returns></returns>
        int SetAttr(int nId, int nValue);
        /// <summary>
        /// ディテクタ破棄
        /// </summary>
        /// <param name="id"></param>
        void DestroyDetector(int id);
        /// <summary>
        /// ディテクタ破棄
        /// </summary>
        /// <param name="id"></param>
        int Abort();
    }
}
