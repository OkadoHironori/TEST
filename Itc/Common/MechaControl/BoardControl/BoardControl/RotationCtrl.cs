
using Board.BoardControl;
using Itc.Common.Event;
using Itc.Common.Extensions;
using Itc.Common.TXEnum;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Board.BoardControl
{
    using CountChangeEventHandler = Action<object, NumUpdateEventArgs>;
    using ChkChangeEventHandler = Action<object, ChkChangeEventArgs>;
    /// <summary>
    /// ボードによる回転
    /// </summary>
    public class RotationCtrl : IRotationCtrl, IDisposable
    {
        /// <summary>
        /// 回転角度の通知
        /// </summary>
        public event CountChangeEventHandler NotifyCount;
        /// <summary>
        /// リセット中（原点復帰中）かどうか通知
        /// </summary>
        public event ChkChangeEventHandler NotifyRestOperation;
        /// <summary>
        /// 状態変化通知
        /// </summary>
        public event EventHandler StsChanged;
        /// <summary>
        /// パラメータ要求イベント
        /// </summary>
        public event EventHandler RequestParamEvent;
        /// <summary>
        /// ボード上の回転角度 (deg)
        /// </summary>
        public float RotPos { get; private set; }
        /// <summary>
        /// ボード上のカウント値(PLS)
        /// </summary>
        public uint RotPosCount { get; private set; } = BoardParam.Default.RotCount;
        /// <summary>
        /// ボード上のエンコード後のカウント
        /// </summary>
        public uint RotPosEncoderCount { get; private set; } = BoardParam.Default.RotEncoderCount;
        /// <summary>
        /// 回転パラメータ　CSV読み込めば基本固定
        /// </summary>
        public RotParamFix Param { get; private set; } = new RotParamFix();
        /// <summary>
        /// ボード上の指令速度値(deg/sec)
        /// </summary>
        public float RotSpeed { get; private set; }
        /// <summary>
        /// ボード上のステータス値
        /// </summary>
        public uint RotSts { get; private set; }
        /// <summary>
        /// ボード上のステータス値：動作中
        /// </summary>
        public bool RotBusy { get; private set; }
        /// <summary>
        /// ボード上のステータス値：定速動作中
        /// </summary>
        public bool RotConst { get; private set; }
        /// <summary>
        /// ボード上のステータス値：準備完了
        /// </summary>
        public bool RotReady { get; private set; }
        /// <summary>
        /// ボード上のステータス値：停止指令入力
        /// </summary>
        public bool RotStop { get; private set; }
        /// <summary>
        /// ボード上のステータス値：原点正常
        /// </summary>
        public bool RotOrg { get; private set; }
        /// <summary>
        /// ボード上のステータス値：＋リミット
        /// </summary>
        public bool RotCwEls { get; private set; }
        /// <summary>
        /// ボード上のステータス値：－リミット
        /// </summary>
        public bool RotCcwEls { get; private set; }
        /// <summary>
        /// ボード上のステータス値：原点センサ
        /// </summary>
        public bool RotOls { get; private set; }
        /// <summary>
        /// ボード上のステータス値：減速センサ
        /// </summary>
        public bool RotDls { get; private set; }
        /// <summary>
        /// ボード上のステータス値：アラーム
        /// </summary>
        public bool RotAlm { get; private set; }
        /// <summary>
        /// ボード上のエラー情報
        /// </summary>
        public uint RotErrSts { get; private set; }
        /// <summary>
        /// GUIへのメッセージ送信
        /// </summary>
        public string SendGUIMes { get; private set; }
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
        /// ボード設定
        /// </summary>
        private readonly IBoardConfig _BoardConf;
        /// <summary>
        /// 
        /// </summary>
        private readonly IBoardProvider _Provider;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RotationCtrl(IBoardConfig boardconf, IBoardProvider provider, IBoardControl control, IBoardSender boardSender)
        {
            _BoardCnt = control;


            _BoardConf = boardconf;
            _BoardConf.EndLoadBoardConf += (s, e) =>
            {
                BoardConfig bcf = s as BoardConfig;
                uint tmpdvenum = bcf.Boards.ToList().Find(p => p.BAxis == BoardAxis.ROT_JIKU).ID;
                if (tmpdvenum == 0) throw new Exception($"{nameof(RotationCtrl)} has exception error");
                DevNum = tmpdvenum;
                Init();
            };
            _BoardConf.RequestParam();

            _ValueSender = boardSender;
            _ValueSender.SetTimer(200);
            _ValueSender.NotifyRot += (s, e) =>
            {
                NotifyCount.Invoke(this, new NumUpdateEventArgs(e.NumValue));//角度(X.XXdeg)を通知
                Debug.WriteLine($"{nameof(UpDownCtrl)}の位置は{e.NumValue.ToString()}時間{DateTime.Now.TimeOfDay}");
            };

            _Provider = provider;
            _Provider.PropertyChanged += (s, e) =>
            {
                var bpservice = s as BoardProvider;
                //DateTime dt = DateTime.Now;

                switch (e.PropertyName)
                {
                    case (nameof(BoardProvider.RotCount)):

                        RotPosCount = bpservice.RotCount;
                        var rotposi = (int)RotPosCount;
                        if (!rotposi.InRange(-134210000, 134210000))//ボードカウント範囲
                        {
                            Stop(StopMode.Fast);
                            throw new Exception($"{RotPosCount.ToString()}ボードカウント範囲を超えました{Environment.NewLine}リセットしてください");
                        }
                        RotPos = _BoardCnt.PlsToDeg((int)RotPosCount, Param.RotStep, Param.RotDir);
                        float dispRot = RotPos > 0 ? (-360.0F + RotPos) : RotPos;//表示角度を-360～0degにしている
                        _ValueSender.PushData(dispRot, nameof(BoardAxis.ROT_JIKU));


                        //Debug.WriteLine($"{dt.TimeOfDay}  {e.PropertyName}ボード角度{RotPos.ToString("0.00")}表示角度{dispRot.ToString("0.00")}");


                        uint encordcntdata = 0;
                        uint retd = Hicpd530.cp530_rReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.RRCTR2, ref encordcntdata);

                        RotPosEncoderCount = encordcntdata;

                        break;
                    case (nameof(BoardProvider.RotSpeed)):

                        RotSpeed = _BoardCnt.PlsToDeg((int)bpservice.RotSpeed, Param.RotStep, 1) * Param.RotMag;
                        //Debug.WriteLine($"{dt.TimeOfDay}  {e.PropertyName}速度{RotSpeed.ToString("0.00")}");
                        break;

                    case (nameof(BoardProvider.RotSts)):

                        RotSts = (uint)bpservice.RotSts;

                        //ステータス分割
                        uint sts = 0;
                        RotReady = false;
                        RotBusy = false;
                        RotConst = false;
                        RotCwEls = false;
                        RotCcwEls = false;
                        RotOls = false;
                        RotDls = false;
                        RotAlm = false;
                        if (Param.RotMotorType == 1)
                        {
                            //準備完了＝ALM無しかつSVONかつ停止中
                            sts = RotSts & 0x0F01;
                            if (sts == 0x0001)
                            {
                                RotReady = true;
                            }
                            //動作中＝加速中、減速中、定速動作中
                            sts = RotSts & 0x0F01;
                            if (sts == 0x0101 || sts == 0x0201 || sts == 0x0401)
                            {
                                RotBusy = true;
                            }
                            //定速動作中
                            sts = RotSts & 0x0F01;
                            if (sts == 0x0401)
                            {
                                RotConst = true;
                            }
                        }
                        else
                        {
                            //準備完了＝ALM無しかつ停止中
                            sts = RotSts & 0x0F00;
                            if (sts == 0x0000)
                            {
                                RotReady = true;
                            }
                            //動作中＝加速中、減速中、定速動作中
                            sts = RotSts & 0x0F00;
                            if (sts == 0x0100 || sts == 0x0200 || sts == 0x0400)
                            {
                                RotBusy = true;
                            }
                            //定速動作中
                            sts = RotSts & 0x0F00;
                            if (sts == 0x0400)
                            {
                                RotConst = true;
                            }
                        }

                        sts = RotSts & 0x0800;
                        if (sts == 0x0800)
                        {
                            RotAlm = true;
                        }
                        sts = RotSts & 0x1000;
                        if (sts == 0x1000)
                        {
                            RotCwEls = true;
                        }
                        sts = RotSts & 0x2000;
                        if (sts == 0x2000)
                        {
                            RotCcwEls = true;
                        }
                        sts = RotSts & 0x4000;
                        if (sts == 0x4000)
                        {
                            RotOls = true;
                        }
                        sts = RotSts & 0x8000;
                        if (sts == 0x8000)
                        {
                            RotDls = true;
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
        public bool Init()
        {
            int temp = 0;
            uint ret = 0;
            if (ret == 0)
            {//ボードのカウント書込 UP/DOWNカウント値書込み(保存している現在位置に設定する)
                ret = Hicpd530.cp530_wReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRCTR1, RotPosCount);
            }
            if (ret == 0)
            {//ボードのエンコーダカウント書込 UP/DOWNカウント値書込み(保存している現在位置に設定する)  
                ret = Hicpd530.cp530_wReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRCTR2, RotPosEncoderCount);
            }

            if (ret == 0)
            {//ｺﾝﾊﾟﾚｰﾀﾃﾞｰﾀ設定
                if (Param.RotNoSlowMode == 1)
                {
                    uint RotCwLmtPls = _BoardCnt.DegToPls(Param.RotCwLmtPos, Param.RotStep, Param.RotDir);
                    ret = Hicpd530.cp530_wReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRCMP2, RotCwLmtPls);
                    if (ret == 0)
                    {
                        uint RotCcwLmtPls = _BoardCnt.DegToPls(Param.RotCcwLmtPos, Param.RotStep, Param.RotDir);
                        ret = Hicpd530.cp530_wReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRCMP1, RotCcwLmtPls);
                    }
                }
                else
                {
                    uint RotCrLowPls = _BoardCnt.DegToPls(Param.RotCrLowPos, Param.RotStep, Param.RotDir);
                    ret = Hicpd530.cp530_wReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRCMP1, RotCrLowPls);
                    if (ret == 0)
                    {
                        uint RotCrHiPls = _BoardCnt.DegToPls(Param.RotCrHiPos, Param.RotStep, Param.RotDir);
                        ret = Hicpd530.cp530_wReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRCMP2, RotCrHiPls);
                    }
                }
            }
            if (ret == 0)
            {//補助速度レジスタセット RFA速度
                uint SubSpd = _BoardCnt.SpdToPps(Param.RotOriginSpdFL, Param.RotStep, (int)Param.RotMag);
                ret = Hicpd530.cp530_wReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRFA, SubSpd);
            }
            if (ret == 0)
            {// 速度倍率設定
                ushort Rmg = (ushort)((300 / Param.RotMag) - 1);
                ret = Hicpd530.cp530_wReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WPRMG, (uint)Rmg);
            }
            if (ret == 0)
            {// レジスタ書込み（減速点:PR5 = 0）
                ret = Hicpd530.cp530_wReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WPRDP, (int)0x00000000);
            }

            if (ret == 0)
            { // レジスタ書込み（環境設定1:RENV1）
                Param.RotEnv1Data = Param.RotEnv1Data | (uint)Param.RotPlsMode;//パルス入力方式設定
                if (Param.RotElsMode == 1)//ELSで減速停止の場合
                {
                    Param.RotEnv1Data = Param.RotEnv1Data | 0x00000008;
                }
                if (Param.RotMotorType == 1)//サーボの場合は偏差カウンタクリア
                {
                    Param.RotEnv1Data = Param.RotEnv1Data | 0x00000800;
                }
                if (Param.RotDlsPolarity == 0)//DLSがA接点の場合
                {
                    Param.RotEnv1Data = Param.RotEnv1Data | 0x00000040;
                }
                if (Param.RotOrgPolarity == 0)//OLSがA接点の場合
                {
                    Param.RotEnv1Data = Param.RotEnv1Data | 0x00000080;
                }
                if (Param.RotAlmPolarity == 0)//ALMがA接点の場合
                {
                    Param.RotEnv1Data = Param.RotEnv1Data | 0x00000200;
                }
                ret = Hicpd530.cp530_wReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRENV1, (uint)Param.RotEnv1Data);
            }

            if (ret == 0)
            {// レジスタ書込み（環境設定2:RENV2）
                Param.RotEnv2Data = Param.RotEnv2Data & 0xFFCFFFFF;//逓倍設定
                temp = Param.RotPlsMult << 20;
                Param.RotEnv2Data = Param.RotEnv2Data | (uint)temp;
                ret = Hicpd530.cp530_wReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRENV2, (uint)Param.RotEnv2Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定3:RENV3）
                Param.RotEnv3Data = Param.RotEnv3Data & 0xFFFFFFF0;
                Param.RotEnv3Data = Param.RotEnv3Data | (uint)Param.RotOriginMode;//原点復帰方式
                ret = Hicpd530.cp530_wReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRENV3, (uint)Param.RotEnv3Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定4:RENV4）
                ret = Hicpd530.cp530_wReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRENV4, (uint)Param.RotEnv4Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定5:RENV5）
                ret = Hicpd530.cp530_wReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRENV5, (uint)Param.RotEnv5Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定6:RENV6）
                ret = Hicpd530.cp530_wReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRENV6, (uint)Param.RotEnv6Data);
            }
            if (ret == 0)
            {// レジスタ書込み（環境設定7:RENV7）
                ret = Hicpd530.cp530_wReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRENV7, (uint)Param.RotEnv7Data);
            }
            if (ret == 0)
            {// レジスタ書込み（アップダウンカウント値 = 動作後に設定,減速レート:PR15 = 0）
                ret = Hicpd530.cp530_wReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WPRDR, (uint)0x00000000);
            }
            // レジスタ書込み（S字区間:PR16 = 0）
            if (ret == 0)
            {
                ret = Hicpd530.cp530_wReg(DevNum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WPRUS, (int)0x00000000);
            }
            // ELS極性選択書込み
            if (ret == 0)
            {

                byte rdata = 0;
                byte wdata = 0;
                ret = Hicpd530.cp530_rPortB(DevNum, 0x80, ref rdata);

                temp = 0x01 << (int)BoardAxis.ROT_JIKU;
                temp = (temp + 1) * -1;
                temp = temp & rdata;
                wdata = (byte)temp;

                if (Param.RotLmtPolarity == 0)
                {
                    temp = 0x01 << (int)BoardAxis.ROT_JIKU;
                    temp = temp | wdata;
                    wdata = (byte)temp;
                }
                ret = Hicpd530.cp530_wPortB(DevNum, 0x80, wdata);
            }

            if (ret == 0)
            {//アラームクリア＆サーボON
                Reset();
                RotReady = true;
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
            if (manualspd > Param.RotManualLimitSpd || manualspd < Param.RotSpdFL)
            {   // 指定速度異常
                SendGUIMes ="Setting Speed is uncorrect";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }
                       
            uint bnum = DevNum;
            uint ret = 0;
            bool result = true;
            RotStop = false;

            //リミット処理
            if (Param.RotLmtNoChk == 0)
            {
                if (RotPos < Param.RotCwLmtPos)
                {
                    if (revmode == RevMode.CW)
                    {
                        if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRENV4, Param.RotEnvLmCwOf))
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
                else if (RotPos > Param.RotCcwLmtPos)
                {
                    if (revmode == RevMode.CCW)
                    {
                        if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRENV4, Param.RotEnvLmCcwOf))
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
                    if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRENV4, Param.RotEnvLmOn))
                    {   //エラー
                        SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                        StsChanged?.Invoke(this, new EventArgs());
                    }
                }
            }
            //減速処理
            if (Param.RotNoSlowMode == 0)
            {
                if (RotPos < Param.RotCrLowPos)
                {
                    if (revmode == RevMode.CCW && manualspd > Param.RotSlowSpd)
                    {
                        manualspd = Param.RotSlowSpd;
                    }
                }

                if (RotPos > Param.RotCrHiPos)
                {
                    if (revmode == RevMode.CW && manualspd > Param.RotSlowSpd)
                    {
                        manualspd = Param.RotSlowSpd;
                    }
                }
            }
            //速度データ生成
            ushort flspd = _BoardCnt.SpdToPps(Param.RotSpdFL, Param.RotStep, (int)Param.RotMag);
            ushort fhspd = _BoardCnt.SpdToPps(manualspd, Param.RotStep, (int)Param.RotMag);
            ushort rate = _BoardCnt.AccelerationRate(Param.RotRateTime, flspd, fhspd);
            long pos;
            if (revmode == RevMode.CW || revmode == RevMode.SCAN_CW)
            {
                pos = 134217727L;
            }
            else
            {
                pos = -134217728L;
            }

            int mode = Param.RotMotorType == 0 ? 0x08008141 : 0x08008341;

            //速度データ書き込み
            ret = _BoardCnt.SetSpdReg(bnum, (int)BoardAxis.ROT_JIKU, fhspd, flspd, rate, (uint)pos, (uint)mode);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            // コマンドバッファ書込み 高速スタート
            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.ROT_JIKU, Cp530l1a.STAUD);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            if (revmode == RevMode.SCAN_CW || revmode == RevMode.SCAN_CCW)
            {
                //タイムアウト時間設定(加速時間＋100msec)
                int cnt = (int)(Param.RotRateTime * 1000) + 500;
                result = WaitMethod(bnum, TimeSpan.FromMilliseconds(cnt), WatchMode.Const);

                if (!result)
                {
                    Stop(StopMode.Fast);
                }
            }
            return result;
        }

        /// <summary>
        /// 
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

            RotStop = false;

            if (!RotReady)
            {

                SendGUIMes = $"Not Ready";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }
            
            if (posimode == PosiMode.Abso)
            {
                if (!RotOrg && Param.RotOriginNoChk ==0)
                {
                    SendGUIMes = $"Not Ready";
                    StsChanged?.Invoke(this, new EventArgs());
                    return false;
                }
                checkpos = indexpos;
                indexpos = indexpos - RotPos;
            }
            else
            {
                checkpos = RotPos + indexpos;
            }

            if (Param.RotLmtNoChk == 0)
            {
                if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRENV4, Param.RotEnvLmOn))
                {   //エラー
                    SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                    StsChanged?.Invoke(this, new EventArgs());

                }

                if (checkpos > Param.RotCwLmtPos)
                {
                    SendGUIMes = $"CW Limt Over";
                    StsChanged?.Invoke(this, new EventArgs());
                    return false;
                }

                if (checkpos < Param.RotCcwLmtPos)
                {
                    SendGUIMes = $"CCW Limt Over";
                    StsChanged?.Invoke(this, new EventArgs());
                    return false;
                }
            }

            //速度データ生成
            ushort flspd = _BoardCnt.SpdToPps(Param.RotIndexSpdFL, Param.RotStep, (int)Param.RotMag);
            ushort fhspd = _BoardCnt.SpdToPps(Param.RotIndexSpdFH, Param.RotStep, (int)Param.RotMag);
            ushort rate = _BoardCnt.AccelerationRate(Param.RotIndexRateTime, flspd, fhspd);
            uint pos = _BoardCnt.DegToPls(indexpos, Param.RotStep, Param.RotDir);
            int mode = Param.RotMotorType == 0 ? 0x08008141 : 0x08008341;

            //速度データ書き込み
            ret = _BoardCnt.SetSpdReg(bnum, (int)BoardAxis.ROT_JIKU, fhspd, flspd, rate, pos, (uint)mode);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            // コマンドバッファ書込み 高速スタート
            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.ROT_JIKU, Cp530l1a.STAUD);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
            }
            bool result = false;

            checkpos = Param.RotInpos * -1;

            if (indexpos <= checkpos || Param.RotInpos <= indexpos)
            {
                //起動確認
                result = WaitMethod(bnum, TimeSpan.FromMilliseconds(500), WatchMode.Busy);
            }
            //完了待ち
            result = WaitMethod(bnum, TimeSpan.FromMilliseconds(Param.RotIndexTimeOut), WatchMode.Ready, cts);
            
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
            if (RotBusy) RotStop = true;
            switch (stopmode)
            {
                case (StopMode.Fast):// 即時停止
                    ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.ROT_JIKU, Cp530l1a.STOP);
                    break;
                case (StopMode.Slow)://減速停止
                    ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.ROT_JIKU, Cp530l1a.SDSTP);
                    break;
                default:
                    //ありえないが、とりあえず即時停止
                    ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.ROT_JIKU, Cp530l1a.STOP);
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
           
            ret = Hicpd530.cp530_rReg(bnum, (int)BoardAxis.ROT_JIKU, Cp530l1a.RREST,ref errsts);
            RotErrSts = errsts;
            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.ROT_JIKU, Cp530l1a.SVRSTON);   // サーボリセットON

            Task.WaitAll(Task.Delay(100));//100mce

            //サーボON
            if (Param.RotMotorType == 1)
            {
                ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.ROT_JIKU, Cp530l1a.SVON);   // サーボON
            }

            bool result = WaitMethod(bnum, TimeSpan.FromMilliseconds(1000), WatchMode.Ready);

            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.ROT_JIKU, Cp530l1a.SVRSTOFF);   // サーボリセットOFF

            RotOrg = false;
            if (ret == 0) return true;

            return false;

        }
        /// <summary>
        /// 原点復帰
        /// </summary>
        public bool Origin(CancellationTokenSource ctoken)
        {
            uint ret = 0;
            RotStop = false;
            bool result = false;
            uint bnum = DevNum;
            ushort flspd = _BoardCnt.SpdToPps(Param.RotOriginSpdFL, Param.RotStep, (int)Param.RotMag);
            ushort fhspd = _BoardCnt.SpdToPps(Param.RotOriginSpdFH, Param.RotStep, (int)Param.RotMag);
            ushort rate = _BoardCnt.AccelerationRate(Param.RotRateTime, flspd, fhspd);
            uint pos = _BoardCnt.DegToPls(Param.RotOriginShift, Param.RotStep, Param.RotDir);
            //+方向原点サーチ 0x08008215;	// +方向原点復帰
            //-方向原点サーチ 0x0800821D;	// -方向原点復帰
            uint mode = 0;

            //原点センサ抜け出し
            if (Param.RotOriginMode == 2 & RotOls)
            {
                result = Index(PosiMode.Rela, Param.RotOriginShift, ctoken);
                if (!result || RotStop) return false;
            }
            if (Param.RotMotorType == 1)
            {
                mode = Param.RotOriginDir == 0 ? (uint)0x08008315 : (uint)0x0800831D;
            }
            else
            {
                mode = Param.RotOriginDir == 0 ? (uint)0x08008115 : (uint)0x0800811D;
            }

            if (!RotReady)
            {
                SendGUIMes = $"Not Ready";
                StsChanged?.Invoke(this, new EventArgs());

                return false;
            }

            if (Param.RotLmtNoChk == 0)
            {
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRENV4, Param.RotEnvLmOn);
                if (ret != 0)
                {
                    SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                    StsChanged?.Invoke(this, new EventArgs());

                    return false;
                }
            }

            //速度データ書き込み
            ret = _BoardCnt.SetSpdReg(bnum, (int)BoardAxis.ROT_JIKU, fhspd, flspd, rate, pos, mode);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());

                return false;
            }

            //高速スタート
            if (ret != Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.ROT_JIKU, Cp530l1a.STAUD))
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }
            //DIOに原点復帰中を通知
            NotifyRestOperation?.Invoke(this, new ChkChangeEventArgs(true));

            //起動確認
            result = WaitMethod(bnum, TimeSpan.FromMilliseconds(1000), WatchMode.Busy);
            RotOrg = false;
            
            //原点復帰完了待ち
            result = WaitMethod(bnum, TimeSpan.FromMilliseconds(Param.RotOriginTimeOut), WatchMode.Ready, ctoken);

            if (result && !RotStop)
            {//ボードのカウント書込 UP/DOWNカウント値書込み(原点位置を設定)
                uint orgpospls = _BoardCnt.DegToPls(Param.RotOriginPos, Param.RotStep, Param.RotDir);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRCTR1, orgpospls);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.ROT_JIKU, Cp530l1a.WRCTR2, orgpospls);
                RotOrg = true;
                result = Index(PosiMode.Rela, Param.RotOriginOffset, ctoken);
                //DIOに原点復帰完了を通知
                NotifyRestOperation?.Invoke(this, new ChkChangeEventArgs(false));
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

                    if (ui_cancel!=null && ui_cancel.IsCancellationRequested)
                    {
                        this.Stop(StopMode.Fast);
                        break;
                    }
                    //ushort stsret = 0;
                    //uint ret = 0;
                    //ret = Hicpd530.cp530_rMstsW(bnum, (int)BoardAxis.ROT_JIKU, ref stsret);
                    if (watchmode == WatchMode.Ready && RotReady)//指定した状態になったら抜ける
                    {
                        break;
                    }
                    if (watchmode == WatchMode.Busy && RotBusy)///指定した状態になったら抜ける
                    {
                        break;
                    }
                    if (watchmode == WatchMode.Const && RotConst)///指定した状態になったら抜ける
                    {
                        break;
                    }
                    if (RotStop)//停止された場合は抜ける
                    {
                        break;
                    }

                    if (com != null)
                    {
                        int tmpPercent = (int)Math.Min(Math.Abs(RotPos - com.Target) / (float)com.Total * 100, 100);
                        com.prog.Report(new CalProgress { Percent = 0, Status = $"{com.Message} {tmpPercent.ToString()} %"});
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
                BoardParam.Default.RotEncoderCount = RotPosEncoderCount;
                BoardParam.Default.RotCount = RotPosCount;
                BoardParam.Default.Save();
            }

            //UIキャンセルの場合はflase
            if(ui_cancel != null && ui_cancel.IsCancellationRequested)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// 回転の有効桁数 + 0.01 IncrementValue 
        /// </summary>
        /// <returns></returns>
        public int GetRotScale()=>Param.RotScale;
        /// <summary>
        /// 回転の最大角度
        /// </summary>
        /// <returns></returns>
        public float GetRotMax()=> Param.RotMaxValue;
        /// <summary>
        /// 回転の最小角度
        /// </summary>
        /// <returns></returns>
        public float GetRotMin()=> Param.RotMinValue;
        /// <summary>
        /// 回転の最大角度
        /// </summary>
        /// <returns></returns>
        public float GetRotSpeedMax() => Param.RotManualLimitSpd;
        /// <summary>
        /// 回転の最小角度
        /// </summary>
        /// <returns></returns>
        public float GetRotSpeedMin() => Param.RotSpdFL;
        /// <summary>
        /// パラメータ要求
        /// </summary>  
        public void RequestParam()
            => RequestParamEvent?.Invoke(this, new EventArgs());
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            if(RotBusy)
            {
                Stop(StopMode.Fast);
            }
        }
    }
    /// <summary>
    /// ボード回転
    /// </summary>
    public interface IRotationCtrl
    {
        /// <summary>
        /// 回転角度の通知
        /// </summary>
        event CountChangeEventHandler NotifyCount;
        /// <summary>
        /// リセット中（原点復帰中）かどうか通知
        /// </summary>
        event ChkChangeEventHandler NotifyRestOperation;
        /// <summary>
        /// 状態変化通知
        /// </summary>
        event EventHandler StsChanged;
        /// <summary>
        /// パラメータ要求イベント
        /// </summary>
        event EventHandler RequestParamEvent;
        /// <summary>
        /// 指定回転
        /// </summary>
        bool Manual(RevMode rotmode, float manualspeed);
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
        bool Index(PosiMode posimode, float indexpos, CancellationTokenSource cts);
        /// <summary>
        /// 回転の有効桁数
        /// </summary>
        /// <returns></returns>
        int GetRotScale();
        /// <summary>
        /// 回転の最大角度
        /// </summary>
        /// <returns></returns>
        float GetRotMax();
        /// <summary>
        /// 回転の最小角度
        /// </summary>
        /// <returns></returns>
        float GetRotMin();
        /// <summary>
        /// 回転の最大速度
        /// </summary>
        /// <returns></returns>
        float GetRotSpeedMax();
        /// <summary>
        /// 回転の最小速度
        /// </summary>
        /// <returns></returns>
        float GetRotSpeedMin();
        /// <summary>
        /// パラメータ要求
        /// </summary>
        void RequestParam();
    }
}
