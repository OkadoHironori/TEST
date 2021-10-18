using IRayControler;
using Itc.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetectorControl
{
    /// <summary>
    /// ディテクタのポインタデータを配列データに変換
    /// </summary>
    public class DetectorData: IDetectorData
    {
        /// <summary>
        /// LUT用データお願いイベント
        /// </summary>
        public event EventHandler SendArrayData;
        /// <summary>
        /// 配列変換完了イベント
        /// </summary>
        public event EventHandler EndConvertArrayData;
        /// <summary>
        /// 配列データ(ushort)
        /// </summary>
        public ushort[] ArrayData { get; private set; }
        /// <summary>
        /// 表示画像幅
        /// </summary>
        public int DispWidth { get; private set; }
        /// <summary>
        /// 表示画像高さ
        /// </summary>
        public int DispHeight { get; private set; }
        /// <summary>
        /// CW90回転
        /// </summary>
        public bool CW90 { get; private set; }
        /// <summary>
        /// CCW90回転
        /// </summary>
        public bool CCW90 { get; private set; }
        /// <summary>
        /// 左右反転
        /// </summary>
        public bool FlipHorizontal { get; private set; }
        /// <summary>
        /// 上下反転
        /// </summary>
        public bool FlipVertical { get; private set; }
        /// <summary>
        /// 検出器データの設定
        /// </summary>
        private readonly IDetConf _DetConf;
        /// <summary>
        /// IRayデータ収集
        /// </summary>
        private readonly IIRaySeqAcquisition _IRaySeqAcq;
        /// <summary>
        /// 上下反転 I/F
        /// </summary>
        private readonly IFlipVertical _FlipVertical;
        /// <summary>
        /// 左右反転 I/F
        /// </summary>
        private readonly IFlipHorizontal _FlipHorizontal;
        /// <summary>
        /// CCW90回転 I/F
        /// </summary>
        private readonly IRotCCW90 _RotCCW90;
        /// <summary>
        /// CW90回転 I/F
        /// </summary>
        private readonly IRotCW90 _RotCW90;
        /// <summary>
        /// ディテクタのポインタデータを配列データに変換
        /// </summary>
        public DetectorData(IIRaySeqAcquisition irayacq, IDetConf conf, IFlipVertical flv, IFlipHorizontal flh, IRotCCW90 ccw90, IRotCW90 cw90)
        {
            _FlipVertical = flv;
            _FlipVertical.EndFlipVertical += (s, e) =>
            {
                var fv = s as FlipVertical;
                DispWidth = fv.Width;
                DispHeight = fv.Height;
                ArrayData = fv.Targets;
            };
            _FlipHorizontal = flh;
            _FlipHorizontal.EndFlipHorizontal += (s, e) =>
            {
                var fh = s as FlipHorizontal;
                DispWidth = fh.Width;
                DispHeight = fh.Height;
                ArrayData = fh.Targets;
            };
            _RotCCW90 = ccw90;
            _RotCCW90.EndRotCCW90 += (s, e) =>
            {
                var rccw90 = s as RotCCW90;
                DispWidth = rccw90.Width;
                DispHeight = rccw90.Height;
                ArrayData = rccw90.Targets;
            };
            _RotCW90 = cw90;
            _RotCW90.EndRotCW90 += (s, e) =>
            {
                var rcw90 = s as RotCW90;
                DispWidth = rcw90.Width;
                DispHeight = rcw90.Height;
                ArrayData = rcw90.Targets;
            };

            _IRaySeqAcq = irayacq;
            _IRaySeqAcq.EndAcqData += (s, e) =>
            {
                var irsq = s as IRaySeqAcquisition;
                int detwidht = irsq.Width;
                int detheight = irsq.Height;
                IntPtr ptr = irsq.DataPtr;
                DoCreateArrayData(ptr, detwidht, detheight);
            };

            _DetConf = conf;
            _DetConf.EndLoadParam += (s, e) => 
            {
                var dc = s as DetConf;
                CW90 = dc.CW90;
                CCW90 = dc.CCW90;
                FlipHorizontal = dc.FlipHorizontal;
                FlipVertical = dc.FlipVertical;
            };
            _DetConf.RequestParam();
        }
        /// <summary>
        /// ポインタから配列への変換完（CW、CCW、上下反転、左右反転）
        /// </summary>
        public void DoCreateArrayData(IntPtr ptr, int detwidht, int detheight)
        {
            DispWidth = detwidht;
            DispHeight = detheight;
            ArrayData = BinaryConverter.ConvertSPtrtoUS(ptr, detwidht * detheight);
            SendArrayData?.Invoke(this, new EventArgs());//LUT用最大値、最小値

            if (FlipVertical)
            {
                _FlipVertical.DoFlipVertical(ArrayData, DispWidth, DispHeight);
            }

            if (FlipHorizontal)
            {
                _FlipHorizontal.DoFlipHorizontal(ArrayData, DispWidth, DispHeight);
            }
            //CW90回転
            if (CW90)
            {
                _RotCW90.DoRotCW90(ArrayData, DispWidth, DispHeight);
            }

            if(CCW90)
            {
                _RotCCW90.DoRotCCW90(ArrayData, DispWidth, DispHeight);
            }


            EndConvertArrayData?.Invoke(this, new EventArgs());
        }
    }
    /// <summary>
    /// ディテクタのポインタデータを配列データに変換I/F
    /// </summary>
    public interface IDetectorData
    {
        /// <summary>
        /// ポインタから配列への変換完（CW、CCW、上下反転、左右反転）
        /// </summary>
        void DoCreateArrayData(IntPtr ptr, int detwidht, int detheight);
        /// <summary>
        /// 配列変換完了イベント
        /// </summary>
        event EventHandler EndConvertArrayData;
        /// <summary>
        /// LUT用データお願いイベント
        /// </summary>
        event EventHandler SendArrayData;
    }
}
