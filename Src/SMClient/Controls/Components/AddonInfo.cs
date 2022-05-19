// Decompiled with JetBrains decompiler
// Type: SMClient.Controls.AddonInfo
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Controls.Components;
using SMClient.Models;
using SMClient.Utils;
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
  public partial class AddonInfo : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty PackageNameProperty = DependencyProperty.Register(nameof (PackageName), typeof (string), typeof (AddonInfo));
    private Package package;

    public string PackageName
    {
      get => (string) this.GetValue(AddonInfo.PackageNameProperty);
      set => this.SetValue(AddonInfo.PackageNameProperty, (object) value);
    }

    public Package Package
    {
      get => this.package;
      set
      {
        this.package = value;
        this.PackageName = this.package.UserfriendlyName;
      }
    }

    public event EventHandler Checked;

    public event EventHandler Selected;

    public AddonInfo()
    {
      this.InitializeComponent();
      this.checkbox.Checked += new EventHandler(this.Checkbox_Checked);
    }

    public bool IsSelected { get; set; }

    public void SetSelected(bool value)
    {
      this.IsSelected = value;
      if (this.IsSelected)
        this.Background.Background = (Brush) new SolidColorBrush(Color.FromArgb((byte) 127, (byte) 81, (byte) 81, (byte) 81));
      else
        this.Background.Background = (Brush) new SolidColorBrush(Color.FromArgb((byte) 127, (byte) 50, (byte) 50, (byte) 50));
    }

    private void Checkbox_Checked(object sender, EventArgs e)
    {
      EventHandler eventHandler = this.Checked;
      if (eventHandler == null)
        return;
      eventHandler((object) this, (EventArgs) new CheckboxEventArgs()
      {
        Selected = (bool) sender,
        Package = this.package
      });
    }

    public AddonInfo(Package package)
    {
      this.InitializeComponent();
      this.checkbox.Checked += new EventHandler(this.Checkbox_Checked);
      this.Package = package;
    }

    private void Grid_MouseEnter(object sender, MouseEventArgs e)
    {
      if (this.IsSelected)
        return;
      this.Background.Background = (Brush) new SolidColorBrush(Color.FromArgb((byte) 127, (byte) 81, (byte) 81, (byte) 81));
    }

    private void Grid_MouseLeave(object sender, MouseEventArgs e)
    {
      if (this.IsSelected)
        return;
      this.Background.Background = (Brush) new SolidColorBrush(Color.FromArgb((byte) 127, (byte) 50, (byte) 50, (byte) 50));
    }

    private void Background_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      EventHandler selected = this.Selected;
      if (selected != null)
        selected((object) this, (EventArgs) null);
      e.Handled = true;
    }
  }
}
