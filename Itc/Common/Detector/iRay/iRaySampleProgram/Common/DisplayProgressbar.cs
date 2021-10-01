using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDetector
{
    public class DisplayProgressbar
    {
        string m_progress;
        string m_output;
        readonly int TotalValue = 35;
	    public void SetProgress(int nValue, int total)
	    {
		    if (nValue <= 0)
			    return;
		    int signalCurNum = nValue;
		    int singalTotal = total;
            if (total > TotalValue)
		    {
                signalCurNum = nValue * TotalValue / total;
                singalTotal = TotalValue;
		    }
            //m_progress.assign(signalCurNum, '#');
            //m_output.assign(singalTotal - signalCurNum + 2, ' ');
            m_progress = new string('#', signalCurNum);
            m_output = new string(' ', singalTotal - signalCurNum + 2);

           Console.Write(string.Format("{0}{1} {2}/{3}\r", m_progress, m_output, nValue, total));
	    }
    }
}
