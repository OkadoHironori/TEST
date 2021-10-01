using System.Runtime.InteropServices;

namespace CTAPI
{
    public class CTLib
    {
        //-----------------------------------------------------------------------------
        // API関数
        // その他のCTライブラリ
        //-----------------------------------------------------------------------------


        //-----------------------------------------------------------------------------
        // < Conereconlib.dll >
        //-----------------------------------------------------------------------------
        /// <summary>
        /// 2次元歪補正ﾊﾟﾗﾒｰﾀ設定
        /// </summary>
        [DllImport("Conereconlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //v17.00 引数detector追加 byやまおか 2010/02/26 'v19.50 v19.41とv18.02の統合 by長野 2013/11/12
        //public static extern void cone_setup_iicrct(
        //    ref float ic, 
        //    ref float jc, 
        //    ref float theta_s, 
        //    ref float dpm, 
        //    ref int nstart, 
        //    ref int nend, 
        //    ref int mstart, 
        //    ref int mend, 
        //    ref float delta_theta, 
        //    ref float n0,
        //    ref float m0, 
        //    ref float delta_im, 
        //    ref float delta_jm, 
        //    ref float ir0, 
        //    ref float jr0, 
        //    ref float kix, 
        //    ref float kjx, 
        //    float b, 
        //    float scan_posi_a, 
        //    float scan_posi_b,
        //    int h_size, 
        //    int v_size, 
        //    float FDD, 
        //    int nd, 
        //    int md, 
        //    int mc, 
        //    ref float hizumi, 
        //    ref int js, 
        //    ref int je, 
        //    ref float delta_ix,
        //    ref float delta_jx, 
        //    float hm, 
        //    float vm, 
        //    int pure_flag, 
        //    int full_distortion, 
        //    int ist, 
        //    int ied, 
        //    int detector
        //);    
        public static extern void cone_setup_iicrct(
             ref float ic,
             ref float jc,
             ref float theta_s,
             ref float dpm,
             ref int nstart,
             ref int nend,
             ref int mstart,
             ref int mend,
             ref float delta_theta,
             ref float n0,
             ref float m0,
             ref float delta_im,
             ref float delta_jm,
             ref float ir0,
             ref float jr0,
             ref float kix,
             ref float kjx,
             float b,
             float scan_posi_a,
             float scan_posi_b,
             int h_size,
             int v_size,
             float FDD,
             int nd,
             int md,
             int mc,
             ref float hizumi,
             ref int js,
             ref int je,
             ref float delta_ix,
             ref float delta_jx,
             float hm,
             float vm,
             int pure_flag,
             int full_distortion,
             int ist,
             int ied,
             int detector,
             int scan_mode
        );


        //-----------------------------------------------------------------------------
        // < RevTools.dll >
        //-----------------------------------------------------------------------------
        /// <summary>
        /// 汎用フォーマットによる保存
        /// </summary>
        /// <param name="InImgName"></param>
        /// <param name="OutImgName"></param>
        [DllImport("RevTools.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int ReversByte(
            string InImgName, 
            string OutImgName
        );


        //-----------------------------------------------------------------------------
        // < bsmtp.dll >
        //-----------------------------------------------------------------------------
        /// <summary>
        /// メール送信
        /// </summary>
        [DllImport("bsmtp.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern string SendMail(
            ref string szServer, 
            ref string szTo, 
            ref string szFrom, 
            ref string szSubject, 
            ref string szBody, 
            ref string szFile
        );


        //-----------------------------------------------------------------------------
        // < Reconlib.dll >
        //-----------------------------------------------------------------------------
        /// <summary>
        /// ？
        /// </summary>
        [DllImport("Reconlib.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int sig_chk();


        //-----------------------------------------------------------------------------
        // < DICOMTransfer.dll >
        //-----------------------------------------------------------------------------
        //DICOM変換関数                                                  'V6.0 append by 間々田 2002/08/07
        [DllImport("DICOMTransfer.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int DICOM_Transfer(
            string LoadFileName, 
            string SaveFileDir, 
            string PatientName, 
            string InstitutionName, 
            string PatientComments
        );


        //-----------------------------------------------------------------------------
        // < ScanCorrect.basにあったもの >
        //-----------------------------------------------------------------------------
        //コントラスト向上（画像データを符号なしで受け取る）
        [DllImport("ProfocxTool.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void ChangeFullRange(
            ref ushort IN_BUFF, 
            int hSize, 
            int vSize, 
            int Min, 
            int Max
        );

        //左右切り落とし画像部分取り出し
        //'Public Declare Sub Pic_Imgside Lib "Videocap.dll"    '変更 by 中島 '99-8-17
        [DllImport("ProfocxTool.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern void Pic_Imgside(
            ref short IN_BUFF, 
            int FrmSiz, 
            int hSize, 
            int vSize
        );


    }
}