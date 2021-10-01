using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

namespace CT30K
{
    public class CallImageProFunction
    {
        private const string DLL_PATH = @"ImageProHelper.dll";

        [DllImport(DLL_PATH, EntryPoint = "CallIpStart")]
        public static extern int CallIpStart();

        [DllImport(DLL_PATH, EntryPoint = "CallIpEnd")]
        public static extern int CallIpEnd();

        [DllImport(DLL_PATH, EntryPoint = "CallCreateShareMem")]
        public static extern int CallCreateShareMem();


        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallDrawHorizon")]
        public static extern int CallDrawHorizon(ushort[] ImageBuff,
                               int height,
                               int width,
                               float phm,
                               float pvm,
                               int RoiLeft,
                               int RoiTop,
                               int RoiRight,
                               int RoiBottom,
                               int HorizonX1,
                               int HorizonY1,
                               int HorizonX2,
                               int HorizonY2,
                               int LRInverse,
                               String SaveFormat,
                               String FileName,
                               int HorizonY1UCone,//Rev20.00 引数追加 by長野 2015/02/06
                               int HorizonY2UCone,//Rev20.00 引数追加 by長野 2015/02/06
                               int HorizonY1LCone,//Rev20.00 引数追加 by長野 2015/02/06
                               int HorizonY2LCone //Rev20.00 引数追加 by長野 2015/02/06
                               );

        //Rev21.00 追加 by長野 2015/02/24
        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallScanoToTiff")]
        public static extern int CallScanoToTiff(
                               int height,
                               int width,
                               String FileName,
                               String SaveFormat,
                               String SaveFileName,
                               int LRInverse
                               );

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallFilter")]
        public static extern int CallFilter(short[] ImageBuff, short[] ImageResultBuff, int height, int width, int filtertype, int size, int stlength, int num, String kernelname);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallFilter")]
        public static extern int CallFilter(ushort[] ImageBuff, ushort[] ImageResultBuff, int height, int width, int filtertype, int size, int stlength, int num, String kernelname);
        
        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallFormatConvertStep1")]
        public static  extern int CallFormatConvertStep1(int height,int width,int iMode);
        
        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallFormatConvertStep2")]
        public static  extern int CallFormatConvertStep2();
        
        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallFormatConvertStep3")]
        public static  extern int CallFormatConvertStep3(int y,int x1,int x2);
        
        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallFormatConvertStep4")]
        public static  extern int CallFormatConvertStep4(int x1,int y1,int x2,int y2);
        
        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallFormatConvertStep5")]
        public static  extern int CallFormatConvertStep5(int x ,int y ,String scalesize ,String  scaleunit ,int fontsize ,String  fontname);
        
        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallFormatConvertStep6")]
        public static  extern int CallFormatConvertStep6(int formatIndex,String  savefilename,String  extension);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallFImageTrans")]
        public static  extern int CallFImageTrans(ushort[] ImageBuff,int height,int width,int modX,int modY,int imgHeight,int imgWidth,int LowLevel,int HiLevel,float PixelsPerUnit);
        
        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallDrawText")]
        public static  extern int CallDrawText(int x ,int y ,String  workStr ,int fontSize ,String  fontname);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallGetWordImage")]
        public static  extern int CallGetWordImage(ushort[] ImageResultBuff, int height,int width,int offsetX,int offsetY);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallGetByteImage")]
        public static  extern int CallGetByteImage(byte[] ImageResultBuff, int height,int width,int offsetX,int offsetY);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallDrawByteImage")]
        public static extern int CallDrawByteImage(byte[] ImageBuff, int height, int width, int offsetX, int offsetY, int IsWsCreate);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallDrawWordImage")]
        public static  extern int CallDrawWordImage(short[] ImageBuff, int height,int width,int offsetX,int offsetY,int IsWsCreate);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallDrawWordImage")]
        public static extern int CallDrawWordImage(ushort[] ImageBuff, int height, int width, int offsetX, int offsetY, int IsWsCreate);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallIpAppCloseAll")]
        public static  extern int CallIpAppCloseAll();

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallTrimImage")]
        public static  extern int CallTrimImage(int height,int width,int modX,int modY,int iMode);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallIpWsScale")]
        public static extern int CallIpWsScale(int height, int width);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallIpWsCreate")]
        public static  extern int  CallIpWsCreate(int height,int width);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallIpWsConvertImage")]
        public static  extern int  CallIpWsConvertImage(int iMode);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallIpWsSaveAs")]
        public static  extern int  CallIpWsSaveAs(String  SaveFileName ,String  Extension);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallIpDocClose")]
        public static  extern int  CallIpDocClose();

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallLoadImageStep1")]
        public static  extern int  CallLoadImageStep1(String  FileName ,String  Extension,float[] result);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallLoadImageStep3")]
        public static  extern int  CallLoadImageStep3(int height,int width,int modX,int modY,int imgHeight,int imgWidth);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallDrawScaleBar")]
        public static  extern int  CallDrawScaleBar(int x1,int y1,int x2,int y2);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallUpdateScaleBar")]
        public static  extern int  CallUpdateScaleBar(byte[] ImageBuff,int height,int width);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallGetColumnDiameter")]
        public static  extern int  CallGetColumnDiameter(ushort[] ImageBuff ,int height ,int width);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallGetHistgramParam")]
        public static  extern int  CallGetHistgramParam(ushort[] ImageBuff ,float[] result ,int height ,int width,int RoiLeft,int RoiTop,int RoiRight,int RoiBottom);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallDoDistanceCorrectStep1")]
        public static  extern int  CallDoDistanceCorrectStep1(ushort[] ImageBuff ,int height ,int width);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallDoDistanceCorrectStep2")]
        public static  extern int  CallDoDistanceCorrectStep2(ushort[] ImageResultBuff ,int height ,int width);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallPutOffsetImage")]
        public static  extern int  CallPutOffsetImage(double[] ImageBuff ,int height ,int width);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallRotationCenterParameterStep1")]
        //public static extern int CallRotationCenterParameterStep1(ushort[] ImageBuff, int height, int width, int RoiLeft, int RoiTop, int RoiRight, int RoiBottom);
        public static extern int CallRotationCenterParameterStep1(ushort[] ImageBuff, ushort[] ImageResultBuff, int height, int width, int RoiLeft, int RoiTop, int RoiRight, int RoiBottom);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallRotationCenterParameterStep2")]
        public static  extern int  CallRotationCenterParameterStep2(ushort[] ImageBuff ,ushort[] ImageResultBuff ,int height ,int width ,int UseFlatPanel,int FlgShadingRot);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallRotationCenterParameterStep3")]
        public static  extern int  CallRotationCenterParameterStep3(ushort[] ImageBuff ,int height ,int width);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallRotationCenterParameterStep4")]
        public static  extern int  CallRotationCenterParameterStep4(int LoLevel ,int HiLevel,int WhiteOnBlack);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallRotationCenterParameterStep5")]
        public static  extern int  CallRotationCenterParameterStep5(ushort[] ImageResultBuff ,int height ,int width);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallRotationCenterWire")]
        public static  extern int  CallRotationCenterWire();

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallVerticalParameterStep1")]
        public static  extern int  CallVerticalParameterStep1(ushort[] ImageBuff ,ushort[] ImageResultBuff ,int height ,int width);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallVerticalParameterStep2")]
        public static  extern int  CallVerticalParameterStep2(ushort[] ImageBuff ,int height ,int width);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallVerticalParameterStep3")]
        public static  extern int  CallVerticalParameterStep3(ushort[] ImageResultBuff,int height,int width,int RoiLeft,int RoiTop,int RoiRight,int RoiBottom,int Inverse,int Shading);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallVerticalParameterStep4")]
        public static  extern int  CallVerticalParameterStep4(ushort[] ImageResultBuff ,int height, int width,int LoLevel ,int HiLevel,int WhiteOnBlack);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallVerticalParameterGetHoles")]
        //変更2014/10/13hata
        //public static extern int CallVerticalParameterGetHoles(ushort[] ImageBuff, int height, int width, ref int kmax, int vm, int hm);  
        public static extern int CallVerticalParameterGetHoles(ushort[] ImageBuff, int height, int width, ref int kmax, float vm, float hm);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallVerticalParameterStep5")]
        //public static extern int CallVerticalParameterStep5(ushort[] ImageBuff, float[] resultX, float[] resultY, float[] resultArea, int height, int width, ref int kmax, int vm, int hm);
        public static extern int CallVerticalParameterStep5(float[] resultX, float[] resultY, float[] resultArea, int kmax);

        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallVerticalParameterStep6")]
        public static  extern int  CallVerticalParameterStep6(ushort[] ImageBuff,int height,int width);

        //追加2014/10/07hata_v19.51反映
        [DllImport(DLL_PATH, CharSet = CharSet.Ansi, EntryPoint = "CallOverlayImage")]
        public static extern int CallOverlayImage(String FilePath0, String FilePath1, String FileName, int pixel);


    }
}
