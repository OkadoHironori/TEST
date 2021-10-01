using System.ComponentModel;
using System.Threading;
using DioLib;
using Itc.Common.Extensions;

namespace Xs.DioMonitor
{
    public abstract class DioModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public SynchronizationContext Context { get; set; }

        #endregion

        /// <summary>
        /// 表示名称
        /// </summary>
        public string Text { get; private set; }
        
        
        protected bool _Checked;
        /// <summary>
        /// On/Off状態
        /// </summary>
        public bool Checked
        {
            get { return _Checked; }
            set
            {
                if (value != _Checked)
                {
                    _Checked = value;

                    this.PropertyChanged.Notice(this, Context);
                }
            }
        }
        
        protected uint _Index = uint.MaxValue;
        /// <summary>
        /// 番号
        /// </summary>
        public uint Index
        {
            get { return _Index; }
            set
            {
                if (value != _Index)
                {
                    _Index = value;

                    Text = _Index.ToString("D2");

                    this.PropertyChanged.Notice(this, Context);
                }
            }
        }

        protected bool _Controlable;
        /// <summary>
        /// 操作可否
        /// </summary>
        public bool Controlable
        {
            get { return _Controlable; }
            set
            {
                if (value != _Controlable)
                {
                    _Controlable = value;

                    this.PropertyChanged.Notice(this, Context);
                }
            }
        }

        public bool ReadOnly { get; set; }

        public Dio Dio { get; set; }

        public IDio IDio { get; set; }

        public abstract void Execute(bool onoff);
   }
}
