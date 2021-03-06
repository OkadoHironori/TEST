
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


namespace Board.BoardControl
{
    using CountChangeEventHandler = Action<object, NumUpdateEventArgs>;
    using ChkChangeEventHandler = Action<object, ChkChangeEventArgs>;
    /// <summary>
    /// ボードによる昇降
    /// </summary>
    public class XStageCtrl : IXStageCtrl, IDisposable
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
        /// GUIへのメッセージ送信
        /// </summary>
        public string SendGUIMes { get; private set; }
        /// <summary>
        /// ボード上の昇降位置 (mm)
        /// </summary>
        public float FXPos { get; private set; }
        /// <summary>
        /// ボード上のカウント値(PLS)
        /// </summary>
        public uint FXPosCount { get; private set; } = BoardParam.Default.XStgCount;
        /// <summary>
        /// ボード上のエンコード後のカウント
        /// </summary>
        public uint FXPosEncoderCount { get; private set; } = BoardParam.Default.XStgEncoderCount;
        /// <summary>
        /// 昇降パラメータ　CSV読み込めば基本固定
        /// </summary>
        public XStgParamFix Param { get; private set; } = new XStgParamFix();
        /// <summary>
        /// ボード上の指令速度値(deg/sec)
        /// </summary>
        public float XStgSpeed { get; private set; }
        /// <summary>
        /// ボード上のステータス値
        /// </summary>
        public uint XStgSts { get; private set; }
        /// <summary>
        /// ボード上のステータス値：動作中
        /// </summary>
        public bool XStgBusy { get; private set; }
        /// <summary>
        /// ボード上のステータス値：定速動作中
        /// </summary>
        public bool XStgConst { get; private set; }
        /// <summary>
        /// ボード上のステータス値：準備完了
        /// </summary>
        public bool XStgReady { get; private set; }
        /// <summary>
        /// ボード上のステータス値：停止指令入力
        /// </summary>
        public bool XStgStop { get; private set; }
        /// <summary>
        /// ボード上のステータス値：原点正常
        /// </summary>
        public bool XStgOrg { get; private set; }
        /// <summary>
        /// ボード上のステータス値：＋リミット
        /// </summary>
        public bool XStgCwEls { get; private set; }
        /// <summary>
        /// ボード上のステータス値：−リミット
        /// </summary>
        public bool XStgCcwEls { get; private set; }
        /// <summary>
        /// ボード上のステータス値：原点センサ
        /// </summary>
        public bool XStgOls { get; private set; }
        /// <summary>
        /// ボード上のステータス値：減速センサ
        /// </summary>
        public bool XStgDls { get; private set; }
        /// <summary>
        /// ボード上のステータス値：アラーム
        /// </summary>
        public bool XStgAlm { get; private set; }
        /// <summary>
        /// ボード上のエラー情報
        /// </summary>
        public uint XStgErrSts { get; private set; }
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
        /// ボード情報I/F
        /// </summary>
        private readonly IBoardProvider _Provider;
        /// <summary>
        /// ボード設定I/F
        /// </summary>
        private readonly IBoardConfig _BoardConf;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public XStageCtrl(IBoardControl board, IBoardProvider provider, IBoardConfig boardconf, IBoardSender bsender)
        {
            _BoardCnt = board;

            _BoardConf = boardconf;
            _BoardConf.EndLoadBoardConf += (s, e) =>
            {
                BoardConfig bcf = s as BoardConfig;
                uint tmpdvenum = bcf.Boards.ToList().Find(p => p.BAxis == BoardAxis.XSTG_JIKU).ID;
                if (tmpdvenum == 0) throw new Exception($"{nameof(XStageCtrl)} has exception error");
                DevNum = tmpdvenum;
                Init();
            };
            _BoardConf.RequestParam();

            _ValueSender = bsender;
            _ValueSender.SetTimer(200);
            _ValueSender.NotifyFX += (s, e) =>
            {
                NotifyCount.Invoke(this, new NumUpdateEventArgs(e.NumValue));//位置を通知
            };

            _Provider = provider;
            _Provider.PropertyChanged += (s, e) =>
            {
                var bpservice = s as BoardProvider;
                DateTime dt = DateTime.Now;

                switch (e.PropertyName)
                {
                    case (nameof(BoardProvider.FXCount)):

                        FXPosCount = bpservice.FXCount;

                        FXPos = _BoardCnt.PlsToDeg((int)FXPosCount, Param.XStgStep, Param.XStgDir);

                        _ValueSender.PushData(FXPos, nameof(BoardAxis.XSTG_JIKU));

                        uint encordcntdata = 0;

                        uint retd = Hicpd530.cp530_rReg(DevNum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.RRCTR2, ref encordcntdata);

                        FXPosEncoderCount = encordcntdata;

                        break;
                    case (nameof(BoardProvider.FXSpeed)):

                        XStgSpeed = bpservice.FXSpeed;
                        Debug.WriteLine($"{dt.TimeOfDay}  {e.PropertyName}速度{XStgSpeed.ToString("0.00")}");
                        break;

                    case (nameof(BoardProvider.FXSts)):

                        XStgSts = (uint)bpservice.FXSts;

                        //ステータス分割
                        uint sts = 0;
                        XStgReady = false;
                        XStgBusy = false;
                        XStgConst = false;
                        XStgCwEls = false;
                        XStgCcwEls = false;
                        XStgOls = false;
                        XStgDls = false;
                        XStgAlm = false;
                        if (Param.XStgMotorType == 1)
                        {
                            //準備完了＝ALM無しかつSVONかつ停止中
                            sts = XStgSts & 0x0F01;
                            if (sts == 0x0001)
                            {
                                XStgReady = true;
                            }
                            //動作中＝加速中、減速中、定速動作中
                            sts = XStgSts & 0x0F01;
                            if (sts == 0x0101 || sts == 0x0201 || sts == 0x0401)
                            {
                                XStgBusy = true;
                            }
                            //定速動作中
                            sts = XStgSts & 0x0F01;
                            if (sts == 0x0401)
                            {
                                XStgConst = true;
                            }
                        }
                        else
                        {
                            //準備完了＝ALM無しかつSVONかつ停止中
                            sts = XStgSts & 0x0F00;
                            if (sts == 0x0000)
                            {
                                XStgReady = true;
                            }
                            //動作中＝加速中、減速中、定速動作中
                            sts = XStgSts & 0x0F01;
                            if (sts == 0x0100 || sts == 0x0200 || sts == 0x0400)
                            {
                                XStgBusy = true;
                            }
                            //定速動作中
                            sts = XStgSts & 0x0F00;
                            if (sts == 0x0400)
                            {
                                XStgConst = true;
                            }
                        }

                        sts = XStgSts & 0x0800;
                        if (sts == 0x0800)
                        {
                            XStgAlm = true;
                        }
                        sts = XStgSts & 0x1000;
                        if (sts == 0x1000)
                        {
                            XStgCwEls = true;
                        }
                        sts = XStgSts & 0x2000;
                        if (sts == 0x2000)
                        {
                            XStgCcwEls = true;
                        }
                        sts = XStgSts & 0x4000;
                        if (sts == 0x4000)
                        {
                            XStgOls = true;
                        }
                        sts = XStgSts & 0x8000;
                        if (sts == 0x8000)
                        {
                            XStgDls = true;
                        }

                        StsChanged?.Invoke(this, new EventArgs());

                        break;
                    case (nameof(BoardProvider.RotErrorMessage)):
                        SendGUIMes = bpservice.RotErrorMessage;
                        StsChanged?.Invoke(this, new EventArgs());

                        break;
                }
            };

        }
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="cont"></param>
        public bool Init()
        {
            uint bnum = DevNum;
            int temp = 0;

            uint ret = 0;
            if (ret == 0)
            {//ボードのカウント書込 UP/DOWNカウント値書込み(保存している現在位置に設定する)
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRCTR1, FXPosCount);
            }
            if (ret == 0)
            {//ボードのエンコーダカウント書込 UP/DOWNカウント値書込み(保存している現在位置に設定する)
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRCTR2, FXPosEncoderCount);
            }

            if (ret == 0)
            {//ｺﾝﾊﾟﾚｰﾀﾃﾞｰﾀ設定
                if (Param.XStgLmtNoChk == 0)
                {
                    uint XStgCwLmtPls = _BoardCnt.DegToPls(Param.XStgCwLmtPos, Param.XStgStep, Param.XStgDir);
                    ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRCMP2, XStgCwLmtPls);
                    if (ret == 0)
                    {
                        uint XStgCcwLmtPls = _BoardCnt.DegToPls(Param.XStgCcwLmtPos, Param.XStgStep, Param.XStgDir);
                        ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRCMP1, XStgCcwLmtPls);
                    }
                    Param.XStgEnv4Data = Param.XStgEnv4Data | 0x00005054;
                }
                if (Param.XStgNoSlowMode == 0)
                {
                    uint XStgCrLowPls = _BoardCnt.DegToPls(Param.XStgCrLowPos, Param.XStgStep, Param.XStgDir);
                    ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRCMP3, XStgCrLowPls);
                    if (ret == 0)
                    {
                        uint XStgCrHiPls = _BoardCnt.DegToPls(Param.XStgCrHiPos, Param.XStgStep, Param.XStgDir);
                        ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRCMP4, XStgCrHiPls);
                    }
                    Param.XStgEnv4Data = Param.XStgEnv4Data | 0x74700000;
                    Param.XStgEnvLmOn = Param.XStgEnvLmOn | 0x74700000;
                    Param.XStgEnvLmOf = Param.XStgEnvLmOf | 0x74700000;
                    Param.XStgEnvLmCwOf = Param.XStgEnvLmCwOf | 0x74700000;
                    Param.XStgEnvLmCcwOf = Param.XStgEnvLmCcwOf | 0x74700000;
                }
            }
            if (ret == 0)
            {//補助速度レジスタセット RFA速度
                uint SubSpd = _BoardCnt.SpdToPps(Param.XStgOriginSpdFL, Param.XStgStep, (int)Param.XStgMag);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRFA, SubSpd);
            }
            if (ret == 0)
            {// 速度倍率設定
                ushort Rmg = (ushort)((300 / Param.XStgMag) - 1);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WPRMG, (uint)Rmg);
            }
            if (ret == 0)
            {// レジスタ書込み（減速点:PR5 = 0）
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WPRDP, (int)0x00000000);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定1:RENV1）
                Param.XStgEnv1Data = Param.XStgEnv1Data | (uint)Param.XStgPlsMode;//パルス入力方式設定
                if (Param.XStgElsMode == 1)//ELSで減速停止の場合
                {
                    Param.XStgEnv1Data = Param.XStgEnv1Data | 0x00000008;
                }
                if (Param.XStgMotorType == 1)//サーボの場合は偏差カウンタクリア
                {
                    Param.XStgEnv1Data = Param.XStgEnv1Data | 0x00000800;
                }
                if (Param.XStgDlsPolarity == 0)//DLSがA接点の場合
                {
                    Param.XStgEnv1Data = Param.XStgEnv1Data | 0x00000040;
                }
                if (Param.XStgOrgPolarity == 0)//OLSがA接点の場合
                {
                    Param.XStgEnv1Data = Param.XStgEnv1Data | 0x00000080;
                }
                if (Param.XStgAlmPolarity == 0)//ALMがA接点の場合
                {
                    Param.XStgEnv1Data = Param.XStgEnv1Data | 0x00000200;
                }
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV1, (uint)Param.XStgEnv1Data); ;
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定2:R7）
                Param.XStgEnv2Data = Param.XStgEnv2Data & 0xFFCFFFFF;//逓倍設定
                temp = Param.XStgPlsMult << 20;
                Param.XStgEnv2Data = Param.XStgEnv2Data | (uint)temp;
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV2, (uint)Param.XStgEnv2Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定3:R8）
                Param.XStgEnv3Data = Param.XStgEnv3Data & 0xFFFFFFF0;//原点復帰方式
                Param.XStgEnv3Data = Param.XStgEnv3Data | (uint)Param.XStgOriginMode;
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV3, (uint)Param.XStgEnv3Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定4: RENV4）
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV4, (uint)Param.XStgEnv4Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定5:RENV5）
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV5, (uint)Param.XStgEnv5Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定6:RENV6）
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV6, (uint)Param.XStgEnv6Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定7:RENV7）
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV7, (uint)Param.XStgEnv7Data);
            }
            if (ret == 0)
            {// レジスタ書込み（アップダウンカウント値 = 動作後に設定,減速レート:PR15 = 0）
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WPRDR, (uint)0x00000000);
            }
            // レジスタ書込み（S字区間:PR16 = 0）
            if (ret == 0)
            {
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WPRUS, (int)0x00000000);
            }
            // ELS極性選択書込み
            if (ret == 0)
            {
                byte rdata = 0;
                byte wdata = 0;
                ret = Hicpd530.cp530_rPortB(bnum, 0x80, ref rdata);

                temp = 0x01 << (int)BoardAxis.XSTG_JIKU;
                temp = (temp + 1) * -1;
                temp = temp & rdata;
                wdata = (byte)temp;

                if (Param.XStgLmtPolarity == 0)
                {
                    temp = 0x01 << (int)BoardAxis.XSTG_JIKU;
                    temp = temp | wdata;
                    wdata = (byte)temp;
                }
                ret = Hicpd530.cp530_wPortB(bnum, 0x80, wdata);
            }
            if (ret == 0)
            {//アラームクリア＆サーボON
                Reset();
                XStgReady = true;
            }
            if (ret != 0)
            {
                //mesbox?.Invoke($"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}");
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
        /// <param name="rotMode">回転方向</param>
        /// <param name="manualspd">回転速度</param>
        /// <returns></returns>
        public bool Manual(RevMode revmode, float manualspd)
        {
            // 速度のチェック
            if (manualspd > Param.XStgManualLimitSpd || manualspd < Param.XStgSpdFL)
            {   // 指定速度異常
                SendGUIMes = $"{nameof(XStageCtrl)} Setting Speed is uncorrect";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            uint bnum = DevNum;
            uint ret = 0;
            bool result = true;
            XStgStop = false;

            //リミット処理
            if (Param.XStgLmtNoChk == 0)
            {
                if (FXPos < Param.XStgCwLmtPos)
                {
                    if (revmode == RevMode.CW)
                    {
                        if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV4, Param.XStgEnvLmCwOf))
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
                else if (FXPos > Param.XStgCcwLmtPos)
                {
                    if (revmode == RevMode.CCW)
                    {
                        if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV4, Param.XStgEnvLmCcwOf))
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
                    if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV4, Param.XStgEnvLmOn))
                    {   //エラー
                        SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                        StsChanged?.Invoke(this, new EventArgs());
                    }
                }
            }
            //減速処理
            if (Param.XStgNoSlowMode == 0)
            {
                if (FXPos < Param.XStgCrLowPos)
                {
                    if (revmode == RevMode.CCW && manualspd > Param.XStgSlowSpd)
                    {
                        manualspd = Param.XStgSlowSpd;
                    }
                }

                if (FXPos > Param.XStgCrHiPos)
                {
                    if (revmode == RevMode.CW && manualspd > Param.XStgSlowSpd)
                    {
                        manualspd = Param.XStgSlowSpd;
                    }
                }
            }

            //速度データ生成
            ushort flspd = _BoardCnt.SpdToPps(Param.XStgSpdFL, Param.XStgStep, (int)Param.XStgMag);
            ushort fhspd = _BoardCnt.SpdToPps(manualspd, Param.XStgStep, (int)Param.XStgMag);
            ushort rate = _BoardCnt.AccelerationRate(Param.XStgRateTime, flspd, fhspd);
            long pos;
            if (revmode == RevMode.CW || revmode == RevMode.SCAN_CW)
            {
                pos = 134217727L;
            }
            else
            {
                pos = -134217728L;
            }
            int mode = Param.XStgMotorType == 0 ? 0x08008141 : 0x08008341;

            //速度データ書き込み
            ret = _BoardCnt.SetSpdReg(bnum, (int)BoardAxis.XSTG_JIKU, fhspd, flspd, rate, (uint)pos, (uint)mode);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            // コマンドバッファ書込み 高速スタート
            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.STAUD);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }
            if (revmode == RevMode.SCAN_CW || revmode == RevMode.SCAN_CCW)
            {
                //タイムアウト時間設定(加速時間＋100msec)
                int cnt = (int)(Param.XStgRateTime * 1000) + 500;
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
        public bool Index(PosiMode posimode, float indexpos, CancellationTokenSource ctoken)
        {
            uint bnum = DevNum;
            uint ret = 0;
            float checkpos;
            XStgStop = false;

            if (!XStgReady)
            {
                SendGUIMes = $"Not Ready";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            if (posimode == PosiMode.Abso)
            {
                if (!XStgOrg && Param.XStgOriginNoChk == 0)
                {
                    SendGUIMes = $"Not Ready";
                    StsChanged?.Invoke(this, new EventArgs());
                    return false;
                }
                checkpos = indexpos;
                indexpos = indexpos - FXPos;
            }
            else
            {
                checkpos = FXPos + indexpos;
            }

            if (Param.XStgLmtNoChk == 0)
            {
                if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV4, Param.XStgEnvLmOn))
                {   //エラー
                    SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                    StsChanged?.Invoke(this, new EventArgs());
                }

                if (checkpos > Param.XStgCwLmtPos)
                {
                    SendGUIMes = $"CW Limt Over";
                    StsChanged?.Invoke(this, new EventArgs());
                    return false;
                }

                if (checkpos < Param.XStgCcwLmtPos)
                {
                    SendGUIMes = $"CCW Limt Over";
                    StsChanged?.Invoke(this, new EventArgs());
                    return false;
                }
            }

            //速度データ生成
            ushort flspd = _BoardCnt.SpdToPps(Param.XStgIndexSpdFL, Param.XStgStep, (int)Param.XStgMag);
            ushort fhspd = _BoardCnt.SpdToPps(Param.XStgIndexSpdFH, Param.XStgStep, (int)Param.XStgMag);
            ushort rate = _BoardCnt.AccelerationRate(Param.XStgIndexRateTime, flspd, fhspd);
            uint pos = _BoardCnt.DegToPls(indexpos, Param.XStgStep, Param.XStgDir);
            int mode = Param.XStgMotorType == 0 ? 0x08008141 : 0x08008341;

            //速度データ書き込み
            ret = _BoardCnt.SetSpdReg(bnum, (int)BoardAxis.XSTG_JIKU, fhspd, flspd, rate, pos, (uint)mode);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            // コマンドバッファ書込み 高速スタート
            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.STAUD);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }
            bool result = false;


            checkpos = Param.XStgInpos * -1;

            if (indexpos <= checkpos || Param.XStgInpos <= indexpos)
            {
                //起動確認
                result = WaitMethod(bnum, TimeSpan.FromMilliseconds(500), WatchMode.Busy);
            }

            //完了待ち
            result = WaitMethod(bnum, TimeSpan.FromMilliseconds(Param.XStgIndexTimeOut), WatchMode.Ready);

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
            if (XStgBusy) XStgStop = true;
            switch (stopmode)
            {
                case (StopMode.Fast):// 即時停止
                    ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.STOP);
                    break;

                case (StopMode.Slow)://減速停止
                    ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.SDSTP);
                    break;
                default:
                    //ありえないが、とりあえず即時停止
                    ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.STOP);
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

            ret = Hicpd530.cp530_rReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.RREST, ref errsts);
            XStgErrSts = errsts;
            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.SVRSTON);   // サーボリセットON

            Task.WaitAll(Task.Delay(100));//100mce

            //サーボON
            if (Param.XStgMotorType == 1)
            {
                ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.SVON);   // サーボON
            }
            
            bool result = WaitMethod(bnum, TimeSpan.FromMilliseconds(1000), WatchMode.Ready);

            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.SVRSTOFF);   // サーボリセットOFF
                       
            XStgOrg = false;
            if (ret == 0) return true;

            return false;
            
        }

        /// <summary>
        /// 原点復帰
        /// </summary>
        public bool Origin(CancellationTokenSource ctoken)
        {
            uint ret = 0;
            XStgStop = false;
            bool result = false;
            uint bnum = DevNum;
            ushort flspd = _BoardCnt.SpdToPps(Param.XStgOriginSpdFL, Param.XStgStep, (int)Param.XStgMag);
            ushort fhspd = _BoardCnt.SpdToPps(Param.XStgOriginSpdFH, Param.XStgStep, (int)Param.XStgMag);
            ushort rate = _BoardCnt.AccelerationRate(Param.XStgRateTime, flspd, fhspd);
            uint pos = _BoardCnt.DegToPls(Param.XStgOriginShift, Param.XStgStep, Param.XStgDir);
            //+方向原点サーチ 0x08008215;	// +方向原点復帰
            //-方向原点サーチ 0x0800821D;	// -方向原点復帰
            uint mode = 0;
            //原点センサ抜け出し
            if (Param.XStgOriginMode == 2 & XStgOls)
            {
                result = Index(PosiMode.Rela, Param.XStgOriginShift, ctoken);
                if (!result || XStgStop) return false;
            }
            if (Param.XStgMotorType == 1)
            {
                mode = Param.XStgOriginDir == 0 ? (uint)0x08008315 : (uint)0x0800831D;
            }
            else
            {
                mode = Param.XStgOriginDir == 0 ? (uint)0x08008115 : (uint)0x0800811D;
            }
            if (!XStgReady)
            {
                SendGUIMes="Not Ready";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            //速度データ書き込み
            ret = _BoardCnt.SetSpdReg(bnum, (int)BoardAxis.XSTG_JIKU, fhspd, flspd, rate, pos, mode);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());

                return false;
            }

            //高速スタート
            if (ret != Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.STAUD))
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }
                    
            //起動確認
            result = WaitMethod(bnum, TimeSpan.FromMilliseconds(1000), WatchMode.Busy);
            XStgOrg = false;
            //DIOに原点復帰中を通知
            //NotifyRestOperation?.Invoke(this, new ChkChangeEventArgs(true));
            //原点復帰完了待ち
            result = WaitMethod(bnum, TimeSpan.FromMilliseconds(Param.XStgOriginTimeOut), WatchMode.Ready, ctoken);

            if (result && !XStgStop)
            {//ボードのカウント書込 UP/DOWNカウント値書込み(原点位置を設定)
                uint orgpospls = _BoardCnt.DegToPls(Param.XStgOriginPos, Param.XStgStep, Param.XStgDir);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRCTR1, orgpospls);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRCTR2, orgpospls);
                XStgOrg = true;
                result = Index(PosiMode.Rela, Param.XStgOriginOffset, ctoken);

                //DIOに原点復帰完了を通知
                //NotifyRestOperation?.Invoke(this, new ChkChangeEventArgs(false));
            }

            NotifyCount.Invoke(this, new NumUpdateEventArgs(FXPos));//位置を通知

            return result;
        }

        /// <summary>
        /// 待ち処理
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="bnum"></param>
        /// <param name="timeout">タイムアウト時間</param>
        /// <returns></returns>
        private bool WaitMethod(uint bnum, TimeSpan timeout, WatchMode watchmode, CancellationTokenSource ui_cancel = null)
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

                    if (watchmode == WatchMode.Ready && XStgReady)//指定した状態になったら抜ける
                    {
                        break;
                    }
                    if (watchmode == WatchMode.Busy && XStgBusy)///指定した状態になったら抜ける
                    {
                        break;
                    }
                    if (watchmode == WatchMode.Const && XStgConst)///指定した状態になったら抜ける
                    {
                        break;
                    }
                    if (XStgStop)
                    {
                        break;
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
                BoardParam.Default.XStgEncoderCount = FXPosEncoderCount;
                BoardParam.Default.XStgCount = FXPosCount;
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
        /// 微調Xの有効数値
        /// </summary>
        /// <returns></returns>
        public int GetScale()=> Param.XStgScale;
        /// <summary>
        /// 微調X軸の最大値
        /// </summary>
        /// <returns></returns>
        public float GetMax() => Param.XStgMaxValue;
        /// <summary>
        /// 微調X軸の最小値
        /// </summary>
        /// <returns></returns>
        public float GetMin() => Param.XStgMinValue;

        /// <summary>
        /// 最大速度 mm/sec
        /// </summary>
        /// <returns></returns>
        public float GetMaxSpd() => Param.XStgManualLimitSpd;
        /// <summary>
        /// 最小速度 mm/sec
        /// </summary>
        /// <returns></returns>
        public float GetMinSpd() => Param.XStgSpdFL;
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            //_BoardPro.Dispose();
        }
    }

    public interface IXStageCtrl
    {
        /// <summary>
        /// 現在値の通知
        /// </summary>
        event CountChangeEventHandler NotifyCount;
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
        bool Origin(CancellationTokenSource ctoken);
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
        /// インデックス移動
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="posimode"></param>
        /// <param name="indexpos"></param>
        /// <returns></returns>
        bool Index(PosiMode posimode, float indexpos, CancellationTokenSource ctoken);
        /// <summary>
        /// 微調Xの有効数値
        /// </summary>
        /// <returns></returns>
        int GetScale();
        /// <summary>
        /// 微調X軸の最大値
        /// </summary>
        /// <returns></returns>
        float GetMax();
        /// <summary>
        /// 微調X軸の最小値
        /// </summary>
        /// <returns></returns>
        float GetMin();
        /// <summary>
        /// 最大速度 mm/sec
        /// </summary>
        /// <returns></returns>
        float GetMaxSpd();
        /// <summary>
        /// 最小速度 mm/sec
        /// </summary>
        /// <returns></returns>
        float GetMinSpd();

    }
}