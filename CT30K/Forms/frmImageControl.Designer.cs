namespace CT30K
{
	partial class frmImageControl
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.fraImageSearch = new System.Windows.Forms.GroupBox();
            this.lblImageSearchMidL = new System.Windows.Forms.Label();
            this.lblImageSearchMidR = new System.Windows.Forms.Label();
            this.sldImageSearch = new System.Windows.Forms.HScrollBar();
            this.lblImageSearchMax = new System.Windows.Forms.Label();
            this.lblImageSearchMin = new System.Windows.Forms.Label();
            this.cmdSliceNextBack1 = new System.Windows.Forms.Button();
            this.cmdSliceNextBack2 = new System.Windows.Forms.Button();
            this.filSliceName = new System.Windows.Forms.ListBox();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShapeMidL = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShapeMidR = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShapeMax = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShapeMin = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.fraContrast = new System.Windows.Forms.GroupBox();
            this.txtWL = new System.Windows.Forms.TextBox();
            this.txtWW = new System.Windows.Forms.TextBox();
            this.cwneWindowWidth = new System.Windows.Forms.NumericUpDown();
            this.lblWindowWidth = new System.Windows.Forms.Label();
            this.cwneWindowLevel = new System.Windows.Forms.NumericUpDown();
            this.lblGamma = new System.Windows.Forms.Label();
            this.cwneGamma = new System.Windows.Forms.NumericUpDown();
            this.lblWindowLevel = new System.Windows.Forms.Label();
            this.sldWindowWidth = new CT30K.CTSliderH();
            this.sldWindowLevel = new CT30K.CTSliderH();
            this.fraSliderRange = new System.Windows.Forms.GroupBox();
            this.optRangeBit1 = new System.Windows.Forms.RadioButton();
            this.optRangeBit0 = new System.Windows.Forms.RadioButton();
            this.Label1 = new System.Windows.Forms.Label();
            this.cmdAllContrast = new System.Windows.Forms.Button();
            this.cmdUndo = new System.Windows.Forms.Button();
            this.fraImageSearch.SuspendLayout();
            this.fraContrast.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneWindowWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneWindowLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneGamma)).BeginInit();
            this.fraSliderRange.SuspendLayout();
            this.SuspendLayout();
            // 
            // fraImageSearch
            // 
            this.fraImageSearch.Controls.Add(this.lblImageSearchMidL);
            this.fraImageSearch.Controls.Add(this.lblImageSearchMidR);
            this.fraImageSearch.Controls.Add(this.sldImageSearch);
            this.fraImageSearch.Controls.Add(this.lblImageSearchMax);
            this.fraImageSearch.Controls.Add(this.lblImageSearchMin);
            this.fraImageSearch.Controls.Add(this.cmdSliceNextBack1);
            this.fraImageSearch.Controls.Add(this.cmdSliceNextBack2);
            this.fraImageSearch.Controls.Add(this.filSliceName);
            this.fraImageSearch.Controls.Add(this.shapeContainer1);
            this.fraImageSearch.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fraImageSearch.Location = new System.Drawing.Point(488, 4);
            this.fraImageSearch.Name = "fraImageSearch";
            this.fraImageSearch.Size = new System.Drawing.Size(337, 157);
            this.fraImageSearch.TabIndex = 12;
            this.fraImageSearch.TabStop = false;
            this.fraImageSearch.Tag = "10511";
            this.fraImageSearch.Text = "#画像サーチ";
            // 
            // lblImageSearchMidL
            // 
            this.lblImageSearchMidL.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblImageSearchMidL.Location = new System.Drawing.Point(88, 133);
            this.lblImageSearchMidL.Name = "lblImageSearchMidL";
            this.lblImageSearchMidL.Size = new System.Drawing.Size(48, 12);
            this.lblImageSearchMidL.TabIndex = 59;
            this.lblImageSearchMidL.Text = "1024";
            this.lblImageSearchMidL.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblImageSearchMidR
            // 
            this.lblImageSearchMidR.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblImageSearchMidR.Location = new System.Drawing.Point(196, 133);
            this.lblImageSearchMidR.Name = "lblImageSearchMidR";
            this.lblImageSearchMidR.Size = new System.Drawing.Size(48, 12);
            this.lblImageSearchMidR.TabIndex = 58;
            this.lblImageSearchMidR.Text = "2048";
            this.lblImageSearchMidR.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // sldImageSearch
            // 
            this.sldImageSearch.LargeChange = 1;
            this.sldImageSearch.Location = new System.Drawing.Point(21, 108);
            this.sldImageSearch.Minimum = 1;
            this.sldImageSearch.Name = "sldImageSearch";
            this.sldImageSearch.Size = new System.Drawing.Size(293, 16);
            this.sldImageSearch.TabIndex = 56;
            this.sldImageSearch.Value = 1;
            this.sldImageSearch.Visible = false;
            this.sldImageSearch.ValueChanged += new System.EventHandler(this.sldImageSearch_PointerValueChanged);
            // 
            // lblImageSearchMax
            // 
            this.lblImageSearchMax.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblImageSearchMax.Location = new System.Drawing.Point(276, 133);
            this.lblImageSearchMax.Name = "lblImageSearchMax";
            this.lblImageSearchMax.Size = new System.Drawing.Size(48, 12);
            this.lblImageSearchMax.TabIndex = 55;
            this.lblImageSearchMax.Text = "4096";
            this.lblImageSearchMax.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblImageSearchMin
            // 
            this.lblImageSearchMin.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblImageSearchMin.Location = new System.Drawing.Point(12, 133);
            this.lblImageSearchMin.Name = "lblImageSearchMin";
            this.lblImageSearchMin.Size = new System.Drawing.Size(48, 12);
            this.lblImageSearchMin.TabIndex = 54;
            this.lblImageSearchMin.Text = "1";
            this.lblImageSearchMin.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cmdSliceNextBack1
            // 
            this.cmdSliceNextBack1.Enabled = false;
            this.cmdSliceNextBack1.Location = new System.Drawing.Point(172, 44);
            this.cmdSliceNextBack1.Name = "cmdSliceNextBack1";
            this.cmdSliceNextBack1.Size = new System.Drawing.Size(93, 33);
            this.cmdSliceNextBack1.TabIndex = 15;
            this.cmdSliceNextBack1.Tag = "10020";
            this.cmdSliceNextBack1.Text = "#次へ＞";
            this.cmdSliceNextBack1.UseVisualStyleBackColor = true;
            this.cmdSliceNextBack1.Click += new System.EventHandler(this.cmdSliceNextBack_Click);
            // 
            // cmdSliceNextBack2
            // 
            this.cmdSliceNextBack2.Enabled = false;
            this.cmdSliceNextBack2.Location = new System.Drawing.Point(52, 44);
            this.cmdSliceNextBack2.Name = "cmdSliceNextBack2";
            this.cmdSliceNextBack2.Size = new System.Drawing.Size(89, 33);
            this.cmdSliceNextBack2.TabIndex = 14;
            this.cmdSliceNextBack2.Tag = "10021";
            this.cmdSliceNextBack2.Text = "#＜戻る";
            this.cmdSliceNextBack2.UseVisualStyleBackColor = true;
            this.cmdSliceNextBack2.Click += new System.EventHandler(this.cmdSliceNextBack_Click);
            // 
            // filSliceName
            // 
            this.filSliceName.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.filSliceName.FormattingEnabled = true;
            this.filSliceName.ItemHeight = 12;
            this.filSliceName.Location = new System.Drawing.Point(240, 15);
            this.filSliceName.Name = "filSliceName";
            this.filSliceName.Size = new System.Drawing.Size(81, 16);
            this.filSliceName.TabIndex = 13;
            this.filSliceName.Visible = false;
            this.filSliceName.SelectedIndexChanged += new System.EventHandler(this.filSliceName_SelectedIndexChanged);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(3, 18);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShapeMidL,
            this.lineShapeMidR,
            this.lineShapeMax,
            this.lineShapeMin});
            this.shapeContainer1.Size = new System.Drawing.Size(331, 136);
            this.shapeContainer1.TabIndex = 57;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShapeMidL
            // 
            this.lineShapeMidL.Name = "lineShapeMidL";
            this.lineShapeMidL.X1 = 109;
            this.lineShapeMidL.X2 = 109;
            this.lineShapeMidL.Y1 = 107;
            this.lineShapeMidL.Y2 = 112;
            // 
            // lineShapeMidR
            // 
            this.lineShapeMidR.Name = "lineShapeMidR";
            this.lineShapeMidR.X1 = 217;
            this.lineShapeMidR.X2 = 217;
            this.lineShapeMidR.Y1 = 107;
            this.lineShapeMidR.Y2 = 112;
            // 
            // lineShapeMax
            // 
            this.lineShapeMax.Name = "lineShapeMax";
            this.lineShapeMax.X1 = 294;
            this.lineShapeMax.X2 = 294;
            this.lineShapeMax.Y1 = 107;
            this.lineShapeMax.Y2 = 112;
            // 
            // lineShapeMin
            // 
            this.lineShapeMin.Name = "lineShapeMin";
            this.lineShapeMin.X1 = 32;
            this.lineShapeMin.X2 = 32;
            this.lineShapeMin.Y1 = 107;
            this.lineShapeMin.Y2 = 112;
            // 
            // fraContrast
            // 
            this.fraContrast.Controls.Add(this.txtWL);
            this.fraContrast.Controls.Add(this.txtWW);
            this.fraContrast.Controls.Add(this.cwneWindowWidth);
            this.fraContrast.Controls.Add(this.lblWindowWidth);
            this.fraContrast.Controls.Add(this.cwneWindowLevel);
            this.fraContrast.Controls.Add(this.lblGamma);
            this.fraContrast.Controls.Add(this.cwneGamma);
            this.fraContrast.Controls.Add(this.lblWindowLevel);
            this.fraContrast.Controls.Add(this.sldWindowWidth);
            this.fraContrast.Controls.Add(this.sldWindowLevel);
            this.fraContrast.Controls.Add(this.fraSliderRange);
            this.fraContrast.Controls.Add(this.cmdAllContrast);
            this.fraContrast.Controls.Add(this.cmdUndo);
            this.fraContrast.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fraContrast.Location = new System.Drawing.Point(6, 4);
            this.fraContrast.Name = "fraContrast";
            this.fraContrast.Size = new System.Drawing.Size(476, 157);
            this.fraContrast.TabIndex = 39;
            this.fraContrast.TabStop = false;
            this.fraContrast.Tag = "10512";
            this.fraContrast.Text = "#階調変換";
            // 
            // txtWL
            // 
            this.txtWL.Location = new System.Drawing.Point(115, 25);
            this.txtWL.Name = "txtWL";
            this.txtWL.Size = new System.Drawing.Size(55, 22);
            this.txtWL.TabIndex = 127;
            this.txtWL.Text = "0";
            this.txtWL.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtWL.TextChanged += new System.EventHandler(this.txtWLWW_TextChanged);
            this.txtWL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtWLWW_KeyDown);
            this.txtWL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtWLWW_KeyPress);
            this.txtWL.Leave += new System.EventHandler(this.txtWLWW_Leave);
            // 
            // txtWW
            // 
            this.txtWW.Location = new System.Drawing.Point(114, 82);
            this.txtWW.Name = "txtWW";
            this.txtWW.Size = new System.Drawing.Size(55, 22);
            this.txtWW.TabIndex = 128;
            this.txtWW.Text = "0";
            this.txtWW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtWW.TextChanged += new System.EventHandler(this.txtWLWW_TextChanged);
            this.txtWW.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtWLWW_KeyDown);
            this.txtWW.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtWLWW_KeyPress);
            this.txtWW.Leave += new System.EventHandler(this.txtWLWW_Leave);
            // 
            // cwneWindowWidth
            // 
            this.cwneWindowWidth.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneWindowWidth.Location = new System.Drawing.Point(170, 82);
            this.cwneWindowWidth.Maximum = new decimal(new int[] {
            16384,
            0,
            0,
            0});
            this.cwneWindowWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cwneWindowWidth.Name = "cwneWindowWidth";
            this.cwneWindowWidth.Size = new System.Drawing.Size(21, 22);
            this.cwneWindowWidth.TabIndex = 43;
            this.cwneWindowWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneWindowWidth.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.cwneWindowWidth.ValueChanged += new System.EventHandler(this.cwneWindowWidth_ValueChanged);
            // 
            // lblWindowWidth
            // 
            this.lblWindowWidth.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblWindowWidth.Location = new System.Drawing.Point(17, 86);
            this.lblWindowWidth.Name = "lblWindowWidth";
            this.lblWindowWidth.Size = new System.Drawing.Size(95, 13);
            this.lblWindowWidth.TabIndex = 42;
            this.lblWindowWidth.Tag = "12826";
            this.lblWindowWidth.Text = "#ウィンドウ幅";
            this.lblWindowWidth.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cwneWindowLevel
            // 
            this.cwneWindowLevel.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneWindowLevel.Location = new System.Drawing.Point(170, 25);
            this.cwneWindowLevel.Maximum = new decimal(new int[] {
            8191,
            0,
            0,
            0});
            this.cwneWindowLevel.Minimum = new decimal(new int[] {
            8192,
            0,
            0,
            -2147483648});
            this.cwneWindowLevel.Name = "cwneWindowLevel";
            this.cwneWindowLevel.Size = new System.Drawing.Size(21, 22);
            this.cwneWindowLevel.TabIndex = 40;
            this.cwneWindowLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneWindowLevel.ValueChanged += new System.EventHandler(this.cwneWindowLevel_ValueChanged);
            // 
            // lblGamma
            // 
            this.lblGamma.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblGamma.Location = new System.Drawing.Point(195, 29);
            this.lblGamma.Name = "lblGamma";
            this.lblGamma.Size = new System.Drawing.Size(50, 12);
            this.lblGamma.TabIndex = 49;
            this.lblGamma.Tag = "12841";
            this.lblGamma.Text = "#ガンマ";
            this.lblGamma.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cwneGamma
            // 
            this.cwneGamma.DecimalPlaces = 2;
            this.cwneGamma.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cwneGamma.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.cwneGamma.Location = new System.Drawing.Point(252, 25);
            this.cwneGamma.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.cwneGamma.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.cwneGamma.Name = "cwneGamma";
            this.cwneGamma.Size = new System.Drawing.Size(69, 22);
            this.cwneGamma.TabIndex = 48;
            this.cwneGamma.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.cwneGamma.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.cwneGamma.Value = new decimal(new int[] {
            100,
            0,
            0,
            131072});
            this.cwneGamma.ValueChanged += new System.EventHandler(this.cwneGamma_ValueChanged);
            // 
            // lblWindowLevel
            // 
            this.lblWindowLevel.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblWindowLevel.Location = new System.Drawing.Point(17, 29);
            this.lblWindowLevel.Name = "lblWindowLevel";
            this.lblWindowLevel.Size = new System.Drawing.Size(95, 13);
            this.lblWindowLevel.TabIndex = 39;
            this.lblWindowLevel.Tag = "12825";
            this.lblWindowLevel.Text = "#ウィンドウレベル";
            this.lblWindowLevel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sldWindowWidth
            // 
            this.sldWindowWidth.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.sldWindowWidth.LargeChange = 5;
            this.sldWindowWidth.Location = new System.Drawing.Point(14, 110);
            this.sldWindowWidth.Maximum = 4096;
            this.sldWindowWidth.Minimum = 1;
            this.sldWindowWidth.Name = "sldWindowWidth";
            this.sldWindowWidth.Reverse = false;
            this.sldWindowWidth.Size = new System.Drawing.Size(335, 32);
            this.sldWindowWidth.SmallChange = 1;
            this.sldWindowWidth.TabIndex = 57;
            this.sldWindowWidth.TicksLabel = CT30K.CTSliderH.TicksLabelType.TicksLabelMidOff;
            this.sldWindowWidth.TicksLine = false;
            this.sldWindowWidth.Value = 2048;
            this.sldWindowWidth.ValueChanged += new System.EventHandler(this.sldWindowWidth_PointerValueChanged);
            this.sldWindowWidth.MouseCaptureChanged += new System.EventHandler(this.sldWindowWidth_MouseCaptureChanged);
            // 
            // sldWindowLevel
            // 
            this.sldWindowLevel.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.sldWindowLevel.LargeChange = 5;
            this.sldWindowLevel.Location = new System.Drawing.Point(14, 51);
            this.sldWindowLevel.Maximum = 2047;
            this.sldWindowLevel.Minimum = -2048;
            this.sldWindowLevel.Name = "sldWindowLevel";
            this.sldWindowLevel.Reverse = false;
            this.sldWindowLevel.Size = new System.Drawing.Size(335, 32);
            this.sldWindowLevel.SmallChange = 1;
            this.sldWindowLevel.TabIndex = 56;
            this.sldWindowLevel.TicksLabel = CT30K.CTSliderH.TicksLabelType.TicksLabelMidOff;
            this.sldWindowLevel.TicksLine = false;
            this.sldWindowLevel.Value = 1;
            this.sldWindowLevel.ValueChanged += new System.EventHandler(this.sldWindowLevel_PointerValueChanged);
            this.sldWindowLevel.MouseCaptureChanged += new System.EventHandler(this.sldWindowLevel_MouseCaptureChanged);
            // 
            // fraSliderRange
            // 
            this.fraSliderRange.Controls.Add(this.optRangeBit1);
            this.fraSliderRange.Controls.Add(this.optRangeBit0);
            this.fraSliderRange.Controls.Add(this.Label1);
            this.fraSliderRange.Font = new System.Drawing.Font("MS UI Gothic", 7.5F);
            this.fraSliderRange.Location = new System.Drawing.Point(362, 83);
            this.fraSliderRange.Name = "fraSliderRange";
            this.fraSliderRange.Size = new System.Drawing.Size(101, 65);
            this.fraSliderRange.TabIndex = 47;
            this.fraSliderRange.TabStop = false;
            this.fraSliderRange.Tag = "12420";
            this.fraSliderRange.Text = "#ｽﾗｲﾀﾞのﾚﾝｼﾞ幅";
            // 
            // optRangeBit1
            // 
            this.optRangeBit1.Location = new System.Drawing.Point(16, 40);
            this.optRangeBit1.Name = "optRangeBit1";
            this.optRangeBit1.Size = new System.Drawing.Size(81, 17);
            this.optRangeBit1.TabIndex = 11;
            this.optRangeBit1.Tag = "12422";
            this.optRangeBit1.Text = "#14ビット";
            this.optRangeBit1.UseVisualStyleBackColor = true;
            this.optRangeBit1.CheckedChanged += new System.EventHandler(this.optRangeBit_CheckedChanged);
            // 
            // optRangeBit0
            // 
            this.optRangeBit0.Location = new System.Drawing.Point(16, 16);
            this.optRangeBit0.Name = "optRangeBit0";
            this.optRangeBit0.Size = new System.Drawing.Size(77, 17);
            this.optRangeBit0.TabIndex = 10;
            this.optRangeBit0.Tag = "12421";
            this.optRangeBit0.Text = "#12ビット";
            this.optRangeBit0.UseVisualStyleBackColor = true;
            this.optRangeBit0.CheckedChanged += new System.EventHandler(this.optRangeBit_CheckedChanged);
            // 
            // Label1
            // 
            this.Label1.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.Label1.Location = new System.Drawing.Point(8, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(81, 33);
            this.Label1.TabIndex = 17;
            this.Label1.Text = "Range of width of a slider";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Label1.Visible = false;
            // 
            // cmdAllContrast
            // 
            this.cmdAllContrast.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdAllContrast.Location = new System.Drawing.Point(362, 47);
            this.cmdAllContrast.Name = "cmdAllContrast";
            this.cmdAllContrast.Size = new System.Drawing.Size(109, 33);
            this.cmdAllContrast.TabIndex = 46;
            this.cmdAllContrast.Tag = "10513";
            this.cmdAllContrast.Text = "#階調一括変換";
            this.cmdAllContrast.UseVisualStyleBackColor = true;
            this.cmdAllContrast.Click += new System.EventHandler(this.cmdAllContrast_Click);
            // 
            // cmdUndo
            // 
            this.cmdUndo.Enabled = false;
            this.cmdUndo.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmdUndo.Location = new System.Drawing.Point(362, 9);
            this.cmdUndo.Name = "cmdUndo";
            this.cmdUndo.Size = new System.Drawing.Size(109, 33);
            this.cmdUndo.TabIndex = 45;
            this.cmdUndo.Tag = "10017";
            this.cmdUndo.Text = "#元に戻す";
            this.cmdUndo.UseVisualStyleBackColor = true;
            this.cmdUndo.Click += new System.EventHandler(this.cmdUndo_Click);
            // 
            // frmImageControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(836, 163);
            this.Controls.Add(this.fraContrast);
            this.Controls.Add(this.fraImageSearch);
            this.Name = "frmImageControl";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.Activated += new System.EventHandler(this.frmImageControl_Activated);
            this.Load += new System.EventHandler(this.frmImageControl_Load);
            this.fraImageSearch.ResumeLayout(false);
            this.fraContrast.ResumeLayout(false);
            this.fraContrast.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cwneWindowWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneWindowLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cwneGamma)).EndInit();
            this.fraSliderRange.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.GroupBox fraImageSearch;
        private System.Windows.Forms.ListBox filSliceName;
		private System.Windows.Forms.Button cmdSliceNextBack1;
        private System.Windows.Forms.Button cmdSliceNextBack2;
        private System.Windows.Forms.GroupBox fraContrast;
        internal System.Windows.Forms.NumericUpDown cwneWindowWidth;
        internal System.Windows.Forms.NumericUpDown cwneWindowLevel;
        private System.Windows.Forms.Label lblGamma;
        internal System.Windows.Forms.NumericUpDown cwneGamma;
        private System.Windows.Forms.GroupBox fraSliderRange;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.RadioButton optRangeBit1;
        private System.Windows.Forms.RadioButton optRangeBit0;
        private System.Windows.Forms.Button cmdAllContrast;
        private System.Windows.Forms.Button cmdUndo;
        private System.Windows.Forms.Label lblWindowWidth;
        private System.Windows.Forms.Label lblWindowLevel;
        private System.Windows.Forms.Label lblImageSearchMax;
        private System.Windows.Forms.Label lblImageSearchMin;
        private System.Windows.Forms.HScrollBar sldImageSearch;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShapeMin;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShapeMax;
        private System.Windows.Forms.Label lblImageSearchMidL;
        private System.Windows.Forms.Label lblImageSearchMidR;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShapeMidL;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShapeMidR;
        private CTSliderH sldWindowLevel;
        private CTSliderH sldWindowWidth;
        private System.Windows.Forms.TextBox txtWL;
        private System.Windows.Forms.TextBox txtWW;
	}
}