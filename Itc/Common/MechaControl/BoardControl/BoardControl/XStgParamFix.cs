using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Board.BoardControl
{
    public class XStgParamFix : IXStgParamFix
    {
        /// <summary>
        /// ��]���s�b�`(mm/P)
        /// </summary>
        public float XStgStep { get; private set; }
        /// <summary>                                       
        /// �{��
        /// </summary>
        public float XStgMag { get; private set; }
        /// <summary>
        /// �A�h���X��������
        /// </summary>
        public int XStgDir { get; private set; }
        /// <summary>
        /// �p���X�o�͕���
        /// </summary>
        public int XStgPlsMode { get; private set; }
        /// <summary>
        /// �p���X���{
        /// </summary>
        public int XStgPlsMult { get; private set; }
        /// <summary>
        /// ���������[�g(�b)
        /// </summary>
        public float XStgRateTime { get; private set; }
        /// <summary>
        /// �e�k��mm(mm/s)
        /// </summary>
        public float XStgSpdFL { get; private set; }
        /// <summary>
        /// �蓮��]�X�s�[�h
        /// </summary>
        public float XStgManualSpd { get; private set; }
        /// <summary>
        /// �蓮���� �ō����x
        /// </summary>
        public float XStgManualLimitSpd { get; private set; }
        /// <summary>
        /// �蓮���� ����^�C���A�E�g�imsec)
        /// </summary>
        public int XStgManualTimeOut { get; set; }
        /// <summary>
        /// �ʒu���� ���������[�g(�b)
        /// </summary>
        public float XStgIndexRateTime { get; set; }
        /// <summary>
        /// �ʒu���� �e�k���x(mm/s)
        /// </summary>
        public float XStgIndexSpdFL { get; set; }
        /// <summary>
        /// �ʒu���� ���쑬�x(rpm)
        /// </summary>
        public float XStgIndexSpdFH { get; set; }
        /// <summary>
        /// �ʒu���� ����^�C���A�E�g(msec)
        /// </summary>
        public int XStgIndexTimeOut { get; set; }
        /// <summary>
        /// ���_���A ���쑬�x(mm/s)
        /// </summary>
        public float XStgOriginSpdFL { get; set; }
        /// <summary>
        /// ���_���A FL���x(mm/s)
        /// </summary>
        public float XStgOriginSpdFH { get; set; }
        /// <summary>
        /// ���_���A ����^�C���A�E�g����(�b)
        /// </summary>
        public int XStgOriginTimeOut { get; set; }
        /// <summary>
        /// ���_���甲���o�������(mm)
        /// </summary>
        public float XStgOriginShift { get; set; }
        /// <summary>
        /// ���_�ʒu(mm)
        /// </summary>
        public float XStgOriginPos { get; set; }
        /// <summary>
        /// ���_�I�t�Z�b�g�ʒu(mm)
        /// </summary>
        public float XStgOriginOffset { get; set; }
        /// <summary>
        /// ���_���AӰ��0:���_�ݻ�Œ�~�A2:Z���Œ�~
        /// </summary>
        public int XStgOriginMode { get; set; }
        /// <summary>
        /// ���_���A����
        /// </summary>
        public int XStgOriginDir { get; set; }
        /// <summary>
        /// ���_���A����
        /// </summary>
        public int XStgOriginNoChk { get; set; }
        /// <summary>
        /// ��~���� ����^�C���A�E�g(�b)
        /// </summary>
        public int XStgStopTimeOut { get; set; }
        /// <summary>
        /// XStgLmtNoChk=1�ŉ�]���������0�ŗL��
        /// </summary>
        public int XStgLmtNoChk { get; set; } = 1;
        /// <summary>
        /// ���]����ЯĈʒu(mm)	
        /// </summary>
        public float XStgCwLmtPos { get; set; } = (float)50.0;
        /// <summary>
        /// �t�]����ЯĈʒu(mm)	
        /// </summary>
        public float XStgCcwLmtPos { get; set; } = (float)-50.0;
        /// <summary>
        /// ���[�^�[���(0:�p���X,1:�T�[�{)
        /// </summary>
        public int XStgMotorType { get; set; }
        /// <summary>
        /// ���_�Z���T�ɐ�(0:a�ځA1:b��)
        /// </summary>
        public int XStgOrgPolarity { get; set; }
        /// <summary>
        /// ���~�b�g�Z���T�ɐ�(0:a�ځA1:b��)
        /// </summary>
        public int XStgLmtPolarity { get; set; }
        /// <summary>
        /// �����Z���T�ɐ�(0:a�ځA1:b��)
        /// </summary>
        public int XStgDlsPolarity { get; set; }
        /// <summary>
        /// �T�[�{�A���[���ɐ�(0:a�ځA1:b��)
        /// </summary>
        public int XStgAlmPolarity { get; set; }
        /// <summary>
        /// ���~�b�g�Z���T���o����~���@(0:����~�A1:������~)
        /// </summary>
        public int XStgElsMode { get; set; }
        /// <summary>
        /// XStgLmtNoChk=1�ŉ�]���������0�ŗL��
        /// </summary>
        public int XStgNoSlowMode { get; set; }
        /// <summary>
        /// CW�����ʒu(mm)	
        /// </summary>
        public float XStgCrLowPos { get; set; }
        /// <summary>
        /// CCW�����ʒu(mm)	
        /// </summary>
        public float XStgCrHiPos { get; set; }
        /// <summary>
        /// ��������mm(mm/sec)	
        /// </summary>
        public float XStgSlowSpd { get; set; }
        /// <summary>
        /// ����\�ȕ\���l(string)
        /// </summary>
        public string XStgCtrIncrement { get; private set; }
        /// <summary>
        /// ����\�ȕ\���l
        /// </summary>
        public int XStgScale { get; private set; }
        /// <summary>
        /// �ő�l
        /// </summary>
        public float XStgMaxValue { get; private set; }
        /// <summary>
        /// �ŏ��l
        /// </summary>
        public float XStgMinValue { get; private set; }
        /// <summary>
        /// Inpos�͈�(�x)	
        /// </summary>
        public float XStgInpos { get; private set; } = (float)0.10;
        /// <summary>
        /// �{�[�g�萔 = 0x00000000
        /// 00000000 00000000 00000000 00000000	//����ڰĖ���
        /// </summary>
        public uint XStgEnvLmOf { get; set; } = 0x00000000;    // 
        /// <summary>
        /// �{�[�h�萔 =  0x00005054
        /// 00000000 00000000 01010000 01010100	//����ڰĈ�v�Ō�����~	
        /// </summary>
        public uint XStgEnvLmOn { get; set; } = 0x00005054;
        /// <summary>
        /// �{�[�h�萔 = 0x00000054
        /// 00000000 00000000 00000000 01010100	//����ڰĈ�v��CW�ЯČ�����~����
        /// </summary>
        public uint XStgEnvLmCwOf { get; set; } = 0x00000054;
        /// <summary>
        /// �{�[�h�萔  = 0x00005000
        /// 00000000 00000000 01010000 00000000	//����ڰĈ�v��CCW�ЯČ�����~����
        /// </summary>
        public uint XStgEnvLmCcwOf { get;  set; } = 0x00005000;
        /// <summary>
        /// ���ϐ� 1 �w�ߏo�͕����A�M���ɐ��ݒ�
        /// </summary>
        public uint XStgEnv1Data { get;  set; } = 0x20434004;
        /// <summary>
        /// ���ϐ� 2 /�ݺ���1���{
        /// </summary>
        public uint XStgEnv2Data { get;  set; } = 0x0020F555;
        /// <summary>
        /// ���ϐ� 3 Z�w��p�������_���A�H
        /// </summary>
        public uint XStgEnv3Data { get;  set; } = 0x00F00002;
        /// <summary>
        /// ���ϐ� 4 �R���p���[�^��v���̓���
        /// </summary>
        public uint XStgEnv4Data { get;  set; } = 0x00000000;
        /// <summary>
        /// ���ϐ� 5
        /// </summary>
        public uint XStgEnv5Data { get;  set; } = 0x00000000;  // 00000000 00000000 00000000 00000000
        /// <summary>
        /// ���ϐ� 6
        /// </summary>
        public uint XStgEnv6Data { get;  set; } = 0x00000000;  // 00000000 00000000 00000000 00000000
        /// <summary>
        /// ���ϐ� 7
        /// </summary>
        public uint XStgEnv7Data { get;  set; } = 0x00000000;  // 00000000 00000000 00000000 00000000
        /// <summary>
        /// �Ǎ������C�x���g
        /// </summary>             
        public event EventHandler EndLoadXStgParam;
        /// <summary>
        /// �p�����[�^�v��
        /// </summary>
        public void Requeset()
            => EndLoadXStgParam?.Invoke(this, new EventArgs());
        /// <summary>
        /// �{�[�h�p�����[�^�t�@�C���ւ̃p�X���擾����B
        /// </summary>
        private static string BoardPath
        {
            get { return Path.Combine(Directory.GetCurrentDirectory(), "TXSParam", "Mecha", "BoardTable", "XStage.csv"); }
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public XStgParamFix()
        {

      
            if (!File.Exists(BoardPath)) throw new Exception($"{BoardPath} dosn't exists");


            //CSV�t�@�C���Ǎ� Tuple
            List<Tuple<string, string>> FObj = File.ReadAllLines(BoardPath)
                    .Select(v => v.Split(','))
                    .Where(j => !string.IsNullOrEmpty(j[0]) && !string.IsNullOrEmpty(j[1]))
                    .Select(c => new Tuple<string, string>(c[0], c[1])).ToList();


            if (FObj.Count() == 0) throw new Exception($"Can't Load {BoardPath} File");


            foreach (var Obj in FObj)
            {
                switch (Obj.Item1)
                {
                    case ("XStgStep"):
                        XStgStep = float.Parse(Obj.Item2);
                        break;
                    case ("XStgMag"):
                        XStgMag = float.Parse(Obj.Item2);
                        break;
                    case ("XStgDir"):
                        XStgDir = int.Parse(Obj.Item2);
                        break;
                    case ("XStgPlsMode"):
                        XStgPlsMode = int.Parse(Obj.Item2);
                        break;
                    case ("XStgPlsMult"):
                        XStgPlsMult = int.Parse(Obj.Item2);
                        break;
                    case ("XStgRateTime"):
                        XStgRateTime = float.Parse(Obj.Item2);
                        break;
                    case ("XStgSpdFL"):
                        XStgSpdFL = float.Parse(Obj.Item2);
                        break;
                    case ("XStgManualSpd"):
                        XStgManualSpd = float.Parse(Obj.Item2);
                        break;
                    case ("XStgManualLimitSpd"):
                        XStgManualLimitSpd = float.Parse(Obj.Item2);
                        break;
                    case ("XStgManualTimeOut"):
                        XStgManualTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("XStgIndexRateTime"):
                        XStgIndexRateTime = float.Parse(Obj.Item2);
                        break;
                    case ("XStgIndexSpdFL"):
                        XStgIndexSpdFL = float.Parse(Obj.Item2);
                        break;
                    case ("XStgIndexSpdFH"):
                        XStgIndexSpdFH = float.Parse(Obj.Item2);
                        break;
                    case ("XStgIndexTimeOut"):
                        XStgIndexTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("XStgOriginSpdFL"):
                        XStgOriginSpdFL = float.Parse(Obj.Item2);
                        break;
                    case ("XStgOriginSpdFH"):
                        XStgOriginSpdFH = float.Parse(Obj.Item2);
                        break;
                    case ("XStgOriginTimeOut"):
                        XStgOriginTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("XStgOriginShift"):
                        XStgOriginShift = float.Parse(Obj.Item2);
                        break;
                    case ("XStgOriginPos"):
                        XStgOriginPos = float.Parse(Obj.Item2);
                        break;
                    case ("XStgOriginOffset"):
                        XStgOriginOffset = float.Parse(Obj.Item2);
                        break;
                    case ("XStgOriginMode"):
                        XStgOriginMode = int.Parse(Obj.Item2);
                        break;
                    case ("XStgOriginDir"):
                        XStgOriginDir = int.Parse(Obj.Item2);
                        break;
                    case ("XStgOriginNoChk"):
                        XStgOriginNoChk = int.Parse(Obj.Item2);
                        break;
                    case ("XStgStopTimeOut"):
                        XStgStopTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("XStgLmtNoChk"):
                        XStgLmtNoChk = int.Parse(Obj.Item2);
                        break;
                    case ("XStgCwLmtPos"):
                        XStgCwLmtPos = float.Parse(Obj.Item2);
                        break;
                    case ("XStgCcwLmtPos"):
                        XStgCcwLmtPos = float.Parse(Obj.Item2);
                        break;
                    case ("XStgMotorType"):
                        XStgMotorType = int.Parse(Obj.Item2);
                        break;
                    case ("XStgOrgPolarity"):
                        XStgOrgPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("XStgLmtPolarity"):
                        XStgLmtPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("XStgDlsPolarity"):
                        XStgDlsPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("XStgAlmPolarity"):
                        XStgAlmPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("XStgElsMode"):
                        XStgElsMode = int.Parse(Obj.Item2);
                        break;
                    case ("XStgNoSlowMode"):
                        XStgNoSlowMode = int.Parse(Obj.Item2);
                        break;
                    case ("XStgCrLowPos"):
                        XStgCrLowPos = float.Parse(Obj.Item2);
                        break;
                    case ("XStgCrHiPos"):
                        XStgCrHiPos = float.Parse(Obj.Item2);
                        break;
                    case ("XStgSlowSpd"):
                        XStgSlowSpd = float.Parse(Obj.Item2);
                        break;

                    case ("XStgCtrIncrement"):

                        XStgCtrIncrement = Obj.Item2;

                        double input = Convert.ToDouble(XStgCtrIncrement);

                        double decimal_part = input % 1;

                        string output = Convert.ToString(decimal_part);

                        XStgScale = (output.Length - 2);

                        break;

                    case ("XStgMaxValue"):
                        XStgMaxValue = float.Parse(Obj.Item2);
                        break;

                    case ("XStgMinValue"):
                        XStgMinValue = float.Parse(Obj.Item2);
                        break;
                    case ("XStgInpos"):
                        XStgInpos = float.Parse(Obj.Item2);
                        break;
                }
            }
        }
    }
    /// <summary>
    /// ����X���̌Œ�p�����[�^�N���X
    /// </summary>
    public interface IXStgParamFix
    {
        /// <summary>
        /// �Ǎ������C�x���g
        /// </summary>
        event EventHandler EndLoadXStgParam;
        /// <summary>
        /// �p�����[�^�v��
        /// </summary>
        void Requeset();
    }
}
