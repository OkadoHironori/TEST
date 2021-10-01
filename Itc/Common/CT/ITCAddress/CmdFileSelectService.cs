using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CTAddress
{
    /// <summary>
    /// ファイル選択サービス実装用
    /// </summary>
    public class CmdFileSelectService : ICmdFileSelectService
    {
        /// <summary>
        /// ファイル検索中に失敗
        /// </summary>
        public event EventHandler ErrorArg;
        /// <summary>
        /// ファイル検索の完了
        /// </summary>
        public event EventHandler EndSerchingFile;
        /// <summary>
        /// インポートするファイル
        /// </summary>
        public IEnumerable<string> CTFiles { get; private set; }
        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string ErrorMes { get; private set; }
        /// <summary>
        /// ターゲットファイルサービス
        /// </summary>
        private readonly ILoadTargetFileService _Target;
        /// <summary>
        /// ファイル選択サービス
        /// </summary>
        private readonly IFileSelectService _FileSelectServie;
        /// <summary>
        /// ファイル選択サービス実装用
        /// </summary>
        /// <param name="target"></param>
        public CmdFileSelectService(ILoadTargetFileService taget, IFileSelectService select)
        {
            _Target = taget;

            _FileSelectServie = select;

            _FileSelectServie.CmpSerchFile += (s, e) =>
            {
                if (s is FileSelectService fss)
                {
                    CTFiles = fss.ImportFiles.ToList().Select(p => p.FileName);
                    EndSerchingFile?.Invoke(this, e);
                }
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filefullpath"></param>
        /// <param name="IsSingle"></param>
        public void SetFile(string filefullpath, bool IsSingle)
        {
            string inffile = Path.ChangeExtension(filefullpath, "inf");

            if (File.Exists(inffile))
            {
                _Target.DoInfLoad(inffile);
                _FileSelectServie.SetTarget(inffile, IsSingle);
            }
            else
            {
                ErrorMes = $"{inffile} file is not exist";
                ErrorArg?.Invoke(this, new EventArgs());
            }
        }
    }
    /// <summary>
    /// DO用のファイル選択サービス I/F
    /// </summary>
    public interface ICmdFileSelectService
    {
        /// <summary>
        /// ファイル検索中に失敗
        /// </summary>
        event EventHandler ErrorArg;
        /// <summary>
        /// ファイル検索の完了
        /// </summary>
        event EventHandler EndSerchingFile;
        /// <summary>
        /// ファイル設定
        /// </summary>
        /// <param name="path"></param>
        void SetFile(string filefullpath, bool IsSingle);
    }
}

