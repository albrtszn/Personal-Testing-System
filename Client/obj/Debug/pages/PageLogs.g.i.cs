﻿#pragma checksum "..\..\..\pages\PageLogs.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "3BDB4FE27FF485328077ACF59186A6803115BD0FD3FDE7599760FDC6289BDEB6"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Client.pages;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using ScottPlot;
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


namespace Client.pages {
    
    
    /// <summary>
    /// PageLogs
    /// </summary>
    public partial class PageLogs : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 23 "..\..\..\pages\PageLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BT_NumBegin;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\pages\PageLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BT_NumPrev;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\pages\PageLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BT_Curr;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\pages\PageLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BT_NumNext;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\pages\PageLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BT_NumEnd;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\pages\PageLogs.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid DG_Logs;
        
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
            System.Uri resourceLocater = new System.Uri("/Client;component/pages/pagelogs.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\pages\PageLogs.xaml"
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
            this.BT_NumBegin = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\..\pages\PageLogs.xaml"
            this.BT_NumBegin.Click += new System.Windows.RoutedEventHandler(this.NumBegin);
            
            #line default
            #line hidden
            return;
            case 2:
            this.BT_NumPrev = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\pages\PageLogs.xaml"
            this.BT_NumPrev.Click += new System.Windows.RoutedEventHandler(this.NumPrev);
            
            #line default
            #line hidden
            return;
            case 3:
            this.BT_Curr = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\..\pages\PageLogs.xaml"
            this.BT_Curr.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.BT_NumNext = ((System.Windows.Controls.Button)(target));
            
            #line 31 "..\..\..\pages\PageLogs.xaml"
            this.BT_NumNext.Click += new System.Windows.RoutedEventHandler(this.NumNext);
            
            #line default
            #line hidden
            return;
            case 5:
            this.BT_NumEnd = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\pages\PageLogs.xaml"
            this.BT_NumEnd.Click += new System.Windows.RoutedEventHandler(this.NumEnd);
            
            #line default
            #line hidden
            return;
            case 6:
            this.DG_Logs = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

