using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using SMClient.Models;
using SMClient.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace SMClient.Managers
{
    public class ArchiveManager
    {
        public static void UnpackStaticWithDeps(Package package, ProgressChangedEventHandler onDownloadProgress = null,
              EventHandler onDownloadComplete = null)
        {
            foreach (Package dependency in package.Dependencies)
                ArchiveManager.UnpackStaticWithDeps(dependency, onDownloadProgress, onDownloadComplete);
            ArchiveManager.UnpackArchive(package, onDownloadProgress, onDownloadComplete);
        }

        public static void UnpackWithDeps(Queue<Package> plainList)
        {
            while (plainList.Count != 0)
                ArchiveManager.UnpackArchive(plainList.Dequeue());
        }

        public static void UnpackArchive(Package branch, ProgressChangedEventHandler onDownloadProgress = null,
              EventHandler onDownloadComplete = null)
        {
            Logger.LogInfo("UnpackArchive: " + branch.Name);
            string archivePathByBranch = Settings.Instance.GetArchivePathByBranch(branch);
            Exception exception = (Exception)null;
            if (!File.Exists(archivePathByBranch))
                throw new SMException("Package " + branch.Name + " does not exist in file system");
            if (!(Path.GetExtension(archivePathByBranch) == ".sma"))
                return;
            ZipFile zipFile = (ZipFile)null;
            try
            {
                zipFile = new ZipFile(File.OpenRead(archivePathByBranch));
                var fileCount = zipFile.Count;
                var idx = 1;
                zipFile.Password = "WEAREHERETOPRAISESPACEMARINEBRETHREN";
                foreach (ZipEntry entry in zipFile)
                {
                    if (entry.IsFile)
                    {
                        if (onDownloadProgress != null)
                        {
                            var percent = (int)(idx * 100 / fileCount);
                            onDownloadProgress(null, new ProgressChangedEventArgs(percent, null));
                        }

                        string name = entry.Name;
                        byte[] buffer = new byte[4096];
                        Stream inputStream = zipFile.GetInputStream(entry);
                        string path = Path.Combine(Settings.Instance.GamePath, name);
                        string directoryName = Path.GetDirectoryName(path);
                        if (directoryName.Length > 0)
                            Directory.CreateDirectory(directoryName);
                        using (FileStream destination = File.Create(path))
                            StreamUtils.Copy(inputStream, (Stream)destination, buffer);
                        idx++;
                    }
                }

                if (onDownloadComplete != null)
                {
                    onDownloadComplete(null, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                exception = (Exception)new SMException("Unable to unpack package " + branch.Name, ex);
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
