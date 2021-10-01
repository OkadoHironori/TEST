using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Threading;

using InterfaceCorpDllWrap;

namespace DioLib
{
    //シンプルなビット操作のみ
    /// <summary>
    /// GPC-2000 - Interface製 DIOボード制御用クラス
    /// </summary>
    public class GpcDioController : DioController
    {
        private static readonly IntPtr InvalidHandleValue = new IntPtr(-1);
        //private static readonly IntPtr InvalidHandleValue = IntPtr.Zero;    //どっち？

        //DIOデバイスハンドル
        protected IntPtr hDio;
        public IntPtr DeviceHandle
        {
            get { return hDio; }
        }

        public override bool Ready
        {
            get
            {
                return (InvalidHandleValue != hDio);
            }
        }

        //コンストラクタ
        internal GpcDioController(byte boardno, byte bitsnum)
        {
            hDio = InvalidHandleValue;

            this.No = boardno;
            this.BitsNum = bitsnum;
        }

        //デストラクタ
        ~GpcDioController()
        {
            Dispose(false);
        }
        /// <summary>
        /// コンストラクタ DI用
        /// </summary>
        public GpcDioController()
        {
            hDio = InvalidHandleValue;
        }
        /// <summary>
        /// ボード名の生成
        /// </summary>
        /// <param name="boardno"></param>
        /// <param name="bitsnum"></param>
        public override string Create(byte boardno, byte bitsnum)
        {
            this.No = boardno;
            this.BitsNum = bitsnum;
            return CreateName(Convert.ToByte(boardno));
        }

        /// <summary>
        /// デバイスオープン
        /// </summary>
        public override void Open()
        {
            System.Diagnostics.Debug.WriteLine("GpcDio Open");
            //base.Open();

            //デバイス名取得
            string gpcname = CreateName(No);

            //デバイスオープン
            hDio = IFCDIO_ANY.DioOpen(gpcname, IFCDIO_ANY.FBIDIO_FLAG_SHARE);
            if (hDio == InvalidHandleValue)
            {
                //初期化失敗
                throw new Exception(DioLib.Properties.Resources.ERROR_NOT_OPEN);
            }
        }

        /// <summary>
        /// デバイスクローズ
        /// </summary>
        public override void Close()
        {
            //base.Close();

            //デバイスクローズ処理
            if (hDio != InvalidHandleValue)
            {
                //エラーチェック？
                uint ret = IFCDIO_ANY.DioClose(hDio);

                if (ret != IFCDIO_ANY.FBIDIO_ERROR_SUCCESS)
                {
                    //Close失敗時処理：どういう時に失敗するのか・・？
                    var err = IFCDIO_ANY.GetErrorMessage(ret);
                    throw new Exception(err);
                }

                hDio = InvalidHandleValue;
            }

            System.Diagnostics.Debug.WriteLine("GpcDio Close");
        }

        #region ビット操作
        
        //DOビット設定 index指定
        public override void SetDo(uint index, bool value)
        {
            //Indexチェック
            base.SetDo(index, value);

            //Convert bool -> int
            int[] buf = new int[1];
            buf[0] = Convert.ToInt32(value);

            //Set
            uint ret = IFCDIO_ANY.DioOutputPoint(hDio, buf, index + 1, 1);
            if (ret != IFCDIO_ANY.FBIDIO_ERROR_SUCCESS)
            {
                //読み取りエラー
                var err = IFCDIO_ANY.GetErrorMessage(ret);
                throw new Exception(err);
            }
        }

        //DOビット設定 全ビット
        public override void SetDo(bool[] bits)
        {
            base.SetDo(bits);

            //Oonvert bool[] -> int[] @Linq
            int[] buf = bits.Select(b => Convert.ToInt32(b)).ToArray();

            uint ret = IFCDIO_ANY.DioOutputPoint(hDio, buf, 1, BitsNum);
            if (ret != IFCDIO_ANY.FBIDIO_ERROR_SUCCESS)
            {
                //読み取りエラー
                var err = IFCDIO_ANY.GetErrorMessage(ret);
                throw new Exception(err);
            }
        }

        //DIビット取得 index指定
        public override bool GetDi(uint index)
        {
            base.GetDi(index);

            int[] buf = new int[1];
            //Get
            uint ret = IFCDIO_ANY.DioInputPoint(hDio, buf, index + 1, 1);
            if (ret != IFCDIO_ANY.FBIDIO_ERROR_SUCCESS)
            {
                //読み取りエラー → 例外
                var err = IFCDIO_ANY.GetErrorMessage(ret);
                throw new Exception(err);
            }

            //Convert int -> bool
            return Convert.ToBoolean(buf[0]);
        }

        //DIビット取得 全ビット
        public override void GetDi(bool[] bits)
        {
            base.GetDi(bits);

            int[] buf = new int[bits.Length];

            uint ret = IFCDIO_ANY.DioInputPoint(hDio, buf, 1, BitsNum);
            if (ret != IFCDIO_ANY.FBIDIO_ERROR_SUCCESS)
            {
                //読み取りエラー → 例外
                var err = IFCDIO_ANY.GetErrorMessage(ret);

                throw new Exception(err);
            }

            //Convert int[] -> bool[] @LINQ
            bool[] tmp = buf.Select(i => Convert.ToBoolean(i)).ToArray();

            Buffer.BlockCopy(tmp, 0, bits, 0, sizeof(bool) * bits.Length);
        }
        #endregion

        //デバイス番号からデバイス名を生成
        public static string CreateName(byte boardno)
        {
            return DioLib.Properties.Resources.DIO_NAME_HEADER + (boardno + 1).ToString();
        }

        #region IDisposable
        protected override void FreeUnmanagedResource()
        {
            if (Ready)
            {
                Close();
            }
        }
        #endregion
    }

}
