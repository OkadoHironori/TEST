
//using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CTAddress;

namespace CTDataImport
{
    /// <summary>
    /// フォルダ選択サービス
    /// </summary>
    public class CmdFolderSelectService: ICmdFolderSelectService
    {
        /// <summary>
        /// インポート用ファイル
        /// </summary>
        public IEnumerable<ImportFile> ImportFiles { get; private set; }
        /// <summary>
        /// スケール（Chara型のスキャンエリア）
        /// </summary>
        public string Scale { get; private set; }
        /// <summary>
        /// マトリックス
        /// </summary>
        public string Matrix { get; private set; }
        /// <summary>
        /// スライスピッチ mm
        /// </summary>
        public float SlicePitch { get; private set; }
        /// <summary>
        /// テーブル位置 mm
        /// </summary>
        public float TblPosi { get; private set; }
        /// <summary>
        /// ディレクトリ読み完了イベント
        /// </summary>
        public event EventHandler EndSetDir;
        /// <summary>
        /// ファイル選択サービス
        /// </summary>
        private readonly IFileSelectService _FileSelectServie;
        /// <summary>
        /// ターゲット読込サービス
        /// </summary>
        private readonly ILoadTargetFileService _Target;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ctinf"></param>
        public CmdFolderSelectService(IServiceProvider service)
        {
            _Target = service.GetService<ILoadTargetFileService>();

            _FileSelectServie = service.GetService<IFileSelectService>();

            _FileSelectServie.CmpSerchFile += (s, e) =>
            {
                if(s is FileSelectService fss)
                {
                    ImportFiles = fss.ImportFiles;
                    Matrix = fss.Matrix;
                    Scale = fss.Scale;
                    SlicePitch = fss.SlicePitch;
                    TblPosi = fss.TblPosi;
                }
            };
        }
        /// <summary>
        /// ファイルのセット
        /// </summary>
        /// <param name="files"></param>
        public void SetDir(string path)
        {
            //CTファイルはあるか？
            IEnumerable<string> queryimg = Directory.EnumerateFiles(
                            path,
                            "*-0001.img",
                            SearchOption.TopDirectoryOnly);

            if (queryimg.Count() > 0)
            {
                var infs = queryimg.
                    Where(p => File.Exists(Path.ChangeExtension(p, "inf"))).
                    Select(q => Path.ChangeExtension(q, "inf"));

                _Target.DoInfLoad(infs.FirstOrDefault());
                _FileSelectServie.SetTargets(infs);
            }
            else
            {
                throw new Exception($"*-0001.img file isn't exist.");
            }

            EndSetDir?.Invoke(this, new EventArgs());
        }
    }
    /// <summary>
    /// フォルダ選択サービス
    /// </summary>
    public interface ICmdFolderSelectService
    {
        /// <summary>
        /// ディレクトリ設定
        /// </summary>
        /// <param name="path"></param>
        void SetDir(string path);
        /// <summary>
        /// ディレクトリ読み完了イベント
        /// </summary>
        event EventHandler EndSetDir;
    }
}
