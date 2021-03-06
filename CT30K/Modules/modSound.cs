using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

using CT30K;

///* ************************************************************************** */
///* システム　　： マイクロＣＴスキャナ TOSCANER-30000MCT Ver4.0               */
///* 客先　　　　： ?????? 殿                                                   */
///* プログラム名： modSound.cs                                                 */
///* 処理概要　　： 音関連モジュール                                            */
///* 注意事項　　： なし                                                        */
///* -------------------------------------------------------------------------- */
///* 適用計算機　： DOS/V PC                                                    */
///* ＯＳ　　　　： Windows 7 64bit  (SP1)                                      */
///* コンパイラ　： VS 2010(C#,.NET)                                            */
///* -------------------------------------------------------------------------- */
///* VERSION     DATE        BY                  CHANGE/COMMENT                 */
///*                                                                            */
///* v23.00      15/09/24    (検S1)長野          新規作成                       */
///*                                                                            */
///* -------------------------------------------------------------------------- */
///* ご注意：                                                                   */
///* 本書の内容の一部または全部を無断で使用、転載することは禁止されています。   */
///*                                                                            */
///* (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2015                 */
///* ************************************************************************** */
///
namespace CT30K
{
    public static class modSound
    {
        public enum PlaySoundFlags : int
        {
            SND_SYNC = 0x0000,
            SND_ASYNC = 0x0001,
            SND_NODEFAULT = 0x0002,
            SND_MEMORY = 0x0004,
            SND_LOOP = 0x0008,
            SND_NOSTOP = 0x0010,
            SND_NOWAIT = 0x00002000,
            SND_ALIAS = 0x00010000,
            SND_ALIAS_AD = 0x0020000,
            SND_RESOURCE = 0x00040004,
            SND_PURGE = 0x00040,
            SND_APPLICATION = 0x0080
        }

        [DllImport("winmm.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool PlaySound(string pszSound, IntPtr hmod, PlaySoundFlags fdwSound);

        //Rev23.21 by長野 2016/02/21
        //'マザーボードのBeep音(Timerと組み合わせてループさせる)
        //'dwFreq        :周波数(Hz)
        //'dwDuration    :継続時間(msec)
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int Beep(int dwFreq, int dwDuration);

        public static void PlayXrayOnWarningSound()
        {
            //Rev26.20 タッチパネル、PCのどちらから音をだすか場合分け
            if (CTSettings.iniValue.HSCWarningSoundMode == 1)
            {
                modSeqComm.SeqBitWrite("XrayPrewarning", true);
                modCT30K.PauseForDoEvents(CTSettings.iniValue.HSCWarningSoundTime);
                modSeqComm.SeqBitWrite("XrayPrewarning", false);
            }
            else
            {
                string Stop = null;
                //Console.Beep(10000,3000);//.NET
                //System.Media.SystemSounds.Beep.Play();//.NET
                PlaySound(CTSettings.iniValue.WavWarningPath, IntPtr.Zero, PlaySoundFlags.SND_ASYNC | PlaySoundFlags.SND_LOOP);

                Thread.Sleep(CTSettings.iniValue.HSCWarningSoundTime * 1000);

                PlaySound(Stop, IntPtr.Zero, PlaySoundFlags.SND_ASYNC);
            }
        }

        //Rev23.21 追加 by長野 2016/02/23
        public static void PlayXrayOnWarningSoundLoopStart()
        {
            //Rev26.20 タッチパネル、PCのどちらから音をだすか場合分け
            if (CTSettings.iniValue.HSCWarningSoundMode == 1)
            {
                modSeqComm.SeqBitWrite("XrayPrewarning", true);
            }
            else
            {
                //Console.Beep(10000,3000);//.NET
                //System.Media.SystemSounds.Beep.Play();//.NET
                PlaySound(CTSettings.iniValue.WavWarningPath, IntPtr.Zero, PlaySoundFlags.SND_ASYNC | PlaySoundFlags.SND_LOOP);
            }
        }

        //Rev23.21 追加 by長野 2016/02/23
        public static void PlayXrayOnWarningSoundLoopStop()
        {
            //Rev26.20 タッチパネル、PCのどちらから音をだすか場合分け
            if (CTSettings.iniValue.HSCWarningSoundMode == 1)
            {
                modSeqComm.SeqBitWrite("XrayPrewarning", false);
            }
            else
            {
                string Stop = null;
                PlaySound(Stop, IntPtr.Zero, PlaySoundFlags.SND_ASYNC);
            }
        }

        /// <summary>
        /// 指定した時間(sec)音を流す
        /// </summary>
        /// <param name="time"></param>
        public static void PlayXraySound(int time)
        {
            PlayXrayOnWarningSoundLoopStart();

            modCT30K.PauseForDoEvents(time);

            PlayXrayOnWarningSoundLoopStop();
        }

    }
}
