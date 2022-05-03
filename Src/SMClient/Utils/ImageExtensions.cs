// Decompiled with JetBrains decompiler
// Type: SMClient.Utils.ImageExtensions
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SMClient.Utils
{
  public static class ImageExtensions
  {
    public static void ChangeSource(
      this Image image,
      ImageSource source,
      TimeSpan fadeOutTime,
      TimeSpan fadeInTime)
    {
      DoubleAnimation fadeInAnimation = new DoubleAnimation(1.0, (Duration) fadeInTime);
      if (image.Source != null)
      {
        DoubleAnimation animation = new DoubleAnimation(0.0, (Duration) fadeOutTime);
        animation.Completed += (EventHandler) ((o, e) =>
        {
          image.Source = source;
          image.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) fadeInAnimation);
        });
        image.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) animation);
      }
      else
      {
        image.Opacity = 0.0;
        image.Source = source;
        image.BeginAnimation(UIElement.OpacityProperty, (AnimationTimeline) fadeInAnimation);
      }
    }
  }
}
