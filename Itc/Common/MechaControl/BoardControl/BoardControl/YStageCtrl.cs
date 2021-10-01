
using Board.BoardControl;
using Itc.Common.Event;
using Itc.Common.TXEnum;
using System;
using System.Collections.Generic;
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
    public class YStageCtrl : IYStageCtrl, IDisposable
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
        public float FYPos { get; private set; }
        /// <summary>
        /// ボード上のカウント値(PLS)
        /// </summary>
        public uint FYPosCount { get; private set; } = BoardParam.Default.YStgCount;
        /// <summary>
        /// ボード上のエンコード後のカウント
        /// </summary>
        public uint FYPosEncoderCount { get; private set; } = BoardParam.Default.YStgEncoderCount;
        /// <summary>
        /// 昇降パラメータ　CSV読み込めば基本固定
        /// </summary>
        public YStgParamFix Param { get; private set; }
        /// <summary>
        /// ボード上の指令速度値(deg/sec)
        /// </summary>
        public float YStgSpeed { get; private set; }
        /// <summary>
        /// ボード上のステータス値
        /// </summary>
        public uint YStgSts { get; private set; }
        /// <summary>
        /// ボード上のステータス値：動作中
        /// </summary>
        public bool YStgBusy { get; private set; }
        /// <summary>
        /// ボード上のステータス値：定速動作中
        /// </summary>
        public bool YStgConst { get; private set; }
        /// <summary>
        /// ボード上のステータス値：準備完了
        /// </summary>
        public bool YStgReady { get; private set; }
        /// <summary>
        /// ボード上のステータス値：停止指令入力
        /// </summary>
        public bool YStgStop { get; private set; }
        /// <summary>
        /// ボード上のステータス値：原点正常
        /// </summary>
        public bool YStgOrg { get; private set; }
        /// <summary>
        /// ボード上のステータス値：＋リミット
        /// </summary>
        public bool YStgCwEls { get; private set; }
        /// <summary>
        /// ボード上のステータス値：－リミット
        /// </summary>
        public bool YStgCcwEls { get; private set; }
        /// <summary>
        /// ボード上のステータス値：原点センサ
        /// </summary>
        public bool YStgOls { get; private set; }
        /// <summary>
        /// ボード上のステータス値：減速センサ
        /// </summary>
        public bool YStgDls { get; private set; }
        /// <summary>
        /// ボード上のステータス値：アラーム
        /// </summary>
        public bool YStgAlm { get; private set; }
        /// <summary>
        /// ボード上のエラー情報
        /// </summary>
        public uint YStgErrSts { get; private set; }
        /// <summary>
        /// デバイス番号
        /// </summary>
        public uint DevNum { get; private set; }
        /// <summary>
        /// ボード制御
        /// </summary>
        private readonly IBoardControl _BoardCnt;
        /// <summary>
        /// ボード定期監視
        /// </summary>
        //private YStageStsProvider _BoardPro;
        /// <summary>
        /// ボード表示値送信
        /// </summary>
        private readonly IBoardSender _ValueSender;
        /// <summary>
        /// ボード設定I/F
        /// </summary>
        private readonly IBoardConfig _BoardConf;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public YStageCtrl(IBoardControl board, IBoardSender bsender, IBoardConfig boardconf)
        {

            _BoardConf = boardconf;
            _BoardConf.EndLoadBoardConf += (s, e) =>
            {
                BoardConfig bcf = s as BoardConfig;
                uint tmpdvenum = bcf.Boards.ToList().Find(p => p.BAxis == BoardAxis.YSTG_JIKU).ID;
                if (tmpdvenum == 0) throw new Exception($"{nameof(RotationCtrl)} has exception error");
                DevNum = tmpdvenum;
                Init();
            };
            _BoardConf.RequestParam();

            _BoardCnt = board;

            _ValueSender = bsender;
            _ValueSender.SetTimer(200);
            _ValueSender.NotifyFY += (s, e) =>
            {
                NotifyCount.Invoke(this, new NumUpdateEventArgs(e.NumValue));//位置を通知
            };

            //_BoardSts = bsts;
            //_BoardSts.FYCntChange += (s, e) =>
            //{
            //    FYPosCount = e.Count;

            //    FYPos = _BoardCnt.PlsToDeg((int)FYPosCount, Param.YStgStep, Param.YStgDir);

            //    _ValueSender.PushData(FYPos, nameof(BoardAxis.YSTG_JIKU));

            //    uint encordcntdata = 0;

            //    uint retd = Hicpd530.cp530_rReg(DevNum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.RRCTR2, ref encordcntdata);

            //    FYPosEncoderCount = encordcntdata;
            //};

            //_BoardSts.FYStsChange += (s, e) =>
            //{
            //    //YStgSts = (uint)e.Count;

            //    ////ステータス分割
            //    //uint sts = 0;
            //    //YStgReady = false;
            //    //YStgBusy = false;
            //    //YStgConst = false;
            //    //YStgCwEls = false;
            //    //YStgCcwEls = false;
            //    //YStgOls = false;
            //    //YStgDls = false;
            //    //YStgAlm = false;
            //    //if (Param.YStgMotorType == 1)
            //    //{
            //    //    //準備完了＝ALM無しかつSVONかつ停止中
            //    //    sts = YStgSts & 0x0F01;
            //    //    if (sts == 0x0001)
            //    //    {
            //    //        YStgReady = true;
            //    //    }
            //    //    //動作中＝加速中、減速中、定速動作中
            //    //    sts = YStgSts & 0x0F01;
            //    //    if (sts == 0x0101 || sts == 0x0201 || sts == 0x0401)
            //    //    {
            //    //        YStgBusy = true;
            //    //    }
            //    //    //定速動作中
            //    //    sts = YStgSts & 0x0F01;
            //    //    if (sts == 0x0401)
            //    //    {
            //    //        YStgConst = true;
            //    //    }
            //    //}
            //    //else
            //    //{
            //    //    //準備完了＝ALM無しかつSVONかつ停止中
            //    //    sts = YStgSts & 0x0F00;
            //    //    if (sts == 0x0000)
            //    //    {
            //    //        YStgReady = true;
            //    //    }
            //    //    //動作中＝加速中、減速中、定速動作中
            //    //    sts = YStgSts & 0x0F01;
            //    //    if (sts == 0x0100 || sts == 0x0200 || sts == 0x0400)
            //    //    {
            //    //        YStgBusy = true;
            //    //    }
            //    //    //定速動作中
            //    //    sts = YStgSts & 0x0F00;
            //    //    if (sts == 0x0400)
            //    //    {
            //    //        YStgConst = true;
            //    //    }
            //    //}

            //    //sts = YStgSts & 0x0800;
            //    //if (sts == 0x0800)
            //    //{
            //    //    YStgAlm = true;
            //    //}
            //    //sts = YStgSts & 0x1000;
            //    //if (sts == 0x1000)
            //    //{
            //    //    YStgCwEls = true;
            //    //}
            //    //sts = YStgSts & 0x2000;
            //    //if (sts == 0x2000)
            //    //{
            //    //    YStgCcwEls = true;
            //    //}
            //    //sts = YStgSts & 0x4000;
            //    //if (sts == 0x4000)
            //    //{
            //    //    YStgOls = true;
            //    //}
            //    //sts = YStgSts & 0x8000;
            //    //if (sts == 0x8000)
            //    //{
            //    //    YStgDls = true;
            //    //}
            //};

            //_BoardSts.FYSpdChange += (s, e) =>
            //{
            //   // YStgSpeed = e.Count;
            //};


        }
        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="cont"></param>
        public bool Init()
        {
            Param = new YStgParamFix();

            uint bnum = DevNum;
            int temp = 0;

            uint ret = 0;
            if (ret == 0)
            {//ボードのカウント書込 UP/DOWNカウント値書込み(保存している現在位置に設定する)
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRCTR1, FYPosCount);
            }
            if (ret == 0)
            {//ボードのエンコーダカウント書込 UP/DOWNカウント値書込み(保存している現在位置に設定する)
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRCTR2, FYPosEncoderCount);
            }

            if (ret == 0)
            {//ｺﾝﾊﾟﾚｰﾀﾃﾞｰﾀ設定
                if (Param.YStgLmtNoChk == 0)
                {
                    uint YStgCwLmtPls = _BoardCnt.DegToPls(Param.YStgCwLmtPos, Param.YStgStep, Param.YStgDir);
                    ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRCMP2, YStgCwLmtPls);
                    if (ret == 0)
                    {
                        uint YStgCcwLmtPls = _BoardCnt.DegToPls(Param.YStgCcwLmtPos, Param.YStgStep, Param.YStgDir);
                        ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRCMP1, YStgCcwLmtPls);
                    }
                    Param.YStgEnv4Data = Param.YStgEnv4Data | 0x00005054;
                }
                if (Param.YStgNoSlowMode == 0)
                {
                    uint YStgCrLowPls = _BoardCnt.DegToPls(Param.YStgCrLowPos, Param.YStgStep, Param.YStgDir);
                    ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRCMP3, YStgCrLowPls);
                    if (ret == 0)
                    {
                        uint YStgCrHiPls = _BoardCnt.DegToPls(Param.YStgCrHiPos, Param.YStgStep, Param.YStgDir);
                        ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRCMP4, YStgCrHiPls);
                    }
                    Param.YStgEnv4Data = Param.YStgEnv4Data | 0x74700000;
                    Param.YStgEnvLmOn = Param.YStgEnvLmOn | 0x74700000;
                    Param.YStgEnvLmOf = Param.YStgEnvLmOf | 0x74700000;
                    Param.YStgEnvLmCwOf = Param.YStgEnvLmCwOf | 0x74700000;
                    Param.YStgEnvLmCcwOf = Param.YStgEnvLmCcwOf | 0x74700000;
                }
            }
            if (ret == 0)
            {//補助速度レジスタセット RFA速度
                uint SubSpd = _BoardCnt.SpdToPps(Param.YStgOriginSpdFL, Param.YStgStep, (int)Param.YStgMag);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRFA, SubSpd);
            }
            if (ret == 0)
            {// 速度倍率設定
                ushort Rmg = (ushort)((300 / Param.YStgMag) - 1);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WPRMG, (uint)Rmg);
            }
            if (ret == 0)
            {// レジスタ書込み（減速点:PR5 = 0）
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WPRDP, (int)0x00000000);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定1:RENV1）
                Param.YStgEnv1Data = Param.YStgEnv1Data | (uint)Param.YStgPlsMode;//パルス入力方式設定
                if (Param.YStgElsMode == 1)//ELSで減速停止の場合
                {
                    Param.YStgEnv1Data = Param.YStgEnv1Data | 0x00000008;
                }
                if (Param.YStgMotorType == 1)//サーボの場合は偏差カウンタクリア
                {
                    Param.YStgEnv1Data = Param.YStgEnv1Data | 0x00000800;
                }
                if (Param.YStgDlsPolarity == 0)//DLSがA接点の場合
                {
                    Param.YStgEnv1Data = Param.YStgEnv1Data | 0x00000040;
                }
                if (Param.YStgOrgPolarity == 0)//OLSがA接点の場合
                {
                    Param.YStgEnv1Data = Param.YStgEnv1Data | 0x00000080;
                }
                if (Param.YStgAlmPolarity == 0)//ALMがA接点の場合
                {
                    Param.YStgEnv1Data = Param.YStgEnv1Data | 0x00000200;
                }
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV1, (uint)Param.YStgEnv1Data); ;
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定2:R7）
                Param.YStgEnv2Data = Param.YStgEnv2Data & 0xFFCFFFFF;//逓倍設定
                temp = Param.YStgPlsMult << 20;
                Param.YStgEnv2Data = Param.YStgEnv2Data | (uint)temp;
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV2, (uint)Param.YStgEnv2Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定3:R8）
                Param.YStgEnv3Data = Param.YStgEnv3Data & 0xFFFFFFF0;//原点復帰方式
                Param.YStgEnv3Data = Param.YStgEnv3Data | (uint)Param.YStgOriginMode;
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV3, (uint)Param.YStgEnv3Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定4: RENV4）
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV4, (uint)Param.YStgEnv4Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定5:RENV5）
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV5, (uint)Param.YStgEnv5Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定6:RENV6）
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV6, (uint)Param.YStgEnv6Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定7:RENV7）
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV7, (uint)Param.YStgEnv7Data);
            }
            if (ret == 0)
            {// レジスタ書込み（アップダウンカウント値 = 動作後に設定,減速レート:PR15 = 0）
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WPRDR, (uint)0x00000000);
            }
            // レジスタ書込み（S字区間:PR16 = 0）
            if (ret == 0)
            {
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WPRUS, (int)0x00000000);
            }
            // ELS極性選択書込み
            if (ret == 0)
            {
                byte rdata = 0;
                byte wdata = 0;
                ret = Hicpd530.cp530_rPortB(bnum, 0x80, ref rdata);

                temp = 0x01 << (int)BoardAxis.YSTG_JIKU;
                temp = (temp + 1) * -1;
                temp = temp & rdata;
                wdata = (byte)temp;

                if (Param.YStgLmtPolarity == 0)
                {
                    temp = 0x01 << (int)BoardAxis.YSTG_JIKU;
                    temp = temp | wdata;
                    wdata = (byte)temp;
                }
                ret = Hicpd530.cp530_wPortB(bnum, 0x80, wdata);
            }
            if (ret == 0)
            {//アラームクリア＆サーボON
                Reset();
                YStgReady = true;
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
        /// <param name="rotMode">回転方向</param>
        /// <param name="manualspd">回転速度</param>
        /// <returns></returns>
        public bool Manual(RevMode revmode, float manualspd)
        {
            // 速度のチェック
            if (manualspd > Param.YStgManualLimitSpd || manualspd < Param.YStgSpdFL)
            {   // 指定速度異常
                SendGUIMes = "Setting Speed is uncorrect";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            uint bnum = DevNum;
            uint ret = 0;
            bool result = true;
            YStgStop = false;

            //リミット処理
            if (Param.YStgLmtNoChk == 0)
            {
                if (FYPos < Param.YStgCwLmtPos)
                {
                    if (revmode == RevMode.CW)
                    {
                        if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV4, Param.YStgEnvLmCwOf))
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
                else if (FYPos > Param.YStgCcwLmtPos)
                {
                    if (revmode == RevMode.CCW)
                    {
                        if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV4, Param.YStgEnvLmCcwOf))
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
                    if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV4, Param.YStgEnvLmOn))
                    {   //エラー
                        SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                        StsChanged?.Invoke(this, new EventArgs());
                    }
                }
            }

            //減速処理
            if (Param.YStgNoSlowMode == 0)
            {
                if (FYPos < Param.YStgCrLowPos)
                {
                    if (revmode == RevMode.CCW && manualspd > Param.YStgSlowSpd)
                    {
                        manualspd = Param.YStgSlowSpd;
                    }
                }

                if (FYPos > Param.YStgCrHiPos)
                {
                    if (revmode == RevMode.CW && manualspd > Param.YStgSlowSpd)
                    {
                        manualspd = Param.YStgSlowSpd;
                    }
                }
            }

            //速度データ生成
            ushort flspd = _BoardCnt.SpdToPps(Param.YStgSpdFL, Param.YStgStep, (int)Param.YStgMag);
            ushort fhspd = _BoardCnt.SpdToPps(manualspd, Param.YStgStep, (int)Param.YStgMag);
            ushort rate = _BoardCnt.AccelerationRate(Param.YStgRateTime, flspd, fhspd);
            long pos;
            if (revmode == RevMode.CW || revmode == RevMode.SCAN_CW)
            {
                pos = 134217727L;
            }
            else
            {
                pos = -134217728L;
            }
            int mode = Param.YStgMotorType == 0 ? 0x08008141 : 0x08008341;

            //速度データ書き込み
            ret = _BoardCnt.SetSpdReg(bnum, (int)BoardAxis.YSTG_JIKU, fhspd, flspd, rate, (uint)pos, (uint)mode);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            // コマンドバッファ書込み 高速スタート
            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.STAUD);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }
            if (revmode == RevMode.SCAN_CW || revmode == RevMode.SCAN_CCW)
            {
                //タイムアウト時間設定(加速時間＋100msec)
                int cnt = (int)(Param.YStgRateTime * 1000) + 500;
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
        public bool Index(PosiMode posimode, float indexpos)
        {
            uint bnum = DevNum;
            uint ret = 0;
            float checkpos;
            YStgStop = false;

            if (!YStgReady)
            {
                SendGUIMes = $"Not Ready";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            if (posimode == PosiMode.Abso)
            {
                if (!YStgOrg && Param.YStgOriginNoChk == 0)
                {
                    SendGUIMes = $"Not Ready";
                    StsChanged?.Invoke(this, new EventArgs());
                    return false;
                }
                checkpos = indexpos;
                indexpos = indexpos - FYPos;
            }
            else
            {
                checkpos = FYPos + indexpos;
            }

            if (Param.YStgLmtNoChk == 0)
            {
                if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV4, Param.YStgEnvLmOn))
                {   //エラー
                    SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                    StsChanged?.Invoke(this, new EventArgs());
                }

                if (checkpos > Param.YStgCwLmtPos)
                {
                    SendGUIMes = "CW Limt Over";
                    StsChanged?.Invoke(this, new EventArgs());
                    return false;
                }

                if (checkpos < Param.YStgCcwLmtPos)
                {
                    SendGUIMes = "CCW Limt Over";
                    StsChanged?.Invoke(this, new EventArgs());
                    return false;
                }
            }

            //速度データ生成
            ushort flspd = _BoardCnt.SpdToPps(Param.YStgIndexSpdFL, Param.YStgStep, (int)Param.YStgMag);
            ushort fhspd = _BoardCnt.SpdToPps(Param.YStgIndexSpdFH, Param.YStgStep, (int)Param.YStgMag);
            ushort rate = _BoardCnt.AccelerationRate(Param.YStgIndexRateTime, flspd, fhspd);
            uint pos = _BoardCnt.DegToPls(indexpos, Param.YStgStep, Param.YStgDir);
            int mode = Param.YStgMotorType == 0 ? 0x08008141 : 0x08008341;

            //速度データ書き込み
            ret = _BoardCnt.SetSpdReg(bnum, (int)BoardAxis.YSTG_JIKU, fhspd, flspd, rate, pos, (uint)mode);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            // コマンドバッファ書込み 高速スタート
            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.STAUD);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }
            bool result = false;

            checkpos = Param.YStgInpos * -1;

            if (indexpos <= checkpos || Param.YStgInpos <= indexpos)
            {
                //起動確認
                result = WaitMethod(bnum, TimeSpan.FromMilliseconds(500), WatchMode.Busy);
            }
            //完了待ち
            result = WaitMethod(bnum, TimeSpan.FromMilliseconds(Param.YStgIndexTimeOut), WatchMode.Ready);

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
            if (YStgBusy) YStgStop = true;
            switch (stopmode)
            {
                case (StopMode.Fast):// 即時停止
                    ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.STOP);
                    break;

                case (StopMode.Slow)://減速停止
                    ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.SDSTP);
                    break;
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

            ret = Hicpd530.cp530_rReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.RREST, ref errsts);
            YStgErrSts = errsts;
            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.SVRSTON);   // サーボリセットON

            Task.WaitAll(Task.Delay(100));//100mce

            //サーボON
            if (Param.YStgMotorType == 1)
            {
                ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.SVON);   // サーボON
            }

            bool result = WaitMethod(bnum, TimeSpan.FromMilliseconds(1000), WatchMode.Ready);

            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.SVRSTOFF);   // サーボリセットOFF

            YStgOrg = false;
            if (ret == 0) return true;

            return false;

        }
        /// <summary>
        /// 原点復帰
        /// </summary>
        public bool Origin(IProgress<CalProgress> prog, CancellationTokenSource ctoken)
        {
            uint ret = 0;
            YStgStop = false;
            bool result = false;
            uint bnum = DevNum;
            ushort flspd = _BoardCnt.SpdToPps(Param.YStgOriginSpdFL, Param.YStgStep, (int)Param.YStgMag);
            ushort fhspd = _BoardCnt.SpdToPps(Param.YStgOriginSpdFH, Param.YStgStep, (int)Param.YStgMag);
            ushort rate = _BoardCnt.AccelerationRate(Param.YStgRateTime, flspd, fhspd);
            uint pos = _BoardCnt.DegToPls(Param.YStgOriginShift, Param.YStgStep, Param.YStgDir);
            //+方向原点サーチ 0x08008215;	// +方向原点復帰
            //-方向原点サーチ 0x0800821D;	// -方向原点復帰
            uint mode = 0;
            //原点センサ抜け出し
            if (Param.YStgOriginMode == 2 & YStgOls)
            {
                result = Index(PosiMode.Rela, Param.YStgOriginShift);
                if (!result || YStgStop) return false;
            }
            if (Param.YStgMotorType == 1)
            {
                mode = Param.YStgOriginDir == 0 ? (uint)0x08008315 : (uint)0x0800831D;
            }
            else
            {
                mode = Param.YStgOriginDir == 0 ? (uint)0x08008115 : (uint)0x0800811D;
            }
            if (!YStgReady)
            {
                SendGUIMes = "Not Ready";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            //速度データ書き込み
            ret = _BoardCnt.SetSpdReg(bnum, (int)BoardAxis.YSTG_JIKU, fhspd, flspd, rate, pos, mode);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            //高速スタート
            if (ret != Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.STAUD))
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            //起動確認
            result = WaitMethod(bnum, TimeSpan.FromMilliseconds(1000), WatchMode.Busy);
            YStgOrg = false;
            //DIOに原点復帰中を通知
            //NotifyRestOperation?.Invoke(this, new ChkChangeEventArgs(true));
            //原点復帰完了待ち
            result = WaitMethod(bnum, TimeSpan.FromMilliseconds(Param.YStgOriginTimeOut), WatchMode.Ready, ctoken);

            if (result && !YStgStop)
            {//ボードのカウント書込 UP/DOWNカウント値書込み(原点位置を設定)
                uint orgpospls = _BoardCnt.DegToPls(Param.YStgOriginPos, Param.YStgStep, Param.YStgDir);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRCTR1, orgpospls);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRCTR2, orgpospls);
                YStgOrg = true;
                result = Index(PosiMode.Rela, Param.YStgOriginOffset);
                //DIOに原点復帰完了を通知
                //NotifyRestOperation?.Invoke(this, new ChkChangeEventArgs(false));
            }

            NotifyCount.Invoke(this, new NumUpdateEventArgs(FYPos));//位置を通知


            return result;
        }

        /// <summary>
        /// 待ち処理
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="bnum"></param>
        /// <param name="timeout">タイムアウト時間</param>
        /// <returns></returns>
        private bool WaitMethod(uint bnum, TimeSpan timeout, WatchMode watchmode,CancellationTokenSource ui_cancel = null)
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
                    if (watchmode == WatchMode.Ready && YStgReady)//指定した状態になったら抜ける
                    {
                        break;
                    }
                    if (watchmode == WatchMode.Busy && YStgBusy)///指定した状態になったら抜ける
                    {
                        break;
                    }
                    if (watchmode == WatchMode.Const && YStgConst)///指定した状態になったら抜ける
                    {
                        break;
                    }
                    if (YStgStop)
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
                BoardParam.Default.YStgEncoderCount = FYPosEncoderCount;
                BoardParam.Default.YStgCount = FYPosCount;
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
        public int GetScale() => Param.YStgScale;
        /// <summary>
        /// 微調X軸の最大値
        /// </summary>
        /// <returns></returns>
        public float GetMax() => Param.YStgMaxValue;
        /// <summary>
        /// 微調X軸の最小値
        /// </summary>
        /// <returns></returns>
        public float GetMin() => Param.YStgMinValue;

        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            //_BoardPro.Dispose();
        }
    }

    /// <summary>
    /// 微調Y軸 I/F
    /// </summary>
    public interface IYStageCtrl
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
        /// 微調Y軸 原点復帰
        /// </summary>
        /// <param name="mesbox"></param>
        bool Origin(IProgress<CalProgress> prog, CancellationTokenSource ctoken);
        /// <summary>
        /// 微調Y軸 ストップ
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
        /// 微調Y軸 インデックス移動
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="posimode"></param>
        /// <param name="indexpos"></param>
        /// <returns></returns>
        bool Index(PosiMode posimode, float indexpos);
        /// <summary>
        /// 微調Yの有効数値
        /// </summary>
        /// <returns></returns>
        int GetScale();
        /// <summary>
        /// 微調Y軸の最大値
        /// </summary>
        /// <returns></returns>
        float GetMax();
        /// <summary>
        /// 微調Y軸の最小値
        /// </summary>
        /// <returns></returns>
        float GetMin();
    }
}