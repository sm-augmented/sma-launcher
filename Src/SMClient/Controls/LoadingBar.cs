// Decompiled with JetBrains decompiler
// Type: SMClient.Controls.LoadingBar
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SMClient.Controls
{
  public class LoadingBar : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty ProgressLabelProperty = DependencyProperty.Register(nameof (ProgressLabel), typeof (string), typeof (LoadingBar), new PropertyMetadata((object) ""));
    public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof (Progress), typeof (int), typeof (LoadingBar), new PropertyMetadata((object) 0));
    internal LoadingBar root;
    private bool _contentLoaded;

    public string ProgressLabel
    {
      get => (string) this.GetValue(LoadingBar.ProgressLabelProperty);
      set => this.SetValue(LoadingBar.ProgressLabelProperty, (object) value);
    }

    public int Progress
    {
      get => (int) this.GetValue(LoadingBar.ProgressProperty);
      set => this.SetValue(LoadingBar.ProgressProperty, (object) value);
    }

    public LoadingBar() => this.InitializeComponent();

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/SMClient;component/controls/components/loadingbar.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.root = (LoadingBar) target;
      else
        this._contentLoaded = true;
    }
  }
}
