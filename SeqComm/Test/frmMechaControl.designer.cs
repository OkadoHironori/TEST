using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Test
{
	//[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class frmMechaControl
	{
		#region "Windows フォーム デザイナによって生成されたコード "
		[System.Diagnostics.DebuggerNonUserCode()]
		public frmMechaControl() : base()
		{
			//この呼び出しは、Windows フォーム デザイナで必要です。
			InitializeComponent();
		}
        //Form は、コンポーネント一覧に後処理を実行するために dispose をオーバーライドします。
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool Disposing)
		{
			if (Disposing) {
				if ((components != null)) {
					components.Dispose();
				}
			}
			base.Dispose(Disposing);
		}

        //Windows フォーム デザイナで必要です。
		private System.ComponentModel.IContainer components;
		public System.Windows.Forms.ToolTip ToolTip1;
		public System.Windows.Forms.Button _cmdSwitch_148;
		public System.Windows.Forms.Button _cmdSwitch_147;
		public System.Windows.Forms.GroupBox _Frame12_6;
		public System.Windows.Forms.Button _cmdSwitch_146;
		public System.Windows.Forms.Button _cmdSwitch_145;
		public System.Windows.Forms.GroupBox _Frame12_5;
		public System.Windows.Forms.Button _cmdSwitch_144;
		public System.Windows.Forms.Button _cmdSwitch_143;
		public System.Windows.Forms.GroupBox _Frame12_4;
		public System.Windows.Forms.Button _cmdSwitch_133;
		public System.Windows.Forms.Button _cmdSwitch_132;
		public System.Windows.Forms.GroupBox _Frame12_3;
		public System.Windows.Forms.Button _cmdSwitch_141;
		public System.Windows.Forms.Button _cmdSwitch_140;
		public System.Windows.Forms.Button _cmdSwitch_139;
		public System.Windows.Forms.Button _cmdSwitch_138;
		public System.Windows.Forms.Button _cmdSwitch_137;
		public System.Windows.Forms.Button _cmdSwitch_136;
		public System.Windows.Forms.Button _cmdSwitch_135;
		public System.Windows.Forms.Button _cmdSwitch_134;
		public System.Windows.Forms.GroupBox Frame28;
		public System.Windows.Forms.Button _cmdSwitch_131;
		public System.Windows.Forms.Button _cmdSwitch_130;
		public System.Windows.Forms.Button _cmdSwitch_129;
		public System.Windows.Forms.Button _cmdSwitch_128;
		public System.Windows.Forms.Button _cmdSwitch_127;
		public System.Windows.Forms.Button _cmdSwitch_126;
		public System.Windows.Forms.Button _cmdSwitch_125;
		public System.Windows.Forms.GroupBox Frame27;
		public System.Windows.Forms.Button _cmdSwitch_124;
		public System.Windows.Forms.Button _cmdSwitch_123;
		public System.Windows.Forms.Button _cmdSwitch_122;
		public System.Windows.Forms.Button _cmdSwitch_121;
		public System.Windows.Forms.Button _cmdSwitch_120;
		public System.Windows.Forms.Button _cmdSwitch_119;
		public System.Windows.Forms.Button _cmdSwitch_118;
		public System.Windows.Forms.Button _cmdSwitch_109;
		public System.Windows.Forms.GroupBox Frame26;
		public System.Windows.Forms.Button _cmdSwitch_107;
		public System.Windows.Forms.Button _cmdSwitch_106;
		public System.Windows.Forms.GroupBox _Frame12_2;
		public System.Windows.Forms.Button _cmdSwitch_117;
		public System.Windows.Forms.Button _cmdSwitch_116;
		public System.Windows.Forms.Button _cmdSwitch_115;
		public System.Windows.Forms.Button _cmdSwitch_114;
		public System.Windows.Forms.Button _cmdSwitch_113;
		public System.Windows.Forms.Button _cmdSwitch_112;
		public System.Windows.Forms.Button _cmdSwitch_111;
		public System.Windows.Forms.Button _cmdSwitch_110;
		public System.Windows.Forms.Button _cmdSwitch_105;
		public System.Windows.Forms.GroupBox _Frame15_1;
		public System.Windows.Forms.TextBox txtEXMTCSet;
        public System.Windows.Forms.TextBox txtEXMTVSet;
		public System.Windows.Forms.Button _cmdSwitch_103;
		public System.Windows.Forms.Button _cmdSwitch_102;
		public System.Windows.Forms.Button _cmdSwitch_101;
		public System.Windows.Forms.Button _cmdSwitch_100;
		public System.Windows.Forms.Button _cmdSwitch_99;
		public System.Windows.Forms.Button _cmdSwitch_98;
		public System.Windows.Forms.Button _cmdSwitch_97;
        public System.Windows.Forms.Button _cmdSwitch_96;
		public System.Windows.Forms.Label _Label1_68;
		public System.Windows.Forms.Label _Label1_67;
		public System.Windows.Forms.Label _Label1_66;
		public System.Windows.Forms.Label _Label1_65;
		public System.Windows.Forms.GroupBox Frame25;
		public System.Windows.Forms.Button _cmdSwitch_95;
		public System.Windows.Forms.Button _cmdSwitch_94;
		public System.Windows.Forms.GroupBox _Frame12_1;
		public System.Windows.Forms.Button _cmdSwitch_92;
		public System.Windows.Forms.Button _cmdSwitch_93;
		public System.Windows.Forms.TextBox txtFIDFineLimit;
		public System.Windows.Forms.TextBox txtFCDFineLimit;
		public System.Windows.Forms.TextBox txtFCDLimit;
		public System.Windows.Forms.Label _Label1_64;
		public System.Windows.Forms.Label _Label1_63;
		public System.Windows.Forms.Label _Label1_62;
		public System.Windows.Forms.Label _Label1_61;
		public System.Windows.Forms.Label _Label1_60;
		public System.Windows.Forms.Label _Label1_59;
		public System.Windows.Forms.GroupBox Frame24;
		public System.Windows.Forms.Button _cmdSwitch_87;
		public System.Windows.Forms.TextBox _txtSpeed_9;
		public System.Windows.Forms.Button _cmdSwitch_86;
		public System.Windows.Forms.Button _cmdSwitch_85;
		public System.Windows.Forms.HScrollBar _hsbSpeed_9;
		public System.Windows.Forms.Button _cmdSwitch_84;
		public System.Windows.Forms.Button _cmdSwitch_83;
		public System.Windows.Forms.TextBox txtXrayRotIndexPosition;
		public System.Windows.Forms.Label _Label1_56;
		public System.Windows.Forms.Label _Label1_55;
		public System.Windows.Forms.Label _Label1_54;
		public System.Windows.Forms.GroupBox Frame23;
		public System.Windows.Forms.Button _cmdSwitch_82;
		public System.Windows.Forms.TextBox txtXrayYIndexPosition;
		public System.Windows.Forms.Button _cmdSwitch_81;
		public System.Windows.Forms.HScrollBar _hsbSpeed_8;
		public System.Windows.Forms.Button _cmdSwitch_80;
		public System.Windows.Forms.Button _cmdSwitch_79;
		public System.Windows.Forms.TextBox _txtSpeed_8;
		public System.Windows.Forms.Button _cmdSwitch_78;
		public System.Windows.Forms.Label _Label1_53;
		public System.Windows.Forms.Label _Label1_52;
		public System.Windows.Forms.Label _Label1_51;
		public System.Windows.Forms.GroupBox Frame22;
		public System.Windows.Forms.TextBox txtXrayXIndexPosition;
		public System.Windows.Forms.Button _cmdSwitch_77;
		public System.Windows.Forms.Button _cmdSwitch_76;
		public System.Windows.Forms.HScrollBar _hsbSpeed_7;
		public System.Windows.Forms.Button _cmdSwitch_75;
		public System.Windows.Forms.Button _cmdSwitch_74;
		public System.Windows.Forms.TextBox _txtSpeed_7;
		public System.Windows.Forms.Button _cmdSwitch_73;
		public System.Windows.Forms.Label _Label1_50;
		public System.Windows.Forms.Label _Label1_49;
		public System.Windows.Forms.Label _Label1_48;
		public System.Windows.Forms.GroupBox Frame21;
		public System.Windows.Forms.Button _cmdSwitch_71;
		public System.Windows.Forms.Button _cmdSwitch_70;
		public System.Windows.Forms.GroupBox Frame20;
		public System.Windows.Forms.Button _cmdSwitch_69;
		public System.Windows.Forms.Button _cmdSwitch_68;
		public System.Windows.Forms.GroupBox Frame19;
		public System.Windows.Forms.Button _cmdSwitch_67;
		public System.Windows.Forms.Button _cmdSwitch_66;
		public System.Windows.Forms.GroupBox Frame18;
		public System.Windows.Forms.Button _cmdSwitch_65;
		public System.Windows.Forms.Button _cmdSwitch_64;
		public System.Windows.Forms.GroupBox Frame17;
		public System.Windows.Forms.HScrollBar _hsbSpeed_6;
		public System.Windows.Forms.Button _cmdSwitch_61;
		public System.Windows.Forms.Button _cmdSwitch_60;
		public System.Windows.Forms.Button _cmdSwitch_59;
		public System.Windows.Forms.TextBox _txtSpeed_6;
		public System.Windows.Forms.Label lblYStgMinSpeed;
		public System.Windows.Forms.Label lblYStgMaxSpeed;
		public System.Windows.Forms.Label lblYStgPosition;
		public System.Windows.Forms.Label _Label1_38;
		public System.Windows.Forms.Label _Label1_34;
		public System.Windows.Forms.Label _Label1_37;
		public System.Windows.Forms.Label _Label1_33;
		public System.Windows.Forms.Label _Label1_36;
		public System.Windows.Forms.Label _Label1_32;
		public System.Windows.Forms.Label _Label1_35;
		public System.Windows.Forms.Label _Label1_31;
		public System.Windows.Forms.GroupBox Frame16;
		public System.Windows.Forms.HScrollBar _hsbSpeed_5;
		public System.Windows.Forms.Button _cmdSwitch_58;
		public System.Windows.Forms.Button _cmdSwitch_30;
		public System.Windows.Forms.Button _cmdSwitch_29;
		public System.Windows.Forms.TextBox _txtSpeed_5;
		public System.Windows.Forms.Label _Label1_30;
		public System.Windows.Forms.Label _Label1_26;
		public System.Windows.Forms.Label _Label1_29;
		public System.Windows.Forms.Label lblXStgMinSpeed;
		public System.Windows.Forms.Label _Label1_25;
		public System.Windows.Forms.Label _Label1_28;
		public System.Windows.Forms.Label lblXStgMaxSpeed;
		public System.Windows.Forms.Label _Label1_24;
		public System.Windows.Forms.Label lblXStgPosition;
		public System.Windows.Forms.Label _Label1_27;
		public System.Windows.Forms.Label _Label1_23;
		public System.Windows.Forms.GroupBox Frame8;
		public System.Windows.Forms.Button _cmdSwitch_57;
		public System.Windows.Forms.Button _cmdSwitch_56;
		public System.Windows.Forms.GroupBox _Frame12_0;
		public System.Windows.Forms.Button _cmdSwitch_108;
		public System.Windows.Forms.Button _cmdSwitch_55;
		public System.Windows.Forms.TextBox _txtSpeed_0;
		public System.Windows.Forms.Button _cmdSwitch_1;
		public System.Windows.Forms.Button _cmdSwitch_2;
		public System.Windows.Forms.HScrollBar _hsbSpeed_0;
		public System.Windows.Forms.Button _cmdSwitch_45;
		public System.Windows.Forms.Button _cmdSwitch_46;
		public System.Windows.Forms.TextBox txtXIndexPosition;
		public System.Windows.Forms.Label _Label1_0;
		public System.Windows.Forms.Label _Label1_40;
		public System.Windows.Forms.Label _Label1_41;
		public System.Windows.Forms.GroupBox Frame1;
		public System.Windows.Forms.Button _cmdSwitch_72;
		public System.Windows.Forms.TextBox _txtSpeed_1;
		public System.Windows.Forms.Button _cmdSwitch_3;
		public System.Windows.Forms.Button _cmdSwitch_4;
		public System.Windows.Forms.HScrollBar _hsbSpeed_1;
		public System.Windows.Forms.Button _cmdSwitch_43;
		public System.Windows.Forms.TextBox txtYIndexPosition;
		public System.Windows.Forms.Button _cmdSwitch_44;
		public System.Windows.Forms.Label _Label1_42;
		public System.Windows.Forms.Label _Label1_43;
		public System.Windows.Forms.Label _Label1_44;
		public System.Windows.Forms.GroupBox Frame2;
		public System.Windows.Forms.Button _cmdSwitch_142;
		public System.Windows.Forms.TextBox txtIIIndexPosition;
		public System.Windows.Forms.HScrollBar _hsbSpeed_4;
		public System.Windows.Forms.TextBox _txtSpeed_4;
		public System.Windows.Forms.Button _cmdSwitch_6;
		public System.Windows.Forms.Button _cmdSwitch_5;
		public System.Windows.Forms.Button _cmdSwitch_7;
		public System.Windows.Forms.Button _cmdSwitch_8;
		public System.Windows.Forms.Label _Label1_47;
		public System.Windows.Forms.Label _Label1_46;
		public System.Windows.Forms.Label _Label1_45;
		public System.Windows.Forms.GroupBox Frame3;
		public System.Windows.Forms.Button _cmdSwitch_11;
		public System.Windows.Forms.Button _cmdSwitch_10;
		public System.Windows.Forms.Button _cmdSwitch_9;
		public System.Windows.Forms.GroupBox Frame4;
		public System.Windows.Forms.Button _cmdSwitch_22;
		public System.Windows.Forms.Button _cmdSwitch_23;
		public System.Windows.Forms.Button _cmdSwitch_24;
		public System.Windows.Forms.Button _cmdSwitch_25;
		public System.Windows.Forms.Button _cmdSwitch_18;
		public System.Windows.Forms.Button _cmdSwitch_19;
		public System.Windows.Forms.Button _cmdSwitch_20;
		public System.Windows.Forms.Button _cmdSwitch_21;
		public System.Windows.Forms.GroupBox Frame5;
		public System.Windows.Forms.Button _cmdSwitch_12;
		public System.Windows.Forms.Button _cmdSwitch_13;
		public System.Windows.Forms.Button _cmdSwitch_14;
		public System.Windows.Forms.Button _cmdSwitch_15;
		public System.Windows.Forms.Button _cmdSwitch_16;
		public System.Windows.Forms.Button _cmdSwitch_17;
		public System.Windows.Forms.GroupBox Frame6;
		public System.Windows.Forms.Button _cmdSwitch_28;
		public System.Windows.Forms.Button _cmdSwitch_27;
		public System.Windows.Forms.Button _cmdSwitch_26;
		public System.Windows.Forms.Button _cmdSwitch_37;
		public System.Windows.Forms.Button _cmdSwitch_38;
		public System.Windows.Forms.GroupBox Frame7;
		public System.Windows.Forms.TextBox txtTrgCycleTime;
		public System.Windows.Forms.Button _cmdSwitch_63;
		public System.Windows.Forms.TextBox txtTrgTime;
		public System.Windows.Forms.TextBox txtScanStartPos;
		public System.Windows.Forms.Button _cmdSwitch_62;
		public System.Windows.Forms.TextBox _txtSpeed_2;
		public System.Windows.Forms.Button _cmdSwitch_31;
		public System.Windows.Forms.Button _cmdSwitch_33;
		public System.Windows.Forms.Button _cmdSwitch_32;
		public System.Windows.Forms.HScrollBar _hsbSpeed_2;
		public System.Windows.Forms.Label _Label1_58;
		public System.Windows.Forms.Label _Label1_57;
		public System.Windows.Forms.Label _Label1_14;
		public System.Windows.Forms.Label _Label1_7;
		public System.Windows.Forms.Label _Label1_13;
		public System.Windows.Forms.Label _Label1_6;
		public System.Windows.Forms.Label _Label1_11;
		public System.Windows.Forms.Label lblRotAccelTm;
		public System.Windows.Forms.Label _Label1_4;
		public System.Windows.Forms.Label _Label1_1;
		public System.Windows.Forms.Label _Label1_8;
		public System.Windows.Forms.Label lblRotPosition;
		public System.Windows.Forms.Label _Label1_2;
		public System.Windows.Forms.Label lblRotMaxSpeed;
		public System.Windows.Forms.Label _Label1_9;
		public System.Windows.Forms.Label _Label1_3;
		public System.Windows.Forms.Label lblRotMinSpeed;
		public System.Windows.Forms.Label _Label1_10;
		public System.Windows.Forms.Label _Label1_5;
		public System.Windows.Forms.Label _Label1_12;
		public System.Windows.Forms.GroupBox Frame9;
		public System.Windows.Forms.TextBox _txtSpeed_3;
		public System.Windows.Forms.Button _cmdSwitch_36;
		public System.Windows.Forms.Button _cmdSwitch_34;
		public System.Windows.Forms.Button _cmdSwitch_35;
		public System.Windows.Forms.HScrollBar _hsbSpeed_3;
		public System.Windows.Forms.Label _Label1_15;
		public System.Windows.Forms.Label _Label1_19;
		public System.Windows.Forms.Label _Label1_16;
		public System.Windows.Forms.Label _Label1_20;
		public System.Windows.Forms.Label _Label1_17;
		public System.Windows.Forms.Label _Label1_21;
		public System.Windows.Forms.Label _Label1_18;
		public System.Windows.Forms.Label _Label1_22;
		public System.Windows.Forms.Label lblUDPosition;
		public System.Windows.Forms.Label lblUDMaxSpeed;
		public System.Windows.Forms.Label lblUDMinSpeed;
		public System.Windows.Forms.GroupBox Frame10;
		public System.Windows.Forms.Button _cmdSwitch_0;
		public System.Windows.Forms.GroupBox Frame11;
		public System.Windows.Forms.Button _cmdSwitch_39;
		public System.Windows.Forms.Button _cmdSwitch_40;
		public System.Windows.Forms.GroupBox Frame13;
		public System.Windows.Forms.Button _cmdSwitch_41;
		public System.Windows.Forms.Button _cmdSwitch_42;
		public System.Windows.Forms.Label lblXray;
		public System.Windows.Forms.Label _Label1_39;
		public System.Windows.Forms.GroupBox Frame14;
		public System.Windows.Forms.Button _cmdSwitch_104;
		public System.Windows.Forms.Button _cmdSwitch_91;
		public System.Windows.Forms.Button _cmdSwitch_90;
		public System.Windows.Forms.Button _cmdSwitch_89;
		public System.Windows.Forms.Button _cmdSwitch_88;
		public System.Windows.Forms.Button _cmdSwitch_54;
		public System.Windows.Forms.Button _cmdSwitch_53;
		public System.Windows.Forms.Button _cmdSwitch_52;
		public System.Windows.Forms.Button _cmdSwitch_51;
		public System.Windows.Forms.Button _cmdSwitch_47;
		public System.Windows.Forms.Button _cmdSwitch_48;
		public System.Windows.Forms.Button _cmdSwitch_49;
		public System.Windows.Forms.Button _cmdSwitch_50;
        public System.Windows.Forms.GroupBox _Frame15_0;
//メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
//Windows フォーム デザイナを使って変更できます。
//コード エディタを使用して、変更しないでください。
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._Frame12_6 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_148 = new System.Windows.Forms.Button();
            this._cmdSwitch_147 = new System.Windows.Forms.Button();
            this._Frame12_5 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_146 = new System.Windows.Forms.Button();
            this._cmdSwitch_145 = new System.Windows.Forms.Button();
            this._Frame12_4 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_144 = new System.Windows.Forms.Button();
            this._cmdSwitch_143 = new System.Windows.Forms.Button();
            this._Frame12_3 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_133 = new System.Windows.Forms.Button();
            this._cmdSwitch_132 = new System.Windows.Forms.Button();
            this.Frame28 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_141 = new System.Windows.Forms.Button();
            this._cmdSwitch_140 = new System.Windows.Forms.Button();
            this._cmdSwitch_139 = new System.Windows.Forms.Button();
            this._cmdSwitch_138 = new System.Windows.Forms.Button();
            this._cmdSwitch_137 = new System.Windows.Forms.Button();
            this._cmdSwitch_136 = new System.Windows.Forms.Button();
            this._cmdSwitch_135 = new System.Windows.Forms.Button();
            this._cmdSwitch_134 = new System.Windows.Forms.Button();
            this.Frame27 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_131 = new System.Windows.Forms.Button();
            this._cmdSwitch_130 = new System.Windows.Forms.Button();
            this._cmdSwitch_129 = new System.Windows.Forms.Button();
            this._cmdSwitch_128 = new System.Windows.Forms.Button();
            this._cmdSwitch_127 = new System.Windows.Forms.Button();
            this._cmdSwitch_126 = new System.Windows.Forms.Button();
            this._cmdSwitch_125 = new System.Windows.Forms.Button();
            this.Frame26 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_124 = new System.Windows.Forms.Button();
            this._cmdSwitch_123 = new System.Windows.Forms.Button();
            this._cmdSwitch_122 = new System.Windows.Forms.Button();
            this._cmdSwitch_121 = new System.Windows.Forms.Button();
            this._cmdSwitch_120 = new System.Windows.Forms.Button();
            this._cmdSwitch_119 = new System.Windows.Forms.Button();
            this._cmdSwitch_118 = new System.Windows.Forms.Button();
            this._cmdSwitch_109 = new System.Windows.Forms.Button();
            this._Frame12_2 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_107 = new System.Windows.Forms.Button();
            this._cmdSwitch_106 = new System.Windows.Forms.Button();
            this._Frame15_1 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_117 = new System.Windows.Forms.Button();
            this._cmdSwitch_116 = new System.Windows.Forms.Button();
            this._cmdSwitch_115 = new System.Windows.Forms.Button();
            this._cmdSwitch_114 = new System.Windows.Forms.Button();
            this._cmdSwitch_113 = new System.Windows.Forms.Button();
            this._cmdSwitch_112 = new System.Windows.Forms.Button();
            this._cmdSwitch_111 = new System.Windows.Forms.Button();
            this._cmdSwitch_110 = new System.Windows.Forms.Button();
            this._cmdSwitch_105 = new System.Windows.Forms.Button();
            this.Frame25 = new System.Windows.Forms.GroupBox();
            this.txtEXMTVSet = new System.Windows.Forms.TextBox();
            this.txtEXMTCSet = new System.Windows.Forms.TextBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this._cmdSwitch_103 = new System.Windows.Forms.Button();
            this._cmdSwitch_102 = new System.Windows.Forms.Button();
            this._cmdSwitch_101 = new System.Windows.Forms.Button();
            this._cmdSwitch_100 = new System.Windows.Forms.Button();
            this._cmdSwitch_99 = new System.Windows.Forms.Button();
            this._cmdSwitch_98 = new System.Windows.Forms.Button();
            this._cmdSwitch_97 = new System.Windows.Forms.Button();
            this._cmdSwitch_96 = new System.Windows.Forms.Button();
            this._Label1_68 = new System.Windows.Forms.Label();
            this._Label1_67 = new System.Windows.Forms.Label();
            this._Label1_66 = new System.Windows.Forms.Label();
            this._Label1_65 = new System.Windows.Forms.Label();
            this._Frame12_1 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_95 = new System.Windows.Forms.Button();
            this._cmdSwitch_94 = new System.Windows.Forms.Button();
            this.Frame24 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_92 = new System.Windows.Forms.Button();
            this._cmdSwitch_93 = new System.Windows.Forms.Button();
            this.txtFIDFineLimit = new System.Windows.Forms.TextBox();
            this.txtFCDFineLimit = new System.Windows.Forms.TextBox();
            this.txtFCDLimit = new System.Windows.Forms.TextBox();
            this._Label1_64 = new System.Windows.Forms.Label();
            this._Label1_63 = new System.Windows.Forms.Label();
            this._Label1_62 = new System.Windows.Forms.Label();
            this._Label1_61 = new System.Windows.Forms.Label();
            this._Label1_60 = new System.Windows.Forms.Label();
            this._Label1_59 = new System.Windows.Forms.Label();
            this.Frame23 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_87 = new System.Windows.Forms.Button();
            this._txtSpeed_9 = new System.Windows.Forms.TextBox();
            this._cmdSwitch_86 = new System.Windows.Forms.Button();
            this._cmdSwitch_85 = new System.Windows.Forms.Button();
            this._hsbSpeed_9 = new System.Windows.Forms.HScrollBar();
            this._cmdSwitch_84 = new System.Windows.Forms.Button();
            this._cmdSwitch_83 = new System.Windows.Forms.Button();
            this.txtXrayRotIndexPosition = new System.Windows.Forms.TextBox();
            this._Label1_56 = new System.Windows.Forms.Label();
            this._Label1_55 = new System.Windows.Forms.Label();
            this._Label1_54 = new System.Windows.Forms.Label();
            this.Frame22 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_82 = new System.Windows.Forms.Button();
            this.txtXrayYIndexPosition = new System.Windows.Forms.TextBox();
            this._cmdSwitch_81 = new System.Windows.Forms.Button();
            this._hsbSpeed_8 = new System.Windows.Forms.HScrollBar();
            this._cmdSwitch_80 = new System.Windows.Forms.Button();
            this._cmdSwitch_79 = new System.Windows.Forms.Button();
            this._txtSpeed_8 = new System.Windows.Forms.TextBox();
            this._cmdSwitch_78 = new System.Windows.Forms.Button();
            this._Label1_53 = new System.Windows.Forms.Label();
            this._Label1_52 = new System.Windows.Forms.Label();
            this._Label1_51 = new System.Windows.Forms.Label();
            this.Frame21 = new System.Windows.Forms.GroupBox();
            this.txtXrayXIndexPosition = new System.Windows.Forms.TextBox();
            this._cmdSwitch_77 = new System.Windows.Forms.Button();
            this._cmdSwitch_76 = new System.Windows.Forms.Button();
            this._hsbSpeed_7 = new System.Windows.Forms.HScrollBar();
            this._cmdSwitch_75 = new System.Windows.Forms.Button();
            this._cmdSwitch_74 = new System.Windows.Forms.Button();
            this._txtSpeed_7 = new System.Windows.Forms.TextBox();
            this._cmdSwitch_73 = new System.Windows.Forms.Button();
            this._Label1_50 = new System.Windows.Forms.Label();
            this._Label1_49 = new System.Windows.Forms.Label();
            this._Label1_48 = new System.Windows.Forms.Label();
            this.Frame20 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_71 = new System.Windows.Forms.Button();
            this._cmdSwitch_70 = new System.Windows.Forms.Button();
            this.Frame19 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_69 = new System.Windows.Forms.Button();
            this._cmdSwitch_68 = new System.Windows.Forms.Button();
            this.Frame18 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_67 = new System.Windows.Forms.Button();
            this._cmdSwitch_66 = new System.Windows.Forms.Button();
            this.Frame17 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_65 = new System.Windows.Forms.Button();
            this._cmdSwitch_64 = new System.Windows.Forms.Button();
            this.Frame16 = new System.Windows.Forms.GroupBox();
            this._hsbSpeed_6 = new System.Windows.Forms.HScrollBar();
            this._cmdSwitch_61 = new System.Windows.Forms.Button();
            this._cmdSwitch_60 = new System.Windows.Forms.Button();
            this._cmdSwitch_59 = new System.Windows.Forms.Button();
            this._txtSpeed_6 = new System.Windows.Forms.TextBox();
            this.lblYStgMinSpeed = new System.Windows.Forms.Label();
            this.lblYStgMaxSpeed = new System.Windows.Forms.Label();
            this.lblYStgPosition = new System.Windows.Forms.Label();
            this._Label1_38 = new System.Windows.Forms.Label();
            this._Label1_34 = new System.Windows.Forms.Label();
            this._Label1_37 = new System.Windows.Forms.Label();
            this._Label1_33 = new System.Windows.Forms.Label();
            this._Label1_36 = new System.Windows.Forms.Label();
            this._Label1_32 = new System.Windows.Forms.Label();
            this._Label1_35 = new System.Windows.Forms.Label();
            this._Label1_31 = new System.Windows.Forms.Label();
            this.Frame8 = new System.Windows.Forms.GroupBox();
            this._hsbSpeed_5 = new System.Windows.Forms.HScrollBar();
            this._cmdSwitch_58 = new System.Windows.Forms.Button();
            this._cmdSwitch_30 = new System.Windows.Forms.Button();
            this._cmdSwitch_29 = new System.Windows.Forms.Button();
            this._txtSpeed_5 = new System.Windows.Forms.TextBox();
            this._Label1_30 = new System.Windows.Forms.Label();
            this._Label1_26 = new System.Windows.Forms.Label();
            this._Label1_29 = new System.Windows.Forms.Label();
            this.lblXStgMinSpeed = new System.Windows.Forms.Label();
            this._Label1_25 = new System.Windows.Forms.Label();
            this._Label1_28 = new System.Windows.Forms.Label();
            this.lblXStgMaxSpeed = new System.Windows.Forms.Label();
            this._Label1_24 = new System.Windows.Forms.Label();
            this.lblXStgPosition = new System.Windows.Forms.Label();
            this._Label1_27 = new System.Windows.Forms.Label();
            this._Label1_23 = new System.Windows.Forms.Label();
            this._Frame12_0 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_57 = new System.Windows.Forms.Button();
            this._cmdSwitch_56 = new System.Windows.Forms.Button();
            this.Frame1 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_108 = new System.Windows.Forms.Button();
            this._cmdSwitch_55 = new System.Windows.Forms.Button();
            this._txtSpeed_0 = new System.Windows.Forms.TextBox();
            this._cmdSwitch_1 = new System.Windows.Forms.Button();
            this._cmdSwitch_2 = new System.Windows.Forms.Button();
            this._hsbSpeed_0 = new System.Windows.Forms.HScrollBar();
            this._cmdSwitch_45 = new System.Windows.Forms.Button();
            this._cmdSwitch_46 = new System.Windows.Forms.Button();
            this.txtXIndexPosition = new System.Windows.Forms.TextBox();
            this._Label1_0 = new System.Windows.Forms.Label();
            this._Label1_40 = new System.Windows.Forms.Label();
            this._Label1_41 = new System.Windows.Forms.Label();
            this.Frame2 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_72 = new System.Windows.Forms.Button();
            this._txtSpeed_1 = new System.Windows.Forms.TextBox();
            this._cmdSwitch_3 = new System.Windows.Forms.Button();
            this._cmdSwitch_4 = new System.Windows.Forms.Button();
            this._hsbSpeed_1 = new System.Windows.Forms.HScrollBar();
            this._cmdSwitch_43 = new System.Windows.Forms.Button();
            this.txtYIndexPosition = new System.Windows.Forms.TextBox();
            this._cmdSwitch_44 = new System.Windows.Forms.Button();
            this._Label1_42 = new System.Windows.Forms.Label();
            this._Label1_43 = new System.Windows.Forms.Label();
            this._Label1_44 = new System.Windows.Forms.Label();
            this.Frame3 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_142 = new System.Windows.Forms.Button();
            this.txtIIIndexPosition = new System.Windows.Forms.TextBox();
            this._hsbSpeed_4 = new System.Windows.Forms.HScrollBar();
            this._txtSpeed_4 = new System.Windows.Forms.TextBox();
            this._cmdSwitch_6 = new System.Windows.Forms.Button();
            this._cmdSwitch_5 = new System.Windows.Forms.Button();
            this._cmdSwitch_7 = new System.Windows.Forms.Button();
            this._cmdSwitch_8 = new System.Windows.Forms.Button();
            this._Label1_47 = new System.Windows.Forms.Label();
            this._Label1_46 = new System.Windows.Forms.Label();
            this._Label1_45 = new System.Windows.Forms.Label();
            this.Frame4 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_11 = new System.Windows.Forms.Button();
            this._cmdSwitch_10 = new System.Windows.Forms.Button();
            this._cmdSwitch_9 = new System.Windows.Forms.Button();
            this.Frame5 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_22 = new System.Windows.Forms.Button();
            this._cmdSwitch_23 = new System.Windows.Forms.Button();
            this._cmdSwitch_24 = new System.Windows.Forms.Button();
            this._cmdSwitch_25 = new System.Windows.Forms.Button();
            this._cmdSwitch_18 = new System.Windows.Forms.Button();
            this._cmdSwitch_19 = new System.Windows.Forms.Button();
            this._cmdSwitch_20 = new System.Windows.Forms.Button();
            this._cmdSwitch_21 = new System.Windows.Forms.Button();
            this.Frame6 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_149 = new System.Windows.Forms.Button();
            this._cmdSwitch_12 = new System.Windows.Forms.Button();
            this._cmdSwitch_13 = new System.Windows.Forms.Button();
            this._cmdSwitch_14 = new System.Windows.Forms.Button();
            this._cmdSwitch_15 = new System.Windows.Forms.Button();
            this._cmdSwitch_16 = new System.Windows.Forms.Button();
            this._cmdSwitch_17 = new System.Windows.Forms.Button();
            this.Frame7 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_28 = new System.Windows.Forms.Button();
            this._cmdSwitch_27 = new System.Windows.Forms.Button();
            this._cmdSwitch_26 = new System.Windows.Forms.Button();
            this._cmdSwitch_37 = new System.Windows.Forms.Button();
            this._cmdSwitch_38 = new System.Windows.Forms.Button();
            this.Frame9 = new System.Windows.Forms.GroupBox();
            this.txtTrgCycleTime = new System.Windows.Forms.TextBox();
            this._cmdSwitch_63 = new System.Windows.Forms.Button();
            this.txtTrgTime = new System.Windows.Forms.TextBox();
            this.txtScanStartPos = new System.Windows.Forms.TextBox();
            this._cmdSwitch_62 = new System.Windows.Forms.Button();
            this._txtSpeed_2 = new System.Windows.Forms.TextBox();
            this._cmdSwitch_31 = new System.Windows.Forms.Button();
            this._cmdSwitch_33 = new System.Windows.Forms.Button();
            this._cmdSwitch_32 = new System.Windows.Forms.Button();
            this._hsbSpeed_2 = new System.Windows.Forms.HScrollBar();
            this._Label1_58 = new System.Windows.Forms.Label();
            this._Label1_57 = new System.Windows.Forms.Label();
            this._Label1_14 = new System.Windows.Forms.Label();
            this._Label1_7 = new System.Windows.Forms.Label();
            this._Label1_13 = new System.Windows.Forms.Label();
            this._Label1_6 = new System.Windows.Forms.Label();
            this._Label1_11 = new System.Windows.Forms.Label();
            this.lblRotAccelTm = new System.Windows.Forms.Label();
            this._Label1_4 = new System.Windows.Forms.Label();
            this._Label1_1 = new System.Windows.Forms.Label();
            this._Label1_8 = new System.Windows.Forms.Label();
            this.lblRotPosition = new System.Windows.Forms.Label();
            this._Label1_2 = new System.Windows.Forms.Label();
            this.lblRotMaxSpeed = new System.Windows.Forms.Label();
            this._Label1_9 = new System.Windows.Forms.Label();
            this._Label1_3 = new System.Windows.Forms.Label();
            this.lblRotMinSpeed = new System.Windows.Forms.Label();
            this._Label1_10 = new System.Windows.Forms.Label();
            this._Label1_5 = new System.Windows.Forms.Label();
            this._Label1_12 = new System.Windows.Forms.Label();
            this.Frame10 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_150 = new System.Windows.Forms.Button();
            this._txtSpeed_3 = new System.Windows.Forms.TextBox();
            this._cmdSwitch_36 = new System.Windows.Forms.Button();
            this._cmdSwitch_34 = new System.Windows.Forms.Button();
            this._cmdSwitch_35 = new System.Windows.Forms.Button();
            this._hsbSpeed_3 = new System.Windows.Forms.HScrollBar();
            this._Label1_15 = new System.Windows.Forms.Label();
            this._Label1_19 = new System.Windows.Forms.Label();
            this._Label1_16 = new System.Windows.Forms.Label();
            this._Label1_20 = new System.Windows.Forms.Label();
            this._Label1_17 = new System.Windows.Forms.Label();
            this._Label1_21 = new System.Windows.Forms.Label();
            this._Label1_18 = new System.Windows.Forms.Label();
            this._Label1_22 = new System.Windows.Forms.Label();
            this.lblUDPosition = new System.Windows.Forms.Label();
            this.lblUDMaxSpeed = new System.Windows.Forms.Label();
            this.lblUDMinSpeed = new System.Windows.Forms.Label();
            this.Frame11 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_0 = new System.Windows.Forms.Button();
            this.Frame13 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_39 = new System.Windows.Forms.Button();
            this._cmdSwitch_40 = new System.Windows.Forms.Button();
            this.Frame14 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_41 = new System.Windows.Forms.Button();
            this._cmdSwitch_42 = new System.Windows.Forms.Button();
            this.lblXray = new System.Windows.Forms.Label();
            this._Label1_39 = new System.Windows.Forms.Label();
            this._Frame15_0 = new System.Windows.Forms.GroupBox();
            this._cmdSwitch_104 = new System.Windows.Forms.Button();
            this._cmdSwitch_91 = new System.Windows.Forms.Button();
            this._cmdSwitch_90 = new System.Windows.Forms.Button();
            this._cmdSwitch_89 = new System.Windows.Forms.Button();
            this._cmdSwitch_88 = new System.Windows.Forms.Button();
            this._cmdSwitch_54 = new System.Windows.Forms.Button();
            this._cmdSwitch_53 = new System.Windows.Forms.Button();
            this._cmdSwitch_52 = new System.Windows.Forms.Button();
            this._cmdSwitch_51 = new System.Windows.Forms.Button();
            this._cmdSwitch_47 = new System.Windows.Forms.Button();
            this._cmdSwitch_48 = new System.Windows.Forms.Button();
            this._cmdSwitch_49 = new System.Windows.Forms.Button();
            this._cmdSwitch_50 = new System.Windows.Forms.Button();
            this._Frame12_6.SuspendLayout();
            this._Frame12_5.SuspendLayout();
            this._Frame12_4.SuspendLayout();
            this._Frame12_3.SuspendLayout();
            this.Frame28.SuspendLayout();
            this.Frame27.SuspendLayout();
            this.Frame26.SuspendLayout();
            this._Frame12_2.SuspendLayout();
            this._Frame15_1.SuspendLayout();
            this.Frame25.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this._Frame12_1.SuspendLayout();
            this.Frame24.SuspendLayout();
            this.Frame23.SuspendLayout();
            this.Frame22.SuspendLayout();
            this.Frame21.SuspendLayout();
            this.Frame20.SuspendLayout();
            this.Frame19.SuspendLayout();
            this.Frame18.SuspendLayout();
            this.Frame17.SuspendLayout();
            this.Frame16.SuspendLayout();
            this.Frame8.SuspendLayout();
            this._Frame12_0.SuspendLayout();
            this.Frame1.SuspendLayout();
            this.Frame2.SuspendLayout();
            this.Frame3.SuspendLayout();
            this.Frame4.SuspendLayout();
            this.Frame5.SuspendLayout();
            this.Frame6.SuspendLayout();
            this.Frame7.SuspendLayout();
            this.Frame9.SuspendLayout();
            this.Frame10.SuspendLayout();
            this.Frame11.SuspendLayout();
            this.Frame13.SuspendLayout();
            this.Frame14.SuspendLayout();
            this._Frame15_0.SuspendLayout();
            this.SuspendLayout();
            // 
            // _Frame12_6
            // 
            this._Frame12_6.BackColor = System.Drawing.SystemColors.Control;
            this._Frame12_6.Controls.Add(this._cmdSwitch_148);
            this._Frame12_6.Controls.Add(this._cmdSwitch_147);
            this._Frame12_6.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Frame12_6.Location = new System.Drawing.Point(448, 696);
            this._Frame12_6.Name = "_Frame12_6";
            this._Frame12_6.Padding = new System.Windows.Forms.Padding(0);
            this._Frame12_6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Frame12_6.Size = new System.Drawing.Size(81, 49);
            this._Frame12_6.TabIndex = 300;
            this._Frame12_6.TabStop = false;
            this._Frame12_6.Text = "ﾃｰﾌﾞﾙY減速";
            // 
            // _cmdSwitch_148
            // 
            this._cmdSwitch_148.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_148.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_148.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_148.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_148.Location = new System.Drawing.Point(8, 16);
            this._cmdSwitch_148.Name = "_cmdSwitch_148";
            this._cmdSwitch_148.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_148.Size = new System.Drawing.Size(33, 25);
            this._cmdSwitch_148.TabIndex = 302;
            this._cmdSwitch_148.Text = "有効";
            this._cmdSwitch_148.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_147
            // 
            this._cmdSwitch_147.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_147.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_147.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_147.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_147.Location = new System.Drawing.Point(40, 16);
            this._cmdSwitch_147.Name = "_cmdSwitch_147";
            this._cmdSwitch_147.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_147.Size = new System.Drawing.Size(33, 25);
            this._cmdSwitch_147.TabIndex = 301;
            this._cmdSwitch_147.Text = "無効";
            this._cmdSwitch_147.UseVisualStyleBackColor = false;
            // 
            // _Frame12_5
            // 
            this._Frame12_5.BackColor = System.Drawing.SystemColors.Control;
            this._Frame12_5.Controls.Add(this._cmdSwitch_146);
            this._Frame12_5.Controls.Add(this._cmdSwitch_145);
            this._Frame12_5.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Frame12_5.Location = new System.Drawing.Point(360, 696);
            this._Frame12_5.Name = "_Frame12_5";
            this._Frame12_5.Padding = new System.Windows.Forms.Padding(0);
            this._Frame12_5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Frame12_5.Size = new System.Drawing.Size(81, 49);
            this._Frame12_5.TabIndex = 297;
            this._Frame12_5.TabStop = false;
            this._Frame12_5.Text = "制限自動復帰";
            // 
            // _cmdSwitch_146
            // 
            this._cmdSwitch_146.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_146.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_146.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_146.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_146.Location = new System.Drawing.Point(8, 16);
            this._cmdSwitch_146.Name = "_cmdSwitch_146";
            this._cmdSwitch_146.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_146.Size = new System.Drawing.Size(33, 25);
            this._cmdSwitch_146.TabIndex = 299;
            this._cmdSwitch_146.Text = "有効";
            this._cmdSwitch_146.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_145
            // 
            this._cmdSwitch_145.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_145.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_145.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_145.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_145.Location = new System.Drawing.Point(40, 16);
            this._cmdSwitch_145.Name = "_cmdSwitch_145";
            this._cmdSwitch_145.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_145.Size = new System.Drawing.Size(33, 25);
            this._cmdSwitch_145.TabIndex = 298;
            this._cmdSwitch_145.Text = "無効";
            this._cmdSwitch_145.UseVisualStyleBackColor = false;
            // 
            // _Frame12_4
            // 
            this._Frame12_4.BackColor = System.Drawing.SystemColors.Control;
            this._Frame12_4.Controls.Add(this._cmdSwitch_144);
            this._Frame12_4.Controls.Add(this._cmdSwitch_143);
            this._Frame12_4.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Frame12_4.Location = new System.Drawing.Point(272, 696);
            this._Frame12_4.Name = "_Frame12_4";
            this._Frame12_4.Padding = new System.Windows.Forms.Padding(0);
            this._Frame12_4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Frame12_4.Size = new System.Drawing.Size(81, 49);
            this._Frame12_4.TabIndex = 294;
            this._Frame12_4.TabStop = false;
            this._Frame12_4.Text = "ﾒｶﾘｾｯﾄ";
            // 
            // _cmdSwitch_144
            // 
            this._cmdSwitch_144.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_144.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_144.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_144.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_144.Location = new System.Drawing.Point(8, 16);
            this._cmdSwitch_144.Name = "_cmdSwitch_144";
            this._cmdSwitch_144.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_144.Size = new System.Drawing.Size(33, 25);
            this._cmdSwitch_144.TabIndex = 296;
            this._cmdSwitch_144.Text = "実行";
            this._cmdSwitch_144.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_143
            // 
            this._cmdSwitch_143.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_143.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_143.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_143.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_143.Location = new System.Drawing.Point(40, 16);
            this._cmdSwitch_143.Name = "_cmdSwitch_143";
            this._cmdSwitch_143.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_143.Size = new System.Drawing.Size(33, 25);
            this._cmdSwitch_143.TabIndex = 295;
            this._cmdSwitch_143.Text = "停止";
            this._cmdSwitch_143.UseVisualStyleBackColor = false;
            // 
            // _Frame12_3
            // 
            this._Frame12_3.BackColor = System.Drawing.SystemColors.Control;
            this._Frame12_3.Controls.Add(this._cmdSwitch_133);
            this._Frame12_3.Controls.Add(this._cmdSwitch_132);
            this._Frame12_3.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._Frame12_3.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Frame12_3.Location = new System.Drawing.Point(816, 600);
            this._Frame12_3.Name = "_Frame12_3";
            this._Frame12_3.Padding = new System.Windows.Forms.Padding(0);
            this._Frame12_3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Frame12_3.Size = new System.Drawing.Size(65, 121);
            this._Frame12_3.TabIndex = 285;
            this._Frame12_3.TabStop = false;
            this._Frame12_3.Text = "ｳｫｰﾑｱｯﾌﾟ";
            // 
            // _cmdSwitch_133
            // 
            this._cmdSwitch_133.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_133.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_133.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_133.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_133.Location = new System.Drawing.Point(8, 80);
            this._cmdSwitch_133.Name = "_cmdSwitch_133";
            this._cmdSwitch_133.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_133.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_133.TabIndex = 287;
            this._cmdSwitch_133.Text = "OFF中";
            this._cmdSwitch_133.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_132
            // 
            this._cmdSwitch_132.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_132.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_132.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_132.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_132.Location = new System.Drawing.Point(8, 32);
            this._cmdSwitch_132.Name = "_cmdSwitch_132";
            this._cmdSwitch_132.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_132.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_132.TabIndex = 286;
            this._cmdSwitch_132.Text = "ON中";
            this._cmdSwitch_132.UseVisualStyleBackColor = false;
            // 
            // Frame28
            // 
            this.Frame28.BackColor = System.Drawing.SystemColors.Control;
            this.Frame28.Controls.Add(this._cmdSwitch_141);
            this.Frame28.Controls.Add(this._cmdSwitch_140);
            this.Frame28.Controls.Add(this._cmdSwitch_139);
            this.Frame28.Controls.Add(this._cmdSwitch_138);
            this.Frame28.Controls.Add(this._cmdSwitch_137);
            this.Frame28.Controls.Add(this._cmdSwitch_136);
            this.Frame28.Controls.Add(this._cmdSwitch_135);
            this.Frame28.Controls.Add(this._cmdSwitch_134);
            this.Frame28.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame28.Location = new System.Drawing.Point(8, 656);
            this.Frame28.Name = "Frame28";
            this.Frame28.Padding = new System.Windows.Forms.Padding(0);
            this.Frame28.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame28.Size = new System.Drawing.Size(257, 89);
            this.Frame28.TabIndex = 281;
            this.Frame28.TabStop = false;
            this.Frame28.Text = "Ｉ．Ｉ．切替";
            // 
            // _cmdSwitch_141
            // 
            this._cmdSwitch_141.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_141.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_141.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_141.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_141.Location = new System.Drawing.Point(192, 48);
            this._cmdSwitch_141.Name = "_cmdSwitch_141";
            this._cmdSwitch_141.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_141.Size = new System.Drawing.Size(57, 33);
            this._cmdSwitch_141.TabIndex = 292;
            this._cmdSwitch_141.Text = "高速切替禁止";
            this._cmdSwitch_141.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_140
            // 
            this._cmdSwitch_140.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_140.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_140.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_140.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_140.Location = new System.Drawing.Point(64, 48);
            this._cmdSwitch_140.Name = "_cmdSwitch_140";
            this._cmdSwitch_140.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_140.Size = new System.Drawing.Size(57, 33);
            this._cmdSwitch_140.TabIndex = 291;
            this._cmdSwitch_140.Text = "CT切替禁止";
            this._cmdSwitch_140.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_139
            // 
            this._cmdSwitch_139.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_139.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_139.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_139.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_139.Location = new System.Drawing.Point(136, 48);
            this._cmdSwitch_139.Name = "_cmdSwitch_139";
            this._cmdSwitch_139.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_139.Size = new System.Drawing.Size(57, 33);
            this._cmdSwitch_139.TabIndex = 290;
            this._cmdSwitch_139.Text = "高速切替許可";
            this._cmdSwitch_139.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_138
            // 
            this._cmdSwitch_138.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_138.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_138.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_138.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_138.Location = new System.Drawing.Point(8, 48);
            this._cmdSwitch_138.Name = "_cmdSwitch_138";
            this._cmdSwitch_138.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_138.Size = new System.Drawing.Size(57, 33);
            this._cmdSwitch_138.TabIndex = 289;
            this._cmdSwitch_138.Text = "CT切替許可";
            this._cmdSwitch_138.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_137
            // 
            this._cmdSwitch_137.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_137.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_137.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_137.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_137.Location = new System.Drawing.Point(136, 16);
            this._cmdSwitch_137.Name = "_cmdSwitch_137";
            this._cmdSwitch_137.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_137.Size = new System.Drawing.Size(57, 33);
            this._cmdSwitch_137.TabIndex = 288;
            this._cmdSwitch_137.Text = "切替ﾘｾｯﾄ";
            this._cmdSwitch_137.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_136
            // 
            this._cmdSwitch_136.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_136.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_136.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_136.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_136.Location = new System.Drawing.Point(64, 16);
            this._cmdSwitch_136.Name = "_cmdSwitch_136";
            this._cmdSwitch_136.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_136.Size = new System.Drawing.Size(57, 33);
            this._cmdSwitch_136.TabIndex = 284;
            this._cmdSwitch_136.Text = "高速用Ｉ．Ｉ．";
            this._cmdSwitch_136.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_135
            // 
            this._cmdSwitch_135.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_135.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_135.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_135.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_135.Location = new System.Drawing.Point(8, 16);
            this._cmdSwitch_135.Name = "_cmdSwitch_135";
            this._cmdSwitch_135.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_135.Size = new System.Drawing.Size(57, 33);
            this._cmdSwitch_135.TabIndex = 283;
            this._cmdSwitch_135.Text = "CT用Ｉ．Ｉ．";
            this._cmdSwitch_135.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_134
            // 
            this._cmdSwitch_134.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_134.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_134.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_134.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_134.Location = new System.Drawing.Point(192, 16);
            this._cmdSwitch_134.Name = "_cmdSwitch_134";
            this._cmdSwitch_134.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_134.Size = new System.Drawing.Size(57, 33);
            this._cmdSwitch_134.TabIndex = 282;
            this._cmdSwitch_134.Text = "切替停止";
            this._cmdSwitch_134.UseVisualStyleBackColor = false;
            // 
            // Frame27
            // 
            this.Frame27.BackColor = System.Drawing.SystemColors.Control;
            this.Frame27.Controls.Add(this._cmdSwitch_131);
            this.Frame27.Controls.Add(this._cmdSwitch_130);
            this.Frame27.Controls.Add(this._cmdSwitch_129);
            this.Frame27.Controls.Add(this._cmdSwitch_128);
            this.Frame27.Controls.Add(this._cmdSwitch_127);
            this.Frame27.Controls.Add(this._cmdSwitch_126);
            this.Frame27.Controls.Add(this._cmdSwitch_125);
            this.Frame27.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame27.Location = new System.Drawing.Point(8, 560);
            this.Frame27.Name = "Frame27";
            this.Frame27.Padding = new System.Windows.Forms.Padding(0);
            this.Frame27.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame27.Size = new System.Drawing.Size(153, 89);
            this.Frame27.TabIndex = 273;
            this.Frame27.TabStop = false;
            this.Frame27.Text = "高速Ｉ．Ｉ．視野";
            // 
            // _cmdSwitch_131
            // 
            this._cmdSwitch_131.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_131.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_131.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_131.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_131.Location = new System.Drawing.Point(56, 48);
            this._cmdSwitch_131.Name = "_cmdSwitch_131";
            this._cmdSwitch_131.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_131.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_131.TabIndex = 280;
            this._cmdSwitch_131.Text = "ｶﾒﾗ電源OFF";
            this._cmdSwitch_131.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_130
            // 
            this._cmdSwitch_130.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_130.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_130.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_130.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_130.Location = new System.Drawing.Point(8, 48);
            this._cmdSwitch_130.Name = "_cmdSwitch_130";
            this._cmdSwitch_130.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_130.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_130.TabIndex = 279;
            this._cmdSwitch_130.Text = "ｶﾒﾗ電源ON";
            this._cmdSwitch_130.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_129
            // 
            this._cmdSwitch_129.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_129.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_129.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_129.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_129.Location = new System.Drawing.Point(40, 16);
            this._cmdSwitch_129.Name = "_cmdSwitch_129";
            this._cmdSwitch_129.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_129.Size = new System.Drawing.Size(33, 33);
            this._cmdSwitch_129.TabIndex = 278;
            this._cmdSwitch_129.Text = "電源OFF";
            this._cmdSwitch_129.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_128
            // 
            this._cmdSwitch_128.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_128.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_128.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_128.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_128.Location = new System.Drawing.Point(8, 16);
            this._cmdSwitch_128.Name = "_cmdSwitch_128";
            this._cmdSwitch_128.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_128.Size = new System.Drawing.Size(33, 33);
            this._cmdSwitch_128.TabIndex = 277;
            this._cmdSwitch_128.Text = "電源ON";
            this._cmdSwitch_128.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_127
            // 
            this._cmdSwitch_127.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_127.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_127.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_127.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_127.Location = new System.Drawing.Point(72, 16);
            this._cmdSwitch_127.Name = "_cmdSwitch_127";
            this._cmdSwitch_127.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_127.Size = new System.Drawing.Size(25, 33);
            this._cmdSwitch_127.TabIndex = 276;
            this._cmdSwitch_127.Text = "９";
            this._cmdSwitch_127.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_126
            // 
            this._cmdSwitch_126.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_126.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_126.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_126.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_126.Location = new System.Drawing.Point(96, 16);
            this._cmdSwitch_126.Name = "_cmdSwitch_126";
            this._cmdSwitch_126.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_126.Size = new System.Drawing.Size(25, 33);
            this._cmdSwitch_126.TabIndex = 275;
            this._cmdSwitch_126.Text = "６";
            this._cmdSwitch_126.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_125
            // 
            this._cmdSwitch_125.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_125.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_125.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_125.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_125.Location = new System.Drawing.Point(120, 16);
            this._cmdSwitch_125.Name = "_cmdSwitch_125";
            this._cmdSwitch_125.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_125.Size = new System.Drawing.Size(25, 33);
            this._cmdSwitch_125.TabIndex = 274;
            this._cmdSwitch_125.Text = "4.5";
            this._cmdSwitch_125.UseVisualStyleBackColor = false;
            // 
            // Frame26
            // 
            this.Frame26.BackColor = System.Drawing.SystemColors.Control;
            this.Frame26.Controls.Add(this._cmdSwitch_124);
            this.Frame26.Controls.Add(this._cmdSwitch_123);
            this.Frame26.Controls.Add(this._cmdSwitch_122);
            this.Frame26.Controls.Add(this._cmdSwitch_121);
            this.Frame26.Controls.Add(this._cmdSwitch_120);
            this.Frame26.Controls.Add(this._cmdSwitch_119);
            this.Frame26.Controls.Add(this._cmdSwitch_118);
            this.Frame26.Controls.Add(this._cmdSwitch_109);
            this.Frame26.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame26.Location = new System.Drawing.Point(544, 104);
            this.Frame26.Name = "Frame26";
            this.Frame26.Padding = new System.Windows.Forms.Padding(0);
            this.Frame26.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame26.Size = new System.Drawing.Size(193, 89);
            this.Frame26.TabIndex = 264;
            this.Frame26.TabStop = false;
            this.Frame26.Text = "Ｉ．Ｉ．絞り";
            // 
            // _cmdSwitch_124
            // 
            this._cmdSwitch_124.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_124.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_124.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_124.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_124.Location = new System.Drawing.Point(144, 32);
            this._cmdSwitch_124.Name = "_cmdSwitch_124";
            this._cmdSwitch_124.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_124.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_124.TabIndex = 272;
            this._cmdSwitch_124.Text = "下閉";
            this._cmdSwitch_124.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_123
            // 
            this._cmdSwitch_123.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_123.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_123.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_123.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_123.Location = new System.Drawing.Point(144, 56);
            this._cmdSwitch_123.Name = "_cmdSwitch_123";
            this._cmdSwitch_123.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_123.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_123.TabIndex = 271;
            this._cmdSwitch_123.Text = "下開";
            this._cmdSwitch_123.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_122
            // 
            this._cmdSwitch_122.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_122.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_122.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_122.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_122.Location = new System.Drawing.Point(96, 56);
            this._cmdSwitch_122.Name = "_cmdSwitch_122";
            this._cmdSwitch_122.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_122.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_122.TabIndex = 270;
            this._cmdSwitch_122.Text = "上閉";
            this._cmdSwitch_122.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_121
            // 
            this._cmdSwitch_121.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_121.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_121.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_121.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_121.Location = new System.Drawing.Point(96, 32);
            this._cmdSwitch_121.Name = "_cmdSwitch_121";
            this._cmdSwitch_121.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_121.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_121.TabIndex = 269;
            this._cmdSwitch_121.Text = "上開";
            this._cmdSwitch_121.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_120
            // 
            this._cmdSwitch_120.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_120.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_120.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_120.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_120.Location = new System.Drawing.Point(8, 56);
            this._cmdSwitch_120.Name = "_cmdSwitch_120";
            this._cmdSwitch_120.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_120.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_120.TabIndex = 268;
            this._cmdSwitch_120.Text = "右閉";
            this._cmdSwitch_120.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_119
            // 
            this._cmdSwitch_119.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_119.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_119.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_119.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_119.Location = new System.Drawing.Point(48, 56);
            this._cmdSwitch_119.Name = "_cmdSwitch_119";
            this._cmdSwitch_119.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_119.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_119.TabIndex = 267;
            this._cmdSwitch_119.Text = "右開";
            this._cmdSwitch_119.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_118
            // 
            this._cmdSwitch_118.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_118.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_118.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_118.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_118.Location = new System.Drawing.Point(48, 24);
            this._cmdSwitch_118.Name = "_cmdSwitch_118";
            this._cmdSwitch_118.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_118.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_118.TabIndex = 266;
            this._cmdSwitch_118.Text = "左閉";
            this._cmdSwitch_118.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_109
            // 
            this._cmdSwitch_109.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_109.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_109.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_109.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_109.Location = new System.Drawing.Point(8, 24);
            this._cmdSwitch_109.Name = "_cmdSwitch_109";
            this._cmdSwitch_109.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_109.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_109.TabIndex = 265;
            this._cmdSwitch_109.Text = "左開";
            this._cmdSwitch_109.UseVisualStyleBackColor = false;
            // 
            // _Frame12_2
            // 
            this._Frame12_2.BackColor = System.Drawing.SystemColors.Control;
            this._Frame12_2.Controls.Add(this._cmdSwitch_107);
            this._Frame12_2.Controls.Add(this._cmdSwitch_106);
            this._Frame12_2.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Frame12_2.Location = new System.Drawing.Point(168, 600);
            this._Frame12_2.Name = "_Frame12_2";
            this._Frame12_2.Padding = new System.Windows.Forms.Padding(0);
            this._Frame12_2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Frame12_2.Size = new System.Drawing.Size(89, 49);
            this._Frame12_2.TabIndex = 260;
            this._Frame12_2.TabStop = false;
            this._Frame12_2.Text = "扉電磁ﾛｯｸ";
            // 
            // _cmdSwitch_107
            // 
            this._cmdSwitch_107.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_107.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_107.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_107.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_107.Location = new System.Drawing.Point(8, 16);
            this._cmdSwitch_107.Name = "_cmdSwitch_107";
            this._cmdSwitch_107.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_107.Size = new System.Drawing.Size(33, 25);
            this._cmdSwitch_107.TabIndex = 262;
            this._cmdSwitch_107.Text = "ON";
            this._cmdSwitch_107.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_106
            // 
            this._cmdSwitch_106.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_106.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_106.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_106.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_106.Location = new System.Drawing.Point(48, 16);
            this._cmdSwitch_106.Name = "_cmdSwitch_106";
            this._cmdSwitch_106.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_106.Size = new System.Drawing.Size(33, 25);
            this._cmdSwitch_106.TabIndex = 261;
            this._cmdSwitch_106.Text = "OFF";
            this._cmdSwitch_106.UseVisualStyleBackColor = false;
            // 
            // _Frame15_1
            // 
            this._Frame15_1.BackColor = System.Drawing.SystemColors.Control;
            this._Frame15_1.Controls.Add(this._cmdSwitch_117);
            this._Frame15_1.Controls.Add(this._cmdSwitch_116);
            this._Frame15_1.Controls.Add(this._cmdSwitch_115);
            this._Frame15_1.Controls.Add(this._cmdSwitch_114);
            this._Frame15_1.Controls.Add(this._cmdSwitch_113);
            this._Frame15_1.Controls.Add(this._cmdSwitch_112);
            this._Frame15_1.Controls.Add(this._cmdSwitch_111);
            this._Frame15_1.Controls.Add(this._cmdSwitch_110);
            this._Frame15_1.Controls.Add(this._cmdSwitch_105);
            this._Frame15_1.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Frame15_1.Location = new System.Drawing.Point(272, 600);
            this._Frame15_1.Name = "_Frame15_1";
            this._Frame15_1.Padding = new System.Windows.Forms.Padding(0);
            this._Frame15_1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Frame15_1.Size = new System.Drawing.Size(257, 89);
            this._Frame15_1.TabIndex = 250;
            this._Frame15_1.TabStop = false;
            this._Frame15_1.Text = "移動有ｾｯﾄ";
            // 
            // _cmdSwitch_117
            // 
            this._cmdSwitch_117.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_117.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_117.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_117.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_117.Location = new System.Drawing.Point(152, 16);
            this._cmdSwitch_117.Name = "_cmdSwitch_117";
            this._cmdSwitch_117.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_117.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_117.TabIndex = 259;
            this._cmdSwitch_117.Text = "DisY軸";
            this._cmdSwitch_117.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_116
            // 
            this._cmdSwitch_116.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_116.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_116.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_116.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_116.Location = new System.Drawing.Point(104, 16);
            this._cmdSwitch_116.Name = "_cmdSwitch_116";
            this._cmdSwitch_116.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_116.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_116.TabIndex = 258;
            this._cmdSwitch_116.Text = "RotY軸";
            this._cmdSwitch_116.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_115
            // 
            this._cmdSwitch_115.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_115.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_115.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_115.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_115.Location = new System.Drawing.Point(56, 16);
            this._cmdSwitch_115.Name = "_cmdSwitch_115";
            this._cmdSwitch_115.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_115.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_115.TabIndex = 257;
            this._cmdSwitch_115.Text = "DisX軸";
            this._cmdSwitch_115.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_114
            // 
            this._cmdSwitch_114.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_114.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_114.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_114.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_114.Location = new System.Drawing.Point(8, 16);
            this._cmdSwitch_114.Name = "_cmdSwitch_114";
            this._cmdSwitch_114.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_114.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_114.TabIndex = 256;
            this._cmdSwitch_114.Text = "RotX軸";
            this._cmdSwitch_114.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_113
            // 
            this._cmdSwitch_113.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_113.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_113.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_113.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_113.Location = new System.Drawing.Point(8, 48);
            this._cmdSwitch_113.Name = "_cmdSwitch_113";
            this._cmdSwitch_113.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_113.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_113.TabIndex = 255;
            this._cmdSwitch_113.Text = "Ver   I.I.軸";
            this._cmdSwitch_113.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_112
            // 
            this._cmdSwitch_112.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_112.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_112.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_112.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_112.Location = new System.Drawing.Point(56, 48);
            this._cmdSwitch_112.Name = "_cmdSwitch_112";
            this._cmdSwitch_112.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_112.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_112.TabIndex = 254;
            this._cmdSwitch_112.Text = "Rot   I.I.軸";
            this._cmdSwitch_112.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_111
            // 
            this._cmdSwitch_111.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_111.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_111.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_111.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_111.Location = new System.Drawing.Point(104, 48);
            this._cmdSwitch_111.Name = "_cmdSwitch_111";
            this._cmdSwitch_111.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_111.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_111.TabIndex = 253;
            this._cmdSwitch_111.Text = "Gain  I.I.軸";
            this._cmdSwitch_111.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_110
            // 
            this._cmdSwitch_110.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_110.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_110.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_110.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_110.Location = new System.Drawing.Point(152, 48);
            this._cmdSwitch_110.Name = "_cmdSwitch_110";
            this._cmdSwitch_110.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_110.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_110.TabIndex = 252;
            this._cmdSwitch_110.Text = "Dis    I.I.軸";
            this._cmdSwitch_110.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_105
            // 
            this._cmdSwitch_105.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_105.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_105.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_105.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_105.Location = new System.Drawing.Point(200, 48);
            this._cmdSwitch_105.Name = "_cmdSwitch_105";
            this._cmdSwitch_105.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_105.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_105.TabIndex = 251;
            this._cmdSwitch_105.Text = "SP    I.I.軸";
            this._cmdSwitch_105.UseVisualStyleBackColor = false;
            // 
            // Frame25
            // 
            this.Frame25.BackColor = System.Drawing.SystemColors.Control;
            this.Frame25.Controls.Add(this.txtEXMTVSet);
            this.Frame25.Controls.Add(this.txtEXMTCSet);
            this.Frame25.Controls.Add(this.numericUpDown2);
            this.Frame25.Controls.Add(this.numericUpDown1);
            this.Frame25.Controls.Add(this._cmdSwitch_103);
            this.Frame25.Controls.Add(this._cmdSwitch_102);
            this.Frame25.Controls.Add(this._cmdSwitch_101);
            this.Frame25.Controls.Add(this._cmdSwitch_100);
            this.Frame25.Controls.Add(this._cmdSwitch_99);
            this.Frame25.Controls.Add(this._cmdSwitch_98);
            this.Frame25.Controls.Add(this._cmdSwitch_97);
            this.Frame25.Controls.Add(this._cmdSwitch_96);
            this.Frame25.Controls.Add(this._Label1_68);
            this.Frame25.Controls.Add(this._Label1_67);
            this.Frame25.Controls.Add(this._Label1_66);
            this.Frame25.Controls.Add(this._Label1_65);
            this.Frame25.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame25.Location = new System.Drawing.Point(536, 600);
            this.Frame25.Name = "Frame25";
            this.Frame25.Padding = new System.Windows.Forms.Padding(0);
            this.Frame25.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame25.Size = new System.Drawing.Size(273, 121);
            this.Frame25.TabIndex = 232;
            this.Frame25.TabStop = false;
            this.Frame25.Text = "X線EXM";
            // 
            // txtEXMTVSet
            // 
            this.txtEXMTVSet.AcceptsReturn = true;
            this.txtEXMTVSet.BackColor = System.Drawing.SystemColors.Window;
            this.txtEXMTVSet.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEXMTVSet.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtEXMTVSet.Location = new System.Drawing.Point(144, 32);
            this.txtEXMTVSet.MaxLength = 0;
            this.txtEXMTVSet.Name = "txtEXMTVSet";
            this.txtEXMTVSet.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtEXMTVSet.Size = new System.Drawing.Size(33, 19);
            this.txtEXMTVSet.TabIndex = 246;
            this.txtEXMTVSet.Text = "30";
            this.txtEXMTVSet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtEXMTVSet.TextChanged += new System.EventHandler(this.txtEXMTVSet_TextChanged);
            // 
            // txtEXMTCSet
            // 
            this.txtEXMTCSet.AcceptsReturn = true;
            this.txtEXMTCSet.BackColor = System.Drawing.SystemColors.Window;
            this.txtEXMTCSet.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEXMTCSet.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtEXMTCSet.Location = new System.Drawing.Point(144, 80);
            this.txtEXMTCSet.MaxLength = 0;
            this.txtEXMTCSet.Name = "txtEXMTCSet";
            this.txtEXMTCSet.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtEXMTCSet.Size = new System.Drawing.Size(33, 19);
            this.txtEXMTCSet.TabIndex = 248;
            this.txtEXMTCSet.Text = "0.01";
            this.txtEXMTCSet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtEXMTCSet.TextChanged += new System.EventHandler(this.txtEXMTCSet_TextChanged);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.DecimalPlaces = 2;
            this.numericUpDown2.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numericUpDown2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown2.Location = new System.Drawing.Point(178, 79);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(20, 20);
            this.numericUpDown2.TabIndex = 252;
            this.numericUpDown2.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numericUpDown1.Location = new System.Drawing.Point(179, 31);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(20, 20);
            this.numericUpDown1.TabIndex = 251;
            this.numericUpDown1.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // _cmdSwitch_103
            // 
            this._cmdSwitch_103.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_103.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_103.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_103.Location = new System.Drawing.Point(56, 88);
            this._cmdSwitch_103.Name = "_cmdSwitch_103";
            this._cmdSwitch_103.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_103.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_103.TabIndex = 240;
            this._cmdSwitch_103.Text = "解除";
            this._cmdSwitch_103.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_102
            // 
            this._cmdSwitch_102.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_102.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_102.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_102.Location = new System.Drawing.Point(56, 56);
            this._cmdSwitch_102.Name = "_cmdSwitch_102";
            this._cmdSwitch_102.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_102.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_102.TabIndex = 239;
            this._cmdSwitch_102.Text = "長期";
            this._cmdSwitch_102.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_101
            // 
            this._cmdSwitch_101.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_101.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_101.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_101.Location = new System.Drawing.Point(56, 24);
            this._cmdSwitch_101.Name = "_cmdSwitch_101";
            this._cmdSwitch_101.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_101.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_101.TabIndex = 238;
            this._cmdSwitch_101.Text = "短期";
            this._cmdSwitch_101.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_100
            // 
            this._cmdSwitch_100.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_100.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_100.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_100.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_100.Location = new System.Drawing.Point(8, 88);
            this._cmdSwitch_100.Name = "_cmdSwitch_100";
            this._cmdSwitch_100.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_100.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_100.TabIndex = 237;
            this._cmdSwitch_100.Text = "ﾘｾｯﾄ";
            this._cmdSwitch_100.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_99
            // 
            this._cmdSwitch_99.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_99.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_99.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_99.Location = new System.Drawing.Point(224, 80);
            this._cmdSwitch_99.Name = "_cmdSwitch_99";
            this._cmdSwitch_99.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_99.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_99.TabIndex = 236;
            this._cmdSwitch_99.Text = "OFF";
            this._cmdSwitch_99.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_98
            // 
            this._cmdSwitch_98.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_98.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_98.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_98.Location = new System.Drawing.Point(224, 32);
            this._cmdSwitch_98.Name = "_cmdSwitch_98";
            this._cmdSwitch_98.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_98.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_98.TabIndex = 235;
            this._cmdSwitch_98.Text = "ON";
            this._cmdSwitch_98.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_97
            // 
            this._cmdSwitch_97.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_97.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_97.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_97.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_97.Location = new System.Drawing.Point(8, 56);
            this._cmdSwitch_97.Name = "_cmdSwitch_97";
            this._cmdSwitch_97.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_97.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_97.TabIndex = 234;
            this._cmdSwitch_97.Text = "ﾛｰｶﾙ";
            this._cmdSwitch_97.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_96
            // 
            this._cmdSwitch_96.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_96.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_96.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_96.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_96.Location = new System.Drawing.Point(8, 24);
            this._cmdSwitch_96.Name = "_cmdSwitch_96";
            this._cmdSwitch_96.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_96.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_96.TabIndex = 233;
            this._cmdSwitch_96.Text = "ﾘﾓｰﾄ";
            this._cmdSwitch_96.UseVisualStyleBackColor = false;
            // 
            // _Label1_68
            // 
            this._Label1_68.AutoSize = true;
            this._Label1_68.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_68.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_68.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_68.Location = new System.Drawing.Point(200, 88);
            this._Label1_68.Name = "_Label1_68";
            this._Label1_68.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_68.Size = new System.Drawing.Size(22, 12);
            this._Label1_68.TabIndex = 244;
            this._Label1_68.Text = "mA";
            // 
            // _Label1_67
            // 
            this._Label1_67.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_67.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_67.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_67.Location = new System.Drawing.Point(104, 80);
            this._Label1_67.Name = "_Label1_67";
            this._Label1_67.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_67.Size = new System.Drawing.Size(65, 17);
            this._Label1_67.TabIndex = 243;
            this._Label1_67.Text = "管電流";
            // 
            // _Label1_66
            // 
            this._Label1_66.AutoSize = true;
            this._Label1_66.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_66.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_66.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_66.Location = new System.Drawing.Point(200, 40);
            this._Label1_66.Name = "_Label1_66";
            this._Label1_66.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_66.Size = new System.Drawing.Size(19, 12);
            this._Label1_66.TabIndex = 242;
            this._Label1_66.Text = "kV";
            // 
            // _Label1_65
            // 
            this._Label1_65.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_65.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_65.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_65.Location = new System.Drawing.Point(104, 32);
            this._Label1_65.Name = "_Label1_65";
            this._Label1_65.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_65.Size = new System.Drawing.Size(65, 17);
            this._Label1_65.TabIndex = 241;
            this._Label1_65.Text = "管電圧";
            // 
            // _Frame12_1
            // 
            this._Frame12_1.BackColor = System.Drawing.SystemColors.Control;
            this._Frame12_1.Controls.Add(this._cmdSwitch_95);
            this._Frame12_1.Controls.Add(this._cmdSwitch_94);
            this._Frame12_1.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Frame12_1.Location = new System.Drawing.Point(168, 472);
            this._Frame12_1.Name = "_Frame12_1";
            this._Frame12_1.Padding = new System.Windows.Forms.Padding(0);
            this._Frame12_1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Frame12_1.Size = new System.Drawing.Size(89, 49);
            this._Frame12_1.TabIndex = 229;
            this._Frame12_1.TabStop = false;
            this._Frame12_1.Text = "ﾌｧﾝﾄﾑ";
            // 
            // _cmdSwitch_95
            // 
            this._cmdSwitch_95.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_95.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_95.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_95.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_95.Location = new System.Drawing.Point(8, 16);
            this._cmdSwitch_95.Name = "_cmdSwitch_95";
            this._cmdSwitch_95.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_95.Size = new System.Drawing.Size(33, 25);
            this._cmdSwitch_95.TabIndex = 231;
            this._cmdSwitch_95.Text = "ON";
            this._cmdSwitch_95.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_94
            // 
            this._cmdSwitch_94.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_94.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_94.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_94.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_94.Location = new System.Drawing.Point(48, 16);
            this._cmdSwitch_94.Name = "_cmdSwitch_94";
            this._cmdSwitch_94.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_94.Size = new System.Drawing.Size(33, 25);
            this._cmdSwitch_94.TabIndex = 230;
            this._cmdSwitch_94.Text = "OFF";
            this._cmdSwitch_94.UseVisualStyleBackColor = false;
            // 
            // Frame24
            // 
            this.Frame24.BackColor = System.Drawing.SystemColors.Control;
            this.Frame24.Controls.Add(this._cmdSwitch_92);
            this.Frame24.Controls.Add(this._cmdSwitch_93);
            this.Frame24.Controls.Add(this.txtFIDFineLimit);
            this.Frame24.Controls.Add(this.txtFCDFineLimit);
            this.Frame24.Controls.Add(this.txtFCDLimit);
            this.Frame24.Controls.Add(this._Label1_64);
            this.Frame24.Controls.Add(this._Label1_63);
            this.Frame24.Controls.Add(this._Label1_62);
            this.Frame24.Controls.Add(this._Label1_61);
            this.Frame24.Controls.Add(this._Label1_60);
            this.Frame24.Controls.Add(this._Label1_59);
            this.Frame24.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame24.Location = new System.Drawing.Point(536, 488);
            this.Frame24.Name = "Frame24";
            this.Frame24.Padding = new System.Windows.Forms.Padding(0);
            this.Frame24.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame24.Size = new System.Drawing.Size(201, 105);
            this.Frame24.TabIndex = 217;
            this.Frame24.TabStop = false;
            this.Frame24.Text = "ﾃｰﾌﾞﾙｲﾝﾝﾀｰﾛｯｸ";
            // 
            // _cmdSwitch_92
            // 
            this._cmdSwitch_92.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_92.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_92.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_92.Location = new System.Drawing.Point(8, 72);
            this._cmdSwitch_92.Name = "_cmdSwitch_92";
            this._cmdSwitch_92.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_92.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_92.TabIndex = 228;
            this._cmdSwitch_92.Text = "有効";
            this._cmdSwitch_92.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_93
            // 
            this._cmdSwitch_93.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_93.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_93.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_93.Location = new System.Drawing.Point(8, 24);
            this._cmdSwitch_93.Name = "_cmdSwitch_93";
            this._cmdSwitch_93.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_93.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_93.TabIndex = 227;
            this._cmdSwitch_93.Text = "解除";
            this._cmdSwitch_93.UseVisualStyleBackColor = false;
            // 
            // txtFIDFineLimit
            // 
            this.txtFIDFineLimit.AcceptsReturn = true;
            this.txtFIDFineLimit.BackColor = System.Drawing.SystemColors.Window;
            this.txtFIDFineLimit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFIDFineLimit.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtFIDFineLimit.Location = new System.Drawing.Point(120, 80);
            this.txtFIDFineLimit.MaxLength = 0;
            this.txtFIDFineLimit.Name = "txtFIDFineLimit";
            this.txtFIDFineLimit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtFIDFineLimit.Size = new System.Drawing.Size(49, 19);
            this.txtFIDFineLimit.TabIndex = 226;
            this.txtFIDFineLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtFIDFineLimit.TextChanged += new System.EventHandler(this.txtFIDFineLimit_TextChanged);
            // 
            // txtFCDFineLimit
            // 
            this.txtFCDFineLimit.AcceptsReturn = true;
            this.txtFCDFineLimit.BackColor = System.Drawing.SystemColors.Window;
            this.txtFCDFineLimit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFCDFineLimit.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtFCDFineLimit.Location = new System.Drawing.Point(120, 48);
            this.txtFCDFineLimit.MaxLength = 0;
            this.txtFCDFineLimit.Name = "txtFCDFineLimit";
            this.txtFCDFineLimit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtFCDFineLimit.Size = new System.Drawing.Size(49, 19);
            this.txtFCDFineLimit.TabIndex = 225;
            this.txtFCDFineLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtFCDFineLimit.TextChanged += new System.EventHandler(this.txtFCDFineLimit_TextChanged);
            // 
            // txtFCDLimit
            // 
            this.txtFCDLimit.AcceptsReturn = true;
            this.txtFCDLimit.BackColor = System.Drawing.SystemColors.Window;
            this.txtFCDLimit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFCDLimit.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtFCDLimit.Location = new System.Drawing.Point(120, 16);
            this.txtFCDLimit.MaxLength = 0;
            this.txtFCDLimit.Name = "txtFCDLimit";
            this.txtFCDLimit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtFCDLimit.Size = new System.Drawing.Size(49, 19);
            this.txtFCDLimit.TabIndex = 224;
            this.txtFCDLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtFCDLimit.TextChanged += new System.EventHandler(this.txtFCDLimit_TextChanged);
            // 
            // _Label1_64
            // 
            this._Label1_64.AutoSize = true;
            this._Label1_64.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_64.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_64.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_64.Location = new System.Drawing.Point(176, 80);
            this._Label1_64.Name = "_Label1_64";
            this._Label1_64.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_64.Size = new System.Drawing.Size(23, 12);
            this._Label1_64.TabIndex = 223;
            this._Label1_64.Text = "mm";
            // 
            // _Label1_63
            // 
            this._Label1_63.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_63.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_63.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_63.Location = new System.Drawing.Point(64, 72);
            this._Label1_63.Name = "_Label1_63";
            this._Label1_63.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_63.Size = new System.Drawing.Size(62, 24);
            this._Label1_63.TabIndex = 222;
            this._Label1_63.Text = "FID FineLimit：";
            // 
            // _Label1_62
            // 
            this._Label1_62.AutoSize = true;
            this._Label1_62.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_62.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_62.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_62.Location = new System.Drawing.Point(176, 48);
            this._Label1_62.Name = "_Label1_62";
            this._Label1_62.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_62.Size = new System.Drawing.Size(23, 12);
            this._Label1_62.TabIndex = 221;
            this._Label1_62.Text = "mm";
            // 
            // _Label1_61
            // 
            this._Label1_61.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_61.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_61.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_61.Location = new System.Drawing.Point(64, 40);
            this._Label1_61.Name = "_Label1_61";
            this._Label1_61.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_61.Size = new System.Drawing.Size(62, 24);
            this._Label1_61.TabIndex = 220;
            this._Label1_61.Text = "FCD FineLimit：";
            // 
            // _Label1_60
            // 
            this._Label1_60.AutoSize = true;
            this._Label1_60.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_60.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_60.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_60.Location = new System.Drawing.Point(176, 16);
            this._Label1_60.Name = "_Label1_60";
            this._Label1_60.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_60.Size = new System.Drawing.Size(23, 12);
            this._Label1_60.TabIndex = 219;
            this._Label1_60.Text = "mm";
            // 
            // _Label1_59
            // 
            this._Label1_59.AutoSize = true;
            this._Label1_59.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_59.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_59.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_59.Location = new System.Drawing.Point(64, 16);
            this._Label1_59.Name = "_Label1_59";
            this._Label1_59.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_59.Size = new System.Drawing.Size(59, 12);
            this._Label1_59.TabIndex = 218;
            this._Label1_59.Text = "FCDLimit：";
            // 
            // Frame23
            // 
            this.Frame23.BackColor = System.Drawing.SystemColors.Control;
            this.Frame23.Controls.Add(this._cmdSwitch_87);
            this.Frame23.Controls.Add(this._txtSpeed_9);
            this.Frame23.Controls.Add(this._cmdSwitch_86);
            this.Frame23.Controls.Add(this._cmdSwitch_85);
            this.Frame23.Controls.Add(this._hsbSpeed_9);
            this.Frame23.Controls.Add(this._cmdSwitch_84);
            this.Frame23.Controls.Add(this._cmdSwitch_83);
            this.Frame23.Controls.Add(this.txtXrayRotIndexPosition);
            this.Frame23.Controls.Add(this._Label1_56);
            this.Frame23.Controls.Add(this._Label1_55);
            this.Frame23.Controls.Add(this._Label1_54);
            this.Frame23.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame23.Location = new System.Drawing.Point(744, 408);
            this.Frame23.Name = "Frame23";
            this.Frame23.Padding = new System.Windows.Forms.Padding(0);
            this.Frame23.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame23.Size = new System.Drawing.Size(137, 185);
            this.Frame23.TabIndex = 198;
            this.Frame23.TabStop = false;
            this.Frame23.Text = "光学系回転";
            // 
            // _cmdSwitch_87
            // 
            this._cmdSwitch_87.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_87.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_87.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_87.Location = new System.Drawing.Point(80, 120);
            this._cmdSwitch_87.Name = "_cmdSwitch_87";
            this._cmdSwitch_87.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_87.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_87.TabIndex = 206;
            this._cmdSwitch_87.Text = "原点";
            this._cmdSwitch_87.UseVisualStyleBackColor = false;
            // 
            // _txtSpeed_9
            // 
            this._txtSpeed_9.AcceptsReturn = true;
            this._txtSpeed_9.BackColor = System.Drawing.SystemColors.Window;
            this._txtSpeed_9.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._txtSpeed_9.ForeColor = System.Drawing.SystemColors.WindowText;
            this._txtSpeed_9.Location = new System.Drawing.Point(64, 16);
            this._txtSpeed_9.MaxLength = 0;
            this._txtSpeed_9.Multiline = true;
            this._txtSpeed_9.Name = "_txtSpeed_9";
            this._txtSpeed_9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._txtSpeed_9.Size = new System.Drawing.Size(45, 18);
            this._txtSpeed_9.TabIndex = 205;
            this._txtSpeed_9.Text = "0";
            this._txtSpeed_9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _cmdSwitch_86
            // 
            this._cmdSwitch_86.BackColor = System.Drawing.Color.Lime;
            this._cmdSwitch_86.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_86.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_86.Location = new System.Drawing.Point(8, 80);
            this._cmdSwitch_86.Name = "_cmdSwitch_86";
            this._cmdSwitch_86.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_86.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_86.TabIndex = 204;
            this._cmdSwitch_86.Text = "逆転";
            this._cmdSwitch_86.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_85
            // 
            this._cmdSwitch_85.BackColor = System.Drawing.Color.Lime;
            this._cmdSwitch_85.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_85.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_85.Location = new System.Drawing.Point(80, 80);
            this._cmdSwitch_85.Name = "_cmdSwitch_85";
            this._cmdSwitch_85.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_85.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_85.TabIndex = 203;
            this._cmdSwitch_85.Text = "正転";
            this._cmdSwitch_85.UseVisualStyleBackColor = false;
            // 
            // _hsbSpeed_9
            // 
            this._hsbSpeed_9.Cursor = System.Windows.Forms.Cursors.Default;
            this._hsbSpeed_9.LargeChange = 1;
            this._hsbSpeed_9.Location = new System.Drawing.Point(8, 48);
            this._hsbSpeed_9.Maximum = 32767;
            this._hsbSpeed_9.Name = "_hsbSpeed_9";
            this._hsbSpeed_9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._hsbSpeed_9.Size = new System.Drawing.Size(121, 17);
            this._hsbSpeed_9.TabIndex = 202;
            this._hsbSpeed_9.TabStop = true;
            // 
            // _cmdSwitch_84
            // 
            this._cmdSwitch_84.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_84.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_84.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_84.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_84.Location = new System.Drawing.Point(8, 152);
            this._cmdSwitch_84.Name = "_cmdSwitch_84";
            this._cmdSwitch_84.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_84.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_84.TabIndex = 201;
            this._cmdSwitch_84.Text = "位置決";
            this._cmdSwitch_84.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_83
            // 
            this._cmdSwitch_83.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_83.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_83.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_83.Location = new System.Drawing.Point(80, 152);
            this._cmdSwitch_83.Name = "_cmdSwitch_83";
            this._cmdSwitch_83.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_83.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_83.TabIndex = 200;
            this._cmdSwitch_83.Text = "停止";
            this._cmdSwitch_83.UseVisualStyleBackColor = false;
            // 
            // txtXrayRotIndexPosition
            // 
            this.txtXrayRotIndexPosition.AcceptsReturn = true;
            this.txtXrayRotIndexPosition.BackColor = System.Drawing.SystemColors.Window;
            this.txtXrayRotIndexPosition.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtXrayRotIndexPosition.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtXrayRotIndexPosition.Location = new System.Drawing.Point(8, 128);
            this.txtXrayRotIndexPosition.MaxLength = 0;
            this.txtXrayRotIndexPosition.Multiline = true;
            this.txtXrayRotIndexPosition.Name = "txtXrayRotIndexPosition";
            this.txtXrayRotIndexPosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtXrayRotIndexPosition.Size = new System.Drawing.Size(49, 18);
            this.txtXrayRotIndexPosition.TabIndex = 199;
            this.txtXrayRotIndexPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtXrayRotIndexPosition.TextChanged += new System.EventHandler(this.txtXrayRotIndexPosition_TextChanged);
            // 
            // _Label1_56
            // 
            this._Label1_56.AutoSize = true;
            this._Label1_56.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_56.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_56.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_56.Location = new System.Drawing.Point(8, 24);
            this._Label1_56.Name = "_Label1_56";
            this._Label1_56.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_56.Size = new System.Drawing.Size(59, 12);
            this._Label1_56.TabIndex = 209;
            this._Label1_56.Text = "運転速度：";
            // 
            // _Label1_55
            // 
            this._Label1_55.AutoSize = true;
            this._Label1_55.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_55.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_55.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_55.Location = new System.Drawing.Point(112, 24);
            this._Label1_55.Name = "_Label1_55";
            this._Label1_55.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_55.Size = new System.Drawing.Size(24, 12);
            this._Label1_55.TabIndex = 208;
            this._Label1_55.Text = "rpm";
            // 
            // _Label1_54
            // 
            this._Label1_54.AutoSize = true;
            this._Label1_54.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_54.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_54.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_54.Location = new System.Drawing.Point(56, 136);
            this._Label1_54.Name = "_Label1_54";
            this._Label1_54.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_54.Size = new System.Drawing.Size(17, 12);
            this._Label1_54.TabIndex = 207;
            this._Label1_54.Text = "度";
            // 
            // Frame22
            // 
            this.Frame22.BackColor = System.Drawing.SystemColors.Control;
            this.Frame22.Controls.Add(this._cmdSwitch_82);
            this.Frame22.Controls.Add(this.txtXrayYIndexPosition);
            this.Frame22.Controls.Add(this._cmdSwitch_81);
            this.Frame22.Controls.Add(this._hsbSpeed_8);
            this.Frame22.Controls.Add(this._cmdSwitch_80);
            this.Frame22.Controls.Add(this._cmdSwitch_79);
            this.Frame22.Controls.Add(this._txtSpeed_8);
            this.Frame22.Controls.Add(this._cmdSwitch_78);
            this.Frame22.Controls.Add(this._Label1_53);
            this.Frame22.Controls.Add(this._Label1_52);
            this.Frame22.Controls.Add(this._Label1_51);
            this.Frame22.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame22.Location = new System.Drawing.Point(744, 208);
            this.Frame22.Name = "Frame22";
            this.Frame22.Padding = new System.Windows.Forms.Padding(0);
            this.Frame22.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame22.Size = new System.Drawing.Size(137, 185);
            this.Frame22.TabIndex = 186;
            this.Frame22.TabStop = false;
            this.Frame22.Text = "光学系Y";
            // 
            // _cmdSwitch_82
            // 
            this._cmdSwitch_82.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_82.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_82.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_82.Location = new System.Drawing.Point(80, 152);
            this._cmdSwitch_82.Name = "_cmdSwitch_82";
            this._cmdSwitch_82.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_82.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_82.TabIndex = 194;
            this._cmdSwitch_82.Text = "停止";
            this._cmdSwitch_82.UseVisualStyleBackColor = false;
            // 
            // txtXrayYIndexPosition
            // 
            this.txtXrayYIndexPosition.AcceptsReturn = true;
            this.txtXrayYIndexPosition.BackColor = System.Drawing.SystemColors.Window;
            this.txtXrayYIndexPosition.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtXrayYIndexPosition.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtXrayYIndexPosition.Location = new System.Drawing.Point(8, 127);
            this.txtXrayYIndexPosition.MaxLength = 0;
            this.txtXrayYIndexPosition.Name = "txtXrayYIndexPosition";
            this.txtXrayYIndexPosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtXrayYIndexPosition.Size = new System.Drawing.Size(49, 19);
            this.txtXrayYIndexPosition.TabIndex = 193;
            this.txtXrayYIndexPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtXrayYIndexPosition.TextChanged += new System.EventHandler(this.txtXrayYIndexPosition_TextChanged);
            // 
            // _cmdSwitch_81
            // 
            this._cmdSwitch_81.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_81.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_81.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_81.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_81.Location = new System.Drawing.Point(8, 152);
            this._cmdSwitch_81.Name = "_cmdSwitch_81";
            this._cmdSwitch_81.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_81.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_81.TabIndex = 192;
            this._cmdSwitch_81.Text = "位置決";
            this._cmdSwitch_81.UseVisualStyleBackColor = false;
            // 
            // _hsbSpeed_8
            // 
            this._hsbSpeed_8.Cursor = System.Windows.Forms.Cursors.Default;
            this._hsbSpeed_8.LargeChange = 1;
            this._hsbSpeed_8.Location = new System.Drawing.Point(8, 48);
            this._hsbSpeed_8.Maximum = 32767;
            this._hsbSpeed_8.Name = "_hsbSpeed_8";
            this._hsbSpeed_8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._hsbSpeed_8.Size = new System.Drawing.Size(121, 17);
            this._hsbSpeed_8.TabIndex = 191;
            this._hsbSpeed_8.TabStop = true;
            // 
            // _cmdSwitch_80
            // 
            this._cmdSwitch_80.BackColor = System.Drawing.Color.Lime;
            this._cmdSwitch_80.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_80.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_80.Location = new System.Drawing.Point(80, 112);
            this._cmdSwitch_80.Name = "_cmdSwitch_80";
            this._cmdSwitch_80.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_80.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_80.TabIndex = 190;
            this._cmdSwitch_80.Text = "拡大";
            this._cmdSwitch_80.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_79
            // 
            this._cmdSwitch_79.BackColor = System.Drawing.Color.Lime;
            this._cmdSwitch_79.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_79.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_79.Location = new System.Drawing.Point(80, 80);
            this._cmdSwitch_79.Name = "_cmdSwitch_79";
            this._cmdSwitch_79.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_79.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_79.TabIndex = 189;
            this._cmdSwitch_79.Text = "縮小";
            this._cmdSwitch_79.UseVisualStyleBackColor = false;
            // 
            // _txtSpeed_8
            // 
            this._txtSpeed_8.AcceptsReturn = true;
            this._txtSpeed_8.BackColor = System.Drawing.SystemColors.Window;
            this._txtSpeed_8.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._txtSpeed_8.ForeColor = System.Drawing.SystemColors.WindowText;
            this._txtSpeed_8.Location = new System.Drawing.Point(64, 16);
            this._txtSpeed_8.MaxLength = 0;
            this._txtSpeed_8.Multiline = true;
            this._txtSpeed_8.Name = "_txtSpeed_8";
            this._txtSpeed_8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._txtSpeed_8.Size = new System.Drawing.Size(33, 18);
            this._txtSpeed_8.TabIndex = 188;
            this._txtSpeed_8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _cmdSwitch_78
            // 
            this._cmdSwitch_78.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_78.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_78.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_78.Location = new System.Drawing.Point(8, 80);
            this._cmdSwitch_78.Name = "_cmdSwitch_78";
            this._cmdSwitch_78.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_78.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_78.TabIndex = 187;
            this._cmdSwitch_78.Text = "原点";
            this._cmdSwitch_78.UseVisualStyleBackColor = false;
            // 
            // _Label1_53
            // 
            this._Label1_53.AutoSize = true;
            this._Label1_53.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_53.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_53.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_53.Location = new System.Drawing.Point(56, 136);
            this._Label1_53.Name = "_Label1_53";
            this._Label1_53.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_53.Size = new System.Drawing.Size(23, 12);
            this._Label1_53.TabIndex = 197;
            this._Label1_53.Text = "mm";
            // 
            // _Label1_52
            // 
            this._Label1_52.AutoSize = true;
            this._Label1_52.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_52.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_52.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_52.Location = new System.Drawing.Point(96, 24);
            this._Label1_52.Name = "_Label1_52";
            this._Label1_52.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_52.Size = new System.Drawing.Size(35, 12);
            this._Label1_52.TabIndex = 196;
            this._Label1_52.Text = "mm/s";
            // 
            // _Label1_51
            // 
            this._Label1_51.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_51.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_51.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_51.Location = new System.Drawing.Point(8, 24);
            this._Label1_51.Name = "_Label1_51";
            this._Label1_51.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_51.Size = new System.Drawing.Size(65, 17);
            this._Label1_51.TabIndex = 195;
            this._Label1_51.Text = "運転速度：";
            // 
            // Frame21
            // 
            this.Frame21.BackColor = System.Drawing.SystemColors.Control;
            this.Frame21.Controls.Add(this.txtXrayXIndexPosition);
            this.Frame21.Controls.Add(this._cmdSwitch_77);
            this.Frame21.Controls.Add(this._cmdSwitch_76);
            this.Frame21.Controls.Add(this._hsbSpeed_7);
            this.Frame21.Controls.Add(this._cmdSwitch_75);
            this.Frame21.Controls.Add(this._cmdSwitch_74);
            this.Frame21.Controls.Add(this._txtSpeed_7);
            this.Frame21.Controls.Add(this._cmdSwitch_73);
            this.Frame21.Controls.Add(this._Label1_50);
            this.Frame21.Controls.Add(this._Label1_49);
            this.Frame21.Controls.Add(this._Label1_48);
            this.Frame21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame21.Location = new System.Drawing.Point(744, 8);
            this.Frame21.Name = "Frame21";
            this.Frame21.Padding = new System.Windows.Forms.Padding(0);
            this.Frame21.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame21.Size = new System.Drawing.Size(137, 185);
            this.Frame21.TabIndex = 174;
            this.Frame21.TabStop = false;
            this.Frame21.Text = "光学系X";
            // 
            // txtXrayXIndexPosition
            // 
            this.txtXrayXIndexPosition.AcceptsReturn = true;
            this.txtXrayXIndexPosition.BackColor = System.Drawing.SystemColors.Window;
            this.txtXrayXIndexPosition.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtXrayXIndexPosition.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtXrayXIndexPosition.Location = new System.Drawing.Point(8, 128);
            this.txtXrayXIndexPosition.MaxLength = 0;
            this.txtXrayXIndexPosition.Multiline = true;
            this.txtXrayXIndexPosition.Name = "txtXrayXIndexPosition";
            this.txtXrayXIndexPosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtXrayXIndexPosition.Size = new System.Drawing.Size(49, 18);
            this.txtXrayXIndexPosition.TabIndex = 182;
            this.txtXrayXIndexPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtXrayXIndexPosition.TextChanged += new System.EventHandler(this.txtXrayXIndexPosition_TextChanged);
            // 
            // _cmdSwitch_77
            // 
            this._cmdSwitch_77.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_77.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_77.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_77.Location = new System.Drawing.Point(80, 152);
            this._cmdSwitch_77.Name = "_cmdSwitch_77";
            this._cmdSwitch_77.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_77.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_77.TabIndex = 181;
            this._cmdSwitch_77.Text = "停止";
            this._cmdSwitch_77.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_76
            // 
            this._cmdSwitch_76.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_76.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_76.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_76.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_76.Location = new System.Drawing.Point(8, 152);
            this._cmdSwitch_76.Name = "_cmdSwitch_76";
            this._cmdSwitch_76.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_76.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_76.TabIndex = 180;
            this._cmdSwitch_76.Text = "位置決";
            this._cmdSwitch_76.UseVisualStyleBackColor = false;
            // 
            // _hsbSpeed_7
            // 
            this._hsbSpeed_7.Cursor = System.Windows.Forms.Cursors.Default;
            this._hsbSpeed_7.LargeChange = 1;
            this._hsbSpeed_7.Location = new System.Drawing.Point(8, 48);
            this._hsbSpeed_7.Maximum = 32767;
            this._hsbSpeed_7.Name = "_hsbSpeed_7";
            this._hsbSpeed_7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._hsbSpeed_7.Size = new System.Drawing.Size(121, 17);
            this._hsbSpeed_7.TabIndex = 179;
            this._hsbSpeed_7.TabStop = true;
            // 
            // _cmdSwitch_75
            // 
            this._cmdSwitch_75.BackColor = System.Drawing.Color.Lime;
            this._cmdSwitch_75.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_75.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_75.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_75.Location = new System.Drawing.Point(80, 80);
            this._cmdSwitch_75.Name = "_cmdSwitch_75";
            this._cmdSwitch_75.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_75.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_75.TabIndex = 178;
            this._cmdSwitch_75.Text = "右移動";
            this._cmdSwitch_75.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_74
            // 
            this._cmdSwitch_74.BackColor = System.Drawing.Color.Lime;
            this._cmdSwitch_74.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_74.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_74.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_74.Location = new System.Drawing.Point(8, 80);
            this._cmdSwitch_74.Name = "_cmdSwitch_74";
            this._cmdSwitch_74.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_74.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_74.TabIndex = 177;
            this._cmdSwitch_74.Text = "左移動";
            this._cmdSwitch_74.UseVisualStyleBackColor = false;
            // 
            // _txtSpeed_7
            // 
            this._txtSpeed_7.AcceptsReturn = true;
            this._txtSpeed_7.BackColor = System.Drawing.SystemColors.Window;
            this._txtSpeed_7.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._txtSpeed_7.ForeColor = System.Drawing.SystemColors.WindowText;
            this._txtSpeed_7.Location = new System.Drawing.Point(64, 16);
            this._txtSpeed_7.MaxLength = 0;
            this._txtSpeed_7.Multiline = true;
            this._txtSpeed_7.Name = "_txtSpeed_7";
            this._txtSpeed_7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._txtSpeed_7.Size = new System.Drawing.Size(33, 18);
            this._txtSpeed_7.TabIndex = 176;
            this._txtSpeed_7.Text = "0";
            this._txtSpeed_7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _cmdSwitch_73
            // 
            this._cmdSwitch_73.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_73.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_73.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_73.Location = new System.Drawing.Point(80, 120);
            this._cmdSwitch_73.Name = "_cmdSwitch_73";
            this._cmdSwitch_73.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_73.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_73.TabIndex = 175;
            this._cmdSwitch_73.Text = "原点";
            this._cmdSwitch_73.UseVisualStyleBackColor = false;
            // 
            // _Label1_50
            // 
            this._Label1_50.AutoSize = true;
            this._Label1_50.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_50.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_50.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_50.Location = new System.Drawing.Point(56, 136);
            this._Label1_50.Name = "_Label1_50";
            this._Label1_50.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_50.Size = new System.Drawing.Size(23, 12);
            this._Label1_50.TabIndex = 185;
            this._Label1_50.Text = "mm";
            // 
            // _Label1_49
            // 
            this._Label1_49.AutoSize = true;
            this._Label1_49.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_49.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_49.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_49.Location = new System.Drawing.Point(96, 24);
            this._Label1_49.Name = "_Label1_49";
            this._Label1_49.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_49.Size = new System.Drawing.Size(35, 12);
            this._Label1_49.TabIndex = 184;
            this._Label1_49.Text = "mm/s";
            // 
            // _Label1_48
            // 
            this._Label1_48.AutoSize = true;
            this._Label1_48.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_48.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_48.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_48.Location = new System.Drawing.Point(8, 24);
            this._Label1_48.Name = "_Label1_48";
            this._Label1_48.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_48.Size = new System.Drawing.Size(59, 12);
            this._Label1_48.TabIndex = 183;
            this._Label1_48.Text = "運転速度：";
            // 
            // Frame20
            // 
            this.Frame20.BackColor = System.Drawing.SystemColors.Control;
            this.Frame20.Controls.Add(this._cmdSwitch_71);
            this.Frame20.Controls.Add(this._cmdSwitch_70);
            this.Frame20.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame20.Location = new System.Drawing.Point(672, 400);
            this.Frame20.Name = "Frame20";
            this.Frame20.Padding = new System.Windows.Forms.Padding(0);
            this.Frame20.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame20.Size = new System.Drawing.Size(65, 81);
            this.Frame20.TabIndex = 170;
            this.Frame20.TabStop = false;
            this.Frame20.Text = "微調Y軸";
            // 
            // _cmdSwitch_71
            // 
            this._cmdSwitch_71.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_71.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_71.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_71.Location = new System.Drawing.Point(8, 48);
            this._cmdSwitch_71.Name = "_cmdSwitch_71";
            this._cmdSwitch_71.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_71.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_71.TabIndex = 172;
            this._cmdSwitch_71.Text = "無効";
            this._cmdSwitch_71.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_70
            // 
            this._cmdSwitch_70.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_70.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_70.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_70.Location = new System.Drawing.Point(8, 24);
            this._cmdSwitch_70.Name = "_cmdSwitch_70";
            this._cmdSwitch_70.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_70.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_70.TabIndex = 171;
            this._cmdSwitch_70.Text = "有効";
            this._cmdSwitch_70.UseVisualStyleBackColor = false;
            // 
            // Frame19
            // 
            this.Frame19.BackColor = System.Drawing.SystemColors.Control;
            this.Frame19.Controls.Add(this._cmdSwitch_69);
            this.Frame19.Controls.Add(this._cmdSwitch_68);
            this.Frame19.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame19.Location = new System.Drawing.Point(600, 400);
            this.Frame19.Name = "Frame19";
            this.Frame19.Padding = new System.Windows.Forms.Padding(0);
            this.Frame19.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame19.Size = new System.Drawing.Size(65, 81);
            this.Frame19.TabIndex = 167;
            this.Frame19.TabStop = false;
            this.Frame19.Text = "微調X軸";
            // 
            // _cmdSwitch_69
            // 
            this._cmdSwitch_69.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_69.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_69.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_69.Location = new System.Drawing.Point(8, 24);
            this._cmdSwitch_69.Name = "_cmdSwitch_69";
            this._cmdSwitch_69.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_69.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_69.TabIndex = 169;
            this._cmdSwitch_69.Text = "有効";
            this._cmdSwitch_69.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_68
            // 
            this._cmdSwitch_68.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_68.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_68.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_68.Location = new System.Drawing.Point(8, 48);
            this._cmdSwitch_68.Name = "_cmdSwitch_68";
            this._cmdSwitch_68.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_68.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_68.TabIndex = 168;
            this._cmdSwitch_68.Text = "無効";
            this._cmdSwitch_68.UseVisualStyleBackColor = false;
            // 
            // Frame18
            // 
            this.Frame18.BackColor = System.Drawing.SystemColors.Control;
            this.Frame18.Controls.Add(this._cmdSwitch_67);
            this.Frame18.Controls.Add(this._cmdSwitch_66);
            this.Frame18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame18.Location = new System.Drawing.Point(672, 304);
            this.Frame18.Name = "Frame18";
            this.Frame18.Padding = new System.Windows.Forms.Padding(0);
            this.Frame18.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame18.Size = new System.Drawing.Size(65, 81);
            this.Frame18.TabIndex = 164;
            this.Frame18.TabStop = false;
            this.Frame18.Text = "X軸機構";
            // 
            // _cmdSwitch_67
            // 
            this._cmdSwitch_67.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_67.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_67.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_67.Location = new System.Drawing.Point(8, 48);
            this._cmdSwitch_67.Name = "_cmdSwitch_67";
            this._cmdSwitch_67.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_67.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_67.TabIndex = 166;
            this._cmdSwitch_67.Text = "X線管";
            this._cmdSwitch_67.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_66
            // 
            this._cmdSwitch_66.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_66.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_66.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_66.Location = new System.Drawing.Point(8, 24);
            this._cmdSwitch_66.Name = "_cmdSwitch_66";
            this._cmdSwitch_66.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_66.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_66.TabIndex = 165;
            this._cmdSwitch_66.Text = "ﾃｰﾌﾞﾙ";
            this._cmdSwitch_66.UseVisualStyleBackColor = false;
            // 
            // Frame17
            // 
            this.Frame17.BackColor = System.Drawing.SystemColors.Control;
            this.Frame17.Controls.Add(this._cmdSwitch_65);
            this.Frame17.Controls.Add(this._cmdSwitch_64);
            this.Frame17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame17.Location = new System.Drawing.Point(672, 208);
            this.Frame17.Name = "Frame17";
            this.Frame17.Padding = new System.Windows.Forms.Padding(0);
            this.Frame17.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame17.Size = new System.Drawing.Size(65, 81);
            this.Frame17.TabIndex = 161;
            this.Frame17.TabStop = false;
            this.Frame17.Text = "検出器";
            // 
            // _cmdSwitch_65
            // 
            this._cmdSwitch_65.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_65.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_65.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_65.Location = new System.Drawing.Point(8, 48);
            this._cmdSwitch_65.Name = "_cmdSwitch_65";
            this._cmdSwitch_65.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_65.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_65.TabIndex = 163;
            this._cmdSwitch_65.Text = "FPD";
            this._cmdSwitch_65.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_64
            // 
            this._cmdSwitch_64.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_64.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_64.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_64.Location = new System.Drawing.Point(8, 24);
            this._cmdSwitch_64.Name = "_cmdSwitch_64";
            this._cmdSwitch_64.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_64.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_64.TabIndex = 162;
            this._cmdSwitch_64.Text = "I.I.";
            this._cmdSwitch_64.UseVisualStyleBackColor = false;
            // 
            // Frame16
            // 
            this.Frame16.BackColor = System.Drawing.SystemColors.Control;
            this.Frame16.Controls.Add(this._hsbSpeed_6);
            this.Frame16.Controls.Add(this._cmdSwitch_61);
            this.Frame16.Controls.Add(this._cmdSwitch_60);
            this.Frame16.Controls.Add(this._cmdSwitch_59);
            this.Frame16.Controls.Add(this._txtSpeed_6);
            this.Frame16.Controls.Add(this.lblYStgMinSpeed);
            this.Frame16.Controls.Add(this.lblYStgMaxSpeed);
            this.Frame16.Controls.Add(this.lblYStgPosition);
            this.Frame16.Controls.Add(this._Label1_38);
            this.Frame16.Controls.Add(this._Label1_34);
            this.Frame16.Controls.Add(this._Label1_37);
            this.Frame16.Controls.Add(this._Label1_33);
            this.Frame16.Controls.Add(this._Label1_36);
            this.Frame16.Controls.Add(this._Label1_32);
            this.Frame16.Controls.Add(this._Label1_35);
            this.Frame16.Controls.Add(this._Label1_31);
            this.Frame16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame16.Location = new System.Drawing.Point(504, 208);
            this.Frame16.Name = "Frame16";
            this.Frame16.Padding = new System.Windows.Forms.Padding(0);
            this.Frame16.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame16.Size = new System.Drawing.Size(161, 185);
            this.Frame16.TabIndex = 133;
            this.Frame16.TabStop = false;
            this.Frame16.Text = "微調Y軸";
            // 
            // _hsbSpeed_6
            // 
            this._hsbSpeed_6.Cursor = System.Windows.Forms.Cursors.Default;
            this._hsbSpeed_6.LargeChange = 1;
            this._hsbSpeed_6.Location = new System.Drawing.Point(8, 96);
            this._hsbSpeed_6.Maximum = 32767;
            this._hsbSpeed_6.Name = "_hsbSpeed_6";
            this._hsbSpeed_6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._hsbSpeed_6.Size = new System.Drawing.Size(145, 17);
            this._hsbSpeed_6.TabIndex = 138;
            this._hsbSpeed_6.TabStop = true;
            // 
            // _cmdSwitch_61
            // 
            this._cmdSwitch_61.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_61.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_61.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_61.Location = new System.Drawing.Point(104, 152);
            this._cmdSwitch_61.Name = "_cmdSwitch_61";
            this._cmdSwitch_61.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_61.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_61.TabIndex = 137;
            this._cmdSwitch_61.Text = "後退";
            this._cmdSwitch_61.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_60
            // 
            this._cmdSwitch_60.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_60.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_60.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_60.Location = new System.Drawing.Point(104, 120);
            this._cmdSwitch_60.Name = "_cmdSwitch_60";
            this._cmdSwitch_60.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_60.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_60.TabIndex = 136;
            this._cmdSwitch_60.Text = "前進";
            this._cmdSwitch_60.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_59
            // 
            this._cmdSwitch_59.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_59.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_59.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_59.Location = new System.Drawing.Point(8, 120);
            this._cmdSwitch_59.Name = "_cmdSwitch_59";
            this._cmdSwitch_59.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_59.Size = new System.Drawing.Size(73, 25);
            this._cmdSwitch_59.TabIndex = 135;
            this._cmdSwitch_59.Text = "原点復帰";
            this._cmdSwitch_59.UseVisualStyleBackColor = false;
            // 
            // _txtSpeed_6
            // 
            this._txtSpeed_6.AcceptsReturn = true;
            this._txtSpeed_6.BackColor = System.Drawing.SystemColors.Window;
            this._txtSpeed_6.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._txtSpeed_6.ForeColor = System.Drawing.SystemColors.WindowText;
            this._txtSpeed_6.Location = new System.Drawing.Point(86, 72);
            this._txtSpeed_6.MaxLength = 0;
            this._txtSpeed_6.Multiline = true;
            this._txtSpeed_6.Name = "_txtSpeed_6";
            this._txtSpeed_6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._txtSpeed_6.Size = new System.Drawing.Size(41, 18);
            this._txtSpeed_6.TabIndex = 134;
            this._txtSpeed_6.Text = "0";
            this._txtSpeed_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblYStgMinSpeed
            // 
            this.lblYStgMinSpeed.BackColor = System.Drawing.SystemColors.Control;
            this.lblYStgMinSpeed.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblYStgMinSpeed.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblYStgMinSpeed.Location = new System.Drawing.Point(86, 56);
            this.lblYStgMinSpeed.Name = "lblYStgMinSpeed";
            this.lblYStgMinSpeed.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblYStgMinSpeed.Size = new System.Drawing.Size(35, 12);
            this.lblYStgMinSpeed.TabIndex = 149;
            this.lblYStgMinSpeed.Text = "0.1";
            this.lblYStgMinSpeed.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblYStgMaxSpeed
            // 
            this.lblYStgMaxSpeed.BackColor = System.Drawing.SystemColors.Control;
            this.lblYStgMaxSpeed.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblYStgMaxSpeed.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblYStgMaxSpeed.Location = new System.Drawing.Point(85, 40);
            this.lblYStgMaxSpeed.Name = "lblYStgMaxSpeed";
            this.lblYStgMaxSpeed.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblYStgMaxSpeed.Size = new System.Drawing.Size(37, 12);
            this.lblYStgMaxSpeed.TabIndex = 148;
            this.lblYStgMaxSpeed.Text = "20";
            this.lblYStgMaxSpeed.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblYStgPosition
            // 
            this.lblYStgPosition.BackColor = System.Drawing.SystemColors.Control;
            this.lblYStgPosition.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblYStgPosition.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblYStgPosition.Location = new System.Drawing.Point(67, 24);
            this.lblYStgPosition.Name = "lblYStgPosition";
            this.lblYStgPosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblYStgPosition.Size = new System.Drawing.Size(54, 12);
            this.lblYStgPosition.TabIndex = 147;
            this.lblYStgPosition.Text = "0";
            this.lblYStgPosition.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _Label1_38
            // 
            this._Label1_38.AutoSize = true;
            this._Label1_38.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_38.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_38.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._Label1_38.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_38.Location = new System.Drawing.Point(128, 80);
            this._Label1_38.Name = "_Label1_38";
            this._Label1_38.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_38.Size = new System.Drawing.Size(32, 11);
            this._Label1_38.TabIndex = 146;
            this._Label1_38.Text = "mm/s";
            // 
            // _Label1_34
            // 
            this._Label1_34.AutoSize = true;
            this._Label1_34.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_34.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_34.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_34.Location = new System.Drawing.Point(8, 72);
            this._Label1_34.Name = "_Label1_34";
            this._Label1_34.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_34.Size = new System.Drawing.Size(83, 12);
            this._Label1_34.TabIndex = 145;
            this._Label1_34.Text = "運転速度設定：";
            // 
            // _Label1_37
            // 
            this._Label1_37.AutoSize = true;
            this._Label1_37.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_37.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_37.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._Label1_37.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_37.Location = new System.Drawing.Point(128, 56);
            this._Label1_37.Name = "_Label1_37";
            this._Label1_37.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_37.Size = new System.Drawing.Size(32, 11);
            this._Label1_37.TabIndex = 144;
            this._Label1_37.Text = "mm/s";
            // 
            // _Label1_33
            // 
            this._Label1_33.AutoSize = true;
            this._Label1_33.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_33.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_33.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_33.Location = new System.Drawing.Point(8, 56);
            this._Label1_33.Name = "_Label1_33";
            this._Label1_33.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_33.Size = new System.Drawing.Size(83, 12);
            this._Label1_33.TabIndex = 143;
            this._Label1_33.Text = "最低速度設定：";
            // 
            // _Label1_36
            // 
            this._Label1_36.AutoSize = true;
            this._Label1_36.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_36.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_36.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._Label1_36.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_36.Location = new System.Drawing.Point(128, 40);
            this._Label1_36.Name = "_Label1_36";
            this._Label1_36.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_36.Size = new System.Drawing.Size(32, 11);
            this._Label1_36.TabIndex = 142;
            this._Label1_36.Text = "mm/s";
            // 
            // _Label1_32
            // 
            this._Label1_32.AutoSize = true;
            this._Label1_32.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_32.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_32.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_32.Location = new System.Drawing.Point(8, 40);
            this._Label1_32.Name = "_Label1_32";
            this._Label1_32.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_32.Size = new System.Drawing.Size(83, 12);
            this._Label1_32.TabIndex = 141;
            this._Label1_32.Text = "最高速度設定：";
            // 
            // _Label1_35
            // 
            this._Label1_35.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_35.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_35.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._Label1_35.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_35.Location = new System.Drawing.Point(128, 24);
            this._Label1_35.Name = "_Label1_35";
            this._Label1_35.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_35.Size = new System.Drawing.Size(17, 9);
            this._Label1_35.TabIndex = 140;
            this._Label1_35.Text = "mm";
            // 
            // _Label1_31
            // 
            this._Label1_31.AutoSize = true;
            this._Label1_31.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_31.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_31.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_31.Location = new System.Drawing.Point(8, 24);
            this._Label1_31.Name = "_Label1_31";
            this._Label1_31.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_31.Size = new System.Drawing.Size(47, 12);
            this._Label1_31.TabIndex = 139;
            this._Label1_31.Text = "現在値：";
            // 
            // Frame8
            // 
            this.Frame8.BackColor = System.Drawing.SystemColors.Control;
            this.Frame8.Controls.Add(this._hsbSpeed_5);
            this.Frame8.Controls.Add(this._cmdSwitch_58);
            this.Frame8.Controls.Add(this._cmdSwitch_30);
            this.Frame8.Controls.Add(this._cmdSwitch_29);
            this.Frame8.Controls.Add(this._txtSpeed_5);
            this.Frame8.Controls.Add(this._Label1_30);
            this.Frame8.Controls.Add(this._Label1_26);
            this.Frame8.Controls.Add(this._Label1_29);
            this.Frame8.Controls.Add(this.lblXStgMinSpeed);
            this.Frame8.Controls.Add(this._Label1_25);
            this.Frame8.Controls.Add(this._Label1_28);
            this.Frame8.Controls.Add(this.lblXStgMaxSpeed);
            this.Frame8.Controls.Add(this._Label1_24);
            this.Frame8.Controls.Add(this.lblXStgPosition);
            this.Frame8.Controls.Add(this._Label1_27);
            this.Frame8.Controls.Add(this._Label1_23);
            this.Frame8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame8.Location = new System.Drawing.Point(336, 208);
            this.Frame8.Name = "Frame8";
            this.Frame8.Padding = new System.Windows.Forms.Padding(0);
            this.Frame8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame8.Size = new System.Drawing.Size(161, 185);
            this.Frame8.TabIndex = 116;
            this.Frame8.TabStop = false;
            this.Frame8.Text = "微調X軸";
            // 
            // _hsbSpeed_5
            // 
            this._hsbSpeed_5.Cursor = System.Windows.Forms.Cursors.Default;
            this._hsbSpeed_5.LargeChange = 1;
            this._hsbSpeed_5.Location = new System.Drawing.Point(8, 96);
            this._hsbSpeed_5.Maximum = 32767;
            this._hsbSpeed_5.Name = "_hsbSpeed_5";
            this._hsbSpeed_5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._hsbSpeed_5.Size = new System.Drawing.Size(145, 17);
            this._hsbSpeed_5.TabIndex = 121;
            this._hsbSpeed_5.TabStop = true;
            // 
            // _cmdSwitch_58
            // 
            this._cmdSwitch_58.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_58.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_58.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_58.Location = new System.Drawing.Point(104, 152);
            this._cmdSwitch_58.Name = "_cmdSwitch_58";
            this._cmdSwitch_58.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_58.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_58.TabIndex = 120;
            this._cmdSwitch_58.Text = "右";
            this._cmdSwitch_58.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_30
            // 
            this._cmdSwitch_30.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_30.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_30.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_30.Location = new System.Drawing.Point(48, 120);
            this._cmdSwitch_30.Name = "_cmdSwitch_30";
            this._cmdSwitch_30.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_30.Size = new System.Drawing.Size(65, 25);
            this._cmdSwitch_30.TabIndex = 119;
            this._cmdSwitch_30.Text = "原点復帰";
            this._cmdSwitch_30.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_29
            // 
            this._cmdSwitch_29.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_29.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_29.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_29.Location = new System.Drawing.Point(16, 152);
            this._cmdSwitch_29.Name = "_cmdSwitch_29";
            this._cmdSwitch_29.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_29.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_29.TabIndex = 118;
            this._cmdSwitch_29.Text = "左";
            this._cmdSwitch_29.UseVisualStyleBackColor = false;
            // 
            // _txtSpeed_5
            // 
            this._txtSpeed_5.AcceptsReturn = true;
            this._txtSpeed_5.BackColor = System.Drawing.SystemColors.Window;
            this._txtSpeed_5.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._txtSpeed_5.ForeColor = System.Drawing.SystemColors.WindowText;
            this._txtSpeed_5.Location = new System.Drawing.Point(86, 72);
            this._txtSpeed_5.MaxLength = 0;
            this._txtSpeed_5.Multiline = true;
            this._txtSpeed_5.Name = "_txtSpeed_5";
            this._txtSpeed_5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._txtSpeed_5.Size = new System.Drawing.Size(41, 18);
            this._txtSpeed_5.TabIndex = 117;
            this._txtSpeed_5.Text = "0";
            this._txtSpeed_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _Label1_30
            // 
            this._Label1_30.AutoSize = true;
            this._Label1_30.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_30.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_30.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._Label1_30.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_30.Location = new System.Drawing.Point(128, 80);
            this._Label1_30.Name = "_Label1_30";
            this._Label1_30.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_30.Size = new System.Drawing.Size(32, 11);
            this._Label1_30.TabIndex = 132;
            this._Label1_30.Text = "mm/s";
            // 
            // _Label1_26
            // 
            this._Label1_26.AutoSize = true;
            this._Label1_26.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_26.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_26.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_26.Location = new System.Drawing.Point(8, 72);
            this._Label1_26.Name = "_Label1_26";
            this._Label1_26.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_26.Size = new System.Drawing.Size(83, 12);
            this._Label1_26.TabIndex = 131;
            this._Label1_26.Text = "運転速度設定：";
            // 
            // _Label1_29
            // 
            this._Label1_29.AutoSize = true;
            this._Label1_29.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_29.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_29.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._Label1_29.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_29.Location = new System.Drawing.Point(128, 56);
            this._Label1_29.Name = "_Label1_29";
            this._Label1_29.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_29.Size = new System.Drawing.Size(32, 11);
            this._Label1_29.TabIndex = 130;
            this._Label1_29.Text = "mm/s";
            // 
            // lblXStgMinSpeed
            // 
            this.lblXStgMinSpeed.BackColor = System.Drawing.SystemColors.Control;
            this.lblXStgMinSpeed.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblXStgMinSpeed.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblXStgMinSpeed.Location = new System.Drawing.Point(90, 56);
            this.lblXStgMinSpeed.Name = "lblXStgMinSpeed";
            this.lblXStgMinSpeed.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblXStgMinSpeed.Size = new System.Drawing.Size(31, 12);
            this.lblXStgMinSpeed.TabIndex = 129;
            this.lblXStgMinSpeed.Text = "0.1";
            this.lblXStgMinSpeed.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _Label1_25
            // 
            this._Label1_25.AutoSize = true;
            this._Label1_25.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_25.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_25.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_25.Location = new System.Drawing.Point(8, 56);
            this._Label1_25.Name = "_Label1_25";
            this._Label1_25.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_25.Size = new System.Drawing.Size(83, 12);
            this._Label1_25.TabIndex = 128;
            this._Label1_25.Text = "最低速度設定：";
            // 
            // _Label1_28
            // 
            this._Label1_28.AutoSize = true;
            this._Label1_28.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_28.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_28.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._Label1_28.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_28.Location = new System.Drawing.Point(128, 40);
            this._Label1_28.Name = "_Label1_28";
            this._Label1_28.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_28.Size = new System.Drawing.Size(32, 11);
            this._Label1_28.TabIndex = 127;
            this._Label1_28.Text = "mm/s";
            // 
            // lblXStgMaxSpeed
            // 
            this.lblXStgMaxSpeed.BackColor = System.Drawing.SystemColors.Control;
            this.lblXStgMaxSpeed.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblXStgMaxSpeed.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblXStgMaxSpeed.Location = new System.Drawing.Point(92, 40);
            this.lblXStgMaxSpeed.Name = "lblXStgMaxSpeed";
            this.lblXStgMaxSpeed.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblXStgMaxSpeed.Size = new System.Drawing.Size(29, 12);
            this.lblXStgMaxSpeed.TabIndex = 126;
            this.lblXStgMaxSpeed.Text = "20";
            this.lblXStgMaxSpeed.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _Label1_24
            // 
            this._Label1_24.AutoSize = true;
            this._Label1_24.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_24.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_24.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_24.Location = new System.Drawing.Point(8, 40);
            this._Label1_24.Name = "_Label1_24";
            this._Label1_24.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_24.Size = new System.Drawing.Size(83, 12);
            this._Label1_24.TabIndex = 125;
            this._Label1_24.Text = "最高速度設定：";
            // 
            // lblXStgPosition
            // 
            this.lblXStgPosition.BackColor = System.Drawing.SystemColors.Control;
            this.lblXStgPosition.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblXStgPosition.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblXStgPosition.Location = new System.Drawing.Point(75, 24);
            this.lblXStgPosition.Name = "lblXStgPosition";
            this.lblXStgPosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblXStgPosition.Size = new System.Drawing.Size(46, 12);
            this.lblXStgPosition.TabIndex = 124;
            this.lblXStgPosition.Text = "0";
            this.lblXStgPosition.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _Label1_27
            // 
            this._Label1_27.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_27.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_27.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._Label1_27.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_27.Location = new System.Drawing.Point(128, 24);
            this._Label1_27.Name = "_Label1_27";
            this._Label1_27.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_27.Size = new System.Drawing.Size(17, 9);
            this._Label1_27.TabIndex = 123;
            this._Label1_27.Text = "mm";
            // 
            // _Label1_23
            // 
            this._Label1_23.AutoSize = true;
            this._Label1_23.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_23.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_23.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_23.Location = new System.Drawing.Point(8, 24);
            this._Label1_23.Name = "_Label1_23";
            this._Label1_23.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_23.Size = new System.Drawing.Size(47, 12);
            this._Label1_23.TabIndex = 122;
            this._Label1_23.Text = "現在値：";
            // 
            // _Frame12_0
            // 
            this._Frame12_0.BackColor = System.Drawing.SystemColors.Control;
            this._Frame12_0.Controls.Add(this._cmdSwitch_57);
            this._Frame12_0.Controls.Add(this._cmdSwitch_56);
            this._Frame12_0.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Frame12_0.Location = new System.Drawing.Point(168, 536);
            this._Frame12_0.Name = "_Frame12_0";
            this._Frame12_0.Padding = new System.Windows.Forms.Padding(0);
            this._Frame12_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Frame12_0.Size = new System.Drawing.Size(89, 49);
            this._Frame12_0.TabIndex = 113;
            this._Frame12_0.TabStop = false;
            this._Frame12_0.Text = "ｽﾗｲｽﾗｲﾄ";
            // 
            // _cmdSwitch_57
            // 
            this._cmdSwitch_57.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_57.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_57.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_57.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_57.Location = new System.Drawing.Point(48, 16);
            this._cmdSwitch_57.Name = "_cmdSwitch_57";
            this._cmdSwitch_57.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_57.Size = new System.Drawing.Size(33, 25);
            this._cmdSwitch_57.TabIndex = 115;
            this._cmdSwitch_57.Text = "OFF";
            this._cmdSwitch_57.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_56
            // 
            this._cmdSwitch_56.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_56.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_56.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_56.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_56.Location = new System.Drawing.Point(8, 16);
            this._cmdSwitch_56.Name = "_cmdSwitch_56";
            this._cmdSwitch_56.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_56.Size = new System.Drawing.Size(33, 25);
            this._cmdSwitch_56.TabIndex = 114;
            this._cmdSwitch_56.Text = "ON";
            this._cmdSwitch_56.UseVisualStyleBackColor = false;
            // 
            // Frame1
            // 
            this.Frame1.BackColor = System.Drawing.SystemColors.Control;
            this.Frame1.Controls.Add(this._cmdSwitch_108);
            this.Frame1.Controls.Add(this._cmdSwitch_55);
            this.Frame1.Controls.Add(this._txtSpeed_0);
            this.Frame1.Controls.Add(this._cmdSwitch_1);
            this.Frame1.Controls.Add(this._cmdSwitch_2);
            this.Frame1.Controls.Add(this._hsbSpeed_0);
            this.Frame1.Controls.Add(this._cmdSwitch_45);
            this.Frame1.Controls.Add(this._cmdSwitch_46);
            this.Frame1.Controls.Add(this.txtXIndexPosition);
            this.Frame1.Controls.Add(this._Label1_0);
            this.Frame1.Controls.Add(this._Label1_40);
            this.Frame1.Controls.Add(this._Label1_41);
            this.Frame1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame1.Location = new System.Drawing.Point(112, 8);
            this.Frame1.Name = "Frame1";
            this.Frame1.Padding = new System.Windows.Forms.Padding(0);
            this.Frame1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame1.Size = new System.Drawing.Size(137, 185);
            this.Frame1.TabIndex = 87;
            this.Frame1.TabStop = false;
            this.Frame1.Text = "ﾃｰﾌﾞﾙX";
            // 
            // _cmdSwitch_108
            // 
            this._cmdSwitch_108.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_108.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_108.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_108.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_108.Location = new System.Drawing.Point(48, 144);
            this._cmdSwitch_108.Name = "_cmdSwitch_108";
            this._cmdSwitch_108.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_108.Size = new System.Drawing.Size(41, 33);
            this._cmdSwitch_108.TabIndex = 263;
            this._cmdSwitch_108.Text = "直線補間";
            this._cmdSwitch_108.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_55
            // 
            this._cmdSwitch_55.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_55.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_55.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_55.Location = new System.Drawing.Point(80, 112);
            this._cmdSwitch_55.Name = "_cmdSwitch_55";
            this._cmdSwitch_55.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_55.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_55.TabIndex = 104;
            this._cmdSwitch_55.Text = "原点";
            this._cmdSwitch_55.UseVisualStyleBackColor = false;
            // 
            // _txtSpeed_0
            // 
            this._txtSpeed_0.AcceptsReturn = true;
            this._txtSpeed_0.BackColor = System.Drawing.SystemColors.Window;
            this._txtSpeed_0.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._txtSpeed_0.ForeColor = System.Drawing.SystemColors.WindowText;
            this._txtSpeed_0.Location = new System.Drawing.Point(64, 16);
            this._txtSpeed_0.MaxLength = 0;
            this._txtSpeed_0.Multiline = true;
            this._txtSpeed_0.Name = "_txtSpeed_0";
            this._txtSpeed_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._txtSpeed_0.Size = new System.Drawing.Size(33, 18);
            this._txtSpeed_0.TabIndex = 94;
            this._txtSpeed_0.Text = "0";
            this._txtSpeed_0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _cmdSwitch_1
            // 
            this._cmdSwitch_1.BackColor = System.Drawing.Color.Lime;
            this._cmdSwitch_1.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_1.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_1.Location = new System.Drawing.Point(8, 80);
            this._cmdSwitch_1.Name = "_cmdSwitch_1";
            this._cmdSwitch_1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_1.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_1.TabIndex = 93;
            this._cmdSwitch_1.Text = "左移動";
            this._cmdSwitch_1.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_2
            // 
            this._cmdSwitch_2.BackColor = System.Drawing.Color.Lime;
            this._cmdSwitch_2.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_2.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_2.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_2.Location = new System.Drawing.Point(80, 80);
            this._cmdSwitch_2.Name = "_cmdSwitch_2";
            this._cmdSwitch_2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_2.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_2.TabIndex = 92;
            this._cmdSwitch_2.Text = "右移動";
            this._cmdSwitch_2.UseVisualStyleBackColor = false;
            // 
            // _hsbSpeed_0
            // 
            this._hsbSpeed_0.Cursor = System.Windows.Forms.Cursors.Default;
            this._hsbSpeed_0.LargeChange = 1;
            this._hsbSpeed_0.Location = new System.Drawing.Point(8, 48);
            this._hsbSpeed_0.Maximum = 32767;
            this._hsbSpeed_0.Name = "_hsbSpeed_0";
            this._hsbSpeed_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._hsbSpeed_0.Size = new System.Drawing.Size(121, 17);
            this._hsbSpeed_0.TabIndex = 91;
            this._hsbSpeed_0.TabStop = true;
            this._hsbSpeed_0.ValueChanged += new System.EventHandler(this.hsbSpeed_ValueChanged);
            this._hsbSpeed_0.MouseCaptureChanged += new System.EventHandler(this.hsbSpeed_MouseCaptureChanged);
            // 
            // _cmdSwitch_45
            // 
            this._cmdSwitch_45.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_45.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_45.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_45.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_45.Location = new System.Drawing.Point(8, 144);
            this._cmdSwitch_45.Name = "_cmdSwitch_45";
            this._cmdSwitch_45.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_45.Size = new System.Drawing.Size(41, 33);
            this._cmdSwitch_45.TabIndex = 90;
            this._cmdSwitch_45.Text = "位置決";
            this._cmdSwitch_45.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_46
            // 
            this._cmdSwitch_46.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_46.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_46.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_46.Location = new System.Drawing.Point(88, 144);
            this._cmdSwitch_46.Name = "_cmdSwitch_46";
            this._cmdSwitch_46.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_46.Size = new System.Drawing.Size(41, 33);
            this._cmdSwitch_46.TabIndex = 89;
            this._cmdSwitch_46.Text = "停止";
            this._cmdSwitch_46.UseVisualStyleBackColor = false;
            // 
            // txtXIndexPosition
            // 
            this.txtXIndexPosition.AcceptsReturn = true;
            this.txtXIndexPosition.BackColor = System.Drawing.SystemColors.Window;
            this.txtXIndexPosition.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtXIndexPosition.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtXIndexPosition.Location = new System.Drawing.Point(8, 120);
            this.txtXIndexPosition.MaxLength = 0;
            this.txtXIndexPosition.Multiline = true;
            this.txtXIndexPosition.Name = "txtXIndexPosition";
            this.txtXIndexPosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtXIndexPosition.Size = new System.Drawing.Size(49, 18);
            this.txtXIndexPosition.TabIndex = 88;
            this.txtXIndexPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtXIndexPosition.TextChanged += new System.EventHandler(this.txtXIndexPosition_TextChanged);
            // 
            // _Label1_0
            // 
            this._Label1_0.AutoSize = true;
            this._Label1_0.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_0.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_0.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_0.Location = new System.Drawing.Point(8, 24);
            this._Label1_0.Name = "_Label1_0";
            this._Label1_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_0.Size = new System.Drawing.Size(59, 12);
            this._Label1_0.TabIndex = 97;
            this._Label1_0.Text = "運転速度：";
            // 
            // _Label1_40
            // 
            this._Label1_40.AutoSize = true;
            this._Label1_40.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_40.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_40.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_40.Location = new System.Drawing.Point(96, 24);
            this._Label1_40.Name = "_Label1_40";
            this._Label1_40.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_40.Size = new System.Drawing.Size(35, 12);
            this._Label1_40.TabIndex = 96;
            this._Label1_40.Text = "mm/s";
            // 
            // _Label1_41
            // 
            this._Label1_41.AutoSize = true;
            this._Label1_41.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_41.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_41.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_41.Location = new System.Drawing.Point(56, 128);
            this._Label1_41.Name = "_Label1_41";
            this._Label1_41.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_41.Size = new System.Drawing.Size(23, 12);
            this._Label1_41.TabIndex = 95;
            this._Label1_41.Text = "mm";
            // 
            // Frame2
            // 
            this.Frame2.BackColor = System.Drawing.SystemColors.Control;
            this.Frame2.Controls.Add(this._cmdSwitch_72);
            this.Frame2.Controls.Add(this._txtSpeed_1);
            this.Frame2.Controls.Add(this._cmdSwitch_3);
            this.Frame2.Controls.Add(this._cmdSwitch_4);
            this.Frame2.Controls.Add(this._hsbSpeed_1);
            this.Frame2.Controls.Add(this._cmdSwitch_43);
            this.Frame2.Controls.Add(this.txtYIndexPosition);
            this.Frame2.Controls.Add(this._cmdSwitch_44);
            this.Frame2.Controls.Add(this._Label1_42);
            this.Frame2.Controls.Add(this._Label1_43);
            this.Frame2.Controls.Add(this._Label1_44);
            this.Frame2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame2.Location = new System.Drawing.Point(256, 8);
            this.Frame2.Name = "Frame2";
            this.Frame2.Padding = new System.Windows.Forms.Padding(0);
            this.Frame2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame2.Size = new System.Drawing.Size(137, 185);
            this.Frame2.TabIndex = 76;
            this.Frame2.TabStop = false;
            this.Frame2.Text = "ﾃｰﾌﾞﾙY";
            // 
            // _cmdSwitch_72
            // 
            this._cmdSwitch_72.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_72.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_72.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_72.Location = new System.Drawing.Point(8, 80);
            this._cmdSwitch_72.Name = "_cmdSwitch_72";
            this._cmdSwitch_72.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_72.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_72.TabIndex = 173;
            this._cmdSwitch_72.Text = "原点";
            this._cmdSwitch_72.UseVisualStyleBackColor = false;
            // 
            // _txtSpeed_1
            // 
            this._txtSpeed_1.AcceptsReturn = true;
            this._txtSpeed_1.BackColor = System.Drawing.SystemColors.Window;
            this._txtSpeed_1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._txtSpeed_1.ForeColor = System.Drawing.SystemColors.WindowText;
            this._txtSpeed_1.Location = new System.Drawing.Point(64, 16);
            this._txtSpeed_1.MaxLength = 0;
            this._txtSpeed_1.Multiline = true;
            this._txtSpeed_1.Name = "_txtSpeed_1";
            this._txtSpeed_1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._txtSpeed_1.Size = new System.Drawing.Size(33, 18);
            this._txtSpeed_1.TabIndex = 83;
            this._txtSpeed_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _cmdSwitch_3
            // 
            this._cmdSwitch_3.BackColor = System.Drawing.Color.Lime;
            this._cmdSwitch_3.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_3.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_3.Location = new System.Drawing.Point(80, 80);
            this._cmdSwitch_3.Name = "_cmdSwitch_3";
            this._cmdSwitch_3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_3.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_3.TabIndex = 82;
            this._cmdSwitch_3.Text = "前進";
            this._cmdSwitch_3.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_4
            // 
            this._cmdSwitch_4.BackColor = System.Drawing.Color.Lime;
            this._cmdSwitch_4.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_4.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_4.Location = new System.Drawing.Point(80, 112);
            this._cmdSwitch_4.Name = "_cmdSwitch_4";
            this._cmdSwitch_4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_4.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_4.TabIndex = 81;
            this._cmdSwitch_4.Text = "後退";
            this._cmdSwitch_4.UseVisualStyleBackColor = false;
            // 
            // _hsbSpeed_1
            // 
            this._hsbSpeed_1.Cursor = System.Windows.Forms.Cursors.Default;
            this._hsbSpeed_1.LargeChange = 1;
            this._hsbSpeed_1.Location = new System.Drawing.Point(8, 48);
            this._hsbSpeed_1.Maximum = 32767;
            this._hsbSpeed_1.Name = "_hsbSpeed_1";
            this._hsbSpeed_1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._hsbSpeed_1.Size = new System.Drawing.Size(121, 17);
            this._hsbSpeed_1.TabIndex = 80;
            this._hsbSpeed_1.TabStop = true;
            this._hsbSpeed_1.ValueChanged += new System.EventHandler(this.hsbSpeed_ValueChanged);
            this._hsbSpeed_1.MouseCaptureChanged += new System.EventHandler(this.hsbSpeed_MouseCaptureChanged);
            // 
            // _cmdSwitch_43
            // 
            this._cmdSwitch_43.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_43.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_43.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_43.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_43.Location = new System.Drawing.Point(8, 144);
            this._cmdSwitch_43.Name = "_cmdSwitch_43";
            this._cmdSwitch_43.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_43.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_43.TabIndex = 79;
            this._cmdSwitch_43.Text = "位置決";
            this._cmdSwitch_43.UseVisualStyleBackColor = false;
            // 
            // txtYIndexPosition
            // 
            this.txtYIndexPosition.AcceptsReturn = true;
            this.txtYIndexPosition.BackColor = System.Drawing.SystemColors.Window;
            this.txtYIndexPosition.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtYIndexPosition.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtYIndexPosition.Location = new System.Drawing.Point(8, 119);
            this.txtYIndexPosition.MaxLength = 0;
            this.txtYIndexPosition.Name = "txtYIndexPosition";
            this.txtYIndexPosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtYIndexPosition.Size = new System.Drawing.Size(49, 19);
            this.txtYIndexPosition.TabIndex = 78;
            this.txtYIndexPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtYIndexPosition.TextChanged += new System.EventHandler(this.txtYIndexPosition_TextChanged);
            // 
            // _cmdSwitch_44
            // 
            this._cmdSwitch_44.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_44.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_44.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_44.Location = new System.Drawing.Point(80, 144);
            this._cmdSwitch_44.Name = "_cmdSwitch_44";
            this._cmdSwitch_44.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_44.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_44.TabIndex = 77;
            this._cmdSwitch_44.Text = "停止";
            this._cmdSwitch_44.UseVisualStyleBackColor = false;
            // 
            // _Label1_42
            // 
            this._Label1_42.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_42.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_42.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_42.Location = new System.Drawing.Point(8, 24);
            this._Label1_42.Name = "_Label1_42";
            this._Label1_42.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_42.Size = new System.Drawing.Size(65, 17);
            this._Label1_42.TabIndex = 86;
            this._Label1_42.Text = "運転速度：";
            // 
            // _Label1_43
            // 
            this._Label1_43.AutoSize = true;
            this._Label1_43.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_43.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_43.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_43.Location = new System.Drawing.Point(96, 24);
            this._Label1_43.Name = "_Label1_43";
            this._Label1_43.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_43.Size = new System.Drawing.Size(35, 12);
            this._Label1_43.TabIndex = 85;
            this._Label1_43.Text = "mm/s";
            // 
            // _Label1_44
            // 
            this._Label1_44.AutoSize = true;
            this._Label1_44.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_44.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_44.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_44.Location = new System.Drawing.Point(56, 128);
            this._Label1_44.Name = "_Label1_44";
            this._Label1_44.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_44.Size = new System.Drawing.Size(23, 12);
            this._Label1_44.TabIndex = 84;
            this._Label1_44.Text = "mm";
            // 
            // Frame3
            // 
            this.Frame3.BackColor = System.Drawing.SystemColors.Control;
            this.Frame3.Controls.Add(this._cmdSwitch_142);
            this.Frame3.Controls.Add(this.txtIIIndexPosition);
            this.Frame3.Controls.Add(this._hsbSpeed_4);
            this.Frame3.Controls.Add(this._txtSpeed_4);
            this.Frame3.Controls.Add(this._cmdSwitch_6);
            this.Frame3.Controls.Add(this._cmdSwitch_5);
            this.Frame3.Controls.Add(this._cmdSwitch_7);
            this.Frame3.Controls.Add(this._cmdSwitch_8);
            this.Frame3.Controls.Add(this._Label1_47);
            this.Frame3.Controls.Add(this._Label1_46);
            this.Frame3.Controls.Add(this._Label1_45);
            this.Frame3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame3.Location = new System.Drawing.Point(400, 8);
            this.Frame3.Name = "Frame3";
            this.Frame3.Padding = new System.Windows.Forms.Padding(0);
            this.Frame3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame3.Size = new System.Drawing.Size(137, 185);
            this.Frame3.TabIndex = 73;
            this.Frame3.TabStop = false;
            this.Frame3.Text = "Ｉ．Ｉ．";
            // 
            // _cmdSwitch_142
            // 
            this._cmdSwitch_142.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_142.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_142.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_142.Location = new System.Drawing.Point(8, 80);
            this._cmdSwitch_142.Name = "_cmdSwitch_142";
            this._cmdSwitch_142.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_142.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_142.TabIndex = 293;
            this._cmdSwitch_142.Text = "原点";
            this._cmdSwitch_142.UseVisualStyleBackColor = false;
            // 
            // txtIIIndexPosition
            // 
            this.txtIIIndexPosition.AcceptsReturn = true;
            this.txtIIIndexPosition.BackColor = System.Drawing.SystemColors.Window;
            this.txtIIIndexPosition.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtIIIndexPosition.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtIIIndexPosition.Location = new System.Drawing.Point(8, 120);
            this.txtIIIndexPosition.MaxLength = 0;
            this.txtIIIndexPosition.Name = "txtIIIndexPosition";
            this.txtIIIndexPosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtIIIndexPosition.Size = new System.Drawing.Size(49, 19);
            this.txtIIIndexPosition.TabIndex = 111;
            this.txtIIIndexPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtIIIndexPosition.TextChanged += new System.EventHandler(this.txtIIIndexPosition_TextChanged);
            // 
            // _hsbSpeed_4
            // 
            this._hsbSpeed_4.Cursor = System.Windows.Forms.Cursors.Default;
            this._hsbSpeed_4.LargeChange = 1;
            this._hsbSpeed_4.Location = new System.Drawing.Point(8, 48);
            this._hsbSpeed_4.Maximum = 32767;
            this._hsbSpeed_4.Name = "_hsbSpeed_4";
            this._hsbSpeed_4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._hsbSpeed_4.Size = new System.Drawing.Size(121, 17);
            this._hsbSpeed_4.TabIndex = 110;
            this._hsbSpeed_4.TabStop = true;
            this._hsbSpeed_4.ValueChanged += new System.EventHandler(this.hsbSpeed_ValueChanged);
            this._hsbSpeed_4.MouseCaptureChanged += new System.EventHandler(this.hsbSpeed_MouseCaptureChanged);
            // 
            // _txtSpeed_4
            // 
            this._txtSpeed_4.AcceptsReturn = true;
            this._txtSpeed_4.BackColor = System.Drawing.SystemColors.Window;
            this._txtSpeed_4.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._txtSpeed_4.ForeColor = System.Drawing.SystemColors.WindowText;
            this._txtSpeed_4.Location = new System.Drawing.Point(64, 16);
            this._txtSpeed_4.MaxLength = 0;
            this._txtSpeed_4.Multiline = true;
            this._txtSpeed_4.Name = "_txtSpeed_4";
            this._txtSpeed_4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._txtSpeed_4.Size = new System.Drawing.Size(33, 18);
            this._txtSpeed_4.TabIndex = 109;
            this._txtSpeed_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _cmdSwitch_6
            // 
            this._cmdSwitch_6.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_6.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_6.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_6.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_6.Location = new System.Drawing.Point(8, 144);
            this._cmdSwitch_6.Name = "_cmdSwitch_6";
            this._cmdSwitch_6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_6.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_6.TabIndex = 106;
            this._cmdSwitch_6.Text = "位置決";
            this._cmdSwitch_6.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_5
            // 
            this._cmdSwitch_5.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_5.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_5.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_5.Location = new System.Drawing.Point(80, 144);
            this._cmdSwitch_5.Name = "_cmdSwitch_5";
            this._cmdSwitch_5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_5.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_5.TabIndex = 105;
            this._cmdSwitch_5.Text = "停止";
            this._cmdSwitch_5.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_7
            // 
            this._cmdSwitch_7.BackColor = System.Drawing.Color.Lime;
            this._cmdSwitch_7.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_7.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_7.Location = new System.Drawing.Point(80, 80);
            this._cmdSwitch_7.Name = "_cmdSwitch_7";
            this._cmdSwitch_7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_7.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_7.TabIndex = 75;
            this._cmdSwitch_7.Text = "前進";
            this._cmdSwitch_7.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_8
            // 
            this._cmdSwitch_8.BackColor = System.Drawing.Color.Lime;
            this._cmdSwitch_8.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_8.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_8.Location = new System.Drawing.Point(80, 112);
            this._cmdSwitch_8.Name = "_cmdSwitch_8";
            this._cmdSwitch_8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_8.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_8.TabIndex = 74;
            this._cmdSwitch_8.Text = "後退";
            this._cmdSwitch_8.UseVisualStyleBackColor = false;
            // 
            // _Label1_47
            // 
            this._Label1_47.AutoSize = true;
            this._Label1_47.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_47.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_47.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_47.Location = new System.Drawing.Point(56, 128);
            this._Label1_47.Name = "_Label1_47";
            this._Label1_47.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_47.Size = new System.Drawing.Size(23, 12);
            this._Label1_47.TabIndex = 112;
            this._Label1_47.Text = "mm";
            // 
            // _Label1_46
            // 
            this._Label1_46.AutoSize = true;
            this._Label1_46.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_46.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_46.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_46.Location = new System.Drawing.Point(96, 24);
            this._Label1_46.Name = "_Label1_46";
            this._Label1_46.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_46.Size = new System.Drawing.Size(35, 12);
            this._Label1_46.TabIndex = 108;
            this._Label1_46.Text = "mm/s";
            // 
            // _Label1_45
            // 
            this._Label1_45.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_45.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_45.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_45.Location = new System.Drawing.Point(8, 24);
            this._Label1_45.Name = "_Label1_45";
            this._Label1_45.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_45.Size = new System.Drawing.Size(65, 17);
            this._Label1_45.TabIndex = 107;
            this._Label1_45.Text = "運転速度：";
            // 
            // Frame4
            // 
            this.Frame4.BackColor = System.Drawing.SystemColors.Control;
            this.Frame4.Controls.Add(this._cmdSwitch_11);
            this.Frame4.Controls.Add(this._cmdSwitch_10);
            this.Frame4.Controls.Add(this._cmdSwitch_9);
            this.Frame4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame4.Location = new System.Drawing.Point(431, 408);
            this.Frame4.Name = "Frame4";
            this.Frame4.Padding = new System.Windows.Forms.Padding(0);
            this.Frame4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame4.Size = new System.Drawing.Size(153, 57);
            this.Frame4.TabIndex = 69;
            this.Frame4.TabStop = false;
            this.Frame4.Text = "ﾁﾙﾄ";
            // 
            // _cmdSwitch_11
            // 
            this._cmdSwitch_11.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_11.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_11.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_11.Location = new System.Drawing.Point(56, 24);
            this._cmdSwitch_11.Name = "_cmdSwitch_11";
            this._cmdSwitch_11.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_11.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_11.TabIndex = 72;
            this._cmdSwitch_11.Text = "水平";
            this._cmdSwitch_11.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_10
            // 
            this._cmdSwitch_10.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_10.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_10.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_10.Location = new System.Drawing.Point(104, 24);
            this._cmdSwitch_10.Name = "_cmdSwitch_10";
            this._cmdSwitch_10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_10.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_10.TabIndex = 71;
            this._cmdSwitch_10.Text = "CW";
            this._cmdSwitch_10.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_9
            // 
            this._cmdSwitch_9.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_9.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_9.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_9.Location = new System.Drawing.Point(8, 24);
            this._cmdSwitch_9.Name = "_cmdSwitch_9";
            this._cmdSwitch_9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_9.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_9.TabIndex = 70;
            this._cmdSwitch_9.Text = "CCW";
            this._cmdSwitch_9.UseVisualStyleBackColor = false;
            // 
            // Frame5
            // 
            this.Frame5.BackColor = System.Drawing.SystemColors.Control;
            this.Frame5.Controls.Add(this._cmdSwitch_22);
            this.Frame5.Controls.Add(this._cmdSwitch_23);
            this.Frame5.Controls.Add(this._cmdSwitch_24);
            this.Frame5.Controls.Add(this._cmdSwitch_25);
            this.Frame5.Controls.Add(this._cmdSwitch_18);
            this.Frame5.Controls.Add(this._cmdSwitch_19);
            this.Frame5.Controls.Add(this._cmdSwitch_20);
            this.Frame5.Controls.Add(this._cmdSwitch_21);
            this.Frame5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame5.Location = new System.Drawing.Point(544, 8);
            this.Frame5.Name = "Frame5";
            this.Frame5.Padding = new System.Windows.Forms.Padding(0);
            this.Frame5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame5.Size = new System.Drawing.Size(193, 89);
            this.Frame5.TabIndex = 60;
            this.Frame5.TabStop = false;
            this.Frame5.Text = "X線ｺﾘﾒｰﾀ";
            // 
            // _cmdSwitch_22
            // 
            this._cmdSwitch_22.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_22.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_22.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_22.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_22.Location = new System.Drawing.Point(8, 24);
            this._cmdSwitch_22.Name = "_cmdSwitch_22";
            this._cmdSwitch_22.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_22.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_22.TabIndex = 68;
            this._cmdSwitch_22.Text = "左開";
            this._cmdSwitch_22.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_23
            // 
            this._cmdSwitch_23.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_23.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_23.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_23.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_23.Location = new System.Drawing.Point(48, 24);
            this._cmdSwitch_23.Name = "_cmdSwitch_23";
            this._cmdSwitch_23.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_23.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_23.TabIndex = 67;
            this._cmdSwitch_23.Text = "左閉";
            this._cmdSwitch_23.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_24
            // 
            this._cmdSwitch_24.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_24.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_24.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_24.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_24.Location = new System.Drawing.Point(48, 56);
            this._cmdSwitch_24.Name = "_cmdSwitch_24";
            this._cmdSwitch_24.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_24.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_24.TabIndex = 66;
            this._cmdSwitch_24.Text = "右開";
            this._cmdSwitch_24.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_25
            // 
            this._cmdSwitch_25.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_25.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_25.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_25.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_25.Location = new System.Drawing.Point(8, 56);
            this._cmdSwitch_25.Name = "_cmdSwitch_25";
            this._cmdSwitch_25.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_25.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_25.TabIndex = 65;
            this._cmdSwitch_25.Text = "右閉";
            this._cmdSwitch_25.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_18
            // 
            this._cmdSwitch_18.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_18.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_18.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_18.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_18.Location = new System.Drawing.Point(96, 32);
            this._cmdSwitch_18.Name = "_cmdSwitch_18";
            this._cmdSwitch_18.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_18.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_18.TabIndex = 64;
            this._cmdSwitch_18.Text = "上開";
            this._cmdSwitch_18.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_19
            // 
            this._cmdSwitch_19.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_19.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_19.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_19.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_19.Location = new System.Drawing.Point(96, 56);
            this._cmdSwitch_19.Name = "_cmdSwitch_19";
            this._cmdSwitch_19.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_19.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_19.TabIndex = 63;
            this._cmdSwitch_19.Text = "上閉";
            this._cmdSwitch_19.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_20
            // 
            this._cmdSwitch_20.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_20.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_20.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_20.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_20.Location = new System.Drawing.Point(144, 56);
            this._cmdSwitch_20.Name = "_cmdSwitch_20";
            this._cmdSwitch_20.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_20.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_20.TabIndex = 62;
            this._cmdSwitch_20.Text = "下開";
            this._cmdSwitch_20.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_21
            // 
            this._cmdSwitch_21.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_21.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_21.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_21.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_21.Location = new System.Drawing.Point(144, 32);
            this._cmdSwitch_21.Name = "_cmdSwitch_21";
            this._cmdSwitch_21.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_21.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_21.TabIndex = 61;
            this._cmdSwitch_21.Text = "下閉";
            this._cmdSwitch_21.UseVisualStyleBackColor = false;
            // 
            // Frame6
            // 
            this.Frame6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.Frame6.Controls.Add(this._cmdSwitch_149);
            this.Frame6.Controls.Add(this._cmdSwitch_12);
            this.Frame6.Controls.Add(this._cmdSwitch_13);
            this.Frame6.Controls.Add(this._cmdSwitch_14);
            this.Frame6.Controls.Add(this._cmdSwitch_15);
            this.Frame6.Controls.Add(this._cmdSwitch_16);
            this.Frame6.Controls.Add(this._cmdSwitch_17);
            this.Frame6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame6.Location = new System.Drawing.Point(168, 408);
            this.Frame6.Name = "Frame6";
            this.Frame6.Padding = new System.Windows.Forms.Padding(0);
            this.Frame6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame6.Size = new System.Drawing.Size(254, 57);
            this.Frame6.TabIndex = 53;
            this.Frame6.TabStop = false;
            this.Frame6.Text = "X線ﾌｨﾙﾀ";
            // 
            // _cmdSwitch_149
            // 
            this._cmdSwitch_149.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_149.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_149.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_149.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_149.Location = new System.Drawing.Point(6, 24);
            this._cmdSwitch_149.Name = "_cmdSwitch_149";
            this._cmdSwitch_149.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_149.Size = new System.Drawing.Size(37, 25);
            this._cmdSwitch_149.TabIndex = 60;
            this._cmdSwitch_149.Text = "ｼｬｯﾀ";
            this._cmdSwitch_149.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_12
            // 
            this._cmdSwitch_12.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_12.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_12.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_12.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_12.Location = new System.Drawing.Point(44, 24);
            this._cmdSwitch_12.Name = "_cmdSwitch_12";
            this._cmdSwitch_12.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_12.Size = new System.Drawing.Size(33, 25);
            this._cmdSwitch_12.TabIndex = 59;
            this._cmdSwitch_12.Text = "無し";
            this._cmdSwitch_12.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_13
            // 
            this._cmdSwitch_13.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_13.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_13.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_13.Location = new System.Drawing.Point(78, 24);
            this._cmdSwitch_13.Name = "_cmdSwitch_13";
            this._cmdSwitch_13.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_13.Size = new System.Drawing.Size(33, 25);
            this._cmdSwitch_13.TabIndex = 58;
            this._cmdSwitch_13.Text = "１";
            this._cmdSwitch_13.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_14
            // 
            this._cmdSwitch_14.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_14.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_14.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_14.Location = new System.Drawing.Point(112, 24);
            this._cmdSwitch_14.Name = "_cmdSwitch_14";
            this._cmdSwitch_14.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_14.Size = new System.Drawing.Size(33, 25);
            this._cmdSwitch_14.TabIndex = 57;
            this._cmdSwitch_14.Text = "２";
            this._cmdSwitch_14.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_15
            // 
            this._cmdSwitch_15.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_15.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_15.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_15.Location = new System.Drawing.Point(146, 24);
            this._cmdSwitch_15.Name = "_cmdSwitch_15";
            this._cmdSwitch_15.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_15.Size = new System.Drawing.Size(33, 25);
            this._cmdSwitch_15.TabIndex = 56;
            this._cmdSwitch_15.Text = "３";
            this._cmdSwitch_15.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_16
            // 
            this._cmdSwitch_16.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_16.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_16.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_16.Location = new System.Drawing.Point(180, 24);
            this._cmdSwitch_16.Name = "_cmdSwitch_16";
            this._cmdSwitch_16.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_16.Size = new System.Drawing.Size(33, 25);
            this._cmdSwitch_16.TabIndex = 55;
            this._cmdSwitch_16.Text = "４";
            this._cmdSwitch_16.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_17
            // 
            this._cmdSwitch_17.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_17.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_17.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_17.Location = new System.Drawing.Point(214, 24);
            this._cmdSwitch_17.Name = "_cmdSwitch_17";
            this._cmdSwitch_17.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_17.Size = new System.Drawing.Size(33, 25);
            this._cmdSwitch_17.TabIndex = 54;
            this._cmdSwitch_17.Text = "５";
            this._cmdSwitch_17.UseVisualStyleBackColor = false;
            // 
            // Frame7
            // 
            this.Frame7.BackColor = System.Drawing.SystemColors.Control;
            this.Frame7.Controls.Add(this._cmdSwitch_28);
            this.Frame7.Controls.Add(this._cmdSwitch_27);
            this.Frame7.Controls.Add(this._cmdSwitch_26);
            this.Frame7.Controls.Add(this._cmdSwitch_37);
            this.Frame7.Controls.Add(this._cmdSwitch_38);
            this.Frame7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame7.Location = new System.Drawing.Point(8, 496);
            this.Frame7.Name = "Frame7";
            this.Frame7.Padding = new System.Windows.Forms.Padding(0);
            this.Frame7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame7.Size = new System.Drawing.Size(153, 57);
            this.Frame7.TabIndex = 47;
            this.Frame7.TabStop = false;
            this.Frame7.Text = "Ｉ．Ｉ．視野";
            // 
            // _cmdSwitch_28
            // 
            this._cmdSwitch_28.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_28.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_28.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_28.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_28.Location = new System.Drawing.Point(120, 16);
            this._cmdSwitch_28.Name = "_cmdSwitch_28";
            this._cmdSwitch_28.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_28.Size = new System.Drawing.Size(25, 33);
            this._cmdSwitch_28.TabIndex = 52;
            this._cmdSwitch_28.Text = "4.5";
            this._cmdSwitch_28.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_27
            // 
            this._cmdSwitch_27.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_27.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_27.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_27.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_27.Location = new System.Drawing.Point(96, 16);
            this._cmdSwitch_27.Name = "_cmdSwitch_27";
            this._cmdSwitch_27.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_27.Size = new System.Drawing.Size(25, 33);
            this._cmdSwitch_27.TabIndex = 51;
            this._cmdSwitch_27.Text = "６";
            this._cmdSwitch_27.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_26
            // 
            this._cmdSwitch_26.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_26.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_26.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_26.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_26.Location = new System.Drawing.Point(72, 16);
            this._cmdSwitch_26.Name = "_cmdSwitch_26";
            this._cmdSwitch_26.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_26.Size = new System.Drawing.Size(25, 33);
            this._cmdSwitch_26.TabIndex = 50;
            this._cmdSwitch_26.Text = "９";
            this._cmdSwitch_26.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_37
            // 
            this._cmdSwitch_37.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_37.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_37.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_37.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_37.Location = new System.Drawing.Point(8, 16);
            this._cmdSwitch_37.Name = "_cmdSwitch_37";
            this._cmdSwitch_37.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_37.Size = new System.Drawing.Size(33, 33);
            this._cmdSwitch_37.TabIndex = 49;
            this._cmdSwitch_37.Text = "電源ON";
            this._cmdSwitch_37.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_38
            // 
            this._cmdSwitch_38.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_38.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_38.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_38.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_38.Location = new System.Drawing.Point(40, 16);
            this._cmdSwitch_38.Name = "_cmdSwitch_38";
            this._cmdSwitch_38.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_38.Size = new System.Drawing.Size(33, 33);
            this._cmdSwitch_38.TabIndex = 48;
            this._cmdSwitch_38.Text = "電源OFF";
            this._cmdSwitch_38.UseVisualStyleBackColor = false;
            // 
            // Frame9
            // 
            this.Frame9.BackColor = System.Drawing.SystemColors.Control;
            this.Frame9.Controls.Add(this.txtTrgCycleTime);
            this.Frame9.Controls.Add(this._cmdSwitch_63);
            this.Frame9.Controls.Add(this.txtTrgTime);
            this.Frame9.Controls.Add(this.txtScanStartPos);
            this.Frame9.Controls.Add(this._cmdSwitch_62);
            this.Frame9.Controls.Add(this._txtSpeed_2);
            this.Frame9.Controls.Add(this._cmdSwitch_31);
            this.Frame9.Controls.Add(this._cmdSwitch_33);
            this.Frame9.Controls.Add(this._cmdSwitch_32);
            this.Frame9.Controls.Add(this._hsbSpeed_2);
            this.Frame9.Controls.Add(this._Label1_58);
            this.Frame9.Controls.Add(this._Label1_57);
            this.Frame9.Controls.Add(this._Label1_14);
            this.Frame9.Controls.Add(this._Label1_7);
            this.Frame9.Controls.Add(this._Label1_13);
            this.Frame9.Controls.Add(this._Label1_6);
            this.Frame9.Controls.Add(this._Label1_11);
            this.Frame9.Controls.Add(this.lblRotAccelTm);
            this.Frame9.Controls.Add(this._Label1_4);
            this.Frame9.Controls.Add(this._Label1_1);
            this.Frame9.Controls.Add(this._Label1_8);
            this.Frame9.Controls.Add(this.lblRotPosition);
            this.Frame9.Controls.Add(this._Label1_2);
            this.Frame9.Controls.Add(this.lblRotMaxSpeed);
            this.Frame9.Controls.Add(this._Label1_9);
            this.Frame9.Controls.Add(this._Label1_3);
            this.Frame9.Controls.Add(this.lblRotMinSpeed);
            this.Frame9.Controls.Add(this._Label1_10);
            this.Frame9.Controls.Add(this._Label1_5);
            this.Frame9.Controls.Add(this._Label1_12);
            this.Frame9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame9.Location = new System.Drawing.Point(8, 208);
            this.Frame9.Name = "Frame9";
            this.Frame9.Padding = new System.Windows.Forms.Padding(0);
            this.Frame9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame9.Size = new System.Drawing.Size(153, 281);
            this.Frame9.TabIndex = 30;
            this.Frame9.TabStop = false;
            this.Frame9.Text = "ﾃｰﾌﾞﾙ回転";
            // 
            // txtTrgCycleTime
            // 
            this.txtTrgCycleTime.AcceptsReturn = true;
            this.txtTrgCycleTime.BackColor = System.Drawing.SystemColors.Window;
            this.txtTrgCycleTime.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTrgCycleTime.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtTrgCycleTime.Location = new System.Drawing.Point(56, 136);
            this.txtTrgCycleTime.MaxLength = 0;
            this.txtTrgCycleTime.Multiline = true;
            this.txtTrgCycleTime.Name = "txtTrgCycleTime";
            this.txtTrgCycleTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtTrgCycleTime.Size = new System.Drawing.Size(65, 18);
            this.txtTrgCycleTime.TabIndex = 216;
            this.txtTrgCycleTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTrgCycleTime.TextChanged += new System.EventHandler(this.txtTrgCycleTime_TextChanged);
            // 
            // _cmdSwitch_63
            // 
            this._cmdSwitch_63.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_63.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_63.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_63.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_63.Location = new System.Drawing.Point(8, 248);
            this._cmdSwitch_63.Name = "_cmdSwitch_63";
            this._cmdSwitch_63.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_63.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_63.TabIndex = 160;
            this._cmdSwitch_63.Text = "ﾄﾘｶﾞOFF";
            this._cmdSwitch_63.UseVisualStyleBackColor = false;
            // 
            // txtTrgTime
            // 
            this.txtTrgTime.AcceptsReturn = true;
            this.txtTrgTime.BackColor = System.Drawing.SystemColors.Window;
            this.txtTrgTime.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTrgTime.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtTrgTime.Location = new System.Drawing.Point(56, 160);
            this.txtTrgTime.MaxLength = 0;
            this.txtTrgTime.Multiline = true;
            this.txtTrgTime.Name = "txtTrgTime";
            this.txtTrgTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtTrgTime.Size = new System.Drawing.Size(65, 18);
            this.txtTrgTime.TabIndex = 159;
            this.txtTrgTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTrgTime.TextChanged += new System.EventHandler(this.txtTrgTime_TextChanged);
            // 
            // txtScanStartPos
            // 
            this.txtScanStartPos.AcceptsReturn = true;
            this.txtScanStartPos.BackColor = System.Drawing.SystemColors.Window;
            this.txtScanStartPos.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtScanStartPos.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtScanStartPos.Location = new System.Drawing.Point(80, 112);
            this.txtScanStartPos.MaxLength = 0;
            this.txtScanStartPos.Multiline = true;
            this.txtScanStartPos.Name = "txtScanStartPos";
            this.txtScanStartPos.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtScanStartPos.Size = new System.Drawing.Size(41, 18);
            this.txtScanStartPos.TabIndex = 158;
            this.txtScanStartPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtScanStartPos.TextChanged += new System.EventHandler(this.txtScanStartPos_TextChanged);
            // 
            // _cmdSwitch_62
            // 
            this._cmdSwitch_62.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_62.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_62.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_62.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_62.Location = new System.Drawing.Point(8, 216);
            this._cmdSwitch_62.Name = "_cmdSwitch_62";
            this._cmdSwitch_62.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_62.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_62.TabIndex = 157;
            this._cmdSwitch_62.Text = "ﾄﾘｶﾞON";
            this._cmdSwitch_62.UseVisualStyleBackColor = false;
            // 
            // _txtSpeed_2
            // 
            this._txtSpeed_2.AcceptsReturn = true;
            this._txtSpeed_2.BackColor = System.Drawing.SystemColors.Window;
            this._txtSpeed_2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._txtSpeed_2.ForeColor = System.Drawing.SystemColors.WindowText;
            this._txtSpeed_2.Location = new System.Drawing.Point(86, 88);
            this._txtSpeed_2.MaxLength = 0;
            this._txtSpeed_2.Multiline = true;
            this._txtSpeed_2.Name = "_txtSpeed_2";
            this._txtSpeed_2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._txtSpeed_2.Size = new System.Drawing.Size(41, 18);
            this._txtSpeed_2.TabIndex = 35;
            this._txtSpeed_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _cmdSwitch_31
            // 
            this._cmdSwitch_31.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_31.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_31.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_31.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_31.Location = new System.Drawing.Point(64, 248);
            this._cmdSwitch_31.Name = "_cmdSwitch_31";
            this._cmdSwitch_31.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_31.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_31.TabIndex = 34;
            this._cmdSwitch_31.Text = "逆転";
            this._cmdSwitch_31.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_33
            // 
            this._cmdSwitch_33.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_33.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_33.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_33.Location = new System.Drawing.Point(72, 216);
            this._cmdSwitch_33.Name = "_cmdSwitch_33";
            this._cmdSwitch_33.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_33.Size = new System.Drawing.Size(65, 25);
            this._cmdSwitch_33.TabIndex = 33;
            this._cmdSwitch_33.Text = "原点復帰";
            this._cmdSwitch_33.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_32
            // 
            this._cmdSwitch_32.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_32.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_32.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_32.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_32.Location = new System.Drawing.Point(104, 248);
            this._cmdSwitch_32.Name = "_cmdSwitch_32";
            this._cmdSwitch_32.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_32.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_32.TabIndex = 32;
            this._cmdSwitch_32.Text = "正転";
            this._cmdSwitch_32.UseVisualStyleBackColor = false;
            // 
            // _hsbSpeed_2
            // 
            this._hsbSpeed_2.Cursor = System.Windows.Forms.Cursors.Default;
            this._hsbSpeed_2.LargeChange = 1;
            this._hsbSpeed_2.Location = new System.Drawing.Point(8, 192);
            this._hsbSpeed_2.Maximum = 32767;
            this._hsbSpeed_2.Name = "_hsbSpeed_2";
            this._hsbSpeed_2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._hsbSpeed_2.Size = new System.Drawing.Size(137, 17);
            this._hsbSpeed_2.TabIndex = 31;
            this._hsbSpeed_2.TabStop = true;
            // 
            // _Label1_58
            // 
            this._Label1_58.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_58.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_58.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._Label1_58.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_58.Location = new System.Drawing.Point(8, 136);
            this._Label1_58.Name = "_Label1_58";
            this._Label1_58.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_58.Size = new System.Drawing.Size(45, 22);
            this._Label1_58.TabIndex = 215;
            this._Label1_58.Text = "ﾄﾘｶﾞ周期時間：";
            // 
            // _Label1_57
            // 
            this._Label1_57.AutoSize = true;
            this._Label1_57.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_57.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_57.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_57.Location = new System.Drawing.Point(128, 136);
            this._Label1_57.Name = "_Label1_57";
            this._Label1_57.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_57.Size = new System.Drawing.Size(23, 12);
            this._Label1_57.TabIndex = 214;
            this._Label1_57.Text = "μs";
            // 
            // _Label1_14
            // 
            this._Label1_14.AutoSize = true;
            this._Label1_14.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_14.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_14.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_14.Location = new System.Drawing.Point(128, 160);
            this._Label1_14.Name = "_Label1_14";
            this._Label1_14.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_14.Size = new System.Drawing.Size(23, 12);
            this._Label1_14.TabIndex = 156;
            this._Label1_14.Text = "μs";
            // 
            // _Label1_7
            // 
            this._Label1_7.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_7.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_7.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._Label1_7.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_7.Location = new System.Drawing.Point(8, 160);
            this._Label1_7.Name = "_Label1_7";
            this._Label1_7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_7.Size = new System.Drawing.Size(45, 19);
            this._Label1_7.TabIndex = 155;
            this._Label1_7.Text = "ﾄﾘｶﾞON時間：";
            // 
            // _Label1_13
            // 
            this._Label1_13.AutoSize = true;
            this._Label1_13.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_13.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_13.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_13.Location = new System.Drawing.Point(128, 112);
            this._Label1_13.Name = "_Label1_13";
            this._Label1_13.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_13.Size = new System.Drawing.Size(17, 12);
            this._Label1_13.TabIndex = 154;
            this._Label1_13.Text = "度";
            // 
            // _Label1_6
            // 
            this._Label1_6.AutoSize = true;
            this._Label1_6.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_6.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_6.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_6.Location = new System.Drawing.Point(8, 112);
            this._Label1_6.Name = "_Label1_6";
            this._Label1_6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_6.Size = new System.Drawing.Size(64, 12);
            this._Label1_6.TabIndex = 153;
            this._Label1_6.Text = "ｽｷｬﾝ開始：";
            // 
            // _Label1_11
            // 
            this._Label1_11.AutoSize = true;
            this._Label1_11.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_11.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_11.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_11.Location = new System.Drawing.Point(128, 72);
            this._Label1_11.Name = "_Label1_11";
            this._Label1_11.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_11.Size = new System.Drawing.Size(11, 12);
            this._Label1_11.TabIndex = 152;
            this._Label1_11.Text = "s";
            // 
            // lblRotAccelTm
            // 
            this.lblRotAccelTm.BackColor = System.Drawing.SystemColors.Control;
            this.lblRotAccelTm.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblRotAccelTm.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblRotAccelTm.Location = new System.Drawing.Point(83, 72);
            this.lblRotAccelTm.Name = "lblRotAccelTm";
            this.lblRotAccelTm.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblRotAccelTm.Size = new System.Drawing.Size(38, 12);
            this.lblRotAccelTm.TabIndex = 151;
            this.lblRotAccelTm.Text = "0.0";
            this.lblRotAccelTm.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _Label1_4
            // 
            this._Label1_4.AutoSize = true;
            this._Label1_4.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_4.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_4.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_4.Location = new System.Drawing.Point(8, 72);
            this._Label1_4.Name = "_Label1_4";
            this._Label1_4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_4.Size = new System.Drawing.Size(80, 12);
            this._Label1_4.TabIndex = 150;
            this._Label1_4.Text = "加速ﾚｰﾄ設定：";
            // 
            // _Label1_1
            // 
            this._Label1_1.AutoSize = true;
            this._Label1_1.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_1.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_1.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_1.Location = new System.Drawing.Point(8, 24);
            this._Label1_1.Name = "_Label1_1";
            this._Label1_1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_1.Size = new System.Drawing.Size(47, 12);
            this._Label1_1.TabIndex = 46;
            this._Label1_1.Text = "現在値：";
            // 
            // _Label1_8
            // 
            this._Label1_8.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_8.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_8.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_8.Location = new System.Drawing.Point(128, 24);
            this._Label1_8.Name = "_Label1_8";
            this._Label1_8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_8.Size = new System.Drawing.Size(17, 17);
            this._Label1_8.TabIndex = 45;
            this._Label1_8.Text = "度";
            // 
            // lblRotPosition
            // 
            this.lblRotPosition.BackColor = System.Drawing.SystemColors.Control;
            this.lblRotPosition.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblRotPosition.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblRotPosition.Location = new System.Drawing.Point(75, 24);
            this.lblRotPosition.Name = "lblRotPosition";
            this.lblRotPosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblRotPosition.Size = new System.Drawing.Size(46, 12);
            this.lblRotPosition.TabIndex = 44;
            this.lblRotPosition.Text = "0";
            this.lblRotPosition.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _Label1_2
            // 
            this._Label1_2.AutoSize = true;
            this._Label1_2.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_2.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_2.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_2.Location = new System.Drawing.Point(8, 40);
            this._Label1_2.Name = "_Label1_2";
            this._Label1_2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_2.Size = new System.Drawing.Size(83, 12);
            this._Label1_2.TabIndex = 43;
            this._Label1_2.Text = "最高速度設定：";
            // 
            // lblRotMaxSpeed
            // 
            this.lblRotMaxSpeed.BackColor = System.Drawing.SystemColors.Control;
            this.lblRotMaxSpeed.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblRotMaxSpeed.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblRotMaxSpeed.Location = new System.Drawing.Point(85, 40);
            this.lblRotMaxSpeed.Name = "lblRotMaxSpeed";
            this.lblRotMaxSpeed.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblRotMaxSpeed.Size = new System.Drawing.Size(36, 12);
            this.lblRotMaxSpeed.TabIndex = 42;
            this.lblRotMaxSpeed.Text = "10";
            this.lblRotMaxSpeed.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _Label1_9
            // 
            this._Label1_9.AutoSize = true;
            this._Label1_9.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_9.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_9.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_9.Location = new System.Drawing.Point(128, 40);
            this._Label1_9.Name = "_Label1_9";
            this._Label1_9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_9.Size = new System.Drawing.Size(24, 12);
            this._Label1_9.TabIndex = 41;
            this._Label1_9.Text = "rpm";
            // 
            // _Label1_3
            // 
            this._Label1_3.AutoSize = true;
            this._Label1_3.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_3.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_3.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_3.Location = new System.Drawing.Point(8, 56);
            this._Label1_3.Name = "_Label1_3";
            this._Label1_3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_3.Size = new System.Drawing.Size(83, 12);
            this._Label1_3.TabIndex = 40;
            this._Label1_3.Text = "最低速度設定：";
            // 
            // lblRotMinSpeed
            // 
            this.lblRotMinSpeed.BackColor = System.Drawing.SystemColors.Control;
            this.lblRotMinSpeed.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblRotMinSpeed.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblRotMinSpeed.Location = new System.Drawing.Point(89, 56);
            this.lblRotMinSpeed.Name = "lblRotMinSpeed";
            this.lblRotMinSpeed.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblRotMinSpeed.Size = new System.Drawing.Size(32, 12);
            this.lblRotMinSpeed.TabIndex = 39;
            this.lblRotMinSpeed.Text = "0.0006";
            this.lblRotMinSpeed.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _Label1_10
            // 
            this._Label1_10.AutoSize = true;
            this._Label1_10.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_10.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_10.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_10.Location = new System.Drawing.Point(128, 56);
            this._Label1_10.Name = "_Label1_10";
            this._Label1_10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_10.Size = new System.Drawing.Size(24, 12);
            this._Label1_10.TabIndex = 38;
            this._Label1_10.Text = "rpm";
            // 
            // _Label1_5
            // 
            this._Label1_5.AutoSize = true;
            this._Label1_5.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_5.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_5.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_5.Location = new System.Drawing.Point(8, 88);
            this._Label1_5.Name = "_Label1_5";
            this._Label1_5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_5.Size = new System.Drawing.Size(83, 12);
            this._Label1_5.TabIndex = 37;
            this._Label1_5.Text = "運転速度設定：";
            // 
            // _Label1_12
            // 
            this._Label1_12.AutoSize = true;
            this._Label1_12.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_12.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_12.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_12.Location = new System.Drawing.Point(128, 88);
            this._Label1_12.Name = "_Label1_12";
            this._Label1_12.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_12.Size = new System.Drawing.Size(24, 12);
            this._Label1_12.TabIndex = 36;
            this._Label1_12.Text = "rpm";
            // 
            // Frame10
            // 
            this.Frame10.BackColor = System.Drawing.SystemColors.Control;
            this.Frame10.Controls.Add(this._cmdSwitch_150);
            this.Frame10.Controls.Add(this._txtSpeed_3);
            this.Frame10.Controls.Add(this._cmdSwitch_36);
            this.Frame10.Controls.Add(this._cmdSwitch_34);
            this.Frame10.Controls.Add(this._cmdSwitch_35);
            this.Frame10.Controls.Add(this._hsbSpeed_3);
            this.Frame10.Controls.Add(this._Label1_15);
            this.Frame10.Controls.Add(this._Label1_19);
            this.Frame10.Controls.Add(this._Label1_16);
            this.Frame10.Controls.Add(this._Label1_20);
            this.Frame10.Controls.Add(this._Label1_17);
            this.Frame10.Controls.Add(this._Label1_21);
            this.Frame10.Controls.Add(this._Label1_18);
            this.Frame10.Controls.Add(this._Label1_22);
            this.Frame10.Controls.Add(this.lblUDPosition);
            this.Frame10.Controls.Add(this.lblUDMaxSpeed);
            this.Frame10.Controls.Add(this.lblUDMinSpeed);
            this.Frame10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame10.Location = new System.Drawing.Point(168, 208);
            this.Frame10.Name = "Frame10";
            this.Frame10.Padding = new System.Windows.Forms.Padding(0);
            this.Frame10.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame10.Size = new System.Drawing.Size(161, 185);
            this.Frame10.TabIndex = 13;
            this.Frame10.TabStop = false;
            this.Frame10.Text = "ﾃｰﾌﾞﾙ昇降";
            // 
            // _cmdSwitch_150
            // 
            this._cmdSwitch_150.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this._cmdSwitch_150.Location = new System.Drawing.Point(15, 152);
            this._cmdSwitch_150.Name = "_cmdSwitch_150";
            this._cmdSwitch_150.Size = new System.Drawing.Size(65, 24);
            this._cmdSwitch_150.TabIndex = 30;
            this._cmdSwitch_150.Text = "位置決め";
            this._cmdSwitch_150.UseVisualStyleBackColor = false;
            // 
            // _txtSpeed_3
            // 
            this._txtSpeed_3.AcceptsReturn = true;
            this._txtSpeed_3.BackColor = System.Drawing.SystemColors.Window;
            this._txtSpeed_3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._txtSpeed_3.ForeColor = System.Drawing.SystemColors.WindowText;
            this._txtSpeed_3.Location = new System.Drawing.Point(86, 72);
            this._txtSpeed_3.MaxLength = 0;
            this._txtSpeed_3.Multiline = true;
            this._txtSpeed_3.Name = "_txtSpeed_3";
            this._txtSpeed_3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._txtSpeed_3.Size = new System.Drawing.Size(41, 18);
            this._txtSpeed_3.TabIndex = 18;
            this._txtSpeed_3.Text = "0";
            this._txtSpeed_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _cmdSwitch_36
            // 
            this._cmdSwitch_36.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_36.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_36.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_36.Location = new System.Drawing.Point(8, 120);
            this._cmdSwitch_36.Name = "_cmdSwitch_36";
            this._cmdSwitch_36.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_36.Size = new System.Drawing.Size(73, 25);
            this._cmdSwitch_36.TabIndex = 17;
            this._cmdSwitch_36.Text = "原点復帰";
            this._cmdSwitch_36.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_34
            // 
            this._cmdSwitch_34.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_34.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_34.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_34.Location = new System.Drawing.Point(104, 120);
            this._cmdSwitch_34.Name = "_cmdSwitch_34";
            this._cmdSwitch_34.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_34.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_34.TabIndex = 16;
            this._cmdSwitch_34.Text = "上昇";
            this._cmdSwitch_34.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_35
            // 
            this._cmdSwitch_35.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_35.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_35.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_35.Location = new System.Drawing.Point(104, 152);
            this._cmdSwitch_35.Name = "_cmdSwitch_35";
            this._cmdSwitch_35.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_35.Size = new System.Drawing.Size(49, 25);
            this._cmdSwitch_35.TabIndex = 15;
            this._cmdSwitch_35.Text = "下降";
            this._cmdSwitch_35.UseVisualStyleBackColor = false;
            // 
            // _hsbSpeed_3
            // 
            this._hsbSpeed_3.Cursor = System.Windows.Forms.Cursors.Default;
            this._hsbSpeed_3.LargeChange = 1;
            this._hsbSpeed_3.Location = new System.Drawing.Point(8, 96);
            this._hsbSpeed_3.Maximum = 32767;
            this._hsbSpeed_3.Name = "_hsbSpeed_3";
            this._hsbSpeed_3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._hsbSpeed_3.Size = new System.Drawing.Size(145, 17);
            this._hsbSpeed_3.TabIndex = 14;
            this._hsbSpeed_3.TabStop = true;
            // 
            // _Label1_15
            // 
            this._Label1_15.AutoSize = true;
            this._Label1_15.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_15.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_15.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_15.Location = new System.Drawing.Point(8, 24);
            this._Label1_15.Name = "_Label1_15";
            this._Label1_15.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_15.Size = new System.Drawing.Size(47, 12);
            this._Label1_15.TabIndex = 29;
            this._Label1_15.Text = "現在値：";
            // 
            // _Label1_19
            // 
            this._Label1_19.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_19.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_19.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._Label1_19.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_19.Location = new System.Drawing.Point(128, 24);
            this._Label1_19.Name = "_Label1_19";
            this._Label1_19.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_19.Size = new System.Drawing.Size(17, 9);
            this._Label1_19.TabIndex = 28;
            this._Label1_19.Text = "mm";
            // 
            // _Label1_16
            // 
            this._Label1_16.AutoSize = true;
            this._Label1_16.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_16.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_16.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_16.Location = new System.Drawing.Point(8, 40);
            this._Label1_16.Name = "_Label1_16";
            this._Label1_16.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_16.Size = new System.Drawing.Size(83, 12);
            this._Label1_16.TabIndex = 27;
            this._Label1_16.Text = "最高速度設定：";
            // 
            // _Label1_20
            // 
            this._Label1_20.AutoSize = true;
            this._Label1_20.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_20.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_20.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._Label1_20.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_20.Location = new System.Drawing.Point(128, 40);
            this._Label1_20.Name = "_Label1_20";
            this._Label1_20.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_20.Size = new System.Drawing.Size(32, 11);
            this._Label1_20.TabIndex = 26;
            this._Label1_20.Text = "mm/s";
            // 
            // _Label1_17
            // 
            this._Label1_17.AutoSize = true;
            this._Label1_17.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_17.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_17.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_17.Location = new System.Drawing.Point(8, 56);
            this._Label1_17.Name = "_Label1_17";
            this._Label1_17.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_17.Size = new System.Drawing.Size(83, 12);
            this._Label1_17.TabIndex = 25;
            this._Label1_17.Text = "最低速度設定：";
            // 
            // _Label1_21
            // 
            this._Label1_21.AutoSize = true;
            this._Label1_21.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_21.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_21.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._Label1_21.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_21.Location = new System.Drawing.Point(128, 56);
            this._Label1_21.Name = "_Label1_21";
            this._Label1_21.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_21.Size = new System.Drawing.Size(32, 11);
            this._Label1_21.TabIndex = 24;
            this._Label1_21.Text = "mm/s";
            // 
            // _Label1_18
            // 
            this._Label1_18.AutoSize = true;
            this._Label1_18.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_18.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_18.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_18.Location = new System.Drawing.Point(8, 72);
            this._Label1_18.Name = "_Label1_18";
            this._Label1_18.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_18.Size = new System.Drawing.Size(83, 12);
            this._Label1_18.TabIndex = 23;
            this._Label1_18.Text = "運転速度設定：";
            // 
            // _Label1_22
            // 
            this._Label1_22.AutoSize = true;
            this._Label1_22.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_22.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_22.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._Label1_22.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_22.Location = new System.Drawing.Point(128, 80);
            this._Label1_22.Name = "_Label1_22";
            this._Label1_22.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_22.Size = new System.Drawing.Size(32, 11);
            this._Label1_22.TabIndex = 22;
            this._Label1_22.Text = "mm/s";
            // 
            // lblUDPosition
            // 
            this.lblUDPosition.BackColor = System.Drawing.SystemColors.Control;
            this.lblUDPosition.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblUDPosition.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblUDPosition.Location = new System.Drawing.Point(67, 24);
            this.lblUDPosition.Name = "lblUDPosition";
            this.lblUDPosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblUDPosition.Size = new System.Drawing.Size(54, 12);
            this.lblUDPosition.TabIndex = 21;
            this.lblUDPosition.Text = "0";
            this.lblUDPosition.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblUDMaxSpeed
            // 
            this.lblUDMaxSpeed.BackColor = System.Drawing.SystemColors.Control;
            this.lblUDMaxSpeed.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblUDMaxSpeed.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblUDMaxSpeed.Location = new System.Drawing.Point(85, 40);
            this.lblUDMaxSpeed.Name = "lblUDMaxSpeed";
            this.lblUDMaxSpeed.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblUDMaxSpeed.Size = new System.Drawing.Size(37, 12);
            this.lblUDMaxSpeed.TabIndex = 20;
            this.lblUDMaxSpeed.Text = "20";
            this.lblUDMaxSpeed.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblUDMinSpeed
            // 
            this.lblUDMinSpeed.BackColor = System.Drawing.SystemColors.Control;
            this.lblUDMinSpeed.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblUDMinSpeed.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblUDMinSpeed.Location = new System.Drawing.Point(83, 56);
            this.lblUDMinSpeed.Name = "lblUDMinSpeed";
            this.lblUDMinSpeed.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblUDMinSpeed.Size = new System.Drawing.Size(38, 12);
            this.lblUDMinSpeed.TabIndex = 19;
            this.lblUDMinSpeed.Text = "0.1";
            this.lblUDMinSpeed.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Frame11
            // 
            this.Frame11.BackColor = System.Drawing.SystemColors.Control;
            this.Frame11.Controls.Add(this._cmdSwitch_0);
            this.Frame11.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Frame11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame11.Location = new System.Drawing.Point(8, 8);
            this.Frame11.Name = "Frame11";
            this.Frame11.Padding = new System.Windows.Forms.Padding(0);
            this.Frame11.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame11.Size = new System.Drawing.Size(97, 49);
            this.Frame11.TabIndex = 11;
            this.Frame11.TabStop = false;
            this.Frame11.Text = "ｴﾗｰ表示､ﾌﾞｻﾞｰ";
            // 
            // _cmdSwitch_0
            // 
            this._cmdSwitch_0.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_0.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_0.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_0.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_0.Location = new System.Drawing.Point(24, 16);
            this._cmdSwitch_0.Name = "_cmdSwitch_0";
            this._cmdSwitch_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_0.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_0.TabIndex = 12;
            this._cmdSwitch_0.Text = "OFF";
            this._cmdSwitch_0.UseVisualStyleBackColor = false;
            // 
            // Frame13
            // 
            this.Frame13.BackColor = System.Drawing.SystemColors.Control;
            this.Frame13.Controls.Add(this._cmdSwitch_39);
            this.Frame13.Controls.Add(this._cmdSwitch_40);
            this.Frame13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame13.Location = new System.Drawing.Point(8, 64);
            this.Frame13.Name = "Frame13";
            this.Frame13.Padding = new System.Windows.Forms.Padding(0);
            this.Frame13.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame13.Size = new System.Drawing.Size(97, 49);
            this.Frame13.TabIndex = 8;
            this.Frame13.TabStop = false;
            this.Frame13.Text = "ﾀｯﾁﾊﾟﾈﾙ操作";
            // 
            // _cmdSwitch_39
            // 
            this._cmdSwitch_39.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_39.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_39.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_39.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_39.Location = new System.Drawing.Point(8, 16);
            this._cmdSwitch_39.Name = "_cmdSwitch_39";
            this._cmdSwitch_39.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_39.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_39.TabIndex = 10;
            this._cmdSwitch_39.Text = "禁止";
            this._cmdSwitch_39.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_40
            // 
            this._cmdSwitch_40.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_40.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_40.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_40.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_40.Location = new System.Drawing.Point(48, 16);
            this._cmdSwitch_40.Name = "_cmdSwitch_40";
            this._cmdSwitch_40.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_40.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_40.TabIndex = 9;
            this._cmdSwitch_40.Text = "許可";
            this._cmdSwitch_40.UseVisualStyleBackColor = false;
            // 
            // Frame14
            // 
            this.Frame14.BackColor = System.Drawing.SystemColors.Control;
            this.Frame14.Controls.Add(this._cmdSwitch_41);
            this.Frame14.Controls.Add(this._cmdSwitch_42);
            this.Frame14.Controls.Add(this.lblXray);
            this.Frame14.Controls.Add(this._Label1_39);
            this.Frame14.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Frame14.Location = new System.Drawing.Point(8, 128);
            this.Frame14.Name = "Frame14";
            this.Frame14.Padding = new System.Windows.Forms.Padding(0);
            this.Frame14.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Frame14.Size = new System.Drawing.Size(97, 65);
            this.Frame14.TabIndex = 5;
            this.Frame14.TabStop = false;
            this.Frame14.Text = "X線外部制御";
            // 
            // _cmdSwitch_41
            // 
            this._cmdSwitch_41.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_41.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_41.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_41.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_41.Location = new System.Drawing.Point(48, 32);
            this._cmdSwitch_41.Name = "_cmdSwitch_41";
            this._cmdSwitch_41.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_41.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_41.TabIndex = 7;
            this._cmdSwitch_41.Text = "OFF";
            this._cmdSwitch_41.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_42
            // 
            this._cmdSwitch_42.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_42.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_42.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_42.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_42.Location = new System.Drawing.Point(8, 32);
            this._cmdSwitch_42.Name = "_cmdSwitch_42";
            this._cmdSwitch_42.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_42.Size = new System.Drawing.Size(41, 25);
            this._cmdSwitch_42.TabIndex = 6;
            this._cmdSwitch_42.Text = "ON";
            this._cmdSwitch_42.UseVisualStyleBackColor = false;
            // 
            // lblXray
            // 
            this.lblXray.AutoSize = true;
            this.lblXray.BackColor = System.Drawing.SystemColors.Control;
            this.lblXray.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblXray.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblXray.Location = new System.Drawing.Point(72, 24);
            this.lblXray.Name = "lblXray";
            this.lblXray.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblXray.Size = new System.Drawing.Size(0, 12);
            this.lblXray.TabIndex = 103;
            // 
            // _Label1_39
            // 
            this._Label1_39.AutoSize = true;
            this._Label1_39.BackColor = System.Drawing.SystemColors.Control;
            this._Label1_39.Cursor = System.Windows.Forms.Cursors.Default;
            this._Label1_39.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Label1_39.Location = new System.Drawing.Point(8, 16);
            this._Label1_39.Name = "_Label1_39";
            this._Label1_39.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Label1_39.Size = new System.Drawing.Size(58, 12);
            this._Label1_39.TabIndex = 102;
            this._Label1_39.Text = "X線照射 ：";
            // 
            // _Frame15_0
            // 
            this._Frame15_0.BackColor = System.Drawing.SystemColors.Control;
            this._Frame15_0.Controls.Add(this._cmdSwitch_104);
            this._Frame15_0.Controls.Add(this._cmdSwitch_91);
            this._Frame15_0.Controls.Add(this._cmdSwitch_90);
            this._Frame15_0.Controls.Add(this._cmdSwitch_89);
            this._Frame15_0.Controls.Add(this._cmdSwitch_88);
            this._Frame15_0.Controls.Add(this._cmdSwitch_54);
            this._Frame15_0.Controls.Add(this._cmdSwitch_53);
            this._Frame15_0.Controls.Add(this._cmdSwitch_52);
            this._Frame15_0.Controls.Add(this._cmdSwitch_51);
            this._Frame15_0.Controls.Add(this._cmdSwitch_47);
            this._Frame15_0.Controls.Add(this._cmdSwitch_48);
            this._Frame15_0.Controls.Add(this._cmdSwitch_49);
            this._Frame15_0.Controls.Add(this._cmdSwitch_50);
            this._Frame15_0.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Frame15_0.Location = new System.Drawing.Point(272, 472);
            this._Frame15_0.Name = "_Frame15_0";
            this._Frame15_0.Padding = new System.Windows.Forms.Padding(0);
            this._Frame15_0.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._Frame15_0.Size = new System.Drawing.Size(257, 121);
            this._Frame15_0.TabIndex = 0;
            this._Frame15_0.TabStop = false;
            this._Frame15_0.Text = "移動有ﾘｾｯﾄ";
            // 
            // _cmdSwitch_104
            // 
            this._cmdSwitch_104.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_104.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_104.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_104.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_104.Location = new System.Drawing.Point(200, 48);
            this._cmdSwitch_104.Name = "_cmdSwitch_104";
            this._cmdSwitch_104.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_104.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_104.TabIndex = 249;
            this._cmdSwitch_104.Text = "SP    I.I.軸";
            this._cmdSwitch_104.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_91
            // 
            this._cmdSwitch_91.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_91.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_91.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_91.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_91.Location = new System.Drawing.Point(152, 80);
            this._cmdSwitch_91.Name = "_cmdSwitch_91";
            this._cmdSwitch_91.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_91.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_91.TabIndex = 213;
            this._cmdSwitch_91.Text = "Xray DisY軸";
            this._cmdSwitch_91.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_90
            // 
            this._cmdSwitch_90.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_90.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_90.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_90.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_90.Location = new System.Drawing.Point(104, 80);
            this._cmdSwitch_90.Name = "_cmdSwitch_90";
            this._cmdSwitch_90.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_90.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_90.TabIndex = 212;
            this._cmdSwitch_90.Text = "Xray RotY軸";
            this._cmdSwitch_90.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_89
            // 
            this._cmdSwitch_89.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_89.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_89.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_89.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_89.Location = new System.Drawing.Point(56, 80);
            this._cmdSwitch_89.Name = "_cmdSwitch_89";
            this._cmdSwitch_89.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_89.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_89.TabIndex = 211;
            this._cmdSwitch_89.Text = "Xray DisX軸";
            this._cmdSwitch_89.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_88
            // 
            this._cmdSwitch_88.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_88.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_88.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_88.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_88.Location = new System.Drawing.Point(8, 80);
            this._cmdSwitch_88.Name = "_cmdSwitch_88";
            this._cmdSwitch_88.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_88.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_88.TabIndex = 210;
            this._cmdSwitch_88.Text = "Xray RotX軸";
            this._cmdSwitch_88.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_54
            // 
            this._cmdSwitch_54.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_54.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_54.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_54.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_54.Location = new System.Drawing.Point(152, 48);
            this._cmdSwitch_54.Name = "_cmdSwitch_54";
            this._cmdSwitch_54.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_54.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_54.TabIndex = 101;
            this._cmdSwitch_54.Text = "Dis    I.I.軸";
            this._cmdSwitch_54.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_53
            // 
            this._cmdSwitch_53.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_53.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_53.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_53.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_53.Location = new System.Drawing.Point(104, 48);
            this._cmdSwitch_53.Name = "_cmdSwitch_53";
            this._cmdSwitch_53.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_53.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_53.TabIndex = 100;
            this._cmdSwitch_53.Text = "Gain  I.I.軸";
            this._cmdSwitch_53.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_52
            // 
            this._cmdSwitch_52.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_52.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_52.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_52.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_52.Location = new System.Drawing.Point(56, 48);
            this._cmdSwitch_52.Name = "_cmdSwitch_52";
            this._cmdSwitch_52.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_52.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_52.TabIndex = 99;
            this._cmdSwitch_52.Text = "Rot   I.I.軸";
            this._cmdSwitch_52.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_51
            // 
            this._cmdSwitch_51.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_51.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_51.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_51.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_51.Location = new System.Drawing.Point(8, 48);
            this._cmdSwitch_51.Name = "_cmdSwitch_51";
            this._cmdSwitch_51.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_51.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_51.TabIndex = 98;
            this._cmdSwitch_51.Text = "Ver   I.I.軸";
            this._cmdSwitch_51.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_47
            // 
            this._cmdSwitch_47.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_47.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_47.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_47.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_47.Location = new System.Drawing.Point(8, 16);
            this._cmdSwitch_47.Name = "_cmdSwitch_47";
            this._cmdSwitch_47.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_47.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_47.TabIndex = 4;
            this._cmdSwitch_47.Text = "RotX軸";
            this._cmdSwitch_47.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_48
            // 
            this._cmdSwitch_48.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_48.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_48.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_48.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_48.Location = new System.Drawing.Point(56, 16);
            this._cmdSwitch_48.Name = "_cmdSwitch_48";
            this._cmdSwitch_48.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_48.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_48.TabIndex = 3;
            this._cmdSwitch_48.Text = "DisX軸";
            this._cmdSwitch_48.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_49
            // 
            this._cmdSwitch_49.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_49.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_49.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_49.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_49.Location = new System.Drawing.Point(104, 16);
            this._cmdSwitch_49.Name = "_cmdSwitch_49";
            this._cmdSwitch_49.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_49.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_49.TabIndex = 2;
            this._cmdSwitch_49.Text = "RotY軸";
            this._cmdSwitch_49.UseVisualStyleBackColor = false;
            // 
            // _cmdSwitch_50
            // 
            this._cmdSwitch_50.BackColor = System.Drawing.SystemColors.Control;
            this._cmdSwitch_50.Cursor = System.Windows.Forms.Cursors.Default;
            this._cmdSwitch_50.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._cmdSwitch_50.ForeColor = System.Drawing.SystemColors.ControlText;
            this._cmdSwitch_50.Location = new System.Drawing.Point(152, 16);
            this._cmdSwitch_50.Name = "_cmdSwitch_50";
            this._cmdSwitch_50.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._cmdSwitch_50.Size = new System.Drawing.Size(49, 33);
            this._cmdSwitch_50.TabIndex = 1;
            this._cmdSwitch_50.Text = "DisY軸";
            this._cmdSwitch_50.UseVisualStyleBackColor = false;
            // 
            // frmMechaControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(887, 748);
            this.ControlBox = false;
            this.Controls.Add(this._Frame12_6);
            this.Controls.Add(this._Frame12_5);
            this.Controls.Add(this._Frame12_4);
            this.Controls.Add(this._Frame12_3);
            this.Controls.Add(this.Frame28);
            this.Controls.Add(this.Frame27);
            this.Controls.Add(this.Frame26);
            this.Controls.Add(this._Frame12_2);
            this.Controls.Add(this._Frame15_1);
            this.Controls.Add(this.Frame25);
            this.Controls.Add(this._Frame12_1);
            this.Controls.Add(this.Frame24);
            this.Controls.Add(this.Frame23);
            this.Controls.Add(this.Frame22);
            this.Controls.Add(this.Frame21);
            this.Controls.Add(this.Frame20);
            this.Controls.Add(this.Frame19);
            this.Controls.Add(this.Frame18);
            this.Controls.Add(this.Frame17);
            this.Controls.Add(this.Frame16);
            this.Controls.Add(this.Frame8);
            this.Controls.Add(this._Frame12_0);
            this.Controls.Add(this.Frame1);
            this.Controls.Add(this.Frame2);
            this.Controls.Add(this.Frame3);
            this.Controls.Add(this.Frame4);
            this.Controls.Add(this.Frame5);
            this.Controls.Add(this.Frame6);
            this.Controls.Add(this.Frame7);
            this.Controls.Add(this.Frame9);
            this.Controls.Add(this.Frame10);
            this.Controls.Add(this.Frame11);
            this.Controls.Add(this.Frame13);
            this.Controls.Add(this.Frame14);
            this.Controls.Add(this._Frame15_0);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Location = new System.Drawing.Point(691, 34);
            this.Name = "frmMechaControl";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ﾒｶｺﾝﾄﾛｰﾙ";
            this.Load += new System.EventHandler(this.frmMechaControl_Load);
            this._Frame12_6.ResumeLayout(false);
            this._Frame12_5.ResumeLayout(false);
            this._Frame12_4.ResumeLayout(false);
            this._Frame12_3.ResumeLayout(false);
            this.Frame28.ResumeLayout(false);
            this.Frame27.ResumeLayout(false);
            this.Frame26.ResumeLayout(false);
            this._Frame12_2.ResumeLayout(false);
            this._Frame15_1.ResumeLayout(false);
            this.Frame25.ResumeLayout(false);
            this.Frame25.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this._Frame12_1.ResumeLayout(false);
            this.Frame24.ResumeLayout(false);
            this.Frame24.PerformLayout();
            this.Frame23.ResumeLayout(false);
            this.Frame23.PerformLayout();
            this.Frame22.ResumeLayout(false);
            this.Frame22.PerformLayout();
            this.Frame21.ResumeLayout(false);
            this.Frame21.PerformLayout();
            this.Frame20.ResumeLayout(false);
            this.Frame19.ResumeLayout(false);
            this.Frame18.ResumeLayout(false);
            this.Frame17.ResumeLayout(false);
            this.Frame16.ResumeLayout(false);
            this.Frame16.PerformLayout();
            this.Frame8.ResumeLayout(false);
            this.Frame8.PerformLayout();
            this._Frame12_0.ResumeLayout(false);
            this.Frame1.ResumeLayout(false);
            this.Frame1.PerformLayout();
            this.Frame2.ResumeLayout(false);
            this.Frame2.PerformLayout();
            this.Frame3.ResumeLayout(false);
            this.Frame3.PerformLayout();
            this.Frame4.ResumeLayout(false);
            this.Frame5.ResumeLayout(false);
            this.Frame6.ResumeLayout(false);
            this.Frame7.ResumeLayout(false);
            this.Frame9.ResumeLayout(false);
            this.Frame9.PerformLayout();
            this.Frame10.ResumeLayout(false);
            this.Frame10.PerformLayout();
            this.Frame11.ResumeLayout(false);
            this.Frame13.ResumeLayout(false);
            this.Frame14.ResumeLayout(false);
            this.Frame14.PerformLayout();
            this._Frame15_0.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        public Button _cmdSwitch_149;
        private Button _cmdSwitch_150;
        private NumericUpDown numericUpDown2;
        public NumericUpDown numericUpDown1;
	}
}
