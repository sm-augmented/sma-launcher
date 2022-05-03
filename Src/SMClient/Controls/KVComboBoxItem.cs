// Decompiled with JetBrains decompiler
// Type: SMClient.Controls.KVComboBoxItem
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System.Windows;
using System.Windows.Controls;

namespace SMClient.Controls
{
  public class KVComboBoxItem : ComboBoxItem
  {
    public static readonly DependencyProperty ValueStringProperty = DependencyProperty.Register(nameof (ValueString), typeof (string), typeof (KVComboBoxItem));

    public string ValueString
    {
      get => (string) this.GetValue(KVComboBoxItem.ValueStringProperty);
      set => this.SetValue(KVComboBoxItem.ValueStringProperty, (object) value);
    }
  }
}
