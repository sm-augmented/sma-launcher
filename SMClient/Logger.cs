// Decompiled with JetBrains decompiler
// Type: SMClient.Logger
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using System;
using System.IO;

namespace SMClient
{
  public class Logger
  {
    private static string logPath = "loglog.log";
    private static StreamWriter logFile;

    public static void Clear()
    {
      if (File.Exists(Logger.logPath))
        File.Delete(Logger.logPath);
      Logger.logFile = File.CreateText(Logger.logPath);
    }

    public static void Flush() => Logger.logFile.Close();

    private static void Log(string line)
    {
      string str = DateTime.Now.ToString("HH':'mm':'ss");
      Logger.logFile.WriteLine(str + " - " + line);
      Logger.logFile.Flush();
    }

    private static void LogError(string line)
    {
      Logger.logFile.WriteLine("ERROR - " + line);
      Logger.logFile.Flush();
    }

    public static void LogError(Exception ex)
    {
      string str = ex.InnerException != null ? "InnerException following\n" : "No InnerException\n";
      Logger.LogError("Exception Info\n\t\tType: " + ex.GetType().Name + "\n\t\tMessage: " + ex.Message + "\n\t\tException source: " + ex.Source + "\n\t\tStack trace: " + ex.StackTrace + "\n\t\t" + str);
      Logger.logFile.Flush();
      if (ex.InnerException == null)
        return;
      Logger.LogError(ex.InnerException);
    }

    public static void LogDebug(string line)
    {
      Logger.logFile.WriteLine("DEBUG - " + line);
      Logger.logFile.Flush();
    }

    public static void LogInfo(string line)
    {
      Logger.logFile.WriteLine(line ?? "");
      Logger.logFile.Flush();
    }

    public static void LogWarning(string line)
    {
      Logger.logFile.WriteLine("WARNING - " + line);
      Logger.logFile.Flush();
    }
  }
}
