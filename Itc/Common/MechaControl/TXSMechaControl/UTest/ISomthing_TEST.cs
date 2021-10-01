using Board.BoardControl;
using Itc.Common.TXEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TXSMechaControl.UTest
{
    /// <summary>
    /// TXSの試料テーブル回転インターフェイス
    /// </summary>
    public interface ISomthing_TEST
    {
        List<float> List { get; }
        List<float> RList { get; }

        void Start();
    }
}
