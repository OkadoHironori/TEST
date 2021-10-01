namespace XrayService
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.cmdXon = new System.Windows.Forms.Button();
            this.cmdXoff = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudVolt = new System.Windows.Forms.NumericUpDown();
            this.nudAmper = new System.Windows.Forms.NumericUpDown();
            this.cmdEventSet = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSetmAErr = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtSetkVErr = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtFocus = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.txtError = new System.Windows.Forms.TextBox();
            this.txtSetVolt = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtInterLock = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAmp = new System.Windows.Forms.TextBox();
            this.txtVolt = new System.Windows.Forms.TextBox();
            this.txtOnoff = new System.Windows.Forms.TextBox();
            this.txtWarmupEnd = new System.Windows.Forms.TextBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSetAmp = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.nudFocus = new System.Windows.Forms.NumericUpDown();
            this.XrayStatus3 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtSAD = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnADJ = new System.Windows.Forms.Button();
            this.btnADA = new System.Windows.Forms.Button();
            this.pnldmy = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.nudVolt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmper)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFocus)).BeginInit();
            this.XrayStatus3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdXon
            // 
            this.cmdXon.Location = new System.Drawing.Point(25, 28);
            this.cmdXon.Name = "cmdXon";
            this.cmdXon.Size = new System.Drawing.Size(80, 30);
            this.cmdXon.TabIndex = 0;
            this.cmdXon.Text = "Xon";
            this.cmdXon.UseVisualStyleBackColor = true;
            this.cmdXon.Click += new System.EventHandler(this.cmdXon_Click);
            // 
            // cmdXoff
            // 
            this.cmdXoff.Location = new System.Drawing.Point(128, 29);
            this.cmdXoff.Name = "cmdXoff";
            this.cmdXoff.Size = new System.Drawing.Size(78, 29);
            this.cmdXoff.TabIndex = 1;
            this.cmdXoff.Text = "Xoff";
            this.cmdXoff.UseVisualStyleBackColor = true;
            this.cmdXoff.Click += new System.EventHandler(this.cmdXoff_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "管電圧";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "管電流";
            // 
            // nudVolt
            // 
            this.nudVolt.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nudVolt.Location = new System.Drawing.Point(101, 75);
            this.nudVolt.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.nudVolt.Name = "nudVolt";
            this.nudVolt.Size = new System.Drawing.Size(105, 22);
            this.nudVolt.TabIndex = 4;
            this.nudVolt.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudVolt.ValueChanged += new System.EventHandler(this.nudVolt_ValueChanged);
            // 
            // nudAmper
            // 
            this.nudAmper.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nudAmper.Location = new System.Drawing.Point(101, 105);
            this.nudAmper.Maximum = new decimal(new int[] {
            1100,
            0,
            0,
            0});
            this.nudAmper.Name = "nudAmper";
            this.nudAmper.Size = new System.Drawing.Size(105, 22);
            this.nudAmper.TabIndex = 5;
            this.nudAmper.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudAmper.ValueChanged += new System.EventHandler(this.nudAmper_ValueChanged);
            // 
            // cmdEventSet
            // 
            this.cmdEventSet.Location = new System.Drawing.Point(25, 326);
            this.cmdEventSet.Name = "cmdEventSet";
            this.cmdEventSet.Size = new System.Drawing.Size(80, 30);
            this.cmdEventSet.TabIndex = 6;
            this.cmdEventSet.Text = "X線選択";
            this.cmdEventSet.UseVisualStyleBackColor = true;
            this.cmdEventSet.Click += new System.EventHandler(this.cmdEventSet_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSetmAErr);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.txtSetkVErr);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txtFocus);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtError);
            this.groupBox1.Controls.Add(this.txtSetVolt);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtInterLock);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtAmp);
            this.groupBox1.Controls.Add(this.txtVolt);
            this.groupBox1.Controls.Add(this.txtOnoff);
            this.groupBox1.Controls.Add(this.txtWarmupEnd);
            this.groupBox1.Controls.Add(this.txtStatus);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(234, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(214, 303);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "イベント";
            // 
            // txtSetmAErr
            // 
            this.txtSetmAErr.Location = new System.Drawing.Point(112, 266);
            this.txtSetmAErr.Name = "txtSetmAErr";
            this.txtSetmAErr.Size = new System.Drawing.Size(79, 19);
            this.txtSetmAErr.TabIndex = 42;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(19, 270);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(73, 12);
            this.label15.TabIndex = 41;
            this.label15.Text = "mA設定エラー";
            // 
            // txtSetkVErr
            // 
            this.txtSetkVErr.Location = new System.Drawing.Point(112, 245);
            this.txtSetkVErr.Name = "txtSetkVErr";
            this.txtSetkVErr.Size = new System.Drawing.Size(79, 19);
            this.txtSetkVErr.TabIndex = 40;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(19, 249);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(70, 12);
            this.label14.TabIndex = 39;
            this.label14.Text = "kV設定エラー";
            // 
            // txtFocus
            // 
            this.txtFocus.Location = new System.Drawing.Point(112, 199);
            this.txtFocus.Name = "txtFocus";
            this.txtFocus.Size = new System.Drawing.Size(79, 19);
            this.txtFocus.TabIndex = 38;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(18, 202);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(36, 12);
            this.label12.TabIndex = 37;
            this.label12.Text = "Focus";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("MS UI Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button1.Location = new System.Drawing.Point(59, 224);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(49, 18);
            this.button1.TabIndex = 36;
            this.button1.Text = "リセット";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(18, 226);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 12);
            this.label11.TabIndex = 35;
            this.label11.Text = "エラー";
            // 
            // txtError
            // 
            this.txtError.Location = new System.Drawing.Point(112, 223);
            this.txtError.Name = "txtError";
            this.txtError.Size = new System.Drawing.Size(79, 19);
            this.txtError.TabIndex = 34;
            // 
            // txtSetVolt
            // 
            this.txtSetVolt.Location = new System.Drawing.Point(112, 134);
            this.txtSetVolt.Name = "txtSetVolt";
            this.txtSetVolt.Size = new System.Drawing.Size(79, 19);
            this.txtSetVolt.TabIndex = 33;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 137);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 32;
            this.label9.Text = "設定電圧";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 181);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 12);
            this.label8.TabIndex = 31;
            this.label8.Text = "InterLock";
            // 
            // txtInterLock
            // 
            this.txtInterLock.Location = new System.Drawing.Point(112, 178);
            this.txtInterLock.Name = "txtInterLock";
            this.txtInterLock.Size = new System.Drawing.Size(79, 19);
            this.txtInterLock.TabIndex = 30;
            this.txtInterLock.TextChanged += new System.EventHandler(this.textBox7_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 12);
            this.label7.TabIndex = 29;
            this.label7.Text = "ステータス";
            // 
            // txtAmp
            // 
            this.txtAmp.Location = new System.Drawing.Point(112, 112);
            this.txtAmp.Name = "txtAmp";
            this.txtAmp.Size = new System.Drawing.Size(79, 19);
            this.txtAmp.TabIndex = 28;
            // 
            // txtVolt
            // 
            this.txtVolt.Location = new System.Drawing.Point(112, 90);
            this.txtVolt.Name = "txtVolt";
            this.txtVolt.Size = new System.Drawing.Size(79, 19);
            this.txtVolt.TabIndex = 27;
            // 
            // txtOnoff
            // 
            this.txtOnoff.Location = new System.Drawing.Point(112, 68);
            this.txtOnoff.Name = "txtOnoff";
            this.txtOnoff.Size = new System.Drawing.Size(79, 19);
            this.txtOnoff.TabIndex = 26;
            // 
            // txtWarmupEnd
            // 
            this.txtWarmupEnd.Location = new System.Drawing.Point(112, 46);
            this.txtWarmupEnd.Name = "txtWarmupEnd";
            this.txtWarmupEnd.Size = new System.Drawing.Size(79, 19);
            this.txtWarmupEnd.TabIndex = 25;
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(112, 24);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(79, 19);
            this.txtStatus.TabIndex = 24;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 115);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 12);
            this.label6.TabIndex = 23;
            this.label6.Text = "フィードバック電流";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 12);
            this.label5.TabIndex = 22;
            this.label5.Text = "フィードバック電圧";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 12);
            this.label4.TabIndex = 21;
            this.label4.Text = "Xon/off";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "ウォームアップ";
            // 
            // txtSetAmp
            // 
            this.txtSetAmp.Location = new System.Drawing.Point(346, 185);
            this.txtSetAmp.Name = "txtSetAmp";
            this.txtSetAmp.Size = new System.Drawing.Size(79, 19);
            this.txtSetAmp.TabIndex = 35;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(252, 188);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 34;
            this.label10.Text = "設定電流";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "0:  処理無し",
            "1:  FeinFoucus",
            "2:  ｲﾍﾞﾝﾄ停止",
            "3:  Kevex",
            "4:  浜ﾎﾄ（90kV L9421）",
            "5:  浜ﾎﾄ（130kV L9181）",
            "6:  浜ﾎﾄ（130kV L9191）",
            "7:  浜ﾎﾄ（230kV L10801）",
            "8:  浜ﾎﾄ（90kV L8601）",
            "9:  浜ﾎﾄ（90kV L9421-02）",
            "10: 450kV用",
            "11: 浜ﾎﾄ（150kV L8121-02）",
            "12: 浜ﾎﾄ（130kV L9181-02）",
            "13: 浜ホト(300kV L12721)",
            "14: 浜ホト(160kV L10711)"});
            this.comboBox1.Location = new System.Drawing.Point(25, 362);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(181, 20);
            this.comboBox1.TabIndex = 36;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(25, 133);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(70, 23);
            this.button2.TabIndex = 37;
            this.button2.Text = "Focus Set";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // nudFocus
            // 
            this.nudFocus.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nudFocus.Location = new System.Drawing.Point(101, 133);
            this.nudFocus.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudFocus.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFocus.Name = "nudFocus";
            this.nudFocus.Size = new System.Drawing.Size(105, 22);
            this.nudFocus.TabIndex = 38;
            this.nudFocus.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // XrayStatus3
            // 
            this.XrayStatus3.Controls.Add(this.label13);
            this.XrayStatus3.Controls.Add(this.txtSAD);
            this.XrayStatus3.Location = new System.Drawing.Point(234, 362);
            this.XrayStatus3.Name = "XrayStatus3";
            this.XrayStatus3.Size = new System.Drawing.Size(214, 88);
            this.XrayStatus3.TabIndex = 40;
            this.XrayStatus3.TabStop = false;
            this.XrayStatus3.Text = "XrayStatus3";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(18, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 12);
            this.label13.TabIndex = 37;
            this.label13.Text = "アライメント";
            // 
            // txtSAD
            // 
            this.txtSAD.Location = new System.Drawing.Point(112, 18);
            this.txtSAD.Name = "txtSAD";
            this.txtSAD.Size = new System.Drawing.Size(79, 19);
            this.txtSAD.TabIndex = 36;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "初期設定ファイル(*.ini)|*.ini";
            this.openFileDialog1.Title = "初期設定ファイルを選択";
            // 
            // btnADJ
            // 
            this.btnADJ.Location = new System.Drawing.Point(25, 267);
            this.btnADJ.Name = "btnADJ";
            this.btnADJ.Size = new System.Drawing.Size(98, 21);
            this.btnADJ.TabIndex = 38;
            this.btnADJ.Text = "アライメント";
            this.btnADJ.UseVisualStyleBackColor = true;
            this.btnADJ.Click += new System.EventHandler(this.btnALLADJ_Click);
            // 
            // btnADA
            // 
            this.btnADA.Location = new System.Drawing.Point(25, 294);
            this.btnADA.Name = "btnADA";
            this.btnADA.Size = new System.Drawing.Size(98, 21);
            this.btnADA.TabIndex = 42;
            this.btnADA.Text = "一括アライメント";
            this.btnADA.UseVisualStyleBackColor = true;
            this.btnADA.Click += new System.EventHandler(this.btnADA_Click);
            // 
            // pnldmy
            // 
            this.pnldmy.Location = new System.Drawing.Point(2, 1);
            this.pnldmy.Name = "pnldmy";
            this.pnldmy.Size = new System.Drawing.Size(42, 21);
            this.pnldmy.TabIndex = 43;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(478, 462);
            this.Controls.Add(this.pnldmy);
            this.Controls.Add(this.btnADA);
            this.Controls.Add(this.btnADJ);
            this.Controls.Add(this.XrayStatus3);
            this.Controls.Add(this.nudFocus);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.txtSetAmp);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdEventSet);
            this.Controls.Add(this.nudAmper);
            this.Controls.Add(this.nudVolt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdXoff);
            this.Controls.Add(this.cmdXon);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudVolt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmper)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFocus)).EndInit();
            this.XrayStatus3.ResumeLayout(false);
            this.XrayStatus3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdXon;
        private System.Windows.Forms.Button cmdXoff;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudVolt;
        private System.Windows.Forms.NumericUpDown nudAmper;
        private System.Windows.Forms.Button cmdEventSet;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtInterLock;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtAmp;
        private System.Windows.Forms.TextBox txtVolt;
        private System.Windows.Forms.TextBox txtOnoff;
        private System.Windows.Forms.TextBox txtWarmupEnd;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSetVolt;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSetAmp;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtError;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown nudFocus;
        private System.Windows.Forms.TextBox txtFocus;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox XrayStatus3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtSAD;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnADJ;
        private System.Windows.Forms.Button btnADA;
        private System.Windows.Forms.TextBox txtSetkVErr;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtSetmAErr;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel pnldmy;
    }
}

