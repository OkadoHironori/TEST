using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Threading;

using InterfaceCorpDllWrap;

namespace DioLib
{
    // 抽象クラス
    /// <summary>
    /// DIOボード制御クラス
    /// </summary>
    public class DioController : IDioController, IDisposable
    {
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="boardno"></param>
        /// <param name="bitsnum"></param>
        /// <returns></returns>
        public virtual string Create(byte boardno, byte bitsnum)
        {
            return null;
        }
        /// <summary>
        /// 制御番号
        /// </summary>
        public byte No { get; protected set; }

        /// <summary>
        /// 制御ビット数
        /// </summary>
        public byte BitsNum { get; protected set; }

        private bool _isOpen;

        public virtual void Open() { _isOpen = true; }
        
        public virtual void Close() { _isOpen = false; }

        public virtual bool Ready { get { return _isOpen; } }

        #region 抽象メソッド
        public virtual void SetDo(uint index, bool value)
        {
            //範囲エラー
            Debug.Assert(InRange(index));
        }
        public virtual void SetDo(bool[] bits)
        {
            //配列長チェック
            Debug.Assert(bits.Length >= BitsNum);
        }
        public virtual bool GetDi(uint index)
        {
            //範囲エラー
            Debug.Assert(InRange(index));

            return false;
        }
        public virtual void GetDi(bool[] bits)
        {
            //配列長チェック
            Debug.Assert(bits.Length >= BitsNum);
        }

        //INDEX範囲チェック
        protected bool InRange(uint index)
        {
            return (index < BitsNum);
        }
        #endregion

        #region IDisposable
        
        // Track whether Dispose has been called.
        protected bool disposed = false;

        /// <summary>
        /// 資源の解放
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 資源の解放
        /// </summary>
        /// <param name="disposing">マネージリソースとアンマネージリソースの両方を解放する場合はtrue。アンマネージリソースだけを解放する場合はfalse。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //Managed解放
                    FreeManagedResource();

                    GC.Collect();
                }

                //Unmanaged解放
                FreeUnmanagedResource();
            }
            this.disposed = true;
        }

        //実際の解放処理
        protected virtual void FreeUnmanagedResource() { }
        protected virtual void FreeManagedResource() { }

        #endregion

    }

    /// <summary>
    /// インターフェイス
    /// </summary>
    public interface IDioController
    {
        /// <summary>
        /// ボード名生成
        /// </summary>
        /// <param name="boardno"></param>
        /// <param name="bitsnum"></param>
        /// <returns></returns>
        string Create(byte boardno, byte bitsnum);
        /// <summary>
        /// 制御番号
        /// </summary>
        byte No { get; }

        /// <summary>
        /// 制御ビット数
        /// </summary>
        byte BitsNum { get; }
        /// <summary>
        /// オープン
        /// </summary>
        void Open();
        /// <summary>
        /// クローズ
        /// </summary>
        void Close();
        /// <summary>
        /// 準備
        /// </summary>
        bool Ready { get; }
        /// <summary>
        /// Doセット
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        void SetDo(uint index, bool value);
        /// <summary>
        /// Doセット
        /// </summary>
        /// <param name="bits"></param>
        void SetDo(bool[] bits);
        /// <summary>
        /// Di取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        bool GetDi(uint index);
        /// <summary>
        /// Di取得
        /// </summary>
        /// <param name="bits"></param>
        void GetDi(bool[] bits);
        /// <summary>
        /// 破棄
        /// </summary>
        void Dispose();
    }

}
