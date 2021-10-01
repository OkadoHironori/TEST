
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
    /// �{�[�h�ɂ�鏸�~
    /// </summary>
    public class XStageCtrl : IXStageCtrl, IDisposable
    {
        /// <summary>
        /// ���~�ʒu�̒ʒm
        /// </summary>
        public event CountChangeEventHandler NotifyCount;
        /// <summary>
        /// ��ԕω��ʒm
        /// </summary>
        public event EventHandler StsChanged;
        /// <summary>
        /// GUI�ւ̃��b�Z�[�W���M
        /// </summary>
        public string SendGUIMes { get; private set; }
        /// <summary>
        /// �{�[�h��̏��~�ʒu (mm)
        /// </summary>
        public float FXPos { get; private set; }
        /// <summary>
        /// �{�[�h��̃J�E���g�l(PLS)
        /// </summary>
        public uint FXPosCount { get; private set; } = BoardParam.Default.XStgCount;
        /// <summary>
        /// �{�[�h��̃G���R�[�h��̃J�E���g
        /// </summary>
        public uint FXPosEncoderCount { get; private set; } = BoardParam.Default.XStgEncoderCount;
        /// <summary>
        /// ���~�p�����[�^�@CSV�ǂݍ��߂Ί�{�Œ�
        /// </summary>
        public XStgParamFix Param { get; private set; } = new XStgParamFix();
        /// <summary>
        /// �{�[�h��̎w�ߑ��x�l(deg/sec)
        /// </summary>
        public float XStgSpeed { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l
        /// </summary>
        public uint XStgSts { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F���쒆
        /// </summary>
        public bool XStgBusy { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F�葬���쒆
        /// </summary>
        public bool XStgConst { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F��������
        /// </summary>
        public bool XStgReady { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F��~�w�ߓ���
        /// </summary>
        public bool XStgStop { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F���_����
        /// </summary>
        public bool XStgOrg { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F�{���~�b�g
        /// </summary>
        public bool XStgCwEls { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F�|���~�b�g
        /// </summary>
        public bool XStgCcwEls { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F���_�Z���T
        /// </summary>
        public bool XStgOls { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F�����Z���T
        /// </summary>
        public bool XStgDls { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F�A���[��
        /// </summary>
        public bool XStgAlm { get; private set; }
        /// <summary>
        /// �{�[�h��̃G���[���
        /// </summary>
        public uint XStgErrSts { get; private set; }
        /// <summary>
        /// �f�o�C�X�ԍ�
        /// </summary>
        public uint DevNum { get; private set; }
        /// <summary>
        /// �{�[�h����
        /// </summary>
        private readonly IBoardControl _BoardCnt;
        /// <summary>
        /// �{�[�h�\���l���M
        /// </summary>
        private readonly IBoardSender _ValueSender;
        /// <summary>
        /// �{�[�h���I/F
        /// </summary>
        private readonly IBoardProvider _Provider;
        /// <summary>
        /// �{�[�h�ݒ�I/F
        /// </summary>
        private readonly IBoardConfig _BoardConf;
        /// <summary>
        /// �R���X�g���N�^
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
                NotifyCount.Invoke(this, new NumUpdateEventArgs(e.NumValue));//�ʒu��ʒm
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
                        Debug.WriteLine($"{dt.TimeOfDay}  {e.PropertyName}���x{XStgSpeed.ToString("0.00")}");
                        break;

                    case (nameof(BoardProvider.FXSts)):

                        XStgSts = (uint)bpservice.FXSts;

                        //�X�e�[�^�X����
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
                            //����������ALM��������SVON����~��
                            sts = XStgSts & 0x0F01;
                            if (sts == 0x0001)
                            {
                                XStgReady = true;
                            }
                            //���쒆���������A�������A�葬���쒆
                            sts = XStgSts & 0x0F01;
                            if (sts == 0x0101 || sts == 0x0201 || sts == 0x0401)
                            {
                                XStgBusy = true;
                            }
                            //�葬���쒆
                            sts = XStgSts & 0x0F01;
                            if (sts == 0x0401)
                            {
                                XStgConst = true;
                            }
                        }
                        else
                        {
                            //����������ALM��������SVON����~��
                            sts = XStgSts & 0x0F00;
                            if (sts == 0x0000)
                            {
                                XStgReady = true;
                            }
                            //���쒆���������A�������A�葬���쒆
                            sts = XStgSts & 0x0F01;
                            if (sts == 0x0100 || sts == 0x0200 || sts == 0x0400)
                            {
                                XStgBusy = true;
                            }
                            //�葬���쒆
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
        /// ������
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="cont"></param>
        public bool Init()
        {
            uint bnum = DevNum;
            int temp = 0;

            uint ret = 0;
            if (ret == 0)
            {//�{�[�h�̃J�E���g���� UP/DOWN�J�E���g�l������(�ۑ����Ă��錻�݈ʒu�ɐݒ肷��)
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRCTR1, FXPosCount);
            }
            if (ret == 0)
            {//�{�[�h�̃G���R�[�_�J�E���g���� UP/DOWN�J�E���g�l������(�ۑ����Ă��錻�݈ʒu�ɐݒ肷��)
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRCTR2, FXPosEncoderCount);
            }

            if (ret == 0)
            {//����ڰ��ް��ݒ�
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
            {//�⏕���x���W�X�^�Z�b�g RFA���x
                uint SubSpd = _BoardCnt.SpdToPps(Param.XStgOriginSpdFL, Param.XStgStep, (int)Param.XStgMag);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRFA, SubSpd);
            }
            if (ret == 0)
            {// ���x�{���ݒ�
                ushort Rmg = (ushort)((300 / Param.XStgMag) - 1);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WPRMG, (uint)Rmg);
            }
            if (ret == 0)
            {// ���W�X�^�����݁i�����_:PR5 = 0�j
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WPRDP, (int)0x00000000);
            }
            if (ret == 0)
            {// ���W�X�^�����݁i���ݒ�1:RENV1�j
                Param.XStgEnv1Data = Param.XStgEnv1Data | (uint)Param.XStgPlsMode;//�p���X���͕����ݒ�
                if (Param.XStgElsMode == 1)//ELS�Ō�����~�̏ꍇ
                {
                    Param.XStgEnv1Data = Param.XStgEnv1Data | 0x00000008;
                }
                if (Param.XStgMotorType == 1)//�T�[�{�̏ꍇ�͕΍��J�E���^�N���A
                {
                    Param.XStgEnv1Data = Param.XStgEnv1Data | 0x00000800;
                }
                if (Param.XStgDlsPolarity == 0)//DLS��A�ړ_�̏ꍇ
                {
                    Param.XStgEnv1Data = Param.XStgEnv1Data | 0x00000040;
                }
                if (Param.XStgOrgPolarity == 0)//OLS��A�ړ_�̏ꍇ
                {
                    Param.XStgEnv1Data = Param.XStgEnv1Data | 0x00000080;
                }
                if (Param.XStgAlmPolarity == 0)//ALM��A�ړ_�̏ꍇ
                {
                    Param.XStgEnv1Data = Param.XStgEnv1Data | 0x00000200;
                }
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV1, (uint)Param.XStgEnv1Data); ;
            }
            if (ret == 0)
            {// ���W�X�^�����݁i���ݒ�2:R7�j
                Param.XStgEnv2Data = Param.XStgEnv2Data & 0xFFCFFFFF;//���{�ݒ�
                temp = Param.XStgPlsMult << 20;
                Param.XStgEnv2Data = Param.XStgEnv2Data | (uint)temp;
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV2, (uint)Param.XStgEnv2Data);
            }
            if (ret == 0)
            {// ���W�X�^�����݁i���ݒ�3:R8�j
                Param.XStgEnv3Data = Param.XStgEnv3Data & 0xFFFFFFF0;//���_���A����
                Param.XStgEnv3Data = Param.XStgEnv3Data | (uint)Param.XStgOriginMode;
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV3, (uint)Param.XStgEnv3Data);
            }
            if (ret == 0)
            {// ���W�X�^�����݁i���ݒ�4: RENV4�j
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV4, (uint)Param.XStgEnv4Data);
            }
            if (ret == 0)
            {// ���W�X�^�����݁i���ݒ�5:RENV5�j
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV5, (uint)Param.XStgEnv5Data);
            }
            if (ret == 0)
            {// ���W�X�^�����݁i���ݒ�6:RENV6�j
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV6, (uint)Param.XStgEnv6Data);
            }
            if (ret == 0)
            {// ���W�X�^�����݁i���ݒ�7:RENV7�j
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV7, (uint)Param.XStgEnv7Data);
            }
            if (ret == 0)
            {// ���W�X�^�����݁i�A�b�v�_�E���J�E���g�l = �����ɐݒ�,�������[�g:PR15 = 0�j
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WPRDR, (uint)0x00000000);
            }
            // ���W�X�^�����݁iS�����:PR16 = 0�j
            if (ret == 0)
            {
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WPRUS, (int)0x00000000);
            }
            // ELS�ɐ��I��������
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
            {//�A���[���N���A���T�[�{ON
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
        /// �蓮��]
        /// </summary>
        /// <param name="mesbox">���b�Z�[�W</param>
        /// <param name="rotMode">��]����</param>
        /// <param name="manualspd">��]���x</param>
        /// <returns></returns>
        public bool Manual(RevMode revmode, float manualspd)
        {
            // ���x�̃`�F�b�N
            if (manualspd > Param.XStgManualLimitSpd || manualspd < Param.XStgSpdFL)
            {   // �w�葬�x�ُ�
                SendGUIMes = $"{nameof(XStageCtrl)} Setting Speed is uncorrect";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            uint bnum = DevNum;
            uint ret = 0;
            bool result = true;
            XStgStop = false;

            //���~�b�g����
            if (Param.XStgLmtNoChk == 0)
            {
                if (FXPos < Param.XStgCwLmtPos)
                {
                    if (revmode == RevMode.CW)
                    {
                        if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRENV4, Param.XStgEnvLmCwOf))
                        {   //�G���[
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
                        {   //�G���[
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
                    {   //�G���[
                        SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                        StsChanged?.Invoke(this, new EventArgs());
                    }
                }
            }
            //��������
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

            //���x�f�[�^����
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

            //���x�f�[�^��������
            ret = _BoardCnt.SetSpdReg(bnum, (int)BoardAxis.XSTG_JIKU, fhspd, flspd, rate, (uint)pos, (uint)mode);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            // �R�}���h�o�b�t�@������ �����X�^�[�g
            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.STAUD);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }
            if (revmode == RevMode.SCAN_CW || revmode == RevMode.SCAN_CCW)
            {
                //�^�C���A�E�g���Ԑݒ�(�������ԁ{100msec)
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
                {   //�G���[
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

            //���x�f�[�^����
            ushort flspd = _BoardCnt.SpdToPps(Param.XStgIndexSpdFL, Param.XStgStep, (int)Param.XStgMag);
            ushort fhspd = _BoardCnt.SpdToPps(Param.XStgIndexSpdFH, Param.XStgStep, (int)Param.XStgMag);
            ushort rate = _BoardCnt.AccelerationRate(Param.XStgIndexRateTime, flspd, fhspd);
            uint pos = _BoardCnt.DegToPls(indexpos, Param.XStgStep, Param.XStgDir);
            int mode = Param.XStgMotorType == 0 ? 0x08008141 : 0x08008341;

            //���x�f�[�^��������
            ret = _BoardCnt.SetSpdReg(bnum, (int)BoardAxis.XSTG_JIKU, fhspd, flspd, rate, pos, (uint)mode);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            // �R�}���h�o�b�t�@������ �����X�^�[�g
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
                //�N���m�F
                result = WaitMethod(bnum, TimeSpan.FromMilliseconds(500), WatchMode.Busy);
            }

            //�����҂�
            result = WaitMethod(bnum, TimeSpan.FromMilliseconds(Param.XStgIndexTimeOut), WatchMode.Ready);

            return result;
        }

        /// <summary>
        /// ��~
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
                case (StopMode.Fast):// ������~
                    ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.STOP);
                    break;

                case (StopMode.Slow)://������~
                    ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.SDSTP);
                    break;
                default:
                    //���肦�Ȃ����A�Ƃ肠����������~
                    ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.STOP);
                    throw new Exception("Unkown stop mode");
            }
            bool result = ret == 0 ? true : false;
            return result;
        }
        /// <summary>
        /// ���Z�b�g
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
            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.SVRSTON);   // �T�[�{���Z�b�gON

            Task.WaitAll(Task.Delay(100));//100mce

            //�T�[�{ON
            if (Param.XStgMotorType == 1)
            {
                ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.SVON);   // �T�[�{ON
            }
            
            bool result = WaitMethod(bnum, TimeSpan.FromMilliseconds(1000), WatchMode.Ready);

            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.SVRSTOFF);   // �T�[�{���Z�b�gOFF
                       
            XStgOrg = false;
            if (ret == 0) return true;

            return false;
            
        }

        /// <summary>
        /// ���_���A
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
            //+�������_�T�[�` 0x08008215;	// +�������_���A
            //-�������_�T�[�` 0x0800821D;	// -�������_���A
            uint mode = 0;
            //���_�Z���T�����o��
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

            //���x�f�[�^��������
            ret = _BoardCnt.SetSpdReg(bnum, (int)BoardAxis.XSTG_JIKU, fhspd, flspd, rate, pos, mode);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());

                return false;
            }

            //�����X�^�[�g
            if (ret != Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.STAUD))
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }
                    
            //�N���m�F
            result = WaitMethod(bnum, TimeSpan.FromMilliseconds(1000), WatchMode.Busy);
            XStgOrg = false;
            //DIO�Ɍ��_���A����ʒm
            //NotifyRestOperation?.Invoke(this, new ChkChangeEventArgs(true));
            //���_���A�����҂�
            result = WaitMethod(bnum, TimeSpan.FromMilliseconds(Param.XStgOriginTimeOut), WatchMode.Ready, ctoken);

            if (result && !XStgStop)
            {//�{�[�h�̃J�E���g���� UP/DOWN�J�E���g�l������(���_�ʒu��ݒ�)
                uint orgpospls = _BoardCnt.DegToPls(Param.XStgOriginPos, Param.XStgStep, Param.XStgDir);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRCTR1, orgpospls);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.XSTG_JIKU, Cp530l1a.WRCTR2, orgpospls);
                XStgOrg = true;
                result = Index(PosiMode.Rela, Param.XStgOriginOffset, ctoken);

                //DIO�Ɍ��_���A������ʒm
                //NotifyRestOperation?.Invoke(this, new ChkChangeEventArgs(false));
            }

            NotifyCount.Invoke(this, new NumUpdateEventArgs(FXPos));//�ʒu��ʒm

            return result;
        }

        /// <summary>
        /// �҂�����
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="bnum"></param>
        /// <param name="timeout">�^�C���A�E�g����</param>
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

                    if (watchmode == WatchMode.Ready && XStgReady)//�w�肵����ԂɂȂ����甲����
                    {
                        break;
                    }
                    if (watchmode == WatchMode.Busy && XStgBusy)///�w�肵����ԂɂȂ����甲����
                    {
                        break;
                    }
                    if (watchmode == WatchMode.Const && XStgConst)///�w�肵����ԂɂȂ����甲����
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
            {//�^�C���A�E�g����
                SendGUIMes = BoardMes.TIMEOUT_ERROR
                    + " " + timeout.TotalSeconds.ToString() + BoardMes.BOARD_SEC
                    + Environment.NewLine
                    + BoardMes.CONFIRM_OPERATION;
                StsChanged?.Invoke(this, new EventArgs());

                return false;
            }
            catch (AggregateException)
            {//��O����
                SendGUIMes = BoardMes.EXCEPTION_ERROR
                    + Environment.NewLine
                    + BoardMes.REBOOT_APP;

                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }
            finally
            {
                //�ۑ����Ă���
                BoardParam.Default.XStgEncoderCount = FXPosEncoderCount;
                BoardParam.Default.XStgCount = FXPosCount;
                BoardParam.Default.Save();
            }
            //UI�L�����Z���̏ꍇ��flase
            if (ui_cancel != null && ui_cancel.IsCancellationRequested)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// ����X�̗L�����l
        /// </summary>
        /// <returns></returns>
        public int GetScale()=> Param.XStgScale;
        /// <summary>
        /// ����X���̍ő�l
        /// </summary>
        /// <returns></returns>
        public float GetMax() => Param.XStgMaxValue;
        /// <summary>
        /// ����X���̍ŏ��l
        /// </summary>
        /// <returns></returns>
        public float GetMin() => Param.XStgMinValue;

        /// <summary>
        /// �ő呬�x mm/sec
        /// </summary>
        /// <returns></returns>
        public float GetMaxSpd() => Param.XStgManualLimitSpd;
        /// <summary>
        /// �ŏ����x mm/sec
        /// </summary>
        /// <returns></returns>
        public float GetMinSpd() => Param.XStgSpdFL;
        /// <summary>
        /// �j��
        /// </summary>
        public void Dispose()
        {
            //_BoardPro.Dispose();
        }
    }

    public interface IXStageCtrl
    {
        /// <summary>
        /// ���ݒl�̒ʒm
        /// </summary>
        event CountChangeEventHandler NotifyCount;
        /// <summary>
        /// ������
        /// </summary>
        bool Init();
        /// <summary>
        /// �w���]
        /// </summary>
        bool Manual(RevMode revmode, float manualspeed);
        /// <summary>
        /// ���_���A
        /// </summary>
        /// <param name="mesbox"></param>
        bool Origin(CancellationTokenSource ctoken);
        /// <summary>
        /// �X�g�b�v
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        bool Stop(StopMode mode);
        /// <summary>
        /// ���Z�b�g
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        bool Reset();
        /// <summary>
        /// �C���f�b�N�X�ړ�
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="posimode"></param>
        /// <param name="indexpos"></param>
        /// <returns></returns>
        bool Index(PosiMode posimode, float indexpos, CancellationTokenSource ctoken);
        /// <summary>
        /// ����X�̗L�����l
        /// </summary>
        /// <returns></returns>
        int GetScale();
        /// <summary>
        /// ����X���̍ő�l
        /// </summary>
        /// <returns></returns>
        float GetMax();
        /// <summary>
        /// ����X���̍ŏ��l
        /// </summary>
        /// <returns></returns>
        float GetMin();
        /// <summary>
        /// �ő呬�x mm/sec
        /// </summary>
        /// <returns></returns>
        float GetMaxSpd();
        /// <summary>
        /// �ŏ����x mm/sec
        /// </summary>
        /// <returns></returns>
        float GetMinSpd();

    }
}