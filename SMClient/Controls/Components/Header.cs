// Decompiled with JetBrains decompiler
// Type: SMClient.Controls.Components.Header
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
  public class Header : UserControl, IComponentConnector
  {
    public static RoutedEvent HomeButtonClickEvent;
    public static RoutedEvent PlayButtonClickEvent;
    public static RoutedEvent ProfileButtonClickEvent;
    public static RoutedEvent SettingsButtonClickEvent;
    public static readonly DependencyProperty UsernameProperty = DependencyProperty.Register(nameof (Username), typeof (string), typeof (Header));
    internal Header root;
    internal HeaderButton homeBtn;
    internal HeaderButton playBtn;
    internal HeaderButton settingsBtn;
    private bool _contentLoaded;

    public event RoutedEventHandler HomeButtonClick
    {
      add => this.AddHandler(Header.HomeButtonClickEvent, (Delegate) value);
      remove => this.RemoveHandler(Header.HomeButtonClickEvent, (Delegate) value);
    }

    public event RoutedEventHandler PlayButtonClick
    {
      add => this.AddHandler(Header.PlayButtonClickEvent, (Delegate) value);
      remove => this.RemoveHandler(Header.PlayButtonClickEvent, (Delegate) value);
    }

    public event RoutedEventHandler ProfileButtonClick
    {
      add => this.AddHandler(Header.ProfileButtonClickEvent, (Delegate) value);
      remove => this.RemoveHandler(Header.ProfileButtonClickEvent, (Delegate) value);
    }

    public event RoutedEventHandler SettingsButtonClick
    {
      add => this.AddHandler(Header.SettingsButtonClickEvent, (Delegate) value);
      remove => this.RemoveHandler(Header.SettingsButtonClickEvent, (Delegate) value);
    }

    public string Username
    {
      get => (string) this.GetValue(Header.UsernameProperty);
      set => this.SetValue(Header.UsernameProperty, (object) value);
    }

    static Header()
    {
      Header.HomeButtonClickEvent = EventManager.RegisterRoutedEvent("HomeButtonClick", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (Header));
      Header.PlayButtonClickEvent = EventManager.RegisterRoutedEvent("PlayButtonClick", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (Header));
      Header.ProfileButtonClickEvent = EventManager.RegisterRoutedEvent("ProfileButtonClick", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (Header));
      Header.SettingsButtonClickEvent = EventManager.RegisterRoutedEvent("SettingsButtonClick", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (Header));
    }

    public Header() => this.InitializeComponent();

    public void SetProgress(double progress, bool isReady)
    {
      if (isReady)
      {
        this.playBtn.IsEnabled = true;
        this.playBtn.Label = "PLAY";
      }
      else
      {
        this.playBtn.IsEnabled = false;
        this.playBtn.Label = string.Format("{0}%", (object) progress);
      }
    }

    private void homeBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.RaiseEvent(new RoutedEventArgs(Header.HomeButtonClickEvent));
      this.playBtn.IsSelected = false;
      this.settingsBtn.IsSelected = false;
      this.homeBtn.IsSelected = true;
    }

    private void playBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.RaiseEvent(new RoutedEventArgs(Header.PlayButtonClickEvent));
      this.playBtn.IsSelected = true;
      this.settingsBtn.IsSelected = false;
      this.homeBtn.IsSelected = false;
    }

    private void profileBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.RaiseEvent(new RoutedEventArgs(Header.ProfileButtonClickEvent));
      this.playBtn.IsSelected = false;
      this.settingsBtn.IsSelected = false;
      this.homeBtn.IsSelected = false;
    }

    private void settingsBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      this.RaiseEvent(new RoutedEventArgs(Header.SettingsButtonClickEvent));
      this.playBtn.IsSelected = false;
      this.settingsBtn.IsSelected = true;
      this.homeBtn.IsSelected = false;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/SMClient;component/controls/components/header.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.root = (Header) target;
          break;
        case 2:
          this.homeBtn = (HeaderButton) target;
          break;
        case 3:
          this.playBtn = (HeaderButton) target;
          break;
        case 4:
          this.settingsBtn = (HeaderButton) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
