using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using CTAPI;
using CT30K.Common;

namespace CT30K
{
    public class scanstop
    {
        public struct ScanStopStruct
        {
            public int scan_stop;
            public int emergency;
            public int ups_power100;
            public int ups_power200;
        }

        const string SCAN_STOP_FILE = "c:\\ct\\common\\scanstop";

        //
        //   スキャンストップファイルの読込
        //
        public static bool ReadScanStop(ref ScanStopStruct theInfo)
        {
            bool functionReturnValue = false;

            FileStream fs = null;
            BinaryReader br = null;
            //ファイルオープン
            fs = new FileStream(SCAN_STOP_FILE, FileMode.OpenOrCreate, FileAccess.Read);
            br = new BinaryReader(fs);

            theInfo.scan_stop = br.ReadInt32();
            theInfo.emergency = br.ReadInt32();
            theInfo.ups_power100 = br.ReadInt32();
            theInfo.ups_power200 = br.ReadInt32();

            functionReturnValue = true;

            return (functionReturnValue);
        }

        //
        //   スキャンストップファイルの書き込み
        //
        public static bool WriteScanStop(ScanStopStruct theInfo)
        {

            //戻り値初期化
            bool functionReturnValue = false;

            FileStream fs = null;
            BinaryWriter bw = null;

            //ファイルオープン
            fs = new FileStream(SCAN_STOP_FILE, FileMode.OpenOrCreate, FileAccess.Write);
            bw = new BinaryWriter(fs);


            bw.Write(theInfo.scan_stop);
            bw.Write(theInfo.emergency);
            bw.Write(theInfo.ups_power100);
            bw.Write(theInfo.ups_power200);

            functionReturnValue = true;

            return functionReturnValue;
        }
    }

}
