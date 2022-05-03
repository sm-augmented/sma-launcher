// Decompiled with JetBrains decompiler
// Type: SMClient.Utils.MessageBoxHelper
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System.Windows;

namespace SMClient.Utils
{
  public static class MessageBoxHelper
  {
    private static MessageBoxResult Show(
      string message,
      string header,
      MessageBoxButton type)
    {
      return MessageBox.Show(Application.Current.MainWindow, message, header, type);
    }

    public static MessageBoxResult ShowWarning(
      string message,
      string header = "WARNING!",
      MessageBoxButton type = MessageBoxButton.OK)
    {
      return MessageBoxHelper.Show(message, header, type);
    }

    public static MessageBoxResult ShowError(
      string message,
      string header = "ERROR!",
      MessageBoxButton type = MessageBoxButton.OK)
    {
      return MessageBoxHelper.Show(message, header, type);
    }
  }
}
