// Decompiled with JetBrains decompiler
// Type: SMClient.Controls.NewsInfo
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Models;
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

namespace SMClient.Controls
{
  public partial class NewsInfo : UserControl, IComponentConnector
  {
    private static ImageSource Base = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/SMClient;component/Resources/BaseBranch.png"));
    private static ImageSource Versus = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/SMClient;component/Resources/Versus.png"));
    private static ImageSource Exterminatus = (ImageSource) new BitmapImage(new Uri("pack://application:,,,/SMClient;component/Resources/Exterminatus.png"));

    public event EventHandler Click;

    public News News { get; private set; }

    public bool IsSelected { get; set; }

    public NewsInfo() => this.InitializeComponent();

    public NewsInfo(News news)
    {
      this.News = news;
      this.InitializeComponent();
      this.baseVerBtn.Content = (object) news.Branch;
      if (news.Branch == nameof (Exterminatus))
        this.baseVerBtn.FontSize = 13.0;
      string branch = news.Branch;
      if (!(branch == nameof (Base)))
      {
        if (!(branch == nameof (Versus)))
        {
          if (branch == nameof (Exterminatus))
            this.verImg.Source = NewsInfo.Exterminatus;
        }
        else
          this.verImg.Source = NewsInfo.Versus;
      }
      else
        this.verImg.Source = NewsInfo.Base;
      this.newsDate.Text = news.Date;
      this.newsName.Text = news.Name;
    }

    public void SetSelected(bool value)
    {
      this.IsSelected = value;
      if (this.IsSelected)
        this.Background.Background = (Brush) new SolidColorBrush(Color.FromArgb((byte) 127, (byte) 100, (byte) 100, (byte) 100));
      else
        this.Background.Background = (Brush) new SolidColorBrush(Color.FromArgb((byte) 0, (byte) 0, (byte) 0, (byte) 0));
    }

    private void BaseVerBtn_MouseEnter(object sender, MouseEventArgs e)
    {
      if (this.IsSelected)
        return;
      this.Background.Background = (Brush) new SolidColorBrush(Color.FromArgb((byte) 127, (byte) 100, (byte) 100, (byte) 100));
    }

    private void BaseVerBtn_MouseLeave(object sender, MouseEventArgs e)
    {
      if (this.IsSelected)
        return;
      this.Background.Background = (Brush) new SolidColorBrush(Color.FromArgb((byte) 0, (byte) 0, (byte) 0, (byte) 0));
    }

    private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      EventHandler click = this.Click;
      if (click != null)
        click((object) this, (EventArgs) null);
      e.Handled = true;
    }
  }
}
