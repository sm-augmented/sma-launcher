// Decompiled with JetBrains decompiler
// Type: SMClient.Data.Managers.ArchiveManager
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using SMClient.Models;
using SMClient.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;

namespace SMClient.Data.Managers
{
  public class ArchiveManager
  {
    public static void UnpackStaticWithDeps(Package package)
    {
      foreach (Package dependency in package.Dependencies)
        ArchiveManager.UnpackStaticWithDeps(dependency);
      ArchiveManager.UnpackArchive(package);
    }

    public static void UnpackWithDeps(Queue<Package> plainList)
    {
      while (plainList.Count != 0)
        ArchiveManager.UnpackArchive(plainList.Dequeue());
    }

    public static void UnpackArchive(Package branch)
    {
      Logger.LogInfo("UnpackArchive: " + branch.Name);
      string archivePathByBranch = Settings.Instance.GetArchivePathByBranch(branch);
      Exception exception = (Exception) null;
      if (!File.Exists(archivePathByBranch))
        throw new SMException("Package " + branch.Name + " does not exist in file system");
      if (!(Path.GetExtension(archivePathByBranch) == ".sma"))
        return;
      ZipFile zipFile = (ZipFile) null;
      try
      {
        zipFile = new ZipFile(File.OpenRead(archivePathByBranch));
        zipFile.Password = "WEAREHERETOPRAISESPACEMARINEBRETHREN";
        foreach (ZipEntry entry in zipFile)
        {
          if (entry.IsFile)
          {
            string name = entry.Name;
            byte[] buffer = new byte[4096];
            Stream inputStream = zipFile.GetInputStream(entry);
            string path = Path.Combine(Settings.Instance.GamePath, name);
            string directoryName = Path.GetDirectoryName(path);
            if (directoryName.Length > 0)
              Directory.CreateDirectory(directoryName);
            using (FileStream destination = File.Create(path))
              StreamUtils.Copy(inputStream, (Stream) destination, buffer);
          }
        }
      }
      catch (Exception ex)
      {
        exception = (Exception) new SMException("Unable to unpack package " + branch.Name, ex);
      }
      finally
      {
        if (zipFile != null)
        {
          zipFile.IsStreamOwner = true;
          zipFile.Close();
        }
        if (exception != null)
          throw exception;
      }
    }
  }
}
