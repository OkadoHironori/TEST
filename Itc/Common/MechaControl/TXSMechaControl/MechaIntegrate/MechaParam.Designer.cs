﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TXSMechaControl.MechaIntegrate {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0")]
    internal sealed partial class MechaParam : global::System.Configuration.ApplicationSettingsBase {
        
        private static MechaParam defaultInstance = ((MechaParam)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new MechaParam())));
        
        public static MechaParam Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("FDD")]
        public global::Itc.Common.TXEnum.OptMechaAxis Opt {
            get {
                return ((global::Itc.Common.TXEnum.OptMechaAxis)(this["Opt"]));
            }
            set {
                this["Opt"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public float RotationSPD {
            get {
                return ((float)(this["RotationSPD"]));
            }
            set {
                this["RotationSPD"] = value;
            }
        }
    }
}
