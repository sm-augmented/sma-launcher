// Decompiled with JetBrains decompiler
// Type: SMClient.Api.HashApi
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using Dropbox.Api.Files;
using Dropbox.Api.Stone;
using SMClient.Data;
using SMClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SMClient.Api
{
  public class HashApi
  {
    public static bool CheckHash(byte[] server, byte[] client) => BitConverter.ToString(server).Replace("-", string.Empty) == BitConverter.ToString(client).Replace("-", string.Empty);

    public static async Task<List<Metadata>> CheckPreviewHash(
      List<Metadata> dboxMeta)
    {
      List<Metadata> metadataList = new List<Metadata>();
      foreach (FileMetadata metadata in dboxMeta.Where<Metadata>((Func<Metadata, bool>) (x => x.IsFile)).Select<Metadata, FileMetadata>((Func<Metadata, FileMetadata>) (x => x.AsFile)))
      {
        string path = System.IO.Path.Combine(Settings.Instance.GamePath, metadata.PathLower.Substring(1, metadata.PathLower.Length - 1));
        if (!HashApi.CheckHashByFile(metadata, path))
          metadataList.Add((Metadata) metadata);
      }
      return (List<Metadata>) null;
    }

    public static async Task<bool> CheckAllHashesAsync(Package branch)
    {
      bool flag1 = await HashApi.CheckClientHash();
      if (!flag1)
        return flag1;
      bool flag2 = await HashApi.CheckDataHash(branch);
      int num = flag2 ? 1 : 0;
      return flag2;
    }

    public static async Task<bool> CheckUpdaterHash() => await HashApi.CheckHashByFile("/SMClientUpdater.exe", "SMClientUpdater.exe");

    public static bool CheckUpdaterHash(string serverHash) => HashApi.CheckHash(serverHash, "SMClientUpdater.exe");

    public static async Task<bool> CheckDataHash(Package branch) => await HashApi.CheckHashByFile("/" + Settings.Instance.GetArchiveNameByBranch(branch), Settings.Instance.GetArchivePathByBranch(branch));

    public static bool CheckDataHash(Package branch, string serverHash) => HashApi.CheckHash(serverHash, Settings.Instance.GetArchivePathByBranch(branch));

    public static async Task<bool> CheckClientHash() => await HashApi.CheckHashByFile("/SMClient.exe", Assembly.GetEntryAssembly().Location);

    public static bool CheckClientHash(string serverHash) => HashApi.CheckHash(serverHash, Assembly.GetEntryAssembly().Location);

    private static bool CheckHashByFile(FileMetadata metadata, string path) => System.IO.File.Exists(path) && new DropboxContentHasher().CalculateHash(path).CompareTo(metadata.ContentHash) == 0;

    private static async Task<bool> CheckHashByFile(string fileName, string path)
    {
      if (!System.IO.File.Exists(path))
        return false;
      string hash = new DropboxContentHasher().CalculateHash(path);
      IDownloadResponse<FileMetadata> fileMetadata = await BaseApi.GetFileMetadata(fileName);
      string contentHash = fileMetadata.Response.ContentHash;
      fileMetadata.Dispose();
      return hash.CompareTo(contentHash) == 0;
    }

    private static bool CheckHash(string serverHash, string path) => System.IO.File.Exists(path) && new DropboxContentHasher().CalculateHash(path).CompareTo(serverHash) == 0;
  }
}
