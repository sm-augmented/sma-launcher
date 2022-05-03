// Decompiled with JetBrains decompiler
// Type: SMClient.Controls.Components.SMCheckbox
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace SMClient.Controls.Components
{
  public class SMCheckbox : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register(nameof (IsChecked), typeof (bool), typeof (SMCheckbox));
    internal Image disabledHighlight;
    internal Image disabled;
    internal Image enabled;
    private bool _contentLoaded;

    public bool IsChecked
    {
      get => (bool) this.GetValue(SMCheckbox.IsCheckedProperty);
      set
      {
        this.SetValue(SMCheckbox.IsCheckedProperty, (object) value);
        this.SetButtons();
      }
    }

    public event EventHandler Checked;

    public SMCheckbox() => this.InitializeComponent();

    private void SetButtons()
    {
      if (this.IsChecked)
      {
        this.disabled.Visibility = Visibility.Collapsed;
        this.enabled.Visibility = Visibility.Visible;
      }
      else
      {
        this.disabled.Visibility = Visibility.Visible;
        this.enabled.Visibility = Visibility.Collapsed;
      }
    }

    private void Disabled_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.IsChecked = false;
      EventHandler eventHandler = this.Checked;
      if (eventHandler != null)
        eventHandler((object) this.IsChecked, (EventArgs) null);
      e.Handled = true;
    }

    public void Enabled_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.IsChecked = true;
      EventHandler eventHandler = this.Checked;
      if (eventHandler != null)
        eventHandler((object) this.IsChecked, (EventArgs) null);
      if (e == null)
        return;
      e.Handled = true;
    }

    private void disabled_MouseEnter(object sender, MouseEventArgs e) => this.disabledHighlight.Visibility = Visibility.Visible;

    private void disabled_MouseLeave(object sender, MouseEventArgs e) => this.disabledHighlight.Visibility = Visibility.Collapsed;

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/SMClient;component/controls/components/smcheckbox.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.disabledHighlight = (Image) target;
          break;
        case 2:
          this.disabled = (Image) target;
          this.disabled.MouseLeftButtonDown += new MouseButtonEventHandler(this.Enabled_MouseLeftButtonDown);
          this.disabled.MouseEnter += new MouseEventHandler(this.disabled_MouseEnter);
          this.disabled.MouseLeave += new MouseEventHandler(this.disabled_MouseLeave);
          break;
        case 3:
          this.enabled = (Image) target;
          this.enabled.MouseLeftButtonDown += new MouseButtonEventHandler(this.Disabled_MouseLeftButtonDown);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
