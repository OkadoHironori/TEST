using System.Runtime.InteropServices;

namespace SeqComm
{
    internal static class MechaControl
    {
    


        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //変更2014/11/28hata_v19.51_dnet
        //public static extern void SpeedWriteTP(int Device, int ManulSpeed);
        public static extern void SpeedWriteTP(int Device, short ManulSpeed);
        
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //変更2014/11/28hata_v19.51_dnet
        //public static extern int UdIndexTP(int Absolute, float Position, int Brake);
        public static extern int UdIndexTP(int Absolute, float Position, short Brake);
        
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //変更2014/11/28hata_v19.51_dnet
        //public static extern int RotIndexTP(int Absolute, float Position, int Brake);
        public static extern int RotIndexTP(int Absolute, float Position, short Brake);
        
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //変更2014/11/28hata_v19.51_dnet
        //public static extern int XStgIndexTP(int Absolute, float Position, int Brake);
        public static extern int XStgIndexTP(int Absolute, float Position, short Brake);
        
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //変更2014/11/28hata_v19.51_dnet
        //public static extern int YStgIndexTP(int Absolute, float Position, int Brake);
        public static extern int YStgIndexTP(int Absolute, float Position, short Brake);
        
        //追加 by 稲葉 05-01-08
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //変更2014/11/28hata_v19.51_dnet
        //public static extern void UdIndexTPexe(int Absolute, float Position, int Brake);
        public static extern void UdIndexTPexe(int Absolute, float Position, short Brake);
        
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //変更2014/11/28hata_v19.51_dnet
        //public static extern void RotIndexTPexe(int Absolute, float Position, int Brake);
        public static extern void RotIndexTPexe(int Absolute, float Position, short Brake);
        
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //変更2014/11/28hata_v19.51_dnet
        //public static extern void XStgIndexTPexe(int Absolute, float Position, int Brake);
        public static extern void XStgIndexTPexe(int Absolute, float Position, short Brake);
        
        [DllImport("mechacontrol.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //変更2014/11/28hata_v19.51_dnet
        //public static extern void YStgIndexTPexe(int Absolute, float Position, int Brake);
        public static extern void YStgIndexTPexe(int Absolute, float Position, short Brake);
    }
}
