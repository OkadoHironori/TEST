/*
  
	共通ライブラリ     通信ポート・インターフェース
 
 
 
	(c) Copyright  TOSHIBA IT & Control Systems Corporation 2017, All Rights Reserved

    History:
        Date        Version     Explanation                             Modifier        
        -------------------------------------------------------------------------------
        2017/06/30  0.0.0.0     コーディング完成                        (AIT)M.KOIKE
 
*/
using System;
using System.Diagnostics;
using System.Text;

namespace Itc.Common.Event
{
    /// <summary>
    /// 通信ポート・インターフェース
    /// </summary>
    public interface ICommunicationPort : IDisposable
    {
        /// <summary>
        /// ポート名
        /// </summary>
        string PortName
        {
            get;
        }

        /// <summary>
        /// 接続確認
        /// </summary>
        bool Connected
        {
            get;
        }

        /// <summary>
        /// 読み取り可能データ量[byte]
        /// </summary>
        int Available
        {
            get;
        }

        /// <summary>
        /// 送信タイムアウト[msec]
        /// </summary>
        int SendTimeout
        {
            get;
            set;
        }

        /// <summary>
        /// 送信バッファ・サイズ[byte]
        /// </summary>
        int SendBufferSize
        {
            get;
            set;
        }

        /// <summary>
        /// 受信タイムアウト[msec]
        /// </summary>
        int ReceiveTimeout
        {
            get;
            set;
        }

        /// <summary>
        /// 受信バッファ・サイズ[byte]
        /// </summary>
        int ReceiveBufferSize
        {
            get;
            set;
        }

        /// <summary>
        /// 終端文字列
        /// </summary>
        string NewLine
        {
            get;
            set;
        }

        /// <summary>
        /// エンコーディング
        /// </summary>
        Encoding Encoding
        {
            get;
            set;
        }

        /// <summary>
        /// 最大リトライ回数
        /// </summary>
        int MaximumRetryCount
        {
            get;
            set;
        }

        /// <summary>
        /// 通信中断
        /// </summary>
        bool Abort
        {
            get;
            set;
        }

        /// <summary>
        /// ソケット通信データ送信
        /// </summary>
        /// <param name="data">送信データ</param>
        void Write(byte[] data);

        /// <summary>
        /// ソケット通信文字列送信
        /// </summary>
        /// <param name="message">送信文字列</param>
        void WriteLine(string message);

        /// <summary>
        /// ソケット通信データ受信
        /// </summary>
        /// <returns>受信データ</returns>
        byte[] Read();

        /// <summary>
        /// ソケット通信文字列受信
        /// </summary>
        /// <returns>受信文字列</returns>
        string ReadLine();

        /// <summary>
        /// ソケット通信切断
        /// </summary>
        void Close();
    }
}
