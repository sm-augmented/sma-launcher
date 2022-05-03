// Decompiled with JetBrains decompiler
// Type: SMClient.Controls.Components.LoadoutSlot
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMA.Core.Models.Dictionaries;
using SMA.Core.Models.Ingame;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace SMClient.Controls.Components
{
  public class LoadoutSlot : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty LoadoutProperty = DependencyProperty.Register("Loadout", typeof (object), typeof (LoadoutSlot));
    public static readonly DependencyProperty LabelNameProperty = DependencyProperty.Register(nameof (LabelName), typeof (string), typeof (LoadoutSlot));
    internal LoadoutSlot root;
    internal Grid Background;
    internal Label baseVerBtn;
    private bool _contentLoaded;

    public event EventHandler Click;

    public bool IsSelected { get; set; }

    public object Value
    {
      get => this.GetValue(LoadoutSlot.LoadoutProperty);
      set
      {
        this.SetValue(LoadoutSlot.LoadoutProperty, value);
        if (value != null && value.GetType() == typeof (Loadout))
          this.LabelName = ((Loadout) value).Name;
        if (value != null && value.GetType() == typeof (Wargear))
          this.LabelName = ((Wargear) value).Name ?? ((Wargear) value).ID;
        if (value == null || !(value.GetType() == typeof (Class)))
          return;
        this.LabelName = ((Class) value).Name;
      }
    }

    public string LabelName
    {
      get => (string) this.GetValue(LoadoutSlot.LabelNameProperty);
      set => this.SetValue(LoadoutSlot.LabelNameProperty, (object) value);
    }

    public LoadoutSlot() => this.InitializeComponent();

    public void SetSelected(bool value)
    {
      this.IsSelected = value;
      if (this.IsSelected)
        this.Background.Background = (Brush) new SolidColorBrush(System.Windows.Media.Color.FromArgb((byte) 127, (byte) 100, (byte) 100, (byte) 100));
      else
        this.Background.Background = (Brush) new SolidColorBrush(System.Windows.Media.Color.FromArgb((byte) 0, (byte) 0, (byte) 0, (byte) 0));
    }

    private void BaseVerBtn_MouseEnter(object sender, MouseEventArgs e)
    {
      if (this.IsSelected)
        return;
      this.Background.Background = (Brush) new SolidColorBrush(System.Windows.Media.Color.FromArgb((byte) 127, (byte) 100, (byte) 100, (byte) 100));
    }

    private void BaseVerBtn_MouseLeave(object sender, MouseEventArgs e)
    {
      if (this.IsSelected)
        return;
      this.Background.Background = (Brush) new SolidColorBrush(System.Windows.Media.Color.FromArgb((byte) 0, (byte) 0, (byte) 0, (byte) 0));
    }

    private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      EventHandler click = this.Click;
      if (click != null)
        click((object) this, (EventArgs) null);
      e.Handled = true;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/SMClient;component/controls/components/loadoutslot.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.root = (LoadoutSlot) target;
          break;
        case 2:
          ((UIElement) target).MouseEnter += new MouseEventHandler(this.BaseVerBtn_MouseEnter);
          ((UIElement) target).MouseLeave += new MouseEventHandler(this.BaseVerBtn_MouseLeave);
          ((UIElement) target).MouseLeftButtonDown += new MouseButtonEventHandler(this.Grid_MouseLeftButtonDown);
          break;
        case 3:
          this.Background = (Grid) target;
          this.Background.MouseLeftButtonDown += new MouseButtonEventHandler(this.Grid_MouseLeftButtonDown);
          break;
        case 4:
          this.baseVerBtn = (Label) target;
          this.baseVerBtn.MouseLeftButtonDown += new MouseButtonEventHandler(this.Grid_MouseLeftButtonDown);
          this.baseVerBtn.MouseEnter += new MouseEventHandler(this.BaseVerBtn_MouseEnter);
          this.baseVerBtn.MouseLeave += new MouseEventHandler(this.BaseVerBtn_MouseLeave);
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
