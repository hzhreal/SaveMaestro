﻿#pragma checksum "..\..\customcrypto.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "E3B0C44298FC1C149AFBF4C8996FB92427AE41E4649B934CA495991B7852B855"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Resign;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Resign {
    
    
    /// <summary>
    /// ResignWindow
    /// </summary>
    public partial class ResignWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 2 "..\..\customcrypto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Resign.ResignWindow customcrypto;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\customcrypto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox filepaths;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\customcrypto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button selectfiles;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\customcrypto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button begin_resign;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\customcrypto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock terminal_resign;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\customcrypto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox accountid_resign;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\customcrypto.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button clear_paths;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SaveMaestro;component/customcrypto.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\customcrypto.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.customcrypto = ((Resign.ResignWindow)(target));
            return;
            case 2:
            this.filepaths = ((System.Windows.Controls.ListBox)(target));
            return;
            case 3:
            this.selectfiles = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\customcrypto.xaml"
            this.selectfiles.Click += new System.Windows.RoutedEventHandler(this.selectfiles_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.begin_resign = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\customcrypto.xaml"
            this.begin_resign.Click += new System.Windows.RoutedEventHandler(this.begin_resign_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.terminal_resign = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.accountid_resign = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.clear_paths = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\customcrypto.xaml"
            this.clear_paths.Click += new System.Windows.RoutedEventHandler(this.clear_paths_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

