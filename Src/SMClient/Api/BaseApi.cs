// Decompiled with JetBrains decompiler
// Type: SMClient.Api.BaseApi
// Assembly: SMClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8FEFC3E2-D24F-47DA-A11F-015A247C9191
// Assembly location: D:\Games\Warhammer 40.000 Space Marine Augmented\SMClient\SMClient.exe

using Dropbox.Api;
using Dropbox.Api.Files;
using Dropbox.Api.Stone;
using SMClient.Data.Dropbox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SMClient.Api
{
  public static class BaseApi
  {
    private static HttpClient HttpClient { get; set; }

    public static string ServerUrl { get; private set; }

    private static DropboxClient DriveService { get; set; }

    public static void Initialize()
    {
      ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
      BaseApi.ServerUrl = "https://smserver.azurewebsites.net";
      BaseApi.HttpClient = new HttpClient();
      BaseApi.HttpClient.BaseAddress = new Uri(BaseApi.ServerUrl);
      BaseApi.HttpClient.DefaultRequestHeaders.Accept.Clear();
      BaseApi.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      DropboxClientConfig config = new DropboxClientConfig()
      {
        HttpClient = new HttpClient()
      };
      config.HttpClient.Timeout = new TimeSpan(0, 20, 0);
      BaseApi.DriveService = new DropboxClient("GMYvCUyDLY0AAAAAAAAHmqhC69m6_8AjWfuYBLXGPKchtZ0ZWf--MfwdFM0rkkvZ", config);
    }

    public static void SetToken()
    {
      if (BaseApi.HttpClient.DefaultRequestHeaders.Contains("Authorization"))
        BaseApi.HttpClient.DefaultRequestHeaders.Remove("Authorization");
      BaseApi.HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.Instance.AccessToken);
    }

    public static async Task<string> GetStringAsync(string url) => await BaseApi.HttpClient.GetStringAsync(url);

    public static async Task<byte[]> GetByteArray(string url) => await BaseApi.HttpClient.GetByteArrayAsync(url);

    public static async Task<IDownloadResponse<FileMetadata>> GetFileMetadata(
      string fileName)
    {
      return await BaseApi.DriveService.Files.DownloadAsync(fileName);
    }

    public static async Task<List<Metadata>> GetFolderContent(
      string folder,
      uint? limit = null)
    {
      List<Metadata> entries = new List<Metadata>();
      ListFolderResult listFolderResult = await BaseApi.DriveService.Files.ListFolderAsync(folder, true, limit: limit);
      entries.AddRange(listFolderResult.Entries.Where<Metadata>((Func<Metadata, bool>) (x => x.IsFile)));
      while (listFolderResult.HasMore)
      {
        if (limit.HasValue)
          goto label_6;
label_2:
        listFolderResult = await BaseApi.DriveService.Files.ListFolderContinueAsync(listFolderResult.Cursor);
        entries.AddRange((IEnumerable<Metadata>) listFolderResult.Entries);
        continue;
label_6:
        if (limit.HasValue)
        {
          long count = (long) entries.Count;
          uint? nullable1 = limit;
          long? nullable2 = nullable1.HasValue ? new long?((long) nullable1.GetValueOrDefault()) : new long?();
          long valueOrDefault = nullable2.GetValueOrDefault();
          if (count < valueOrDefault & nullable2.HasValue)
            goto label_2;
          else
            break;
        }
        else
          break;
      }
      List<Metadata> folderContent = entries;
      entries = (List<Metadata>) null;
      return folderContent;
    }

    public static async Task<DownloadedFile> DownloadFile(
      IDownloadResponse<FileMetadata> metadata,
      ProgressChangedEventHandler onDownloadProgress = null,
      EventHandler onDownloadComplete = null)
    {
      ulong fileSize = metadata.Response.Size;
      byte[] buffer = new byte[1048576];
      DownloadedFile downloadedFile;
      using (Stream contentAsStreamAsync = await metadata.GetContentAsStreamAsync())
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          for (int count = contentAsStreamAsync.Read(buffer, 0, 1048576); count > 0; count = contentAsStreamAsync.Read(buffer, 0, 1048576))
          {
            memoryStream.Write(buffer, 0, count);
            int int32 = Convert.ToInt32(100UL * (ulong) memoryStream.Length / fileSize);
            ProgressChangedEventHandler changedEventHandler = onDownloadProgress;
            if (changedEventHandler != null)
              changedEventHandler((object) null, new ProgressChangedEventArgs(int32, (object) null));
          }
          EventHandler eventHandler = onDownloadComplete;
          if (eventHandler != null)
            eventHandler((object) null, new EventArgs());
          downloadedFile = new DownloadedFile()
          {
            File = memoryStream.ToArray(),
            Hash = metadata.Response.ContentHash
          };
        }
      }
      buffer = (byte[]) null;
      return downloadedFile;
    }

    public static async Task<HttpResponseMessage> GetAsync(string url) => await BaseApi.HttpClient.GetAsync(url);

    public static async Task<HttpResponseMessage> PostAsync(
      string url,
      HttpContent content)
    {
      return await BaseApi.HttpClient.PostAsync(url, content);
    }

    public static async Task<string> PostAsyncForString(string url, HttpContent content)
    {
      HttpResponseMessage httpResponseMessage = await BaseApi.HttpClient.PostAsync(url, content);
      if (httpResponseMessage.IsSuccessStatusCode)
        return await httpResponseMessage.Content.ReadAsStringAsync();
      throw new Exception("Status code doesn't indicate success");
    }
  }
}
