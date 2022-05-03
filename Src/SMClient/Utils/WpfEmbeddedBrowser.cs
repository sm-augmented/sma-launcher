// Decompiled with JetBrains decompiler
// Type: SMClient.Utils.WpfEmbeddedBrowser
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using IdentityModel.OidcClient.Browser;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SMClient.Utils
{
  public class WpfEmbeddedBrowser : IBrowser
  {
    private BrowserOptions _options;

    public async Task<BrowserResult> InvokeAsync(
      BrowserOptions options,
      CancellationToken cancellationToken = default (CancellationToken))
    {
      this._options = options;
      Window window1 = new Window();
      window1.Width = 900.0;
      window1.Height = 625.0;
      window1.Title = "IdentityServer Demo Login";
      Window window = window1;
      WebBrowser webBrowser = new WebBrowser();
      SemaphoreSlim signal = new SemaphoreSlim(0, 1);
      BrowserResult result = new BrowserResult()
      {
        ResultType = (BrowserResultType) 2
      };
      webBrowser.Navigating += (NavigatingCancelEventHandler) ((s, e) =>
      {
        if (!this.BrowserIsNavigatingToRedirectUri(e.Uri))
          return;
        e.Cancel = true;
        result = new BrowserResult()
        {
          ResultType = (BrowserResultType) 0,
          Response = e.Uri.AbsoluteUri
        };
        signal.Release();
        window.Close();
      });
      window.Closing += (CancelEventHandler) ((s, e) => signal.Release());
      window.Content = (object) webBrowser;
      window.Show();
      webBrowser.Source = new Uri(this._options.StartUrl);
      await signal.WaitAsync();
      return result;
    }

    private bool BrowserIsNavigatingToRedirectUri(Uri uri) => uri.AbsoluteUri.StartsWith(this._options.EndUrl);
  }
}
