using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Threading;

using InterfaceCorpDllWrap;


//最終的にnamespace Xs.Dio に変更。。。？
namespace DioLib
{
    //DIOボード種類：他のボードに適用できるかは不明
    public enum DioBoardType { GPC2000 = 0 };

    //DIOクラス
    public class Dio : IDio, IDisposable
    {
        public DioStatus Status { get; protected set; }
        internal IDioController DioCtrl { get; private set; }
        //DI,DO共有メモリ
        protected IDioMappedMemoryAccessor DiAccessor { get; set; }
        protected IDioMappedMemoryAccessor DoAccessor { get; set; }

        ////コンストラクタ //外部からは隠蔽。Factoryクラスから生成する。
        internal Dio(DioController ctrl, DioMappedMemoryAccessor dimem, DioMappedMemoryAccessor domem)
        {

            #region Null Check
            #endregion

            DioCtrl = ctrl ?? throw new ArgumentNullException("ctrl");
            DiAccessor = dimem ?? throw new ArgumentNullException("dimem");
            DoAccessor = domem ?? throw new ArgumentNullException("domem");
        }
        /// <summary>
        /// コンストラクタ DIパターン
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="dimem"></param>
        /// <param name="domem"></param>
        public Dio(IDioController ctrl, IDioMappedMemoryAccessor dimem, IDioMappedMemoryAccessor domem)
        {
            DioCtrl = ctrl;
            DiAccessor = dimem;
            DoAccessor = domem;
        }
        /// <summary>
        /// Dioクラス生成
        /// </summary>
        public void Create(DioBoardType type, int boad_no, int bit_num)
        {
            var mappedname = DioCtrl.Create(Convert.ToByte(boad_no), Convert.ToByte(bit_num));
            DiAccessor.Create(bit_num, mappedname + DioLib.Properties.Resources.DI_NAME_SUFFIX);
            DoAccessor.Create(bit_num, mappedname + DioLib.Properties.Resources.DO_NAME_SUFFIX);
        }

        ////監視用も一緒にする？
        //DioWatcher Watcher { get; set; }
        ////未使用
        //public event EventHandler<DiChangedEventArgs> DiChanged
        //{
        //    add { Watcher.DiChanged += value; }
        //    remove { Watcher.DiChanged -= value; }
        //}

        public void Open()
        {
            Debug.Assert(null != DioCtrl);

            DioCtrl.Open();

            //Status.State = DioState.Ready;
            if(null != Status)
                Status.State = DioCtrl.Ready ? DioState.Ready : DioState.Error;
        }

        public void Close()
        {
            //Debug.Assert(null != DioCtrl);

            if (null != Status)
                Status.State = DioState.None;

            if (null == DioCtrl) return;

            DioCtrl.Close();
        }

        #region ビット数
        //DIビット数
        public int DiBits
        {
            get
            {
                return (null == DiAccessor) ? 0 : DiAccessor.BitsNum;
            }
            set
            {
                DiBits = value;
            }
        }
        //DOビット数
        public virtual int DoBits
        {
            get
            {
                return (null == DoAccessor) ? 0 : DoAccessor.BitsNum;
            }
            set
            {
                DoBits = value;
            }
        }
        #endregion

        //Virtualの場合、メモリに二度アクセスすることになるが、
        //基本的にDEBUG時にしか使わないので、効率が落ちてもよしとする。
        //ビット操作に排他処理を追加
        #region DIビット操作
        public void SetDi(bool value, uint index)
        {
            lock (DioCtrl)
            {
                //※DIへのセットはVirtualのみ可能
                if (DioCtrl is VirtualDioController)
                {
                    Debug.Assert(null != DiAccessor);

                    DiAccessor.SetBit(value, index);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
        public bool GetDi(uint index)
        {
            lock (DioCtrl)
            {
                Debug.Assert(null != DioCtrl);
                Debug.Assert(null != DiAccessor);

                bool ret = DioCtrl.GetDi(index);
                DiAccessor.SetBit(ret, index);
                return ret;
            }
        }
        public void GetDiAll(bool[] buf)
        {
            lock (DioCtrl)
            {

                Debug.Assert(null != DioCtrl);
                Debug.Assert(null != DiAccessor);
                Debug.Assert(null != buf);

                DioCtrl.GetDi(buf);

                DiAccessor.SetAllBits(buf);
            }
        }
        #endregion

        #region DOビット操作
        public bool GetDo(uint index)
        {
            lock (DioCtrl)
            {
                Debug.Assert(null != DoAccessor);
                return DoAccessor.GetBit(index);
            }
        }
        public void GetDo(bool[] buf)
        {
            lock (DioCtrl)
            {
                Debug.Assert(null != DoAccessor);
                Debug.Assert(null != buf);

                DoAccessor.GetAllBits(buf);
            }
        }

        public void SetDo(bool value, uint index)
        {
            lock (DioCtrl)
            {
                Debug.Assert(null != DioCtrl);
                Debug.Assert(null != DoAccessor);
                DioCtrl.SetDo(index, value);
                DoAccessor.SetBit(value, index);
            }
        }
        public void SetDo(bool[] buf)
        {
            lock (DioCtrl)
            {
                Debug.Assert(null != DioCtrl);
                Debug.Assert(null != DoAccessor);
                Debug.Assert(null != buf);

                DioCtrl.SetDo(buf);

                DoAccessor.SetAllBits(buf);
            }
        }
        
        //指定したビットをまとめてセットする
        public void SetDo(Dictionary<uint, bool> index)
        {
            bool[] values = new bool[DoBits];

            //現在のビット情報を取得
            GetDo(values);

            //指定した箇所だけ変更
            foreach (var idx in index)
            {
                values[idx.Key] = idx.Value;
            }

            SetDo(values);
        }

        //全DiBitをOff
        public void ClearDo()
        {
            if (null == DioCtrl) return;

            if (!DioCtrl.Ready) return;

            Debug.Assert(DoBits > 0);

            bool[] bits = new bool[DoBits];
            Array.Clear(bits, 0, bits.Length);

            SetDo(bits);

            System.Diagnostics.Debug.WriteLine("ClearDo.Done");
        }
        #endregion


        #region IDisposable
        /// <summary>
        /// リソースが解放されているかどうかを示します。
        /// </summary>
        bool _disposed;

        /// <summary>
        /// Dioで使用されるすべてのリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        /// <summary>
        /// Dioで使用されるすべてのアンマネージリソースを解放し、オプションでマネージリソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージリソースとアンマネージリソースの両方を解放する場合はtrue。アンマネージリソースだけを解放する場合はfalse。</param>
        protected void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            if (disposing)
            {
                //ここにマネージリソースの解放処理を記述する
            }

            //ここにアンマネージリソースの解放処理を記述する
            if(null != DioCtrl)
            {
                DioCtrl.Dispose();
                DioCtrl = null;
            }

            if(null != DiAccessor)
            {
                DiAccessor.Dispose();
                DiAccessor = null;
            }
            if (null != DoAccessor)
            {
                DoAccessor.Dispose();
                DoAccessor = null;
            }
        }

        ~Dio()
        {
            Dispose(false);
        }

        #endregion
      
    }

    //仮、DIOのステータス管理
    public enum DioState { None, Ready, Run, Error, Emurated };
    
    public class DioStatus 
    {
        //INotifyPropertyChanged追加・・？
        public DioState State { get; internal set; }
        public string LastError { get; internal set; }
    }

    /// <summary>
    /// Dioインターフェイス
    /// </summary>
    public interface IDio
    {
        /// <summary>
        /// DIO閉
        /// </summary>
        void Close();
        /// <summary>
        /// DIO開
        /// </summary>
        void Create(DioBoardType type, int boad_no, int bit_num);
        /// <summary>
        /// DIOボードオープン
        /// </summary>
        void Open();
        /// <summary>
        /// DIを取得
        /// </summary>
        /// <param name="buf"></param>
        void GetDiAll(bool[] buf);
        /// <summary>
        /// DOをセットする
        /// </summary>
        /// <param name="buf"></param>
        void SetDo(bool value, uint index);
        /// <summary>
        /// DOをセットする
        /// </summary>
        /// <param name="buf"></param>
        void GetDo(bool[] buf);
        /// <summary>
        /// Doのビット数
        /// </summary>
        int DoBits { get; set; }

    }
}


