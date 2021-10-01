

namespace DioLib
{
    using System;
    using System.IO.MemoryMappedFiles;
    using System.Threading;

    /// <summary>
    /// DIO用共有メモリ
    /// </summary>
    public class DioMappedMemoryAccessor : IDioMappedMemoryAccessor, IDisposable
    {
        //共有メモリ
        protected MemoryMappedFile mmf { get; set; }

        //共有メモリへのアクセサ
        protected MemoryMappedViewAccessor accessor { get; set; }
        
        //排他処理用mutex
        protected Mutex mutex { get; set; }

        #region プロパティ

        //ビット数
        public int BitsNum { get; protected set; }

        //共有メモリのマップ名
        public string MappedName { get; protected set; }
        
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="bitsnum">ビット数</param>
        /// <param name="mappedname">マップ名</param>
        internal DioMappedMemoryAccessor(int bitsnum, string mappedname)
        {
            CreateMethod(bitsnum, mappedname);
        }

        public DioMappedMemoryAccessor()
        {
            
        }
        /// <summary>
        /// メソッドから生成
        /// </summary>
        /// <param name="bitsnum"></param>
        /// <param name="mappedname"></param>
        public void Create(int bitsnum, string mappedname)
        {
            CreateMethod(bitsnum, mappedname);
        }
        /// <summary>
        /// 生成メソッド
        /// </summary>
        /// <param name="bitsnum"></param>
        /// <param name="mappedname"></param>
        private void CreateMethod(int bitsnum, string mappedname)
        {
            //パラメーターチェック
            if (bitsnum <= 0)
            {
                //パラメーターエラー
                throw new ArgumentOutOfRangeException("bitsnum", bitsnum, DioLib.Properties.Resources.ERROR_DIOLIB);
            }

            if (string.IsNullOrWhiteSpace(mappedname))
            {
                throw new ArgumentException(DioLib.Properties.Resources.ERROR_DIOLIB, "mappedname");
            }


            MappedName = mappedname;

            BitsNum = bitsnum;

            //MemoryMappedFile 作成
            mmf = MemoryMappedFile.CreateOrOpen(MappedName, sizeof(bool) * BitsNum);

            //Accessor作成
            accessor = mmf.CreateViewAccessor();

            //Mutex作成
            mutex = new Mutex(false, MappedName + DioLib.Properties.Resources.MUTEX_NAME_SUFFIX);
        }

        ~DioMappedMemoryAccessor()
        {
            Dispose(false);
        }
        
        // マネージド解放処理
        protected void ManagedFree()
        {
            if (null != mutex)
            {
                mutex.Close();
                mutex = null;
            }

            if (null != accessor)
            {
                accessor.Dispose();
                accessor = null;
            }

            if (null != mmf)
            {
                mmf.Dispose();
                mmf = null;
            }
        }

        #region Bit操作
        /*
         * mutex.WaitOne()でタイムアウトを設定する？
         */ 

        /// <summary>
        /// 指定IndexのBit情報を取得
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool GetBit(uint index)
        {
            System.Diagnostics.Debug.Assert(null != mutex);
            System.Diagnostics.Debug.Assert(null != accessor);

            //Indexチェック
            if (index >= BitsNum)
            {
                throw new ArgumentOutOfRangeException("index", index, DioLib.Properties.Resources.ERROR_DIOLIB);
            }

            //タイムアウトはどうするか
            mutex.WaitOne();
            try
            {
                return accessor.ReadBoolean(index);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// 全Bit情報を取得
        /// </summary>
        /// <param name="bits"></param>
        public void GetAllBits(bool[] bits)
        {
            System.Diagnostics.Debug.Assert(null != mutex);
            System.Diagnostics.Debug.Assert(null != accessor);

            if (null == bits)
            {
                throw new ArgumentNullException("bits");
            }

            //長さチェック
            if (bits.Length < BitsNum)
            {
                throw new ArgumentOutOfRangeException("bits.Length", bits.Length, DioLib.Properties.Resources.ERROR_DIOLIB);
            }

            //タイムアウトはどうするか
            mutex.WaitOne();
            try
            {
                accessor.ReadArray<bool>(0, bits, 0, BitsNum);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// 指定IndexのBit情報を設定
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public void SetBit(bool value, uint index)
        {
            System.Diagnostics.Debug.Assert(null != mutex);
            System.Diagnostics.Debug.Assert(null != accessor);

            //Indexチェック
            if (index >= BitsNum)
            {
                throw new ArgumentOutOfRangeException("index", index, DioLib.Properties.Resources.ERROR_DIOLIB);
            }

            mutex.WaitOne();
            try
            {
                accessor.Write(index, value);
            }
            finally
            {
                mutex.ReleaseMutex();
            }

        }

        /// <summary>
        /// 全Bit情報を設定
        /// </summary>
        /// <param name="bits"></param>
        public void SetAllBits(bool[] bits)
        {
            System.Diagnostics.Debug.Assert(null != mutex);
            System.Diagnostics.Debug.Assert(null != accessor);

            if (null == bits)
            {
                throw new ArgumentNullException("bits");
            }

            //長さチェック
            if (bits.Length < BitsNum)
            {
                throw new ArgumentOutOfRangeException("bits.Length", bits.Length, DioLib.Properties.Resources.ERROR_DIOLIB);
            }

            mutex.WaitOne();
            try
            {
                accessor.WriteArray<bool>(0, bits, 0, BitsNum);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
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
                if (disposing)  //Managed解放
                {
                    ManagedFree();

                    GC.Collect();
                }

                //Unmanaged解放
            }
            this.disposed = true;
        }

        #endregion
    }
    /// <summary>
    /// DIO用共有メモリのインターフェイス
    /// </summary>
    public interface IDioMappedMemoryAccessor
    {
        void Create(int bitsnum, string mappedname);
        /// <summary>
        /// ビット数
        /// </summary>
        int BitsNum { get; }
        /// <summary>
        /// 共有メモリのマップ名
        /// </summary>
        string MappedName { get; }

        bool GetBit(uint index);
        void SetBit(bool value, uint index);
        void SetAllBits(bool[] bits);
        void GetAllBits(bool[] bits);
        /// <summary>
        /// 破棄
        /// </summary>
        void Dispose();
    }
}