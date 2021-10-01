using PLCController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MechaControl
{
    /// <summary>
    /// FDD制御クラス
    /// </summary>
    public class FCD_Ctrl : IFCD_Ctrl, IDisposable, INotifyPropertyChanged
    {

        /// <summary>
        /// 変更通知
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// 状態変更インベント
        /// </summary>
        public event EventHandler StsChanged;
        /// <summary>
        /// 現在状態の要求インベント
        /// </summary>
        public event EventHandler RequestEvent;
        /// <summary>
        /// FCD
        /// </summary>
        private float _StsFCD;
        /// <summary>
        /// FCD
        /// </summary>
        public float StsFCD
        {
            get => _StsFCD;
            set
            {
                if (_StsFCD == value)
                    return;
                _StsFCD = value;
                RaisePropertyChanged(nameof(StsFCD));
            }
        }
        /// <summary>
        /// 運転速度
        /// </summary>
        private float _stsSpeed;
        ///運転速度
        /// </summary>
        public float StsSpeed
        {
            get => _stsSpeed;
            set
            {
                if (_stsSpeed == value)
                    return;
                _stsSpeed = value;
                RaisePropertyChanged(nameof(StsSpeed));
            }
        }
        /// <summary>
        /// 回転大ﾃｰﾌﾞﾙ有無    '追加 by 稲葉 14-02-26    //2014/10/06_v1951反映
        /// </summary>
        private bool _StsRotLargeTable;
        /// <summary>
        ///  回転大ﾃｰﾌﾞﾙ有無    '追加 by 稲葉 14-02-26    //2014/10/06_v1951反映
        /// </summary>
        public bool StsRotLargeTable
        {
            get => _StsRotLargeTable;
            set
            {
                if (_StsRotLargeTable == value)
                    return;
                _StsRotLargeTable = value;
                RaisePropertyChanged(nameof(StsRotLargeTable));
            }
        }
        /// <summary>
        /// Fcd軸のオーバーヒート
        /// </summary>
        private bool _StsFcdDriverHeat;
        /// <summary>
        /// Fcd軸のオーバーヒート
        /// </summary>
        public bool StsFcdDriverHeat
        {
            get => _StsFcdDriverHeat;
            set
            {
                if (_StsFcdDriverHeat == value)
                    return;
                _StsFcdDriverHeat = value;
                RaisePropertyChanged(nameof(StsFcdDriverHeat));
            }
        }
        /// <summary>
        /// FcdのHome位置
        /// </summary>
        private float _FCDHome;
        /// Fcd軸のHome位置
        /// </summary>
        public float FCDHome
        {
            get => _FCDHome;
            set
            {
                if (_FCDHome == value)
                    return;
                _FCDHome = value;
                RaisePropertyChanged(nameof(FCDHome));
            }
        }
        /// <summary>
        /// Fcdの原点位置
        /// </summary>
        private float _FCDOrigin;
        /// Fcdの原点位置
        /// </summary>
        public float FCDOrigin
        {
            get => _FCDOrigin;
            set
            {
                if (_FCDOrigin == value)
                    return;
                _FCDOrigin = value;
                RaisePropertyChanged(nameof(FCDOrigin));
            }
        }
        /// <summary>
        /// 浜松Fcd1の位置
        /// </summary>
        private float _FCD_Hama1;
        /// <summary>
        /// 浜松Fcd1の位置
        /// </summary>
        public float FCD_Hama1
        {
            get => _FCD_Hama1;
            set
            {
                if (_FCD_Hama1 == value)
                    return;
                _FCD_Hama1 = value;
                RaisePropertyChanged(nameof(FCD_Hama1));
            }
        }
        /// <summary>
        /// 浜松Fcd2の位置
        /// </summary>
        private float _FCD_Hama2;
        /// <summary>
        /// 浜松Fcd2の位置
        /// </summary>
        public float FCD_Hama2
        {
            get => _FCD_Hama2;
            set
            {
                if (_FCD_Hama2 == value)
                    return;
                _FCD_Hama2 = value;
                RaisePropertyChanged(nameof(FCD_Hama2));
            }
        }
        /// <summary>        
        /// 前進限
        /// </summary>
        public bool _stsFLimit;
        /// <summary>        
        /// 前進限
        /// </summary>
        public bool StsFLimit
        {
            get => _stsFLimit;
            set
            {
                if (_stsFLimit == value)
                    return;
                _stsFLimit = value;
                RaisePropertyChanged(nameof(StsFLimit));
            }
        }
        /// <summary>        
        ///  後退限
        /// </summary>
        public bool _stsBLimit;
        /// <summary>        
        ///  後退限
        /// </summary>
        public bool StsBLimit
        {
            get => _stsBLimit;
            set
            {
                if (_stsBLimit == value)
                    return;
                _stsBLimit = value;
                RaisePropertyChanged(nameof(StsBLimit));
            }
        }
        /// <summary>        
        /// 前進時に接触
        /// </summary>
        public bool _stsFTouch;
        /// <summary>        
        /// 前進時に接触
        /// </summary>
        public bool StsFTouch
        {
            get => _stsFTouch;
            set
            {
                if (_stsFTouch == value)
                    return;
                _stsFTouch = value;
                RaisePropertyChanged(nameof(StsFTouch));
            }
        }
        /// <summary>        
        ///  後退時に接触
        /// </summary>
        public bool _stsBTouch;
        /// <summary>        
        ///  後退時に接触
        /// </summary>
        public bool StsBTouch
        {
            get => _stsBTouch;
            set
            {
                if (_stsBTouch == value)
                    return;
                _stsBTouch = value;
                RaisePropertyChanged(nameof(StsBTouch));
            }
        }
        /// <summary>
        /// 接触
        /// </summary>
        public bool IsTouch => StsBTouch | StsFTouch;
        /// <summary>
        /// 最高速度
        /// </summary>
        public float StsMaxSpeed { get; private set; }
        public bool IsMaxSpeedFix = false;

        /// <summary>
        /// 最低速度
        /// </summary>
        public float StsMinSpeed { get; private set; }
        public bool IsMinSpeedFix = false;
        /// <summary>
        /// ｲﾝﾃﾞｯｸｽ減速設定状態  '追加 by 稲葉 10-10-19 何に使ってるの？
        /// </summary>
        public bool StsIndexSlow { get; private set; }
        /// <summary>        
        /// 原点位置
        /// </summary>
        public bool _stsOrigin;
        /// <summary>        
        /// 原点位置
        /// </summary>
        public bool StsOrigin
        {
            get => _stsOrigin;
            set
            {
                if (_stsOrigin == value)
                    return;
                _stsOrigin = value;
                RaisePropertyChanged(nameof(StsOrigin));
            }
        }
        /// <summary>        
        /// ビジー
        /// </summary>
        public bool _stsBusy;
        /// <summary>        
        /// ビジー
        /// </summary>
        public bool StsBusy
        {
            get => _stsBusy;
            set
            {
                if (_stsBusy == value)
                    return;
                _stsBusy = value;
                RaisePropertyChanged(nameof(StsBusy));
            }
        }

        /// <summary>
        /// テーブルがX線源と干渉する限界FCD ※自動校正用
        /// </summary>
        public float FCD_Limit { get; private set; }
        /// <summary>
        /// FCD最小値
        /// </summary>
        public float Min_fcd { get; private set; }
        /// <summary>
        /// FCD最大値
        /// </summary>
        public float Max_fcd { get; private set; }
        /// <summary>
        /// 選択しているスピード
        /// </summary>
        public SelectSPD CurrentSpd { get; private set; }
        /// <summary>
        /// 回転大テーブル(リング幅)
        /// </summary>
        public float LargeTableRingWidth { get; private set; }
        /// <summary>
        /// キャンセルトークンソース
        /// </summary>
        public CancellationTokenSource Cantok { get; private set; } = new CancellationTokenSource();
        /// <summary>
        /// PLCコントロール
        /// </summary>
        private readonly IPLCControl _PLCCtrl;
        /// <summary>
        /// FCD固定値
        /// </summary>
        private readonly IFCD_Fix _FCDFix;
        /// <summary>
        /// FDD制御クラス
        /// </summary>
        public FCD_Ctrl(IPLCControl plcctrl, IFCD_Fix fcdfix)
        {
            _PLCCtrl = plcctrl;
            _PLCCtrl.PLCChanged += (s, e) => 
            {
                PLCControl pctrl = s as PLCControl;
                switch (pctrl.ElementName)
                {
                    case (nameof(StsFCD)):
                        StsFCD = pctrl.FloatStatus;
                        break;
                    case ("stsRotLargeTable"):

                        StsRotLargeTable = (s as PLCmodel).BoolStatus;

                        break;

                    case ("FCDOrigin"):

                        FCDOrigin = (s as PLCmodel).FloatStatus;

                        break;

                    case ("FCDHome"):

                        FCDHome = (s as PLCmodel).FloatStatus;

                        break;

                    case ("stsFcdSpeed"):

                        StsSpeed = (s as PLCmodel).FloatStatus;

                        break;

                    case ("stsFcdMaxSpeed"):

                        if (!IsMaxSpeedFix)
                        {
                            StsMaxSpeed = (s as PLCmodel).FloatStatus;
                        }

                        IsMaxSpeedFix = true;
                        //SpdChanged?.Invoke(this, new EventArgs());

                        break;

                    case ("stsFcdMinSpeed"):

                        if (!IsMinSpeedFix)
                        {
                            StsMinSpeed = (s as PLCmodel).FloatStatus;
                        }

                        IsMinSpeedFix = true;
                        //SpdChanged?.Invoke(this, new EventArgs());

                        break;

                    case ("stsFcdIndexSlow"):

                        StsIndexSlow = (s as PLCmodel).BoolStatus;

                        break;

                    case ("stsFcdFLimit"):

                        StsFLimit = (s as PLCmodel).BoolStatus;

                        break;

                    case ("stsFcdBLimit"):

                        StsBLimit = (s as PLCmodel).BoolStatus;

                        break;

                    case ("stsFcdFTouch"):

                        StsFTouch = (s as PLCmodel).BoolStatus;

                        break;

                    case ("stsFcdBTouch"):

                        StsBTouch = (s as PLCmodel).BoolStatus;

                        break;

                    case ("stsFcdDriverHeat"):

                        StsFcdDriverHeat = (s as PLCmodel).BoolStatus;

                        break;

                    case ("FCD_Coli1"):

                        FCD_Hama1 = (s as PLCmodel).FloatStatus;

                        break;

                    case ("FCD_Coli2"):

                        FCD_Hama2 = (s as PLCmodel).FloatStatus;

                        break;
                    case ("stsFcdOrigin"):

                        StsOrigin = (s as PLCmodel).BoolStatus;

                        break;

                    case ("stsFcdBusy"):

                        StsBusy = (s as PLCmodel).BoolStatus;

                        break;

                }

            };


            _FCDFix = fcdfix;
            _FCDFix.EndLoadFixParam += (s, e) =>
            {
                FCD_Fix fixp = s as FCD_Fix;
                FCD_Limit = fixp.FCD_Limit;
                Min_fcd = fixp.Min_fcd;
                Max_fcd = fixp.Max_fcd;
                LargeTableRingWidth = fixp.LargeTableRingWidth;
                CurrentSpd = fixp.CurrentSpd;
            };
        }
        /// <summary>
        /// 原点復帰指示
        /// </summary>
        public void Origin()
        {
            Cantok = new CancellationTokenSource();

            _PLCCtrl.SendMessage("PanelInhibit", true);//タッチパネル操作禁止

            _PLCCtrl.SendMessage("FcdOrigin", true);//実行コマンド

            Task.WaitAll(Task.Delay(10000));

            if (!WaitOrginMethod(TimeSpan.FromSeconds(70), Cantok.Token))//70秒待っても完了しなかったらエラーとしたい。
            {
                Stop();
            }

            _PLCCtrl.SendMessage("PanelInhibit", false);//タッチパネル操作禁止解除

            StsChanged?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// インデックス操作
        /// </summary>
        public void Index(float  IdxValue)
        {
            Cantok = new CancellationTokenSource();

            _PLCCtrl.SendMessage("PanelInhibit", true);//タッチパネル操作禁止

            _PLCCtrl.SendMessage("FcdIndexPosition", IdxValue);//実行コマンド

            Task.WaitAll(Task.Delay(500));

            _PLCCtrl.SendMessage("FcdIndex", true);

            WaitOrginMethod(TimeSpan.FromSeconds(70), Cantok.Token);

            _PLCCtrl.SendMessage("PanelInhibit", false);

            StsChanged?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// 停止 兎に角とめるスタンス
        /// </summary>
        /// <param name="errormes"></param>
        /// <returns></returns>
        public void Stop()
        {
            _PLCCtrl.SendMessage("FcdBackward", false);

            _PLCCtrl.SendMessage("FcdForward", false);

            _PLCCtrl.SendMessage("FcdIndexStop", true);

            _PLCCtrl.SendMessage("PanelInhibit", false);

            return;
        }

        /// <summary>
        /// リセット動作の待ちメソッド
        /// </summary>
        /// <param name="mesbox">message</param>
        /// <param name="timeout">timeout</param>
        /// <param name="prog">進捗</param>
        /// <param name="ctoken">キャンセル</param>
        /// <returns></returns>
        private bool WaitOrginMethod(TimeSpan timeout, CancellationToken ctoken)
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

                    if (StsOrigin)
                    {
                        break;
                    }

                    if (ctoken.IsCancellationRequested)
                    {
                        break;
                    }
                    Task.WaitAll(Task.Delay(100));//100msec
                }
            });

            try
            {
                t.Wait(source.Token);
            }
            catch (OperationCanceledException)
            {   //タイムアウト処理
                //mesbox?.Invoke($"{MechaRes.TIMEOUT_ERROR} {timeout.TotalSeconds.ToString()}{Environment.NewLine}{MechaRes.CONFIRM_OPERATION}");
                return false;
            }
            catch (AggregateException)
            {   //例外処理
                //mesbox?.Invoke($"{MechaRes.EXCEPTION_ERROR}{Environment.NewLine}{MechaRes.CONFIRM_OPERATION}");
                return false;
            }

            if (ctoken.IsCancellationRequested)
            {
                //prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_STOP });
                return false;
            }
            //prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_COMPLEAT });
            return true;
        }
        /// <summary>
        /// パラメータ要求
        /// </summary>
        public void Request() 
            => RequestEvent?.Invoke(this, new EventArgs());
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
    public interface IFCD_Ctrl
    {
        /// <summary>
        /// 変更通知
        /// </summary>
        event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 状態変更インベント
        /// </summary>
        event EventHandler StsChanged;
        /// <summary>
        /// 現在状態の要求インベント
        /// </summary>
        event EventHandler RequestEvent;
        /// <summary>
        /// 原点復帰
        /// </summary>
        void Origin();
        /// <summary>
        /// インデックス操作
        /// </summary>
        void Index(float IdxValue);
        /// <summary>
        /// パラメータ要求
        /// </summary>
        void Request();
        /// <summary>
        /// 停止要求
        /// </summary>
        void Stop();

    }
}
