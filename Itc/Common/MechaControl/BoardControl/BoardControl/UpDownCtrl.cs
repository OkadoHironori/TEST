
using Board.BoardControl;
using Itc.Common.Event;
using Itc.Common.TXEnum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Board.BoardControl
{
    using CountChangeEventHandler = Action<object, NumUpdateEventArgs>;
    using ChkChangeEventHandler = Action<object, ChkChangeEventArgs>;
    /// <summary>
    /// ボードによる昇降
    /// </summary>
    public class UpDownCtrl : IUpDownCtrl, IDisposable
    {
        /// <summary>
        /// 昇降位置の通知
        /// </summary>
        public event CountChangeEventHandler NotifyCount;
        /// <summary>
        /// 状態変化通知
        /// </summary>
        public event EventHandler StsChanged;
        /// <summary>
        /// 応答要求
        /// </summary>
        public event EventHandler Respons;
        /// <summary>
        /// GUIへのメッセージ送信
        /// </summary>
        public string SendGUIMes { get; private set; }
        /// <summary>
        /// ボード上の昇降位置 (mm)
        /// </summary>
        public float UdPos { get; private set; }
        /// <summary>
        /// ボード上のカウント値(PLS)
        /// </summary>
        public uint UdPosCount { get; private set; } = BoardParam.Default.UdCount;
        /// <summary>
        /// ボード上のエンコード後のカウント
        /// </summary>
        public uint UdPosEncoderCount { get; private set; } = BoardParam.Default.UdEncoderCount;
        /// <summary>
        /// 昇降パラメータ　CSV読み込めば基本固定
        /// </summary>
        public UdParamFix Param { get; private set; } = new UdParamFix();
        /// <summary>
        /// ボード上の指令速度値(deg/sec)
        /// </summary>
        public float UdSpeed { get; private set; }
        /// <summary>
        /// ボード上のステータス値
        /// </summary>
        public uint UdSts { get; private set; }
        /// <summary>
        /// ボード上のステータス値：動作中
        /// </summary>
        public bool UdBusy { get; private set; }
        /// <summary>
        /// ボード上のステータス値：定速動作中
        /// </summary>
        public bool UdConst { get; private set; }
        /// <summary>
        /// ボード上のステータス値：準備完了
        /// </summary>
        public bool UdReady { get; private set; }
        /// <summary>
        /// ボード上のステータス値：停止指令入力
        /// </summary>
        public bool UdStop { get; private set; }
        /// <summary>
        /// ボード上のステータス値：原点正常
        /// </summary>
        public bool UdOrg { get; private set; }
        /// <summary>
        /// ボード上のステータス値：＋リミット
        /// </summary>
        public bool UdCwEls { get; private set; }
        /// <summary>
        /// ボード上のステータス値：－リミット
        /// </summary>
        public bool UdCcwEls { get; private set; }
        /// <summary>
        /// ボード上のステータス値：原点センサ
        /// </summary>
        public bool UdOls { get; private set; }
        /// <summary>
        /// ボード上のステータス値：減速センサ
        /// </summary>
        public bool UdDls { get; private set; }
        /// <summary>
        /// ボード上のステータス値：アラーム
        /// </summary>
        public bool UdAlm { get; private set; }
        /// <summary>
        /// ボード上のエラー情報
        /// </summary>
        public uint UdErrSts { get; private set; }
        /// <summary>
        /// デバイス番号
        /// </summary>
        public uint DevNum { get; private set; }
        /// <summary>
        /// ボード制御
        /// </summary>
        private readonly IBoardControl _BoardCnt;
        /// <summary>
        /// ボード表示値送信
        /// </summary>
        private readonly IBoardSender _ValueSender;
        /// <summary>
        /// ボード設定I/F
        /// </summary>
        private readonly IBoardConfig _BoardConf;
        /// <summary>
        /// ボード情報I/F
        /// </summary>
        private readonly IBoardProvider _Provider;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UpDownCtrl(IBoardControl ctrl, IBoardConfig boardconf, IBoardProvider provider, IBoardSender boardSender)
        {
            _BoardCnt = ctrl;

            _BoardConf = boardconf;
            _BoardConf.EndLoadBoardConf += (s, e) =>
            {
                BoardConfig bcf = s as BoardConfig;
                uint tmpdvenum = bcf.Boards.ToList().Find(p => p.BAxis == BoardAxis.UD_JIKU).ID;
                if (tmpdvenum == 0) throw new Exception($"{nameof(RotationCtrl)} has exception error");
                DevNum = tmpdvenum;
                Init();
            };
            _BoardConf.RequestParam();

            _ValueSender = boardSender;
            _ValueSender.SetTimer(200);
            _ValueSender.NotifyUd += (s, e) =>
            {
                NotifyCount.Invoke(this, new NumUpdateEventArgs(e.NumValue));//PLC通知
                Debug.WriteLine($"{nameof(UpDownCtrl)}の位置は{e.NumValue.ToString()}");
            };

            _Provider = provider;
            _Provider.PropertyChanged += (s, e) =>
            {
                var bpservice = s as BoardProvider;
                DateTime dt = DateTime.Now;
                switch (e.PropertyName)
                {
                    case (nameof(BoardProvider.UDCount)):

                        UdPosCount = bpservice.UDCount;
                        UdPos = _BoardCnt.PlsToDeg((int)UdPosCount, Param.UdStep, Param.UdDir);
                        _ValueSender.PushData(UdPos, nameof(BoardAxis.UD_JIKU));
                        uint encordcntdata = 0;
                        uint retd = Hicpd530.cp530_rReg(DevNum, (int)BoardAxis.UD_JIKU, Cp530l1a.RRCTR2, ref encordcntdata);
                        UdPosEncoderCount = encordcntdata;

                        break;

                    case (nameof(BoardProvider.UDSpeed)):

                        UdSpeed = _BoardCnt.PlsToDeg((int)bpservice.UDSpeed, Param.UdStep, 1) * Param.UdMag;
                        //Debug.WriteLine($"{dt.TimeOfDay}  {e.PropertyName}速度{UdSpeed.ToString("0.00")}");
                        break;

                    case (nameof(BoardProvider.UDSts)):

                        UdSts = bpservice.UDSts;
                        //ステータス分割
                        uint sts = 0;
                        UdReady = false;
                        UdBusy = false;
                        UdConst = false;
                        UdCwEls = false;
                        UdCcwEls = false;
                        UdOls = false;
                        UdDls = false;
                        UdAlm = false;
                        if (Param.UdMotorType == 1)
                        {
                            //準備完了＝ALM無しかつSVONかつ停止中
                            sts = UdSts & 0x0F01;
                            if (sts == 0x0001)
                            {
                                UdReady = true;
                            }
                            //動作中＝加速中、減速中、定速動作中
                            sts = UdSts & 0x0F01;
                            if (sts == 0x0101 || sts == 0x0201 || sts == 0x0401)
                            {
                                UdBusy = true;
                            }
                            //定速動作中
                            sts = UdSts & 0x0F01;
                            if (sts == 0x0401)
                            {
                                UdConst = true;
                            }
                        }
                        else
                        {
                            //準備完了＝ALM無しかつ停止中
                            sts = UdSts & 0x0F00;
                            if (sts == 0x0000)
                            {
                                UdReady = true;
                            }
                            //動作中＝加速中、減速中、定速動作中
                            sts = UdSts & 0x0F00;
                            if (sts == 0x0100 || sts == 0x0200 || sts == 0x0400)
                            {
                                UdBusy = true;
                            }
                            //定速動作中
                            sts = UdSts & 0x0F00;
                            if (sts == 0x0400)
                            {
                                UdConst = true;
                            }
                        }
                        sts = UdSts & 0x0800;
                        if (sts == 0x0800)
                        {
                            UdAlm = true;
                        }
                        sts = UdSts & 0x1000;
                        if (sts == 0x1000)
                        {
                            UdCwEls = true;
                        }
                        sts = UdSts & 0x2000;
                        if (sts == 0x2000)
                        {
                            UdCcwEls = true;
                        }
                        sts = UdSts & 0x4000;
                        if (sts == 0x4000)
                        {
                            UdOls = true;
                        }
                        sts = UdSts & 0x8000;
                        if (sts == 0x8000)
                        {
                            UdDls = true;
                        }

                        StsChanged?.Invoke(this, new EventArgs());

                        break;

                    case (nameof(BoardProvider.UDErrorMessage)):

                        SendGUIMes = bpservice.UDErrorMessage;

                        StsChanged?.Invoke(this, new EventArgs());

                        break;

                };
            };
        }
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="cont"></param>
        public bool Init()
        {
            //Param = new UdParamFix();
            uint bnum = DevNum;
            int temp = 0;

            uint ret = 0;
            if (ret == 0)
            {//ボードのカウント書込 UP/DOWNカウント値書込み(保存している現在位置に設定する)
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRCTR1, UdPosCount);
            }
            if (ret == 0)
            {//ボードのエンコーダカウント書込 UP/DOWNカウント値書込み(保存している現在位置に設定する)
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRCTR2, UdPosEncoderCount);
            }

            if (ret == 0)
            {//ｺﾝﾊﾟﾚｰﾀﾃﾞｰﾀ設定
                if (Param.UdLmtNoChk == 0)
                {
                    uint UdCwLmtPls = _BoardCnt.DegToPls(Param.UdCwLmtPos, Param.UdStep, Param.UdDir);
                    ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRCMP2, UdCwLmtPls);
                    if (ret == 0)
                    {
                        uint UdCcwLmtPls = _BoardCnt.DegToPls(Param.UdCcwLmtPos, Param.UdStep, Param.UdDir);
                        ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRCMP1, UdCcwLmtPls);
                    }
                    Param.UdEnv4Data = Param.UdEnv4Data | 0x00005054;
                }
                if (Param.UdNoSlowMode == 0)
                {
                    uint UdCrLowPls = _BoardCnt.DegToPls(Param.UdCrLowPos, Param.UdStep, Param.UdDir);
                    ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRCMP3, UdCrLowPls);
                    if (ret == 0)
                    {
                        uint UdCrHiPls = _BoardCnt.DegToPls(Param.UdCrHiPos, Param.UdStep, Param.UdDir);
                        ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRCMP4, UdCrHiPls);
                    }
                    Param.UdEnv4Data = Param.UdEnv4Data | 0x74700000;
                    Param.UdEnvLmOn = Param.UdEnvLmOn | 0x74700000;
                    Param.UdEnvLmOf = Param.UdEnvLmOf | 0x74700000;
                    Param.UdEnvLmCwOf = Param.UdEnvLmCwOf | 0x74700000;
                    Param.UdEnvLmCcwOf = Param.UdEnvLmCcwOf | 0x74700000;
                }
            }
            if (ret == 0)
            {//補助速度レジスタセット RFA速度
                uint SubSpd = _BoardCnt.SpdToPps(Param.UdOriginSpdFL, Param.UdStep, (int)Param.UdMag);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRFA, SubSpd);
            }
            if (ret == 0)
            {// 速度倍率設定
                ushort Rmg = (ushort)((300 / Param.UdMag) - 1);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WPRMG, (uint)Rmg);
            }
            if (ret == 0)
            {// レジスタ書込み（減速点:PR5 = 0）
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WPRDP, (int)0x00000000);
            }
            if (ret == 0)
            { // レジスタ書込み（環境設定1:RENV1）
                Param.UdEnv1Data = Param.UdEnv1Data | (uint)Param.UdPlsMode;//パルス入力方式設定
                if (Param.UdElsMode == 1)//ELSで減速停止の場合
                {
                    Param.UdEnv1Data = Param.UdEnv1Data | 0x00000008;
                }
                if (Param.UdMotorType == 1)//サーボの場合は偏差カウンタクリア
                {
                    Param.UdEnv1Data = Param.UdEnv1Data | 0x00000800;
                }
                if (Param.UdDlsPolarity == 0)//DLSがA接点の場合
                {
                    Param.UdEnv1Data = Param.UdEnv1Data | 0x00000040;
                }
                if (Param.UdOrgPolarity == 0)//OLSがA接点の場合
                {
                    Param.UdEnv1Data = Param.UdEnv1Data | 0x00000080;
                }
                if (Param.UdAlmPolarity == 0)//ALMがA接点の場合
                {
                    Param.UdEnv1Data = Param.UdEnv1Data | 0x00000200;
                }
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRENV1, (uint)Param.UdEnv1Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定2:R7）
                Param.UdEnv2Data = Param.UdEnv2Data & 0xFFCFFFFF;//逓倍設定
                temp = Param.UdPlsMult << 20;
                Param.UdEnv2Data = Param.UdEnv2Data | (uint)temp;
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRENV2, (uint)Param.UdEnv2Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定3:R8）
                Param.UdEnv3Data = Param.UdEnv3Data & 0xFFFFFFF0;//原点復帰方式
                Param.UdEnv3Data = Param.UdEnv3Data | (uint)Param.UdOriginMode;
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRENV3, (uint)Param.UdEnv3Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定4: RENV4）
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRENV4, (uint)Param.UdEnv4Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定5:RENV5）
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRENV5, (uint)Param.UdEnv5Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定6:RENV6）
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRENV6, (uint)Param.UdEnv6Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定7:RENV7）
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRENV7, (uint)Param.UdEnv7Data);
            }
            if (ret == 0)
            {// レジスタ書込み（アップダウンカウント値 = 動作後に設定,減速レート:PR15 = 0）
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WPRDR, (uint)0x00000000);
            }
            // レジスタ書込み（S字区間:PR16 = 0）
            if (ret == 0)
            {
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WPRUS, (int)0x00000000);
            }
            // ELS極性選択書込み
            if (ret == 0)
            {
                byte rdata = 0;
                byte wdata = 0;
                ret = Hicpd530.cp530_rPortB(bnum, 0x80, ref rdata);

                temp = 0x01 << (int)BoardAxis.UD_JIKU;
                temp = (temp + 1) * -1;
                temp = temp & rdata;
                wdata = (byte)temp;

                if (Param.UdLmtPolarity == 0)
                {
                    temp = 0x01 << (int)BoardAxis.UD_JIKU;
                    temp = temp | wdata;
                    wdata = (byte)temp;
                }
                ret = Hicpd530.cp530_wPortB(bnum, 0x80, wdata);
            }
            if (ret == 0)
            {//アラームクリア＆サーボON
                Reset();
                UdReady = true;
            }
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());

                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 手動回転
        /// </summary>
        /// <param name="mesbox">メッセージ</param>
        /// <param name="revMode">回転方向</param>
        /// <param name="manualspd">回転速度</param>
        /// <returns></returns>
        public bool Manual(RevMode revmode, float manualspd)
        {
            // 速度のチェック
             if (manualspd > Param.UdManualLimitSpd || manualspd < Param.UdSpdFL)
            {   // 指定速度異常
                SendGUIMes = $"Setting Speed is uncorrect";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            uint bnum = DevNum;
            uint ret = 0;
            bool result = true;
            UdStop = false;

            //リミット処理
            if (Param.UdLmtNoChk == 0)
            {
                if (UdPos < Param.UdCwLmtPos)
                {
                    if (revmode == RevMode.CW)
                    {
                        if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRENV4, Param.UdEnvLmCwOf))
                        {   //エラー
                            SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                            StsChanged?.Invoke(this, new EventArgs());
                        }
                    }
                    else
                    {
                        throw new Exception("Undeveloped");
                    }
                }
                else if (UdPos > Param.UdCcwLmtPos)
                {
                    if (revmode == RevMode.CCW)
                    {
                        if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRENV4, Param.UdEnvLmCcwOf))
                        {   //エラー
                            SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                            StsChanged?.Invoke(this, new EventArgs());
                        }
                    }
                    else
                    {
                        throw new Exception("Undeveloped");
                    }
                }
                else
                {
                    if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRENV4, Param.UdEnvLmOn))
                    {   //エラー
                        SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                        StsChanged?.Invoke(this, new EventArgs());
                    }
                }
            }

            //減速処理
            if (Param.UdNoSlowMode == 0)
            {
                if (UdPos < Param.UdCrLowPos)
                {
                    if (revmode == RevMode.CCW && manualspd > Param.UdSlowSpd)
                    {
                        manualspd = Param.UdSlowSpd;
                    }
                }

                if (UdPos > Param.UdCrHiPos)
                {
                    if (revmode == RevMode.CW && manualspd > Param.UdSlowSpd)
                    {
                        manualspd = Param.UdSlowSpd;
                    }
                }
            }

            //速度データ生成
            ushort flspd = _BoardCnt.SpdToPps(Param.UdSpdFL, Param.UdStep, (int)Param.UdMag);
            ushort fhspd = _BoardCnt.SpdToPps(manualspd, Param.UdStep, (int)Param.UdMag);
            ushort rate = _BoardCnt.AccelerationRate(Param.UdRateTime, flspd, fhspd);
            long pos;
            if (revmode == RevMode.CW || revmode == RevMode.SCAN_CW)
            {
                pos = 134217727L;
            }
            else
            {
                pos = -134217728L;
            }
            int mode = Param.UdMotorType == 0 ? 0x08008141 : 0x08008341;
            //速度データ書き込み
            ret = _BoardCnt.SetSpdReg(bnum, (int)BoardAxis.UD_JIKU, fhspd, flspd, rate, (uint)pos, (uint)mode);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            // コマンドバッファ書込み 高速スタート
            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.STAUD);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            if (revmode == RevMode.SCAN_CW || revmode == RevMode.SCAN_CCW)
            {
                //タイムアウト時間設定(加速時間＋100msec)
                int cnt = (int)(Param.UdRateTime * 1000) + 500;
                result = WaitMethod(bnum, TimeSpan.FromMilliseconds(cnt), WatchMode.Const);

                if (!result)
                {
                    Stop(StopMode.Fast);
                }
            }
            return result;
        }

        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="coordi"></param>
        /// <param name="indexpos"></param>
        /// <returns></returns>
        public bool Index(PosiMode posimode, float indexpos, CancellationTokenSource cts)
        {
            uint bnum = DevNum;
            uint ret = 0;
            float checkpos;
            UdStop = false;

            if (!UdReady)
            {
                SendGUIMes = $"Not Ready";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            if (posimode == PosiMode.Abso)
            {
                if (!UdOrg && Param.UdOriginNoChk == 0)
                {
                    SendGUIMes = $"Not Ready";
                    StsChanged?.Invoke(this, new EventArgs());
                    return false;
                }
                checkpos = indexpos;
                indexpos = indexpos - UdPos;
            }
            else
            {
                checkpos = UdPos + indexpos;
            }

            if (Param.UdLmtNoChk == 0)
            {
                if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRENV4, Param.UdEnvLmOn))
                {   //エラー
                    SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                    StsChanged?.Invoke(this, new EventArgs());
                }

                if (checkpos > Param.UdCwLmtPos)
                {
                    SendGUIMes = "CW Limt Over";
                    StsChanged?.Invoke(this, new EventArgs());
                    return false;
                }

                if (checkpos < Param.UdCcwLmtPos)
                {
                    SendGUIMes = "CCW Limt Over";
                    StsChanged?.Invoke(this, new EventArgs());
                    return false;
                }
            }

            //速度データ生成
            ushort flspd = _BoardCnt.SpdToPps(Param.UdIndexSpdFL, Param.UdStep, (int)Param.UdMag);
            ushort fhspd = _BoardCnt.SpdToPps(Param.UdIndexSpdFH, Param.UdStep, (int)Param.UdMag);
            ushort rate = _BoardCnt.AccelerationRate(Param.UdIndexRateTime, flspd, fhspd);
            uint pos = _BoardCnt.DegToPls(indexpos, Param.UdStep, Param.UdDir);
            int mode = Param.UdMotorType == 0 ? 0x08008141 : 0x08008341;

            //速度データ書き込み
            ret = _BoardCnt.SetSpdReg(bnum, (int)BoardAxis.UD_JIKU, fhspd, flspd, rate, pos, (uint)mode);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            // コマンドバッファ書込み 高速スタート
            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.STAUD);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }
            bool result = false;


            checkpos = Param.UdInpos * -1;

            if (indexpos <= checkpos || Param.UdInpos <= indexpos)
            {
                //起動確認
                result = WaitMethod(bnum, TimeSpan.FromMilliseconds(500), WatchMode.Busy, cts);
            }

            //完了待ち
            result = WaitMethod(bnum, TimeSpan.FromMilliseconds(Param.UdIndexTimeOut), WatchMode.Ready, cts);

            return result;
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool Stop(StopMode stopmode)
        {
            uint bnum = DevNum;
            uint ret = 0;
            if (UdBusy) UdStop = true;
            switch (stopmode)
            {
                case (StopMode.Fast):// 即時停止
                    ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.STOP);
                    break;
                case (StopMode.Slow)://減速停止
                    ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.SDSTP);
                    break;
                default:
                    //ありえないが、とりあえず即時停止
                    ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.STOP);
                    throw new Exception("Unkown stop mode");
            }
            bool result = ret == 0 ? true : false;
            return result;
        }
        /// <summary>
        /// リセット
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public bool Reset()
        {
            uint bnum = DevNum;
            uint ret = 0;
            uint errsts = 0;

            ret = Hicpd530.cp530_rReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.RREST, ref errsts);
            UdErrSts = errsts;
            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.SVRSTON);   // サーボリセットON

            Task.WaitAll(Task.Delay(100));//100mce

            //サーボON
            if (Param.UdMotorType == 1)
            {
                ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.SVON);   // サーボON
            }

            bool result = WaitMethod(bnum, TimeSpan.FromMilliseconds(1000), WatchMode.Ready);

            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.SVRSTOFF);   // サーボリセットOFF

            UdOrg = false;
            if (ret == 0) return true;

            return false;

        }
        /// <summary>
        /// 原点復帰
        /// </summary>
        public bool Origin(CancellationTokenSource ctoken)
        {
            uint ret = 0;
            UdStop = false;
            bool result = false;
            uint bnum = DevNum;
            ushort flspd = _BoardCnt.SpdToPps(Param.UdOriginSpdFL, Param.UdStep, (int)Param.UdMag);
            ushort fhspd = _BoardCnt.SpdToPps(Param.UdOriginSpdFH, Param.UdStep, (int)Param.UdMag);
            ushort rate = _BoardCnt.AccelerationRate(Param.UdRateTime, flspd, fhspd);
            uint pos = _BoardCnt.DegToPls(Param.UdOriginShift, Param.UdStep, Param.UdDir);
            //+方向原点サーチ 0x08008215;	// +方向原点復帰
            //-方向原点サーチ 0x0800821D;	// -方向原点復帰
            uint mode = 0;

            //原点センサ抜け出し
            if (Param.UdOriginMode == 2 & UdOls)
            {
                result = Index(PosiMode.Rela, Param.UdOriginShift, ctoken);
                if (!result || UdStop) return false;
            }
            if (Param.UdMotorType == 1)
            {
                mode = Param.UdOriginDir == 0 ? (uint)0x08008315 : (uint)0x0800831D;
            }
            else
            {
                mode = Param.UdOriginDir == 0 ? (uint)0x08008115 : (uint)0x0800811D;
            }
            if (!UdReady)
            {
                SendGUIMes = $"Not Ready";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }
            //速度データ書き込み
            ret = _BoardCnt.SetSpdReg(bnum, (int)BoardAxis.UD_JIKU, fhspd, flspd, rate, pos, mode);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }
   
            //高速スタート
            if (ret != Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.STAUD))
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            //起動確認
            result = WaitMethod(bnum, TimeSpan.FromMilliseconds(500), WatchMode.Busy);
            UdOrg = false;


            //原点復帰完了待ち
            result = WaitMethod(bnum, TimeSpan.FromMilliseconds(Param.UdOriginTimeOut), WatchMode.Ready, ctoken);
          
            if (result && !UdStop)
            {//ボードのカウント書込 UP/DOWNカウント値書込み(原点位置を設定)
                uint orgpospls = _BoardCnt.DegToPls(Param.UdOriginPos, Param.UdStep, Param.UdDir);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRCTR1, orgpospls);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.UD_JIKU, Cp530l1a.WRCTR2, orgpospls);
                UdOrg = true;
                result = Index(PosiMode.Rela, Param.UdOriginOffset, ctoken);
            }                     
            return result;
        }

        /// <summary>
        /// 待ち処理
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="bnum"></param>
        /// <param name="timeout">タイムアウト時間</param>
        /// <returns></returns>
        private bool WaitMethod(
            uint bnum,
            TimeSpan timeout,
            WatchMode watchmode,
            CancellationTokenSource ui_cancel = null,
            ComProgress com = null)
        {
            var source = new CancellationTokenSource();
            source.CancelAfter(timeout);
            Task t = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (source.IsCancellationRequested)
                    {
                        break;
                    }

                    if (ui_cancel != null && ui_cancel.IsCancellationRequested)
                    {
                        this.Stop(StopMode.Fast);
                        break;
                    }

                    if (watchmode == WatchMode.Ready && UdReady)//指定した状態になったら抜ける
                    {
                        break;
                    }
                    if (watchmode == WatchMode.Busy && UdBusy)///指定した状態になったら抜ける
                    {
                        break;
                    }
                    if (watchmode == WatchMode.Const && UdConst)///指定した状態になったら抜ける
                    {
                        break;
                    }
                    if (UdStop)
                    {
                        break;
                    }

                    if (com != null)
                    {
                        int tmpPercent = (int)Math.Min(Math.Abs(UdPos - com.Target) / (float)com.Total * 100, 100);
                        com.prog.Report(new CalProgress { Percent = 0, Status = $"{com.Message} {tmpPercent.ToString()} %" });
                    }

                    Task.WaitAll(Task.Delay(20));//20mce
                }
            }, source.Token);

            try
            {
                t.Wait(source.Token);
            }
            catch (OperationCanceledException)
            {//タイムアウト処理
                SendGUIMes = BoardMes.TIMEOUT_ERROR
                    + " " + timeout.TotalSeconds.ToString() + BoardMes.BOARD_SEC
                    + Environment.NewLine
                    + BoardMes.CONFIRM_OPERATION;
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }
            catch (AggregateException)
            {//例外処理
                SendGUIMes = BoardMes.EXCEPTION_ERROR
                    + Environment.NewLine
                    + BoardMes.REBOOT_APP;

                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }
            finally
            {
                //保存しておく
                BoardParam.Default.UdEncoderCount = UdPosEncoderCount;
                BoardParam.Default.UdCount = UdPosCount;
                BoardParam.Default.Save();
            }

            //UIキャンセルの場合はflase
            if (ui_cancel != null && ui_cancel.IsCancellationRequested)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// パラメータ要求
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public void Request() 
            => Respons?.Invoke(this, new EventArgs());
        /// <summary>
        /// 昇降の有効桁数
        /// </summary>
        /// <returns></returns>
        public int GetUDScale() => Param.UdScale;
        /// <summary>
        /// 昇降の最大値
        /// </summary>
        /// <returns></returns>
        public float GetUDMax() => Param.UdMaxValue;// Param.UdCwLmtPos;
        /// <summary>
        /// 昇降の最小値
        /// </summary>
        /// <returns></returns>
        public float GetUDMin() => Param.UdMinValue;
        /// <summary>
        /// 昇降のスピード最大値
        /// </summary>
        /// <returns></returns>
        public float GetUDSpeedMax() => Param.UdManualLimitSpd;
        /// <summary>
        /// 昇降のスピード最小値
        /// </summary>
        /// <returns></returns>
        public float GetUDSpeedMin() => Param.UdSpdFL;
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            if (UdBusy)
            {
                Stop(StopMode.Fast);
            }
        }
    }
    public interface IUpDownCtrl
    {
        /// <summary>
        /// 応答要求
        /// </summary>
        event EventHandler Respons;
        /// <summary>
        /// 現在値の通知
        /// </summary>
        event CountChangeEventHandler NotifyCount;
        /// <summary>
        /// 状態変化通知
        /// </summary>
        event EventHandler StsChanged;
        /// <summary>
        /// 初期化
        /// </summary>
        bool Init();
        /// <summary>
        /// 指定回転
        /// </summary>
        bool Manual(RevMode revmode, float manualspeed);
        /// <summary>
        /// 原点復帰
        /// </summary>
        /// <param name="mesbox"></param>
        bool Origin(CancellationTokenSource token);
        /// <summary>
        /// ストップ
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        bool Stop(StopMode mode);
        /// <summary>
        /// リセット
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        bool Reset();
        /// <summary>
        /// パラメータ要求
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        void Request();
        /// <summary>
        /// インデックス移動
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="posimode"></param>
        /// <param name="indexpos"></param>
        /// <returns></returns>
        bool Index(PosiMode posimode, float indexpos, CancellationTokenSource token);
        /// <summary>
        /// 昇降の有効桁数
        /// </summary>
        /// <returns></returns>
        int GetUDScale();
        /// <summary>
        /// 昇降の最大値
        /// </summary>
        /// <returns></returns>
        float GetUDMax();
        /// <summary>
        /// 昇降の最小値
        /// </summary>
        /// <returns></returns>
        float GetUDMin();
        /// <summary>
        /// 昇降のスピード最大値
        /// </summary>
        /// <returns></returns>
        float GetUDSpeedMax();
        /// <summary>
        /// 昇降のスピード最小値
        /// </summary>
        /// <returns></returns>
        float GetUDSpeedMin();
    }
}