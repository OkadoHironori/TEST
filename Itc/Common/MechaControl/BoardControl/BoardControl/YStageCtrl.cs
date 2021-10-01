
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
    /// �{�[�h�ɂ�鏸�~
    /// </summary>
    public class YStageCtrl : IYStageCtrl, IDisposable
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
        public float FYPos { get; private set; }
        /// <summary>
        /// �{�[�h��̃J�E���g�l(PLS)
        /// </summary>
        public uint FYPosCount { get; private set; } = BoardParam.Default.YStgCount;
        /// <summary>
        /// �{�[�h��̃G���R�[�h��̃J�E���g
        /// </summary>
        public uint FYPosEncoderCount { get; private set; } = BoardParam.Default.YStgEncoderCount;
        /// <summary>
        /// ���~�p�����[�^�@CSV�ǂݍ��߂Ί�{�Œ�
        /// </summary>
        public YStgParamFix Param { get; private set; }
        /// <summary>
        /// �{�[�h��̎w�ߑ��x�l(deg/sec)
        /// </summary>
        public float YStgSpeed { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l
        /// </summary>
        public uint YStgSts { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F���쒆
        /// </summary>
        public bool YStgBusy { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F�葬���쒆
        /// </summary>
        public bool YStgConst { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F��������
        /// </summary>
        public bool YStgReady { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F��~�w�ߓ���
        /// </summary>
        public bool YStgStop { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F���_����
        /// </summary>
        public bool YStgOrg { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F�{���~�b�g
        /// </summary>
        public bool YStgCwEls { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F�|���~�b�g
        /// </summary>
        public bool YStgCcwEls { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F���_�Z���T
        /// </summary>
        public bool YStgOls { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F�����Z���T
        /// </summary>
        public bool YStgDls { get; private set; }
        /// <summary>
        /// �{�[�h��̃X�e�[�^�X�l�F�A���[��
        /// </summary>
        public bool YStgAlm { get; private set; }
        /// <summary>
        /// �{�[�h��̃G���[���
        /// </summary>
        public uint YStgErrSts { get; private set; }
        /// <summary>
        /// �f�o�C�X�ԍ�
        /// </summary>
        public uint DevNum { get; private set; }
        /// <summary>
        /// �{�[�h����
        /// </summary>
        private readonly IBoardControl _BoardCnt;
        /// <summary>
        /// �{�[�h����Ď�
        /// </summary>
        //private YStageStsProvider _BoardPro;
        /// <summary>
        /// �{�[�h�\���l���M
        /// </summary>
        private readonly IBoardSender _ValueSender;
        /// <summary>
        /// �{�[�h�ݒ�I/F
        /// </summary>
        private readonly IBoardConfig _BoardConf;
        /// <summary>
        /// �R���X�g���N�^
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
                NotifyCount.Invoke(this, new NumUpdateEventArgs(e.NumValue));//�ʒu��ʒm
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

            //    ////�X�e�[�^�X����
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
            //    //    //����������ALM��������SVON����~��
            //    //    sts = YStgSts & 0x0F01;
            //    //    if (sts == 0x0001)
            //    //    {
            //    //        YStgReady = true;
            //    //    }
            //    //    //���쒆���������A�������A�葬���쒆
            //    //    sts = YStgSts & 0x0F01;
            //    //    if (sts == 0x0101 || sts == 0x0201 || sts == 0x0401)
            //    //    {
            //    //        YStgBusy = true;
            //    //    }
            //    //    //�葬���쒆
            //    //    sts = YStgSts & 0x0F01;
            //    //    if (sts == 0x0401)
            //    //    {
            //    //        YStgConst = true;
            //    //    }
            //    //}
            //    //else
            //    //{
            //    //    //����������ALM��������SVON����~��
            //    //    sts = YStgSts & 0x0F00;
            //    //    if (sts == 0x0000)
            //    //    {
            //    //        YStgReady = true;
            //    //    }
            //    //    //���쒆���������A�������A�葬���쒆
            //    //    sts = YStgSts & 0x0F01;
            //    //    if (sts == 0x0100 || sts == 0x0200 || sts == 0x0400)
            //    //    {
            //    //        YStgBusy = true;
            //    //    }
            //    //    //�葬���쒆
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
        /// ������
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
            {//�{�[�h�̃J�E���g���� UP/DOWN�J�E���g�l������(�ۑ����Ă��錻�݈ʒu�ɐݒ肷��)
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRCTR1, FYPosCount);
            }
            if (ret == 0)
            {//�{�[�h�̃G���R�[�_�J�E���g���� UP/DOWN�J�E���g�l������(�ۑ����Ă��錻�݈ʒu�ɐݒ肷��)
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRCTR2, FYPosEncoderCount);
            }

            if (ret == 0)
            {//����ڰ��ް��ݒ�
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
            {//�⏕���x���W�X�^�Z�b�g RFA���x
                uint SubSpd = _BoardCnt.SpdToPps(Param.YStgOriginSpdFL, Param.YStgStep, (int)Param.YStgMag);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRFA, SubSpd);
            }
            if (ret == 0)
            {// ���x�{���ݒ�
                ushort Rmg = (ushort)((300 / Param.YStgMag) - 1);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WPRMG, (uint)Rmg);
            }
            if (ret == 0)
            {// ���W�X�^�����݁i�����_:PR5 = 0�j
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WPRDP, (int)0x00000000);
            }
            if (ret == 0)
            {// ���W�X�^�����݁i���ݒ�1:RENV1�j
                Param.YStgEnv1Data = Param.YStgEnv1Data | (uint)Param.YStgPlsMode;//�p���X���͕����ݒ�
                if (Param.YStgElsMode == 1)//ELS�Ō�����~�̏ꍇ
                {
                    Param.YStgEnv1Data = Param.YStgEnv1Data | 0x00000008;
                }
                if (Param.YStgMotorType == 1)//�T�[�{�̏ꍇ�͕΍��J�E���^�N���A
                {
                    Param.YStgEnv1Data = Param.YStgEnv1Data | 0x00000800;
                }
                if (Param.YStgDlsPolarity == 0)//DLS��A�ړ_�̏ꍇ
                {
                    Param.YStgEnv1Data = Param.YStgEnv1Data | 0x00000040;
                }
                if (Param.YStgOrgPolarity == 0)//OLS��A�ړ_�̏ꍇ
                {
                    Param.YStgEnv1Data = Param.YStgEnv1Data | 0x00000080;
                }
                if (Param.YStgAlmPolarity == 0)//ALM��A�ړ_�̏ꍇ
                {
                    Param.YStgEnv1Data = Param.YStgEnv1Data | 0x00000200;
                }
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV1, (uint)Param.YStgEnv1Data); ;
            }
            if (ret == 0)
            {// ���W�X�^�����݁i���ݒ�2:R7�j
                Param.YStgEnv2Data = Param.YStgEnv2Data & 0xFFCFFFFF;//���{�ݒ�
                temp = Param.YStgPlsMult << 20;
                Param.YStgEnv2Data = Param.YStgEnv2Data | (uint)temp;
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV2, (uint)Param.YStgEnv2Data);
            }
            if (ret == 0)
            {// ���W�X�^�����݁i���ݒ�3:R8�j
                Param.YStgEnv3Data = Param.YStgEnv3Data & 0xFFFFFFF0;//���_���A����
                Param.YStgEnv3Data = Param.YStgEnv3Data | (uint)Param.YStgOriginMode;
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV3, (uint)Param.YStgEnv3Data);
            }
            if (ret == 0)
            {// ���W�X�^�����݁i���ݒ�4: RENV4�j
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV4, (uint)Param.YStgEnv4Data);
            }
            if (ret == 0)
            {// ���W�X�^�����݁i���ݒ�5:RENV5�j
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV5, (uint)Param.YStgEnv5Data);
            }
            if (ret == 0)
            {// ���W�X�^�����݁i���ݒ�6:RENV6�j
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV6, (uint)Param.YStgEnv6Data);
            }
            if (ret == 0)
            {// ���W�X�^�����݁i���ݒ�7:RENV7�j
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV7, (uint)Param.YStgEnv7Data);
            }
            if (ret == 0)
            {// ���W�X�^�����݁i�A�b�v�_�E���J�E���g�l = �����ɐݒ�,�������[�g:PR15 = 0�j
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WPRDR, (uint)0x00000000);
            }
            // ���W�X�^�����݁iS�����:PR16 = 0�j
            if (ret == 0)
            {
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WPRUS, (int)0x00000000);
            }
            // ELS�ɐ��I��������
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
            {//�A���[���N���A���T�[�{ON
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
        /// �蓮��]
        /// </summary>
        /// <param name="mesbox">���b�Z�[�W</param>
        /// <param name="rotMode">��]����</param>
        /// <param name="manualspd">��]���x</param>
        /// <returns></returns>
        public bool Manual(RevMode revmode, float manualspd)
        {
            // ���x�̃`�F�b�N
            if (manualspd > Param.YStgManualLimitSpd || manualspd < Param.YStgSpdFL)
            {   // �w�葬�x�ُ�
                SendGUIMes = "Setting Speed is uncorrect";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            uint bnum = DevNum;
            uint ret = 0;
            bool result = true;
            YStgStop = false;

            //���~�b�g����
            if (Param.YStgLmtNoChk == 0)
            {
                if (FYPos < Param.YStgCwLmtPos)
                {
                    if (revmode == RevMode.CW)
                    {
                        if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV4, Param.YStgEnvLmCwOf))
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
                else if (FYPos > Param.YStgCcwLmtPos)
                {
                    if (revmode == RevMode.CCW)
                    {
                        if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV4, Param.YStgEnvLmCcwOf))
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
                    if (ret != Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRENV4, Param.YStgEnvLmOn))
                    {   //�G���[
                        SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                        StsChanged?.Invoke(this, new EventArgs());
                    }
                }
            }

            //��������
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

            //���x�f�[�^����
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

            //���x�f�[�^��������
            ret = _BoardCnt.SetSpdReg(bnum, (int)BoardAxis.YSTG_JIKU, fhspd, flspd, rate, (uint)pos, (uint)mode);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            // �R�}���h�o�b�t�@������ �����X�^�[�g
            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.STAUD);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }
            if (revmode == RevMode.SCAN_CW || revmode == RevMode.SCAN_CCW)
            {
                //�^�C���A�E�g���Ԑݒ�(�������ԁ{100msec)
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
                {   //�G���[
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

            //���x�f�[�^����
            ushort flspd = _BoardCnt.SpdToPps(Param.YStgIndexSpdFL, Param.YStgStep, (int)Param.YStgMag);
            ushort fhspd = _BoardCnt.SpdToPps(Param.YStgIndexSpdFH, Param.YStgStep, (int)Param.YStgMag);
            ushort rate = _BoardCnt.AccelerationRate(Param.YStgIndexRateTime, flspd, fhspd);
            uint pos = _BoardCnt.DegToPls(indexpos, Param.YStgStep, Param.YStgDir);
            int mode = Param.YStgMotorType == 0 ? 0x08008141 : 0x08008341;

            //���x�f�[�^��������
            ret = _BoardCnt.SetSpdReg(bnum, (int)BoardAxis.YSTG_JIKU, fhspd, flspd, rate, pos, (uint)mode);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            // �R�}���h�o�b�t�@������ �����X�^�[�g
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
                //�N���m�F
                result = WaitMethod(bnum, TimeSpan.FromMilliseconds(500), WatchMode.Busy);
            }
            //�����҂�
            result = WaitMethod(bnum, TimeSpan.FromMilliseconds(Param.YStgIndexTimeOut), WatchMode.Ready);

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
            if (YStgBusy) YStgStop = true;
            switch (stopmode)
            {
                case (StopMode.Fast):// ������~
                    ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.STOP);
                    break;

                case (StopMode.Slow)://������~
                    ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.SDSTP);
                    break;
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

            ret = Hicpd530.cp530_rReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.RREST, ref errsts);
            YStgErrSts = errsts;
            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.SVRSTON);   // �T�[�{���Z�b�gON

            Task.WaitAll(Task.Delay(100));//100mce

            //�T�[�{ON
            if (Param.YStgMotorType == 1)
            {
                ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.SVON);   // �T�[�{ON
            }

            bool result = WaitMethod(bnum, TimeSpan.FromMilliseconds(1000), WatchMode.Ready);

            ret = Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.SVRSTOFF);   // �T�[�{���Z�b�gOFF

            YStgOrg = false;
            if (ret == 0) return true;

            return false;

        }
        /// <summary>
        /// ���_���A
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
            //+�������_�T�[�` 0x08008215;	// +�������_���A
            //-�������_�T�[�` 0x0800821D;	// -�������_���A
            uint mode = 0;
            //���_�Z���T�����o��
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

            //���x�f�[�^��������
            ret = _BoardCnt.SetSpdReg(bnum, (int)BoardAxis.YSTG_JIKU, fhspd, flspd, rate, pos, mode);
            if (ret != 0)
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            //�����X�^�[�g
            if (ret != Hicpd530.cp530_wCmdW(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.STAUD))
            {
                SendGUIMes = $"{BoardMes.BOARD_ERROR}{Environment.NewLine}{ret.ToString("X8")}";
                StsChanged?.Invoke(this, new EventArgs());
                return false;
            }

            //�N���m�F
            result = WaitMethod(bnum, TimeSpan.FromMilliseconds(1000), WatchMode.Busy);
            YStgOrg = false;
            //DIO�Ɍ��_���A����ʒm
            //NotifyRestOperation?.Invoke(this, new ChkChangeEventArgs(true));
            //���_���A�����҂�
            result = WaitMethod(bnum, TimeSpan.FromMilliseconds(Param.YStgOriginTimeOut), WatchMode.Ready, ctoken);

            if (result && !YStgStop)
            {//�{�[�h�̃J�E���g���� UP/DOWN�J�E���g�l������(���_�ʒu��ݒ�)
                uint orgpospls = _BoardCnt.DegToPls(Param.YStgOriginPos, Param.YStgStep, Param.YStgDir);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRCTR1, orgpospls);
                ret = Hicpd530.cp530_wReg(bnum, (int)BoardAxis.YSTG_JIKU, Cp530l1a.WRCTR2, orgpospls);
                YStgOrg = true;
                result = Index(PosiMode.Rela, Param.YStgOriginOffset);
                //DIO�Ɍ��_���A������ʒm
                //NotifyRestOperation?.Invoke(this, new ChkChangeEventArgs(false));
            }

            NotifyCount.Invoke(this, new NumUpdateEventArgs(FYPos));//�ʒu��ʒm


            return result;
        }

        /// <summary>
        /// �҂�����
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="bnum"></param>
        /// <param name="timeout">�^�C���A�E�g����</param>
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
                    if (watchmode == WatchMode.Ready && YStgReady)//�w�肵����ԂɂȂ����甲����
                    {
                        break;
                    }
                    if (watchmode == WatchMode.Busy && YStgBusy)///�w�肵����ԂɂȂ����甲����
                    {
                        break;
                    }
                    if (watchmode == WatchMode.Const && YStgConst)///�w�肵����ԂɂȂ����甲����
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
                BoardParam.Default.YStgEncoderCount = FYPosEncoderCount;
                BoardParam.Default.YStgCount = FYPosCount;
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
        public int GetScale() => Param.YStgScale;
        /// <summary>
        /// ����X���̍ő�l
        /// </summary>
        /// <returns></returns>
        public float GetMax() => Param.YStgMaxValue;
        /// <summary>
        /// ����X���̍ŏ��l
        /// </summary>
        /// <returns></returns>
        public float GetMin() => Param.YStgMinValue;

        /// <summary>
        /// �j��
        /// </summary>
        public void Dispose()
        {
            //_BoardPro.Dispose();
        }
    }

    /// <summary>
    /// ����Y�� I/F
    /// </summary>
    public interface IYStageCtrl
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
        /// ����Y�� ���_���A
        /// </summary>
        /// <param name="mesbox"></param>
        bool Origin(IProgress<CalProgress> prog, CancellationTokenSource ctoken);
        /// <summary>
        /// ����Y�� �X�g�b�v
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
        /// ����Y�� �C���f�b�N�X�ړ�
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="posimode"></param>
        /// <param name="indexpos"></param>
        /// <returns></returns>
        bool Index(PosiMode posimode, float indexpos);
        /// <summary>
        /// ����Y�̗L�����l
        /// </summary>
        /// <returns></returns>
        int GetScale();
        /// <summary>
        /// ����Y���̍ő�l
        /// </summary>
        /// <returns></returns>
        float GetMax();
        /// <summary>
        /// ����Y���̍ŏ��l
        /// </summary>
        /// <returns></returns>
        float GetMin();
    }
}