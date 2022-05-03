// Decompiled with JetBrains decompiler
// Type: SMClient.Models.Package
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.Collections.Generic;

namespace SMClient.Models
{
  [Serializable]
  public class Package
  {
    public string Name { get; set; }

    public string UserfriendlyName { get; set; }

    public string Description { get; set; }

    public string Version { get; set; }

    public bool IsVisible { get; set; }

    public bool IsStatic { get; set; }

    public bool CountedAsBranch { get; set; }

    public int FlatPriority { get; set; }

    public int Online { get; set; }

    public string DependenciesRaw { get; set; }

    public List<Package> Dependencies { get; set; }

    public bool IsUpdated { get; set; }

    public bool IsLoaded { get; set; }

    public Package() => this.Dependencies = new List<Package>();

    public override string ToString() => this.Name;
  }
}
