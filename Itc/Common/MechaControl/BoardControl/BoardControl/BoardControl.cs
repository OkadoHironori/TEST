using Itc.Common.TXEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Board.BoardControl
{
    public class BoardControl : IBoardControl, IDisposable
    {
        public uint SetSpdReg(uint bnum, ushort axis, uint fhspd, uint flspd, uint rate, uint pos, uint mode)
        {
            uint ret = 0;
            //速度レジスタ書込み FH速度
            ret = Hicpd530.cp530_wReg(bnum, axis, Cp530l1a.WPRFH, fhspd);
            if (ret != 0)
            {
                return ret;
            }
            // 速度レジスタ書込み FL速度
            ret = Hicpd530.cp530_wReg(bnum, axis, Cp530l1a.WPRFL, flspd);
            if (ret != 0)
            {
                return ret;
            }
            // 加減速レートレジスタ書込み
            ret = Hicpd530.cp530_wReg(bnum, axis, Cp530l1a.WPRUR, rate);
            if (ret != 0)
            {
                return ret;
            }
            // 減速レートレジスタセット
            ret = Hicpd530.cp530_wReg(bnum, axis, Cp530l1a.WPRDR, rate);
            if (ret != 0)
            {
                return ret;
            }
            // 移動量レジスタセット
            ret = Hicpd530.cp530_wReg(bnum, axis, Cp530l1a.WPRMV, pos);
            if (ret != 0)
            {
                return ret;
            }
            // 動作モードレジスタセット
            ret = Hicpd530.cp530_wReg(bnum, axis, Cp530l1a.WPRMD, mode);
            if (ret != 0)
            {
                return ret;
            }
            return ret;
        }
        /// <summary>
        /// 加減速レートを求める
        /// </summary>
        /// <param name="UdRateTime"></param>
        /// <param name="flspd"></param>
        /// <param name="fhspd"></param>
        /// <returns></returns>
        public ushort AccelerationRate(double ratetime, ushort flspd, ushort fhspd)
        {
            ushort ans = 0;

            if (fhspd != flspd)
            {
                ans = (ushort)(((ratetime * 19660800) / (4 * (fhspd - flspd))) - 1);    // 加減速レート
            }

            if (fhspd == flspd || ans > ushort.MaxValue)
            {
                ans = ushort.MaxValue;
            }
            else if (ans < 1)
            {
                ans = 1;
            }

            return (ans);
        }
        /// <summary>
        /// deg -> plsに変換
        /// </summary>
        /// <param name="deg"></param>
        /// <param name="step"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public uint DegToPls(float deg, double step, int dir)
        {
            return (uint)Math.Round(deg / step * dir);
        }
        /// <summary>
        /// mms から PPSに変換
        /// </summary>
        /// <param name="spd"></param>
        /// <param name="step"></param>
        /// <param name="mag"></param>
        /// <returns></returns>
        public ushort SpdToPps(float spd, double step, int mag)
        {
            uint ret = 0;
            ret = (uint)(spd / step / mag);

            ret = Math.Min(ret, ushort.MaxValue);
            ret = Math.Max(ret, 1);

            return (ushort)ret;
        }
        /// <summary>
        /// Pls -> Degに変換
        /// </summary>
        /// <param name="pls"></param>
        /// <param name="step"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public float PlsToDeg(int pls, double step, int dir)
        {
            return (float)Math.Round((pls * (float)step * dir), 3);
        }
        /// <summary>
        /// 破棄
        /// 破棄するときにボードを閉じる
        /// </summary>
        public void Dispose()
        {
            //foreach (var dev in DevInfs)
            //{
            //    uint unRet = Hicpd530.cp530_CloseDevice(dev.DevNum);
            //    if (unRet != 0)
            //    {
            //        var mes = $"{ BoardMes.BOARD_CLOSE_ERROR}{Environment.NewLine}{unRet.ToString("X8")}";
            //        MessageBox.Show(mes);
            //        return;
            //    }
            //}
            //return;
        }
    }

    public interface IBoardControl
    {
        /// <summary>
        /// ボード初期化
        /// </summary>
        /// <param name="boardnum"></param>
        //Devinf Init(Action<string> mesbox, BoardAxis axis);
        /// <summary>
        /// X線・検出器昇降タイプか？
        /// </summary>
        /// <returns></returns>
        //bool IsXrayUDType();
        // <summary>
        /// 速度、位置、動作モードを設定する
        /// </summary>
        /// <param name="SetSpdReg"></param>
        /// <param name="bnum"></param>
        /// <param name="axis"></param>        
        /// <param name="fhspd"></param>
        /// <param name="flspd"></param>
        /// <param name="rate"></param>
        /// <param name="pos"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        uint SetSpdReg(uint bnum, ushort axis, uint fhspd, uint flspd, uint rate, uint pos, uint mode);
        /// <summary>
        /// パルス →　Degに変換
        /// </summary>
        /// <param name="Count"></param>
        /// <returns></returns>
        float PlsToDeg(int pls, double step, int dir);
        /// <summary>
        /// RPM から PPSに変換
        /// </summary>
        /// <param name="Rpm"></param>
        /// <returns></returns>
        ushort SpdToPps(float spd, double step, int mag);
        /// <summary>
        /// deg -> plsに変換
        /// </summary>
        /// <param name="MmDeg"></param>
        /// <returns></returns>
        uint DegToPls(float deg, double step, int dir);
        /// <summary>
        /// 加減速レートを求める
        /// </summary>
        /// <param name="ratetime"></param>
        /// <param name="flspd"></param>
        /// <param name="fhspd"></param>
        /// <returns></returns>
        ushort AccelerationRate(double ratetime, ushort flspd, ushort fhspd);
    }
}
