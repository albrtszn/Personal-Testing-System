﻿#pragma checksum "..\..\..\pages\PageTestOne.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "2CD9DB83BCFC1E65AEBF2E31BD5C9895E4DFE05FA7552E7B4F36165ED79BD424"
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
    /// PageTestOne
    /// </summary>
    public partial class PageTestOne : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 96 "..\..\..\pages\PageTestOne.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TB_Name;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\pages\PageTestOne.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button exportPdf_BT;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\..\pages\PageTestOne.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button exportDoc_BT;
        
        #line default
        #line hidden
        
        
        #line 114 "..\..\..\pages\PageTestOne.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TB_Discrib;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\..\pages\PageTestOne.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TB_Comp;
        
        #line default
        #line hidden
        
        
        #line 118 "..\..\..\pages\PageTestOne.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox TestLB;
        
        #line default
        #line hidden
        
        
        #line 213 "..\..\..\pages\PageTestOne.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BT_ADD_Q;
        
        #line default
        #line hidden
        
        
        #line 227 "..\..\..\pages\PageTestOne.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border LoadProgress;
        
        #line default
        #line hidden
        
        
        #line 241 "..\..\..\pages\PageTestOne.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border BRquestion;
        
        #line default
        #line hidden
        
        
        #line 260 "..\..\..\pages\PageTestOne.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock TBMessage;
        
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
            System.Uri resourceLocater = new System.Uri("/Client;component/pages/pagetestone.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\pages\PageTestOne.xaml"
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
            this.TB_Name = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.exportPdf_BT = ((System.Windows.Controls.Button)(target));
            
            #line 101 "..\..\..\pages\PageTestOne.xaml"
            this.exportPdf_BT.Click += new System.Windows.RoutedEventHandler(this.Button_export_pdf);
            
            #line default
            #line hidden
            return;
            case 3:
            this.exportDoc_BT = ((System.Windows.Controls.Button)(target));
            
            #line 108 "..\..\..\pages\PageTestOne.xaml"
            this.exportDoc_BT.Click += new System.Windows.RoutedEventHandler(this.Button_export_doc);
            
            #line default
            #line hidden
            return;
            case 4:
            this.TB_Discrib = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.TB_Comp = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.TestLB = ((System.Windows.Controls.ListBox)(target));
            return;
            case 10:
            this.BT_ADD_Q = ((System.Windows.Controls.Button)(target));
            
            #line 213 "..\..\..\pages\PageTestOne.xaml"
            this.BT_ADD_Q.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 220 "..\..\..\pages\PageTestOne.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Back);
            
            #line default
            #line hidden
            return;
            case 12:
            this.LoadProgress = ((System.Windows.Controls.Border)(target));
            return;
            case 13:
            this.BRquestion = ((System.Windows.Controls.Border)(target));
            return;
            case 14:
            
            #line 253 "..\..\..\pages\PageTestOne.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonClose);
            
            #line default
            #line hidden
            return;
            case 15:
            this.TBMessage = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 16:
            
            #line 269 "..\..\..\pages\PageTestOne.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_delete);
            
            #line default
            #line hidden
            return;
            case 17:
            
            #line 274 "..\..\..\pages\PageTestOne.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonClose);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 7:
            
            #line 152 "..\..\..\pages\PageTestOne.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Edit);
            
            #line default
            #line hidden
            break;
            case 8:
            
            #line 159 "..\..\..\pages\PageTestOne.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Delet);
            
            #line default
            #line hidden
            break;
            case 9:
            
            #line 162 "..\..\..\pages\PageTestOne.xaml"
            ((System.Windows.Controls.Image)(target)).ContextMenuClosing += new System.Windows.Controls.ContextMenuEventHandler(this.Button_Delet);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}
