// Decompiled with JetBrains decompiler
// Type: SMClient.Settings
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Models;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SMClient
{
  [Serializable]
  public class Settings
  {
    private static Settings instance;

    public string AccessToken { get; set; }

    public string GamePath { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public string DataVersion { get; set; }

    public bool StartWindowed { get; set; }

    public bool OldLaunchWay { get; set; }

    public string LastSelection { get; set; }

    public bool UseNoHud { get; set; }

    [XmlIgnore]
    public string SpaceMarineEXEPath => Path.Combine(this.GamePath, "SpaceMarine.exe");

    [XmlIgnore]
    public string SMAEXEPath => Path.Combine(this.GamePath, "SMA.exe");

    [XmlIgnore]
    public string LauncherPath => Directory.GetCurrentDirectory();

    [XmlIgnore]
    public string DataPath => Path.Combine(this.LauncherPath, "Data\\");

    [XmlIgnore]
    public string PreviewPath => Path.Combine(this.GamePath, "preview");

    [XmlIgnore]
    public string ModPreviewPath => Path.Combine(this.GamePath, "modview");

    [XmlIgnore]
    public string PreviewBackupPath => Path.Combine(this.GamePath, "preview-backup");

    [XmlIgnore]
    public string ExePath => Path.Combine(this.GamePath, "SpaceMarine.exe");

    [XmlIgnore]
    public string DllPath => Path.Combine(this.GamePath, "sma.dll");

    [XmlIgnore]
    public string DIPath => Path.Combine(this.GamePath, "dinput8.dll");

    [XmlIgnore]
    public string CppRestPath => Path.Combine(this.GamePath, "cpprest141_2_10.dll");

    [XmlIgnore]
    public string LocalePath => Path.Combine(this.GamePath, "data\\locale");

    [XmlIgnore]
    public string ConfigPath => Path.Combine(this.GamePath, "data\\config");

    [XmlIgnore]
    public string ModfigPath => Path.Combine(this.GamePath, "data\\modfig");

    [XmlIgnore]
    public string SMALogPath => Path.Combine(this.GamePath, "smalog.log");

    [XmlIgnore]
    public string GameLogPath => Path.Combine(this.GamePath, "gamelog.log");

    public static Settings Instance
    {
      get
      {
        if (Settings.instance == null)
          Settings.LoadInstance();
        return Settings.instance;
      }
    }

    public bool GamePathFine => !string.IsNullOrEmpty(this.GamePath) && File.Exists(this.ExePath);

    public string GetArchiveNameByBranch(Package branch) => branch.Name + ".sma";

    public string GetLocalePathByBranch(Package branch)
    {
      string str = "";
      if (branch.Name == "Versus")
        str = "loc-vn";
      else if (branch.Name == "Versus-test")
        str = "loc-vt";
      else if (branch.Name == "Exterminatus")
        str = "loc-en";
      else if (branch.Name == "Exterminatus-test")
        str = "loc-et";
      return !string.IsNullOrEmpty(str) ? Path.Combine(this.GamePath, "data\\" + str) : "";
    }

    public string GetArchivePathByBranch(Package branch) => Path.Combine(this.DataPath, this.GetArchiveNameByBranch(branch));

    private static void LoadInstance()
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (Settings));
      bool flag = false;
      if (File.Exists("Settings.xml"))
      {
        using (FileStream fileStream = new FileStream("Settings.xml", FileMode.Open))
        {
          StreamReader streamReader = new StreamReader((Stream) fileStream, Encoding.UTF8);
          Settings.instance = (Settings) xmlSerializer.Deserialize((TextReader) streamReader);
          flag = true;
          streamReader.Close();
        }
      }
      if (flag)
        return;
      Settings.instance = new Settings();
    }

    private static void SaveInstance()
    {
      using (FileStream fileStream = new FileStream("Settings.xml", FileMode.Create))
        new XmlSerializer(typeof (Settings)).Serialize((TextWriter) new StreamWriter((Stream) fileStream, Encoding.UTF8), (object) Settings.instance);
    }

    public void Save() => Settings.SaveInstance();
  }
}
