using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itc.Common.Event
{

    using NumUpdateEventHandler = Action<object, NumUpdateEventArgs>;
    using ChkChangeEventHandler = Action<object, ChkChangeEventArgs>;

    /// <summary>
    /// NumUpDownボタンのイベントハンドラ
    /// </summary>
    public class NumUpdateEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NumUpdateEventArgs(float value) : base()
        {
            NumValue = value;
        }
        /// <summary>
        /// Floatデータ
        /// </summary>
        public float NumValue { get; set; }
    }

    /// <summary>
    /// チェックボタンのイベントハンドラ
    /// </summary>
    public class ChkChangeEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ChkChangeEventArgs(bool chk) : base()
        {
            Chk = chk;
        }
        /// <summary>
        /// チェック
        /// </summary>
        public bool Chk { get; set; }
    }

    /// <summary>
    /// 選択リストのイベントハンドラ
    /// </summary>
    public class SelectChangeEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SelectChangeEventArgs(string selname) : base()
        {
            SelectName = selname;
        }
        /// <summary>
        /// 選択した名前
        /// </summary>
        public string SelectName { get; set; }
    }

    /// <summary>
    /// メッセージのイベントハンドラ
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MessageEventArgs(string message) : base()
        {
            Message = message;
        }
        /// <summary>
        /// メッセージ
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// カウント変更時のイベントハンドラ（uint）
    /// </summary>
    public class UIntCntChangeEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UIntCntChangeEventArgs(uint count) : base()
        {
            Count = count;
        }
        /// <summary>
        /// 選択した名前
        /// </summary>
        public uint Count { get; set; }
    }
    /// <summary>
    /// カウント変更時のイベントハンドラ(ushort)
    /// </summary>
    public class UShortCntChangeEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UShortCntChangeEventArgs(ushort count) : base()
        {
            Count = count;
        }
        /// <summary>
        /// 選択した名前
        /// </summary>
        public ushort Count { get; set; }
    }

    /// <summary>
    /// PLCコマンドのイベントハンドラ
    /// </summary>
    public class PLCCommandEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PLCCommandEventArgs(byte[] resbyte, string cmdstring = null) : base()
        {
            CmdByte = resbyte;
            Cmdstring = cmdstring;
        }
        /// <summary>
        /// byte用
        /// </summary>
        public byte[] CmdByte { get; set; }
        /// <summary>
        /// string
        /// </summary>
        public string Cmdstring { get; set; }
    }
    /// <summary>
    /// PLCコマンド(bool)のイベントハンドラ
    /// </summary>
    public class PLCBitResEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PLCBitResEventArgs(IList<bool> _resbit, string type=null) : base()
        {
            ResBit = _resbit;
            Type = type;
        }
        /// <summary>
        /// boolリスト
        /// </summary>
        public IList<bool> ResBit { get; set; }
        /// <summary>
        /// ﾀｲﾌﾟ
        /// </summary>
        public string Type { get; set; }
    }
    /// <summary>
    /// PLCコマンド(text)のイベントハンドラ
    /// </summary>
    public class PLCWordResEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PLCWordResEventArgs(string[] _resWord, string type= null) : base()
        {
            ResWord = _resWord;
            Type = type;
        }
        /// <summary>
        /// PLCコマンド(text)
        /// </summary>
        public string[] ResWord { get; set; }
        /// <summary>
        /// ﾀｲﾌﾟ
        /// </summary>
        public string Type { get; set; }
    }

    /// <summary>
    /// DIOデータイベントハンドラ
    /// </summary>
    public class DioEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DioEventArgs(bool[] diodata) : base()
        {
            Diodata = diodata;
        }
        /// <summary>
        /// bool用
        /// </summary>
        public bool[] Diodata { get; set; }
    }

    /// <summary>
    /// 検出器からデータを受信したときのイベントハンドラ
    /// </summary>
    public class DetectorResEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetectorResEventArgs(IntPtr resptr) : base()
        {
            ResPtr = resptr;
        }
        /// <summary>
        /// ポインタ
        /// </summary>
        public IntPtr ResPtr { get; set; }
    }
}
