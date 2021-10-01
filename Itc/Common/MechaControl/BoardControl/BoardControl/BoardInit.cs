using System;
using System.Collections.Generic;
using System.Linq;

namespace Board.BoardControl
{
    /// <summary>
    /// ボード初期化
    /// </summary>
    public class BoardInit : IBoardInit, IDisposable
    {
        /// <summary>
        /// ボード情報読込イベント
        /// </summary>
        public event EventHandler EndLoadBoard;
        /// <summary>
        /// 検知できるボードの最大値
        /// </summary>
        private readonly int BoardMax = 16;
        /// <summary>
        /// デバイス情報
        /// </summary>
        public IEnumerable<Devinf> DevInfs { get;private set; }
        /// <summary>
        /// ボード初期化
        /// </summary>
        public BoardInit()
        {
            if (DevInfs == null)
            {

                Hicpd530.HPCDEVICEINFO[] h = new Hicpd530.HPCDEVICEINFO[BoardMax];
                uint cnt = 0;//デバイス個数
                uint unRet = 0;//ボードの返値

                // ボード枚数取得
                if (unRet != Hicpd530.cp530_GetDeviceCount(ref cnt))
                {
                    throw new Exception(
                        $"{BoardMes.BOARD_CONNECTION_ERROR}" +
                        $"{Environment.NewLine}" +
                        $"{BoardMes.CONFIRM_BOARD}");
                }

                // ボード枚数が0枚または16枚以上の時はエラー
                if ((0 == cnt) || (16 <= cnt))
                {
                    throw new Exception(
                        $"Cannot deteced HPCD Board");
                }

                //デバイス情報取得 所得失敗時はエラー
                if (unRet != Hicpd530.cp530_GetDeviceInfo(ref cnt, h))
                {
                    throw new Exception(
                        $"Cannot get HPCD Board information");
                }
                //デバイス情報取得
                IEnumerable<Hicpd530.HPCDEVICEINFO> devInfs = Enumerable.Range(0, (int)cnt).Select(p => h[p]);
                List<Devinf> tmpdevInfs = new List<Devinf>();
                foreach (var dev in devInfs)
                {
                    var refdev = dev;
                    uint tmpdevId = 0;
                    // デバイスオープン
                    if (unRet != Cp530l1a.hcp530_DevOpen(ref tmpdevId, ref refdev))
                    {
                        throw new Exception(
                            $"Board Open Error");
                    }
                    tmpdevInfs.Add(new Devinf()
                    {
                        DevNum = tmpdevId,
                        Hpcdeviceinfo = refdev
                    });
                }
                DevInfs = tmpdevInfs;
            }
        }
        /// <summary>
        /// ボード初期化
        /// </summary>
        /// <param name="devinfs"></param>
        public BoardInit(IEnumerable<Devinf> devinfs)
        {
            DevInfs = devinfs;
        }
        /// <summary>
        /// パラメータ要求
        /// </summary>
        public void RequestParam()
        => EndLoadBoard?.Invoke(this, new EventArgs());
        /// <summary>
        /// 破棄
        /// 破棄するときにボードを閉じる
        /// </summary>
        public void Dispose()
        {
            foreach (var dev in DevInfs)
            {
                uint unRet = Hicpd530.cp530_CloseDevice(dev.DevNum);
                if (unRet != 0)
                {
                    throw new Exception($"{ BoardMes.BOARD_CLOSE_ERROR}{Environment.NewLine}{unRet.ToString("X8")}");
                }
            }
            return;
        }
    }
    /// <summary>
    /// デバイス情報
    /// </summary>
    public class Devinf
    {
        /// <summary>
        /// デバイス番号
        /// </summary>
        public uint DevNum { get; set; }
        /// <summary>
        /// デバイス情報
        /// </summary>
        public Hicpd530.HPCDEVICEINFO Hpcdeviceinfo { get; set; }

    }
    /// <summary>
    /// ボード初期化I/F
    /// </summary>
    public interface IBoardInit
    {
        /// <summary>
        /// ボード情報
        /// </summary>
        event EventHandler EndLoadBoard;
        /// <summary>
        /// パラメータ要求
        /// </summary>
        void RequestParam();
    }

}
