/*
  
	共通ライブラリ     PINGクラス
 
 
 
	(c) Copyright  TOSHIBA IT & Control Systems Corporation 2017, All Rights Reserved

    History:
        Date        Version     Explanation                             Modifier        
        -------------------------------------------------------------------------------
        2017/02/08  0.0.0.0     コーディング完成                        (AIT)M.KOIKE
 
*/
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;

namespace Itc.Common.Event
{
    /// <summary>
    /// PINGクラス
    /// </summary>
    public class Ping
    {
        /// <summary>
        /// PING送信先ホスト名
        /// </summary>
        private string m_HostName = string.Empty;

        /// <summary>
        /// 最大ループ回数
        /// </summary>
        private int m_MaximumLoopCount = 3;

        /// <summary>
        /// タイムアウト時間(msec)
        /// </summary>
        private static int m_Timeout = 1000;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="host_name">ホスト名</param>
        public Ping(string host_name = null)
        {
            if (!string.IsNullOrEmpty(host_name))
            {
                m_HostName = host_name;
            }
        }

        /// <summary>
        /// 最大ループ回数
        /// </summary>
        public int MaximumLoopCount
        {
            get
            {
                return m_MaximumLoopCount;
            }
            set
            {
                if (value > 0)
                {
                    m_MaximumLoopCount = value;
                }
                else
                {
                    m_MaximumLoopCount = 0;
                }
            }
        }

        /// <summary>
        /// タイムアウト時間(msec)
        /// </summary>
        public static int Timeout
        {
            get
            {
                return m_Timeout;
            }
            set
            {
                if (value >= System.Threading.Timeout.Infinite)
                {
                    m_Timeout = value;
                }
            }
        }

        /// <summary>
        /// コンピュータPING応答確認
        /// </summary>
        /// <returns>
        /// true :PING応答あり
        /// false:PING応答なし
        /// </returns>
        public bool CheckAlive()
        {
            bool alive = false;

            for (int i = 0; i < m_MaximumLoopCount; i++)
            {
                System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();

                try
                {
                    //PING送信
                    PingReply reply;
                    if (m_Timeout >= 0)
                    {
                        reply = p.Send(m_HostName, m_Timeout);
                    }
                    else
                    {
                        reply = p.Send(m_HostName);
                    }

                    //結果取得
                    alive = (reply.Status == IPStatus.Success);
                }
                catch
                {
                    alive = false;
                }
                finally
                {
                    p.Dispose();
                }

                if (alive)
                {
                    break;
                }

                Thread.Sleep(100);
            }

            return alive;
        }
    }
}
