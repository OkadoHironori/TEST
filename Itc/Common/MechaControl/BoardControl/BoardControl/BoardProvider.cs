using Itc.Common.Event;
using Itc.Common.Extensions;
using Itc.Common.TXEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board.BoardControl
{
    /// <summary>
    /// ボードデータ送信クラス
    /// </summary>
    public class BoardProvider : IBoardProvider, INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// 変更通知
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        
        ///// <summary>
        ///// PLCProviderのイベントハンドラ
        ///// </summary>
        //public event Property​Changed​Event​Handler BoardValueChanged;
        /// <summary>
        /// ボードパラメータ
        /// </summary>
        public IEnumerable<BoardChk> Boards { get; private set; }
        /// <summary>
        /// 回転カウンタ
        /// </summary>
        private uint _RotCount;
        /// <summary>
        /// 回転カウンタ
        /// </summary>
        public uint RotCount
        {
            get => _RotCount;
            set
            {
                if (_RotCount == value)
                    return;
                _RotCount = value;
                RaisePropertyChanged(nameof(RotCount));
            }
        }
        /// <summary>
        /// 回転状態
        /// </summary>
        private ushort _RotSts;
        /// <summary>
        /// 回転状態
        /// </summary>
        public ushort RotSts
        {
            get => _RotSts;
            set
            {
                if (_RotSts == value)
                    return;
                _RotSts = value;
                RaisePropertyChanged(nameof(RotSts));
            }
        }
        /// <summary>
        /// 回転速度
        /// </summary>
        private uint _RotSpeed;
        /// <summary>
        ///回転速度
        /// </summary>
        public uint RotSpeed
        {
            get => _RotSpeed;
            set
            {
                if (_RotSpeed == value)
                    return;
                _RotSpeed = value;
                RaisePropertyChanged(nameof(RotSpeed));
            }
        }
        /// <summary>
        /// 回転エラーメッセージ
        /// </summary>
        private string _RotErrorMessage;
        /// <summary>
        /// 回転エラーメッセージ
        /// </summary>
        public string RotErrorMessage
        {
            get => _RotErrorMessage;
            set
            {
                if (_RotErrorMessage == value)
                    return;
                _RotErrorMessage = value;
                RaisePropertyChanged(nameof(RotErrorMessage));
            }
        }
        /// <summary>
        /// 昇降カウンタ
        /// </summary>
        private uint _UDCount;
        /// <summary>
        /// 昇降カウンタ
        /// </summary>
        public uint UDCount
        {
            get => _UDCount;
            set
            {
                if (_UDCount == value)
                    return;
                _UDCount = value;
                RaisePropertyChanged(nameof(UDCount));
            }
        }
        /// <summary>
        /// 昇降状態
        /// </summary>
        public ushort _UDSts;
        /// <summary>
        /// 昇降状態
        /// </summary>
        public ushort UDSts
        {
            get => _UDSts;
            set
            {
                if (_UDSts == value)
                    return;
                _UDSts = value;
                RaisePropertyChanged(nameof(UDSts));
            }
        }
        /// <summary>
        /// 昇降速度
        /// </summary>
        private uint _UDSpeed;
        /// <summary>
        /// 昇降速度
        /// </summary>
        public uint UDSpeed
        {
            get => _UDSpeed;
            set
            {
                if (_UDSpeed == value)
                    return;
                _UDSpeed = value;
                RaisePropertyChanged(nameof(UDSpeed));
            }
        }
        /// <summary>
        /// 昇降エラーメッセージ
        /// </summary>
        private string _UDErrorMessage;
        /// <summary>
        /// 昇降エラーメッセージ
        /// </summary>
        public string UDErrorMessage
        {
            get => _UDErrorMessage;
            set
            {
                if (_UDErrorMessage == value)
                    return;
                _UDErrorMessage = value;
                RaisePropertyChanged(nameof(UDErrorMessage));
            }
        }
        /// <summary>
        /// X軸微調_カウンタ
        /// </summary>
        private uint _FXCount;
        /// <summary>
        /// X軸微調_カウンタ
        /// </summary>
        public uint FXCount
        {
            get => _FXCount;
            set
            {
                if (_FXCount == value)
                    return;
                _FXCount = value;
                RaisePropertyChanged(nameof(FXCount));
            }
        }
        /// <summary>
        /// X軸微調_状態
        /// </summary>
        public ushort _FXSts;
        /// <summary>
        /// X軸微調_状態
        /// </summary>
        public ushort FXSts
        {
            get => _FXSts;
            set
            {
                if (_FXSts == value)
                    return;
                _FXSts = value;
                RaisePropertyChanged(nameof(FXSts));
            }
        }
        /// <summary>
        /// X軸微調_速度
        /// </summary>
        private uint _FXSpeed;
        /// <summary>
        /// X軸微調_速度
        /// </summary>
        public uint FXSpeed
        {
            get => _FXSpeed;
            set
            {
                if (_FXSpeed == value)
                    return;
                _FXSpeed = value;
                RaisePropertyChanged(nameof(FXSpeed));
            }
        }
        /// <summary>
        /// X軸微調_エラーメッセージ
        /// </summary>
        private string _FXErrorMessage;
        /// <summary>
        /// X軸微調_エラーメッセージ
        /// </summary>
        public string FXErrorMessage
        {
            get => _FXErrorMessage;
            set
            {
                if (_FXErrorMessage == value)
                    return;
                _FXErrorMessage = value;
                RaisePropertyChanged(nameof(FXErrorMessage));
            }
        }
        /// <summary>
        /// Y軸微調_カウンタ
        /// </summary>
        private uint _FYCount;
        /// <summary>
        /// Y軸微調_カウンタ
        /// </summary>
        public uint FYCount
        {
            get => _FYCount;
            set
            {
                if (_FYCount == value)
                    return;
                _FYCount = value;
                RaisePropertyChanged(nameof(FYCount));
            }
        }
        /// <summary>
        /// Y軸微調_状態
        /// </summary>
        public ushort _FYSts;
        /// <summary>
        /// Y軸微調_状態
        /// </summary>
        public ushort FYSts
        {
            get => _FYSts;
            set
            {
                if (_FYSts == value)
                    return;
                _FYSts = value;
                RaisePropertyChanged(nameof(FYSts));
            }
        }
        /// <summary>
        /// Y軸微調_速度
        /// </summary>
        private uint _FYSpeed;
        /// <summary>
        /// Y軸微調_速度
        /// </summary>
        public uint FYSpeed
        {
            get => _FYSpeed;
            set
            {
                if (_FYSpeed == value)
                    return;
                _FYSpeed = value;
                RaisePropertyChanged(nameof(FYSpeed));
            }
        }
        /// <summary>
        /// Y軸微調_エラーメッセージ
        /// </summary>
        private string _FYErrorMessage;
        /// <summary>
        /// Y軸微調_エラーメッセージ
        /// </summary>
        public string FYErrorMessage
        {
            get => _FYErrorMessage;
            set
            {
                if (_FYErrorMessage == value)
                    return;
                _FYErrorMessage = value;
                RaisePropertyChanged(nameof(FYErrorMessage));
            }
        }
        /// <summary>
        /// ボードの定期監視用
        /// </summary>
        private System.Timers.Timer _Timer = null;
        /// <summary>
        /// ボード設定
        /// </summary>
        private readonly IBoardConfig _BoardConf;
        /// <summary>
        /// ボードデータ送信クラス
        /// </summary>
        public BoardProvider(IBoardConfig config)
        {
            _BoardConf = config;
            _BoardConf.EndLoadBoardConf += (s, e) =>
            {
                BoardConfig bc = s as BoardConfig;
                Boards = bc.Boards;
            };
            _BoardConf.RequestParam();

        }

        /// <summary>
        /// ボードチェッカの開始
        /// </summary>
        public void CheckerStart()
        {
            if (_Timer == null)
            {
                _Timer = new System.Timers.Timer
                {
                    Interval = 10,
                    AutoReset = false
                };

                _Timer.Elapsed += delegate
                {
                    foreach (var d in Boards)
                    {
                        uint ret = 0;

                        uint countdata = 0;

                        ret = Hicpd530.cp530_rReg(d.ID, d.Axis, Cp530l1a.RRCTR1, ref countdata);
                        if (ret != 0)
                        {
                            throw new Exception($"{BoardMes.BOARD_ERROR}");
                        }

                        switch (d.BAxis)
                        {
                            case (BoardAxis.ROT_JIKU):
                                RotCount = countdata;
                                break;
                            case (BoardAxis.UD_JIKU):
                                UDCount = countdata;
                                break;
                            case (BoardAxis.XSTG_JIKU):
                                FXCount = countdata;
                                break;
                            case (BoardAxis.YSTG_JIKU):
                                FYCount = countdata;
                                break;
                        }

                        //ボード速度の監視
                        uint boardspeed = 0;
                        ret = Hicpd530.cp530_rReg(d.ID, d.Axis, Cp530l1a.RRSPD, ref boardspeed);
                        if (ret != 0)
                        {
                            throw new Exception($"{BoardMes.BOARD_ERROR}");
                        }

                        switch (d.BAxis)
                        {
                            case (BoardAxis.ROT_JIKU):
                                RotSpeed = boardspeed & 0x00ff; //下位16bitのみ、この後で速度倍率をかける必要あり
                                break;
                            case (BoardAxis.UD_JIKU):
                                UDSpeed = boardspeed & 0x00ff; //下位16bitのみ、この後で速度倍率をかける必要あり
                                break;
                            case (BoardAxis.XSTG_JIKU):
                                FXSpeed = boardspeed & 0x00ff; //下位16bitのみ、この後で速度倍率をかける必要あり
                                break;
                            case (BoardAxis.YSTG_JIKU):
                                FYSpeed = boardspeed & 0x00ff; //下位16bitのみ、この後で速度倍率をかける必要あり
                                break;
                        }

                        //ボードステータスの監視
                        ushort sts = 0;
                        ret = Hicpd530.cp530_rSstsW(d.ID, d.Axis, ref sts);
                        if (ret != 0)
                        {
                            throw new Exception($"{BoardMes.BOARD_ERROR}");
                        }

                        switch (d.BAxis)
                        {
                            case (BoardAxis.ROT_JIKU):
                                RotSts = sts;
                                break;
                            case (BoardAxis.UD_JIKU):
                                UDSts = sts;
                                break;
                            case (BoardAxis.XSTG_JIKU):
                                FXSts = sts;
                                break;
                            case (BoardAxis.YSTG_JIKU):
                                FYSts = sts;
                                break;
                        }

                        //ボードのエラーメッセージ監視
                        //ボードエラーの監視
                        uint errorcode = 0;
                        ret = Hicpd530.cp530_rMstsW(d.ID, d.Axis, ref sts);
                        if (ret != 0)
                        {
                            throw new Exception($"{nameof(BoardProvider)}{Environment.NewLine}{BoardMes.BOARD_ERROR}");
                        }
                        if ((sts & 0x0010) == 0x0010)
                        {
                            ret = Hicpd530.cp530_rReg(d.ID, d.Axis, Cp530l1a.RREST, ref errorcode);
                            if (ret != 0)
                            {
                                throw new Exception($"{nameof(BoardProvider)}{Environment.NewLine}{BoardMes.BOARD_ERROR}");
                            }

                            if (errorcode != 0)
                            {
                                if ((errorcode & 0x20) == 0x20)
                                {
                                    switch (d.BAxis)
                                    {
                                        case (BoardAxis.ROT_JIKU):
                                            RotErrorMessage = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{BoardMes.BOARD_ERROR_P_XELS}";
                                            break;
                                        case (BoardAxis.UD_JIKU):
                                            UDErrorMessage = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{BoardMes.BOARD_ERROR_P_XELS}";
                                            break;
                                        case (BoardAxis.XSTG_JIKU):
                                            FXErrorMessage = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{BoardMes.BOARD_ERROR_P_XELS}";
                                            break;
                                        case (BoardAxis.YSTG_JIKU):
                                            FYErrorMessage = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{BoardMes.BOARD_ERROR_P_XELS}";
                                            break;
                                    }


                                }
                                if ((errorcode & 0x40) == 0x40)
                                {
                                    switch (d.BAxis)
                                    {
                                        case (BoardAxis.ROT_JIKU):
                                            RotErrorMessage = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{BoardMes.BOARD_ERROR_M_XELS}";
                                            break;
                                        case (BoardAxis.UD_JIKU):
                                            UDErrorMessage = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{BoardMes.BOARD_ERROR_M_XELS}";
                                            break;
                                        case (BoardAxis.XSTG_JIKU):
                                            FXErrorMessage = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{BoardMes.BOARD_ERROR_M_XELS}";
                                            break;
                                        case (BoardAxis.YSTG_JIKU):
                                            FYErrorMessage = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{BoardMes.BOARD_ERROR_M_XELS}";
                                            break;
                                    }

                                }
                                if ((errorcode & 0x80) == 0x80)
                                {
                                    switch (d.BAxis)
                                    {
                                        case (BoardAxis.ROT_JIKU):
                                            RotErrorMessage = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{BoardMes.BOARD_ERROR_XSVALM}";
                                            break;
                                        case (BoardAxis.UD_JIKU):
                                            UDErrorMessage = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{BoardMes.BOARD_ERROR_XSVALM}";
                                            break;
                                        case (BoardAxis.XSTG_JIKU):
                                            FXErrorMessage = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{BoardMes.BOARD_ERROR_XSVALM}";
                                            break;
                                        case (BoardAxis.YSTG_JIKU):
                                            FYErrorMessage = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{BoardMes.BOARD_ERROR_XSVALM}";
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (d.BAxis)
                                    {
                                        case (BoardAxis.ROT_JIKU):
                                            RotErrorMessage = BoardMes.BOARD_ERROR;
                                            break;
                                        case (BoardAxis.UD_JIKU):
                                            UDErrorMessage  = BoardMes.BOARD_ERROR;
                                            break;
                                        case (BoardAxis.XSTG_JIKU):
                                            FXErrorMessage = BoardMes.BOARD_ERROR;
                                            break;
                                        case (BoardAxis.YSTG_JIKU):
                                            FYErrorMessage = BoardMes.BOARD_ERROR; 
                                            break;
                                    }

                                }
                            }
                        }
                        else
                        {
                            switch (d.BAxis)
                            {
                                case (BoardAxis.ROT_JIKU):
                                    _RotErrorMessage = String.Empty;
                                    break;
                                case (BoardAxis.UD_JIKU):
                                    UDErrorMessage = String.Empty;
                                    break;
                                case (BoardAxis.XSTG_JIKU):
                                    FXErrorMessage = String.Empty;
                                    break;
                                case (BoardAxis.YSTG_JIKU):
                                    FYErrorMessage = String.Empty;
                                    break;
                            }

                        }
                    }
                    _Timer.Start();
                };
                _Timer.Start();
            }
        }
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            if (_Timer != null)
            {
                _Timer.Enabled = false;//タイマー停止
                _Timer.Stop();

                Task.WaitAll(Task.Delay(100));
                _Timer.Dispose();
                _Timer = null;
            }

            Boards = null;

            return;
        }
    }
    public interface IBoardProvider
    {
        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// ボードチェッカの開始
        /// </summary>
        void CheckerStart();
    }
}
