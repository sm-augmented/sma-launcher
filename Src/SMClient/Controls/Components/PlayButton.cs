// Decompiled with JetBrains decompiler
// Type: SMClient.Controls.PlayButton
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
using System.Windows.Media;

namespace SMClient.Controls
{
  public partial class PlayButton : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty ButtonLabelProperty = DependencyProperty.Register(nameof (ButtonLabel), typeof (string), typeof (PlayButton));

    public EventHandler Click { get; set; }

    public string ButtonLabel
    {
      get => (string) this.GetValue(PlayButton.ButtonLabelProperty);
      set => this.SetValue(PlayButton.ButtonLabelProperty, (object) value);
    }

    public PlayButton()
    {
      this.InitializeComponent();
      this.IsEnabledChanged += new DependencyPropertyChangedEventHandler(this.PlayButton_IsEnabledChanged);
    }

    private void PlayButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      this.disabledStub.Visibility = this.IsEnabled ? Visibility.Collapsed : Visibility.Visible;
      this.enabled.Visibility = this.IsEnabled ? Visibility.Visible : Visibility.Collapsed;
    }

    private void ProgressBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
    }

    private void Label_MouseEnter(object sender, MouseEventArgs e)
    {
      if (!this.IsEnabled)
        return;
      ((Control) sender).Foreground = (Brush) new SolidColorBrush(Color.FromRgb(byte.MaxValue, (byte) 185, (byte) 0));
    }

    private void Label_MouseLeave(object sender, MouseEventArgs e) => this.label.Foreground = (Brush) new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));

    private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (this.IsEnabled)
      {
        EventHandler click = this.Click;
        if (click != null)
          click(sender, (EventArgs) e);
        this.label.Foreground = (Brush) new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
      }
      e.Handled = true;
    }
  }
}
