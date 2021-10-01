using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DioLib;

namespace Xs.DioMonitor
{
    public partial class Form1 : Form
    {
        bool Debug { get; set; }

        bool Controlable { get; set; }

        DioLib.Dio Dio { get; set; }

        List<DiModel> DiModels { get; set; }

        List<DoModel> DoModels { get; set; }

        protected System.Timers.Timer DioTimer;

        public Form1()
        {
            InitializeComponent();
        }

        public Form1(bool debug, bool controlable)
            : this()
        {
            this.Debug = debug;

            this.Controlable = controlable;

            this.KeyPreview = true;
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        private void Init()
        {
            DioFactory factory = DioFactory.GetFactory(DioBoardType.GPC2000, Debug);

            System.Threading.SynchronizationContext context = System.Threading.SynchronizationContext.Current;

            int n = 32;

            Dio = factory.CreateDio(0, n);

            DiModels = Enumerable
                .Range(0, n)
                .Select((i) => new DiModel() { Index = (uint)i, Dio = this.Dio, Context = context, Controlable = this.Controlable, ReadOnly = !Debug })
                .ToList();

            IEnumerable<CheckButton> di_buttons = DiModels.Select((m) => CreateButton(m));
            flowLayoutPanel1.Controls.AddRange(di_buttons.ToArray());

            DoModels = Enumerable
                .Range(0, n)
                .Select((i) => new DoModel() { Index = (uint)i, Dio = this.Dio, Context = context, Controlable = this.Controlable, ReadOnly = !Debug })
                .ToList();

            IEnumerable<CheckButton> do_buttons = DoModels.Select((m) => CreateButton(m));
            flowLayoutPanel2.Controls.AddRange(do_buttons.ToArray());


            Dio.Open();

            DioTimer = new System.Timers.Timer();
            DioTimer.Elapsed += DioTimer_Elapsed;
            DioTimer.Interval = 30;
            DioTimer.Start();
        }

        /// <summary>
        /// 終了時処理
        /// </summary>
        private void Term()
        {
            if (null != DioTimer)
            {
                DioTimer.Stop();

                System.Threading.Thread.Sleep((int)DioTimer.Interval);
            }

            if (null != Dio)
            {
                Dio.Dispose();
                Dio = null;
            }
        }

        readonly object lockobj = new object();

        void DioTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (lockobj)
            {
                try
                {
                    //DI
                    bool[] dibits = new bool[32];

                    this.Dio.GetDiAll(dibits);

                    for (int i = 0; i < DiModels.Count; ++i)
                    {
                        DiModels[i].Checked = dibits[i];
                    }

                    //DO
                    bool[] dobits = new bool[32];

                    this.Dio.GetDo(dobits);

                    for (int i = 0; i < DoModels.Count; ++i)
                    {
                        DoModels[i].Checked = dobits[i];
                    }
                }
                finally
                {
                    //
                }
            }
        }

        private CheckButton CreateButton(DioModel model)
        {
            var button = chkDummy.Clone() as CheckButton;

            button.DioModel = model;

            return button;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Term();
        }

        #region

        //
        System.Threading.Timer Timer;

        public void InitTimer()
        {
            this.Timer = new System.Threading.Timer(new System.Threading.TimerCallback(Timer_Elapsed));
        }

        const int msec = 20;
        const int axiscnt = 15;
        const int meascnt = 4;
        bool loop = false;

        void Timer_Elapsed(object state)
        {
            try
            {
                while (loop)
                {
                    if (!DiModels[15].Checked) return;

                    _title("workset");

                    waitDo(0, true, msec, () => { diSet(0, true, msec); });

                    diSet(0, false, 10);
                    diSet(0, false, 10);

                    pos();

                    axis(axiscnt);

                    for (int i = 0; i < meascnt; ++i)
                        measure();

                    axis(axiscnt);

                    for (int i = 0; i < meascnt; ++i)
                        measure();

                    axis(axiscnt);

                    for (int i = 0; i < meascnt; ++i)
                        measure();

                    axis(axiscnt);

                    for (int i = 0; i < meascnt - 1; ++i)
                        measure();

                    waitDo(4, true, msec, () => { diSet(4, true, msec); });

                    //結果取得
                    diSet(4, true, msec);
                    System.Threading.Thread.Sleep(msec);

                    waitDo(4, false, msec, () => { diSet(4, false, msec); });

                    waitDo(0, false, msec, () => { diSet(0, false, msec); });

                    System.Threading.Thread.Sleep(200);
                }
            }
            finally
            {

            }
        }

        private void _title(string text)
        {
            if(InvokeRequired)
            {
                Invoke(new Action<string>(_title), text);
                return;
            }

            toolStripStatusLabel1.Text = text;
        }
        private void _t2(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(_t2), text);
                return;
            }

            toolStripStatusLabel2.Text = text;
        }

        void axis(int cnt)
        {
            _title("axis");

            //diSet(1, true, 0);

            waitDo(2, true, msec, () => { diSet(2, true, msec); });

            //diSet(1, false, 0);
            //diSet(2, false, msec);

            for (int i = 0; i < cnt; ++i)
            {
                capture(msec);

                System.Threading.Thread.Sleep(100);
            }

            waitDo(4, true, msec, () => { diSet(4, true, msec); });

            waitDo(4, false, msec, () => { diSet(4, false, msec); });

            waitDo(2, false, msec, () => { diSet(2, false, msec); });

            System.Threading.Thread.Sleep(msec);
        }

        void pos()
        {
            _title("pos");

            //diSet(5, true, 0);
            waitDo(2, true, msec, () => { diSet(2, true, msec); });

            //diSet(5, false, 0);
            //diSet(2, false, msec);

            capture(msec);

            waitDo(4, true, msec, () => { diSet(4, true, msec); });

            waitDo(4, false, msec, () => { diSet(4, false, msec); });

            waitDo(2, false, msec, () => { diSet(2, false, msec); });

            System.Threading.Thread.Sleep(msec);
        }

        void measure()
        {
            _title("measure");

            waitDo(2, true, msec, () => { diSet(2, true, msec); });

            //diSet(2, false, msec);

            capture(msec);

            waitDo(2, false, msec, () => { diSet(2, false, msec); });

            System.Threading.Thread.Sleep(msec);

        }

        void capture(int waitmsec)
        {
            waitDo(3, true, waitmsec, () => { diSet(3, true, waitmsec); });

            waitDo(3, false, waitmsec, () => { diSet(3, false, waitmsec); });
        }

        void diSet(int no, bool onoff, int waitmsec)
        {
            this.DiModels[no].Checked = onoff;

            System.Threading.Thread.Sleep(waitmsec);
        }

        void waitDo(int no, bool onoff, int waitmsec)
        {
            _t2(string.Format("wait:{0} {1}", no, onoff ? "ON" : "OFF"));

            while (this.DoModels[no].Checked != onoff)
            {
                System.Threading.Thread.Sleep(50);

                Application.DoEvents();
            }

            System.Threading.Thread.Sleep(waitmsec);
        }

        void waitDo(int no, bool onoff, int waitmsec, Action action)
        {
            _t2(string.Format("wait:{0} {1}", no, onoff ? "ON" : "OFF"));

            do
            {
                if (null != action) action();

                System.Threading.Thread.Sleep(10);

                //Application.DoEvents();
            } while (this.DoModels[no].Checked != onoff);

            System.Threading.Thread.Sleep(waitmsec);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                KeyDown_ForDebug(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        [System.Diagnostics.Conditional("DEBUG")]
        private void KeyDown_ForDebug(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S)
            {
                statusStrip1.Visible = true;

                if (null == Timer) InitTimer();
                loop = true;
                this.Timer.Change(0, System.Threading.Timeout.Infinite);
            }
            else if (e.KeyCode == Keys.E)
            {
                loop = false;
                this.Timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            }
        }

        #endregion

    }
}
