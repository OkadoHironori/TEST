using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Board.BoardControl
{
    public class YStgParamFix: IYStgParamFix
    {
        /// <summary>
        /// ��]���s�b�`(mm/P)
        /// </summary>
        public float YStgStep { get; set; }
        /// <summary>                                       
        /// �{��
        /// </summary>
        public float YStgMag { get; set; }
        /// <summary>
        /// �A�h���X��������
        /// </summary>
        public int YStgDir { get; set; }
        /// <summary>
        /// �p���X�o�͕���
        /// </summary>
        public int YStgPlsMode { get; set; }
        /// <summary>
        /// �p���X���{
        /// </summary>
        public int YStgPlsMult { get; set; }
        /// <summary>
        /// ���������[�g(�b)
        /// </summary>
        public float YStgRateTime { get; set; }
        /// <summary>
        /// �e�k��mm(mm/s)
        /// </summary>
        public float YStgSpdFL { get; set; }
        /// <summary>
        /// �蓮��]�X�s�[�h
        /// </summary>
        public float YStgManualSpd { get; set; }
        /// <summary>
        /// �蓮���� �ō����x
        /// </summary>
        public float YStgManualLimitSpd { get; set; }
        /// <summary>
        /// �蓮���� ����^�C���A�E�g�imsec)
        /// </summary>
        public int YStgManualTimeOut { get; set; }
        /// <summary>
        /// �ʒu���� ���������[�g(�b)
        /// </summary>
        public float YStgIndexRateTime { get; set; }
        /// <summary>
        /// �ʒu���� �e�k���x(mm/s)
        /// </summary>
        public float YStgIndexSpdFL { get; set; }
        /// <summary>
        /// �ʒu���� ���쑬�x(rpm)
        /// </summary>
        public float YStgIndexSpdFH { get; set; }
        /// <summary>
        /// �ʒu���� ����^�C���A�E�g(msec)
        /// </summary>
        public int YStgIndexTimeOut { get; set; }
        /// <summary>
        /// ���_���A ���쑬�x(mm/s)
        /// </summary>
        public float YStgOriginSpdFL { get; set; }
        /// <summary>
        /// ���_���A FL���x(mm/s)
        /// </summary>
        public float YStgOriginSpdFH { get; set; }
        /// <summary>
        /// ���_���A ����^�C���A�E�g����(�b)
        /// </summary>
        public int YStgOriginTimeOut { get; set; }
        /// <summary>
        /// ���_���甲���o�������(mm)
        /// </summary>
        public float YStgOriginShift { get; set; }
        /// <summary>
        /// ���_�ʒu(mm)
        /// </summary>
        public float YStgOriginPos { get; set; }
        /// <summary>
        /// ���_�I�t�Z�b�g�ʒu(mm)
        /// </summary>
        public float YStgOriginOffset { get; set; }
        /// <summary>
        /// ���_���AӰ��0:���_�ݻ�Œ�~�A2:Z���Œ�~
        /// </summary>
        public int YStgOriginMode { get; set; }
        /// <summary>
        /// ���_���A����
        /// </summary>
        public int YStgOriginDir { get; set; }
        /// <summary>
        /// ���_���A����
        /// </summary>
        public int YStgOriginNoChk { get; set; }
        /// <summary>
        /// ��~���� ����^�C���A�E�g(�b)
        /// </summary>
        public int YStgStopTimeOut { get; set; }
        /// <summary>
        /// YStgLmtNoChk=1�ŉ�]���������0�ŗL��
        /// </summary>
        public int YStgLmtNoChk { get; set; } = 1;
        /// <summary>
        /// ���]����ЯĈʒu(mm)	
        /// </summary>
        public float YStgCwLmtPos { get; set; } = (float)50.0;
        /// <summary>
        /// �t�]����ЯĈʒu(mm)	
        /// </summary>
        public float YStgCcwLmtPos { get; set; } = (float)-50.0;
        /// <summary>
        /// ���[�^�[���(0:�p���X,1:�T�[�{)
        /// </summary>
        public int YStgMotorType { get; set; }
        /// <summary>
        /// ���_�Z���T�ɐ�(0:a�ځA1:b��)
        /// </summary>
        public int YStgOrgPolarity { get; set; }
        /// <summary>
        /// ���~�b�g�Z���T�ɐ�(0:a�ځA1:b��)
        /// </summary>
        public int YStgLmtPolarity { get; set; }
        /// <summary>
        /// �����Z���T�ɐ�(0:a�ځA1:b��)
        /// </summary>
        public int YStgDlsPolarity { get; set; }
        /// <summary>
        /// �T�[�{�A���[���ɐ�(0:a�ځA1:b��)
        /// </summary>
        public int YStgAlmPolarity { get; set; }
        /// <summary>
        /// ���~�b�g�Z���T���o����~���@(0:����~�A1:������~)
        /// </summary>
        public int YStgElsMode { get; set; }
        /// <summary>
        /// YStgLmtNoChk=1�ŉ�]���������0�ŗL��
        /// </summary>
        public int YStgNoSlowMode { get; set; }
        /// <summary>
        /// CW�����ʒu(mm)	
        /// </summary>
        public float YStgCrLowPos { get; set; }
        /// <summary>
        /// CCW�����ʒu(mm)	
        /// </summary>
        public float YStgCrHiPos { get; set; }
        /// <summary>
        /// ���������x(mm/sec)	
        /// </summary>
        public float YStgSlowSpd { get; set; }
        /// <summary>
        /// ����\�ȕ\���l(string)
        /// </summary>                                                            
        public string YStgCtrIncrement { get; private set; }
        /// <summary>
        /// ����\�ȕ\���l
        /// </summary>
        public int YStgScale { get; private set; }
        /// <summary>
        /// �ő�l
        /// </summary>
        public float YStgMaxValue { get; private set; }
        /// <summary>
        /// �ŏ��l
        /// </summary>
        public float YStgMinValue { get; private set; }
        /// <summary>
        /// Inpos�͈�(�x)	
        /// </summary>
        public float YStgInpos { get; private set; } = (float)0.10;
        /// <summary>
        /// �{�[�g�萔 = 0x00000000
        /// 00000000 00000000 00000000 00000000	//����ڰĖ���
        /// </summary>
        public uint YStgEnvLmOf { get; set; } = 0x00000000;    // 
        /// <summary>
        /// �{�[�h�萔 =  0x00005054
        /// 00000000 00000000 01010000 01010100	//����ڰĈ�v�Ō�����~	
        /// </summary>
        public uint YStgEnvLmOn { get; set; } = 0x00005054;
        /// <summary>
        /// �{�[�h�萔 = 0x00000054
        /// 00000000 00000000 00000000 01010100	//����ڰĈ�v��CW�ЯČ�����~����
        /// </summary>
        public uint YStgEnvLmCwOf { get; set; } = 0x00000054;
        /// <summary>
        /// �{�[�h�萔  = 0x00005000
        /// 00000000 00000000 01010000 00000000	//����ڰĈ�v��CCW�ЯČ�����~����
        /// </summary>
        public uint YStgEnvLmCcwOf { get; set; } = 0x00005000;
        /// <summary>
        /// ���ϐ� 1 �w�ߏo�͕����A�M���ɐ��ݒ�
        /// </summary>
        //public uint Env1Data { get; set; } = 0x204348C4;
        public uint YStgEnv1Data { get; set; } = 0x20434004;
        /// <summary>
        /// ���ϐ� 2 /�ݺ���1���{
        /// </summary>
        public uint YStgEnv2Data { get; set; } = 0x0020F555;
        /// <summary>
        /// ���ϐ� 3 Z�w��p�������_���A�H
        /// </summary>
        public uint YStgEnv3Data { get; set; } = 0x00F00002;
        /// <summary>
        /// ���ϐ� 4 �R���p���[�^��v���̓���
        /// </summary>
        public uint YStgEnv4Data { get; set; } = 0x00000000;
        /// <summary>
        /// ���ϐ� 5
        /// </summary>
        public uint YStgEnv5Data { get; set; } = 0x00000000;  // 00000000 00000000 00000000 00000000
        /// <summary>
        /// ���ϐ� 6
        /// </summary>
        public uint YStgEnv6Data { get; set; } = 0x00000000;  // 00000000 00000000 00000000 00000000
        /// <summary>
        /// ���ϐ� 7
        /// </summary>
        public uint YStgEnv7Data { get; set; } = 0x00000000;  // 00000000 00000000 00000000 00000000
        /// <summary>
        /// �Ǎ������C�x���g
        /// </summary>             
        public event EventHandler EndLoadYStgParam;
        /// <summary>
        /// �p�����[�^�v��
        /// </summary>
        public void Requeset()
            => EndLoadYStgParam?.Invoke(this, new EventArgs());

        /// <summary>
        /// �{�[�h�p�����[�^�t�@�C���ւ̃p�X���擾����B
        /// </summary>
        private static string BoardPath
        {
            get { return Path.Combine(Directory.GetCurrentDirectory(), "TXSParam", "Mecha", "BoardTable", "YStage.csv"); }
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public YStgParamFix()
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
                    case ("YStgStep"):
                        YStgStep = float.Parse(Obj.Item2);
                        break;
                    case ("YStgMag"):
                        YStgMag = float.Parse(Obj.Item2);
                        break;
                    case ("YStgDir"):
                        YStgDir = int.Parse(Obj.Item2);
                        break;
                    case ("YStgPlsMode"):
                        YStgPlsMode = int.Parse(Obj.Item2);
                        break;
                    case ("YStgPlsMult"):
                        YStgPlsMult = int.Parse(Obj.Item2);
                        break;
                    case ("YStgRateTime"):
                        YStgRateTime = float.Parse(Obj.Item2);
                        break;
                    case ("YStgSpdFL"):
                        YStgSpdFL = float.Parse(Obj.Item2);
                        break;
                    case ("YStgManualSpd"):
                        YStgManualSpd = float.Parse(Obj.Item2);
                        break;
                    case ("YStgManualLimitSpd"):
                        YStgManualLimitSpd = float.Parse(Obj.Item2);
                        break;
                    case ("YStgManualTimeOut"):
                        YStgManualTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("YStgIndexRateTime"):
                        YStgIndexRateTime = float.Parse(Obj.Item2);
                        break;
                    case ("YStgIndexSpdFL"):
                        YStgIndexSpdFL = float.Parse(Obj.Item2);
                        break;
                    case ("YStgIndexSpdFH"):
                        YStgIndexSpdFH = float.Parse(Obj.Item2);
                        break;
                    case ("YStgIndexTimeOut"):
                        YStgIndexTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("YStgOriginSpdFL"):
                        YStgOriginSpdFL = float.Parse(Obj.Item2);
                        break;
                    case ("YStgOriginSpdFH"):
                        YStgOriginSpdFH = float.Parse(Obj.Item2);
                        break;
                    case ("YStgOriginTimeOut"):
                        YStgOriginTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("YStgOriginShift"):
                        YStgOriginShift = float.Parse(Obj.Item2);
                        break;
                    case ("YStgOriginPos"):
                        YStgOriginPos = float.Parse(Obj.Item2);
                        break;
                    case ("YStgOriginOffset"):
                        YStgOriginOffset = float.Parse(Obj.Item2);
                        break;
                    case ("YStgOriginMode"):
                        YStgOriginMode = int.Parse(Obj.Item2);
                        break;
                    case ("YStgOriginDir"):
                        YStgOriginDir = int.Parse(Obj.Item2);
                        break;
                    case ("YStgOriginNoChk"):
                        YStgOriginNoChk = int.Parse(Obj.Item2);
                        break;
                    case ("YStgStopTimeOut"):
                        YStgStopTimeOut = int.Parse(Obj.Item2);
                        break;
                    case ("YStgLmtNoChk"):
                        YStgLmtNoChk = int.Parse(Obj.Item2);
                        break;
                    case ("YStgCwLmtPos"):
                        YStgCwLmtPos = float.Parse(Obj.Item2);
                        break;
                    case ("YStgCcwLmtPos"):
                        YStgCcwLmtPos = float.Parse(Obj.Item2);
                        break;
                    case ("YStgMotorType"):
                        YStgMotorType = int.Parse(Obj.Item2);
                        break;
                    case ("YStgOrgPolarity"):
                        YStgOrgPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("YStgLmtPolarity"):
                        YStgLmtPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("YStgDlsPolarity"):
                        YStgDlsPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("YStgAlmPolarity"):
                        YStgAlmPolarity = int.Parse(Obj.Item2);
                        break;
                    case ("YStgElsMode"):
                        YStgElsMode = int.Parse(Obj.Item2);
                        break;
                    case ("YStgNoSlowMode"):
                        YStgNoSlowMode = int.Parse(Obj.Item2);
                        break;
                    case ("YStgCrLowPos"):
                        YStgCrLowPos = float.Parse(Obj.Item2);
                        break;
                    case ("YStgCrHiPos"):
                        YStgCrHiPos = float.Parse(Obj.Item2);
                        break;
                    case ("YStgSlowSpd"):
                        YStgSlowSpd = float.Parse(Obj.Item2);
                        break;

                    case ("YStgCtrIncrement"):

                        YStgCtrIncrement = Obj.Item2;

                        double input = Convert.ToDouble(YStgCtrIncrement);

                        double decimal_part = input % 1;

                        string output = Convert.ToString(decimal_part);

                        YStgScale = (output.Length - 2);

                        break;

                    case ("YStgMaxValue"):
                        YStgMaxValue = float.Parse(Obj.Item2);
                        break;

                    case ("YStgMinValue"):
                        YStgMinValue = float.Parse(Obj.Item2);
                        break;
                    case ("YStgInpos"):
                        YStgInpos = float.Parse(Obj.Item2);
                        break;
                }
            }
            return;
        }
    }

    public interface IYStgParamFix
    {
        /// <summary>
        /// �Ǎ������C�x���g
        /// </summary>             
        event EventHandler EndLoadYStgParam;
        /// <summary>
        /// �p�����[�^�v��
        /// </summary>
        void Requeset();
    }
}
