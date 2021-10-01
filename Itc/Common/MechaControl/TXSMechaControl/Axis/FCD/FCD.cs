
using Itc.Common.Extensions;
using Itc.Common.TXEnum;
using PLCController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace TXSMechaControl.FCD
{

    /// <summary>
    /// FCD関係
    /// </summary>
    public class FCD : BindableBase, IFCD, IDisposable
    {
        /// <summary>
        /// FCDのSPDが更新された
        /// </summary>
        public event EventHandler SpdChanged;
        /// <summary>
        /// FCD
        /// </summary>
        private float _StsFCD;
        /// <summary>
        /// FCD
        /// </summary>
        public float StsFCD
        {
            get { return this._StsFCD; }
            set { SetProperty(ref _StsFCD, value); }
        }
        /// <summary>
        /// 運転速度
        /// </summary>
        private float _stsSpeed;
        ///運転速度
        /// </summary>
        public float StsSpeed
        {
            get { return this._stsSpeed; }
            set { SetProperty(ref _stsSpeed, value); }
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
            get { return this._StsRotLargeTable; }
            set { SetProperty(ref _StsRotLargeTable, value); }
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
            get { return this._StsFcdDriverHeat; }
            set { SetProperty(ref _StsFcdDriverHeat, value); }
        }
        /// <summary>
        /// FcdのHome位置
        /// </summary>
        private float _FCDHome;
        /// Fcd軸のHome位置
        /// </summary>
        public float FCDHome
        {
            get { return this._FCDHome; }
            set { SetProperty(ref _FCDHome, value); }
        }
        /// <summary>
        /// Fcdの原点位置
        /// </summary>
        private float _FCDOrigin;
        /// Fcdの原点位置
        /// </summary>
        public float FCDOrigin
        {
            get { return this._FCDOrigin; }
            set { SetProperty(ref _FCDOrigin, value); }
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
            get { return this._FCD_Hama1; }
            set { SetProperty(ref _FCD_Hama1, value); }
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
            get { return this._FCD_Hama2; }
            set { SetProperty(ref _FCD_Hama2, value); }
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
            get { return this._stsFLimit; }
            set { SetProperty(ref _stsFLimit, value); }
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
            get { return this._stsBLimit; }
            set { SetProperty(ref _stsBLimit, value); }
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
            get { return this._stsFTouch; }
            set { SetProperty(ref _stsFTouch, value); }
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
            get { return this._stsBTouch; }
            set { SetProperty(ref _stsBTouch, value); }
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
            get { return this._stsOrigin; }
            set { SetProperty(ref _stsOrigin, value); }
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
            get { return this._stsBusy; }
            set { SetProperty(ref _stsBusy, value); }
        }
        /// <summary>
        /// FCD固定パラメータ
        /// </summary>
        public FCD_Param Param { get; }
        /// <summary>
        /// FCDに関する何かしらが変更された
        /// </summary>
        public event EventHandler FCDChanged;
        /// <summary>
        /// FCDの変更通知
        /// </summary>
        public FCD_Provider FcdProvider { get; }
        /// <summary>
        /// PLC制御I/F
        /// </summary>
        private readonly IPLCControl _PLC;
        /// <summary>
        /// FCDコンストラクタ
        /// </summary>
        public FCD(IPLCControl plc)
        {
            _PLC = plc;
            FcdProvider = new FCD_Provider(plc);

            //FCDが変更されたとき
            FcdProvider.ObserveProperty(() => FcdProvider.StsFCD)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });

            //FCD_Homeが変更されたとき
            FcdProvider.ObserveProperty(() => FcdProvider.FCDHome)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });
            //FCD_Originが変更されたとき
            FcdProvider.ObserveProperty(() => FcdProvider.FCDOrigin)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });

            //スピード更新
            FcdProvider.ObserveProperty(() => FcdProvider.StsSpeed)
                    .Subscribe(_ =>
                    {
                        Param.CurrentSpd.SPD = FcdProvider.StsSpeed;
                        FCDChanged?.Invoke(this, new EventArgs());
                    });            

            //リミットに触れたとき
            FcdProvider.ObserveProperty(() => FcdProvider.StsBLimit)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });
            FcdProvider.ObserveProperty(() => FcdProvider.StsFLimit)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });

            //前進時接触センサ若しくは後退時接触センサに触れたとき
            FcdProvider.ObserveProperty(() => FcdProvider.StsFTouch)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });
            FcdProvider.ObserveProperty(() => FcdProvider.StsBTouch)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });

            //FCDのオーバーヒートエラーが発生
            FcdProvider.ObserveProperty(() => FcdProvider.StsFcdDriverHeat)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });

            //ラージテーブル切替
            FcdProvider.ObserveProperty(() => FcdProvider.StsRotLargeTable)
                    .Subscribe(_ =>
                    {
                        FCDChanged?.Invoke(this, new EventArgs());
                    });

            //速度の初期設定が変更されたとき
            FcdProvider.SpdChanged += (s, e) =>
            {
                FCDChanged?.Invoke(this, new EventArgs());
            };


            Param = new FCD_Param();
            if (!Param.SetParam())
            {
                throw new Exception($"{nameof(Param)} is failed");
            }
        }
        /// <summary>
        /// 初期化
        /// </summary>
        public bool Init(Action<string> mes)
        {
            if (_PLC.ConnectPLC(mes))
            {
                _PLC.SendMessage("FCDLimit", Param.FCD_Limit, mes);

                _PLC.SendMessage("FCDFineLimit", Param.FCD_Limit, mes);

                _PLC.SendMessage("LargeTableRingWidth", Param.LargeTableRingWidth, mes);

                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新要求
        /// </summary>
        public void UpdateAll()
        {
            FCDChanged?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// 原点復帰
        /// </summary>
        public bool Origin(Action<string> errormes, IProgress<CalProgress> prog, CancellationToken ctoken)
        {
            _PLC.SendMessage("PanelInhibit", true, errormes);//タッチパネル操作禁止
            _PLC.SendMessage("FcdOrigin", true, errormes);//実行コマンド

            prog.Report(new CalProgress { Percent = 0, Status = MechaRes.RESET_Process });
            
            if (!WaitOrginMethod(errormes, TimeSpan.FromSeconds(70), prog, ctoken))//70秒待っても完了しなかったらエラーとしたい。
            {
                Stop(errormes);
                return false;
            }

            _PLC.SendMessage("PanelInhibit", false, errormes);//タッチパネル操作禁止解除
            return true;
        }
        /// <summary>
        /// リセット動作の待ちメソッド
        /// </summary>
        /// <param name="mesbox">message</param>
        /// <param name="timeout">timeout</param>
        /// <param name="prog">進捗</param>
        /// <param name="ctoken">キャンセル</param>
        /// <returns></returns>
        private bool WaitOrginMethod(Action<string> mesbox, TimeSpan timeout, IProgress<CalProgress> prog, CancellationToken ctoken)
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

                    if (FcdProvider.StsOrigin)
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
                mesbox?.Invoke($"{MechaRes.TIMEOUT_ERROR} {timeout.TotalSeconds.ToString()}{Environment.NewLine}{MechaRes.CONFIRM_OPERATION}");
                prog.Report(new CalProgress { Percent = 0, Status = MechaRes.TIMEOUT_ERROR });
                return false;
            }
            catch (AggregateException)
            {   //例外処理
                mesbox?.Invoke($"{MechaRes.EXCEPTION_ERROR}{Environment.NewLine}{MechaRes.CONFIRM_OPERATION}");
                return false;
            }

            if (ctoken.IsCancellationRequested)
            {
                prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_STOP });
                return false;
            }
            prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_COMPLEAT });
            return true;
        }
        /// <summary>
        /// Index操作の待ち処理
        /// </summary>
        /// <param name="mesbox"></param>
        /// <param name="bnum"></param>
        /// <param name="timeout">タイムアウト時間</param>
        /// <returns></returns>
        private bool WaitIndexMethod(Action<string> mesbox, TimeSpan timeout, IProgress<CalProgress> prog, CancellationToken ctoken, float Object, float Total)
        {
            List<bool> busymoni = new List<bool>();
            var source = new CancellationTokenSource();
            source.CancelAfter(timeout);
            Task t = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    int tmpPercent = (int)Math.Min(Math.Abs(FcdProvider.StsFCD - Object) / (float)Total * 100, 100);

                    prog.Report(new CalProgress { Percent = 0, Status = MechaRes.INDEX_Process + " " + tmpPercent.ToString() + "%" });

                    if (source.IsCancellationRequested)
                    {
                        break;
                    }

                    if (FcdProvider.StsFCD == Object)
                    {
                        break;
                    }

                    if (busymoni.Count > 20 && !busymoni.Last())
                    {
                        break;
                    }
                    busymoni.Add(FcdProvider.StsBusy);

                    if (busymoni.Count > 20 && !busymoni.Last())
                    {
                        break;
                    }
                    busymoni.Add(FcdProvider.StsBusy);

                    if (ctoken.IsCancellationRequested)
                    {
                        break;
                    }

                    Task.WaitAll(Task.Delay(100));//100msec
                }
            }, source.Token);

            try
            {
                t.Wait(source.Token);
            }
            catch (OperationCanceledException)
            {   //タイムアウト処理
                mesbox?.Invoke($"{MechaRes.TIMEOUT_ERROR} {timeout.TotalSeconds.ToString()}{Environment.NewLine}{MechaRes.TIMEOUT_ERROR}");
                prog.Report(new CalProgress { Percent = 0, Status = MechaRes.TIMEOUT_ERROR });
                return false;
            }
            catch (AggregateException)
            {   //例外処理
                mesbox?.Invoke($"{MechaRes.EXCEPTION_ERROR}{Environment.NewLine}{MechaRes.CONFIRM_OPERATION}");
                return false;
            }

            if(ctoken.IsCancellationRequested)
            {
                prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_STOP });
                return false;
            }

            prog.Report(new CalProgress { Percent = 0, Status = MechaRes.PROCESS_COMPLEAT });
            return true;
        }
        /// <summary>
        /// マニュアル動作
        /// </summary>
        public bool Manual(MDirMode mode, Action<string> errormes)
        {
            switch(mode)
            {
                case (MDirMode.Forward):
                    _PLC.SendMessage("FcdForward", true, errormes);
                    break;
                case (MDirMode.Backward):
                    _PLC.SendMessage("FcdBackward", true, errormes);
                    break;
                default:
                    Stop(errormes);//ストップ
                    break;
            }

            return true;
        }
        /// <summary>
        /// 停止 兎に角とめるスタンス
        /// </summary>
        /// <param name="errormes"></param>
        /// <returns></returns>
        public bool Stop(Action<string> errormes)
        {
            _PLC.SendMessage("FcdBackward", false, errormes);

            _PLC.SendMessage("FcdForward", false, errormes);

            _PLC.SendMessage("FcdIndexStop", true, errormes);

            _PLC.SendMessage("PanelInhibit", false, errormes);

            return true;
        }
        /// <summary>
        /// インデックス操作
        /// </summary>
        /// <param name="posi"></param>
        /// <param name="errormes"></param>
        /// <returns></returns>
        public bool Index(float posi, Action<string> errormes, IProgress<CalProgress> prog, CancellationToken ctoken)
        {
            //速度は最速にする
            float current = FcdProvider.StsSpeed;
            float quermax = Param.Speedlist.ToList().Max(p => p.SPD);
            if (UpdateSpeed(quermax, errormes))
            {
                _PLC.SendMessage("FcdIndexPosition", posi, errormes);
                Task.WaitAll(Task.Delay(400));
                _PLC.SendMessage("FcdIndex", true, errormes);

                //総距離
                float length = (float)Math.Abs(FcdProvider.StsFCD - posi);

                if (!WaitIndexMethod(errormes, TimeSpan.FromSeconds(60), prog, ctoken, posi, length))//60秒待っても完了しなかったらエラーとしたい。
                {
                    Stop(errormes);
                    UpdateSpeed(current, errormes);
                    return false;
                }
               
                UpdateSpeed(current, errormes);//速度を戻す

                return true;
            }

            return false;
        }
        /// <summary>
        /// 速度更新
        /// </summary>
        /// <param name="selspddisp"></param>
        /// <param name="mes"></param>
        public void UpdateSpeed(string selspddisp, Action<string> mes)
        {
            var quer = Param.Speedlist.ToList().Find(p => p.DispName == selspddisp);
            if (quer != null)
            {
                _PLC.SendMessage("FCDspeed", quer.SPD, mes);
            }
            else
            {
                throw new Exception($"{nameof(Param.Speedlist)} is failed");
            }
        }
        /// <summary>
        /// 速度更新(直入力　float)
        /// </summary>
        public bool UpdateSpeed(float selspddisp, Action<string> mes)
        {
            if (selspddisp != FcdProvider.StsSpeed)//スピードの設定値が同じなら抜ける
            {
                var quer = Param.Speedlist.ToList().Find(p => p.SPD == selspddisp);
                if (quer != null)
                {
                    _PLC.SendMessage("FCDspeed", quer.SPD, mes);
                }
                else
                {
                    _PLC.SendMessage("FCDspeed", selspddisp, mes);
                }


                var source = new CancellationTokenSource();
                source.CancelAfter(TimeSpan.FromSeconds(4));
                Task t = Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        if (source.IsCancellationRequested)
                        {
                            break;
                        }

                        if (FcdProvider.StsSpeed == selspddisp)
                        {
                            break;
                        }
                        Task.WaitAll(Task.Delay(100));//100msec
                }
                }, source.Token);


                try
                {
                    t.Wait(source.Token);
                }
                catch (OperationCanceledException)
                {   //タイムアウト処理
                    mes?.Invoke($"{MechaRes.TIMEOUT_ERROR} {TimeSpan.FromSeconds(3).ToString()}{Environment.NewLine}{MechaRes.SPEED_SET_ERROR}");
                    return false;
                }
            }

            return true;

        }
        /// <summary>
        /// 原点位置の更新
        /// </summary>
        public void UpdateOrigin(float origin, Action<string> errormes)
        {
            _PLC.SendMessage("FCDOrigin", origin, errormes);
        }
        /// <summary>
        /// ホーム位置の更新
        /// </summary>
        public void UpdateHome(float home, Action<string> errormes)
        {
            _PLC.SendMessage("FCDHome", home, errormes);
        }
        /// <summary>
        /// X線干渉位置 1の更新
        /// </summary>
        public void UpdateHamamatu1(float hama1, Action<string> errormes)
        {
            _PLC.SendMessage("FCD_Coli1", hama1, errormes);
        }
        /// <summary>
        /// X線干渉位置 2の更新
        /// </summary>
        public void UpdateHamamatu2(float hama2, Action<string> errormes)
        {
            _PLC.SendMessage("FCD_Coli2", hama2, errormes);
        }
        /// <summary>
        /// FCD値を要求
        /// </summary>
        /// <returns></returns>
        public float GetFCD() => FcdProvider.StsFCD;
        /// <summary>
        /// FCDの有効桁数の取得
        /// </summary>
        /// <returns></returns>
        public int GetFCDDeip() => _PLC.ConvertScaleToDecip("stsFCD");
        /// <summary>
        /// FCD Home値の取得
        /// </summary>
        /// <returns></returns>
        public float GetFCDHome() => FcdProvider.FCDHome;
        /// <summary>
        /// FCD Homeの有効桁数の取得
        /// </summary>
        /// <returns></returns>
        public int GetFCDHomeDeip() => _PLC.ConvertScaleToDecip("FCDHome");
        /// <summary>
        /// FCD原点位置の取得
        /// </summary>
        /// <returns></returns>
        public float GetFCDOrigin() => FcdProvider.FCDOrigin;
        /// <summary>
        /// FCD 原点位置の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        public int GetFCDOriginDeip() => _PLC.ConvertScaleToDecip("FCDOrigin");
        /// <summary>
        /// FCD 浜松干渉位置１の取得
        /// </summary>
        /// <returns></returns>
        public float GetFCDHama1() => FcdProvider.FCD_Hama1;
        /// <summary>
        /// FCD 浜松干渉位置１の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        public int GetFCDHama1Deip() => _PLC.ConvertScaleToDecip("FCD_Coli1");
        /// <summary>
        /// FCD 浜松干渉位置2の取得
        /// </summary>
        /// <returns></returns>
        public float GetFCDHama2() => FcdProvider.FCD_Hama2;
        /// <summary>
        /// FCD 浜松干渉位置２の有効桁数の取得
        /// </summary>
        /// <returns></returns>
        public int GetFCDHama2Deip() => _PLC.ConvertScaleToDecip("FCD_Coli2");
        /// <summary>
        /// 破棄
        /// </summary>
        public void Dispose()
        {
            FcdProvider.Dispose(); 
        }
    }
}
