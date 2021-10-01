using Microsoft.Extensions.DependencyInjection;
using CTAddress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CTAddress
{
    public class CalSlicePitchService : ICalSlicePitchService
    {
        /// <summary>
        /// スライスピッチ
        /// </summary>
        public float SlicePitch { get; private set; }
        /// <summary>
        /// ターゲットファイル表示
        /// </summary>
        public string TargetFile { get; private set; }
        /// <summary>
        /// スライスピッチ計算完了イベント
        /// </summary>
        public event EventHandler EndCalPitch;
        /// <summary>
        /// ターゲットファイルサービス
        /// </summary>
        private readonly ILoadTargetFileService _Target;
        /// <summary>
        /// ターゲットファイルサービス
        /// </summary>
        private readonly ILoadInfFileService _LoadInf;
        /// <summary>
        /// スライスピッチ計算サービス
        /// </summary>
        public CalSlicePitchService(ILoadTargetFileService taget, ILoadInfFileService load)
        {

            _Target = taget;
            _Target.EndLoadInf += (s, e) =>
            {
                if (s is LoadTargetFileService ltfs)
                {
                    TargetFile = ltfs.TargetFile;
                }
            };

            _LoadInf = load;
        }
        /// <summary>
        /// スライスピッチ計算
        /// </summary>
        /// <param name="file"></param>
        public void DoCalSlicePitch(IEnumerable<ImportFile> files)
        {
            if (string.IsNullOrEmpty(TargetFile))
            {
                _Target.DoInfLoad(files.FirstOrDefault().FileName);
            }

            ImportFile taget = null;
            var TmpFiles = files.Select(p => p.FileName.ToUpper()).ToList().Find(p => p == TargetFile.ToUpper());
            if (TmpFiles != null)
            {
                taget = files.ToList().Find(p => p.FileName.ToUpper() == TmpFiles);
            }

            ImportFile nextMicaget = files.Where(q => q.ConeNo == taget.ConeNo).ToList().Find(p => p.SliceNo == taget.SliceNo + 1);

            ImportFile preMicaget = files.Where(q => q.ConeNo == taget.ConeNo).ToList().Find(p => p.SliceNo == taget.SliceNo - 1);

            IEnumerable<ImportFile> nexconetagets = files.Where(q => q.ConeNo == taget.ConeNo + 1);

            ImportFile nexconetaget = null;
            if (nexconetagets.Count() != 0)
            {
                nexconetaget = nexconetagets.Aggregate((p, q) => p.SliceNo < q.SliceNo ? p : q);
            }

            ImportFile preconetaget = null;
            IEnumerable<ImportFile> preconetagets = files.Where(q => q.ConeNo == taget.ConeNo - 1);
            if (preconetagets.Count() != 0)
            {
                preconetaget = preconetagets.Aggregate((p, q) => p.SliceNo > q.SliceNo ? p : q);
            }

            var slicepitch = 0F;
            if (nextMicaget != null)
            {
                slicepitch = CalSlicePitch(taget, nextMicaget);
            }
            else if (preMicaget != null)
            {
                slicepitch = CalSlicePitch(taget, preMicaget);
            }
            else if (nexconetagets.Count() != 0)
            {
                slicepitch = CalSlicePitch(taget, nexconetaget);
            }
            else if (preconetagets.Count() != 0)
            {
                slicepitch = CalSlicePitch(taget, preconetaget);
            }

            SlicePitch = slicepitch;

            EndCalPitch?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taget"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        private float CalSlicePitch(ImportFile taget, ImportFile next)
        {
            float posi1 = (float)Math.Round(_LoadInf.GetSlicePosi(Path.ChangeExtension(taget.FileName, "inf")), 3, MidpointRounding.AwayFromZero);

            float posi2 = (float)Math.Round(_LoadInf.GetSlicePosi(Path.ChangeExtension(next.FileName, "inf")), 3, MidpointRounding.AwayFromZero);

            return (float)Math.Round(Math.Abs(posi1 - posi2), 3, MidpointRounding.AwayFromZero);
        }
    }
    /// <summary>
    /// スライスピッチ計算サービス I/F
    /// </summary>
    public interface ICalSlicePitchService
    {
        /// <summary>
        /// スライスピッチの計算完了
        /// </summary>
        event EventHandler EndCalPitch;
        /// <summary>
        /// スライスピッチ計算
        /// </summary>
        /// <param name="file"></param>
        void DoCalSlicePitch(IEnumerable<ImportFile> files);
    }
}
