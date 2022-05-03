// Decompiled with JetBrains decompiler
// Type: SMClient.Utils.FileHelper
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using SMClient.Models.Exceptions;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SMClient.Utils
{
  public class FileHelper
  {
    private static void DoFileAndWait(Action action, string path, bool isDir, int timeout = 30000)
    {
      using (FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(path), Path.GetFileName(path)))
      {
        using (ManualResetEventSlim mre = new ManualResetEventSlim())
        {
          fileSystemWatcher.EnableRaisingEvents = true;
          fileSystemWatcher.Deleted += (FileSystemEventHandler) ((sender, e) => mre.Set());
          fileSystemWatcher.Renamed += (RenamedEventHandler) ((sender, e) => mre.Set());
          fileSystemWatcher.Changed += (FileSystemEventHandler) ((sender, e) => mre.Set());
          action();
          mre.Wait(timeout);
        }
      }
    }

    private static void DeleteDirectory(string targetDir)
    {
      File.SetAttributes(targetDir, FileAttributes.Normal);
      string[] files = Directory.GetFiles(targetDir);
      string[] directories = Directory.GetDirectories(targetDir);
      foreach (string path in files)
      {
        File.SetAttributes(path, FileAttributes.Normal);
        File.Delete(path);
      }
      foreach (string targetDir1 in directories)
        FileHelper.DeleteDirectory(targetDir1);
      Directory.Delete(targetDir, false);
    }

    public static void DeleteFile(string path)
    {
      if (!File.Exists(path) && !Directory.Exists(path))
        return;
      bool flag = File.GetAttributes(path).HasFlag((Enum) FileAttributes.Directory);
      if (flag)
      {
        FileHelper.DeleteDirectory(path);
      }
      else
      {
        File.SetAttributes(path, FileAttributes.Normal);
        File.Delete(path);
      }
      if (File.Exists(path) && !flag || Directory.Exists(path) & flag)
        throw new SMException("Unable to delete file/directory: " + path);
    }

    public static void DeleteFileAndWait(string path, int timeout = 30000)
    {
      if (!File.Exists(path) && !Directory.Exists(path))
        return;
      bool isDir = File.GetAttributes(path).HasFlag((Enum) FileAttributes.Directory);
      FileHelper.DoFileAndWait((Action) (() =>
      {
        if (isDir)
          Directory.Delete(path, true);
        else
          File.Delete(path);
      }), path, isDir, timeout);
      if (File.Exists(path) && !isDir || Directory.Exists(path) & isDir)
        throw new SMException("Unable to delete file/directory: " + path);
    }

    public static async Task DeleteFileAndWaitAsync(string path, int timeout = 30000) => await Task.Run((Action) (() => FileHelper.DeleteFileAndWait(path, timeout)));

    public static void CopyFile(string source, string dest)
    {
      if (!File.Exists(source) && !Directory.Exists(source))
        return;
      bool flag = File.GetAttributes(source).HasFlag((Enum) FileAttributes.Directory);
      string directoryName = Path.GetDirectoryName(dest);
      if (!Directory.Exists(directoryName))
        Directory.CreateDirectory(directoryName);
      if (!flag)
        File.Copy(source, dest, true);
      if (!File.Exists(dest) && !flag || !Directory.Exists(dest) & flag)
        throw new SMException("Unable to rename file/directory: " + source + " to " + dest);
    }

    public static void CopyFileAndWait(string source, string dest, int timeout = 30000)
    {
      if (!File.Exists(source) && !Directory.Exists(source))
        return;
      bool isDir = File.GetAttributes(source).HasFlag((Enum) FileAttributes.Directory);
      FileHelper.DoFileAndWait((Action) (() =>
      {
        string directoryName = Path.GetDirectoryName(dest);
        if (!Directory.Exists(directoryName))
          Directory.CreateDirectory(directoryName);
        if (isDir)
          return;
        File.Copy(source, dest, true);
      }), source, isDir, timeout);
      if (File.Exists(source) && !isDir || Directory.Exists(source) & isDir || !File.Exists(dest) && !isDir || !Directory.Exists(dest) & isDir)
        throw new SMException("Unable to rename file/directory: " + source + " to " + dest);
    }

    public static void RenameFile(string source, string dest)
    {
      if (!File.Exists(source) && !Directory.Exists(source))
        return;
      bool flag = File.GetAttributes(source).HasFlag((Enum) FileAttributes.Directory);
      if (flag)
      {
        Directory.Move(source, dest);
      }
      else
      {
        string directoryName = Path.GetDirectoryName(dest);
        if (!Directory.Exists(directoryName))
          Directory.CreateDirectory(directoryName);
        File.Move(source, dest);
      }
      if (File.Exists(source) && !flag || Directory.Exists(source) & flag || !File.Exists(dest) && !flag || !Directory.Exists(dest) & flag)
        throw new SMException("Unable to rename file/directory: " + source + " to " + dest);
    }

    public static void MoveDirectory(string source, string dest)
    {
      Logger.LogInfo("MoveDirectory: " + source + " to " + dest);
      FileHelper.DeleteFile(dest);
      FileHelper.RenameFile(source, dest);
    }

    public static void CopyDirectory(string source, string dest)
    {
      Logger.LogInfo("CopyDirectory: " + source + " to " + dest);
      FileHelper.DeleteFile(dest);
      foreach (string directory in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
        Directory.CreateDirectory(directory.Replace(source, dest));
      foreach (string file in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
        File.Copy(file, file.Replace(source, dest), true);
    }
  }
}
