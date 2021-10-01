using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iDetector
{
    public delegate void SdkCallbackEventHandler(int nEventID, int nEventLevel,
        string strMsg, int nParam1, int nParam2, int nPtrParamLen, IntPtr pParam);
    public struct AttrResult
    {
        public int nVal;
        public float fVal;
        public String strVal;
    }
    public class Detector : IDisposable
    {
        private int mCurCmdId;
        private int mDetectorID = 0;
        private int mLastError;
        private bool mIsInitilized = false;
        private AutoResetEvent mWaitAckEvent;
        private List<SdkCallbackEventHandler> mHandlerList;
        private SdkInterface.SdkCallbackHandler sdkHandler;
        public Detector()
        {
            mWaitAckEvent = new AutoResetEvent(false);
            mHandlerList = new List<SdkCallbackEventHandler>();
            sdkHandler = new SdkInterface.SdkCallbackHandler(OnSdkCallback);
        }
        public void Dispose()
        {
            mWaitAckEvent.Close();
            SdkInterface.Destroy(mDetectorID);
            mHandlerList.Clear();
        }
        public int Create(string work_dir, SdkCallbackEventHandler callback)
        {
            int nResult = 0;
            try
            {
                nResult = SdkInterface.Create(work_dir, sdkHandler, ref mDetectorID);
                mIsInitilized = true;
                mHandlerList.Add(callback);
            }
            catch
            {
                nResult = SdkInterface.Err_DllCreateObjFailed;
            }
            return nResult;
        }
        public int Abort()
        {
            return SdkInterface.Abort(mDetectorID);
        }
        public int SetAttr(int nId, int nValue)
        {
            if (!mIsInitilized)
                return SdkInterface.Err_NotInitialized;

            IRayVariant var = new IRayVariant();
            var.vt = IRAY_VAR_TYPE.IVT_INT;
            var.val.nVal = nValue;
            return SdkInterface.SetAttr(mDetectorID, nId, ref var);
        }
        public int SetAttr(int nId, float fValue)
        {
            if (!mIsInitilized)
                return SdkInterface.Err_NotInitialized;
            IRayVariant var = new IRayVariant();
            var.vt = IRAY_VAR_TYPE.IVT_FLT;
            var.val.fVal = fValue;
            return SdkInterface.SetAttr(mDetectorID, nId, ref var);
        }
        public int SetAttr(int nId, String str)
        {
            if (!mIsInitilized)
                return SdkInterface.Err_NotInitialized;

            IRayVariant var = new IRayVariant();
            var.vt = IRAY_VAR_TYPE.IVT_STR;
            var.val.strVal = str;
            return SdkInterface.SetAttr(mDetectorID, nId, ref var);
        }
        public int GetAttr(int nId, ref AttrResult result)
        {
            if (!mIsInitilized)
                return SdkInterface.Err_NotInitialized;

            IRayVariant var = new IRayVariant();
            int nRet = SdkInterface.GetAttr(mDetectorID, nId, ref var);
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
            if (!mIsInitilized)
                return result;

            IRayVariant var = new IRayVariant();
            int nRet = SdkInterface.GetAttr(mDetectorID, nId, ref var);
            if (nRet == SdkInterface.Err_OK)
            {
                if (var.vt == IRAY_VAR_TYPE.IVT_INT)
                {
                    result = var.val.nVal;
                }
                
            }
            return result;
        }
        public String GetErrorInfo(int nErrorCode)
        {
            ErrorInfo info = new ErrorInfo();
            SdkInterface.GetErrInfo(nErrorCode, ref info);
            return info.strDescription;
        }
        public int Invoke(int cmdId)
        {
            if (!mIsInitilized)
                return SdkInterface.Err_NotInitialized;

            mWaitAckEvent.Reset();
            mCurCmdId = cmdId;
            return SdkInterface.Invoke(mDetectorID, cmdId, null, 0);
        }
        public int Invoke(int cmdId, int nValue)
        {
            if (!mIsInitilized)
                return SdkInterface.Err_NotInitialized;

            mWaitAckEvent.Reset();
            mCurCmdId = cmdId;
            IRayCmdParam[] param = new IRayCmdParam[1];
            param[0].pt = IRAY_PARAM_TYPE.IPT_VARIANT;
            param[0].var.vt = IRAY_VAR_TYPE.IVT_INT;
            param[0].var.val.nVal = nValue;
            return SdkInterface.Invoke(mDetectorID, cmdId, param, 1);
        }
        public int Invoke(int cmdId, int nPar1, int nPar2)
        {
            if (!mIsInitilized)
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

            return SdkInterface.Invoke(mDetectorID, cmdId, param, param.Length);
        }
        public int Invoke(int cmdId, float fValue)
        {
            if (!mIsInitilized)
                return SdkInterface.Err_NotInitialized;

            mWaitAckEvent.Reset();
            mCurCmdId = cmdId;
            IRayCmdParam[] param = new IRayCmdParam[1];
            param[0].pt = IRAY_PARAM_TYPE.IPT_VARIANT;
            param[0].var.vt = IRAY_VAR_TYPE.IVT_FLT;
            param[0].var.val.fVal = fValue;
            return SdkInterface.Invoke(mDetectorID, cmdId, param, 1);
        }
        public int Invoke(int cmdId, String strValue)
        {
            if (!mIsInitilized)
                return SdkInterface.Err_NotInitialized;

            mWaitAckEvent.Reset();
            mCurCmdId = cmdId;
            IRayCmdParam[] param = new IRayCmdParam[1];
            param[0].pt = IRAY_PARAM_TYPE.IPT_VARIANT;
            param[0].var.vt = IRAY_VAR_TYPE.IVT_STR;
            param[0].var.val.strVal = strValue;
            return SdkInterface.Invoke(mDetectorID, cmdId, param, 1);
        }
        public int Invoke(int cmdId, int nPara1, int nPara2, string strValue1, string strValue2)
        {
            if (!mIsInitilized)
                return SdkInterface.Err_NotInitialized;

            mWaitAckEvent.Reset();
            mCurCmdId = cmdId;
            IRayCmdParam[] param = new IRayCmdParam[4];
            param[0].pt = IRAY_PARAM_TYPE.IPT_VARIANT;
            param[0].var = new IRayVariant();
            param[0].var.vt = IRAY_VAR_TYPE.IVT_INT;
            param[0].var.val.nVal = nPara1;
            param[1].pt = IRAY_PARAM_TYPE.IPT_VARIANT;
            param[1].var = new IRayVariant();
            param[1].var.vt = IRAY_VAR_TYPE.IVT_INT;
            param[1].var.val.nVal = nPara2;
            param[2].pt = IRAY_PARAM_TYPE.IPT_VARIANT;
            param[2].var = new IRayVariant();
            param[2].var.vt = IRAY_VAR_TYPE.IVT_STR;
            param[2].var.val.strVal = strValue1;
            param[3].pt = IRAY_PARAM_TYPE.IPT_VARIANT;
            param[3].var = new IRayVariant();
            param[3].var.vt = IRAY_VAR_TYPE.IVT_STR;
            param[3].var.val.strVal = strValue2;
            return SdkInterface.Invoke(mDetectorID, cmdId, param, param.Length);
        }
        public int WaitEvent(int timeout)
        {
            if (mWaitAckEvent.WaitOne(timeout))
                return mLastError;
            else
                return SdkInterface.Err_TaskTimeOut;
        }
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
        public void OnSdkCallback(int nDetectorID, int nEventID, int nEventLevel,
            IntPtr pszMsg, int nParam1, int nParam2, int nPtrParamLen, IntPtr pParam)
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

        static public string GetWorkDirPath()
        {
            //string file_path = "..\\common\\workdir_path.txt";
            //if (!File.Exists(file_path))
            //{
            //    file_path = "workdir_path.txt";
            //    if (!File.Exists(file_path))
            //        //return null;
            //        throw new FileNotFoundException("workdir_path.txt not existed!");
            //}

            string workdir = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "work_dir", "NDT0505J"));

            //StreamReader fs = new StreamReader(file_path);
            //string workdir = fs.ReadLine();
            //fs.Close();
            return workdir;
        }
    }
}
