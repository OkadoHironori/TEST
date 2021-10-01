using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace CTAddress
{
    /// <summary>
    /// 三次元画像の計算クラス
    /// </summary>
    public class VolumeConverter : IVolumeConverter
    {
        /// <summary>
        /// ボリューム情報更新
        /// </summary>
        public event EventHandler UpdateVolumeParam;
        /// <summary>
        /// 変換係数
        /// </summary>
        public AffineParam AffineParam { get; private set; }
        /// <summary>
        /// MPR縦方向サイズ
        /// </summary>
        public int DispStackSize { get; private set; }
        /// <summary>
        /// 表示マトリックスサイズ
        /// </summary>
        public int DispMatrix { get; private set; }
        /// <summary>
        /// 表示範囲
        /// </summary>
        public float DispArea { get; private set; }
        /// <summary>
        /// 画像の8隅の座標
        /// </summary>
        public IEnumerable<Point3D> Point3s;
        /// <summary>
        /// 画像インポートサービス
        /// </summary>
        private readonly IDoImportService _DoImportService;
        /// <summary>
        /// 三次元画像の計算クラス
        /// </summary>
        /// <param name="service"></param>
        public VolumeConverter(IDoImportService service)
        {
            _DoImportService = service;
            _DoImportService.SetDoImportParam += (s, e) => 
            {
                if(s is DoImportService dis)
                {
                    DispMatrix = dis.DispMatrix;
                    DispStackSize = dis.DispStackSize;
                    DispArea = dis.ImportScanArea;

                    AffineParam = new AffineParam()
                    {
                        Ctdotsize = (double)dis.ImportDotSize,
                        Ctpitchsize = Math.Round(dis.SlicePitch,3,MidpointRounding.AwayFromZero),
                        Manualdotsize = 0,
                        ImagestockNumOrg = dis.ImportFiles.Count(),
                        ImagestockNumCmp = dis.DispStackSize,
                        ImagestockNumTr = dis.DispStackSize,
                        MatrixCmpOrg = dis.Matrix,
                        MatrixCmpTr = dis.DispMatrix,
                        Transfer1 = 0,
                        Transfer2 = 0,
                        TransferNext = 0,
                        TransferX = 0,
                        TransferY = 0,
                        TransferZ = 0,
                    };

                    var mat = dis.DispMatrix;
                    var stc = dis.ImportFiles.Count();
                    List<Point3D> pts = new List<Point3D>
                    { 
                        new Point3D(mat, mat,   0d),
                        new Point3D(0d,  0d,    0d),
                        new Point3D(0d,  mat,   0d),
                        new Point3D(mat, 0d,    0d),
                        new Point3D(mat, mat,   stc),
                        new Point3D(0,   0,     stc),
                        new Point3D(0,   mat,   stc),
                        new Point3D(mat, mat,   stc),
                    };
                    Point3s = pts;

                    UpdateVolumeParam?.Invoke(this, new EventArgs());
                }
            };
        }
        /// <summary>
        /// 断面変換後の中心位置の計算
        /// </summary>
        /// <param name="cbase">断面変換情報(ベース)</param>
        /// <param name="rotangle">回転角度（XY平面及びオブリーク1の回転角度）</param>
        private Point3D GetPointAfterAffine(Point3D POI, double tr1, double tr2, double tr3, Point3D Ido = new Point3D())
        {
            ////アフィン変換に必要なパラメータを構造体に入れる
            //var inf = new DoProLibWrap.ConvertBase()
            //{
            //    ctdotsize = ConvertInf.ctdotsize,
            //    ctpitchsize = ConvertInf.ctpitchsize,
            //    manualdotsize = 0,
            //    imagestockNumOrg = ConvertInf.imagestockNumOrg,
            //    imagestockNumTr = ConvertInf.imagestockNumTr,
            //    matrixCmpOrg = ConvertInf.matrixCmpOrg,
            //    matrixCmpTr = ConvertInf.matrixCmpTr,
            //    transfer1 = tr1,
            //    transfer2 = tr2,
            //    transferNext = tr3,
            //    transferX = Ido.X,
            //    transferY = Ido.Y,
            //    transferZ = Ido.Z,
            //};

            int Di = 4;
            double[,] iMat = new double[Di, Di];

            //try
            //{
            //    bool result = false;
            //    result = DoProLibWrap.GetMatrix(inf, iMat);
            //}
            //catch
            //{
            //    throw new Exception("ERROR_GET_INVERSE_MAT");
            //}

            double[] Xout = new double[Di];
            double[] Xr = new double[Di];

            Xr[0] = POI.X;
            Xr[1] = POI.Y;
            Xr[2] = POI.Z;
            Xr[3] = 1;

            Xout[0] = iMat[0, 0] * Xr[0] + iMat[0, 1] * Xr[1] + iMat[0, 2] * Xr[2] + iMat[0, 3] * Xr[3];
            Xout[1] = iMat[1, 0] * Xr[0] + iMat[1, 1] * Xr[1] + iMat[1, 2] * Xr[2] + iMat[1, 3] * Xr[3];
            Xout[2] = iMat[2, 0] * Xr[0] + iMat[2, 1] * Xr[1] + iMat[2, 2] * Xr[2] + iMat[2, 3] * Xr[3];
            Xout[3] = iMat[3, 0] * Xr[0] + iMat[3, 1] * Xr[1] + iMat[3, 2] * Xr[2] + iMat[3, 3] * Xr[3];

            double rx = Xout[0];
            double ry = Xout[1];
            double rz = Xout[2];

            return new Point3D(rx, ry, rz);
        }
        /// <summary>
        /// 角度変換
        /// </summary>
        /// <param name="tr1"></param>
        /// <param name="tr2"></param>
        /// <param name="tr3"></param>
        public void DoConvertTransform(double tr1, double tr2, double tr3, Point3D Ido)
        {
            //List<Point3D> matlist = new List<Point3D>();
            //foreach(var pt in Point3s)
            //{
            //    matlist.Add(GetPointAfterAffine(pt, tr1, tr2, tr3, Ido));
            //}
            //double xmax = matlist.Max(p => p.X);
            //double xmin = matlist.Min(p => p.X);
            //double width = xmax - xmin;

            //double ymax = matlist.Max(p => p.Y);
            //double ymin = matlist.Min(p => p.Y);
            //double height = ymax - ymin;

            //double zmax = matlist.Max(p => p.Z);
            //double zmin = matlist.Min(p => p.Z);
            //double stack = zmax - zmin;

            //DispStackSize = (int)Math.Ceiling(stack);

            //DispMatrix = (int)Math.Ceiling(Math.Max(width, height));

            //var conv = new DoProLibWrap.ConvertBase()
            //{
            //    ctdotsize = ConvertInf.ctdotsize,
            //    ctpitchsize = ConvertInf.ctpitchsize,
            //    manualdotsize = 0,
            //    imagestockNumOrg = ConvertInf.imagestockNumOrg,
            //    imagestockNumTr = DispStackSize,
            //    matrixCmpOrg = ConvertInf.matrixCmpOrg,
            //    matrixCmpTr = DispMatrix,
            //    transfer1 = tr1,
            //    transfer2 = tr2,
            //    transferNext = tr3,
            //    transferX = Ido.X,
            //    transferY = Ido.Y,
            //    transferZ = Ido.Z,
            //};

            //ConvertInf = conv;

            UpdateVolumeParam?.Invoke(this, new EventArgs());
        }
    }

    /// <summary>
    /// 三次元画像の計算クラス I/F
    /// </summary>
    public interface IVolumeConverter
    {
        /// <summary>
        /// ボリューム情報更新
        /// </summary>
        event EventHandler UpdateVolumeParam;
        /// <summary>
        /// 角度変換
        /// </summary>
        /// <param name="tr1">回転1_XY角</param>
        /// <param name="tr2">回転2_なす角</param>
        /// <param name="tr3">回転3_XY角</param>
        /// <param name="Ido">XYZ移動</param>
        void DoConvertTransform(double tr1, double tr2, double tr3, Point3D Ido);
    }

    public class AffineParam
    {
        /// <summary>
        /// XYの角度α
        /// </summary>
        public double Transfer1 { get; set; }
        /// <summary>
        /// ZX,ZYのなす角θ
        /// </summary>
        public double Transfer2 { get; set; }
        /// <summary>
        /// 二回目のXYの角度
        /// </summary>
        public double TransferNext { get; set; }
        /// <summary>
        /// 中心軸への移動方向 X軸
        /// </summary>
        public double TransferX { get; set; }
        /// <summary>
        /// 中心軸への移動方向 Y軸
        /// </summary>
        public double TransferY { get; set; }
        /// <summary>
        /// 中心軸への移動方向 Z軸
        /// </summary>
        public double TransferZ { get; set; }
        /// <summary>
        /// マトリックスサイズ(org)
        /// </summary>
        public int MatrixCmpOrg { get; set; }
        /// <summary>
        /// 画像枚数(org)_dummy方式なので変換後の枚数にした
        /// </summary>
        public int ImagestockNumOrg { get; set; }
        /// <summary>
        /// 画像圧縮枚数処理
        /// </summary>
        public int ImagestockNumCmp { get; set; }
        /// <summary>
        /// マトリックスサイズ(変換後)
        /// </summary>
        public int MatrixCmpTr { get; set; }
        /// <summary>
        /// 画像枚数(変換後)
        /// </summary>
        public int ImagestockNumTr { get; set; }
        /// <summary>
        /// 1画素サイズ
        /// </summary>
        public double Ctdotsize { get; set; }
        /// <summary>
        /// ピッチサイズ
        /// </summary>
        public double Ctpitchsize { get; set; }
        /// <summary>
        ///画素変更の場合
        /// </summary>
        public double Manualdotsize { get; set; }
    }
}
