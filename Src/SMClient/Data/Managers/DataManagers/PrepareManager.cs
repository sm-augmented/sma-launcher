// Decompiled with JetBrains decompiler
// Type: SMClient.Data.Managers.DataManagers.PrepareManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Models.Exceptions;
using SMClient.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SMClient.Data.Managers.DataManagers
{
  public class PrepareManager
  {
    private static Stack<Action> restoreActions = new Stack<Action>();

    public static void RemoveStaticData() => FileHelper.DeleteFile(Settings.Instance.ModPreviewPath);

    private static void ClearStaticData()
    {
      try
      {
        if (!Directory.Exists(Settings.Instance.ModPreviewPath))
        {
          PackageManager.ReUnpackStaticArchives();
        }
        else
        {
          foreach (string path in Directory.EnumerateDirectories(Settings.Instance.ModPreviewPath).Where<string>((Func<string, bool>) (x => Path.GetFileName(x) != "art" && Path.GetFileName(x) != "fx")))
            FileHelper.DeleteFile(path);
        }
      }
      catch (Exception ex)
      {
        throw new SMException("Unable to clear static data", ex);
      }
    }

    private static void ExecuteActionStack(Stack<Action> actions)
    {
      while (actions.Count > 0)
      {
        Action action = actions.Pop();
        try
        {
          action();
        }
        catch (Exception ex)
        {
          actions.Push(action);
          throw new SMException("Unable to execute action while restoring", ex);
        }
      }
    }

    public static void BackupData()
    {
      try
      {
        FileHelper.MoveDirectory(Settings.Instance.PreviewPath, Settings.Instance.PreviewBackupPath);
        PrepareManager.restoreActions.Push((Action) (() => FileHelper.MoveDirectory(Settings.Instance.PreviewBackupPath, Settings.Instance.PreviewPath)));
        PrepareManager.restoreActions.Push(new Action(PrepareManager.ClearStaticData));
        FileHelper.MoveDirectory(Settings.Instance.ModPreviewPath, Settings.Instance.PreviewPath);
        PrepareManager.restoreActions.Push((Action) (() => FileHelper.MoveDirectory(Settings.Instance.PreviewPath, Settings.Instance.ModPreviewPath)));
        PrepareManager.restoreActions.Push((Action) (() => FileHelper.MoveDirectory(Settings.Instance.ConfigPath + "-backup", Settings.Instance.ConfigPath)));
        PrepareManager.restoreActions.Push((Action) (() => FileHelper.DeleteFile(Settings.Instance.ConfigPath)));
        FileHelper.MoveDirectory(Settings.Instance.ConfigPath, Settings.Instance.ConfigPath + "-backup");
        PrepareManager.restoreActions.Push((Action) (() => FileHelper.MoveDirectory(Settings.Instance.LocalePath + "-backup", Settings.Instance.LocalePath)));
        PrepareManager.restoreActions.Push((Action) (() => FileHelper.DeleteFile(Settings.Instance.LocalePath)));
        FileHelper.MoveDirectory(Settings.Instance.LocalePath, Settings.Instance.LocalePath + "-backup");
      }
      catch (Exception ex)
      {
        PrepareManager.ExecuteActionStack(PrepareManager.restoreActions);
        throw new SMException("Unable to backup data", ex);
      }
    }

    public static void PrepareData(bool oldWay)
    {
      Logger.LogInfo("PrepareData: Data preparation");
      PrepareManager.restoreActions.Clear();
      PrepareManager.ClearStaticData();
    }

    public static void RestoreData()
    {
      if (PrepareManager.restoreActions.Count > 0)
        PrepareManager.ExecuteActionStack(PrepareManager.restoreActions);
      else
        PrepareManager.ClearStaticData();
    }
  }
}
