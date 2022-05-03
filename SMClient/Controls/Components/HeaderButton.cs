// Decompiled with JetBrains decompiler
// Type: SMClient.Controls.Components.HeaderButton
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
using System.Windows.Media.Imaging;

namespace SMClient.Controls.Components
{
  public class HeaderButton : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(nameof (ImageSource), typeof (string), typeof (HeaderButton), new PropertyMetadata((PropertyChangedCallback) ((o, e) => ((HeaderButton) o).Image = (System.Windows.Media.ImageSource) new BitmapImage(new Uri("pack://application:,,," + (string) e.NewValue)))));
    public static readonly DependencyProperty ImageSourceHoverProperty = DependencyProperty.Register(nameof (ImageSourceHover), typeof (string), typeof (HeaderButton), new PropertyMetadata((PropertyChangedCallback) ((o, e) => ((HeaderButton) o).ImageHover = (System.Windows.Media.ImageSource) new BitmapImage(new Uri("pack://application:,,," + (string) e.NewValue)))));
    public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof (Label), typeof (string), typeof (HeaderButton));
    public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof (IsSelected), typeof (bool), typeof (HeaderButton));
    internal HeaderButton root;
    internal System.Windows.Controls.Image btnIcon;
    internal System.Windows.Controls.Label btnLabel;
    private bool _contentLoaded;

    protected System.Windows.Media.ImageSource Image { get; set; }

    public string ImageSource
    {
      get => (string) this.GetValue(HeaderButton.ImageSourceProperty);
      set => this.SetValue(HeaderButton.ImageSourceProperty, (object) value);
    }

    protected System.Windows.Media.ImageSource ImageHover { get; set; }

    public string ImageSourceHover
    {
      get => (string) this.GetValue(HeaderButton.ImageSourceHoverProperty);
      set => this.SetValue(HeaderButton.ImageSourceHoverProperty, (object) value);
    }

    public string Label
    {
      get => (string) this.GetValue(HeaderButton.LabelProperty);
      set => this.SetValue(HeaderButton.LabelProperty, (object) value);
    }

    public bool IsSelected
    {
      get => (bool) this.GetValue(HeaderButton.IsSelectedProperty);
      set
      {
        this.SetValue(HeaderButton.IsSelectedProperty, (object) value);
        if (this.IsSelected)
        {
          this.btnIcon.Source = this.ImageHover;
          this.btnLabel.Foreground = (Brush) new SolidColorBrush(Color.FromRgb((byte) 225, (byte) 165, (byte) 26));
        }
        else
        {
          this.btnIcon.Source = this.Image;
          this.btnLabel.Foreground = (Brush) new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
        }
      }
    }

    public HeaderButton() => this.InitializeComponent();

    private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
    {
      if (!this.IsSelected)
        this.btnIcon.Source = this.ImageHover;
      this.btnLabel.Foreground = (Brush) new SolidColorBrush(Color.FromRgb((byte) 225, (byte) 165, (byte) 26));
    }

    private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
    {
      if (this.IsSelected)
        return;
      this.btnIcon.Source = this.Image;
      this.btnLabel.Foreground = (Brush) new SolidColorBrush(Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/SMClient;component/controls/components/headerbutton.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.root = (HeaderButton) target;
          break;
        case 2:
          ((UIElement) target).MouseEnter += new MouseEventHandler(this.StackPanel_MouseEnter);
          ((UIElement) target).MouseLeave += new MouseEventHandler(this.StackPanel_MouseLeave);
          break;
        case 3:
          this.btnIcon = (System.Windows.Controls.Image) target;
          break;
        case 4:
          this.btnLabel = (System.Windows.Controls.Label) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
