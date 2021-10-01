using System;

namespace Itc.Common.Controls
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public ValueChangedEventArgs(decimal value, int index)
        {
            Value = value;
            Index = index;
        }

        /// <summary>
        /// 変更後の値
        /// </summary>
        public decimal Value { get; }

        /// <summary>
        /// 0:High, 1:Low
        /// </summary>
        public int Index { get; }
    }
}
