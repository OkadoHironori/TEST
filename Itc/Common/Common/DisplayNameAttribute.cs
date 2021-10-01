using System;

namespace Itc.Common
{
    /// <summary>
    /// 表示名属性 -> 「System.ComponentModel.DisplayNameAttribute」がある
    /// </summary>
    [Obsolete("", true)]
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class DisplayNameAttribute : Attribute
    {
        #region - BackingField -

        string _resource;
        private Type _resourceType;

        #endregion

        /// <summary>
        /// 表示名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// リソース名
        /// </summary>
        public string Resource
        {
            get { return _resource; }
            set
            {
                _resource = value;

                SetNameFromResource();
            }
        }

        /// <summary>
        /// リソースの型
        /// </summary>
        public Type ResourceType
        {
            get { return _resourceType; }
            set
            {
                _resourceType = value;

                SetNameFromResource();
            }
        }
        
        //リソース型とリソース名から表示名をセットする
        private void SetNameFromResource()
        {
            if (_resourceType == null || _resource == null)
            {
                return;
            }

            System.Reflection.PropertyInfo propertyInfo = _resourceType.GetProperty(_resource);

            Name = propertyInfo?.GetValue(_resourceType) as string;
        }

        /// <summary>
        /// 表示名属性
        /// </summary>
        public DisplayNameAttribute() { }

        /// <summary>
        ///  表示名属性
        /// </summary>
        /// <param name="name">表示名</param>
        public DisplayNameAttribute(string name)
        {
            this.Name = name;
        }
    }

}
