﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Stazis {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.3.0.0")]
    public sealed partial class AppSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static AppSettings defaultInstance = ((AppSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new AppSettings())));
        
        public static AppSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AutoResizeRowHeight {
            get {
                return ((bool)(this["AutoResizeRowHeight"]));
            }
            set {
                this["AutoResizeRowHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AutoResizeColumnHeaderHeight {
            get {
                return ((bool)(this["AutoResizeColumnHeaderHeight"]));
            }
            set {
                this["AutoResizeColumnHeaderHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AutoResizeColumnHeaderWidth {
            get {
                return ((bool)(this["AutoResizeColumnHeaderWidth"]));
            }
            set {
                this["AutoResizeColumnHeaderWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AutoResizeColumnWidth {
            get {
                return ((bool)(this["AutoResizeColumnWidth"]));
            }
            set {
                this["AutoResizeColumnWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SaveSearchResult {
            get {
                return ((bool)(this["SaveSearchResult"]));
            }
            set {
                this["SaveSearchResult"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>Электронная таблица Excel|*.xls;*.xlsx|</string>
  <string>Файл CSV|*.csv|</string>
  <string>База данных SQLite|*.sqlite3;*.db;*.cdb</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection SupportedImportTypes {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["SupportedImportTypes"]));
            }
            set {
                this["SupportedImportTypes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection RecentList {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["RecentList"]));
            }
            set {
                this["RecentList"] = value;
            }
        }
    }
}
