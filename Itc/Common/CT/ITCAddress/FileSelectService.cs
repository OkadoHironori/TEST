using CTAddress;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CTAddress
{
    /// <summary>
    /// フォルダ参照クラス
    /// </summary>
    public class FileSelectService : IFileSelectService
    {
        /// <summary>
        /// ファイル検索完了
        /// </summary>
        public event EventHandler CmpSerchFile;
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string ErrorMes { get; private set; }
        /// <summary>
        /// インポート用ファイル
        /// </summary>
        public IEnumerable<ImportFile> ImportFiles { get; private set; }
        /// <summary>
        /// ターゲットファイル
        /// </summary>
        public string TargetFile { get; private set; }
        /// <summary>
        /// スライスピッチ
        /// </summary>
        public float SlicePitch { get; private set; }
        /// <summary>
        /// スケール（Chara型のスキャンエリア）
        /// </summary>
        public string Scale { get; private set; }
        /// <summary>
        /// マトリックス
        /// </summary>
        public string Matrix { get; private set; }
        /// <summary>
        /// テーブル位置 mm
        /// </summary>
        public float TblPosi { get; private set; }
        /// <summary>
        /// システム名
        /// </summary>
        public string SystemName { get; private set; }
        /// <summary>
        /// 画素サイズ
        /// </summary>
        public float DotSize => (float)(float.Parse(Scale) / int.Parse(Matrix));
        /// <summary>
        /// ターゲットファイルサービス
        /// </summary>
        private readonly ILoadTargetFileService _Target;
        /// <summary>
        /// スライスピッチを計算するサービス
        /// </summary>
        private readonly ICalSlicePitchService _CalSlicePicth;
        /// <summary>
        /// 付帯情報読込サービス
        /// </summary>
        private readonly ILoadInfFileService _InfLoad;
        /// <summary>
        /// ファイル選択サービス
        /// </summary>
        public FileSelectService(ILoadTargetFileService target, ICalSlicePitchService calsp, ILoadInfFileService infload)
        {
            _Target = target;
            _Target.EndLoadInf += (s, e) =>
            {
                if (s is LoadTargetFileService dtfs)
                {
                    TargetFile = dtfs.TargetFile;
                    Matrix = dtfs.Matrix;
                    Scale = dtfs.Scale;
                    SystemName = dtfs.SystemName;

                    if (!string.IsNullOrEmpty(dtfs.TblPosi))
                    {
                        TblPosi = float.Parse(dtfs.TblPosi);
                    }
                }
            };

            _CalSlicePicth = calsp;
            _CalSlicePicth.EndCalPitch += (s, e) =>
            {
                if (s is CalSlicePitchService csps)
                {
                    SlicePitch = csps.SlicePitch;
                }
            };

            _InfLoad = infload;
        }
        /// <summary>
        /// フォルダ選択によるファイルリスト生成
        /// </summary>
        public void SetTargets(IEnumerable<string> targets)
        {
            IEnumerable<ImportFile> candidate = new List<ImportFile>();
            foreach (var target in targets)
            {
                candidate = GetFileNames(target);
            }

            _CalSlicePicth.DoCalSlicePitch(candidate);

            GetImportFiles(candidate, targets.FirstOrDefault(), SlicePitch, out IEnumerable<ImportFile> files);

            ImportFiles = files;

            CmpSerchFile?.Invoke(this, new EventArgs());

        }
        /// <summary>
        /// 直選択によるファイルリスト作成
        /// </summary>
        /// <param name="target"></param>
        /// <param name="IsSingle"></param>
        public void SetTarget(string target, bool IsSingle)
        {
            if (IsSingle)
            {

                GetConeNumAndFileNum(Path.GetFileNameWithoutExtension(target), out int initcon, out int iniFileNum);

                float posi = (float)Math.Round(_InfLoad.GetSlicePosi(Path.ChangeExtension(target, "inf")), 3, MidpointRounding.AwayFromZero);

                List<ImportFile> tmpimport = new List<ImportFile>()
                {
                    new ImportFile()
                    {
                        FileName = target,
                        ConeNo = initcon,
                        ScanPosi = posi,
                    }
                };

                ImportFiles = tmpimport;

            }
            else
            {
                IEnumerable<ImportFile> candidate = GetFileNames(target);

                _CalSlicePicth.DoCalSlicePitch(candidate);

                GetImportFiles(candidate, target, SlicePitch, out IEnumerable<ImportFile> files);

                ImportFiles = files;

            }

            CmpSerchFile?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// インポートするファイルを選択する
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        private void GetImportFiles(IEnumerable<ImportFile> cds, string target, float slicepitch,
            out IEnumerable<ImportFile> outimportfiles
            )
        {

            float posi = (float)Math.Round(_InfLoad.GetSlicePosi(Path.ChangeExtension(target, "inf")), 3, MidpointRounding.AwayFromZero);

            float plusposi = posi;

            float minusposi = posi;

            string currentTarget = Path.GetFileNameWithoutExtension(target);

            string pluscurrent = currentTarget;

            string minuscurrent = currentTarget;

            GetConeNumAndFileNum(pluscurrent, out int initcon, out int iniFileNum);

            int pluscurrentCone = initcon;

            int minuscurrentCone = initcon;

            //conenolist.Add(initcon);

            List<ImportFile> importfiles = new List<ImportFile>()
            {
                new ImportFile()
                {
                    FileName = currentTarget,
                    ConeNo = initcon,
                    ScanPosi = posi,
                },
            };


            while (true)
            {

                GetConeNumAndFileNum(pluscurrent, out int currentCon, out int currentFileNum);

                plusposi = (float)Math.Round(plusposi + slicepitch, 3, MidpointRounding.AwayFromZero);

                GetNextFile(cds, pluscurrent, currentCon, currentFileNum + 1, plusposi, out string nextTarget, out bool fileSucess);

                pluscurrentCone = currentCon;

                pluscurrent = nextTarget;

                if (fileSucess == false)
                {
                    pluscurrentCone = pluscurrentCone + 1;

                    IEnumerable<ImportFile> conetagets = cds.Where(q => q.ConeNo == pluscurrentCone);
                    ImportFile nexconetaget = null;
                    if (conetagets.Any())
                    {
                        nexconetaget = conetagets.Aggregate((p, q) => p.SliceNo < q.SliceNo ? p : q);

                        GetNextConeFile(cds, pluscurrentCone, nexconetaget.SliceNo, plusposi, out string nexConeTarget, out bool coneSucess);

                        pluscurrent = nexConeTarget;

                        if (coneSucess == false)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                importfiles.Add
                    (new ImportFile()
                    {
                        FileName = pluscurrent,
                        ConeNo = pluscurrentCone,
                        ScanPosi = plusposi,
                    });

            }

            while (true)
            {
                GetConeNumAndFileNum(minuscurrent, out int currentCon, out int currentFileNum);

                minusposi = (float)Math.Round(minusposi - slicepitch, 3, MidpointRounding.AwayFromZero);

                GetNextFile(cds, minuscurrent, currentCon, currentFileNum - 1, minusposi, out string nextTarget, out bool fileSucess);

                minuscurrentCone = currentCon;

                minuscurrent = nextTarget;

                if (fileSucess == false)
                {
                    minuscurrentCone = minuscurrentCone - 1;

                    IEnumerable<ImportFile> nexconetagets = cds.Where(q => q.ConeNo == minuscurrentCone);
                    ImportFile nexconetaget = null;
                    if (nexconetagets.Any())
                    {
                        nexconetaget = nexconetagets.Aggregate((p, q) => p.SliceNo < q.SliceNo ? p : q);

                        GetNextConeFile(cds, minuscurrentCone, nexconetaget.SliceNo, minusposi, out string nexConeTarget, out bool coneSucess);

                        minuscurrent = nexConeTarget;

                        if (coneSucess == false)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                importfiles.Add(new ImportFile()
                {
                    FileName = minuscurrent,
                    ConeNo = minuscurrentCone,
                    ScanPosi = minusposi,
                });

            }

            foreach (var tmp in importfiles)
            {
                if(!Path.HasExtension(tmp.FileName))
                {
                    tmp.FileName = tmp.FileName + ".img";
                }

                tmp.FileName = Path.Combine(Path.GetDirectoryName(target), tmp.FileName);
            }


            outimportfiles = importfiles;

            return;
        }
        /// <summary>
        /// 次のファイルのコーンをインポート
        /// </summary>
        /// <param name="cn"></param>
        /// <param name="fn"></param>
        /// <param name="next"></param>
        /// <param name="Success"></param>
        private void GetNextConeFile(IEnumerable<ImportFile> cds, int nextcn, int nextfn, float posi, out string next, out bool Success)
        {
            next = string.Empty;

            Success = false;

            var quernext = cds.ToList().Find(p => p.ConeNo == nextcn && p.SliceNo == nextfn);

            if (quernext != null)
            {
                if (posi == _InfLoad.GetSlicePosi(Path.ChangeExtension(quernext.FileName, "inf")))
                {
                    next = Path.GetFileNameWithoutExtension(quernext.FileName);
                    Success = true;
                }
                else
                {
                    Success = false;
                }
            }
            else
            {
                Success = false;
            }
        }
        /// <summary>
        /// 次のファイルをインポート
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cn"></param>
        /// <param name="fn"></param>
        /// <param name="next"></param>
        /// <param name="Success"></param>
        private void GetNextFile(IEnumerable<ImportFile> cds, string file, int nextcn, int nextfn, float posi, out string next, out bool Success)
        {
            next = string.Empty;

            Success = false;

            var quernext = cds.ToList().Find(p => p.ConeNo == nextcn && p.SliceNo == nextfn);

            if (quernext != null)
            {
                if (posi == _InfLoad.GetSlicePosi(Path.ChangeExtension(quernext.FileName, "inf")))
                {
                    next = Path.GetFileNameWithoutExtension(quernext.FileName);
                    Success = true;
                }
                else
                {
                    Success = false;
                }
            }
            else
            {
                Success = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tmpname"></param>
        /// <param name="conenum"></param>
        /// <param name="filenum"></param>
        private void GetConeNumAndFileNum(string tmpname, out int conenum, out int filenum)
        {
            var tmpnames = tmpname.Split('-');
            conenum = 0;
            filenum = 0;

            if (tmpnames.Length > 1)
            {
                //末尾の連番を取り除く
                if (Regex.IsMatch(tmpnames[(tmpnames.Length - 1)], @"-[0-9]{3}$") && Regex.IsMatch(tmpnames[(tmpnames.Length - 3)], @"-[0-9]{3}$"))             //３桁の場合
                {
                    filenum = int.Parse(tmpnames[(tmpnames.Length - 1)]);
                    conenum = int.Parse(tmpnames[(tmpnames.Length - 2)]);
                }
                else if (Regex.IsMatch(tmpnames[(tmpnames.Length - 1)], "[0-9]{4}$") && Regex.IsMatch(tmpnames[(tmpnames.Length - 2)], "[0-9]{3}$"))        //４桁の場合
                {
                    filenum = int.Parse(tmpnames[(tmpnames.Length - 1)]);
                    conenum = int.Parse(tmpnames[(tmpnames.Length - 2)]);
                }
                else
                {
                    filenum = int.Parse(tmpnames[(tmpnames.Length - 1)]);
                    conenum = 0;
                }
            }
        }

        /// <summary>
        /// インポートするファイルの候補を列挙するメソッド
        /// </summary>
        /// <param name="target"></param>
        private IEnumerable<ImportFile> GetFileNames(string targetname)
        {
            List<ImportFile> candidatefile = new List<ImportFile>();

            var filename = targetname;
            var dir = Path.GetDirectoryName(filename);
            filename = Path.GetFileNameWithoutExtension(filename);

            //ズーミング番号（-Z001）があれば取り除く
            if (Regex.IsMatch(filename, @"-[zZ]001$"))
            {
                filename = filename.Remove(filename.Length - 5);
            }

            //末尾の連番を取り除く
            if (Regex.IsMatch(filename, @"-[0-9]{3}$"))             //３桁の場合
            {
                var renbanMicro = filename.Substring(filename.Length - 4, 4);
                var renbanIndust = filename.Substring(filename.Length - 3, 3);
                if (uint.TryParse(renbanMicro, out uint resminum))
                {
                    filename = filename.Remove(filename.Length - 8);
                }
                else if (uint.TryParse(renbanIndust, out uint resinnum))
                {
                    filename = filename.Remove(filename.Length - 4);
                }
            }
            else if (Regex.IsMatch(filename, @"-[0-9]{4}$"))        //４桁の場合
            {
                filename = filename.Remove(filename.Length - 8);
            }
            //CTファイル検索
            var query = Directory.EnumerateFiles(
                            dir, // 検索開始ディレクトリ
                            $"{filename}*.img",
                            SearchOption.TopDirectoryOnly);

            if (query != null)
            {
                foreach (var name in query)
                {
                    if (File.Exists(Path.ChangeExtension(name, "inf")))
                    {
                        var tmpname = Path.GetFileNameWithoutExtension(name);
                        if (Regex.IsMatch(tmpname, @"-[zZ]001$"))//ズーミング番号（-Z001）があれば取り除く
                        {
                            tmpname = filename.Remove(tmpname.Length - 5);
                        }

                        var tmpnames = tmpname.Split('-');

                        if (tmpnames.Length > 1)
                        {
                            int fn = 0;
                            int cn = 0;

                            //末尾の連番を取り除く
                            if (Regex.IsMatch(tmpnames[(tmpnames.Length - 1)], @"-[0-9]{3}$") && Regex.IsMatch(tmpnames[(tmpnames.Length - 3)], @"-[0-9]{3}$"))             //３桁の場合
                            {
                                fn = int.Parse(tmpnames[(tmpnames.Length - 1)]);
                                cn = int.Parse(tmpnames[(tmpnames.Length - 2)]);
                            }
                            else if (Regex.IsMatch(tmpnames[(tmpnames.Length - 1)], "[0-9]{4}$") && Regex.IsMatch(tmpnames[(tmpnames.Length - 2)], "[0-9]{3}$"))        //４桁の場合
                            {
                                fn = int.Parse(tmpnames[(tmpnames.Length - 1)]);
                                cn = int.Parse(tmpnames[(tmpnames.Length - 2)]);
                            }
                            else
                            {
                                fn = int.Parse(tmpnames[(tmpnames.Length - 1)]);
                                cn = 0;
                            }

                            ImportFile imf = new ImportFile()
                            {
                                FileName = Path.Combine(dir, name),
                                ConeNo = cn,
                                SliceNo = fn
                            };
                            candidatefile.Add(imf);
                        }
                        //else if(tmpnames.Length == 2)
                        //{
                        //    int fn = 0;
                        //    int cn = 0;

                        //    //末尾の連番を取り除く
                        //    if (Regex.IsMatch(tmpnames[(tmpnames.Length - 1)], @"-[0-9]{3}$") && Regex.IsMatch(tmpnames[(tmpnames.Length - 3)], @"-[0-9]{3}$"))             //３桁の場合
                        //    {
                        //        fn = int.Parse(tmpnames[(tmpnames.Length - 1)]);
                        //        cn = int.Parse(tmpnames[(tmpnames.Length - 2)]);
                        //    }
                        //    else if (Regex.IsMatch(tmpnames[(tmpnames.Length - 1)], "[0-9]{4}$") && Regex.IsMatch(tmpnames[(tmpnames.Length - 2)], "[0-9]{3}$"))        //４桁の場合
                        //    {
                        //        fn = int.Parse(tmpnames[(tmpnames.Length - 1)]);
                        //        cn = int.Parse(tmpnames[(tmpnames.Length - 2)]);
                        //    }
                        //    else
                        //    {
                        //        fn = int.Parse(tmpnames[(tmpnames.Length - 1)]);
                        //        cn = 0;
                        //    }

                        //    ImportFile imf = new ImportFile()
                        //    {
                        //        FileName = Path.Combine(dir, name),
                        //        ConeNo = 0,
                        //        SliceNo = fn
                        //    };
                        //    candidatefile.Add(imf);
                        //}
                    }
                }
            }
            return candidatefile;
        }
    }
    /// <summary>
    /// フォルダ参照I/F
    /// </summary>
    public interface IFileSelectService
    {
        /// <summary>
        /// ファイル検索完了
        /// </summary>
        event EventHandler CmpSerchFile;
        /// <summary>
        /// フォルダ選択時のターゲットファイルS
        /// </summary>
        void SetTargets(IEnumerable<string> targets);
        /// <summary>
        /// ファイル選択時のターゲットファイル
        /// </summary>
        void SetTarget(string target, bool IsSingle = false);
    }
}
