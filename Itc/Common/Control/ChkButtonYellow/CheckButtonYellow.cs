//---------------------------------------------------------------------------------------------------- 
// �v���O�������F CT.Controls Ver1.0   
// �t�@�C�����@�F CheckButton.cs    
// �����T�v�@�@�F On/Off�l�����{�^���N���X
// ���ӎ����@�@�F   
//  
// �n�r�@�@�@�@�F Windows XP Professional (SP2) 
// �R���p�C���@�F Microsoft Visual C# 2005  
//  
// VERSION  DATE        BY              CHANGE/COMMENT     
// v1.00    2008/04/16  (SS1)�ԁX�c     �V�K�쐬
//
// (C)Copyright TOSHIBA IT & CONTROL SYSTEMS CORPORATION 2008
//----------------------------------------------------------------------------------------------------
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Itc.Common.Controls
{
    /// <summary>
    /// On/Off�l�����{�^���iCheckBox����h���j
    /// </summary>
    public partial class CheckButtonYellow : CheckBox
    {
        //�I�����i�`�F�b�N���j�̃{�^���̐F�F�K��l�́u���F�v
        private Color myCheckedColor = Color.FromArgb(0xff, 0xff, 0x7f);

        //�I�t���i�A���`�F�b�N���j�̃{�^���̐F�F�K��l�́u�D�F�v
        private Color myUnCheckedColor = Color.FromKnownColor(KnownColor.Control);
        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public CheckButtonYellow()
            : base()
        {
            //�A�s�A�����X���u�{�^���v�ɂ���
            this.Appearance = Appearance.Button;

            //�^�u�ɂ��t�H�[�J�X�ړ����֎~
            this.TabStop = false;
        }

        /// <summary>
        /// �I�����i�`�F�b�N���j�̐F
        /// </summary>
        public Color CheckedColor
        {
            get { return myCheckedColor; }
            set { myCheckedColor = value; }
        }

        /// <summary>
        /// �I�t���i�A���`�F�b�N���j�̐F
        /// </summary>
        public Color UnCheckedColor
        {
            get { return myUnCheckedColor; }
            set { myUnCheckedColor = value; }
        }

        /// <summary>
        /// �l�ύX������
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCheckedChanged(EventArgs e)
        {
            //�w�i�F�̐ݒ�
            this.BackColor = this.Checked ? myCheckedColor : myUnCheckedColor;

            //�p�����̏��������s
            base.OnCheckedChanged(e);
        }
    }
}
