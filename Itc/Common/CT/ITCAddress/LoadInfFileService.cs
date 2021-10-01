using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTAddress
{
    /// <summary>
    /// 付帯情報読込クラス
    /// </summary>
    public class LoadInfFileService : ILoadInfFileService
    {
        /// <summary>
        /// システム名
        /// </summary>
        public string SystemName { get; private set; }
        /// <summary>
        /// マイクロCTの付帯情報サービスI/F
        /// </summary>
        private readonly IMicroCTInfService _MicroCTInf;
        /// <summary>
        /// 産業用CTの付帯情報サービスI/F
        /// </summary>
        private readonly IIndustrialCTInfService _IndustrialCTInf;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="service"></param>
        public LoadInfFileService(IMicroCTInfService mct, IIndustrialCTInfService indct)
        {
            _MicroCTInf = mct;
            _MicroCTInf.EndLoadInf += (s, e) => 
            {
                if (s is MicroCTInfService mctis)
                {
                    SystemName = mctis.SystemName;
                }
            };

            _IndustrialCTInf = indct;
            _IndustrialCTInf.EndLoadInf += (s, e) =>
            {
                if (s is IndustrialCTInfService ictis)
                {
                    SystemName = ictis.SystemName;
                }
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public float GetSlicePosi(string path)
        {
            var bytedata = File.ReadAllBytes(path);
            switch (SystemName)
            {
                case ("TOSCANER-30000"):
                    return _MicroCTInf.GetSlicePosi(bytedata);
                case ("TOSCANER-20000"):
                    return _IndustrialCTInf.GetSlicePosi(bytedata);

                default:
                    throw new Exception("Unregist File Name");

            }
        }
    }
    /// <summary>
    /// 付帯情報読込クラス
    /// </summary>
    public interface ILoadInfFileService
    {
        float GetSlicePosi(string path);
    }

}
