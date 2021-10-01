using PLCController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MechaMaintCnt.AuxSel;
using Itc.Common.Event;
using MechaMaintCnt.FCD;
using MechaMaintCnt.FDD;
using MechaMaintCnt.TblY;

namespace MechaMaintCnt
{
    using MessageEventHandler = Action<object, MessageEventArgs>;

    public class MechaMaint: IMechaMaint, IDisposable
    {
        /// <summary>
        /// メカ停止
        /// </summary>
        public event EventHandler MechaStop;
        /// <summary>
        /// 動作禁止
        /// </summary>
        public bool CanMove { get; private set; }
        /// <summary>
        /// メカ動作状態？
        /// </summary>
        public bool IsBesy { get; private set; }
        /// <summary>
        /// Formの状態変更
        /// </summary>
        public event EventHandler StsChanged;
        /// <summary>
        /// ロック状態変更
        /// </summary>
        public event EventHandler LockStsChanged;
        /// <summary>
        /// PCからPLCに通知するコマンド
        /// </summary>
        public event MessageEventHandler PCtoPLCCmd;
        /// <summary>
        /// オプション設定
        /// </summary>
        public IAuxSel _AuxSel { get; }
        /// <summary>
        /// FCD調整
        /// </summary>
        public IFCD _FCD { get; }
        /// <summary>
        /// FDD調整
        /// </summary>
        public IFDD _FDD { get; }
        /// <summary>
        /// FDD調整
        /// </summary>
        public ITblY _TblY { get; }
        /// <summary>
        /// 変更通知用
        /// </summary>
        public MaintProvider Provider { get; }
        /// <summary>
        /// PLCモニタ
        /// </summary>
        private readonly IPLCMonitor _PLCMonitor;
        /// <summary>
        /// メカメンテナンス
        /// </summary>
        /// <param name="monitor"></param>
        public MechaMaint(IPLCMonitor monitor, IAuxSel auxSel, IFCD fcd, IFDD fdd, ITblY tbly)
        {
            _AuxSel = auxSel;
            _AuxSel.AUXChanged += (s, e) =>
            {
                var sel = s as AuxSel.AuxSel;

                if(sel.AuxProvider.DoorInterlock)
                {
                    if (sel.AuxProvider.DoorEMLock)
                    {
                        CanMove = true;
                    }
                    else
                    {
                        CanMove = false;
                    }
                }
                else
                {
                    CanMove = true;//常に動かせる
                }


                LockStsChanged?.Invoke(this, e);
                

            };

            _TblY = tbly;
            _TblY.CmdChanged += (s, e) =>
            {
                var tblsts = s as TblY.TblY;
                if(tblsts.IsManualMode)
                {
                    return;//マニュアルモードは何もしない
                }
                else if(tblsts.IsOriginMode)//原点復帰
                {
                    IsBesy = tblsts.IsOriginMode ;
                    StsChanged?.Invoke(this, e);
                }
                else if(tblsts.IsIndexMode)//Index指示 besyフラグに従う
                {
                    IsBesy = tblsts.IsIndexMode;
                    StsChanged?.Invoke(this, e);
                }
                else
                {
                    IsBesy = false;
                    StsChanged?.Invoke(this, e);
                }
            };

            _FCD = fcd;
            _FCD.CmdChanged += (s, e) =>
            {
                var fcdsts = s as FCD.FCD;
                if (fcdsts.IsManualMode)
                {
                    return;//マニュアルモードは何もしない
                }
                else if (fcdsts.IsOriginMode)//原点復帰
                {
                    IsBesy = fcdsts.IsOriginMode;
                    StsChanged?.Invoke(this, e);
                }
                else if(fcdsts.IsIndexMode)//Index指示 besyフラグに従う
                {
                    IsBesy = fcdsts.IsIndexMode;
                    StsChanged?.Invoke(this, e);
                }
                else
                {
                    IsBesy = false;
                    StsChanged?.Invoke(this, e);
                }
            };


            _FDD = fdd;
            _FDD.CmdChanged += (s, e) =>
             {
                 var fddsts = s as FDD.FDD;
                 if (fddsts.IsManualMode)
                 {
                     return;//マニュアルモードは何もしない
                 }
                 else if (fddsts.IsOriginMode)//原点復帰
                 {
                     IsBesy = fddsts.IsOriginMode;
                     StsChanged?.Invoke(this, e);
                 }
                 else if(fddsts.IsIndexMode)//Index指示 besyフラグに従う
                 {
                     IsBesy = fddsts.IsIndexMode;
                     StsChanged?.Invoke(this, e);
                 }
                 else
                 {
                     IsBesy = false;
                     StsChanged?.Invoke(this, e);
                 }
             };


            _PLCMonitor = monitor;
            _PLCMonitor.RequestPLC += (s, e) =>
             {
                 PCtoPLCCmd?.Invoke(s, e);
             };


            Provider = new MaintProvider(monitor);
        }
        /// <summary>
        /// データ読取
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        public void SetSerialData(byte[] data, PLCDataType type) => _PLCMonitor.SetSerialData(data, type);
        /// <summary>
        /// データ取得 WordDataのみ
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        public void SetSerialData(string data, PLCDataType type) => _PLCMonitor.SetSerialData(data, type);
        /// <summary>
        /// データ送信要求コマンド取得
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetRequestCmd(PLCDataType type) => _PLCMonitor.GetRequestCmd(type);
        /// <summary>
        /// 停止ボタン
        /// </summary>
        public void RequestStopAll()
        {
            _FCD.Stop(null);
            _TblY.Stop(null);
            _FDD.Stop(null);

            MechaStop?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {

        }
    }

    public interface IMechaMaint
    {

        /// <summary>
        /// ロック状態変更
        /// </summary>
        event EventHandler LockStsChanged;
        /// <summary>
        /// メカ停止
        /// </summary>
        event EventHandler MechaStop;
        /// <summary>
        /// 状態変更
        /// </summary>
        event EventHandler StsChanged;
        /// <summary>
        /// PCからPLCに通知するコマンド
        /// </summary>
        event MessageEventHandler PCtoPLCCmd;
        /// <summary>
        /// 
        /// </summary>
        ITblY _TblY { get; }
        /// <summary>
        /// 
        /// </summary>
        IFDD _FDD { get; }
        /// <summary>
        /// 
        /// </summary>
        IFCD _FCD { get; }
        /// <summary>
        /// 
        /// </summary>
        IAuxSel _AuxSel { get; }
        /// <summary>
        /// PLCのデータをセット(byte配列)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        void SetSerialData(byte[] data, PLCDataType type);
        /// <summary>
        /// PLCのデータをセット(string配列)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        void SetSerialData(string data, PLCDataType type);
        /// <summary>
        /// コマンドを返します。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetRequestCmd(PLCDataType type);
        /// <summary>
        /// 停止ボタン
        /// </summary>
        void RequestStopAll();

    }
}
